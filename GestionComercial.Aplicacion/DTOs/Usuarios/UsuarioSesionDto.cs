namespace GestionComercial.Aplicacion.DTOs.Usuarios
{
    public class UsuarioSesionDto
    {
        public int    IdUsuario   { get; set; }
        public string Nombre      { get; set; } = string.Empty;
        public string Apellido    { get; set; } = string.Empty;
        public string Email       { get; set; } = string.Empty;
        public string Rol         { get; set; } = string.Empty;
        public int    IdSucursal  { get; set; }
        public string Sucursal    { get; set; } = string.Empty;
        public int    IdEmpresa   { get; set; }
        public string Empresa     { get; set; } = string.Empty;
       
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Inicial        => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
