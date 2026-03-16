namespace GestionComercial.Aplicacion.DTOs.Productos
{
    public class StockDto
    {
        public int    IdProducto    { get; set; }
        public string ProductoNombre { get; set; }
        public int    IdSucursal    { get; set; }
        public string SucursalNombre { get; set; }
        public int    StockActual   { get; set; }
        public int    StockMinimo   { get; set; }
        public bool   StockBajo     => StockActual <= StockMinimo;
        public bool   SinStock      => StockActual == 0;
    }
}
