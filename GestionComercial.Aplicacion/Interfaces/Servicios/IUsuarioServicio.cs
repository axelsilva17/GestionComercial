using GestionComercial.Aplicacion.DTOs.Usuarios;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IUsuarioServicio
    {
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync(int idSucursal);
        Task<UsuarioDto?>             ObtenerPorIdAsync(int id);
        Task<UsuarioDto>              CrearAsync(string nombre, string apellido, string email, string password, int idRol, int idSucursal);
        Task                          CambiarPasswordAsync(int idUsuario, string passwordActual, string passwordNuevo);
        Task                          DesactivarAsync(int id);
    }
}
