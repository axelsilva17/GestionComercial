using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Aplicacion.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación de IAuditoriaAppService.
    /// Delega la lógica de negocio a IAuditoriaServicio y combina los resultados
    /// para optimizar la llamada desde el popup de auditoría de caja.
    /// Incluye KPIs de fraude y filtros avanzados.
    /// </summary>
    public class AuditoriaAppService : IAuditoriaAppService
    {
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly IUnitOfWork _uow;
        private readonly SesionServicio _sesion;

        // Horarios límite para detectar movimientos atípicos
        private const int HORA_INICIO_JORNADA = 8;   // 8:00 AM
        private const int HORA_FIN_JORNADA = 22;       // 10:00 PM

        public AuditoriaAppService(
            IAuditoriaServicio auditoriaServicio,
            IUnitOfWork uow,
            SesionServicio sesion)
        {
            _auditoriaServicio = auditoriaServicio 
                ?? throw new ArgumentNullException(nameof(auditoriaServicio));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _sesion = sesion ?? throw new ArgumentNullException(nameof(sesion));
        }

        public async Task<AuditoriaCompletaCajaDto> ObtenerAuditoriaCompletaCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            // Ejecutar ambas consultas en paralelo para máxima eficiencia
            var tareaAuditoriaCajas = _auditoriaServicio.ObtenerAuditoriaCajaAsync(fechaDesde, fechaHasta);
            var tareaMovimientosCaja = _auditoriaServicio.ObtenerAuditoriaMovimientoCajaAsync(fechaDesde, fechaHasta);

            await Task.WhenAll(tareaAuditoriaCajas, tareaMovimientosCaja);

            return new AuditoriaCompletaCajaDto
            {
                AuditoriaCajas = (await tareaAuditoriaCajas).ToList(),
                MovimientosCaja = (await tareaMovimientosCaja).ToList()
            };
        }

        /// <inheritdoc />
        public async Task<KpiFraudeDto> CalcularKpisFraudeAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            var kpi = new KpiFraudeDto();
            var idSucursal = _sesion.IdSucursal != 0 ? _sesion.IdSucursal : 1; // Default a 1 si no hay sesión

            // 1. Caja con mayor diferencia
            kpi.CajaMayorDiferencia = await ObtenerCajaMayorDiferenciaAsync(fechaDesde, fechaHasta, idSucursal);

            // 2. Ventas anuladas por usuario
            kpi.VentasAnuladasPorUsuario = await ObtenerVentasAnuladasPorUsuarioAsync(fechaDesde, fechaHasta);

            // 3. Movimientos fuera de horario
            kpi.MovimientosFueraHorario = await ObtenerMovimientosFueraHorarioAsync(fechaDesde, fechaHasta);

            // 4. Forma de pago por vendedor
            kpi.FormasPagoPorVendedor = await ObtenerFormasPagoPorVendedorAsync(fechaDesde, fechaHasta);

            return kpi;
        }

        /// <inheritdoc />
        public async Task<AuditoriaFiltradaDto> ObtenerAuditoriaFiltradaAsync(
            FiltroAuditoriaDto filtros)
        {
            var resultado = new AuditoriaFiltradaDto
            {
                FiltrosAplicados = filtros
            };

            // Usar el repositorio directamente para filtros flexibles
            var query = _uow.Auditoria.ObtenerAuditoriaFiltradaAsync(
                idUsuario: filtros.IdUsuario,
                tipoOperacion: filtros.TipoOperacionCodigo,
                nombreTabla: filtros.NombreTabla,
                fechaDesde: filtros.FechaDesde,
                fechaHasta: filtros.FechaHasta);

            var entidades = await query;

            // Convertir a DTOs con deserialización
            var dtos = entidades.Select(e => MapearADto(e)).ToList();

            resultado.TotalRegistros = dtos.Count;
            resultado.PaginaActual = filtros.PaginaActual;
            resultado.TamanioPagina = filtros.TamanioPagina;

            // Aplicar paginación
            var skip = (resultado.PaginaActual - 1) * resultado.TamanioPagina;
            resultado.Registros = dtos
                .Skip(skip)
                .Take(resultado.TamanioPagina)
                .ToList();

            return resultado;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaCajaDeserializadaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? idUsuario = null,
            string? tipoOperacion = null)
        {
            var entidades = await _uow.Auditoria.ObtenerAuditoriaFiltradaAsync(
                idUsuario: idUsuario,
                tipoOperacion: tipoOperacion != null ? (tipoOperacion switch
                {
                    "Creación" => 1,
                    "Modificación" => 2,
                    "Eliminación" => 3,
                    _ => (int?)null
                }) : null,
                nombreTabla: "Cajas",
                fechaDesde: fechaDesde,
                fechaHasta: fechaHasta);

            var dtos = entidades.Select(e => MapearADto(e)).ToList();

            // Deserializar JSON para cada registro
            foreach (var dto in dtos)
            {
                dto.DeserializarJson();
            }

            return dtos;
        }

        #region Métodos privados de KPIs

        /// <summary>
        /// Identifica la caja con mayor diferencia entre total de ventas y total cobrado.
        /// </summary>
        private async Task<CajaMayorDiferenciaDto?> ObtenerCajaMayorDiferenciaAsync(
            DateTime? fechaDesde, DateTime? fechaHasta,
            int idSucursal)  // Usar parámetro en vez de hardcodear
        {
            // Obtener cierres de caja en el período
            var cierres = await _uow.Cajas.ObtenerHistorialAsync(
                idSucursal: idSucursal,
                desde: fechaDesde ?? DateTime.Now.AddMonths(-1),
                hasta: fechaHasta ?? DateTime.Now);

            Caja? cajaMayorDiferencia = null;
            decimal mayorDiferenciaAbs = 0;

            foreach (var caja in cierres.Where(c => c.FechaCierre.HasValue))
            {
                var totalVentas = caja.Ventas?
                    .Where(v => v.Estado != (int)EstadoVentaEnum.Anulada)  // Solo ventas NO anuladas
                    .Sum(v => v.TotalFinal) ?? 0;

                var montoFinal = caja.MontoFinal ?? 0;
                var diferencia = Math.Abs(montoFinal - totalVentas);

                if (diferencia > mayorDiferenciaAbs)
                {
                    mayorDiferenciaAbs = diferencia;
                    cajaMayorDiferencia = caja;
                }
            }

            if (cajaMayorDiferencia == null)
                return null;

            var totalVentasFinal = cajaMayorDiferencia.Ventas?
                .Where(v => v.Estado != (int)EstadoVentaEnum.Anulada)
                .Sum(v => v.TotalFinal) ?? 0;

            var dif = (cajaMayorDiferencia.MontoFinal ?? 0) - totalVentasFinal;
            var porcentaje = totalVentasFinal != 0 ? Math.Abs(dif / totalVentasFinal * 100) : 0;

            return new CajaMayorDiferenciaDto
            {
                IdCaja = cajaMayorDiferencia.Id,
                FechaApertura = cajaMayorDiferencia.FechaApertura,
                UsuarioCierre = cajaMayorDiferencia.UsuarioCierre?.Nombre ?? "—",
                TotalVentas = totalVentasFinal,
                TotalCobrado = cajaMayorDiferencia.MontoFinal ?? 0,
                Diferencia = dif,
                PorcentajeDiferencia = porcentaje,
                NivelAlerta = porcentaje switch
                {
                    > 10 => "Critical",
                    > 5 => "Warning",
                    _ => "Normal"
                }
            };
        }

        /// <summary>
        /// Obtiene ventas anuladas agrupadas por usuario.
        /// </summary>
        private async Task<List<VentaAnuladaDto>> ObtenerVentasAnuladasPorUsuarioAsync(
            DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var desde = fechaDesde ?? DateTime.Now.AddMonths(-1);
            var hasta = fechaHasta ?? DateTime.Now;

            var ventasAnuladas = await _uow.Ventas.ObtenerVentasAnuladasAsync(desde, hasta);

            var agrupado = ventasAnuladas
                .GroupBy(v => new { v.Id_usuario, Nombre = v.Usuario?.Nombre ?? "Desconocido" })
                .Select(g => new VentaAnuladaDto
                {
                    IdUsuario = g.Key.Id_usuario,
                    NombreUsuario = g.Key.Nombre,
                    Cantidad = g.Count(),
                    TotalAnulado = g.Sum(v => v.TotalFinal)
                })
                .OrderByDescending(v => v.Cantidad)
                .ToList();

            // Calcular porcentaje del total
            var totalGeneral = agrupado.Sum(v => v.TotalAnulado);
            foreach (var item in agrupado)
            {
                item.PorcentajeDelTotal = totalGeneral != 0
                    ? $"{item.TotalAnulado / totalGeneral * 100:F1}%"
                    : "0%";
            }

            return agrupado;
        }

        /// <summary>
        /// Obtiene movimientos de caja fuera del horario laboral (antes 8am, después 10pm).
        /// </summary>
        private async Task<List<MovimientoFueraHorarioDto>> ObtenerMovimientosFueraHorarioAsync(
            DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var desde = fechaDesde ?? DateTime.Now.AddMonths(-1);
            var hasta = fechaHasta ?? DateTime.Now;

            var movimientos = await _uow.MovimientosCaja.ObtenerPorPeriodoAsync(desde, hasta);

            return movimientos
                .Where(m => m.Fecha.Hour < HORA_INICIO_JORNADA || m.Fecha.Hour >= HORA_FIN_JORNADA)
                .Select(m => new MovimientoFueraHorarioDto
                {
                    Id = m.Id,
                    IdCaja = m.Id_caja,
                    Fecha = m.Fecha,
                    Hora = m.Fecha.Hour,
                    TipoMovimiento = m.Tipo == 1 ? "Ingreso" : "Egreso",
                    Monto = m.Monto,
                    Usuario = m.Usuario?.Nombre ?? "—",
                    Concepto = m.Concepto ?? "—",
                    Razon = m.Fecha.Hour < HORA_INICIO_JORNADA
                        ? $"Antes de {HORA_INICIO_JORNADA}:00"
                        : $"Después de {HORA_FIN_JORNADA}:00"
                })
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }

        /// <summary>
        /// Obtiene desglose de formas de pago por vendedor.
        /// </summary>
        private async Task<List<FormaPagoVendedorDto>> ObtenerFormasPagoPorVendedorAsync(
            DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var desde = fechaDesde ?? DateTime.Now.AddMonths(-1);
            var hasta = fechaHasta ?? DateTime.Now;

            var ventas = await _uow.Ventas.ObtenerPorPeriodoAsync(desde, hasta);
            var pagos = await _uow.Pagos.ObtenerPagosPorPeriodoAsync(desde, hasta);

            var resultado = new List<FormaPagoVendedorDto>();

            var groupedByUser = ventas
                .Where(v => v.Estado != (int)EstadoVentaEnum.Anulada)
                .GroupBy(v => new { v.Id_usuario, NombreVendedor = v.Usuario?.Nombre ?? "Desconocido" });

            foreach (var grupo in groupedByUser)
            {
                var vendedorPagos = pagos.Where(p => grupo.Any(v => v.Id == p.Id_venta)).ToList();
                var ventaIds = grupo.Select(v => v.Id).ToHashSet();

                var dto = new FormaPagoVendedorDto
                {
                    IdUsuario = grupo.Key.Id_usuario,
                    NombreUsuario = grupo.Key.NombreVendedor,
                    TotalEfectivo = vendedorPagos
                        .Where(p => p.Id_metodoPago == (int)MetodoPagoEnum.Efectivo)
                        .Sum(p => p.Monto),
                    TotalTarjeta = vendedorPagos
                        .Where(p => p.Id_metodoPago == (int)MetodoPagoEnum.Tarjeta)
                        .Sum(p => p.Monto),
                    TotalTransferencia = vendedorPagos
                        .Where(p => p.Id_metodoPago == (int)MetodoPagoEnum.Transferencia)
                        .Sum(p => p.Monto),
                    TotalOtro = vendedorPagos
                        .Where(p => p.Id_metodoPago > (int)MetodoPagoEnum.Transferencia)
                        .Sum(p => p.Monto),
                    CantidadVentas = grupo.Count()
                };

                resultado.Add(dto);
            }

            return resultado.OrderByDescending(r => r.CantidadVentas).ToList();
        }

        #endregion

        /// <summary>
        /// Mapea una entidad AuditoriaLog a AuditoriaLogDto.
        /// </summary>
        private static AuditoriaLogDto MapearADto(Dominio.Entidades.Auditoria.AuditoriaLog entidad)
        {
            return new AuditoriaLogDto
            {
                Id = entidad.Id,
                NombreTabla = entidad.NombreTabla,
                RegistroId = entidad.RegistroId,
                TipoOperacion = entidad.TipoOperacion switch
                {
                    1 => "Creación",
                    2 => "Modificación",
                    3 => "Eliminación",
                    _ => "Desconocido"
                },
                Usuario = entidad.NombreUsuario ?? "—",
                FechaOperacion = entidad.FechaOperacion,
                ValoresAnteriores = entidad.ValoresAnteriores,
                ValoresNuevos = entidad.ValoresNuevos
            };
        }
    }
}
