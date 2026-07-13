using GestionComercial.Aplicacion.DTOs;
using GestionComercial.Aplicacion.DTOs.Inventario;
using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IInventarioServicio
    {
        ///         /// Obtiene movimientos de stock con filtros y paginación.
        /// La paginación se ejecuta EN SQL, no en memoria.
        Task<PagedResult<MovimientoStockDto>> ObtenerMovimientosAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int pagina,
            int itemsPorPagina,
            int idEmpresa);

        ///         /// Obtiene movimientos de un producto específico.
        Task<IEnumerable<MovimientoStockDto>> ObtenerMovimientosPorProductoAsync(int idProducto);

        ///         /// Exporta movimientos a Excel.
        Task<byte[]> ExportarAExcelAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int idEmpresa);

        ///         /// Obtiene productos con stock crítico (bajo mínimo).
        Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa);

        ///         /// Registra un nuevo movimiento de inventario y actualiza el stock del producto.
        Task RegistrarMovimientoAsync(
            int idProducto,
            string tipoMovimiento,
            decimal cantidad,
            string? observacion,
            int idSucursal,
            int idUsuario,
            bool guardarCambios = true);
    }
}