using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Organizacion
{
    public class Sucursal : EntidadBase
    {
        public string  Nombre     { get; set; } = string.Empty;
        public string  Direccion  { get; set; } = string.Empty;
        public string? Telefono   { get; set; }
        public int     Id_empresa { get; set; }

        public Empresa              Empresa  { get; set; } = null!;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Caja.Caja>    Cajas    { get; set; } = new List<Caja.Caja>();
        public ICollection<Venta>   Ventas   { get; set; } = new List<Venta>();
        public ICollection<Compra>  Compras  { get; set; } = new List<Compra>();
    }
}
