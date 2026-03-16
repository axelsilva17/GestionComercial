namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    public class ReportesStockDto
    {
        public int    IdProducto     { get; set; }
        public string ProductoNombre { get; set; }
        public string Categoria      { get; set; }
        public string Sucursal       { get; set; }
        public int    StockActual    { get; set; }
        public int    StockMinimo    { get; set; }
        public bool   StockBajo      => StockActual <= StockMinimo;
        public bool   SinStock       => StockActual == 0;
        public string Estado         => SinStock ? "Sin Stock" : StockBajo ? "Stock Bajo" : "Normal";
    }
}
