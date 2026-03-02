using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Compras
{
    public class Compra : EntidadBase
    {
        public DateTime Fecha        { get; set; } = DateTime.Now;
        public decimal  Total        { get; set; }
        public int      Estado       { get; set; } = 1; // 1=Pendiente 2=Recibida 3=Anulada
        public string?  Observacion  { get; set; }
        public int      Id_proveedor { get; set; }
        public int      Id_sucursal  { get; set; }
        public int      Id_usuario   { get; set; }

        public Proveedor              Proveedor { get; set; } = null!;
        public Sucursal               Sucursal  { get; set; } = null!;
        public Usuario                Usuario   { get; set; } = null!;
        public ICollection<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();
    }

}
