using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.UI.ViewModels.Base;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class PerfilViewModel : NavigableViewModel
    {
        // ── Datos del usuario logueado ────────────────────────────────────────
        private UsuarioSesionDto _sesion = new();
        public UsuarioSesionDto Sesion
        {
            get => _sesion;
            set { _sesion = value; NotifyOfPropertyChange(() => Sesion); }
        }

        // ── Panel datos personales ────────────────────────────────────────────
        private bool   _panelDatosVisible;
        private string _editNombre   = string.Empty;
        private string _editApellido = string.Empty;

        public bool PanelDatosVisible
        {
            get => _panelDatosVisible;
            set { _panelDatosVisible = value; NotifyOfPropertyChange(() => PanelDatosVisible); }
        }
        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public string EditApellido
        {
            get => _editApellido;
            set { _editApellido = value; NotifyOfPropertyChange(() => EditApellido); }
        }

        // ── Panel cambiar contraseña ──────────────────────────────────────────
        private bool   _panelPasswordVisible;
        private string _passActual    = string.Empty;
        private string _passNuevo     = string.Empty;
        private string _passConfirmar = string.Empty;

        public bool PanelPasswordVisible
        {
            get => _panelPasswordVisible;
            set { _panelPasswordVisible = value; NotifyOfPropertyChange(() => PanelPasswordVisible); }
        }
        public string PassActual
        {
            get => _passActual;
            set { _passActual = value; NotifyOfPropertyChange(() => PassActual); }
        }
        public string PassNuevo
        {
            get => _passNuevo;
            set { _passNuevo = value; NotifyOfPropertyChange(() => PassNuevo); }
        }
        public string PassConfirmar
        {
            get => _passConfirmar;
            set { _passConfirmar = value; NotifyOfPropertyChange(() => PassConfirmar); }
        }

        // ── Mensaje éxito ─────────────────────────────────────────────────────
        private string _mensajeExito = string.Empty;
        public string MensajeExito
        {
            get => _mensajeExito;
            set { _mensajeExito = value; NotifyOfPropertyChange(() => MensajeExito); NotifyOfPropertyChange(() => TieneExito); }
        }
        public bool TieneExito => !string.IsNullOrEmpty(MensajeExito);

        public async Task CargarAsync()
        {
            await Task.Delay(100);
            // TODO: obtener del servicio de sesión actual
            Sesion = new UsuarioSesionDto
            {
                IdUsuario      = 1,
                Nombre         = "Juan",
                Apellido       = "García",
                IdSucursal     = 1,
                Sucursal = "Casa Central",
                IdEmpresa      = 1,
                Empresa  = "Mi Empresa S.R.L.",
                Rol     = "Administrador"
            };
        }

        // ── Datos personales ──────────────────────────────────────────────────
        public void AbrirEdicionDatos()
        {
            EditNombre        = Sesion.Nombre;
            EditApellido      = Sesion.Apellido;
            MensajeExito      = string.Empty;
            LimpiarError();
            PanelDatosVisible = true;
        }

        public void CerrarPanelDatos() => PanelDatosVisible = false;

        public async Task GuardarDatos()
        {
            if (string.IsNullOrWhiteSpace(EditNombre))   { MostrarError("El nombre es obligatorio.");   return; }
            if (string.IsNullOrWhiteSpace(EditApellido)) { MostrarError("El apellido es obligatorio."); return; }
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(300); // TODO: await servicio
                Sesion = new UsuarioSesionDto
                {
                    IdUsuario      = Sesion.IdUsuario,
                    Nombre         = EditNombre,
                    Apellido       = EditApellido,
                    IdSucursal     = Sesion.IdSucursal,
                    Sucursal = Sesion.Sucursal,
                    IdEmpresa      = Sesion.IdEmpresa,
                    Empresa  = Sesion.Empresa,
                    Rol     = Sesion.Rol
                };
                MensajeExito      = "Datos actualizados correctamente.";
                PanelDatosVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Contraseña ────────────────────────────────────────────────────────
        public void AbrirCambioPassword()
        {
            PassActual           = string.Empty;
            PassNuevo            = string.Empty;
            PassConfirmar        = string.Empty;
            MensajeExito         = string.Empty;
            LimpiarError();
            PanelPasswordVisible = true;
        }

        public void CerrarPanelPassword() => PanelPasswordVisible = false;

        public async Task GuardarPassword()
        {
            if (string.IsNullOrWhiteSpace(PassActual))    { MostrarError("Ingresá tu contraseña actual."); return; }
            if (string.IsNullOrWhiteSpace(PassNuevo))     { MostrarError("Ingresá la nueva contraseña."); return; }
            if (PassNuevo != PassConfirmar)               { MostrarError("Las contraseñas no coinciden."); return; }
            if (PassNuevo.Length < 6)                     { MostrarError("La contraseña debe tener al menos 6 caracteres."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(300); // TODO: await servicio.CambiarPassword(...)
                MensajeExito         = "Contraseña actualizada correctamente.";
                PanelPasswordVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
