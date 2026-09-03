using GestionComercial.Aplicacion.DTOs;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    ///     /// Servicio singleton que mantiene los datos del usuario logueado en memoria.
    /// Se popula en el LoginViewModel tras autenticación exitosa y se inyecta
    /// en cualquier ViewModel que necesite saber quién está logueado.
    public class SesionServicio
    {
        private readonly IUnitOfWork? _uow;
        private readonly DevCredencialesConfig? _devCreds;
        private UsuarioSesionDto _sesion = new();

        public SesionServicio(IUnitOfWork? uow = null, DevCredencialesConfig? devCreds = null)
        {
            _uow = uow;
            _devCreds = devCreds;
        }

        public int    IdUsuario   => _sesion.IdUsuario;
        public int    IdSucursal  => _sesion.IdSucursal;
        public int    IdEmpresa   => _sesion.IdEmpresa;
        public string Rol         => _sesion.Rol;
        public string Nombre      => _sesion.NombreCompleto;
        public bool   EsDesarrollador => !string.IsNullOrEmpty(_devCreds?.Email) && _sesion?.Email == _devCreds.Email;

        // IdCaja se setea cuando se abre caja
        public int? IdCajaActual { get; set; }

        // Turno se setea junto con IdCajaActual al abrir caja
        public string? TurnoActual { get; set; }

        /// <summary>
        /// Verifica si el usuario logueado tiene un permiso específico (por código).
        /// </summary>
        public bool HasPermission(string codigoPermiso)
            => _sesion.Permisos?.Contains(codigoPermiso) == true;

        public void IniciarSesion(UsuarioSesionDto sesion)
        {
            _sesion      = sesion;
            IdCajaActual = null;
            TurnoActual  = null;
        }

        public void CerrarSesion()
        {
            _sesion      = new();
            IdCajaActual = null;
            TurnoActual  = null;
        }

        public UsuarioSesionDto ObtenerSesion() => _sesion;

        /// <summary>
        /// Recarga los permisos del usuario logueado desde la base de datos.
        /// Se usa cuando los permisos de su rol cambian en caliente.
        /// </summary>
        public async Task RecargarPermisosAsync()
        {
            // Sin acceso a la BD no hay nada que recargar
            if (_uow == null) return;
            await RecargarPermisosAsync(_uow);
        }

        /// <summary>
        /// Recarga los permisos del usuario logueado usando el IUnitOfWork del llamador.
        /// Garantiza un contexto fresco aunque el singleton no tenga (o conserve uno
        /// viejo) el IUnitOfWork inyectado en el constructor.
        /// </summary>
        public async Task RecargarPermisosAsync(IUnitOfWork uow)
        {
            // Sin usuario real (ej: modo desarrollador) no hay nada que recargar
            if (uow == null || _sesion.IdUsuario <= 0) return;

            var permisos = await uow.Usuarios.ObtenerPermisosAsync(_sesion.IdUsuario);
            _sesion.Permisos = new HashSet<string>(permisos);

            System.Diagnostics.Debug.WriteLine(
                $"[Sesion] Permisos recargados para {_sesion.Email}: {string.Join(", ", permisos)}");
        }
    }
}
