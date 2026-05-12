namespace GestionComercial.Dominio.Interfaces.Servicios
{
    /// <summary>
    /// Servicio de hashing de contraseñas.
    /// Abstracción para desacoplar la infraestructura (BCrypt, etc.)
    /// de las capas superiores.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Genera un hash seguro de una contraseña en texto plano.
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Hash seguro para almacenar en BD</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con un hash almacenado.
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <param name="hash">Hash almacenado</param>
        /// <returns>True si coincide</returns>
        bool VerifyPassword(string password, string hash);
    }
}
