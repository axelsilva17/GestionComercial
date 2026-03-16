namespace GestionComercial.Aplicacion.DTOs.Productos
{
    /// <summary>
    /// Alias de ProductoListadoDto — usado en el VM del listado y sidebar.
    /// Mantiene compatibilidad con el ViewModel generado.
    /// </summary>
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

    /// <summary>
    /// DTO liviano para poblar el ComboBox de categorías en filtros y formularios.
    /// </summary>
    public class CategoriaItemDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public int? CategoriaPadre { get; set; }
    }

    /// <summary>
    /// DTO liviano para poblar el ComboBox de unidades de medida en formularios.
    /// </summary>
    public class UnidadMedidaItemDto
    {
        public int IdUnidadMedida { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
    }
}