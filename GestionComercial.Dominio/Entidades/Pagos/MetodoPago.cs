using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Pagos
{
    public class MetodoPago
    {
        public int    Id         { get; set; }
        public string Nombre     { get; set; } = string.Empty;
        public bool   EsEfectivo { get; set; }
        public bool   Activo     { get; set; } = true;
        public int    Id_empresa { get; set; }

        public Empresa           Empresa { get; set; } = null!;
        public ICollection<Pago> Pagos   { get; set; } = new List<Pago>();
    }


}
