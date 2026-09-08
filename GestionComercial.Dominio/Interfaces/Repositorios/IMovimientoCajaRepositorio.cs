using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IMovimientoCajaRepositorio : IRepositorioBase<TipoMovimientoCaja>
    {
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorCajaAsync(int idCaja, CancellationToken ct = default);
        
        ///         /// Obtiene todos los movimientos de caja en un período para análisis de auditoría.
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Resumen SQL agregado de ingresos (Tipo == 1) y egresos (Tipo != 1) para todas las
        /// cajas de una sucursal abiertas en el período. No materializa el grafo de cajas.
        /// </summary>
        Task<(decimal Ingresos, decimal Egresos)> ObtenerResumenPorSucursalAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Ingresos manuales (Tipo == 1 sin venta) y egresos (Tipo == 2) por caja para TODAS las
        /// cajas activas de la sucursal abiertas en el período. Reemplaza el N+1 de auditoría:
        /// replica la semántica de ObtenerPorCajaAsync sin materializar cada caja.
        /// </summary>
        Task<List<MovimientoCajaResumenPorCajaRow>> ObtenerResumenPorCajaEnPeriodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Proyección ligera para exportar movimientos de caja del período: fecha, tipo, monto,
        /// concepto, usuario y caja, sin materializar el grafo completo (AsNoTracking, ordenado).
        /// </summary>
        Task<List<MovimientoCajaExportRow>> ObtenerMovimientosExportAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
    }

    /// <summary>
    /// Resumen de movimientos manuales de una caja en su turno (para auditoría).
    /// </summary>
    public record MovimientoCajaResumenPorCajaRow(int IdCaja, decimal Ingresos, decimal Egresos);

    /// <summary>
    /// Fila de export de movimientos de caja (proyección ligera, sin grafo de entidades).
    /// </summary>
    public record MovimientoCajaExportRow(
        int Id, DateTime Fecha, int Tipo, decimal Monto, string Concepto, string UsuarioNombre, int CajaId);
}
