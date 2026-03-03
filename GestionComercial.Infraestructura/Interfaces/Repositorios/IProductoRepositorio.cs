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
    public interface IProductoRepositorio : IRepositorioBase<Producto>
    {
        Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra);
        Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int idEmpresa);
        Task<IEnumerable<Producto>> ObtenerPorEmpresaAsync(int idEmpresa);
        Task<IEnumerable<object>> ObtenerStockCriticoAsync(int idEmpresa);
        Task<Producto> ObtenerPorIdConDetallesAsync(int id);
    }

}
