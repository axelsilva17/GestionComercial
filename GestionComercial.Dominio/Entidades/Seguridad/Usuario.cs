using GestionComercial.Dominio.Entidades.Organizacion;

namespace GestionComercial.Dominio.Entidades.Seguridad
{


    public class Usuario : EntidadBase
    {
        public string    Nombre        { get; set; } = string.Empty;
        public string    Apellido      { get; set; } = string.Empty;
        public string    Email         { get; set; } = string.Empty;
        public string    PasswordHash  { get; set; } = string.Empty;
        public DateTime? UltimoAcceso  { get; set; }
        public int       Id_sucursal   { get; set; }
        public int       Id_rol        { get; set; }

        public Sucursal Sucursal { get; set; } = null!;
        public Rol      Rol      { get; set; } = null!;

        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Inicial        => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
