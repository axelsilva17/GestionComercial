using GestionComercial.Aplicacion.DTOs.Clientes;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IClienteServicio
    {
        Task<IEnumerable<ClienteDto>> ObtenerTodosAsync(int idEmpresa);
        Task<(IEnumerable<ClienteDto> Items, int TotalCount)> ObtenerTodosPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, bool? soloActivos = null);
        Task<ClienteDto?>             ObtenerPorIdAsync(int id);
        Task<ClienteDto>              CrearAsync(ClienteCrearDto dto);
        Task                          ActualizarAsync(ClienteActualizarDto dto);
        Task                          DesactivarAsync(int id);
        Task                          ActivarAsync(int id);
        Task<int>                     ContarClientesConVentasAsync(int idEmpresa);
    }
}
