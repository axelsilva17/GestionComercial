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
        public async Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa, int? umbral = null, CancellationToken ct = default)
        {
            var lista = await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa && p.Activo
                    && p.StockActual <= (umbral.HasValue && umbral.Value > 0 ? (decimal)umbral.Value : p.StockMinimo))
                .Include(p => p.Categoria)
                .ToListAsync(ct);
            return lista.OrderBy(p => p.StockActual);
        }

        // Conteo en SQL con el mismo filtro que ObtenerStockCriticoAsync (KPI sin materializar).
        public async Task<int> ContarStockCriticoAsync(int idEmpresa, int? umbral = null, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .CountAsync(p => p.Id_empresa == idEmpresa && p.Activo
                    && p.StockActual <= (umbral.HasValue && umbral.Value > 0 ? (decimal)umbral.Value : p.StockMinimo), ct);

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

        // Ajuste masivo de precios: una sola UPDATE SQL. El filtro replica EXACTAMENTE el preview
        // del popup (ObtenerPorEmpresaPaginadoAsync): empresa + texto (StartsWith) + categoría + activo.
        // Los precios se tratan como REAL (las columnas decimal se almacenan como texto en SQLite) y
        // cada fila conserva su precio si el resultado NO supera el guard de no-negativos.
        public async Task<int> AplicarAjustePreciosMasivoAsync(
            int idEmpresa, string? texto, int? idCategoria, bool? soloActivos,
            decimal factor, decimal delta, bool esPorcentaje, bool aplicarVenta, bool aplicarCosto,
            CancellationToken ct = default)
        {
            // {0} empresa, {1} aplicarVenta, {2} factor, {3} delta, {4} esPorcentaje, {5} aplicarCosto
            var parametros = new List<object> { idEmpresa };
            var sql = "UPDATE Producto SET " +
                "PrecioVentaActual = CASE WHEN {1} = 1 THEN " +
                "CASE WHEN {4} = 1 THEN " +
                "CASE WHEN ROUND(CAST(PrecioVentaActual AS REAL) * {2}, 2) > 0 " +
                "THEN ROUND(CAST(PrecioVentaActual AS REAL) * {2}, 2) ELSE PrecioVentaActual END " +
                "ELSE " +
                "CASE WHEN CAST(PrecioVentaActual AS REAL) + {3} > 0 " +
                "THEN CAST(PrecioVentaActual AS REAL) + {3} ELSE PrecioVentaActual END END " +
                "ELSE PrecioVentaActual END, " +
                "PrecioCostoActual = CASE WHEN {5} = 1 THEN " +
                "CASE WHEN {4} = 1 THEN " +
                "CASE WHEN ROUND(CAST(PrecioCostoActual AS REAL) * {2}, 2) > 0 " +
                "THEN ROUND(CAST(PrecioCostoActual AS REAL) * {2}, 2) ELSE PrecioCostoActual END " +
                "ELSE " +
                "CASE WHEN CAST(PrecioCostoActual AS REAL) + {3} > 0 " +
                "THEN CAST(PrecioCostoActual AS REAL) + {3} ELSE PrecioCostoActual END END " +
                "ELSE PrecioCostoActual END " +
                "WHERE Id_empresa = {0}";

            parametros.Add(aplicarVenta ? 1 : 0); // {1}
            parametros.Add(factor);               // {2}
            parametros.Add(delta);                // {3}
            parametros.Add(esPorcentaje ? 1 : 0); // {4}
            parametros.Add(aplicarCosto ? 1 : 0); // {5}

            if (idCategoria.HasValue && idCategoria.Value > 0)
            {
                parametros.Add(idCategoria.Value);
                sql += $" AND Id_categoria = {{{parametros.Count - 1}}}";
            }

            if (soloActivos.HasValue)
            {
                parametros.Add(soloActivos.Value ? 1 : 0);
                sql += $" AND Activo = {{{parametros.Count - 1}}}";
            }

            if (!string.IsNullOrWhiteSpace(texto))
            {
                parametros.Add(texto.Trim());
                sql += $" AND (Nombre LIKE {{{parametros.Count - 1}}} || '%' " +
                       $"OR CodigoBarra LIKE {{{parametros.Count - 1}}} || '%')";
            }

            return await _context.Database.ExecuteSqlRawAsync(sql, parametros.ToArray(), ct);
        }
    }

    // ── Tipos para SqlQueryRaw (EF Core 8) ─────────────────────────
    public record MetricasProducto(int ProductosActivos, int ProductosStockBajo, int ProductosSinStock);
}