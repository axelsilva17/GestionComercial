using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Cliente
{
    public class Cliente : EntidadBase
    {
        public string  Nombre     { get; set; } = string.Empty;
        public int  Documento  { get; set; } 
        public string? Telefono   { get; set; }
        public string? Email      { get; set; }
        public int     Id_empresa { get; set; }
        public bool    Activo     { get; set; } = true;

        public Empresa            Empresa { get; set; } = null!;
        public ICollection<Venta> Ventas  { get; set; } = new List<Venta>();

        public string Inicial => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
