using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IPagoRepositorio : IRepositorioBase<Pago>
    {
        ///         /// Agrupa los pagos por método de pago en un rango de fechas para una sucursal.
        /// Opcionalmente filtra por una caja específica.
        /// Retorna lista de (NombreMetodo, MontoTotal, Cantidad).
        Task<IEnumerable<(string Metodo, decimal Total, int Cantidad)>> ObtenerTotalesPorMetodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int? idCaja = null, CancellationToken ct = default);
        
        /// <summary>
        /// Totales por método de pago agrupados por mes (año-mes) para una sucursal y período.
        /// Una sola consulta agrupada (evita el N+1 mensual de la exportación).
        /// </summary>
        Task<List<PagoMesMetodoRow>> ObtenerTotalesPorMetodoMensualAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
        
        ///         /// Obtiene todos los pagos en un período para análisis de auditoría.
        Task<IEnumerable<Pago>> ObtenerPagosPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Totales por método de pago agrupados por caja para TODAS las cajas activas de la
        /// sucursal abiertas en el período. Reemplaza el N+1 de auditoría: una sola consulta
        /// SQL agregada (ventas del turno de cada caja, Estado = 2).
        /// </summary>
        Task<List<PagoCajaMetodoRow>> ObtenerTotalesPorMetodoPorCajaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
    }

    /// <summary>
    /// Total por método de pago dentro de un mes (AnioMes = año*100 + mes, p.ej. 202401).
    /// </summary>
    public record PagoMesMetodoRow(int AnioMes, string Metodo, decimal Total, int Cantidad);

    /// <summary>
    /// Total y cantidad por método de pago dentro de una caja (para el resumen de auditoría por caja).
    /// </summary>
    public record PagoCajaMetodoRow(int IdCaja, string Metodo, decimal Total, int Cantidad);
}
