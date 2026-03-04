using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace GestionComercial.Aplicacion.Servicios
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUnitOfWork _uow;
        public AutenticacionServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<UsuarioSesionDto?> LoginAsync(string email, string password)
        {
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Buscando email: '{email}'");

            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email);

            System.Diagnostics.Debug.WriteLine($"[LOGIN] Usuario encontrado: {usuario?.Email ?? "NULL"}");
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Hash en BD: {usuario?.PasswordHash ?? "NULL"}");

            if (usuario == null)
                return null;

            System.Diagnostics.Debug.WriteLine($"[LOGIN] Password recibido: '{password}'");
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Hash largo: {usuario.PasswordHash?.Length}");
            var testHash = BC.HashPassword(password, 12);
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Hash generado ahora: '{testHash}'");
            bool passwordValido = BC.Verify(password, usuario.PasswordHash);
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Password valido: {passwordValido}");

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
            => BC.HashPassword(password, workFactor: 12);
    }
}