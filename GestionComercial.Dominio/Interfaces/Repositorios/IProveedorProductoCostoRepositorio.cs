using GestionComercial.Dominio.Entidades.Proveedores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IProveedorProductoCostoRepositorio : IRepositorioBase<ProveedorProductoCosto>
    {
        Task<IEnumerable<ProveedorProductoCosto>> ObtenerPorProveedorAsync(int idProveedor);
        Task<ProveedorProductoCosto?> ObtenerPorProveedorYProductoAsync(int idProveedor, int idProducto);
    }
}
