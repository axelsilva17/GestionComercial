namespace GestionComercial.Dominio.Entidades.Descuento
{
    public class DescuentoMetodoPago
    {
        public int Id_descuentoConfiguracion { get; set; }
        public int Id_metodoPago { get; set; }

        public DescuentoConfiguracion DescuentoConfiguracion { get; set; } = null!;
        public Pagos.MetodoPago MetodoPago { get; set; } = null!;
    }
}