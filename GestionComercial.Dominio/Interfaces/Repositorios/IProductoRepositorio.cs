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

        /// <summary>
        /// Cuenta productos con stock crítico (StockActual <= StockMinimo, Activo) en SQL.
        /// Mismo filtro que ObtenerStockCriticoAsync sin materializar filas. Útil para KPIs.
        /// </summary>
        Task<int> ContarStockCriticoAsync(int idEmpresa, CancellationToken ct = default);
        
        // Búsqueda con StartsWith (prefijo) para uso de índices - reemplaza Contains/LIKE '%term%'
        Task<List<Producto>> BuscarProductosAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);

        // Búsqueda con Contains (subcadena LIKE '%term%') para búsquedas de texto libre
        Task<List<Producto>> BuscarProductosContieneAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);

        // Búsqueda exacta por código de barras (case-insensitive) para escáner
        Task<Producto?> BuscarPorCodigoBarraExactoAsync(int idEmpresa, string codigoBarra, CancellationToken ct = default);

        // Agregación SQL de métricas (activos, stock bajo, sin stock) sin materializar productos.
        Task<(int ProductosActivos, int ProductosStockBajo, int ProductosSinStock)>
            ObtenerMetricasAsync(int idEmpresa, CancellationToken ct = default);
    }
}