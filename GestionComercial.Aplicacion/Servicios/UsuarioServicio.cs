using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioServicio(IUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync(int idSucursal)
        {
            var usuarios = await _uow.Usuarios.ObtenerPorSucursalAsync(idSucursal);
            return usuarios.Select(MapearDto);
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
        {
            var u = await _uow.Usuarios.ObtenerPorIdAsync(id);
            return u == null ? null : MapearDto(u);
        }

        public async Task<UsuarioDto> CrearAsync(string nombre, string apellido, string email, string password, int idRol, int idSucursal)
        {
            if (await _uow.Usuarios.ExisteAsync(u => u.Email == email))
                throw new InvalidOperationException($"El email {email} ya está en uso");

            var passwordHash = _passwordHasher.HashPassword(password);
            var usuario = Usuario.Crear(nombre, apellido, email, passwordHash, idSucursal, idRol);

            await _uow.Usuarios.AgregarAsync(usuario);
            await _uow.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(usuario.Id) ?? throw new Exception("Error al crear usuario");
        }

        public async Task CambiarPasswordAsync(int idUsuario, string passwordActual, string passwordNuevo)
        {
            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(idUsuario)
                ?? throw new KeyNotFoundException($"Usuario {idUsuario} no encontrado");

            if (!_passwordHasher.VerifyPassword(passwordActual, usuario.PasswordHash))
                throw new UnauthorizedAccessException("Contraseña actual incorrecta");

            var nuevoHash = _passwordHasher.HashPassword(passwordNuevo);
            usuario.ActualizarPassword(nuevoHash);
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario {id} no encontrado");

            usuario.Inactivar();
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        private static UsuarioDto MapearDto(Usuario u) => new()
        {
            IdUsuario = u.Id,
            Nombre    = u.Nombre,
            Apellido  = u.Apellido,
            Email     = u.Email,
            Activo    = u.Activo,
            Rol       = u.Rol?.Nombre ?? string.Empty,
        };
    }
}
