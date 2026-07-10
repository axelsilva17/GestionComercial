namespace GestionComercial.Dominio.Enumeraciones
{
    ///     /// Enum para métodos de pago default del sistema.
    /// Los IDs corresponden a los valores sembrados en la DB inicial.
    public enum MetodoPagoEnum
    {
        Efectivo = 1,
        Tarjeta = 2,
        Transferencia = 3,
        Otro = 4  // Cualquier método adicional
    }
}