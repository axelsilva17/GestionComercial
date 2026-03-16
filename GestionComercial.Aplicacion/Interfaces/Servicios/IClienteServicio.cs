using GestionComercial.Aplicacion.DTOs.Clientes;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IClienteServicio
    {
        Task<IEnumerable<ClienteDto>> ObtenerTodosAsync(int idEmpresa);
        Task<ClienteDto?>             ObtenerPorIdAsync(int id);
        Task<ClienteDto>              CrearAsync(ClienteCrearDto dto);
        Task                          ActualizarAsync(ClienteActualizarDto dto);
        Task                          DesactivarAsync(int id);
    }
}
