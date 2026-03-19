namespace GestionComercial.Dominio.Entidades.Ventas
{
    /// <summary>
    /// Representa un descuento aplicado a un ítem específico de venta.
    /// Permite múltiples descuentos por ítem con porcentaje y monto.
    /// </summary>
    public class VentaDetalleDescuento
    {
        public int      Id          { get; set; }
        public int      Id_detalle { get; set; }
        public decimal  Porcentaje  { get; set; }
        public decimal  Monto      { get; set; }
        public string?  Descripcion { get; set; }

        public VentaDetalle Detalle { get; set; } = null!;
    }
}