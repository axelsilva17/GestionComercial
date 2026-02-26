namespace GestionComercial.Aplicacion.DTOs.Productos
{
    public class ProductoCrearDto
    {
        public string  Nombre            { get; set; }
        public string  CodigoBarra       { get; set; }
        public decimal PrecioVentaActual { get; set; }
        public decimal PrecioCostoActual { get; set; }
        public int     StockActual       { get; set; }
        public int     StockMinimo       { get; set; }
        public int     IdEmpresa         { get; set; }
        public int     IdCategoria       { get; set; }
        public int     IdUnidadMedida    { get; set; }
    }
}
