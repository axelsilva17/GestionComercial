using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class VentaRepositorio : RepositorioBase<Venta>, IVentaRepostorio
    {
        public VentaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Venta?> ObtenerConDetallesAsync(int idVenta)
            => await _dbSet
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .Include(v => v.Detalles).ThenInclude(d => d.Descuentos)
                .Include(v => v.Pagos).ThenInclude(p => p.MetodoPago)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(v => v.Id == idVenta);

        public async Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal)
            => await _dbSet
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta && v.Id_sucursal == idSucursal)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente)
            => await _dbSet
                .Where(v => v.Id_cliente == idCliente)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<IEnumerable<Venta>> ObtenerConDetallesPorFechaAsync(int idEmpresa, DateTime desde, DateTime hasta)
            => await _dbSet
                .Where(v => v.Sucursal.Id_empresa == idEmpresa
                         && v.Fecha >= desde
                         && v.Fecha <= hasta)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto).ThenInclude(p => p!.Categoria)
                .Include(v => v.Usuario)
                .Include(v => v.Sucursal)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal)
        {
            var hoy = DateTime.Today;
            return await _dbSet
                .Where(v => v.Id_sucursal == idSucursal && v.Fecha >= hoy && v.Estado == 2)
                .SumAsync(v => v.TotalFinal);
        }

        public async Task<IEnumerable<Venta>> ObtenerVentasAnuladasAsync(DateTime desde, DateTime hasta)
            => await _dbSet
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta && v.Estado == 3) // 3 = Anulada
                .Include(v => v.Usuario)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<IEnumerable<Venta>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta)
            => await _dbSet
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
                .Include(v => v.Usuario)
                .Include(v => v.Pagos).ThenInclude(p => p.MetodoPago)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>>
            ObtenerTopProductosAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, int top)
        {
            var rows = await _context.Database
                .SqlQueryRaw<TopProductoAgrupado>(
                    @"SELECT vd.Id_producto AS IdProducto,
                             p.Nombre,
                             COALESCE(c.Nombre, '') AS Categoria,
                             CAST(SUM(vd.Cantidad) AS INTEGER) AS Cantidad,
                             SUM(vd.Subtotal) AS Ingresos,
                             SUM(vd.CostoUnitario * vd.Cantidad) AS Costo,
                             MAX(v.Fecha) AS UltimaFecha
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      LEFT JOIN Categoria c ON p.Id_categoria = c.Id
                      WHERE v.Id_sucursal = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY Cantidad DESC
                      LIMIT {3}",
                    idSucursal, desde, hasta, top)
                .ToListAsync();
            return rows.Select(r => (r.IdProducto, r.Nombre, r.Categoria, r.Cantidad, r.Ingresos, r.Costo, r.UltimaFecha)).ToList();
        }

        public async Task<List<(int IdProducto, string Nombre, string Categoria, decimal StockActual, int CantidadVendida, DateTime? UltimaVenta)>>
            ObtenerRotacionProductosAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta)
        {
            var rows = await _context.Database
                .SqlQueryRaw<RotacionProductoAgrupado>(
                    @"SELECT vd.Id_producto AS IdProducto,
                             p.Nombre,
                             COALESCE(c.Nombre, '') AS Categoria,
                             p.StockActual,
                             CAST(SUM(vd.Cantidad) AS INTEGER) AS CantidadVendida,
                             MAX(v.Fecha) AS UltimaVenta
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      LEFT JOIN Categoria c ON p.Id_categoria = c.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY CantidadVendida DESC",
                    idEmpresa, desde, hasta)
                .ToListAsync();
            return rows.Select(r => (r.IdProducto, r.Nombre, r.Categoria, r.StockActual, r.CantidadVendida, r.UltimaVenta)).ToList();
        }

        public async Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>>
            ObtenerTopProductosPorEmpresaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int top)
        {
            var rows = await _context.Database
                .SqlQueryRaw<TopProductoAgrupado>(
                    @"SELECT vd.Id_producto AS IdProducto,
                             p.Nombre,
                             COALESCE(c.Nombre, '') AS Categoria,
                             CAST(SUM(vd.Cantidad) AS INTEGER) AS Cantidad,
                             SUM(vd.Subtotal) AS Ingresos,
                             SUM(vd.CostoUnitario * vd.Cantidad) AS Costo,
                             MAX(v.Fecha) AS UltimaFecha
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      LEFT JOIN Categoria c ON p.Id_categoria = c.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY Cantidad DESC
                      LIMIT {3}",
                    idEmpresa, desde, hasta, top)
                .ToListAsync();
            return rows.Select(r => (r.IdProducto, r.Nombre, r.Categoria, r.Cantidad, r.Ingresos, r.Costo, r.UltimaFecha)).ToList();
        }
    }

    // ── Tipos para SqlQueryRaw (EF Core 8) ─────────────────────────
    public record TopProductoAgrupado(
        int IdProducto, string Nombre, string Categoria,
        int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha);

    public record RotacionProductoAgrupado(
        int IdProducto, string Nombre, string Categoria,
        decimal StockActual, int CantidadVendida, DateTime? UltimaVenta);
}