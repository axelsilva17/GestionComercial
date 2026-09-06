using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ProductoRepositorio : RepositorioBase<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.CodigoBarra == codigoBarra && p.Activo, ct);

        public async Task<bool> ExisteCodigoBarraAsync(string codigo, int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AnyAsync(p => p.CodigoBarra == codigo && p.Id_empresa == idEmpresa, ct);

        public async Task<bool> ExisteNombreEnCategoriaAsync(string nombre, int idCategoria, int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AnyAsync(p => p.Nombre == nombre
                                       && p.Id_categoria == idCategoria
                                       && p.Id_empresa == idEmpresa, ct);

        public async Task<int> ObtenerStockAsync(int idProducto, CancellationToken ct = default)
            => (int)await _dbSet
                .Where(p => p.Id == idProducto)
                .Select(p => p.StockActual)
                .FirstOrDefaultAsync(ct);

        public async Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa && p.Activo && p.StockActual <= p.StockMinimo)
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nombre)
                .ToListAsync(ct);

        // Materialize before ordering: SQLite does not support ORDER BY on decimal columns.
        // The filtered set (under-stock only) is small, so in-memory sort is safe.
        public async Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa, CancellationToken ct = default)
        {
            var lista = await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa && p.Activo && p.StockActual <= p.StockMinimo)
                .Include(p => p.Categoria)
                .ToListAsync(ct);
            return lista.OrderBy(p => p.StockActual);
        }

        // Conteo en SQL con el mismo filtro que ObtenerStockCriticoAsync (KPI sin materializar).
        public async Task<int> ContarStockCriticoAsync(int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .CountAsync(p => p.Id_empresa == idEmpresa && p.Activo && p.StockActual <= p.StockMinimo, ct);

        public async Task<IEnumerable<Producto>> ObtenerPorEmpresaAsync(int idEmpresa, bool soloActivos = true, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking().Where(p => p.Id_empresa == idEmpresa);
            if (soloActivos)
                query = query.Where(p => p.Activo);
            return await query
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .OrderBy(p => p.Nombre)
                .ToListAsync(ct);
        }

        public async Task<Producto?> ObtenerPorIdConDetallesAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

        ///         /// Agrega muchos productos en una sola operación, optimizado para importación masiva.
        public async Task AgregarRangoMasivoAsync(IEnumerable<Producto> productos, bool disableTracking = true, CancellationToken ct = default)
        {
            if (disableTracking)
            {
                // Desactivar change tracking para bulk insert (ahorra memoria y CPU)
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
            }

            await _dbSet.AddRangeAsync(productos, ct);
            await _context.SaveChangesAsync(ct);

            if (disableTracking)
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        // ── Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion ──

        public async Task<List<UnidadMedida>> ObtenerUnidadesMedidaDistintasAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(p => p.UnidadMedida)
                .Select(p => p.UnidadMedida)
                .Where(u => u != null)
                .Distinct()
                .ToListAsync(ct)!;

        public async Task<List<Producto>> ObtenerPorCategoriaAsync(int idCategoria, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(p => p.Id_categoria == idCategoria)
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .ToListAsync(ct);

        public async Task<List<Producto>> ObtenerConCodigoBarraPorEmpresaAsync(int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa && p.CodigoBarra != null)
                .ToListAsync(ct);

        public async Task<int> ContarProductosConStockBajoAsync(int idEmpresa, CancellationToken ct = default)
            => await _dbSet
                .CountAsync(p => p.Id_empresa == idEmpresa
                              && p.Activo
                              && p.StockActual <= p.StockMinimo
                              && p.StockActual > 0, ct);

        // Materialize before ordering: SQLite does not support ORDER BY on decimal columns.
        // The filtered set (under-stock, positive stock only) is small, so in-memory sort+take is safe.
        public async Task<List<Producto>> ObtenerConStockBajoConLimiteAsync(int idEmpresa, int limite, CancellationToken ct = default)
        {
            var lista = await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa
                         && p.Activo
                         && p.StockActual <= p.StockMinimo
                         && p.StockActual > 0)
                .Include(p => p.Categoria)
                .ToListAsync(ct);
            return lista.OrderBy(p => p.StockActual).Take(limite).ToList();
        }

        // ── Búsqueda con StartsWith (prefijo) para uso de índices ────────────────
        public async Task<List<Producto>> BuscarProductosAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking().Where(p => p.Id_empresa == idEmpresa);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                var term = texto.Trim();
                // StartsWith → EF Core traduce a LIKE 'term%' (usa índice)
                query = query.Where(p =>
                    EF.Functions.Like(p.Nombre, term + "%") ||
                    EF.Functions.Like(p.CodigoBarra, term + "%"));
            }

            if (idCategoria.HasValue && idCategoria.Value > 0)
                query = query.Where(p => p.Id_categoria == idCategoria.Value);

            if (soloActivos.HasValue)
                query = query.Where(p => p.Activo == soloActivos.Value);

            return await query
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nombre)
                .Take(take)
                .ToListAsync(ct);
        }

        // ── Búsqueda con Contains (subcadena LIKE '%term%') ──────────────────────
        public async Task<List<Producto>> BuscarProductosContieneAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking().Where(p => p.Id_empresa == idEmpresa);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                var term = texto.Trim();
                // Contains → EF Core traduce a LIKE '%term%' (subcadena en nombre o código de barra)
                query = query.Where(p =>
                    EF.Functions.Like(p.Nombre, "%" + term + "%") ||
                    EF.Functions.Like(p.CodigoBarra, "%" + term + "%"));
            }

            if (idCategoria.HasValue && idCategoria.Value > 0)
                query = query.Where(p => p.Id_categoria == idCategoria.Value);

            if (soloActivos.HasValue)
                query = query.Where(p => p.Activo == soloActivos.Value);

            return await query
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nombre)
                .Take(take)
                .ToListAsync(ct);
        }

        // ── Búsqueda exacta por código de barras (case-insensitive para SQLite) ──
        public async Task<Producto?> BuscarPorCodigoBarraExactoAsync(int idEmpresa, string codigoBarra, CancellationToken ct = default)
        {
            var term = (codigoBarra ?? string.Empty).Trim();
            if (term.Length == 0) return null;

            return await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa
                    && p.Activo
                    && p.CodigoBarra != null
                    && EF.Functions.Collate(p.CodigoBarra, "NOCASE") == term)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<(IEnumerable<Producto> Items, int TotalCount)> ObtenerPorEmpresaPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, int? idCategoria = null, bool? soloActivos = null, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking().Where(p => p.Id_empresa == idEmpresa);

            if (soloActivos.HasValue)
                query = query.Where(p => p.Activo == soloActivos.Value);

            if (idCategoria.HasValue && idCategoria.Value > 0)
                query = query.Where(p => p.Id_categoria == idCategoria.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                // StartsWith para uso de índices
                query = query.Where(p =>
                    EF.Functions.Like(p.Nombre, term + "%") ||
                    EF.Functions.Like(p.CodigoBarra, term + "%"));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .OrderBy(p => p.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        // Una sola consulta SQL agregada (1 fila) sobre productos activos de la empresa.
        // Evita materializar los 50K productos en memoria para calcular las 3 métricas.
        public async Task<(int ProductosActivos, int ProductosStockBajo, int ProductosSinStock)>
            ObtenerMetricasAsync(int idEmpresa, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<MetricasProducto>(
                    @"SELECT
                             COUNT(*) AS ProductosActivos,
                             SUM(CASE WHEN StockActual > 0 AND StockActual <= 10 THEN 1 ELSE 0 END) AS ProductosStockBajo,
                             SUM(CASE WHEN StockActual <= 0 THEN 1 ELSE 0 END) AS ProductosSinStock
                      FROM Producto
                      WHERE Id_empresa = {0}
                        AND Activo = 1",
                    idEmpresa)
                .ToListAsync(ct);

            var r = rows.FirstOrDefault() ?? new MetricasProducto(0, 0, 0);
            return (r.ProductosActivos, r.ProductosStockBajo, r.ProductosSinStock);
        }
    }

    // ── Tipos para SqlQueryRaw (EF Core 8) ─────────────────────────
    public record MetricasProducto(int ProductosActivos, int ProductosStockBajo, int ProductosSinStock);
}