using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Ventas;

namespace GestionComercial.Dominio.Entidades.Cliente
{
    /// <summary>
    /// Entidad Cliente con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var cliente = Cliente.Crear(nombre, documento, idEmpresa);
    /// </summary>
    public class Cliente : EntidadBase
    {
        // ── Backing fields ──
        private string _nombre = string.Empty;
        private int _documento;
        private string? _telefono;
        private string? _email;
        private int _id_empresa;

        // Nota: 'Activo' hereda de EntidadBase (backing field compartido)

        // ── Propiedades con validación ──
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public int Documento 
        { 
            get => _documento; 
            set => _documento = value >= 0 ? value : throw new ArgumentException("Documento inválido."); 
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
        public int Id_empresa { get => _id_empresa; set => _id_empresa = value; }

        // Nota: 'Activo' ya viene de EntidadBase con getter/setter

        // ── Relaciones ──
        public Empresa Empresa { get; set; } = null!;
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        // ── Constructor vacío (para EF Core) ──
        public Cliente() { }

        // ── Factory method ──
        public static Cliente Crear(string nombre, int documento, int idEmpresa, 
            string? telefono = null, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (documento < 0)
                throw new ArgumentException("El documento no puede ser negativo.", nameof(documento));
            if (idEmpresa <= 0)
                throw new ArgumentException("ID de empresa inválido.", nameof(idEmpresa));
            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                throw new ArgumentException("Email inválido.", nameof(email));

            return new Cliente
            {
                _nombre = nombre.Trim(),
                _documento = documento,
                _id_empresa = idEmpresa,
                _telefono = telefono?.Trim(),
                _email = email?.Trim().ToLower(),
                Activo = true,
                FechaAlta = DateTime.Now
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Actualiza datos del cliente. Solo si está activo.
        /// </summary>
        public void Actualizar(string nombre, int documento, string? telefono, string? email)
        {
            if (!Activo)
                throw new InvalidOperationException("No se puede modificar un cliente inactivo.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (documento <= 0)
                throw new ArgumentException("Documento inválido.", nameof(documento));

            _nombre = nombre.Trim();
            _documento = documento;
            _telefono = telefono?.Trim();
            
            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                throw new ArgumentException("Email inválido.", nameof(email));
            _email = email?.Trim().ToLower();
        }

        /// <summary>
        /// Inactiva el cliente (soft delete).
        /// </summary>
        public override void Inactivar()
        {
            if (!Activo) return;
            base.Inactivar();
        }

        public override void Reactivar()
        {
            if (Activo) return;
            base.Reactivar();
        }

        /// <summary>
        /// Busca una venta específica en el historial.
        /// </summary>
        public Venta? BuscarVenta(int idVenta)
            => Ventas.FirstOrDefault(v => v.Id == idVenta);

        /// <summary>
        /// Total de compras del cliente.
        /// </summary>
        public decimal TotalCompras => Ventas
            .Where(v => v.EsPagada)
            .Sum(v => v.TotalFinal);

        /// <summary>
        /// Cantidad de compras.
        /// </summary>
        public int CantidadCompras => Ventas
            .Where(v => v.EsPagada)
            .Count();

        // ── Propiedades computed ──
        public string Inicial => string.IsNullOrEmpty(_nombre) ? "?" : _nombre[0].ToString().ToUpper();
        
        public bool EsActivo => Activo;

        public string NombreDisplay => Activo 
            ? _nombre 
            : $"{_nombre} (Inactivo)";

        public bool EsNuevo => Ventas == null || !Ventas.Any();

        // ── Helpers privados ──
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
