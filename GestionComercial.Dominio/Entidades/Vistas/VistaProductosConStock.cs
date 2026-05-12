namespace GestionComercial.Dominio.Entidades.Vistas
{
    /// <summary>
    /// Entidad de solo lectura para la vista VistaProductosConStock.
    /// Mapea a: CREATE VIEW VistaProductosConStock AS ...
    /// </summary>
    public class VistaProductosConStock
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? CodigoBarra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Stock { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? UnidadMedidaNombre { get; set; }
        public bool Activo { get; set; }
    }
}