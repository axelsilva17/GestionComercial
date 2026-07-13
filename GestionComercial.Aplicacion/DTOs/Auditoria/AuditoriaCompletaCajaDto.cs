using System;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    ///     /// DTO que combina auditoría de cajas y movimientos de caja
    /// para el popup de auditoría completa de caja.
    public class AuditoriaCompletaCajaDto
    {
        ///         /// Registros de auditoría de la tabla Cajas.
        public List<AuditoriaLogDto> AuditoriaCajas { get; set; } = new();

        ///         /// Registros de auditoría de la tabla MovimientoCaja.
        public List<AuditoriaLogDto> MovimientosCaja { get; set; } = new();
    }
}
