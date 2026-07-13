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
    }
}
