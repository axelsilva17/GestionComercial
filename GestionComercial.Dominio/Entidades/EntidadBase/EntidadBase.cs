namespace GestionComercial.Dominio.Entidades
{
    /// <summary>
    /// Entidad base con patrón DDD.
    /// 
    /// Heredan: Producto, Venta, Cliente, Proveedor, Usuario, Caja, Compra, Sucursal, Empresa.
    /// Las subclases pueden sobreescribir el comportamiento de Activo/Inactivar via virtual.
    /// </summary>
    public abstract class EntidadBase
    {
        // ── Backing fields ──
        private DateTime _fechaAlta = DateTime.Now;
        protected bool _activo = true;

        // ── Propiedades con encapsulamiento ──
        public int Id { get; set; }

        public DateTime FechaAlta 
        { 
            get => _fechaAlta; 
            set => _fechaAlta = value; 
        }

        public bool Activo 
        { 
            get => _activo; 
            set => _activo = value; 
        }

        // ── Constructor protegido (para que solo las subclases puedan instanciar) ──
        protected EntidadBase()
        {
            _fechaAlta = DateTime.Now;
            _activo   = true;
        }

        // ── Métodos de dominio virtuales ──

        /// <summary>
        /// Inactiva la entidad (soft delete).
        /// </summary>
        public virtual void Inactivar()
        {
            _activo = false;
        }

        /// <summary>
        /// Reactiva la entidad.
        /// </summary>
        public virtual void Reactivar()
        {
            _activo = true;
        }
    }
}