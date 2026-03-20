using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IVentaRepostorio : IRepositorioBase<Venta>
    {
        Task<Venta?> ObtenerConDetallesAsync(int idVenta);
        Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal);
        Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente);
        Task<IEnumerable<Venta>> ObtenerConDetallesPorFechaAsync(int idEmpresa, DateTime desde, DateTime hasta);
        Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal);
        
        /// <summary>
        /// Obtiene ventas anuladas en un período para auditoría de fraude.
        /// </summary>
        Task<IEnumerable<Venta>> ObtenerVentasAnuladasAsync(DateTime desde, DateTime hasta);
        
        /// <summary>
        /// Obtiene ventas por período con pagos incluidos para análisis.
        /// </summary>
        Task<IEnumerable<Venta>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta);
    }
}