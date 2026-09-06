using GestionComercial.Aplicacion.DTOs.Reportes;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IReporteServicio
    {
        Task<IEnumerable<ReporteVendedorDto>> VentasPorVendedorAsync(int idSucursal, DateTime desde, DateTime hasta);
        Task<IEnumerable<ReporteMargenDto>>   MargenPorProductoAsync(int idEmpresa, DateTime desde, DateTime hasta, int? top = null);
        Task<IEnumerable<ReportesStockDto>>   StockCriticoAsync(int idEmpresa);
        Task<IEnumerable<ReporteRotacionDto>> RotacionProductosAsync(int idEmpresa, DateTime desde, DateTime hasta, int? top = null);
        Task<IEnumerable<ReporteTopProductoDto>> TopProductosAsync(int idSucursal, DateTime desde, DateTime hasta, int top = 20);
        Task<IEnumerable<ReporteMetodosPagoDto>> MetodosPagoUtilizadosAsync(int idSucursal, DateTime desde, DateTime hasta);

        /// <summary>
        /// Totales por método de pago agrupados por mes (una sola consulta SQL agrupada).
        /// Mantiene Cantidad = 0 para preservar el contenido actual del export mensual.
        /// </summary>
        Task<IEnumerable<MetodosPagoMesExportDto>> MetodosPagoMensualAsync(int idSucursal, DateTime desde, DateTime hasta);
        Task<IEnumerable<VentaPorDiaDto>>    VentasPorDiaAsync(int idEmpresa, DateTime desde, DateTime hasta);
        Task<IEnumerable<VentaPorSucursalDto>> VentasPorSucursalAsync(int idEmpresa, DateTime desde, DateTime hasta);
        Task<KpiGeneralDto>                  KpisGeneralesAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta);

        /// <summary>
        /// Base KPI set only (totals, transactions, average ticket) — skips the ranking/stock
        /// extras (MejorVendedor, MejorProducto, ProductosBajoStock) that the entry paths
        /// never display. Same DTO shape as KpisGeneralesAsync.
        /// </summary>
        Task<KpiGeneralDto>                  KpisVentasBaseAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta);

        /// <summary>
        /// Cantidad de clientes distintos con ventas en la sucursal y período.
        /// </summary>
        Task<int>                            ClientesUnicosAsync(int idSucursal, DateTime desde, DateTime hasta);

        /// <summary>
        /// Resumen de ventas por sucursal y período (total, cantidad y promedio) como agregación SQL.
        /// No filtra por Estado (incluye anuladas) para replicar la semántica del reporte admin.
        /// </summary>
        Task<(decimal TotalVentas, int CantidadVentas, decimal PromedioVenta)>
            ResumenVentasPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta);
    }
}
