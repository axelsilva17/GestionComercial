using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    // Adapter for App.xaml.cs compatibility (uses IAutenticacionServicio)
    public class AuthAdapter : IAutenticacionServicio
    {
        private readonly AuthService _inner = new AuthService();
        public bool IsCurrentUserAdmin() => _inner.IsCurrentUserAdmin();
        public Task<UsuarioSesionDto?> LoginAsync(string email, string password)
            => throw new NotSupportedException("Use AutenticacionServicio via DI for login");
        public string HashPassword(string password)
            => throw new NotSupportedException("Use AutenticacionServicio via DI for password hashing");
    }
}