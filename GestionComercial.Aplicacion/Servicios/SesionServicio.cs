using GestionComercial.Aplicacion.DTOs.Usuarios;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio singleton que mantiene los datos del usuario logueado en memoria.
    /// Se popula en el LoginViewModel tras autenticación exitosa y se inyecta
    /// en cualquier ViewModel que necesite saber quién está logueado.
    /// </summary>
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

        public void IniciarSesion(UsuarioSesionDto sesion)
        {
            _sesion      = sesion;
            IdCajaActual = null;
        }

        public void CerrarSesion()
        {
            _sesion      = new();
            IdCajaActual = null;
        }

        public UsuarioSesionDto ObtenerSesion() => _sesion;
    }
}
