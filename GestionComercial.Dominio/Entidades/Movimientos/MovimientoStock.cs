using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Movimientos
{
    ///     /// Entidad MovimientoStock con patrón DDD.
    /// 
    /// Preferir factory methods:
    ///   var mov = MovimientoStock.Entrada(...);
    ///   var mov = MovimientoStock.Salida(...);
    ///   var mov = MovimientoStock.Ajuste(...);
    public class MovimientoStock
    {
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

        ///         /// Crea un movimiento de entrada (compra, ajuste positivo, etc.)
        public static MovimientoStock Entrada(decimal cantidad, decimal stockActual,
            int idProducto, int idSucursal, int idUsuario, string? observacion = null, int? referenciaId = null)
        {
            return Crear(TipoMovimientoStockEnum.Entrada, cantidad, stockActual, idProducto, idSucursal, idUsuario, observacion, referenciaId);
        }

        ///         /// Crea un movimiento de salida (venta, ajuste negativo, etc.)
        public static MovimientoStock Salida(decimal cantidad, decimal stockActual,
            int idProducto, int idSucursal, int idUsuario, string? observacion = null, int? referenciaId = null)
        {
            return Crear(TipoMovimientoStockEnum.Salida, cantidad, stockActual, idProducto, idSucursal, idUsuario, observacion, referenciaId);
        }

        ///         /// Crea un movimiento de ajuste con signo.
        /// 'delta' es el cambio firmado sobre el stock anterior:
        ///   delta > 0 -> AjustePositivo (aumenta stock)
        ///   delta < 0 -> AjusteNegativo (disminuye stock)
        /// El stock nuevo se calcula como stockAnterior + delta (piso 0)
        /// y la Cantidad almacenada queda como el valor absoluto del delta.
        public static MovimientoStock Ajuste(decimal delta, decimal stockAnterior,
            int idProducto, int idSucursal, int idUsuario, string? observacion = null, int? referenciaId = null)
        {
            var tipo = delta >= 0 ? TipoMovimientoStockEnum.AjustePositivo : TipoMovimientoStockEnum.AjusteNegativo;
            return Crear(tipo, delta, stockAnterior, idProducto, idSucursal, idUsuario, observacion, referenciaId);
        }

        private static MovimientoStock Crear(TipoMovimientoStockEnum tipo, decimal cantidad, decimal stockAnterior,
            int idProducto, int idSucursal, int idUsuario, string? observacion, int? referenciaId)
        {
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
                    if (cantidad <= 0)
                        throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
                    stockNuevo = stockAnterior + cantidad;
                    break;
                case TipoMovimientoStockEnum.Salida:
                    if (cantidad <= 0)
                        throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
                    if (stockAnterior < cantidad)
                        throw new InvalidOperationException($"Stock insuficiente. Actual: {stockAnterior}, solicitado: {cantidad}");
                    stockNuevo = stockAnterior - cantidad;
                    break;
                case TipoMovimientoStockEnum.AjustePositivo:
                case TipoMovimientoStockEnum.AjusteNegativo:
                    // 'cantidad' es el delta CON SIGNO. Validamos que la magnitud sea > 0.
                    if (cantidad == 0)
                        throw new ArgumentException("El ajuste no puede ser cero.", nameof(cantidad));
                    stockNuevo = Math.Max(0, stockAnterior + cantidad);
                    cantidad = Math.Abs(cantidad);  // Guardar magnitud (consistente con Entrada/Salida)
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

        public bool EsEntrada => _tipoMovimiento == (int)TipoMovimientoStockEnum.Entrada;
        public bool EsSalida  => _tipoMovimiento == (int)TipoMovimientoStockEnum.Salida;
        public bool EsAjustePositivo => _tipoMovimiento == (int)TipoMovimientoStockEnum.AjustePositivo;
        public bool EsAjusteNegativo => _tipoMovimiento == (int)TipoMovimientoStockEnum.AjusteNegativo;
        public bool EsAjuste => EsAjustePositivo || EsAjusteNegativo;

        public string TipoDisplay => ((TipoMovimientoStockEnum)_tipoMovimiento).ToString();

        public decimal DeltaStock => _stockNuevo - _stockAnterior;
    }
}