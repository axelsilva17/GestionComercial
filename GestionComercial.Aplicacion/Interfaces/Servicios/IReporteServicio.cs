using GestionComercial.Aplicacion.DTOs.Reportes;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IReporteServicio
    {
        Task<IEnumerable<ReporteVendedorDto>> VentasPorVendedorAsync(int idSucursal, DateTime desde, DateTime hasta);
        Task<IEnumerable<ReporteMargenDto>>   MargenPorProductoAsync(int idEmpresa, DateTime desde, DateTime hasta);
        Task<IEnumerable<ReportesStockDto>>   StockCriticoAsync(int idEmpresa);
        Task<IEnumerable<ReporteRotacionDto>> RotacionProductosAsync(int idEmpresa, DateTime desde, DateTime hasta);
        Task<IEnumerable<ReporteTopProductoDto>> TopProductosAsync(int idSucursal, DateTime desde, DateTime hasta, int top = 20);
        Task<IEnumerable<ReporteMetodosPagoDto>> MetodosPagoUtilizadosAsync(int idSucursal, DateTime desde, DateTime hasta);
    }
}
