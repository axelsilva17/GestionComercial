using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Dominio.Entidades.Auditoria
{
    ///     /// Representa un registro de auditoría que captura cambios en las entidades del sistema.
    public class AuditoriaLog
    {
        private string _nombreTabla = string.Empty;
        private string? _nombreUsuario;
        private string? _valoresAnteriores;
        private string? _valoresNuevos;
        private string? _workstation;

        [Key]
        public int Id { get; set; }  // Para EF Core

        ///         /// Nombre de la tabla que fue modificada.
        [Required]
        [MaxLength(100)]
        public string NombreTabla 
        { 
            get => _nombreTabla; 
            set => _nombreTabla = value ?? string.Empty; 
        }

        ///         /// Identificador de la fila afectada.
        public int RegistroId { get; set; }

        ///         /// Tipo de operación: Insert, Update, Delete.
        public int TipoOperacion { get; set; }

        ///         /// Nombre del usuario que realizó el cambio.
        public int? IdUsuario { get; set; }

        ///         /// Nombre de usuario (opcional, para consultas rápidas sin join).
        [MaxLength(100)]
        public string? NombreUsuario 
        { 
            get => _nombreUsuario; 
            set => _nombreUsuario = value; 
        }

        ///         /// Fecha y hora del cambio.
        public DateTime FechaOperacion { get; set; } = DateTime.Now;

        ///         /// Estado anterior del registro (serializado como JSON).
        public string? ValoresAnteriores 
        { 
            get => _valoresAnteriores; 
            set => _valoresAnteriores = value; 
        }

        ///         /// Estado nuevo del registro (serializado como JSON).
        public string? ValoresNuevos 
        { 
            get => _valoresNuevos; 
            set => _valoresNuevos = value; 
        }

        ///         /// IP o identificación de la workstation desde donde se hizo el cambio.
        [MaxLength(50)]
        public string? Workstation 
        { 
            get => _workstation; 
            set => _workstation = value; 
        }

        ///         /// Identificador de la empresa (para multi-tenant).
        public int? IdEmpresa { get; set; }

        ///         /// Identificador de la sucursal donde se hizo el cambio.
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

        ///         /// Crea un log de inserción.
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

        ///         /// Crea un log de actualización.
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

        ///         /// Crea un log de eliminación.
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