using GestionComercial.Aplicacion.DTOs.Auditoria;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    /// <summary>
    /// Servicio de aplicación para operaciones de auditoría orientadas a la UI.
    /// Combina múltiples fuentes de auditoría en llamadas eficientes.
    /// </summary>
    public interface IAuditoriaAppService
    {
        /// <summary>
        /// Obtiene la auditoría completa de caja (Cajas + MovimientosCaja) en una sola llamada.
        /// Optimizado para el popup de auditoría de caja.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango (inclusive).</param>
        /// <param name="fechaHasta">Fecha final del rango (inclusive).</param>
        /// <returns>DTO con ambas listas de auditoría.</returns>
        Task<AuditoriaCompletaCajaDto> ObtenerAuditoriaCompletaCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta);
    }
}
