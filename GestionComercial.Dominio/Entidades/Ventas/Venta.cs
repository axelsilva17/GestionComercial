using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Ventas
{
    /// <summary>
    /// Entidad Venta con patrón DDD.
    /// 
    /// Preferir factory methods Crear() y CrearDesdeCarrito():
    ///   var venta = Venta.Crear(idSucursal, idCliente, idUsuario, idCaja);
    /// </summary>
    public class Venta : EntidadBase
    {
        // ── Backing fields para encapsulamiento ──
        private DateTime _fecha = DateTime.Now;
        private decimal _totalBruto;
        private decimal _totalDescuento;
        private decimal _totalFinal;
        private int _estado = 1;  // Pendiente por defecto
        private string? _observacion;
        private int _id_sucursal;
        private int _id_cliente;
        private int _id_usuario;
        private int? _id_caja;
        private decimal? _efectivoRecibido;
        private string? _motivoAnulacion;
        private DateTime? _fechaAnulacion;
        private int? _usuarioAnulacionId;

        // ── Propiedades con validación ──
        public DateTime Fecha 
        { 
            get => _fecha; 
            set => _fecha = value; 
        }
        public decimal TotalBruto 
        { 
            get => _totalBruto; 
            set => _totalBruto = value >= 0 ? value : 0; 
        }
        public decimal TotalDescuento 
        { 
            get => _totalDescuento; 
            set => _totalDescuento = value >= 0 ? value : 0; 
        }
        public decimal TotalFinal 
        { 
            get => _totalFinal; 
            set => _totalFinal = value >= 0 ? value : 0; 
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
        public int Id_sucursal { get => _id_sucursal; set => _id_sucursal = value; }
        public int Id_cliente { get => _id_cliente; set => _id_cliente = value; }
        public int Id_usuario { get => _id_usuario; set => _id_usuario = value; }
        public int? Id_caja { get => _id_caja; set => _id_caja = value; }
        public decimal? EfectivoRecibido 
        { 
            get => _efectivoRecibido; 
            set => _efectivoRecibido = value >= 0 ? value : null; 
        }

        // ── Campos de anulación ──
        public string? MotivoAnulacion 
        { 
            get => _motivoAnulacion; 
            set => _motivoAnulacion = value; 
        }
        public DateTime? FechaAnulacion { get => _fechaAnulacion; set => _fechaAnulacion = value; }
        public int? UsuarioAnulacionId { get => _usuarioAnulacionId; set => _usuarioAnulacionId = value; }

        // ── Relaciones ──
        public Sucursal Sucursal { get; set; } = null!;
        public Cliente.Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public Caja.Caja Caja { get; set; } = null!;
        public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

        // ── Constructor vacío (para EF Core) ──
        public Venta() { }

        // ── Factory method principal ──
        public static Venta Crear(int idSucursal, int idCliente, int idUsuario, int? idCaja = null)
        {
            if (idSucursal <= 0)
                throw new ArgumentException("ID de sucursal inválido.", nameof(idSucursal));
            if (idCliente <= 0)
                throw new ArgumentException("ID de cliente inválido.", nameof(idCliente));
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.", nameof(idUsuario));

            return new Venta
            {
                _id_sucursal = idSucursal,
                _id_cliente = idCliente,
                _id_usuario = idUsuario,
                _id_caja = idCaja,
                _fecha = DateTime.Now,
                _estado = (int)EstadoVentaEnum.Pendiente,
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Añade un ítem a la venta y actualiza totales.
        /// </summary>
        public void AgregarDetalle(VentaDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException(nameof(detalle));
            
            // No permitir modify si ya está pagada
            if (_estado == (int)EstadoVentaEnum.Pagada)
                throw new InvalidOperationException("No se puede modificar una venta pagada.");
            if (_estado == (int)EstadoVentaEnum.Anulada)
                throw new InvalidOperationException("No se puede modificar una venta anulada.");

            Detalles.Add(detalle);
            RecalcularTotales();
        }

        /// <summary>
        /// Quita un ítem y recalcula.
        /// </summary>
        public void QuitarDetalle(int detalleId)
        {
            if (_estado == (int)EstadoVentaEnum.Pagada)
                throw new InvalidOperationException("No se puede modificar una venta pagada.");
            if (_estado == (int)EstadoVentaEnum.Anulada)
                throw new InvalidOperationException("No se puede modificar una venta anulada.");

            var detalle = Detalles.FirstOrDefault(d => d.Id == detalleId);
            if (detalle != null)
            {
                Detalles.Remove(detalle);
                RecalcularTotales();
            }
        }

        /// <summary>
        /// Recalcula bruto, descuento y final desde los detalles.
        /// </summary>
        public void RecalcularTotales()
        {
            _totalBruto = Detalles.Sum(d => d.Subtotal);
            _totalDescuento = Detalles.Sum(d => d.Descuento);
            _totalFinal = _totalBruto - _totalDescuento;
        }

        /// <summary>
        /// Marcar como pagada. Solo si está pendiente.
        /// </summary>
        public void MarcarPagada()
        {
            if (_estado != (int)EstadoVentaEnum.Pendiente)
                throw new InvalidOperationException(
                    $"No se puede pagar. Estado actual: {Estado}");

            _estado = (int)EstadoVentaEnum.Pagada;
        }

        /// <summary>
        /// Marcar como pendiente (para reverses).
        /// </summary>
        public void MarcarPendiente()
        {
            if (_estado == (int)EstadoVentaEnum.Anulada)
                throw new InvalidOperationException("No se puede revertir una venta anulada.");
            
            _estado = (int)EstadoVentaEnum.Pendiente;
        }

        /// <summary>
        /// Anula la venta con motivo. Requiere que esté pendiente o pagada.
        /// </summary>
        public void Anular(string motivo, int idUsuarioAnulacion)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de anulación es requerido.", nameof(motivo));
            if (_estado == (int)EstadoVentaEnum.Anulada)
                throw new InvalidOperationException("La venta ya está anulada.");

            _estado = (int)EstadoVentaEnum.Anulada;
            _motivoAnulacion = motivo;
            _fechaAnulacion = DateTime.Now;
            _usuarioAnulacionId = idUsuarioAnulacion;
        }

        /// <summary>
        /// Agregar pago.
        /// </summary>
        public void AgregarPago(Pago pago)
        {
            if (pago == null)
                throw new ArgumentNullException(nameof(pago));
            
            if (_estado == (int)EstadoVentaEnum.Anulada)
                throw new InvalidOperationException("No se puede pagar una venta anulada.");

            Pagos.Add(pago);
        }

        /// <summary>
        /// Total de pagos registrados.
        /// </summary>
        public decimal TotalPagado => Pagos.Sum(p => p.Monto);

        /// <summary>
        /// Verifica si el total pagado cubre el total de la venta.
        /// </summary>
        public bool EstaPagada => TotalPagado >= _totalFinal;

        /// <summary>
        /// Cambio/restante.
        /// </summary>
        public decimal Cambio => TotalPagado > _totalFinal 
            ? TotalPagado - _totalFinal 
            : 0;

        // ── Propiedades computed ──
        public bool EsPendiente => _estado == (int)EstadoVentaEnum.Pendiente;
        public bool EsPagada => _estado == (int)EstadoVentaEnum.Pagada;
        public bool EsAnulada => _estado == (int)EstadoVentaEnum.Anulada;
        public bool PuedeModificarse => EsPendiente;
        public bool PuedePagarse => EsPendiente && Detalles.Any();
    }
}
