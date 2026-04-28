namespace GestionComercial.Aplicacion.DTOs.Productos
{
    /// <summary>DTO para importar productos desde Excel con soporte de creación y actualización.</summary>
    public class ProductoImportarDto
    {
        public string  Nombre            { get; set; } = string.Empty;
        public string  CodigoBarra       { get; set; } = string.Empty;
        public decimal PrecioVentaActual { get; set; }
        public decimal PrecioCostoActual { get; set; }
        public int     StockActual       { get; set; }
        public int     StockMinimo       { get; set; }
        public string  Categoria         { get; set; } = string.Empty;
        public string  UnidadMedida      { get; set; } = string.Empty;
        public int     IdEmpresa         { get; set; }
        public int     IdCategoria       { get; set; }
        public int     IdUnidadMedida    { get; set; }
        public bool    EsActualizacion    { get; set; }
    }
}