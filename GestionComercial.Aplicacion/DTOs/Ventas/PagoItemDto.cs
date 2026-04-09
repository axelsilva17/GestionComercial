namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    /// <summary>
    /// Representa un método de pago con su monto en una venta.
    /// Una venta puede tener múltiples PagoItemDto (pago mixto).
    /// </summary>
    public class PagoItemDto
    {
        public int     IdMetodoPago  { get; set; }
        public string  NombreMetodo  { get; set; } = string.Empty;
        public bool    EsEfectivo    { get; set; }
        public decimal Monto         { get; set; }
        /// <summary>
        /// Monto del vuelto dado al cliente (solo aplica para pagos en efectivo).
        /// Este valor se registra como egreso en la caja.
        /// </summary>
        public decimal Vuelto { get; set; }
    }
}
