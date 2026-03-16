using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Organizacion;

namespace GestionComercial.Dominio.Entidades.Proveedores
{
    public class Proveedor : EntidadBase
    {
        public string  Nombre     { get; set; } = string.Empty;
        public string? Telefono   { get; set; }
        public string? Email      { get; set; }
        public string? CUIT       { get; set; }
        public int     Id_empresa { get; set; }

        public Empresa             Empresa  { get; set; } = null!;
        public ICollection<Compra> Compras  { get; set; } = new List<Compra>();
    }
}
