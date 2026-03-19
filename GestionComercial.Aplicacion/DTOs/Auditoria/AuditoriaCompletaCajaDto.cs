using System;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    /// <summary>
    /// DTO que combina auditoría de cajas y movimientos de caja
    /// para el popup de auditoría completa de caja.
    /// </summary>
    public class AuditoriaCompletaCajaDto
    {
        /// <summary>
        /// Registros de auditoría de la tabla Cajas.
        /// </summary>
        public List<AuditoriaLogDto> AuditoriaCajas { get; set; } = new();

        /// <summary>
        /// Registros de auditoría de la tabla MovimientoCaja.
        /// </summary>
        public List<AuditoriaLogDto> MovimientosCaja { get; set; } = new();
    }
}
