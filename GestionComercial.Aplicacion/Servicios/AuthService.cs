using System;
using System.Security.Principal;

namespace GestionComercial.Aplicacion.Servicios
{
    // Implementación de ejemplo; en la siguiente iteración conectaremos con el servicio de autenticación real.
    public class AuthService
    {
        public bool IsCurrentUserAdmin()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }
}