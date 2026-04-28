using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Organizacion
{
    /// <summary>
    /// Entidad Empresa con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var empresa = Empresa.Crear(nombre, cuit, direccion);
    /// </summary>
    public class Empresa : EntidadBase
    {
        // ── Backing fields ──
        private string _nombre = string.Empty;
        private string _cuit = string.Empty;
        private string _direccion = string.Empty;
        private string? _email;
        private string? _telefono;

        // ── Propiedades con validación ──
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

        // ── Relaciones ──
        public ICollection<Sucursal>   Sucursales  { get; set; } = new List<Sucursal>();
        public ICollection<Categoria>  Categorias  { get; set; } = new List<Categoria>();
        public ICollection<Cliente.Cliente>   Clientes    { get; set; } = new List<Cliente.Cliente>();
        public ICollection<Proveedor>  Proveedores { get; set; } = new List<Proveedor>();
        public ICollection<MetodoPago> MetodosPago { get; set; } = new List<MetodoPago>();
        public ICollection<Producto.Producto>  Productos   { get; set; } = new List<Producto.Producto>();

        // ── Constructor vacío (para EF Core) ──
        public Empresa() { }

        // ── Factory method ──
        public static Empresa Crear(string nombre, string cuit, string direccion,
            string? email = null, string? telefono = null)
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
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Actualiza datos de la empresa.
        /// </summary>
        public void Actualizar(string nombre, string cuit, string direccion, string? email, string? telefono)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(cuit))
                throw new ArgumentException("El CUIT es requerido.", nameof(cuit));

            _nombre = nombre.Trim();
            _cuit = cuit.Trim();
            _direccion = direccion.Trim();
            _email = email?.Trim().ToLower();
            _telefono = telefono?.Trim();
        }

        /// <summary>
        /// Valida formato de CUIT (XX-XXXXXXXX-X).
        /// </summary>
        public bool CUITValido => !string.IsNullOrEmpty(_cuit) 
            && System.Text.RegularExpressions.Regex.IsMatch(_cuit, @"^\d{2}-\d{8}-\d{1}$");

        // ── Propiedades computed ──
        public int CantidadSucursales => Sucursales.Count;
        public int CantidadClientes => Clientes.Count;
        public int CantidadProveedores => Proveedores.Count;
        public int CantidadProductos => Productos.Count;
    }
}