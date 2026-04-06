using System;
using System.Security.Principal;
using GestionComercial.Aplicacion.Interfaces.Autenticacion;

namespace GestionComercial.Aplicacion.Servicios
{
    // Implementación de ejemplo; en la siguiente iteración conectaremos con el servicio de autenticación real.
    public class AuthService : IAuthService
    {
        public bool IsCurrentUserAdmin()
        {
            // Desarrollo/QA: posibilidad de forzar admin con variable de entorno
            var testFlag = Environment.GetEnvironmentVariable("ON_DEMAND_ADMIN_TEST");
            if (!string.IsNullOrEmpty(testFlag) && testFlag == "1")
            {
                return true;
            }
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
