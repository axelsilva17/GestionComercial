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
        private readonly SesionServicio? _sesion;

        public UsuarioServicio(IUnitOfWork uow, IPasswordHasher passwordHasher, SesionServicio? sesion = null)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _sesion = sesion;
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
            if (_sesion != null && !_sesion.HasPermission("Usuarios.Gestionar"))
                throw new InvalidOperationException("No tenés permiso para crear usuarios.");

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
            if (_sesion != null && !_sesion.HasPermission("Usuarios.Gestionar"))
                throw new InvalidOperationException("No tenés permiso para cambiar contraseñas.");

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
            if (_sesion != null && !_sesion.HasPermission("Usuarios.Gestionar"))
                throw new InvalidOperationException("No tenés permiso para desactivar usuarios.");

            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario {id} no encontrado");

            usuario.Inactivar();
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        public async Task ReactivarAsync(int id)
        {
            if (_sesion != null && !_sesion.HasPermission("Usuarios.Gestionar"))
                throw new InvalidOperationException("No tenés permiso para reactivar usuarios.");

            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario {id} no encontrado");

            usuario.Activo = true;
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        public async Task ActualizarDatosAsync(int idUsuario, string nombre, string apellido)
        {
            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(idUsuario)
                ?? throw new KeyNotFoundException($"Usuario {idUsuario} no encontrado");

            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        public async Task EliminarAsync(int idUsuario)
        {
            if (_sesion != null && !_sesion.HasPermission("Usuarios.Gestionar"))
                throw new InvalidOperationException("No tenés permiso para eliminar usuarios.");

            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(idUsuario)
                ?? throw new KeyNotFoundException($"Usuario {idUsuario} no encontrado");

            // No permitir eliminar el último Gerente
            if (usuario.Id_rol == 1)
            {
                var adminsCount = await _uow.Usuarios.ContarAsync(u => u.Id_rol == 1 && u.Activo);
                if (adminsCount <= 1)
                    throw new InvalidOperationException("No se puede eliminar el último usuario con rol Gerente.");
            }

            // Delegar permisos al Gerente si el usuario tiene un rol diferente
            if (usuario.Id_rol != 1)
            {
                var rolesConPermisos = await _uow.Roles.ObtenerTodosConPermisosAsync();
                var rolUsuario = rolesConPermisos.FirstOrDefault(r => r.Id == usuario.Id_rol);
                var gerente = rolesConPermisos.FirstOrDefault(r => r.Id == 1);

                if (rolUsuario?.RolPermisos != null && gerente != null)
                {
                    var permisosGerenteSet = new HashSet<int>(
                        gerente.RolPermisos?.Select(rp => rp.Id_permiso) ?? Enumerable.Empty<int>());

                    var permisosAgregados = rolUsuario.RolPermisos
                        .Where(rp => !permisosGerenteSet.Contains(rp.Id_permiso))
                        .Select(rp => rp.Id_permiso)
                        .ToList();

                    if (permisosAgregados.Count > 0)
                    {
                        var nuevosPermisosGerente = permisosGerenteSet.ToList();
                        nuevosPermisosGerente.AddRange(permisosAgregados);
                        await _uow.Roles.ActualizarPermisosRolAsync(1, nuevosPermisosGerente);
                    }
                }
            }

            _uow.Usuarios.Eliminar(usuario);
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
