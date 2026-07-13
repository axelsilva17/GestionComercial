namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    ///     /// Representa un método de pago con su monto en una venta.
    /// Una venta puede tener múltiples PagoItemDto (pago mixto).
    public class PagoItemDto
    {
        public int     IdMetodoPago  { get; set; }
        public string  NombreMetodo  { get; set; } = string.Empty;
        public string  Categoria     { get; set; } = "Otro";
        public decimal Monto         { get; set; }
        ///         /// Monto del vuelto dado al cliente (solo aplica para pagos en efectivo).
        /// Este valor se registra como egreso en la caja.
        public decimal Vuelto { get; set; }
    }
}
