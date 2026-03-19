namespace GestionComercial.Dominio.Entidades.Ventas
{
    /// <summary>
    /// Representa un impuesto aplicado a un ítem específico de venta.
    /// </summary>
    public class VentaDetalleImpuesto
    {
        public int     Id            { get; set; }
        public int     Id_detalle   { get; set; }
        public int     Id_tipoImpuesto { get; set; }
        public decimal Porcentaje   { get; set; }
        public decimal Monto        { get; set; }

        public VentaDetalle Detalle { get; set; } = null!;
    }
}