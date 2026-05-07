using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;

namespace GestionComercial.Persistencia.Repositorio
{
    public class RolRepositorio : RepositorioBase<Rol>, IRolRepositorio
    {
        public RolRepositorio(GestionComercialContext context) : base(context) { }
    }
}
