using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface ICompraRepositorio : IRepositorioBase<Compra>
    {
        Task<Compra?> ObtenerConDetallesAsync(int idCompra);
        Task<IEnumerable<Compra>> ObtenerPorProveedorAsync(int idProveedor);
        Task<IEnumerable<Compra>> ObtenerPorSucursalAsync(int idSucursal);
    }
}