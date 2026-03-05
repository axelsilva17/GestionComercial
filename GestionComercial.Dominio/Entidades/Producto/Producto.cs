using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Producto
{
    public class UnidadMedida
    {
        public int    Id          { get; set; }
        public string Nombre      { get; set; } = string.Empty;
        public string Abreviatura { get; set; } = string.Empty;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }

    public class Categoria : EntidadBase
    {
        public string  Nombre            { get; set; } = string.Empty;
        public int?    CategoriaPadre_id { get; set; }
        public int     Id_empresa        { get; set; }

        public Categoria?             CategoriaPadre { get; set; }
        public ICollection<Categoria> SubCategorias  { get; set; } = new List<Categoria>();
        public Empresa                Empresa        { get; set; } = null!;
        public ICollection<Producto>  Productos      { get; set; } = new List<Producto>();
    }

    public class Producto : EntidadBase
    {
        public string  Nombre            { get; set; } = string.Empty;
        public string? CodigoBarra       { get; set; }
        public string? Descripcion       { get; set; }
        public decimal PrecioVentaActual { get; set; }
        public decimal PrecioCostoActual { get; set; }
        public decimal StockActual       { get; set; }
        public decimal StockMinimo       { get; set; }
        public int     Id_empresa        { get; set; }
        public int     Id_categoria      { get; set; }
        public int     Id_unidadMedida   { get; set; }

        public Empresa      Empresa      { get; set; } = null!;
        public Categoria    Categoria    { get; set; } = null!;
        public UnidadMedida UnidadMedida { get; set; } = null!;

        public ICollection<VentaDetalle>    VentaDetalles  { get; set; } = new List<VentaDetalle>();
        public ICollection<CompraDetalle>   CompraDetalles { get; set; } = new List<CompraDetalle>();
        public ICollection<MovimientoStock> Movimientos    { get; set; } = new List<MovimientoStock>();

        public bool    StockBajo => StockActual <= StockMinimo;
        public decimal Margen    => PrecioVentaActual > 0
            ? (PrecioVentaActual - PrecioCostoActual) / PrecioVentaActual * 100
            : 0;
    }
}
