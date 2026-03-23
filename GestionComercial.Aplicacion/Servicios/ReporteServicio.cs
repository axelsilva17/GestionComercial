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
            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(desde, hasta, idSucursal);
            return ventas
                .GroupBy(v => v.Id_usuario)
                .Select(g => new ReporteVendedorDto
                {
                    IdUsuario      = g.Key,
                    UsuarioNombre  = g.First().Usuario != null
                        ? $"{g.First().Usuario!.Nombre} {g.First().Usuario!.Apellido}"
                        : string.Empty,
                    Sucursal       = g.First().Sucursal?.Nombre ?? string.Empty,
                    CantidadVentas = g.Count(),
                    TotalVendido   = g.Sum(v => v.TotalFinal),
                    PromedioVenta  = g.Average(v => v.TotalFinal),
                    TotalDescuentos = g.Sum(v => v.TotalDescuento),
                });
        }

        public async Task<IEnumerable<ReporteMargenDto>> MargenPorProductoAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            return ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .Select(g =>
                {
                    var ingresos   = g.Sum(d => d.Subtotal);
                    var costo      = g.Sum(d => d.CostoUnitario * d.Cantidad);
                    var margenTotal = ingresos - costo;
                    return new ReporteMargenDto
                    {
                        IdProducto       = g.Key,
                        ProductoNombre   = g.First().Producto?.Nombre ?? string.Empty,
                        Categoria        = g.First().Producto?.Categoria?.Nombre ?? string.Empty,
                        PrecioVenta      = g.First().PrecioUnitario,
                        PrecioCosto      = g.First().CostoUnitario,
                        MargenUnitario   = g.First().PrecioUnitario - g.First().CostoUnitario,
                        MargenPorcentaje = ingresos > 0 ? (margenTotal / ingresos) * 100 : 0,
                        CantidadVendida  = (int)g.Sum(d => d.Cantidad),
                        MargenTotal      = margenTotal,
                    };
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
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            return ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .Select(g =>
                {
                    var ingresos   = g.Sum(d => d.Subtotal);
                    var costo      = g.Sum(d => d.CostoUnitario * d.Cantidad);
                    var margenTotal = ingresos - costo;
                    return new ReporteRotacionDto
                    {
                        IdProducto       = g.Key,
                        ProductoNombre   = g.First().Producto?.Nombre ?? string.Empty,
                        Categoria        = g.First().Producto?.Categoria?.Nombre ?? string.Empty,
                        StockActual      = (int)(g.First().Producto?.StockActual ?? 0),
                        CantidadVendida  = (int)g.Sum(d => d.Cantidad),
                        CantidadComprada = 0, // requiere cruzar con compras
                        IndiceRotacion   = g.First().Producto?.StockActual > 0
                            ? g.Sum(d => d.Cantidad) / g.First().Producto!.StockActual
                            : 0,
                        UltimaVenta  = ventas
                            .Where(v => v.Detalles.Any(d => d.Id_producto == g.Key))
                            .Max(v => v.Fecha),
                        UltimaCompra = DateTime.MinValue,
                    };
                })
                .OrderByDescending(r => r.CantidadVendida);
        }

        public async Task<IEnumerable<ReporteTopProductoDto>> TopProductosAsync(int idSucursal, DateTime desde, DateTime hasta, int top = 20)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(_uow.Sucursales.ObtenerPorIdAsync(idSucursal).Result?.Id_empresa ?? 0, desde, hasta);
            var ventasSucursal = ventas.Where(v => v.Id_sucursal == idSucursal);
            return ventasSucursal
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .Select(g =>
                {
                    var ingresos = g.Sum(d => d.Subtotal);
                    var costo = g.Sum(d => d.CostoUnitario * d.Cantidad);
                    var margenTotal = ingresos - costo;
                    return new ReporteTopProductoDto
                    {
                        IdProducto = g.Key,
                        ProductoNombre = g.First().Producto?.Nombre ?? string.Empty,
                        Categoria = g.First().Producto?.Categoria?.Nombre ?? string.Empty,
                        CantidadVendida = (int)g.Sum(d => d.Cantidad),
                        Ingresos = ingresos,
                        MargenTotal = margenTotal,
                        MargenPorcentaje = ingresos > 0 ? (double)(margenTotal / ingresos) * 100 : 0,
                    };
                })
                .OrderByDescending(r => r.CantidadVendida)
                .Take(top);
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
                Cantidad = 0, // No tenemos quantity from this method, would need another call
                Porcentaje = totalGeneral > 0 ? (double)(p.Total / totalGeneral) * 100 : 0,
});
        }
    }
}
