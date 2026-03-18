using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Dominio.Entidades.Auditoria
{
    /// <summary>
    /// Representa un registro de auditoría que captura cambios en las entidades del sistema.
    /// </summary>
    public class AuditoriaLog
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la tabla que fue modificada.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NombreTabla { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la fila afectada.
        /// </summary>
        public int RegistroId { get; set; }

        /// <summary>
        /// Tipo de operación: Insert, Update, Delete.
        /// </summary>
        public int TipoOperacion { get; set; }

        /// <summary>
        /// Nombre del usuario que realizó el cambio.
        /// </summary>
        public int? IdUsuario { get; set; }

        /// <summary>
        /// Nombre de usuario (opcional, para consultas rápidas sin join).
        /// </summary>
        [MaxLength(100)]
        public string? NombreUsuario { get; set; }

        /// <summary>
        /// Fecha y hora del cambio.
        /// </summary>
        public DateTime FechaOperacion { get; set; }

        /// <summary>
        /// Estado anterior del registro (serializado como JSON).
        /// </summary>
        public string? ValoresAnteriores { get; set; }

        /// <summary>
        /// Estado nuevo del registro (serializado como JSON).
        /// </summary>
        public string? ValoresNuevos { get; set; }

        /// <summary>
        /// IP o identificación de la workstation desde donde se hizo el cambio.
        /// </summary>
        [MaxLength(50)]
        public string? Workstation { get; set; }

        /// <summary>
        /// Identificador de la empresa (para multi-tenant).
        /// </summary>
        public int? IdEmpresa { get; set; }

        /// <summary>
        /// Identificador de la sucursal donde se hizo el cambio.
        /// </summary>
        public int? IdSucursal { get; set; }

        // Navegación
        [ForeignKey(nameof(IdUsuario))]
        public virtual Seguridad.Usuario? Usuario { get; set; }

        [ForeignKey(nameof(IdEmpresa))]
        public virtual Organizacion.Empresa? Empresa { get; set; }

        [ForeignKey(nameof(IdSucursal))]
        public virtual Organizacion.Sucursal? Sucursal { get; set; }
    }
}
