using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IProductoRepositorio : IRepositorioBase<Producto>
    {
        Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra, CancellationToken ct = default);
        Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int idEmpresa, CancellationToken ct = default);
        Task<IEnumerable<Producto>> ObtenerPorEmpresaAsync(int idEmpresa, bool soloActivos = true, CancellationToken ct = default);
        Task<(IEnumerable<Producto> Items, int TotalCount)> ObtenerPorEmpresaPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, int? idCategoria = null, bool? soloActivos = null, CancellationToken ct = default);
        Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa, CancellationToken ct = default);
        Task<Producto?> ObtenerPorIdConDetallesAsync(int id, CancellationToken ct = default);
        Task<bool> ExisteCodigoBarraAsync(string codigo, int idEmpresa, CancellationToken ct = default);
        Task<bool> ExisteNombreEnCategoriaAsync(string nombre, int idCategoria, int idEmpresa, CancellationToken ct = default);
        Task<int> ObtenerStockAsync(int idProducto, CancellationToken ct = default);
        Task AgregarRangoMasivoAsync(IEnumerable<Producto> productos, bool disableTracking = true, CancellationToken ct = default);

        // Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion
        Task<List<UnidadMedida>> ObtenerUnidadesMedidaDistintasAsync(CancellationToken ct = default);
        Task<List<Producto>> ObtenerPorCategoriaAsync(int idCategoria, CancellationToken ct = default);
        Task<List<Producto>> ObtenerConCodigoBarraPorEmpresaAsync(int idEmpresa, CancellationToken ct = default);
        Task<int> ContarProductosConStockBajoAsync(int idEmpresa, CancellationToken ct = default);
        Task<List<Producto>> ObtenerConStockBajoConLimiteAsync(int idEmpresa, int limite, CancellationToken ct = default);
        
        // Búsqueda con StartsWith (prefijo) para uso de índices - reemplaza Contains/LIKE '%term%'
        Task<List<Producto>> BuscarProductosAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);
    }
}