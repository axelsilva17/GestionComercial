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
                .Select(g =>
                {
                    var primerVenta = g.FirstOrDefault();
                    return new ReporteVendedorDto
                    {
                        IdUsuario      = g.Key,
                        UsuarioNombre  = primerVenta?.Usuario != null
                            ? $"{primerVenta.Usuario.Nombre} {primerVenta.Usuario.Apellido}"
                            : string.Empty,
                        Sucursal       = primerVenta?.Sucursal?.Nombre ?? string.Empty,
                        CantidadVentas = g.Count(),
                        TotalVendido   = g.Sum(v => v.TotalFinal),
                        PromedioVenta  = g.Average(v => v.TotalFinal),
                        TotalDescuentos = g.Sum(v => v.TotalDescuento),
                    };
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
                    var primerDetalle = g.FirstOrDefault();
                    var ingresos   = g.Sum(d => d.Subtotal);
                    var costo      = g.Sum(d => d.CostoUnitario * d.Cantidad);
                    var margenTotal = ingresos - costo;
                    return new ReporteMargenDto
                    {
                        IdProducto       = g.Key,
                        ProductoNombre   = primerDetalle?.Producto?.Nombre ?? string.Empty,
                        Categoria        = primerDetalle?.Producto?.Categoria?.Nombre ?? string.Empty,
                        PrecioVenta      = primerDetalle?.PrecioUnitario ?? 0,
                        PrecioCosto      = primerDetalle?.CostoUnitario ?? 0,
                        MargenUnitario   = (primerDetalle?.PrecioUnitario ?? 0) - (primerDetalle?.CostoUnitario ?? 0),
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
                    var primerDetalle = g.FirstOrDefault();
                    var ingresos   = g.Sum(d => d.Subtotal);
                    var costo      = g.Sum(d => d.CostoUnitario * d.Cantidad);
                    var margenTotal = ingresos - costo;
                    var stockActual = primerDetalle?.Producto?.StockActual ?? 0;
                    return new ReporteRotacionDto
                    {
                        IdProducto       = g.Key,
                        ProductoNombre   = primerDetalle?.Producto?.Nombre ?? string.Empty,
                        Categoria        = primerDetalle?.Producto?.Categoria?.Nombre ?? string.Empty,
                        StockActual      = (int)stockActual,
                        CantidadVendida  = (int)g.Sum(d => d.Cantidad),
                        CantidadComprada = 0, // requiere cruzar con compras
                        IndiceRotacion   = stockActual > 0
                            ? g.Sum(d => d.Cantidad) / stockActual
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
            var sucursal = await _uow.Sucursales.ObtenerPorIdAsync(idSucursal);
            var idEmpresa = sucursal?.Id_empresa ?? 0;
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            var ventasSucursal = ventas.Where(v => v.Id_sucursal == idSucursal);
            return ventasSucursal
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .Select(g =>
                {
                    var primerDetalle = g.FirstOrDefault();
                    var ingresos = g.Sum(d => d.Subtotal);
                    var costo = g.Sum(d => d.CostoUnitario * d.Cantidad);
                    var margenTotal = ingresos - costo;
                    return new ReporteTopProductoDto
                    {
                        IdProducto = g.Key,
                        ProductoNombre = primerDetalle?.Producto?.Nombre ?? string.Empty,
                        Categoria = primerDetalle?.Producto?.Categoria?.Nombre ?? string.Empty,
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
                Cantidad = 0,
                Porcentaje = totalGeneral > 0 ? (double)(p.Total / totalGeneral) * 100 : 0,
            });
        }

        public async Task<IEnumerable<VentaPorDiaDto>> VentasPorDiaAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            return ventas
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new VentaPorDiaDto
                {
                    Dia = g.Key.ToString("dd/MM"),
                    Total = g.Sum(v => v.TotalFinal),
                    Cantidad = g.Count(),
                })
                .OrderBy(d => d.Dia);
        }

        public async Task<IEnumerable<VentaPorSucursalDto>> VentasPorSucursalAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            var totalGeneral = ventas.Sum(v => v.TotalFinal);
            return ventas
                .GroupBy(v => v.Id_sucursal)
                .Select(g => new VentaPorSucursalDto
                {
                    SucursalNombre = g.FirstOrDefault()?.Sucursal?.Nombre ?? $"Sucursal {g.Key}",
                    Total = g.Sum(v => v.TotalFinal),
                    Cantidad = g.Count(),
                    Porcentaje = totalGeneral > 0 ? (g.Sum(v => v.TotalFinal) / totalGeneral) * 100 : 0,
                })
                .OrderByDescending(s => s.Total);
        }

        public async Task<KpiGeneralDto> KpisGeneralesAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerConDetallesPorFechaAsync(idEmpresa, desde, hasta);
            var totalVentas = ventas.Sum(v => v.TotalFinal);
            var totalTransacciones = ventas.Count();
            var stockCritico = await _uow.Productos.ObtenerStockCriticoAsync(idEmpresa);

            // Top vendedor
            var topVendedor = ventas
                .GroupBy(v => v.Id_usuario)
                .OrderByDescending(g => g.Sum(v => v.TotalFinal))
                .FirstOrDefault();
            var nombreVendedor = topVendedor?.FirstOrDefault()?.Usuario != null
                ? $"{topVendedor.First().Usuario.Nombre} {topVendedor.First().Usuario.Apellido}"
                : "";

            // Top producto
            var topProducto = ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => d.Id_producto)
                .OrderByDescending(g => g.Sum(d => d.Subtotal))
                .FirstOrDefault();
            var nombreProducto = topProducto?.FirstOrDefault()?.Producto?.Nombre ?? "";

            return new KpiGeneralDto
            {
                TotalVentasPeriodo = totalVentas,
                TotalTransacciones = totalTransacciones,
                TicketPromedio = totalTransacciones > 0 ? totalVentas / totalTransacciones : 0,
                ProductosBajoStock = stockCritico.Count(),
                MejorVendedor = nombreVendedor,
                MejorProducto = nombreProducto,
            };
        }
    }
}
