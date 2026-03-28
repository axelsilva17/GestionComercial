using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Caja
{
    public class Caja : EntidadBase
    {
        public DateTime  FechaApertura       { get; set; } = DateTime.Now;
        public DateTime? FechaCierre         { get; set; }
        public decimal   MontoInicial        { get; set; }
        public decimal?  MontoFinal          { get; set; }
        public int       Estado              { get; set; } = 1; // 1=Abierta 2=Cerrada
        public string?   Observacion         { get; set; }
        public int       UsuarioApertura_id  { get; set; }
        public int?      UsuarioCierre_id    { get; set; }
        public int       Id_sucursal         { get; set; }

        public Sucursal  Sucursal        { get; set; } = null!;
        public Usuario   UsuarioApertura { get; set; } = null!;
        public Usuario?  UsuarioCierre   { get; set; }
        public ICollection<TipoMovimientoCaja> Movimientos { get; set; } = new List<TipoMovimientoCaja>();
        public ICollection<Venta>          Ventas      { get; set; } = new List<Venta>();

        public bool EstaAbierta => Estado == 1;
        public bool EsPrimaria { get; set; }
        public string? Turno { get; set; }


    }

}
