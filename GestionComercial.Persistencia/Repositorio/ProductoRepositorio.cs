using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ProductoRepositorio : RepositorioBase<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra)
            => await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.CodigoBarra == codigoBarra && p.Activo);
        public async Task<bool> ExisteCodigoBarraAsync(string codigo, int idEmpresa)
    => await _dbSet.AnyAsync(p => p.CodigoBarra == codigo && p.Id_empresa == idEmpresa);

        public async Task<bool> ExisteNombreEnCategoriaAsync(string nombre, int idCategoria, int idEmpresa)
            => await _dbSet.AnyAsync(p => p.Nombre == nombre
                                       && p.Id_categoria == idCategoria
                                       && p.Id_empresa == idEmpresa);

        public async Task<int> ObtenerStockAsync(int idProducto)
            => (int)await _dbSet
                .Where(p => p.Id == idProducto)
                .Select(p => p.StockActual)
                .FirstOrDefaultAsync();
        public async Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int idEmpresa)
            => await _dbSet
                .Where(p => p.Id_empresa == idEmpresa && p.Activo && p.StockActual <= p.StockMinimo)
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

        public async Task<IEnumerable<Producto>> ObtenerPorEmpresaAsync(int idEmpresa)
            => await _dbSet
                .Where(p => p.Id_empresa == idEmpresa && p.Activo)
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

        public async Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa)
        {
            // Materializar primero y ordenar en memoria porque SQLite no soporta ORDER BY con decimal
            var productos = await _dbSet
                .Where(p => p.Id_empresa == idEmpresa && p.Activo && p.StockActual <= p.StockMinimo)
                .Include(p => p.Categoria)
                .ToListAsync();
            return productos.OrderBy(p => p.StockActual);
        }

        public async Task<Producto?> ObtenerPorIdConDetallesAsync(int id)
            => await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.Id == id);

        ///         /// Agrega muchos productos en una sola operación, optimizado para importación masiva.
        public async Task AgregarRangoMasivoAsync(IEnumerable<Producto> productos, bool disableTracking = true)
        {
            if (disableTracking)
            {
                // Desactivar change tracking para bulk insert (ahorra memoria y CPU)
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
            }

            await _dbSet.AddRangeAsync(productos);
            await _context.SaveChangesAsync();

            if (disableTracking)
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        // ── Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion ──

        public async Task<List<UnidadMedida>> ObtenerUnidadesMedidaDistintasAsync()
            => await _dbSet
                .Include(p => p.UnidadMedida)
                .Select(p => p.UnidadMedida)
                .Where(u => u != null)
                .Distinct()
                .ToListAsync()!;

        public async Task<List<Producto>> ObtenerPorCategoriaAsync(int idCategoria)
            => await _dbSet
                .Where(p => p.Id_categoria == idCategoria)
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .ToListAsync();

        public async Task<List<Producto>> ObtenerConCodigoBarraPorEmpresaAsync(int idEmpresa)
            => await _dbSet
                .Where(p => p.Id_empresa == idEmpresa && p.CodigoBarra != null)
                .ToListAsync();

        public async Task<int> ContarProductosConStockBajoAsync(int idEmpresa)
            => await _dbSet
                .CountAsync(p => p.Id_empresa == idEmpresa
                              && p.Activo
                              && p.StockActual <= p.StockMinimo
                              && p.StockActual > 0);

        public async Task<List<Producto>> ObtenerConStockBajoConLimiteAsync(int idEmpresa, int limite)
            => await _dbSet
                .Where(p => p.Id_empresa == idEmpresa
                         && p.Activo
                         && p.StockActual <= p.StockMinimo
                         && p.StockActual > 0)
                .Include(p => p.Categoria)
                .OrderBy(p => p.StockActual)
                .Take(limite)
                .ToListAsync();
    }
}