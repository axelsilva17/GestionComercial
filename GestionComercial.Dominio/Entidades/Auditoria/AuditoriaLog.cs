using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Dominio.Entidades.Auditoria
{
    /// <summary>
    /// Representa un registro de auditoría que captura cambios en las entidades del sistema.
    /// </summary>
    public class AuditoriaLog
    {
        // ── Backing fields ──
        private string _nombreTabla = string.Empty;
        private string? _nombreUsuario;
        private string? _valoresAnteriores;
        private string? _valoresNuevos;
        private string? _workstation;

        // ── Propiedades con validación ──
        [Key]
        public int Id { get; set; }  // Para EF Core

        /// <summary>
        /// Nombre de la tabla que fue modificada.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NombreTabla 
        { 
            get => _nombreTabla; 
            set => _nombreTabla = value ?? string.Empty; 
        }

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
        public string? NombreUsuario 
        { 
            get => _nombreUsuario; 
            set => _nombreUsuario = value; 
        }

        /// <summary>
        /// Fecha y hora del cambio.
        /// </summary>
        public DateTime FechaOperacion { get; set; } = DateTime.Now;

        /// <summary>
        /// Estado anterior del registro (serializado como JSON).
        /// </summary>
        public string? ValoresAnteriores 
        { 
            get => _valoresAnteriores; 
            set => _valoresAnteriores = value; 
        }

        /// <summary>
        /// Estado nuevo del registro (serializado como JSON).
        /// </summary>
        public string? ValoresNuevos 
        { 
            get => _valoresNuevos; 
            set => _valoresNuevos = value; 
        }

        /// <summary>
        /// IP o identificación de la workstation desde donde se hizo el cambio.
        /// </summary>
        [MaxLength(50)]
        public string? Workstation 
        { 
            get => _workstation; 
            set => _workstation = value; 
        }

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

        // ── Constructor vacío (para EF Core) ──
        public AuditoriaLog() { }

        // ── Factory methods ──

        /// <summary>
        /// Crea un log de inserción.
        /// </summary>
        public static AuditoriaLog Insert(string tabla, int registroId, int? idUsuario, 
            string? nombreUsuario = null, int? idEmpresa = null, int? idSucursal = null,
            string? workstation = null, string? valoresNuevos = null)
        {
            return new AuditoriaLog
            {
                _nombreTabla = tabla.Trim(),
                RegistroId = registroId,
                TipoOperacion = 1, // Insert
                IdUsuario = idUsuario,
                _nombreUsuario = nombreUsuario,
                FechaOperacion = DateTime.Now,
                _valoresAnteriores = null,
                _valoresNuevos = valoresNuevos,
                _workstation = workstation,
                IdEmpresa = idEmpresa,
                IdSucursal = idSucursal
            };
        }

        /// <summary>
        /// Crea un log de actualización.
        /// </summary>
        public static AuditoriaLog Update(string tabla, int registroId, int? idUsuario,
            string? nombreUsuario = null, int? idEmpresa = null, int? idSucursal = null,
            string? workstation = null, string? valoresAnteriores = null, string? valoresNuevos = null)
        {
            return new AuditoriaLog
            {
                _nombreTabla = tabla.Trim(),
                RegistroId = registroId,
                TipoOperacion = 2, // Update
                IdUsuario = idUsuario,
                _nombreUsuario = nombreUsuario,
                FechaOperacion = DateTime.Now,
                _valoresAnteriores = valoresAnteriores,
                _valoresNuevos = valoresNuevos,
                _workstation = workstation,
                IdEmpresa = idEmpresa,
                IdSucursal = idSucursal
            };
        }

        /// <summary>
        /// Crea un log de eliminación.
        /// </summary>
        public static AuditoriaLog Delete(string tabla, int registroId, int? idUsuario,
            string? nombreUsuario = null, int? idEmpresa = null, int? idSucursal = null,
            string? workstation = null, string? valoresAnteriores = null)
        {
            return new AuditoriaLog
            {
                _nombreTabla = tabla.Trim(),
                RegistroId = registroId,
                TipoOperacion = 3, // Delete
                IdUsuario = idUsuario,
                _nombreUsuario = nombreUsuario,
                FechaOperacion = DateTime.Now,
                _valoresAnteriores = valoresAnteriores,
                _valoresNuevos = null,
                _workstation = workstation,
                IdEmpresa = idEmpresa,
                IdSucursal = idSucursal
            };
        }

        // ── Propiedades computed ──
        public bool EsInsert => TipoOperacion == 1;
        public bool EsUpdate => TipoOperacion == 2;
        public bool EsDelete => TipoOperacion == 3;

        public string TipoOperacionDisplay => TipoOperacion switch
        {
            1 => "Insert",
            2 => "Update",
            3 => "Delete",
            _ => "Unknown"
        };
    }
}