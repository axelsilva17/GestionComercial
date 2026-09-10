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
            var clientes = (await _uow.Clientes.ObtenerPorEmpresaAsync(idEmpresa)).ToList();
            var conteosVentasPagadas = await _uow.Clientes
                .ContarVentasPagadasPorClientesAsync(idEmpresa, clientes.Select(c => c.Id));
            return clientes.Select(c => MapearDto(c, conteosVentasPagadas));
        }

        public async Task<(IEnumerable<ClienteDto> Items, int TotalCount)> ObtenerTodosPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, bool? soloActivos = null)
        {
            var (items, totalCount) = await _uow.Clientes.ObtenerPorEmpresaPaginadoAsync(idEmpresa, page, pageSize, searchTerm, soloActivos);
            var lista = items.ToList();
            var conteosVentasPagadas = await _uow.Clientes
                .ContarVentasPagadasPorClientesAsync(idEmpresa, lista.Select(c => c.Id));
            return (lista.Select(c => MapearDto(c, conteosVentasPagadas)), totalCount);
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _uow.Clientes.ObtenerPorIdAsync(id);
            if (c == null) return null;

            var conteosVentasPagadas = await _uow.Clientes
                .ContarVentasPagadasPorClientesAsync(c.Id_empresa, new[] { c.Id });
            return MapearDto(c, conteosVentasPagadas);
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

        public async Task ActivarAsync(int id)
        {
            if (_sesion != null && !_sesion.HasPermission("Clientes.Crear"))
                throw new InvalidOperationException("No tenés permiso para activar clientes.");

            var cliente = await _uow.Clientes.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Cliente {id} no encontrado");
            cliente.Reactivar();
            _uow.Clientes.Actualizar(cliente);
            await _uow.GuardarCambiosAsync();
        }

        private static ClienteDto MapearDto(Cliente c, IReadOnlyDictionary<int, int> conteosVentasPagadas) => new()
        {
            IdCliente = c.Id,
            Nombre    = c.Nombre,
            Documento = c.Documento,
            Telefono  = uint.TryParse(c.Telefono, out var tel) ? tel : 0,
            Email     = c.Email ?? string.Empty,
            IdEmpresa = c.Id_empresa,
            Activo    = c.Activo,
            TotalVentas = conteosVentasPagadas.GetValueOrDefault(c.Id),
        };
    }
}
