using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Pagos
{
    public class Pago
    {
        public int      Id            { get; set; }
        public decimal  Monto         { get; set; }
        public DateTime Fecha         { get; set; } = DateTime.Now;
        public int      Id_venta      { get; set; }
        public int      Id_metodoPago { get; set; }
        
        // ── Link a MovimientoCaja (solo para pagos en efectivo) ─────────────────
        public int?     Id_movimientoCaja { get; set; }

        public Venta            Venta          { get; set; } = null!;
        public MetodoPago       MetodoPago     { get; set; } = null!;
        public TipoMovimientoCaja? MovimientoCaja { get; set; }
    }
}
