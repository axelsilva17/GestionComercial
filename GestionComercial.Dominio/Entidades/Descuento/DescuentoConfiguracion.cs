using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
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

        public TipoDescuentoEnum Tipo { get; set; }
        public ModoDescuentoEnum ModoDescuento { get; set; }
        public decimal Valor { get; set; }
        public int? Id_producto { get; set; }
        public int? Id_categoria { get; set; }
        public int? Id_metodoPago { get; set; }
        public int Id_empresa { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int Prioridad { get; set; }

        public bool EstaVigente =>
            Activo
            && (FechaDesde == null || FechaDesde <= DateTime.Now)
            && (FechaHasta == null || FechaHasta >= DateTime.Now);

        public Empresa? Empresa { get; set; }
        public Producto.Producto? Producto { get; set; }
        public Categoria? Categoria { get; set; }
        public MetodoPago? MetodoPago { get; set; }

        protected DescuentoConfiguracion() { }

        public static DescuentoConfiguracion Crear(
            string nombre,
            TipoDescuentoEnum tipo,
            decimal valor,
            int idEmpresa,
            int? idProducto = null,
            int? idCategoria = null,
            int? idMetodoPago = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int prioridad = 0)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es requerido.");
            if (valor <= 0 || valor > 100)
                throw new InvalidOperationException("El valor debe ser mayor a 0 y menor o igual a 100.");
            if (idEmpresa <= 0)
                throw new InvalidOperationException("El ID de empresa debe ser mayor a 0.");
            if (tipo == TipoDescuentoEnum.Producto && idProducto == null)
                throw new InvalidOperationException("Tipo Producto requiere Id_producto.");
            if (tipo == TipoDescuentoEnum.Categoria && idCategoria == null)
                throw new InvalidOperationException("Tipo Categoria requiere Id_categoria.");
            if (tipo == TipoDescuentoEnum.MetodoPago && idMetodoPago == null)
                throw new InvalidOperationException("Tipo MetodoPago requiere Id_metodoPago.");
            if (tipo != TipoDescuentoEnum.MetodoPago && idMetodoPago != null)
                throw new InvalidOperationException("Id_metodoPago solo puede ser asignado para Tipo MetodoPago.");
            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaHasta < fechaDesde)
                throw new InvalidOperationException("FechaHasta debe ser >= FechaDesde.");

            return new DescuentoConfiguracion
            {
                _nombre = nombre.Trim(),
                Tipo = tipo,
                ModoDescuento = ModoDescuentoEnum.Porcentaje,
                Valor = valor,
                Id_producto = idProducto,
                Id_categoria = idCategoria,
                Id_metodoPago = idMetodoPago,
                Id_empresa = idEmpresa,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Prioridad = prioridad,
                Activo = true,
                FechaAlta = DateTime.Now
            };
        }

        public void Actualizar(
            string nombre,
            TipoDescuentoEnum tipo,
            decimal valor,
            int? idProducto,
            int? idCategoria,
            int? idMetodoPago,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int prioridad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es requerido.");
            if (valor <= 0 || valor > 100)
                throw new InvalidOperationException("El valor debe ser mayor a 0 y menor o igual a 100.");
            if (tipo == TipoDescuentoEnum.Producto && idProducto == null)
                throw new InvalidOperationException("Tipo Producto requiere Id_producto.");
            if (tipo == TipoDescuentoEnum.Categoria && idCategoria == null)
                throw new InvalidOperationException("Tipo Categoria requiere Id_categoria.");
            if (tipo == TipoDescuentoEnum.MetodoPago && idMetodoPago == null)
                throw new InvalidOperationException("Tipo MetodoPago requiere Id_metodoPago.");
            if (tipo != TipoDescuentoEnum.MetodoPago && idMetodoPago != null)
                throw new InvalidOperationException("Id_metodoPago solo puede ser asignado para Tipo MetodoPago.");
            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaHasta < fechaDesde)
                throw new InvalidOperationException("FechaHasta debe ser >= FechaDesde.");

            _nombre = nombre.Trim();
            Tipo = tipo;
            Valor = valor;
            Id_producto = idProducto;
            Id_categoria = idCategoria;
            Id_metodoPago = idMetodoPago;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Prioridad = prioridad;
        }
    }
}
