using System;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    /// <summary>
    /// DTO para mostrar registros de auditoría en la UI.
    /// </summary>
    public class AuditoriaLogDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la tabla afectada ("Cajas" | "MovimientosCaja").
        /// </summary>
        public string NombreTabla { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del registro afectado.
        /// </summary>
        public int RegistroId { get; set; }

        /// <summary>
        /// Tipo de operación localized ("Creación" | "Modificación" | "Eliminación").
        /// </summary>
        public string TipoOperacion { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario que realizó la operación.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de la operación.
        /// </summary>
        public DateTime FechaOperacion { get; set; }

        /// <summary>
        /// Estado anterior del registro (JSON serializado).
        /// </summary>
        public string? ValoresAnteriores { get; set; }

        /// <summary>
        /// Estado nuevo del registro (JSON serializado).
        /// </summary>
        public string? ValoresNuevos { get; set; }
    }
}
