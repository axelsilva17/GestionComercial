namespace GestionComercial.Dominio.Entidades.Seguridad
{
    public class RolPermiso : EntidadBase
    {
        public int     Id    { get; set; }
        public int     PermisoId { get; set; }
        public Rol     Rol        { get; set; } = null!;
        public Permiso Permiso    { get; set; } = null!;
    }
}
