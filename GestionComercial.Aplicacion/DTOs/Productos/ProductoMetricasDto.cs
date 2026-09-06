namespace GestionComercial.Aplicacion.DTOs.Productos
{
    public class ProductoMetricasDto
    {
        public int ProductosActivos    { get; set; }
        public int ProductosStockBajo  { get; set; }
        public int ProductosSinStock   { get; set; }
    }
}
