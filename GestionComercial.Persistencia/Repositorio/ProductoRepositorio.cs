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
                .Where(p => p.Id_empresa == idEmpresa)
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

        public async Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa)
            => await _dbSet
                .Where(p => p.Id_empresa == idEmpresa && p.Activo && p.StockActual <= p.StockMinimo)
                .Include(p => p.Categoria)
                .OrderBy(p => p.StockActual)
                .ToListAsync();

        public async Task<Producto?> ObtenerPorIdConDetallesAsync(int id)
            => await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// Agrega muchos productos en una sola operación, optimizado para importación masiva.
        /// </summary>
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
    }
}