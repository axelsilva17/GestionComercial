using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public AutenticacionServicio(IUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioSesionDto?> LoginAsync(string email, string password)
        {
            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email);

            if (usuario == null)
                return null;

            if (usuario.EstaBloqueado)
            {
                var restante = (int)(usuario.BloqueadoHasta!.Value - DateTime.Now).TotalMinutes + 1;
                throw new NegocioException($"Usuario bloqueado temporalmente por intentos fallidos. Intentá de nuevo en {restante} minutos.");
            }

            if (!usuario.PuedeAcceder)
                return null;

            bool passwordValido = _passwordHasher.VerifyPassword(password, usuario.PasswordHash);

            if (!passwordValido)
            {
                usuario.RegistrarAccesoFallido();
                _uow.Usuarios.Actualizar(usuario);
                await _uow.GuardarCambiosAsync();
                throw new NegocioException("Email o contraseña incorrectos.");
            }

            usuario.RegistrarAccesoExitoso();
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();

            var permisos = await _uow.Usuarios.ObtenerPermisosAsync(usuario.Id);

            return new UsuarioSesionDto
            {
                IdUsuario = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = usuario.Rol?.Nombre ?? string.Empty,
                IdSucursal = usuario.Id_sucursal,
                Sucursal = usuario.Sucursal?.Nombre ?? string.Empty,
                IdEmpresa = usuario.Sucursal?.Id_empresa ?? 0,
                Empresa = usuario.Sucursal?.Empresa?.Nombre ?? string.Empty,
                Permisos = new HashSet<string>(permisos),
            };
        }

        public string HashPassword(string password)
            => _passwordHasher.HashPassword(password);

        public bool IsCurrentUserAdmin()
        {
            var authService = new AuthService();
            return authService.IsCurrentUserAdmin();
        }
    }
}