using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly IUnitOfWork _uow;
        public ClienteServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync(int idEmpresa)
        {
            var clientes = await _uow.Clientes.ObtenerPorEmpresaAsync(idEmpresa);
            return clientes.Select(c => new ClienteDto
            {
                IdCliente  = c.Id,
                Nombre     = c.Nombre,
                Apellido   = c.Apellido,
                Documento  = c.Documento,
                Telefono   = c.Telefono,
                Email      = c.Email,
                Activo     = c.Activo,
            });
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _uow.Clientes.ObtenerPorIdAsync(id);
            if (c == null) return null;
            return new ClienteDto
            {
                IdCliente = c.Id,
                Nombre    = c.Nombre,
                Apellido  = c.Apellido,
                Documento = c.Documento,
                Telefono  = c.Telefono,
                Email     = c.Email,
                Activo    = c.Activo,
            };
        }

        public async Task<ClienteDto> CrearAsync(ClienteCrearDto dto)
        {
            var cliente = new Clientes
            {
                Nombre    = dto.Nombre,
                Apellido  = dto.Apellido,
                Documento = dto.Documento,
                Telefono  = dto.Telefono,
                Email     = dto.Email,
                Activo    = true,
                Id_empresa = dto.IdEmpresa,
            };
            await _uow.Clientes.AgregarAsync(cliente);
            await _uow.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(cliente.Id) ?? throw new Exception("Error al crear cliente");
        }

        public async Task ActualizarAsync(ClienteActualizarDto dto)
        {
            var cliente = await _uow.Clientes.ObtenerPorIdAsync(dto.IdCliente)
                ?? throw new KeyNotFoundException($"Cliente {dto.IdCliente} no encontrado");
            cliente.Nombre   = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Telefono = dto.Telefono;
            cliente.Email    = dto.Email;
            _uow.Clientes.Actualizar(cliente);
            await _uow.GuardarCambiosAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var cliente = await _uow.Clientes.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Cliente {id} no encontrado");
            cliente.Activo = false;
            _uow.Clientes.Actualizar(cliente);
            await _uow.GuardarCambiosAsync();
        }
    }
}
