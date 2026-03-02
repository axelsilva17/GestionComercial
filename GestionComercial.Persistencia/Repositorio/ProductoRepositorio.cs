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
    }
}
