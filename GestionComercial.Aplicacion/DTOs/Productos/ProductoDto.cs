namespace GestionComercial.Aplicacion.DTOs.Productos
{
    public class ProductoDto
    {
        public int     IdProducto       { get; set; }
        public string  Nombre           { get; set; }
        public string  CodigoBarra      { get; set; }
        public decimal PrecioVentaActual { get; set; }
        public decimal PrecioCostoActual { get; set; }
        public int     StockActual      { get; set; }
        public int     StockMinimo      { get; set; }
        public bool    Activo           { get; set; }
        public int     IdEmpresa        { get; set; }
        public int     IdCategoria      { get; set; }
        public string  CategoriaNombre  { get; set; }
        public int     IdUnidadMedia    { get; set; }
        public string  UnidadMedida     { get; set; }
        public string  Inicial => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
