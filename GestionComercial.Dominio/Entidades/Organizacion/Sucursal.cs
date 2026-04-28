using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Organizacion
{
    /// <summary>
    /// Entidad Sucursal con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var sucursal = Sucursal.Crear(nombre, direccion, idEmpresa);
    /// </summary>
    public class Sucursal : EntidadBase
    {
        // ── Backing fields ──
        private string _nombre = string.Empty;
        private string _direccion = string.Empty;
        private string? _telefono;
        private int _id_empresa;

        // ── Propiedades con validación ──
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public string Direccion 
        { 
            get => _direccion; 
            set => _direccion = value ?? string.Empty; 
        }
        public string? Telefono 
        { 
            get => _telefono; 
            set => _telefono = value; 
        }
        public int Id_empresa { get => _id_empresa; set => _id_empresa = value; }

        // ── Relaciones ──
        public Empresa              Empresa  { get; set; } = null!;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Caja.Caja>    Cajas    { get; set; } = new List<Caja.Caja>();
        public ICollection<Venta>   Ventas   { get; set; } = new List<Venta>();
        public ICollection<Compra>  Compras  { get; set; } = new List<Compra>();

        // ── Constructor vacío (para EF Core) ──
        public Sucursal() { }

        // ── Factory method ──
        public static Sucursal Crear(string nombre, string direccion, int idEmpresa, string? telefono = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ArgumentException("La dirección es requerida.", nameof(direccion));
            if (idEmpresa <= 0)
                throw new ArgumentException("ID de empresa inválido.", nameof(idEmpresa));

            return new Sucursal
            {
                _nombre = nombre.Trim(),
                _direccion = direccion.Trim(),
                _id_empresa = idEmpresa,
                _telefono = telefono?.Trim(),
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Actualiza datos de la sucursal.
        /// </summary>
        public void Actualizar(string nombre, string direccion, string? telefono)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            _nombre = nombre.Trim();
            _direccion = direccion.Trim();
            _telefono = telefono?.Trim();
        }

        // ── Propiedades computed ──
        public int CantidadUsuarios => Usuarios.Count;
        public int CantidadCajas => Cajas.Count;
        public int CantidadVentas => Ventas.Count;
        public string Display => $"{_nombre} ({Direccion})";
    }
}