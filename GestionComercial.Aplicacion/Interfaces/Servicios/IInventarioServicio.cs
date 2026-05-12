using GestionComercial.Aplicacion.DTOs.Inventario;
using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IInventarioServicio
    {
        /// <summary>
        /// Obtiene movimientos de stock con filtros y paginación.
        /// </summary>
        Task<(IEnumerable<MovimientoStockDto> Movimientos, int Total)> ObtenerMovimientosAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int pagina,
            int itemsPorPagina,
            int idEmpresa);

        /// <summary>
        /// Obtiene movimientos de un producto específico.
        /// </summary>
        Task<IEnumerable<MovimientoStockDto>> ObtenerMovimientosPorProductoAsync(int idProducto);

        /// <summary>
        /// Exporta movimientos a Excel.
        /// </summary>
        Task<byte[]> ExportarAExcelAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int idEmpresa);

        /// <summary>
        /// Obtiene productos con stock crítico (bajo mínimo).
        /// </summary>
        Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa);

        /// <summary>
        /// Registra un nuevo movimiento de inventario y actualiza el stock del producto.
        /// </summary>
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