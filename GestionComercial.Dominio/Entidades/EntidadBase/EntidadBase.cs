namespace GestionComercial.Dominio.Entidades
{
    ///     /// Entidad base con patrón DDD.
    /// 
    /// Heredan: Producto, Venta, Cliente, Proveedor, Usuario, Caja, Compra, Sucursal, Empresa.
    /// Las subclases pueden sobreescribir el comportamiento de Activo/Inactivar via virtual.
    public abstract class EntidadBase
    {
        private DateTime _fechaAlta = DateTime.Now;
        protected bool _activo = true;

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

        ///         /// Inactiva la entidad (soft delete).
        public virtual void Inactivar()
        {
            _activo = false;
        }

        ///         /// Reactiva la entidad.
        public virtual void Reactivar()
        {
            _activo = true;
        }
    }
}