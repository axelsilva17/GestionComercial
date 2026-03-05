namespace GestionComercial.Dominio.Entidades.Seguridad
{
    public class RolPermiso : EntidadBase
    {
        public int     Id_rol     { get; set; }
        public int     Id_permiso { get; set; }
        public Rol     Rol        { get; set; } = null!;
        public Permiso Permiso    { get; set; } = null!;
    }
}
