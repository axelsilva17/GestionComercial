namespace GestionComercial.Aplicacion.DTOs.Productos
{
    ///     /// Alias de ProductoListadoDto — usado en el VM del listado y sidebar.
    /// Mantiene compatibilidad con el ViewModel generado.
    public class ProductoItemDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string CodigoBarra { get; set; }
        public decimal PrecioVentaActual { get; set; }
        public decimal PrecioCostoActual { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public bool Activo { get; set; }
        public string CategoriaNombre { get; set; }
        public string UnidadMedida { get; set; }
        public string Inicial => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }

    ///     /// DTO liviano para poblar el ComboBox de categorías en filtros y formularios.
    public class CategoriaItemDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public int? CategoriaPadre { get; set; }
    }

    ///     /// DTO liviano para poblar el ComboBox de unidades de medida en formularios.
    public class UnidadMedidaItemDto
    {
        public int IdUnidadMedida { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
    }
}