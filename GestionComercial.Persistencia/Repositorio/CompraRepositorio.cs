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
                .Include(c => c.Detalles).ThenInclude(d => d.Producto).ThenInclude(p => p.Categoria)
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == idCompra, ct);

        public async Task<IEnumerable<Compra>> ObtenerPorProveedorAsync(int idProveedor, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_proveedor == idProveedor)
                .Include(c => c.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Compra>> ObtenerPorSucursalAsync(int idSucursal, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal)
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Compra>> ObtenerPorPeriodoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal && c.Fecha >= desde && c.Fecha <= hasta)
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync(ct);

        // ── Nuevo: métricas agregadas en SQL ─────────────────────────────────────
        public async Task<(decimal Total, int Count, decimal Promedio, string ProveedorTop, int ProductosRepuestos)?> 
            ObtenerMetricasComprasAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            // NOTA: las métricas se calculan con subconsultas escalares SIN unir CompraDetalle.
            // Un LEFT JOIN a los detalles multiplicaba SUM(c.Total) y COUNT(*) por la cantidad
            // de líneas de cada compra (ej.: una compra con 3 detalles triplicaba su total en
            // el Resumen gerencial). También se excluyen las compras anuladas (Estado = 3),
            // igual que el resto de las agregaciones de reportes.
            var rows = await _context.Database
                .SqlQueryRaw<MetricasComprasRaw>(
                    @"SELECT
                          (SELECT COALESCE(SUM(c2.Total), 0) FROM Compra c2
                           WHERE c2.Id_sucursal = {0} AND c2.Fecha >= {1} AND c2.Fecha <= {2} AND c2.Estado != 3) AS Total,
                          (SELECT COUNT(*) FROM Compra c2
                           WHERE c2.Id_sucursal = {0} AND c2.Fecha >= {1} AND c2.Fecha <= {2} AND c2.Estado != 3) AS Count,
                          (
                              SELECT p.Nombre
                              FROM Compra c2
                              INNER JOIN Proveedor p ON c2.Id_proveedor = p.Id
                              WHERE c2.Id_sucursal = {0}
                                AND c2.Fecha >= {1}
                                AND c2.Fecha <= {2}
                                AND c2.Estado != 3
                              GROUP BY c2.Id_proveedor
                              ORDER BY COUNT(*) DESC
                              LIMIT 1
                          ) AS ProveedorTop,
                          (SELECT COALESCE(SUM(cd.Cantidad), 0) FROM CompraDetalle cd
                           INNER JOIN Compra c3 ON cd.Id_compra = c3.Id
                           WHERE c3.Id_sucursal = {0} AND c3.Fecha >= {1} AND c3.Fecha <= {2} AND c3.Estado != 3) AS ProductosRepuestos",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);

            var r = rows.FirstOrDefault();
            if (r == null || r.Count == 0) return null;
            var promedio = r.Total / r.Count;
            return (r.Total, r.Count, promedio, r.ProveedorTop ?? "—", r.ProductosRepuestos);
        }

        // ── Nuevo: compras paginadas ────────────────────────────────────────────
        public async Task<(IEnumerable<Compra> Items, int TotalCount)> ObtenerPorSucursalPaginadoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int page, int pageSize,
            int? idProveedor = null, string? busquedaProveedor = null, CancellationToken ct = default)
        {
            // Date range always applies (both "Todos" and specific provider).
            // Provider scope: specific provider → filter by provider id;
            // "Todos" → filter by branch (sucursal).
            var query = _dbSet.AsNoTracking()
                .Where(c => c.Fecha >= desde && c.Fecha <= hasta);

            query = idProveedor is > 0
                ? query.Where(c => c.Id_proveedor == idProveedor)
                : query.Where(c => c.Id_sucursal == idSucursal);

            if (!string.IsNullOrWhiteSpace(busquedaProveedor))
            {
                // Case-insensitive provider search. SQLite's instr() (what string.Contains
                // translates to) IGNORES the COLLATE NOCASE on the column — the search was
                // case-sensitive. LIKE against the NOCASE-collated column is case-insensitive
                // (LIKE in SQLite folds ASCII case and the explicit collation reinforces it).
                var termino = busquedaProveedor.Trim();
                query = query.Where(c => c.Proveedor != null
                    && EF.Functions.Like(EF.Functions.Collate(c.Proveedor.Nombre, "NOCASE"), $"%{termino}%"));
            }

            query = query
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(c => c.Fecha);

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        // Tipo para SqlQueryRaw — Promedio no se mapea desde SQL: se calcula en C# (Total / Count)
        private record MetricasComprasRaw(
            decimal Total,
            int Count,
            string? ProveedorTop,
            int ProductosRepuestos);

        // ── Compras agrupadas por mes para reporte gerencial ──────────────────
        public async Task<List<(int AnioMes, decimal Total)>> ObtenerComprasPorMesAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<CompraMesRaw>(
                    @"SELECT CAST(strftime('%Y', c.Fecha) AS INTEGER) * 100 + CAST(strftime('%m', c.Fecha) AS INTEGER) AS AnioMes,
                             COALESCE(SUM(c.Total), 0) AS Total
                      FROM Compra c
                      WHERE c.Id_sucursal = {0}
                        AND c.Fecha >= {1}
                        AND c.Fecha <= {2}
                        AND c.Estado != 3
                      GROUP BY strftime('%Y-%m', c.Fecha)
                      ORDER BY AnioMes ASC",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);

            return rows.Select(r => (r.AnioMes, r.Total)).ToList();
        }

        private record CompraMesRaw(int AnioMes, decimal Total);
    }
}
