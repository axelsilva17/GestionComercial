using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;

namespace GestionComercial.Dominio.Entidades.Descuento
{
    public class DescuentoConfiguracion : EntidadBase
    {
        private string _nombre = string.Empty;

        public string Nombre
        {
            get => _nombre;
            set => _nombre = value ?? string.Empty;
        }

        public ModoDescuentoEnum ModoDescuento { get; set; }
        public AlcanceDescuentoEnum Alcance { get; set; } = AlcanceDescuentoEnum.Producto;
        public decimal Valor { get; set; }
        public int? Id_producto { get; set; }
        public int? Id_categoria { get; set; }
        public bool AplicaCualquierMetodoPago { get; set; } = true;
        public int Id_empresa { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public bool EstaVigente =>
            Activo
            && (FechaDesde == null || FechaDesde <= DateTime.Now)
            && (FechaHasta == null || FechaHasta >= DateTime.Now);

        public Empresa? Empresa { get; set; }
        public Producto.Producto? Producto { get; set; }
        public Categoria? Categoria { get; set; }
        public ICollection<DescuentoMetodoPago> DescuentosMetodosPago { get; set; } = new List<DescuentoMetodoPago>();

        protected DescuentoConfiguracion() { }

        public static DescuentoConfiguracion Crear(
            string nombre,
            decimal valor,
            int idEmpresa,
            int? idProducto = null,
            int? idCategoria = null,
            bool aplicaCualquierMetodoPago = true,
            List<int>? idsMetodosPago = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es requerido.");
            if (valor <= 0 || valor > 100)
                throw new InvalidOperationException("El valor debe ser mayor a 0 y menor o igual a 100.");
            if (idEmpresa <= 0)
                throw new InvalidOperationException("El ID de empresa debe ser mayor a 0.");

            if (alcance == AlcanceDescuentoEnum.MetodoPago)
            {
                // Alcance MetodoPago: producto y categoria deben ser null
                if (idProducto.HasValue || idCategoria.HasValue)
                    throw new InvalidOperationException("Para descuentos por método de pago, no puede asignar producto o categoría.");
                if (aplicaCualquierMetodoPago)
                    throw new InvalidOperationException("Para descuentos por método de pago, no puede aplicar a cualquier método.");
                if (idsMetodosPago == null || idsMetodosPago.Count == 0)
                    throw new InvalidOperationException("Debe seleccionar al menos un método de pago.");
            }
            else
            {
                // Alcance Producto/Categoria: validación original
                if (idProducto.HasValue && idCategoria.HasValue)
                    throw new InvalidOperationException("No se puede asignar producto y categoría simultáneamente.");
                if (!idProducto.HasValue && !idCategoria.HasValue)
                    throw new InvalidOperationException("Se requiere un producto o categoría.");
                if (!aplicaCualquierMetodoPago && (idsMetodosPago == null || idsMetodosPago.Count == 0))
                    throw new InvalidOperationException("Debe indicar cualquier método o seleccionar al menos una tarjeta.");
            }

            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaHasta < fechaDesde)
                throw new InvalidOperationException("FechaHasta debe ser >= FechaDesde.");

            return new DescuentoConfiguracion
            {
                _nombre = nombre.Trim(),
                ModoDescuento = ModoDescuentoEnum.Porcentaje,
                Alcance = alcance,
                Valor = valor,
                Id_producto = idProducto,
                Id_categoria = idCategoria,
                AplicaCualquierMetodoPago = aplicaCualquierMetodoPago,
                Id_empresa = idEmpresa,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Activo = true,
                FechaAlta = DateTime.Now
            };
        }

        public void Actualizar(
            string nombre,
            decimal valor,
            int? idProducto,
            int? idCategoria,
            bool aplicaCualquierMetodoPago,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es requerido.");
            if (valor <= 0 || valor > 100)
                throw new InvalidOperationException("El valor debe ser mayor a 0 y menor o igual a 100.");

            if (alcance == AlcanceDescuentoEnum.MetodoPago)
            {
                if (idProducto.HasValue || idCategoria.HasValue)
                    throw new InvalidOperationException("Para descuentos por método de pago, no puede asignar producto o categoría.");
                if (aplicaCualquierMetodoPago)
                    throw new InvalidOperationException("Para descuentos por método de pago, no puede aplicar a cualquier método.");
            }
            else
            {
                if (idProducto.HasValue && idCategoria.HasValue)
                    throw new InvalidOperationException("No se puede asignar producto y categoría simultáneamente.");
                if (!idProducto.HasValue && !idCategoria.HasValue)
                    throw new InvalidOperationException("Se requiere un producto o categoría.");
            }

            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaHasta < fechaDesde)
                throw new InvalidOperationException("FechaHasta debe ser >= FechaDesde.");

            _nombre = nombre.Trim();
            Valor = valor;
            Id_producto = idProducto;
            Id_categoria = idCategoria;
            AplicaCualquierMetodoPago = aplicaCualquierMetodoPago;
            Alcance = alcance;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
        }
    }
}