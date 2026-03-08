using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly IUnitOfWork _uow;
        public ClienteServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync(int idEmpresa)
        {
            var clientes = await _uow.Clientes.ObtenerPorEmpresaAsync(idEmpresa);
            return clientes.Select(MapearDto);
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _uow.Clientes.ObtenerPorIdAsync(id);
            return c == null ? null : MapearDto(c);
        }

        public async Task<ClienteDto> CrearAsync(ClienteCrearDto dto)
        {
            var cliente = new Cliente
            {
                Nombre     = dto.Nombre,
                Documento  = dto.Documento,
                Telefono   = dto.Telefono,
                Email      = dto.Email,
                Id_empresa = dto.IdEmpresa,
            };
            await _uow.Clientes.AgregarAsync(cliente);
            await _uow.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(cliente.Id) ?? throw new Exception("Error al crear cliente");
        }

        public async Task ActualizarAsync(ClienteActualizarDto dto)
        {
            var cliente = await _uow.Clientes.ObtenerPorIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Cliente {dto.Id} no encontrado");
            cliente.Nombre    = dto.Nombre;
            cliente.Documento = dto.Documento;
            cliente.Telefono  = dto.Telefono;
            cliente.Email     = dto.Email;
            _uow.Clientes.Actualizar(cliente);
            await _uow.GuardarCambiosAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var cliente = await _uow.Clientes.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Cliente {id} no encontrado");
            // La entidad no tiene Activo — soft delete via eliminación o flag adicional
            _uow.Clientes.Eliminar(cliente);
            await _uow.GuardarCambiosAsync();
        }

        private static ClienteDto MapearDto(Cliente c) => new()
        {
            IdCliente = c.Id,
            Nombre    = c.Nombre,
            Documento = c.Documento,
            Telefono  = uint.TryParse(c.Telefono, out var tel) ? tel : 0,
            Email     = c.Email ?? string.Empty,
            IdEmpresa = c.Id_empresa,
        };
    }
}
