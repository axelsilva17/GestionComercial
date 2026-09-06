using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class CompraRepositorio : RepositorioBase<Compra>, ICompraRepositorio
    {
        public CompraRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Compra?> ObtenerConDetallesAsync(int idCompra, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(c => c.Detalles).ThenInclude(d => d.Producto)
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == idCompra, ct);

        public async Task<IEnumerable<Compra>> ObtenerPorProveedorAsync(int idProveedor, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_proveedor == idProveedor)
                .Include(c => c.Detalles)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Compra>> ObtenerPorSucursalAsync(int idSucursal, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal)
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Compra>> ObtenerPorPeriodoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal && c.Fecha >= desde && c.Fecha <= hasta)
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync(ct);

        // ── Nuevo: métricas agregadas en SQL ─────────────────────────────────────
        public async Task<(decimal Total, int Count, decimal Promedio, string ProveedorTop, int ProductosRepuestos)?> 
            ObtenerMetricasComprasAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<MetricasComprasRaw>(
                    @"SELECT 
                          COALESCE(SUM(c.Total), 0) AS Total,
                          COUNT(*) AS Count,
                          CASE WHEN COUNT(*) > 0 THEN COALESCE(SUM(c.Total), 0) / COUNT(*) ELSE 0 END AS Promedio,
                          (
                              SELECT p.Nombre
                              FROM Compra c2
                              INNER JOIN Proveedor p ON c2.Id_proveedor = p.Id
                              WHERE c2.Id_sucursal = c.Id_sucursal
                                AND c2.Fecha >= {1}
                                AND c2.Fecha <= {2}
                              GROUP BY c2.Id_proveedor
                              ORDER BY COUNT(*) DESC
                              LIMIT 1
                          ) AS ProveedorTop,
                          COALESCE(SUM(cd.Cantidad), 0) AS ProductosRepuestos
                      FROM Compra c
                      LEFT JOIN CompraDetalle cd ON c.Id = cd.Id_compra
                      WHERE c.Id_sucursal = {0}
                        AND c.Fecha >= {1}
                        AND c.Fecha <= {2}",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);

            var r = rows.FirstOrDefault();
            if (r == null || r.Count == 0) return null;
            return (r.Total, r.Count, r.Promedio, r.ProveedorTop ?? "—", r.ProductosRepuestos);
        }

        // ── Nuevo: compras paginadas ────────────────────────────────────────────
        public async Task<(IEnumerable<Compra> Items, int TotalCount)> ObtenerPorSucursalPaginadoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int page, int pageSize,
            int? idProveedor = null, string? busquedaProveedor = null, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal && c.Fecha >= desde && c.Fecha <= hasta);

            if (idProveedor is > 0)
                query = query.Where(c => c.Id_proveedor == idProveedor);

                if (!string.IsNullOrWhiteSpace(busquedaProveedor))
                    query = query.Where(c => c.Proveedor != null
                        && EF.Functions.Collate(c.Proveedor.Nombre, "NOCASE").Contains(busquedaProveedor));

            query = query
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles)
                .OrderByDescending(c => c.Fecha);

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        // Tipo para SqlQueryRaw
        private record MetricasComprasRaw(
            decimal Total,
            int Count,
            decimal Promedio,
            string? ProveedorTop,
            int ProductosRepuestos);
    }
}