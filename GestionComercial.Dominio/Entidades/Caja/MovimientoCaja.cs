using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Caja
{

    public class MovimientoCaja
    {
        public int      Id           { get; set; }
        public int      Tipo         { get; set; } // 1=Ingreso 2=Egreso
        public decimal  Monto        { get; set; }
        public DateTime Fecha        { get; set; } = DateTime.Now;
        public string?  Concepto     { get; set; }
        public int?     ReferenciaId { get; set; }
        public int      Id_caja      { get; set; }
        public int      Id_usuario   { get; set; }

        public Caja    Caja    { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
