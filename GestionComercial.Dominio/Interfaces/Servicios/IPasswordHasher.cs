namespace GestionComercial.Dominio.Interfaces.Servicios
{
    ///     /// Servicio de hashing de contraseñas.
    /// Abstracción para desacoplar la infraestructura (BCrypt, etc.)
    /// de las capas superiores.
    public interface IPasswordHasher
    {
        ///         /// Genera un hash seguro de una contraseña en texto plano.
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Hash seguro para almacenar en BD</returns>
        string HashPassword(string password);

        ///         /// Verifica si una contraseña en texto plano coincide con un hash almacenado.
        /// <param name="password">Contraseña en texto plano</param>
        /// <param name="hash">Hash almacenado</param>
        /// <returns>True si coincide</returns>
        bool VerifyPassword(string password, string hash);
    }
}
