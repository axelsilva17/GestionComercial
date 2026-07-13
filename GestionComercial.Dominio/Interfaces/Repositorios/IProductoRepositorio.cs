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

        // Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion
        Task<List<UnidadMedida>> ObtenerUnidadesMedidaDistintasAsync();
        Task<List<Producto>> ObtenerPorCategoriaAsync(int idCategoria);
        Task<List<Producto>> ObtenerConCodigoBarraPorEmpresaAsync(int idEmpresa);
        Task<int> ContarProductosConStockBajoAsync(int idEmpresa);
        Task<List<Producto>> ObtenerConStockBajoConLimiteAsync(int idEmpresa, int limite);
    }
}