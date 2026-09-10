using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Importacion;
using GestionComercial.Dominio.Entidades.Proveedores;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IProductoServicio
    {
        Task<IEnumerable<ProductoListadoDto>> ObtenerTodosAsync(int idEmpresa, bool soloActivos = true, CancellationToken ct = default);
        Task<(IEnumerable<ProductoListadoDto> Items, int TotalCount)> ObtenerTodosPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, int? idCategoria = null, bool? soloActivos = null, CancellationToken ct = default);
        Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa, CancellationToken ct = default);
        Task<ProductoDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<ProductoDto> CrearAsync(ProductoCrearDto dto, CancellationToken ct = default);
        Task ActualizarAsync(ProductoActualizarDto dto, CancellationToken ct = default);
        Task<(ProductoDto Producto, bool FueActualizacion)> CrearOActualizarAsync(ProductoImportarDto dto, bool actualizarExistentes, CancellationToken ct = default);
        // Nuevo: ajuste de precios por proveedor (global por empresa del proveedor)
        Task<(int Nuevos, int Actualizados)> AjustePreciosPorProveedorAsync(int idProveedor, decimal porcentaje, CancellationToken ct = default);
        Task<ImportResult> ImportarMasivoAsync(IEnumerable<ProductoImportarDto> dtos, bool actualizarExistentes, IProgress<(int current, int total, string message)>? progreso = null, CancellationToken ct = default);
        Task DesactivarAsync(int id, CancellationToken ct = default);
        Task ActivarAsync(int id, CancellationToken ct = default);
        Task ActualizarPreciosLoteAsync(IEnumerable<ProductoActualizarDto> dtos, CancellationToken ct = default);

        // Búsqueda con StartsWith (prefijo) para uso de índices
        Task<IEnumerable<ProductoListadoDto>> BuscarProductosAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);

        // Búsqueda con Contains (subcadena) para búsquedas de texto libre
        Task<IEnumerable<ProductoListadoDto>> BuscarProductosContieneAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default);

        // Búsqueda exacta por código de barras (case-insensitive) para escáner
        Task<ProductoListadoDto?> BuscarPorCodigoBarraExactoAsync(int idEmpresa, string codigoBarra, CancellationToken ct = default);

        // Reference data
        Task<IEnumerable<CategoriaItemDto>> ObtenerCategoriasAsync(int idEmpresa, CancellationToken ct = default);
        Task<CategoriaItemDto> CrearCategoriaAsync(int idEmpresa, string nombre, CancellationToken ct = default);
        Task<CategoriaItemDto> ActualizarCategoriaAsync(int idCategoria, string nuevoNombre, CancellationToken ct = default);
        Task<bool> EliminarCategoriaAsync(int idCategoria, CancellationToken ct = default);
        Task<int> EliminarProductosPorCategoriaAsync(int idCategoria, CancellationToken ct = default);
        Task<IEnumerable<UnidadMedidaItemDto>> ObtenerUnidadesMedidaAsync(CancellationToken ct = default);
        Task<IEnumerable<Proveedor>> ObtenerProveedoresAsync(CancellationToken ct = default);

        ///         /// Devuelve el umbral global de stock crítico configurado en la empresa.
        Task<int> ObtenerUmbralStockCriticoAsync(int idEmpresa, CancellationToken ct = default);

        ///         /// Métricas de productos (activos, stock bajo, sin stock) vía consulta SQL agregada.
        Task<ProductoMetricasDto> ObtenerMetricasAsync(int idEmpresa, CancellationToken ct = default);

        // Ajuste masivo de precios: aplica la operación del popup a TODO el set filtrado
        // (empresa + texto + categoría + activo) en una sola UPDATE SQL. Escala a decenas de miles.
        Task<int> AplicarAjusteMasivoAsync(
            int idEmpresa, string? texto, int? idCategoria, bool? soloActivos,
            string tipoAjuste, string direccionAjuste, decimal porcentaje, decimal montoFijo,
            bool aplicarVenta, bool aplicarCosto, CancellationToken ct = default);
    }
}
