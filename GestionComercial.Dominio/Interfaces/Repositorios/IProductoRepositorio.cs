using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IProductoRepositorio : IRepositorioBase<Producto>
    {
        Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra);
        Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int idEmpresa);
        Task<IEnumerable<Producto>> ObtenerPorEmpresaAsync(int idEmpresa);
        Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa);
        Task<Producto?> ObtenerPorIdConDetallesAsync(int id);
        Task<bool> ExisteCodigoBarraAsync(string codigo, int idEmpresa);
        Task<bool> ExisteNombreEnCategoriaAsync(string nombre, int idCategoria, int idEmpresa);
        Task<int> ObtenerStockAsync(int idProducto);
        Task AgregarRangoMasivoAsync(IEnumerable<Producto> productos, bool disableTracking = true);
    }
}