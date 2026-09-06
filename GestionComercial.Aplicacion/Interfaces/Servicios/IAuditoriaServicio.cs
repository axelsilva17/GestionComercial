using GestionComercial.Aplicacion.DTOs.Auditoria;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    ///     /// Servicio para consultar el trail de auditoría de caja.
    public interface IAuditoriaServicio
    {
        ///         /// Obtiene registros de auditoría de la tabla Cajas.
        Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta);

        ///         /// Obtiene registros de auditoría de la tabla MovimientoCaja.
        Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaMovimientoCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta);

        /// <summary>
        /// Totales agregados de la auditoría de cajas del período (cantidad y suma de diferencias)
        /// calculados en SQL. Evita materializar las filas que solo se cuentan y suman.
        /// </summary>
        Task<(int TotalRegistros, decimal DiferenciaTotal)> ObtenerAuditoriaCajaTotalesAsync(
            DateTime fechaDesde,
            DateTime fechaHasta);

        /// <summary>
        /// Últimos `take` registros de auditoría de cajas del período (proyección en SQL).
        /// El DTO se devuelve sin deserializar: el consumidor deserializa solo lo que muestra.
        /// </summary>
        Task<List<AuditoriaLogDto>> ObtenerAuditoriaCajaRecienteAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int take);
    }
}
