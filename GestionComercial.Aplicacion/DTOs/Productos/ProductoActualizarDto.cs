namespace GestionComercial.Aplicacion.DTOs.Productos
{
    public class ProductoActualizarDto
    {
        public int     IdProducto        { get; set; }
        public string  Nombre            { get; set; }
        public string  CodigoBarra       { get; set; }
        public decimal PrecioVentaActual { get; set; }
        public decimal PrecioCostoActual { get; set; }
        public int     StockMinimo       { get; set; }
        public bool    Activo            { get; set; }
        public int     IdCategoria       { get; set; }
        public int     IdUnidadMedia     { get; set; }
    }
}
