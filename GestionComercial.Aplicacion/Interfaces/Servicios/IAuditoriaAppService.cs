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

        /// <summary>
        /// Calcula los KPIs de fraude para el período especificado.
        /// Incluye: caja con mayor diferencia, ventas anuladas, movimientos fuera de horario,
        /// y forma de pago por vendedor.
        /// </summary>
        /// <param name="fechaDesde">Fecha inicial del rango.</param>
        /// <param name="fechaHasta">Fecha final del rango.</param>
        /// <returns>DTO con los KPIs de fraude calculados.</returns>
        Task<KpiFraudeDto> CalcularKpisFraudeAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta);

        /// <summary>
        /// Obtiene auditoría filtrada por usuario, tipo de operación y rango de fechas.
        /// Aplica paginación para optimizar la consulta.
        /// </summary>
        /// <param name="filtros">Criterios de filtrado.</param>
        /// <returns>Resultado paginado con registros de auditoría.</returns>
        Task<AuditoriaFiltradaDto> ObtenerAuditoriaFiltradaAsync(
            FiltroAuditoriaDto filtros);

        /// <summary>
        /// Obtiene auditoría de caja con valores JSON deserializados.
        /// Para mostrar cambios de forma legible en la UI.
        /// </summary>
        Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaCajaDeserializadaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int? idUsuario = null,
            string? tipoOperacion = null);
    }
}
