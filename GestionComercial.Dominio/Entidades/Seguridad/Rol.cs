
using GestionComercial.Dominio.Entidades.Organizacion;

namespace GestionComercial.Dominio.Entidades.Seguridad
{

    public class Rol
    {
        public int    Id_rol         { get; set; }
        public string Nombre      { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public static implicit operator Rol(int v)
        {
            throw new NotImplementedException();
        }
    }


}
