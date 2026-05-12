using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Producto
{
    /// <summary>
    /// Unidad de medida para productos.
    /// </summary>
    public class UnidadMedida
    {
        // ── Backing fields ──
        private string _nombre = string.Empty;
        private string _abreviatura = string.Empty;

        // ── Propiedades con validación ──
        public int Id { get; set; }  // Para EF Core

        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public string Abreviatura 
        { 
            get => _abreviatura; 
            set => _abreviatura = value ?? string.Empty; 
        }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

        // ── Constructor vacío (para EF Core) ──
        public UnidadMedida() { }

        // ── Factory method ──
        public static UnidadMedida Crear(string nombre, string abreviatura)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(abreviatura))
                throw new ArgumentException("La abreviatura es requerida.", nameof(abreviatura));

            return new UnidadMedida
            {
                _nombre = nombre.Trim(),
                _abreviatura = abreviatura.Trim().ToUpper()
            };
        }

        /// <summary>
        /// Actualiza datos.
        /// </summary>
        public void Actualizar(string nombre, string abreviatura)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(abreviatura))
                throw new ArgumentException("La abreviatura es requerida.", nameof(abreviatura));

            _nombre = nombre.Trim();
            _abreviatura = abreviatura.Trim().ToUpper();
        }

        public string Display => $"{_nombre} ({_abreviatura})";
    }

    /// <summary>
    /// Categoría de productos con soporte para jerarquía.
    /// </summary>
    public class Categoria : EntidadBase
    {
        // ── Backing fields ──
        private string _nombre = string.Empty;
        private int? _categoriaPadre_id;
        private int _id_empresa;

        // ── Propiedades con validación ──
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public int? CategoriaPadre_id 
        { 
            get => _categoriaPadre_id; 
            set => _categoriaPadre_id = value; 
        }
        public int Id_empresa { get => _id_empresa; set => _id_empresa = value; }

        public Categoria?             CategoriaPadre { get; set; }
        public ICollection<Categoria> SubCategorias  { get; set; } = new List<Categoria>();
        public Empresa                Empresa        { get; set; } = null!;
        public ICollection<Producto>  Productos      { get; set; } = new List<Producto>();

        // ── Constructor vacío (para EF Core) ──
        public Categoria() { }

        // ── Factory method ──
        public static Categoria Crear(string nombre, int idEmpresa, int? categoriaPadreId = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (idEmpresa <= 0)
                throw new ArgumentException("ID de empresa inválido.", nameof(idEmpresa));

            return new Categoria
            {
                _nombre = nombre.Trim(),
                _id_empresa = idEmpresa,
                _categoriaPadre_id = categoriaPadreId,
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        /// <summary>
        /// Actualiza nombre y categoría padre.
        /// </summary>
        public void Actualizar(string nombre, int? categoriaPadreId = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            _nombre = nombre.Trim();
            _categoriaPadre_id = categoriaPadreId;
        }

        /// <summary>
        /// Verifica si es categoría raíz (sin padre).
        /// </summary>
        public bool EsRaiz => !_categoriaPadre_id.HasValue;

        /// <summary>
        /// Profundidad en la jerarquía.
        /// </summary>
        public int Nivel => CategoriaPadre != null ? CategoriaPadre.Nivel + 1 : 0;

        public int CantidadProductos => Productos.Count;
        public int CantidadSubcategorias => SubCategorias.Count;
    }

    /// <summary>
    /// Entidad Producto con patrón DDD.
    /// 
    /// Preferir factory method Crear() en vez de new Producto():
    ///   var producto = Producto.Crear(nombre, precioVenta, precioCosto, ...);
    /// </summary>
    public class Producto : EntidadBase
    {
        // ── Propiedades con backing fields para encapsulamiento ──
        private string _nombre = string.Empty;
        private string? _codigoBarra;
        private string? _descripcion;
        private decimal _precioVentaActual;
        private decimal _precioCostoActual;
        private decimal _stockActual;
        private decimal _stockMinimo;
        private int _id_empresa;
        private int _id_categoria;
        private int _id_unidadMedida;

        // ── Propiedades públicas con getter/setter ──
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public string? CodigoBarra 
        { 
            get => _codigoBarra; 
            set => _codigoBarra = value; 
        }
        public string? Descripcion 
        { 
            get => _descripcion; 
            set => _descripcion = value; 
        }
        public decimal PrecioVentaActual 
        { 
            get => _precioVentaActual; 
            set => _precioVentaActual = value >= 0 ? value : 0; 
        }
        public decimal PrecioCostoActual 
        { 
            get => _precioCostoActual; 
            set => _precioCostoActual = value >= 0 ? value : 0; 
        }
        public decimal StockActual 
        { 
            get => _stockActual; 
            set => _stockActual = value >= 0 ? value : 0; 
        }
        public decimal StockMinimo 
        { 
            get => _stockMinimo; 
            set => _stockMinimo = value >= 0 ? value : 0; 
        }
        public int Id_empresa { get => _id_empresa; set => _id_empresa = value; }
        public int Id_categoria { get => _id_categoria; set => _id_categoria = value; }
        public int Id_unidadMedida { get => _id_unidadMedida; set => _id_unidadMedida = value; }

        // ── Relaciones ──
        public Empresa      Empresa      { get; set; } = null!;
        public Categoria    Categoria    { get; set; } = null!;
        public UnidadMedida UnidadMedida { get; set; } = null!;

        public ICollection<VentaDetalle>    VentaDetalles  { get; set; } = new List<VentaDetalle>();
        public ICollection<CompraDetalle>   CompraDetalles { get; set; } = new List<CompraDetalle>();
        public ICollection<MovimientoStock> Movimientos    { get; set; } = new List<MovimientoStock>();

        // ── Constructor vacío (para EF Core) ──
        public Producto() { }

        // ── Constructor con parámetros (reemplazado por factory method Crear) ──
        public Producto(string nombre, decimal precioVenta, decimal precioCosto, 
            int idEmpresa, int idCategoria, int idUnidadMedida)
        {
            _nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            _precioVentaActual = precioVenta >= 0 ? precioVenta : 0;
            _precioCostoActual = precioCosto >= 0 ? precioCosto : 0;
            _id_empresa = idEmpresa;
            _id_categoria = idCategoria;
            _id_unidadMedida = idUnidadMedida;
            FechaAlta = DateTime.Now;
            Activo = true;
        }

        // ── Factory method estático (forma preferida) ──
        public static Producto Crear(
            string nombre,
            decimal precioVenta,
            decimal precioCosto,
            int idEmpresa,
            int idCategoria,
            int idUnidadMedida,
            string? codigoBarra = null,
            string? descripcion = null,
            decimal stockMinimo = 10)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (idEmpresa <= 0)
                throw new ArgumentException("ID de empresa inválido.", nameof(idEmpresa));
            if (idCategoria <= 0)
                throw new ArgumentException("ID de categoría inválido.", nameof(idCategoria));
            if (idUnidadMedida <= 0)
                throw new ArgumentException("ID de unidad de medida inválido.", nameof(idUnidadMedida));

            return new Producto(nombre, precioVenta, precioCosto, idEmpresa, idCategoria, idUnidadMedida)
            {
                CodigoBarra = codigoBarra,
                Descripcion = descripcion,
                StockMinimo = stockMinimo
            };
        }

        // ── Propiedades de dominio (computed) ──
        public bool StockBajo => _stockActual <= _stockMinimo;
        
        public decimal Margen => _precioVentaActual > 0
            ? (_precioVentaActual - _precioCostoActual) / _precioVentaActual * 100
            : 0;

        // ── Métodos de dominio ──
        
        /// <summary>
        /// Descuenta stock de forma atómica. Lanza si no hay suficiente.
        /// </summary>
        public void DescontarStock(decimal cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
            if (_stockActual < cantidad)
                throw new InvalidOperationException(
                    $"Stock insuficiente. Actual: {_stockActual}, solicitado: {cantidad}");
            _stockActual -= cantidad;
        }

        /// <summary>
        /// Agrega stock.
        /// </summary>
        public void AgregarStock(decimal cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(cantidad));
            _stockActual += cantidad;
        }

        /// <summary>
        /// Actualiza precios con validación.
        /// </summary>
        public void ActualizarPrecios(decimal nuevoPrecioVenta, decimal nuevoPrecioCosto)
        {
            if (nuevoPrecioVenta < 0)
                throw new ArgumentException("El precio de venta no puede ser negativo.", nameof(nuevoPrecioVenta));
            if (nuevoPrecioCosto < 0)
                throw new ArgumentException("El precio de costo no puede ser negativo.", nameof(nuevoPrecioCosto));
            
            _precioVentaActual = nuevoPrecioVenta;
            _precioCostoActual = nuevoPrecioCosto;
        }

        /// <summary>
        /// Inactiva el producto (soft delete).
        /// </summary>
        public override void Inactivar() => Activo = false;

        /// <summary>
        /// Reactiva el producto.
        /// </summary>
        public override void Reactivar() => Activo = true;
    }
}