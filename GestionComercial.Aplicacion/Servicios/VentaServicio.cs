using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class VentaServicio : IVentaServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly IServicioImpresion _servicioImpresion;
        private readonly SesionServicio _sesion;

        public VentaServicio(IUnitOfWork uow, IServicioImpresion servicioImpresion, SesionServicio sesion)
        {
            _uow = uow;
            _servicioImpresion = servicioImpresion;
            _sesion = sesion;
        }

        public async Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(
            int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(desde, hasta, idSucursal);
            return ventas.Select(MapearResumen);
        }

        /// <summary>
        /// Feature 3: Obtiene ventas con filtros opcionales y combinables.
        /// </summary>
        public async Task<IEnumerable<VentaResumenDto>> ObtenerVentasAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string? dniCliente = null,
            int? estado = null)
        {
            // Usar rango de fechas default si no se especifican
            var desde = fechaDesde ?? DateTime.Today.AddDays(-30);
            var hasta = fechaHasta ?? DateTime.Today.AddDays(1).AddSeconds(-1);

            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(desde, hasta, idSucursal);

            // Aplicar filtros opcionales
            if (!string.IsNullOrWhiteSpace(dniCliente))
            {
                // Convertir a string para buscar por contenido (partial match)
                var dniBuscado = dniCliente.Trim().ToLower();
                ventas = ventas.Where(v => v.Cliente != null && 
                    v.Cliente.Documento.ToString().Contains(dniBuscado));
            }

            if (estado.HasValue)
            {
                ventas = ventas.Where(v => v.Estado == estado.Value);
            }

            return ventas.Select(MapearResumen);
        }

        public async Task<VentaDto?> ObtenerPorIdAsync(int id)
        {
            var v = await _uow.Ventas.ObtenerConDetallesAsync(id);
            return v == null ? null : MapearDto(v);
        }

        public async Task<VentaDto> CrearAsync(VentaCrearDto dto)
        {
            // ── Validar stock antes de tocar nada ─────────────────────────────
            foreach (var item in dto.Items)
            {
                var p = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)
                    ?? throw new ProductoNoEncontradoException(item.IdProducto);
                if (p.StockActual < item.Cantidad)
                    throw new StockInsuficienteException(p.Nombre, (int)p.StockActual, item.Cantidad);
            }

            // ── Crear venta PENDIENTE (Estado=1) ──────────────────────────────
            var venta = new Venta
            {
                Fecha          = DateTime.Now,
                Estado         = (int)EstadoVentaEnum.Pendiente,
                Id_sucursal    = dto.IdSucursal,
                Id_cliente     = dto.IdCliente,
                Id_usuario     = dto.IdUsuario,
                Id_caja        = dto.IdCaja,
                TotalBruto     = 0,
                TotalDescuento = 0, // Se calcula acumulando descuentos por ítem
                TotalFinal     = 0,
            };

            decimal totalBruto = 0;
            decimal totalDescuentos = 0;

            foreach (var item in dto.Items)
            {
                var producto  = (await _uow.Productos.ObtenerPorIdAsync(item.IdProducto))!;
                var subtotal  = producto.PrecioVentaActual * item.Cantidad;
                totalBruto   += subtotal;

                var detalle = new VentaDetalle
                {
                    Id_producto    = item.IdProducto,
                    Cantidad       = item.Cantidad,
                    PrecioUnitario = producto.PrecioVentaActual,
                    CostoUnitario  = producto.PrecioCostoActual,
                    Subtotal       = subtotal,
                    MargenUnitario = producto.PrecioVentaActual - producto.PrecioCostoActual,
                };

                // ── Aplicar descuentos por ítem ────────────────────────────────
                foreach (var dtoDesc in item.Descuentos)
                {
                    var descuento = new VentaDetalleDescuento
                    {
                        Porcentaje  = dtoDesc.Porcentaje,
                        Monto       = dtoDesc.Monto,
                        Descripcion = dtoDesc.Descripcion,
                    };
                    detalle.Descuentos.Add(descuento);
                    totalDescuentos += dtoDesc.Monto;
                }

                venta.Detalles.Add(detalle);

                producto.StockActual -= item.Cantidad;
                _uow.Productos.Actualizar(producto);
            }

            venta.TotalBruto = totalBruto;
            venta.TotalDescuento = totalDescuentos;
            venta.TotalFinal = totalBruto - totalDescuentos;

            await _uow.Ventas.AgregarAsync(venta);
            await _uow.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(venta.Id)
                ?? throw new NegocioException("Error al crear venta.");
        }

        /// <summary>
        /// Registra los pagos y recién aquí marca la venta como Pagada.
        /// Soporta pagos mixtos (efectivo + tarjeta + QR, etc.).
        /// Para pagos en efectivo, crea automáticamente un movimiento de caja.
        /// El vuelto se registra como egreso en la caja.
        /// </summary>
        public async Task RegistrarPagoAsync(int idVenta, List<PagoItemDto> pagos)
        {
            var venta = await _uow.Ventas.ObtenerConDetallesAsync(idVenta)
                ?? throw new VentaInvalidaException($"Venta #{idVenta} no encontrada.");

            if (venta.Estado == (int)EstadoVentaEnum.Pagada)
                throw new VentaInvalidaException("La venta ya está pagada.");
            if (venta.Estado == (int)EstadoVentaEnum.Anulada)
                throw new VentaInvalidaException("La venta está anulada y no puede pagarse.");

            var totalPagado = pagos.Sum(p => p.Monto);
            if (totalPagado < venta.TotalFinal)
                throw new NegocioException(
                    $"El monto pagado (${totalPagado:N2}) es menor al total (${venta.TotalFinal:N2}).");

            // Calcular el total de vuelto de todos los pagos en efectivo
            decimal totalVuelto = pagos.Where(p => p.EsEfectivo).Sum(p => p.Vuelto);

            foreach (var pago in pagos.Where(p => p.Monto > 0))
            {
                var pagoEntity = new GestionComercial.Dominio.Entidades.Pagos.Pago
                {
                    Id_venta      = idVenta,
                    Id_metodoPago = pago.IdMetodoPago,
                    Monto         = pago.Monto,
                };

                // ── Crear movimiento de caja SOLO para pagos en efectivo ─────────
                if (pago.EsEfectivo && venta.Id_caja.HasValue)
                {
                    // El monto del movimiento es: efectivo recibido - vuelto dado
                    var montoNeto = pago.Monto - pago.Vuelto;

                    var movimiento = new TipoMovimientoCaja
                    {
                        Tipo         = (int)TipoMovimientoCajaEnum.Ingreso,
                        Monto        = montoNeto,
                        Concepto     = $"Venta #{venta.Id} (recibido: ${pago.Monto:N2}, vuelto: ${pago.Vuelto:N2})",
                        ReferenciaId = venta.Id,
                        Id_venta     = venta.Id,
                        Id_caja      = venta.Id_caja.Value,
                        Id_usuario   = venta.Id_usuario,
                        Fecha        = DateTime.Now,
                    };
                    await _uow.MovimientosCaja.AgregarAsync(movimiento);
                    await _uow.GuardarCambiosAsync();
                    
                    // Vincular el pago con el movimiento de caja
                    pagoEntity.Id_movimientoCaja = movimiento.Id;

                    // ── Registrar monto efectivo para cierre de caja ───────────────
                    // Guardamos el monto TOTAL recibido (sin restar el vuelto) para el cierre
                    venta.EfectivoRecibido = (venta.EfectivoRecibido ?? 0) + pago.Monto;

                    // ── Registrar el vuelto como egreso si corresponde ───────────
                    if (pago.Vuelto > 0)
                    {
                        var movimientoVuelto = new TipoMovimientoCaja
                        {
                            Tipo         = (int)TipoMovimientoCajaEnum.Egreso,
                            Monto        = pago.Vuelto,
                            Concepto     = $"Vuelto venta #{venta.Id}",
                            ReferenciaId = venta.Id,
                            Id_venta     = venta.Id,
                            Id_caja      = venta.Id_caja.Value,
                            Id_usuario   = venta.Id_usuario,
                            Fecha        = DateTime.Now,
                        };
                        await _uow.MovimientosCaja.AgregarAsync(movimientoVuelto);
                    }
                }

                await _uow.Pagos.AgregarAsync(pagoEntity);
            }

            venta.Estado = (int)EstadoVentaEnum.Pagada;
            _uow.Ventas.Actualizar(venta);
            await _uow.GuardarCambiosAsync();

            // ── Imprimir ticket post-pago (fire-and-forget) ──────────────────────
            _ = ImprimirTicketAsync(venta.Id);
        }

        /// <summary>
        /// Imprime el ticket de manera asíncrona (no bloquea la respuesta).
        /// Los errores de impresión NO deben revertir el pago.
        /// </summary>
        private async Task ImprimirTicketAsync(int idVenta)
        {
            try
            {
                var ventaCompleta = await _uow.Ventas.ObtenerConDetallesAsync(idVenta);
                if (ventaCompleta == null) return;

                var ventaDto = MapearDto(ventaCompleta);
                var pagosDto = ventaCompleta.Pagos.Select(p => new PagoDto
                {
                    IdPago       = p.Id,
                    Monto        = p.Monto,
                    IdMetodoPago = p.Id_metodoPago,
                    MetodoNombre = p.MetodoPago?.Nombre ?? "Desconocido",
                    EsEfectivo   = p.MetodoPago?.EsEfectivo ?? false,
                }).ToList();

                _servicioImpresion.ImprimirTicket(ventaDto, pagosDto);
            }
            catch
            {
                // Fire-and-forget: los errores de impresión no deben afectar la transacción
            }
        }

        /// <summary>
        /// Anula la venta con motivo obligatorio y devuelve el stock.
        /// Los pagos ya registrados quedan en BD como historial.
        /// ObtenerTotalDelDiaAsync filtra solo Estado=Pagada así que
        /// las ventas anuladas no afectan reportes de ingresos.
        /// </summary>
        /// <param name="id">ID de la venta a anular</param>
        /// <param name="motivo">Motivo obligatorio de la anulación</param>
        /// <exception cref="VentaInvalidaException">Si la venta no existe o ya está anulada</exception>
        /// <exception cref="ArgumentException">Si el motivo está vacío</exception>
        public async Task CancelarAsync(int id, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de anulación es obligatorio.", nameof(motivo));

            var venta = await _uow.Ventas.ObtenerConDetallesAsync(id)
                ?? throw new VentaInvalidaException($"Venta #{id} no encontrada.");

            if (venta.Estado == (int)EstadoVentaEnum.Anulada)
                throw new VentaInvalidaException("La venta ya está anulada.");

            // Devolver stock siempre (estaba pendiente o pagada)
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(detalle.Id_producto);
                if (producto != null)
                {
                    producto.StockActual += detalle.Cantidad;
                    _uow.Productos.Actualizar(producto);
                }
            }

            // ── Registrar datos de anulación ──────────────────────────────────
            venta.Estado           = (int)EstadoVentaEnum.Anulada;
            venta.MotivoAnulacion  = motivo;
            venta.FechaAnulacion   = DateTime.Now;
            venta.UsuarioAnulacionId = venta.Id_usuario; // Usuario que realiza la anulación

            _uow.Ventas.Actualizar(venta);
            await _uow.GuardarCambiosAsync();
        }

        public async Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal)
            => await _uow.Ventas.ObtenerTotalDelDiaAsync(idSucursal);

        // ── Mapeos ────────────────────────────────────────────────────────────
        private static VentaResumenDto MapearResumen(Venta v) => new()
        {
            IdVenta       = v.Id,
            Fecha         = v.Fecha,
            TotalFinal    = v.TotalFinal,
            Estado        = MapEstado(v.Estado),
            ClienteNombre = v.Cliente?.Nombre ?? "Consumidor Final",
            UsuarioNombre = v.Usuario != null
                ? $"{v.Usuario.Nombre} {v.Usuario.Apellido}" : string.Empty,
        };

        private static VentaDto MapearDto(Venta v) => new()
        {
            IdVenta        = v.Id,
            Fecha          = v.Fecha,
            TotalBruto     = v.TotalBruto,
            TotalDescuento = v.TotalDescuento,
            TotalFinal     = v.TotalFinal,
            Estado         = MapEstado(v.Estado),
            ClienteNombre  = v.Cliente?.Nombre ?? "Consumidor Final",
            UsuarioNombre  = v.Usuario != null
                ? $"{v.Usuario.Nombre} {v.Usuario.Apellido}" : string.Empty,
            IdCaja         = v.Id_caja ?? 0,
            Items = v.Detalles.Select(d => new VentaDetalleDto
            {
                IdProducto     = d.Id_producto,
                ProductoNombre = d.Producto?.Nombre ?? string.Empty,
                Cantidad       = (int)d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                CostoUnitario  = d.CostoUnitario,
                Subtotal       = d.Subtotal,
                MargenUnitario = d.MargenUnitario,
            }).ToList(),
        };

        private static string MapEstado(int e) => e switch
        {
            1 => "Pendiente",
            2 => "Pagada",
            3 => "Anulada",
            _ => "Desconocido",
        };
    }
}
