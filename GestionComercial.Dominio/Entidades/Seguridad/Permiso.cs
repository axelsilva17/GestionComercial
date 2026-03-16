namespace GestionComercial.Dominio.Entidades.Seguridad
{
    public class Permiso : EntidadBase
    {
        public string  Nombre      { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}
