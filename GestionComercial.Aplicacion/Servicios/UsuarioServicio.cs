using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using BC = BCrypt.Net.BCrypt;

namespace GestionComercial.Aplicacion.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUnitOfWork _uow;
        public UsuarioServicio(IUnitOfWork uow) => _uow = uow;

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

            // ── Crear usuario usando factory method DDD ────────────────────────
            var passwordHash = BC.HashPassword(password, workFactor: 12);
            var usuario = Usuario.Crear(nombre, apellido, email, passwordHash, idSucursal, idRol);

            await _uow.Usuarios.AgregarAsync(usuario);
            await _uow.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(usuario.Id) ?? throw new Exception("Error al crear usuario");
        }

        public async Task CambiarPasswordAsync(int idUsuario, string passwordActual, string passwordNuevo)
        {
            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(idUsuario)
                ?? throw new KeyNotFoundException($"Usuario {idUsuario} no encontrado");

            if (!BC.Verify(passwordActual, usuario.PasswordHash))
                throw new UnauthorizedAccessException("Contraseña actual incorrecta");

            // ── Usar método de dominio para actualizar password ─────────────────
            var nuevoHash = BC.HashPassword(passwordNuevo, workFactor: 12);
            usuario.ActualizarPassword(nuevoHash);
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario {id} no encontrado");

            // ── Usar método de dominio para inactivar ────────────────────────
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
