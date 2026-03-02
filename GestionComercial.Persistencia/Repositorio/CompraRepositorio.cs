using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class CompraRepositorio : RepositorioBase<Compra>, ICompraRepositorio
    {
        public CompraRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Compra?> ObtenerConDetallesAsync(int idCompra)
            => await _dbSet
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

        public async Task<IEnumerable<Compra>> ObtenerPorProveedorAsync(int idProveedor)
            => await _dbSet
                .Where(c => c.Id_proveedor == idProveedor)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
    }
}
