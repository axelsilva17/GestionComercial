using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Ventas
{
    /// <summary>
    /// Entidad VentaDetalle con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var detalle = VentaDetalle.Crear(producto, cantidad, precioUnitario);
    /// </summary>
    public class VentaDetalle
    {
        // ── Backing fields ──
        private decimal _cantidad;
        private decimal _precioUnitario;
        private decimal _costoUnitario;
        private decimal _descuento;
        private decimal _subtotal;
        private decimal _margenUnitario;
        private int _id_venta;
        private int _id_producto;

        // ── Propiedades con validación/encapsulamiento ──
        public int Id { get; set; }  // Para EF Core

        public decimal Cantidad 
        { 
            get => _cantidad; 
            set => _cantidad = value > 0 ? value : throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(value)); 
        }
        public decimal PrecioUnitario 
        { 
            get => _precioUnitario; 
            set => _precioUnitario = value >= 0 ? value : 0; 
        }
        public decimal CostoUnitario 
        { 
            get => _costoUnitario; 
            set => _costoUnitario = value >= 0 ? value : 0; 
        }
        public decimal Descuento 
        { 
            get => _descuento; 
            set => _descuento = value >= 0 ? value : 0; 
        }
        public decimal Subtotal 
        { 
            get => _subtotal; 
            set => _subtotal = value; 
        }
        public decimal MargenUnitario 
        { 
            get => _margenUnitario; 
            set => _margenUnitario = value; 
        }
        public int Id_venta { get => _id_venta; set => _id_venta = value; }
        public int Id_producto { get => _id_producto; set => _id_producto = value; }

        public Venta Venta { get; set; } = null!;
        public Producto.Producto Producto { get; set; } = null!;

        // ── Descuentos e Impuestos por ítem ─────────────────────────────────────
        public ICollection<VentaDetalleDescuento> Descuentos { get; set; } = new List<VentaDetalleDescuento>();
        public ICollection<VentaDetalleImpuesto> Impuestos  { get; set; } = new List<VentaDetalleImpuesto>();

        // ── Constructor vacío (para EF Core) ──
        public VentaDetalle() { }

        // ── Factory method ──
        public static VentaDetalle Crear(Producto.Producto producto, decimal cantidad, 
            decimal precioUnitario, decimal costoUnitario = 0, decimal descuentoPorItem = 0)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
            if (precioUnitario < 0)
                throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(precioUnitario));

            var detalle = new VentaDetalle
            {
                _id_producto = producto.Id,
                _cantidad = cantidad,
                _precioUnitario = precioUnitario,
                _costoUnitario = costoUnitario,
                _descuento = descuentoPorItem
            };

            detalle.Producto = producto;
            
            // Calcular subtotal y margen inicial
            detalle.Recalcular();
            
            return detalle;
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Recalcula subtotal y margen desde los valores actuales.
        /// </summary>
        public void Recalcular()
        {
            // Descuentos por porcentaje
            var descuentoTotal = Descuentos.Sum(d => d.Monto);
            if (_descuento > 0)
                descuentoTotal += _descuento;

            _subtotal = (_cantidad * _precioUnitario) - descuentoTotal;
            
            // Margen = Precio - Costo (por unidad)
            if (_precioUnitario > 0 && _costoUnitario > 0)
            {
                _margenUnitario = _precioUnitario - _costoUnitario;
            }
        }

        /// <summary>
        /// Agrega un descuento al ítem.
        /// </summary>
        public void AgregarDescuento(VentaDetalleDescuento descuento)
        {
            if (descuento == null)
                throw new ArgumentNullException(nameof(descuento));
            Descuentos.Add(descuento);
            Recalcular();
        }

        /// <summary>
        /// Agrega un impuesto al ítem.
        /// </summary>
        public void AgregarImpuesto(VentaDetalleImpuesto impuesto)
        {
            if (impuesto == null)
                throw new ArgumentNullException(nameof(impuesto));
            Impuestos.Add(impuesto);
        }

        // ── Propiedades computed ──
        public decimal DescuentoTotal => _descuento + Descuentos.Sum(d => d.Monto);
        
        public decimal ImpuestosTotal => Impuestos.Sum(i => i.Monto);
        
        public decimal TotalFinal => _subtotal + ImpuestosTotal;

        public bool TieneDescuentos => Descuentos.Any() || _descuento > 0;
        public bool TieneImpuestos => Impuestos.Any();
    }
}