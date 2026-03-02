using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ReporteServicio : IReporteServicio
    {
        private readonly IUnitOfWork _uow;
        public ReporteServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<ReporteVendedorDto>> VentasPorVendedorAsync(int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerPorSucursalYFechaAsync(idSucursal, desde, hasta);
            return ventas
                .GroupBy(v => v.Id_usuario)
                .Select(g => new ReporteVendedorDto
                {
                    NombreVendedor  = $"{g.First().Usuario?.Nombre} {g.First().Usuario?.Apellido}".Trim(),
                    CantidadVentas  = g.Count(),
                    TotalVentas     = g.Sum(v => v.TotalFinal),
                    TicketPromedio  = g.Average(v => v.TotalFinal),
                });
        }

        public async Task<IEnumerable<ReporteMargenDto>> MargenPorProductoAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            return ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .Select(g => new ReporteMargenDto
                {
                    NombreProducto  = g.First().Producto?.Nombre ?? string.Empty,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    Ingresos        = g.Sum(d => d.Subtotal),
                    Costo           = g.Sum(d => d.CostoUnitario * d.Cantidad),
                    Margen          = g.Sum(d => d.Subtotal) > 0
                        ? (double)((g.Sum(d => d.Subtotal) - g.Sum(d => d.CostoUnitario * d.Cantidad))
                          / g.Sum(d => d.Subtotal) * 100)
                        : 0,
                });
        }

        public async Task<IEnumerable<ReportesStockDto>> StockCriticoAsync(int idEmpresa)
        {
            var productos = await _uow.Productos.ObtenerStockCriticoAsync(idEmpresa);
            return productos.Select(p => new ReportesStockDto
            {
                NombreProducto = p.Nombre,
                StockActual    = p.StockActual,
                StockMinimo    = p.StockMinimo,
                Categoria      = p.Categoria?.Nombre ?? string.Empty,
            });
        }

        public async Task<IEnumerable<ReporteRotacionDto>> RotacionProductosAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            return ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .Select(g => new ReporteRotacionDto
                {
                    NombreProducto  = g.First().Producto?.Nombre ?? string.Empty,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    Ingresos        = g.Sum(d => d.Subtotal),
                })
                .OrderByDescending(r => r.CantidadVendida);
        }
    }
}
