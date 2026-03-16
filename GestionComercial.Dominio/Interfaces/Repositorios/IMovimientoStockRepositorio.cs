using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{

    public interface IMovimientoStockRepositorio : IRepositorioBase<MovimientoStock>
    {
        Task<IEnumerable<MovimientoStock>> ObtenerPorProductoAsync(int idProducto);
        Task<IEnumerable<MovimientoStock>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal);
    }

   
}
