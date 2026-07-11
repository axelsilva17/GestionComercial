using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Organizacion
{
    ///     /// Entidad Empresa con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var empresa = Empresa.Crear(nombre, cuit, direccion);
    public class Empresa : EntidadBase
    {
        private string _nombre = string.Empty;
        private string _cuit = string.Empty;
        private string _direccion = string.Empty;
        private string? _email;
        private string? _telefono;
        private string? _logoUrl;

        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public string CUIT 
        { 
            get => _cuit; 
            set => _cuit = value ?? string.Empty; 
        }
        public string Direccion 
        { 
            get => _direccion; 
            set => _direccion = value ?? string.Empty; 
        }
        public string? Email 
        { 
            get => _email; 
            set => _email = value; 
        }
        public string? Telefono 
        { 
            get => _telefono; 
            set => _telefono = value; 
        }
        public string? LogoUrl 
        { 
            get => _logoUrl; 
            set => _logoUrl = value; 
        }

        // ── Configuración ──
        private int _umbralStockCritico = 10;
        public int UmbralStockCritico
        {
            get => _umbralStockCritico;
            set => _umbralStockCritico = Math.Max(1, value);
        }

        // ── Relaciones ──
        public ICollection<Sucursal>   Sucursales  { get; set; } = new List<Sucursal>();
        public ICollection<Categoria>  Categorias  { get; set; } = new List<Categoria>();
        public ICollection<Cliente.Cliente>   Clientes    { get; set; } = new List<Cliente.Cliente>();
        public ICollection<Proveedor>  Proveedores { get; set; } = new List<Proveedor>();
        public ICollection<MetodoPago> MetodosPago { get; set; } = new List<MetodoPago>();
        public ICollection<Producto.Producto>  Productos   { get; set; } = new List<Producto.Producto>();

        // ── Constructor vacío (para EF Core) ──
        public Empresa() { }

        public static Empresa Crear(string nombre, string cuit, string direccion,
            string? email = null, string? telefono = null, string? logoUrl = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(cuit))
                throw new ArgumentException("El CUIT es requerido.", nameof(cuit));
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ArgumentException("La dirección es requerida.", nameof(direccion));

            return new Empresa
            {
                _nombre = nombre.Trim(),
                _cuit = cuit.Trim(),
                _direccion = direccion.Trim(),
                _email = email?.Trim().ToLower(),
                _telefono = telefono?.Trim(),
                _logoUrl = logoUrl?.Trim(),
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        ///         /// Actualiza datos de la empresa.
        public void Actualizar(string nombre, string direccion, string? email, string? telefono, string? logoUrl = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            _nombre = nombre.Trim();
            _direccion = direccion.Trim();
            _email = email?.Trim().ToLower();
            _telefono = telefono?.Trim();
            if (logoUrl != null) _logoUrl = logoUrl.Trim();
        }

        ///         /// Valida formato de CUIT (XX-XXXXXXXX-X).
        public bool CUITValido => !string.IsNullOrEmpty(_cuit) 
            && System.Text.RegularExpressions.Regex.IsMatch(_cuit, @"^\d{2}-\d{8}-\d{1}$");

        public int CantidadSucursales => Sucursales.Count;
        public int CantidadClientes => Clientes.Count;
        public int CantidadProveedores => Proveedores.Count;
        public int CantidadProductos => Productos.Count;
    }
}