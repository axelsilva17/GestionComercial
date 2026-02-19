namespace GestionComercial.Aplicacion.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int    IdUsuario    { get; set; }
        public string Nombre       { get; set; }
        public string Apellido     { get; set; }
        public bool   Activo       { get; set; }
        public int    IdSucursal   { get; set; }
        public string SucursalNombre { get; set; }
        public string RolNombre    { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Inicial => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
