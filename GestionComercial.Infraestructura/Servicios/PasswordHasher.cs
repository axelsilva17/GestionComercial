using GestionComercial.Dominio.Interfaces.Servicios;
using BC = BCrypt.Net.BCrypt;

namespace GestionComercial.Infraestructura.Servicios
{
    ///     /// Implementación de IPasswordHasher usando BCrypt.Net-Next.
    /// Factor de trabajo (workFactor) = 12 para un balance entre seguridad y rendimiento.
    public class PasswordHasher : IPasswordHasher
    {
        // WorkFactor = 12 → ~250ms por hash en hardware moderno
        // Aumentar a 13/14 si se necesita mayor seguridad (más lento)
        private const int WorkFactor = 12;

        public string HashPassword(string password)
        {
            return BC.HashPassword(password, workFactor: WorkFactor);
        }

        public bool VerifyPassword(string password, string hash)
        {
            // Manejar caso de hash nulo/vacío por seguridad
            if (string.IsNullOrEmpty(hash))
                return false;

            return BC.Verify(password, hash);
        }
    }
}
