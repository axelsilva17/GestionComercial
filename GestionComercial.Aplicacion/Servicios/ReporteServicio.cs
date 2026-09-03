using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ReporteServicio : IReporteServicio
    {
        private readonly IUnitOfWork _uow;
        public ReporteServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<ReporteVendedorDto>> VentasPorVendedorAsync(int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerVentasPorVendedorAgrupadoAsync(idSucursal, desde, hasta);
            return ventas.Select(v => new ReporteVendedorDto
            {
                IdUsuario      = v.IdUsuario,
                UsuarioNombre  = v.UsuarioNombre,
                Sucursal       = v.SucursalNombre,
                CantidadVentas = v.CantidadVentas,
                TotalVendido   = v.TotalVendido,
                PromedioVenta  = v.CantidadVentas > 0 ? v.TotalVendido / v.CantidadVentas : 0,
                TotalDescuentos = v.TotalDescuentos,
            });
        }

        public async Task<IEnumerable<ReporteMargenDto>> MargenPorProductoAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            // Agregación SQL directa — no carga entidades a memoria
            var productos = await _uow.Ventas.ObtenerTopProductosPorEmpresaAgrupadoAsync(idEmpresa, desde, hasta, int.MaxValue);
            return productos.Select(p => new ReporteMargenDto
            {
                IdProducto       = p.IdProducto,
                ProductoNombre   = p.Nombre,
                Categoria        = p.Categoria,
                PrecioVenta      = p.Cantidad > 0 ? p.Ingresos / p.Cantidad : 0,
                PrecioCosto      = p.Cantidad > 0 ? p.Costo / p.Cantidad : 0,
                MargenUnitario   = p.Cantidad > 0 ? (p.Ingresos - p.Costo) / p.Cantidad : 0,
                MargenPorcentaje = p.Ingresos > 0 ? (p.Ingresos - p.Costo) / p.Ingresos * 100 : 0,
                CantidadVendida  = p.Cantidad,
                MargenTotal      = p.Ingresos - p.Costo,
            });
        }

        public async Task<IEnumerable<ReportesStockDto>> StockCriticoAsync(int idEmpresa)
        {
            var productos = await _uow.Productos.ObtenerStockCriticoAsync(idEmpresa);
            return productos.Select(p => new ReportesStockDto
            {
                IdProducto     = p.Id,
                ProductoNombre = p.Nombre,
                Categoria      = p.Categoria?.Nombre ?? string.Empty,
                Sucursal       = string.Empty,
                StockActual    = (int)p.StockActual,
                StockMinimo    = (int)p.StockMinimo,
            });
        }

        public async Task<IEnumerable<ReporteRotacionDto>> RotacionProductosAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            // Agregación SQL directa — no carga entidades a memoria
            var rotacion = await _uow.Ventas.ObtenerRotacionProductosAgrupadoAsync(idEmpresa, desde, hasta);
            return rotacion.Select(r => new ReporteRotacionDto
            {
                IdProducto       = r.IdProducto,
                ProductoNombre   = r.Nombre,
                Categoria        = r.Categoria,
                StockActual      = (int)r.StockActual,
                CantidadVendida  = r.CantidadVendida,
                CantidadComprada = 0,
                IndiceRotacion   = r.StockActual > 0
                    ? r.CantidadVendida / r.StockActual
                    : 0,
                UltimaVenta  = r.UltimaVenta ?? DateTime.MinValue,
                UltimaCompra = DateTime.MinValue,
            });
        }

        public async Task<IEnumerable<ReporteTopProductoDto>> TopProductosAsync(int idSucursal, DateTime desde, DateTime hasta, int top = 20)
        {
            // Agregación SQL directa — no carga entidades a memoria
            var topProductos = await _uow.Ventas.ObtenerTopProductosAgrupadoAsync(idSucursal, desde, hasta, top);
            return topProductos.Select(tp => new ReporteTopProductoDto
            {
                IdProducto       = tp.IdProducto,
                ProductoNombre   = tp.Nombre,
                Categoria        = tp.Categoria,
                CantidadVendida  = tp.Cantidad,
                Ingresos         = tp.Ingresos,
                MargenTotal      = tp.Ingresos - tp.Costo,
                MargenPorcentaje = tp.Ingresos > 0
                    ? (double)((tp.Ingresos - tp.Costo) / tp.Ingresos) * 100
                    : 0,
            });
        }

        public async Task<IEnumerable<ReporteMetodosPagoDto>> MetodosPagoUtilizadosAsync(int idSucursal, DateTime desde, DateTime hasta)
        {
            var pagos = await _uow.Pagos.ObtenerTotalesPorMetodoAsync(idSucursal, desde, hasta);
            var listaPagos = pagos.ToList();
            var totalGeneral = listaPagos.Sum(p => p.Total);
            return listaPagos.Select(p => new ReporteMetodosPagoDto
            {
                Metodo = p.Metodo,
                Total = p.Total,
                Cantidad = 0,
                Porcentaje = totalGeneral > 0 ? (double)(p.Total / totalGeneral) * 100 : 0,
            });
        }

        public async Task<IEnumerable<VentaPorDiaDto>> VentasPorDiaAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerVentasPorDiaAgrupadoAsync(idEmpresa, desde, hasta);
            return ventas.Select(v => new VentaPorDiaDto
            {
                Dia = v.Dia,
                Total = v.Total,
                Cantidad = v.Cantidad,
            });
        }

        public async Task<IEnumerable<VentaPorSucursalDto>> VentasPorSucursalAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerVentasPorSucursalAgrupadoAsync(idEmpresa, desde, hasta);
            var totalGeneral = ventas.Sum(v => v.Total);
            return ventas.Select(v => new VentaPorSucursalDto
            {
                SucursalNombre = v.SucursalNombre,
                Total = v.Total,
                Cantidad = v.Cantidad,
                Porcentaje = totalGeneral > 0 ? (v.Total / totalGeneral) * 100 : 0,
            });
        }

        public async Task<KpiGeneralDto> KpisGeneralesAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta)
        {
            var kpisTask = _uow.Ventas.ObtenerKpisVentasAsync(idEmpresa, idSucursal, desde, hasta);
            var stockTask = _uow.Productos.ObtenerStockCriticoAsync(idEmpresa);
            var topVendedorTask = _uow.Ventas.ObtenerTopVendedorAsync(idEmpresa, desde, hasta);
            var topProductoTask = _uow.Ventas.ObtenerTopProductoAsync(idEmpresa, desde, hasta);

            await Task.WhenAll(kpisTask, stockTask, topVendedorTask, topProductoTask);

            var kpis = await kpisTask;
            var stockCritico = await stockTask;
            var nombreVendedor = await topVendedorTask ?? "";
            var nombreProducto = await topProductoTask ?? "";

            return new KpiGeneralDto
            {
                TotalVentasPeriodo = kpis?.TotalVentas ?? 0,
                TotalTransacciones = kpis?.TotalTransacciones ?? 0,
                TicketPromedio = kpis?.TicketPromedio ?? 0,
                ProductosBajoStock = stockCritico.Count(),
                MejorVendedor = nombreVendedor,
                MejorProducto = nombreProducto,
            };
        }
    }
}
