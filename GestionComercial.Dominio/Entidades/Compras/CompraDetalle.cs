using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Compras
{


    public class CompraDetalle
    {
        public int     Id          { get; set; }
        public decimal Cantidad    { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal Subtotal    { get; set; }
        public int     Id_compra   { get; set; }
        public int     Id_producto { get; set; }

        public Compra  Compra   { get; set; } = null!;
        public Producto.Producto Producto { get; set; } = null!;
    }
}
