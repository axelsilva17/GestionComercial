using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Organizacion;

namespace GestionComercial.Dominio.Entidades.Proveedores
{
    public class Proveedor : EntidadBase
    {
        private string _nombre = string.Empty;
        private string? _telefono;
        private string? _email;
        private string? _cuit;
        private int _id_empresa;

        // ── Propiedades con validación ──
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public string? Telefono 
        { 
            get => _telefono; 
            set => _telefono = value; 
        }
        public string? Email 
        { 
            get => _email; 
            set => _email = value; 
        }
        public string? CUIT 
        { 
            get => _cuit; 
            set => _cuit = value; 
        }
        public int Id_empresa { get => _id_empresa; set => _id_empresa = value; }

        // ── Relaciones ──
        public Empresa             Empresa  { get; set; } = null!;
        public ICollection<Compra> Compras  { get; set; } = new List<Compra>();

        // ── Constructor vacío (para EF Core) ──
        public Proveedor() { }

        // ── Factory method ──
        public static Proveedor Crear(string nombre, int idEmpresa, 
            string? telefono = null, string? email = null, string? cuit = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (idEmpresa <= 0)
                throw new ArgumentException("ID de empresa inválido.", nameof(idEmpresa));

            return new Proveedor
            {
                _nombre = nombre.Trim(),
                _id_empresa = idEmpresa,
                _telefono = telefono?.Trim(),
                _email = email?.Trim().ToLower(),
                _cuit = cuit?.Trim(),
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Actualiza datos del proveedor.
        /// </summary>
        public void Actualizar(string nombre, string? telefono, string? email, string? cuit)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            _nombre = nombre.Trim();
            _telefono = telefono?.Trim();
            _email = email?.Trim().ToLower();
            _cuit = cuit?.Trim();
        }

        /// <summary>
        /// Valida formato de CUIT (XX-XXXXXXXX-X).
        /// </summary>
        public bool CUITValido => string.IsNullOrEmpty(_cuit) 
            ? false 
            : System.Text.RegularExpressions.Regex.IsMatch(_cuit, @"^\d{2}-\d{8}-\d{1}$");

        // ── Propiedades computed ──
        public string NombreDisplay => Activo ? _nombre : $"{_nombre} (Inactivo)";
        public int CantidadCompras => Compras.Count;
    }
}
