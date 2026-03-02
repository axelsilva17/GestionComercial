using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces.Repositorios;

public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObtenerPorSucursalAsync(int idSucursal);
}
