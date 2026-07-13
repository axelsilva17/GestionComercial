using GestionComercial.Aplicacion.DTOs.Usuarios;
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

            bool passwordValido = _passwordHasher.VerifyPassword(password, usuario.PasswordHash);

            if (!passwordValido)
                return null;

            usuario.UltimoAcceso = DateTime.Now;
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();

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