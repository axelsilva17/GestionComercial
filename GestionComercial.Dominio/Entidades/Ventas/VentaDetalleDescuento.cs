namespace GestionComercial.Dominio.Entidades.Ventas
{
    /// <summary>
    /// Representa un descuento aplicado a un ítem específico de venta.
    /// Permite múltiples descuentos por ítem con porcentaje y monto.
    /// </summary>
    public class VentaDetalleDescuento
    {
        // ── Backing fields ──
        private decimal _porcentaje;
        private decimal _monto;
        private int _id_detalle;
        private string? _descripcion;

        // ── Propiedades con validación ──
        public int Id { get; set; }  // Para EF Core

        public int Id_detalle 
        { 
            get => _id_detalle; 
            set => _id_detalle = value; 
        }
        public decimal Porcentaje 
        { 
            get => _porcentaje; 
            set => _porcentaje = value >= 0 ? value : 0; 
        }
        public decimal Monto 
        { 
            get => _monto; 
            set => _monto = value >= 0 ? value : 0; 
        }
        public string? Descripcion 
        { 
            get => _descripcion; 
            set => _descripcion = value; 
        }

        public VentaDetalle Detalle { get; set; } = null!;

        // ── Constructor vacío (para EF Core) ──
        public VentaDetalleDescuento() { }

        // ── Factory methods ──
        
        /// <summary>
        /// Crea un descuento por monto fijo.
        /// </summary>
        public static VentaDetalleDescuento PorMonto(decimal monto, int idDetalle, string? descripcion = null)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.", nameof(monto));

            return new VentaDetalleDescuento
            {
                _id_detalle = idDetalle,
                _monto = monto,
                _porcentaje = 0,
                _descripcion = descripcion?.Trim()
            };
        }

        /// <summary>
        /// Crea un descuento por porcentaje.
        /// </summary>
        public static VentaDetalleDescuento PorPorcentaje(decimal porcentaje, decimal montoBase, int idDetalle, string? descripcion = null)
        {
            if (porcentaje <= 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100.", nameof(porcentaje));

            return new VentaDetalleDescuento
            {
                _id_detalle = idDetalle,
                _porcentaje = porcentaje,
                _monto = montoBase * (porcentaje / 100m),
                _descripcion = descripcion?.Trim()
            };
        }

        /// <summary>
        /// Recalcula el monto basándose en un nuevo precio base.
        /// </summary>
        public void RecalcularMonto(decimal precioBase)
        {
            if (_porcentaje > 0)
                _monto = precioBase * (_porcentaje / 100m);
        }

        public bool EsPorMonto => _porcentaje == 0 && _monto > 0;
        public bool EsPorPorcentaje => _porcentaje > 0;
    }

    /// <summary>
    /// Representa un impuesto aplicado a un ítem específico de venta.
    /// </summary>
    public class VentaDetalleImpuesto
    {
        // ── Backing fields ──
        private decimal _porcentaje;
        private decimal _monto;
        private int _id_detalle;
        private int _id_tipoImpuesto;

        // ── Propiedades con validación ──
        public int Id { get; set; }  // Para EF Core

        public int Id_detalle 
        { 
            get => _id_detalle; 
            set => _id_detalle = value; 
        }
        public int Id_tipoImpuesto 
        { 
            get => _id_tipoImpuesto; 
            set => _id_tipoImpuesto = value; 
        }
        public decimal Porcentaje 
        { 
            get => _porcentaje; 
            set => _porcentaje = value >= 0 ? value : 0; 
        }
        public decimal Monto 
        { 
            get => _monto; 
            set => _monto = value >= 0 ? value : 0; 
        }

        public VentaDetalle Detalle { get; set; } = null!;

        // ── Constructor vacío (para EF Core) ──
        public VentaDetalleImpuesto() { }

        // ── Factory method ──
        public static VentaDetalleImpuesto Crear(decimal porcentaje, decimal montoBase, 
            int idDetalle, int idTipoImpuesto)
        {
            if (porcentaje <= 0)
                throw new ArgumentException("El porcentaje debe ser mayor a 0.", nameof(porcentaje));

            return new VentaDetalleImpuesto
            {
                _id_detalle = idDetalle,
                _id_tipoImpuesto = idTipoImpuesto,
                _porcentaje = porcentaje,
                _monto = montoBase * (porcentaje / 100m)
            };
        }

        /// <summary>
        /// Recalcula el monto basándose en un nuevo precio base.
        /// </summary>
        public void RecalcularMonto(decimal precioBase)
        {
            _monto = precioBase * (_porcentaje / 100m);
        }
    }
}