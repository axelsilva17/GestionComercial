using GestionComercial.Aplicacion.DTOs.Auditoria;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    /// <summary>
    /// Servicio para consultar el trail de auditoría de caja.
    /// </summary>
    public interface IAuditoriaServicio
    {
        /// <summary>
        /// Obtiene registros de auditoría de la tabla Cajas.
        /// </summary>
        Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta);

        /// <summary>
        /// Obtiene registros de auditoría de la tabla MovimientoCaja.
        /// </summary>
        Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaMovimientoCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta);
    }
}
