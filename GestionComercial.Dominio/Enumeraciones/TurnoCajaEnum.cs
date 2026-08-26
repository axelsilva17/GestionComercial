namespace GestionComercial.Dominio.Enumeraciones
{
    /// <summary>
    /// Turnos disponibles para una caja.
    /// Los valores se almacenan como string en la DB para compatibilidad.
    /// </summary>
    public enum TurnoCajaEnum
    {
        Manana = 1,
        Tarde = 2,
        Noche = 3
    }

    /// <summary>
    /// Extensiones para convertir entre TurnoCajaEnum y string (DB).
    /// </summary>
    public static class TurnoCajaEnumExtensions
    {
        public static string ToDisplayString(this TurnoCajaEnum turno) => turno switch
        {
            TurnoCajaEnum.Manana => "Mañana",
            TurnoCajaEnum.Tarde  => "Tarde",
            TurnoCajaEnum.Noche  => "Noche",
            _                    => turno.ToString()
        };

        public static string? ToDisplayStringOrNull(this TurnoCajaEnum? turno)
            => turno?.ToDisplayString();

        /// <summary>
        /// Convierte un string de la DB a TurnoCajaEnum (case-insensitive).
        /// Retorna null si no coincide con ningún valor conocido.
        /// </summary>
        public static TurnoCajaEnum? FromString(string? value)
            => (value?.Trim().ToLowerInvariant()) switch
            {
                "mañana" or "manana" => TurnoCajaEnum.Manana,
                "tarde"              => TurnoCajaEnum.Tarde,
                "noche"              => TurnoCajaEnum.Noche,
                _                    => null
            };
    }
}
