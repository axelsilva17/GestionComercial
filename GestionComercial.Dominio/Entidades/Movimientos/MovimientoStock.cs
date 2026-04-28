using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Movimientos
{
    /// <summary>
    /// Entidad MovimientoStock con patrón DDD.
    /// 
    /// Preferir factory methods:
    ///   var mov = MovimientoStock.Entrada(...);
    ///   var mov = MovimientoStock.Salida(...);
    ///   var mov = MovimientoStock.Ajuste(...);
    /// </summary>
    public class MovimientoStock
    {
        // ── Backing fields ──
        private int _tipoMovimiento;
        private string? _observacion;
        private decimal _cantidad;
        private decimal _stockAnterior;
        private decimal _stockNuevo;
        private DateTime _fecha = DateTime.Now;
        private int? _referenciaId;
        private int _id_sucursal;
        private int _id_producto;
        private int _id_usuario;

        // ── Propiedades con validación ──
        public int Id { get; set; }  // Para EF Core

        public int TipoMovimiento 
        { 
            get => _tipoMovimiento; 
            set => _tipoMovimiento = value; 
        }
        public string? Observacion 
        { 
            get => _observacion; 
            set => _observacion = value; 
        }
        public decimal Cantidad 
        { 
            get => _cantidad; 
            set => _cantidad = value >= 0 ? value : 0; 
        }
        public decimal StockAnterior 
        { 
            get => _stockAnterior; 
            set => _stockAnterior = value; 
        }
        public decimal StockNuevo 
        { 
            get => _stockNuevo; 
            set => _stockNuevo = value; 
        }
        public DateTime Fecha 
        { 
            get => _fecha; 
            set => _fecha = value; 
        }
        public int? ReferenciaId 
        { 
            get => _referenciaId; 
            set => _referenciaId = value; 
        }
        public int Id_sucursal { get => _id_sucursal; set => _id_sucursal = value; }
        public int Id_producto { get => _id_producto; set => _id_producto = value; }
        public int Id_usuario { get => _id_usuario; set => _id_usuario = value; }

        // ── Relaciones ──
        public Sucursal Sucursal { get; set; } = null!;
        public Producto.Producto Producto { get; set; } = null!;
        public Usuario  Usuario  { get; set; } = null!;

        // ── Constructor vacío (para EF Core) ──
        public MovimientoStock() { }

        // ── Factory methods ──
        
        /// <summary>
        /// Crea un movimiento de entrada (compra, ajuste positivo, etc.)
        /// </summary>
        public static MovimientoStock Entrada(decimal cantidad, decimal stockActual,
            int idProducto, int idSucursal, int idUsuario, string? observacion = null, int? referenciaId = null)
        {
            return Crear(TipoMovimientoStockEnum.Entrada, cantidad, stockActual, idProducto, idSucursal, idUsuario, observacion, referenciaId);
        }

        /// <summary>
        /// Crea un movimiento de salida (venta, ajuste negativo, etc.)
        /// </summary>
        public static MovimientoStock Salida(decimal cantidad, decimal stockActual,
            int idProducto, int idSucursal, int idUsuario, string? observacion = null, int? referenciaId = null)
        {
            return Crear(TipoMovimientoStockEnum.Salida, cantidad, stockActual, idProducto, idSucursal, idUsuario, observacion, referenciaId);
        }

        /// <summary>
        /// Crea un movimiento de ajuste (recount, etc.)
        /// </summary>
        public static MovimientoStock Ajuste(decimal cantidad, decimal stockAnterior,
            int idProducto, int idSucursal, int idUsuario, string? observacion = null, int? referenciaId = null)
        {
            return Crear(TipoMovimientoStockEnum.Ajuste, cantidad, stockAnterior, idProducto, idSucursal, idUsuario, observacion, referenciaId);
        }

        private static MovimientoStock Crear(TipoMovimientoStockEnum tipo, decimal cantidad, decimal stockAnterior,
            int idProducto, int idSucursal, int idUsuario, string? observacion, int? referenciaId)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
            if (idProducto <= 0)
                throw new ArgumentException("ID de producto inválido.", nameof(idProducto));
            if (idSucursal <= 0)
                throw new ArgumentException("ID de sucursal inválido.", nameof(idSucursal));
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.", nameof(idUsuario));

            decimal stockNuevo;
            switch (tipo)
            {
                case TipoMovimientoStockEnum.Entrada:
                    stockNuevo = stockAnterior + cantidad;
                    break;
                case TipoMovimientoStockEnum.Salida:
                    if (stockAnterior < cantidad)
                        throw new InvalidOperationException($"Stock insuficiente. Actual: {stockAnterior}, solicitado: {cantidad}");
                    stockNuevo = stockAnterior - cantidad;
                    break;
                case TipoMovimientoStockEnum.Ajuste:
                    // En ajustes, 'cantidad' es el stock nuevo, no el delta
                    stockNuevo = cantidad;
                    cantidad = Math.Abs(cantidad - stockAnterior);  // Convertir a delta para consistencia
                    break;
                default:
                    throw new ArgumentException($"Tipo de movimiento inválido: {tipo}");
            }

            return new MovimientoStock
            {
                _tipoMovimiento = (int)tipo,
                _cantidad = cantidad,
                _stockAnterior = stockAnterior,
                _stockNuevo = stockNuevo,
                _id_producto = idProducto,
                _id_sucursal = idSucursal,
                _id_usuario = idUsuario,
                _observacion = observacion?.Trim(),
                _referenciaId = referenciaId,
                _fecha = DateTime.Now
            };
        }

        // ── Propiedades computed ──
        public bool EsEntrada => _tipoMovimiento == (int)TipoMovimientoStockEnum.Entrada;
        public bool EsSalida  => _tipoMovimiento == (int)TipoMovimientoStockEnum.Salida;
        public bool EsAjuste => _tipoMovimiento == (int)TipoMovimientoStockEnum.Ajuste;

        public string TipoDisplay => ((TipoMovimientoStockEnum)_tipoMovimiento).ToString();

        public decimal DeltaStock => _stockNuevo - _stockAnterior;
    }
}