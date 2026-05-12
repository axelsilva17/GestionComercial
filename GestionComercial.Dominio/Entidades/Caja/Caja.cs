using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Caja
{
    /// <summary>
    /// Entidad Caja con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var caja = Caja.Crear(idSucursal, idUsuarioApertura, montoInicial, esPrimaria, turno);
    /// </summary>
    public class Caja : EntidadBase
    {
        // ── Backing fields ──
        private DateTime _fechaApertura = DateTime.Now;
        private DateTime? _fechaCierre;
        private decimal _montoInicial;
        private decimal? _montoFinal;
        private int _estado = 1;
        private string? _observacion;
        private int _usuarioApertura_id;
        private int? _usuarioCierre_id;
        private int _id_sucursal;
        private bool _esPrimaria;
        private string? _turno;

        // ── Propiedades con validación ──
        public DateTime FechaApertura 
        { 
            get => _fechaApertura; 
            set => _fechaApertura = value; 
        }
        public DateTime? FechaCierre 
        { 
            get => _fechaCierre; 
            set => _fechaCierre = value; 
        }
        public decimal MontoInicial 
        { 
            get => _montoInicial; 
            set => _montoInicial = value >= 0 ? value : 0; 
        }
        public decimal? MontoFinal 
        { 
            get => _montoFinal; 
            set => _montoFinal = value >= 0 ? value : null; 
        }
        public int Estado 
        { 
            get => _estado; 
            set => _estado = value; 
        }
        public string? Observacion 
        { 
            get => _observacion; 
            set => _observacion = value; 
        }
        public int UsuarioApertura_id { get => _usuarioApertura_id; set => _usuarioApertura_id = value; }
        public int? UsuarioCierre_id { get => _usuarioCierre_id; set => _usuarioCierre_id = value; }
        public int Id_sucursal { get => _id_sucursal; set => _id_sucursal = value; }
        public bool EsPrimaria 
        { 
            get => _esPrimaria; 
            set => _esPrimaria = value; 
        }
        public string? Turno 
        { 
            get => _turno; 
            set => _turno = value; 
        }

        // ── Relaciones ──
        public Sucursal  Sucursal        { get; set; } = null!;
        public Usuario UsuarioApertura { get; set; } = null!;
        public Usuario? UsuarioCierre   { get; set; }
        public ICollection<TipoMovimientoCaja> Movimientos { get; set; } = new List<TipoMovimientoCaja>();
        public ICollection<Venta>          Ventas      { get; set; } = new List<Venta>();

        // ── Constructor vacío (para EF Core) ──
        public Caja() { }

        // ── Factory method ──
        public static Caja Crear(int idSucursal, int idUsuarioApertura, 
            decimal montoInicial = 0, bool esPrimaria = false, string? turno = null)
        {
            if (idSucursal <= 0)
                throw new ArgumentException("ID de sucursal inválido.", nameof(idSucursal));
            if (idUsuarioApertura <= 0)
                throw new ArgumentException("ID de usuario inválido.", nameof(idUsuarioApertura));
            if (montoInicial < 0)
                throw new ArgumentException("El monto inicial no puede ser negativo.", nameof(montoInicial));

            return new Caja
            {
                _id_sucursal = idSucursal,
                _usuarioApertura_id = idUsuarioApertura,
                _montoInicial = montoInicial,
                _esPrimaria = esPrimaria,
                _turno = turno?.Trim(),
                _fechaApertura = DateTime.Now,
                _estado = (int)EstadoCajaEnum.Abierta,
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Abre la caja (re-apertura). Solo si está cerrada.
        /// </summary>
        public void Abrir(int idUsuario, decimal montoInicial)
        {
            if (EsAbierta)
                throw new InvalidOperationException("La caja ya está abierta.");
            if (montoInicial < 0)
                throw new ArgumentException("El monto inicial no puede ser negativo.", nameof(montoInicial));

            _estado = (int)EstadoCajaEnum.Abierta;
            _fechaApertura = DateTime.Now;
            _fechaCierre = null;
            _montoInicial = montoInicial;
            _montoFinal = null;
            _observacion = null;
        }

        /// <summary>
        /// Cierra la caja. Solo si está abierta.
        /// </summary>
        public void Cerrar(int idUsuarioCierre, decimal? montoFinal = null)
        {
            if (!EsAbierta)
                throw new InvalidOperationException("La caja ya está cerrada.");
            
            // Validar que no haya ventas pendientes (se hace en el servicio)
            
            _estado = (int)EstadoCajaEnum.Cerrada;
            _fechaCierre = DateTime.Now;
            _usuarioCierre_id = idUsuarioCierre;
            _montoFinal = montoFinal;
        }

        /// <summary>
        /// Agrega monto adicional (aperturas parciales).
        /// </summary>
        public void AgregarMonto(decimal monto)
        {
            if (!EsAbierta)
                throw new InvalidOperationException("La caja está cerrada.");
            if (monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.", nameof(monto));

            _montoInicial += monto;
        }

        /// <summary>
        /// Obtiene el monto actual en caja.
        /// </summary>
        public decimal MontoActual => _montoInicial + (Ventas.Sum(v => v.TotalFinal) - Ventas.Sum(v => v.TotalPagado));

        // ── Propiedades computed ──
        public bool EstaAbierta => _estado == (int)EstadoCajaEnum.Abierta;
        
        public bool EstaCerrada => _estado == (int)EstadoCajaEnum.Cerrada;

        public bool EsAbierta => _estado == (int)EstadoCajaEnum.Abierta;

        public string Display => $"{(_esPrimaria ? "⭐ " : "")}Caja {Id} ({Turno ?? "Sin turno"})";
    }
}