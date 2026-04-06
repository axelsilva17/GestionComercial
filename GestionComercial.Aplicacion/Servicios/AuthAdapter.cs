using GestionComercial.Aplicacion.Interfaces.Autenticacion;
using GestionComercial.Aplicacion.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    // Adapter bridging old interface and new
    public class AuthAdapter : IAuthService, IAutenticacionServicio
    {
        private readonly AuthService _inner = new AuthService();
        public bool IsCurrentUserAdmin() => _inner.IsCurrentUserAdmin();
    }
}
