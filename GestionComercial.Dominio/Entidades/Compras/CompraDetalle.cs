using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Compras
{
    /// <summary>
    /// Entidad CompraDetalle con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var detalle = CompraDetalle.Crear(producto, cantidad, precioCosto);
    /// </summary>
    public class CompraDetalle
    {
        // ── Backing fields ──
        private decimal _cantidad;
        private decimal _precioCosto;
        private decimal _subtotal;
        private int _id_compra;
        private int _id_producto;

        // ── Propiedades con validación ──
        public int Id { get; set; }  // Para EF Core

        public decimal Cantidad 
        { 
            get => _cantidad; 
            set => _cantidad = value > 0 ? value : throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(value)); 
        }
        public decimal PrecioCosto 
        { 
            get => _precioCosto; 
            set => _precioCosto = value >= 0 ? value : 0; 
        }
        public decimal Subtotal 
        { 
            get => _subtotal; 
            set => _subtotal = value; 
        }
        public int Id_compra { get => _id_compra; set => _id_compra = value; }
        public int Id_producto { get => _id_producto; set => _id_producto = value; }

        public Compra  Compra   { get; set; } = null!;
        public Producto.Producto Producto { get; set; } = null!;

        // ── Constructor vacío (para EF Core) ──
        public CompraDetalle() { }

        // ── Factory method ──
        public static CompraDetalle Crear(Producto.Producto producto, decimal cantidad, decimal precioCosto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
            if (precioCosto < 0)
                throw new ArgumentException("El precio de costo no puede ser negativo.", nameof(precioCosto));

            var detalle = new CompraDetalle
            {
                _id_producto = producto.Id,
                _cantidad = cantidad,
                _precioCosto = precioCosto
            };

            detalle.Producto = producto;
            detalle.Recalcular();
            
            return detalle;
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Recalcula el subtotal.
        /// </summary>
        public void Recalcular()
        {
            _subtotal = _cantidad * _precioCosto;
        }

        /// <summary>
        /// Actualiza el precio de costo.
        /// </summary>
        public void ActualizarPrecio(decimal nuevoPrecioCosto)
        {
            if (nuevoPrecioCosto < 0)
                throw new ArgumentException("El precio de costo no puede ser negativo.", nameof(nuevoPrecioCosto));

            _precioCosto = nuevoPrecioCosto;
            Recalcular();
        }
    }
}