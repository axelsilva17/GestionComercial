using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class VentaServicio : IVentaServicio
    {
        private readonly IUnitOfWork _uow;
        public VentaServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(
            int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(desde, hasta, idSucursal);
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
            // ¡NO se pone Estado=2 aquí! Solo se marca Pagada en RegistrarPagoAsync.
            var venta = new Venta
            {
                Fecha          = DateTime.Now,
                Estado         = 1,               // 1=Pendiente, 2=Pagada, 3=Anulada
                Id_sucursal    = dto.IdSucursal,
                Id_cliente     = dto.IdCliente,
                Id_usuario     = dto.IdUsuario,
                Id_caja        = dto.IdCaja,
                TotalBruto     = 0,
                TotalDescuento = dto.TotalDescuento,
                TotalFinal     = 0,
            };

            decimal totalBruto = 0;
            foreach (var item in dto.Items)
            {
                var producto  = (await _uow.Productos.ObtenerPorIdAsync(item.IdProducto))!;
                var subtotal  = producto.PrecioVentaActual * item.Cantidad;
                totalBruto   += subtotal;

                venta.Detalles.Add(new VentaDetalle
                {
                    Id_producto    = item.IdProducto,
                    Cantidad       = item.Cantidad,
                    PrecioUnitario = producto.PrecioVentaActual,
                    CostoUnitario  = producto.PrecioCostoActual,
                    Subtotal       = subtotal,
                    MargenUnitario = producto.PrecioVentaActual - producto.PrecioCostoActual,
                });

                producto.StockActual -= item.Cantidad;
                _uow.Productos.Actualizar(producto);
            }

            venta.TotalBruto = totalBruto;
            venta.TotalFinal = totalBruto - dto.TotalDescuento;
            // ← SIN venta.Estado = 2 — ese era el bug

            await _uow.Ventas.AgregarAsync(venta);
            await _uow.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(venta.Id)
                ?? throw new NegocioException("Error al crear venta.");
        }

        /// <summary>
        /// Registra los pagos y recién aquí marca la venta como Pagada.
        /// Soporta pagos mixtos (efectivo + tarjeta + QR, etc.).
        /// El vuelto NO se guarda — es solo informativo para el vendedor.
        /// </summary>
        public async Task RegistrarPagoAsync(int idVenta, List<PagoItemDto> pagos)
        {
            var venta = await _uow.Ventas.ObtenerConDetallesAsync(idVenta)
                ?? throw new VentaInvalidaException($"Venta #{idVenta} no encontrada.");

            if (venta.Estado == 2)
                throw new VentaInvalidaException("La venta ya está pagada.");
            if (venta.Estado == 3)
                throw new VentaInvalidaException("La venta está anulada y no puede pagarse.");

            var totalPagado = pagos.Sum(p => p.Monto);
            if (totalPagado < venta.TotalFinal)
                throw new NegocioException(
                    $"El monto pagado (${totalPagado:N2}) es menor al total (${venta.TotalFinal:N2}).");

            foreach (var pago in pagos.Where(p => p.Monto > 0))
            {
                await _uow.Pagos.AgregarAsync(new GestionComercial.Dominio.Entidades.Pagos.Pago
                {
                    Id_venta      = idVenta,
                    Id_metodoPago = pago.IdMetodoPago,
                    Monto         = pago.Monto,
                });
            }

            venta.Estado = 2; // Pagada — recién acá
            _uow.Ventas.Actualizar(venta);
            await _uow.GuardarCambiosAsync();
        }

        /// <summary>
        /// Anula la venta y devuelve el stock.
        /// Los pagos ya registrados quedan en BD como historial.
        /// ObtenerTotalDelDiaAsync filtra solo Estado=2 así que
        /// las ventas anuladas no afectan reportes de ingresos.
        /// </summary>
        public async Task CancelarAsync(int id)
        {
            var venta = await _uow.Ventas.ObtenerConDetallesAsync(id)
                ?? throw new VentaInvalidaException($"Venta #{id} no encontrada.");

            if (venta.Estado == 3)
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

            venta.Estado = 3;
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
