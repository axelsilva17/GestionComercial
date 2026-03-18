using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Dominio.Entidades.Auditoria
{
    /// <summary>
    /// Catálogo de tablas que el sistema de auditoría monitorea.
    /// </summary>
    public class TablaAuditada
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la tabla en la base de datos.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NombreTabla { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del propósito de la tabla.
        /// </summary>
        [MaxLength(255)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Indica si la auditoría está habilitada para esta tabla.
        /// </summary>
        public bool Habilitada { get; set; } = true;

        /// <summary>
        /// Lista de campos que se excluyen de la auditoría (separados por coma).
        /// </summary>
        [MaxLength(500)]
        public string? CamposExcluidos { get; set; }

        /// <summary>
        /// Fecha de creación del registro.
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
