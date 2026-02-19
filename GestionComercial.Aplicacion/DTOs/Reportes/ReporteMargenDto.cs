namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    public class ReporteMargenDto
    {
        public int     IdProducto     { get; set; }
        public string  ProductoNombre { get; set; }
        public string  Categoria      { get; set; }
        public decimal PrecioVenta    { get; set; }
        public decimal PrecioCosto    { get; set; }
        public decimal MargenUnitario { get; set; }
        public decimal MargenPorcentaje { get; set; }
        public int     CantidadVendida { get; set; }
        public decimal MargenTotal    { get; set; }
    }
}
