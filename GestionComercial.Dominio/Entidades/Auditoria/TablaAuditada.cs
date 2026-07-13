using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Dominio.Entidades.Auditoria
{
    ///     /// Catálogo de tablas que el sistema de auditoría monitorea.
    public class TablaAuditada
    {
        [Key]
        public int Id { get; set; }

        ///         /// Nombre de la tabla en la base de datos.
        [Required]
        [MaxLength(100)]
        public string NombreTabla { get; set; } = string.Empty;

        ///         /// Descripción del propósito de la tabla.
        [MaxLength(255)]
        public string? Descripcion { get; set; }

        ///         /// Indica si la auditoría está habilitada para esta tabla.
        public bool Habilitada { get; set; } = true;

        ///         /// Lista de campos que se excluyen de la auditoría (separados por coma).
        [MaxLength(500)]
        public string? CamposExcluidos { get; set; }

        ///         /// Fecha de creación del registro.
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
