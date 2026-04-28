using GestionComercial.Dominio.Entidades.Organizacion;

namespace GestionComercial.Dominio.Entidades.Seguridad
{
    /// <summary>
    /// Entidad Usuario con patrón DDD.
    /// 
    /// Preferir factory method Crear():
    ///   var usuario = Usuario.Crear(nombre, apellido, email, passwordHash, idSucursal, idRol);
    /// </summary>
    public class Usuario : EntidadBase
    {
        // ── Backing fields ──
        private string _nombre = string.Empty;
        private string _apellido = string.Empty;
        private string _email = string.Empty;
        private string _passwordHash = string.Empty;
        private string? _preguntaSecreta;
        private string? _respuestaHash;
        private int _intentosFallidos = 0;
        private DateTime? _bloqueadoHasta;
        private int _id_sucursal;
        private int _id_rol;

        // ── Propiedades con validación/encapsulamiento ──
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value ?? string.Empty; 
        }
        public string Apellido 
        { 
            get => _apellido; 
            set => _apellido = value ?? string.Empty; 
        }
        public string Email 
        { 
            get => _email; 
            set => _email = value?.ToLower().Trim() ?? string.Empty; 
        }
        public string PasswordHash 
        { 
            get => _passwordHash; 
            set => _passwordHash = value ?? string.Empty; 
        }
        public DateTime? UltimoAcceso { get; set; }
        
        public string? PreguntaSecreta 
        { 
            get => _preguntaSecreta; 
            set => _preguntaSecreta = value; 
        }
        public string? RespuestaHash 
        { 
            get => _respuestaHash; 
            set => _respuestaHash = value; 
        }
        public int IntentosFallidos 
        { 
            get => _intentosFallidos; 
            set => _intentosFallidos = value; 
        }
        public DateTime? BloqueadoHasta 
        { 
            get => _bloqueadoHasta; 
            set => _bloqueadoHasta = value; 
        }

        public int Id_sucursal { get => _id_sucursal; set => _id_sucursal = value; }
        public int Id_rol { get => _id_rol; set => _id_rol = value; }

        // ── Relaciones ──
        public Sucursal Sucursal { get; set; } = null!;
        public Rol      Rol      { get; set; } = null!;

        // ── Constructor vacío (para EF Core) ──
        public Usuario() { }

        // ── Factory method ──
        public static Usuario Crear(string nombre, string apellido, string email, string passwordHash,
            int idSucursal, int idRol, string? preguntaSecreta = null, string? respuestaHash = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es requerido.", nameof(apellido));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido.", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El password es requerido.", nameof(passwordHash));
            if (idSucursal <= 0)
                throw new ArgumentException("ID de sucursal inválido.", nameof(idSucursal));
            if (idRol <= 0)
                throw new ArgumentException("ID de rol inválido.", nameof(idRol));

            return new Usuario
            {
                _nombre = nombre.Trim(),
                _apellido = apellido.Trim(),
                _email = email.Trim().ToLower(),
                _passwordHash = passwordHash,
                _id_sucursal = idSucursal,
                _id_rol = idRol,
                _preguntaSecreta = preguntaSecreta,
                _respuestaHash = respuestaHash,
                _intentosFallidos = 0,
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }

        // ── Métodos de dominio ──

        /// <summary>
        /// Registra un intento de login exitoso.
        /// </summary>
        public void RegistrarAccesoExitoso()
        {
            _intentosFallidos = 0;
            _bloqueadoHasta = null;
            UltimoAcceso = DateTime.Now;
        }

        /// <summary>
        /// Registra un intento de login fallido. Bloquea tras 3 intentos.
        /// </summary>
        public void RegistrarAccesoFallido(int maxIntentos = 3, TimeSpan? duracionBloqueo = null)
        {
            _intentosFallidos++;
            
            if (_intentosFallidos >= maxIntentos)
            {
                _bloqueadoHasta = DateTime.Now + (duracionBloqueo ?? TimeSpan.FromMinutes(30));
            }
        }

        /// <summary>
        /// Desbloquea el usuario manualmente.
        /// </summary>
        public void Desbloquear()
        {
            _intentosFallidos = 0;
            _bloqueadoHasta = null;
        }

        /// <summary>
        /// Actualiza el password.
        /// </summary>
        public void ActualizarPassword(string nuevoPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(nuevoPasswordHash))
                throw new ArgumentException("El password no puede estar vacío.", nameof(nuevoPasswordHash));

            _passwordHash = nuevoPasswordHash;
            _intentosFallidos = 0;  // Resetear intentos al cambiar password
            _bloqueadoHasta = null;
        }

        /// <summary>
        /// Actualiza datos del usuario.
        /// </summary>
        public void Actualizar(string nombre, string apellido, string email, int idSucursal, int idRol)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es requerido.", nameof(apellido));

            _nombre = nombre.Trim();
            _apellido = apellido.Trim();
            _email = email.Trim().ToLower();
            _id_sucursal = idSucursal;
            _id_rol = idRol;
        }

        /// <summary>
        /// Inactiva el usuario (override con track de auditoría).
        /// </summary>
        public override void Inactivar()
        {
            // No permitir inactivarse a uno mismo si es el único activo
            // Esto se valida en el servicio, no aquí
            base.Inactivar();
            _bloqueadoHasta = null;  // Desbloquear al inactivar
        }

        // ── Propiedades computed ──
        public string NombreCompleto => $"{_nombre} {_apellido}".Trim();
        
        public string Inicial => string.IsNullOrEmpty(_nombre) ? "?" : _nombre[0].ToString().ToUpper();

        public bool EstaBloqueado => _bloqueadoHasta.HasValue && _bloqueadoHasta > DateTime.Now;

        public bool PuedeAcceder => Activo && !EstaBloqueado;
    }
}