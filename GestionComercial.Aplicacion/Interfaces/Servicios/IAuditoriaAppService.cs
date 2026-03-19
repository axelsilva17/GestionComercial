using GestionComercial.Aplicacion.DTOs.Auditoria;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    /// <summary>
    /// Interface para el servicio de auditoría.
    /// </summary>
    public interface IAuditoriaAppService
    {
        /// <summary>
        /// Obtiene la auditoría completa de caja (aperturas, cierres y movimientos)
        /// dentro del rango de fechas indicado.
        /// </summary>
        Task<AuditoriaCompletaCajaDto> ObtenerAuditoriaCompletaCajaAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken cancellationToken = default);
    }
}
