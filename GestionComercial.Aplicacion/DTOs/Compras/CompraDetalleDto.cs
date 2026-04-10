namespace GestionComercial.Aplicacion.DTOs.Compras
{
    public class CompraDetalleDto
    {
        public int     IdDetalle      { get; set; }
        public int     IdProducto     { get; set; }
        public string  ProductoNombre { get; set; }
        public string  CodigoBarra    { get; set; }
        public string  CategoriaNombre { get; set; }
        public int     Cantidad       { get; set; }
        public decimal PrecioCosto    { get; set; }
        public decimal SubTotal       { get; set; }
        public int     Id_proveedor   { get; set; } // Agregado para filtro
    }
}
