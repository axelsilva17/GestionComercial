using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Auditoria
{
    public class AuditoriaLog
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int? UsuarioId { get; set; }
        public OperacionAuditoriaEnum TipoOperacion { get; set; }
        public string Tabla { get; set; } = string.Empty;
        public int RegistroId { get; set; }
        public string? Detalles { get; set; }

        // Navegación
        public Usuario? Usuario { get; set; }
    }
}
