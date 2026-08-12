using GestionComercial.Aplicacion.DTOs.Usuarios;

namespace GestionComercial.Aplicacion.Servicios
{
    ///     /// Servicio singleton que mantiene los datos del usuario logueado en memoria.
    /// Se popula en el LoginViewModel tras autenticación exitosa y se inyecta
    /// en cualquier ViewModel que necesite saber quién está logueado.
    public class SesionServicio
    {
        private UsuarioSesionDto _sesion = new();

        public int    IdUsuario   => _sesion.IdUsuario;
        public int    IdSucursal  => _sesion.IdSucursal;
        public int    IdEmpresa   => _sesion.IdEmpresa;
        public string Rol         => _sesion.Rol;
        public string Nombre      => _sesion.NombreCompleto;

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
    }
}
