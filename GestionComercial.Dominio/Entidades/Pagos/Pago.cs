using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Pagos
{
    /// <summary>
    /// Entidad Pago con patrón DDD.
    /// 
    /// Preferir factory method:
    ///   var pago = Pago.Crear(monto, idVenta, idMetodoPago);
    /// </summary>
    public class Pago
    {
        // ── Backing fields ──
        private decimal _monto;
        private DateTime _fecha = DateTime.Now;
        private int _id_venta;
        private int _id_metodoPago;
        private int? _id_movimientoCaja;

        // ── Propiedades con validación ──
        public int Id { get; set; }  // Para EF Core

        public decimal Monto 
        { 
            get => _monto; 
            set => _monto = value > 0 ? value : throw new ArgumentException("El monto debe ser mayor a 0.", nameof(value)); 
        }
        public DateTime Fecha 
        { 
            get => _fecha; 
            set => _fecha = value; 
        }
        public int Id_venta { get => _id_venta; set => _id_venta = value; }
        public int Id_metodoPago { get => _id_metodoPago; set => _id_metodoPago = value; }
        
        // ── Link a MovimientoCaja (solo para pagos en efectivo) ─────────────────
        public int? Id_movimientoCaja { get => _id_movimientoCaja; set => _id_movimientoCaja = value; }

        public Venta            Venta          { get; set; } = null!;
        public MetodoPago       MetodoPago     { get; set; } = null!;
        public TipoMovimientoCaja? MovimientoCaja { get; set; }

        // ── Constructor vacío (para EF Core) ──
        public Pago() { }

        // ── Factory method ──
        public static Pago Crear(decimal monto, int idVenta, int idMetodoPago)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.", nameof(monto));
            if (idVenta <= 0)
                throw new ArgumentException("ID de venta inválido.", nameof(idVenta));
            if (idMetodoPago <= 0)
                throw new ArgumentException("ID de método de pago inválido.", nameof(idMetodoPago));

            return new Pago
            {
                _monto = monto,
                _id_venta = idVenta,
                _id_metodoPago = idMetodoPago,
                _fecha = DateTime.Now
            };
        }

        /// <summary>
        /// Vincula con un movimiento de caja (para efectivo).
        /// </summary>
        public void VincularMovimientoCaja(int idMovimientoCaja)
        {
            if (idMovimientoCaja <= 0)
                throw new ArgumentException("ID de movimiento de caja inválido.", nameof(idMovimientoCaja));
            if (Id_metodoPago != (int)MetodoPagoEnum.Efectivo)
                throw new InvalidOperationException("Solo los pagos en efectivo pueden vincularse a un movimiento de caja.");

            _id_movimientoCaja = idMovimientoCaja;
        }

        // ── Propiedades computed ──
        public bool EsEfectivo => Id_metodoPago == (int)MetodoPagoEnum.Efectivo;
        public bool EsTarjeta => Id_metodoPago == (int)MetodoPagoEnum.Tarjeta;
        public bool EsTransferencia => Id_metodoPago == (int)MetodoPagoEnum.Transferencia;
        public bool Linkeado => _id_movimientoCaja.HasValue;
    }
}