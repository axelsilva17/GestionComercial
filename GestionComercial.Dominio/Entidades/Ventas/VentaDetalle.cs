using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Ventas
{

    public class VentaDetalle
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal MargenUnitario { get; set; }
        public int Id_venta { get; set; }
        public int Id_producto { get; set; }

        public Venta Venta { get; set; } = null!;
        public Producto.Producto Producto { get; set; } = null!;
    }
}
