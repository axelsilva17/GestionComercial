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
        /// <summary>
        /// Igual que ObtenerPorEmpresaAsync pero SIN materializar navegaciones
        /// (Categoria/UnidadMedida). Para flujos de UPDATE masivo (ej. ajuste de precios por
        /// proveedor): evita que EF Core intente trackear otra instancia con la misma key →
        /// "cannot be tracked because another instance with the same key value ... is already being tracked".
        /// </summary>
        Task<IEnumerable<Producto>> ObtenerPorEmpresaSinNavegacionesAsync(int idEmpresa, bool soloActivos = true, CancellationToken ct = default);
        Task<(IEnumerable<Producto> Items, int TotalCount)> ObtenerPorEmpresaPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, int? idCategoria = null, bool? soloActivos = null, CancellationToken ct = default);
        Task<IEnumerable<Producto>> ObtenerStockCriticoAsync(int idEmpresa, int? umbral = null, CancellationToken ct = default);
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
        /// Cuenta productos con stock crítico (Activo) en SQL. Si se pasa umbral se compara
        /// contra él (StockActual &lt;= umbral); si no, cae al StockMinimo de cada producto.
        /// Mismo filtro que ObtenerStockCriticoAsync sin materializar filas. Útil para KPIs.
        /// </summary>
        Task<int> ContarStockCriticoAsync(int idEmpresa, int? umbral = null, CancellationToken ct = default);
        
        // Búsqueda con StartsWith (prefijo) para uso de índices - reemplaza Contains/LIKE '%term%'
        Task<List<Producto>> BuscarProductosAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);

        // Búsqueda con Contains (subcadena LIKE '%term%') para búsquedas de texto libre
        Task<List<Producto>> BuscarProductosContieneAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);

        // Búsqueda exacta por código de barras (case-insensitive) para escáner
        Task<Producto?> BuscarPorCodigoBarraExactoAsync(int idEmpresa, string codigoBarra, CancellationToken ct = default);

        // Agregación SQL de métricas (activos, stock bajo, sin stock) sin materializar productos.
        Task<(int ProductosActivos, int ProductosStockBajo, int ProductosSinStock)>
            ObtenerMetricasAsync(int idEmpresa, CancellationToken ct = default);

        // Ajuste masivo de precios: una sola UPDATE SQL con el MISMO filtro que el preview del popup
        // (empresa + texto StartsWith + categoría + activo). factor/delta replican el cálculo del
        // preview (porcentaje → factor 1±pct/100; fijo → delta ±monto). Devuelve filas afectadas.
        Task<int> AplicarAjustePreciosMasivoAsync(
            int idEmpresa, string? texto, int? idCategoria, bool? soloActivos,
            decimal factor, decimal delta, bool esPorcentaje, bool aplicarVenta, bool aplicarCosto,
            CancellationToken ct = default);
    }
}