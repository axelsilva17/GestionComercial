using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;

namespace GestionComercial.Persistencia.Repositorio
{
    public class PermisoRepositorio : RepositorioBase<Permiso>, IPermisoRepositorio
    {
        public PermisoRepositorio(GestionComercialContext context) : base(context) { }
    }
}
