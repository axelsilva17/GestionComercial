using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly SesionServicio? _sesion;
        private readonly IValidator<ClienteCrearDto>? _crearValidator;
        private readonly IValidator<ClienteActualizarDto>? _actualizarValidator;

        public ClienteServicio(
            IUnitOfWork uow,
            SesionServicio? sesion = null,
            IValidator<ClienteCrearDto>? crearValidator = null,
            IValidator<ClienteActualizarDto>? actualizarValidator = null)
        {
            _uow = uow;
            _sesion = sesion;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync(int idEmpresa)
        {
            var clientes = await _uow.Clientes.ObtenerPorEmpresaAsync(idEmpresa);
            return clientes.Select(MapearDto);
        }

        public async Task<(IEnumerable<ClienteDto> Items, int TotalCount)> ObtenerTodosPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, bool? soloActivos = null)
        {
            var (items, totalCount) = await _uow.Clientes.ObtenerPorEmpresaPaginadoAsync(idEmpresa, page, pageSize, searchTerm, soloActivos);
            return (items.Select(MapearDto), totalCount);
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _uow.Clientes.ObtenerPorIdAsync(id);
            return c == null ? null : MapearDto(c);
        }

        public async Task<ClienteDto> CrearAsync(ClienteCrearDto dto)
        {
            if (_sesion != null && !_sesion.HasPermission("Clientes.Crear"))
                throw new InvalidOperationException("No tenés permiso para crear clientes.");

            if (_crearValidator != null)
            {
                var result = await _crearValidator.ValidateAsync(dto);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

            var cliente = new Cliente
            {
                Nombre     = dto.Nombre,
                Documento  = dto.Documento,
                Telefono   = dto.Telefono,
                Email      = dto.Email,
                Id_empresa = dto.IdEmpresa,
                Activo     = dto.Activo,
            };
            await _uow.Clientes.AgregarAsync(cliente);
            await _uow.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(cliente.Id) ?? throw new Exception("Error al crear cliente");
        }

        public async Task ActualizarAsync(ClienteActualizarDto dto)
        {
            if (_sesion != null && !_sesion.HasPermission("Clientes.Crear"))
                throw new InvalidOperationException("No tenés permiso para editar clientes.");

            if (_actualizarValidator != null)
            {
                var result = await _actualizarValidator.ValidateAsync(dto);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

            var cliente = await _uow.Clientes.ObtenerPorIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Cliente {dto.Id} no encontrado");
            cliente.Nombre    = dto.Nombre;
            cliente.Documento = dto.Documento;
            cliente.Telefono  = dto.Telefono;
            cliente.Email     = dto.Email;
            cliente.Activo    = dto.Activo;
            _uow.Clientes.Actualizar(cliente);
            await _uow.GuardarCambiosAsync();
        }

        public async Task<int> ContarClientesConVentasAsync(int idEmpresa)
            => await _uow.Clientes.ContarClientesConVentasAsync(idEmpresa);

        public async Task DesactivarAsync(int id)
        {
            if (_sesion != null && !_sesion.HasPermission("Clientes.Crear"))
                throw new InvalidOperationException("No tenés permiso para desactivar clientes.");

            var cliente = await _uow.Clientes.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Cliente {id} no encontrado");
            // Soft delete: marcar como inactivo
            cliente.Activo = false;
            _uow.Clientes.Actualizar(cliente);
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
            Activo    = c.Activo,
            TotalVentas = c.CantidadCompras,
        };
    }
}
