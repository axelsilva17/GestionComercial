namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaDetalleDto
    {
        public int IdDetalle { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal MargenUnitario { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoBarra { get; set; }
    }
}