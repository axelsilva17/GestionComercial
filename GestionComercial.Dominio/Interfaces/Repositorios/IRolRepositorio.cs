using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IRolRepositorio : IRepositorioBase<Rol>
    {
        Task<IEnumerable<Rol>> ObtenerTodosConPermisosAsync();
        Task ActualizarPermisosRolAsync(int rolId, List<int> permisoIds);
    }
}
