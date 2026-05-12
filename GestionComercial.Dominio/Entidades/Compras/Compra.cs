using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Compras
{
    /// <summary>
    /// Entidad Compra con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var compra = Compra.Crear(idProveedor, idSucursal, idUsuario);
    /// </summary>
    public class Compra : EntidadBase
    {
        // ── Backing fields ──
        private DateTime _fecha = DateTime.Now;
        private decimal _total;
        private int _estado = 1;
        private string? _observacion;
        private int _id_proveedor;
        private int _id_sucursal;
        private int _id_usuario;

        // ── Propiedades con validación ──
        public DateTime Fecha 
        { 
            get => _fecha; 
            set => _fecha = value; 
        }
        public decimal Total 
        { 
            get => _total; 
            set => _total = value >= 0 ? value : 0; 
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
        public int Id_proveedor { get => _id_proveedor; set => _id_proveedor = value; }
        public int Id_sucursal { get => _id_sucursal; set => _id_sucursal = value; }
        public int Id_usuario { get => _id_usuario; set => _id_usuario = value; }

        // ── Relaciones ──
        public Proveedor              Proveedor { get; set; } = null!;
        public Sucursal               Sucursal  { get; set; } = null!;
        public Usuario                Usuario   { get; set; } = null!;
        public ICollection<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();

        // ── Constructor vacío (para EF Core) ──
        public Compra() { }

        // ── Factory method ──
        public static Compra Crear(int idProveedor, int idSucursal, int idUsuario, string? observacion = null)
        {
            if (idProveedor <= 0)
                throw new ArgumentException("ID de proveedor inválido.", nameof(idProveedor));
            if (idSucursal <= 0)
                throw new ArgumentException("ID de sucursal inválido.", nameof(idSucursal));
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.", nameof(idUsuario));

            return new Compra
            {
                _id_proveedor = idProveedor,
                _id_sucursal = idSucursal,
                _id_usuario = idUsuario,
                _fecha = DateTime.Now,
                _estado = (int)EstadoCompraEnum.Pendiente,
                _observacion = observacion?.Trim(),
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Añade un ítem a la compra y actualiza el total.
        /// </summary>
        public void AgregarDetalle(CompraDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException(nameof(detalle));
            
            if (_estado == (int)EstadoCompraEnum.Recibida)
                throw new InvalidOperationException("No se puede modificar una compra recibida.");
            if (_estado == (int)EstadoCompraEnum.Anulada)
                throw new InvalidOperationException("No se puede modificar una compra anulada.");

            Detalles.Add(detalle);
            RecalcularTotal();
        }

        /// <summary>
        /// Quita un ítem y recalcula el total.
        /// </summary>
        public void QuitarDetalle(int detalleId)
        {
            if (_estado == (int)EstadoCompraEnum.Recibida)
                throw new InvalidOperationException("No se puede modificar una compra recibida.");
            if (_estado == (int)EstadoCompraEnum.Anulada)
                throw new InvalidOperationException("No se puede modificar una compra anulada.");

            var detalle = Detalles.FirstOrDefault(d => d.Id == detalleId);
            if (detalle != null)
            {
                Detalles.Remove(detalle);
                RecalcularTotal();
            }
        }

        /// <summary>
        /// Recalcula el total desde los detalles.
        /// </summary>
        public void RecalcularTotal()
        {
            _total = Detalles.Sum(d => d.Subtotal);
        }

        /// <summary>
        /// Marca como recibida. Solo si está pendiente.
        /// </summary>
        public void MarcarRecibida()
        {
            if (_estado != (int)EstadoCompraEnum.Pendiente)
                throw new InvalidOperationException(
                    $"No se puede recepcionar. Estado actual: {(EstadoCompraEnum)_estado}");

            _estado = (int)EstadoCompraEnum.Recibida;
        }

        /// <summary>
        /// Anula la compra. Solo si está pendiente.
        /// </summary>
        public void Anular(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de anulación es requerido.", nameof(motivo));
            if (_estado == (int)EstadoCompraEnum.Recibida)
                throw new InvalidOperationException("No se puede anular una compra recibida. Deben invertirse primero los movimientos de stock.");
            if (_estado == (int)EstadoCompraEnum.Anulada)
                throw new InvalidOperationException("La compra ya está anulada.");

            _estado = (int)EstadoCompraEnum.Anulada;
            _observacion = $"ANULADA: {motivo}";
        }

        // ── Propiedades computed ──
        public bool EsPendiente => _estado == (int)EstadoCompraEnum.Pendiente;
        public bool EsRecibida  => _estado == (int)EstadoCompraEnum.Recibida;
        public bool EsAnulada   => _estado == (int)EstadoCompraEnum.Anulada;
        public bool PuedeModificarse => EsPendiente;
    }
}