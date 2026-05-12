using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ProveedorProductoCostoRepositorio : RepositorioBase<ProveedorProductoCosto>, IProveedorProductoCostoRepositorio
    {
        public ProveedorProductoCostoRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<ProveedorProductoCosto>> ObtenerPorProveedorAsync(int idProveedor)
            => await _dbSet.Where(p => p.IdProveedor == idProveedor).ToListAsync();

        public async Task<ProveedorProductoCosto?> ObtenerPorProveedorYProductoAsync(int idProveedor, int idProducto)
            => await _dbSet.FirstOrDefaultAsync(p => p.IdProveedor == idProveedor && p.IdProducto == idProducto);
    }
}
