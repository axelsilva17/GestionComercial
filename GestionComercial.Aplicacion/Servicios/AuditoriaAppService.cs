using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación de IAuditoriaAppService.
    /// Delega la lógica de negocio a IAuditoriaServicio y combina los resultados
    /// para optimizar la llamada desde el popup de auditoría de caja.
    /// </summary>
    public class AuditoriaAppService : IAuditoriaAppService
    {
        private readonly IAuditoriaServicio _auditoriaServicio;

        public AuditoriaAppService(IAuditoriaServicio auditoriaServicio)
        {
            _auditoriaServicio = auditoriaServicio 
                ?? throw new ArgumentNullException(nameof(auditoriaServicio));
        }

        public async Task<AuditoriaCompletaCajaDto> ObtenerAuditoriaCompletaCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            // Ejecutar ambas consultas en paralelo para máxima eficiencia
            var tareaAuditoriaCajas = _auditoriaServicio.ObtenerAuditoriaCajaAsync(fechaDesde, fechaHasta);
            var tareaMovimientosCaja = _auditoriaServicio.ObtenerAuditoriaMovimientoCajaAsync(fechaDesde, fechaHasta);

            await Task.WhenAll(tareaAuditoriaCajas, tareaMovimientosCaja);

            return new AuditoriaCompletaCajaDto
            {
                AuditoriaCajas = tareaAuditoriaCajas.Result.ToList(),
                MovimientosCaja = tareaMovimientosCaja.Result.ToList()
            };
        }
    }
}
