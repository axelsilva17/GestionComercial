using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Ventas
{
    public class Venta : EntidadBase
    {
        public DateTime Fecha          { get; set; } = DateTime.Now;
        public decimal  TotalBruto     { get; set; }
        public decimal  TotalDescuento { get; set; }
        public decimal  TotalFinal     { get; set; }
        public int      Estado         { get; set; } = 1;
        public string?  Observacion    { get; set; }
        public int      Id_sucursal    { get; set; }
        public int      Id_cliente     { get; set; }
        public int      Id_usuario     { get; set; }
        public int?     Id_caja        { get; set; }

        public Sucursal                  Sucursal { get; set; } = null!;
        public Cliente.Cliente           Cliente  { get; set; } = null!;
        public Usuario                   Usuario  { get; set; } = null!;
        public Caja.Caja Caja { get; set; } = null!;
        public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
        public ICollection<Pago>         Pagos    { get; set; } = new List<Pago>();
    }


}
