using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class PerfilViewModel : NavigableViewModel
    {
        private readonly AutenticacionServicio        _authServicio;
        private readonly RecuperacionContrasenaServicio _recuperacionServicio;

        public PerfilViewModel(
            AutenticacionServicio authServicio,
            RecuperacionContrasenaServicio recuperacionServicio)
        {
            _authServicio         = authServicio;
            _recuperacionServicio = recuperacionServicio;
        }

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

        // ── Panel pregunta secreta ────────────────────────────────────────────
        private bool   _panelPreguntaVisible;
        private string _preguntaSeleccionada = string.Empty;
        private string _respuestaPregunta    = string.Empty;
        private string _confirmarRespuesta   = string.Empty;

        public bool PanelPreguntaVisible
        {
            get => _panelPreguntaVisible;
            set { _panelPreguntaVisible = value; NotifyOfPropertyChange(() => PanelPreguntaVisible); }
        }

        public List<string> PreguntasDisponibles => PreguntasSecretas.Lista;

        public string PreguntaSeleccionada
        {
            get => _preguntaSeleccionada;
            set { _preguntaSeleccionada = value; NotifyOfPropertyChange(() => PreguntaSeleccionada); }
        }
        public string RespuestaPregunta
        {
            get => _respuestaPregunta;
            set { _respuestaPregunta = value; NotifyOfPropertyChange(() => RespuestaPregunta); }
        }
        public string ConfirmarRespuesta
        {
            get => _confirmarRespuesta;
            set { _confirmarRespuesta = value; NotifyOfPropertyChange(() => ConfirmarRespuesta); }
        }

        // Indica si el usuario ya tiene configurada la pregunta
        private bool _tienePreguntaConfigurada;
        public bool TienePreguntaConfigurada
        {
            get => _tienePreguntaConfigurada;
            set { _tienePreguntaConfigurada = value; NotifyOfPropertyChange(() => TienePreguntaConfigurada); NotifyOfPropertyChange(() => TextoBotonPregunta); }
        }
        public string TextoBotonPregunta => TienePreguntaConfigurada
            ? "Cambiar pregunta secreta"
            : "Configurar pregunta secreta";

        // ── Mensaje éxito / error ─────────────────────────────────────────────
        private string _mensajeExito = string.Empty;
        public string MensajeExito
        {
            get => _mensajeExito;
            set { _mensajeExito = value; NotifyOfPropertyChange(() => MensajeExito); NotifyOfPropertyChange(() => TieneExito); }
        }
        public bool TieneExito => !string.IsNullOrEmpty(MensajeExito);

        // ── Carga ─────────────────────────────────────────────────────────────
        public async Task CargarAsync(UsuarioSesionDto sesion)
        {
            Sesion = sesion;

            // Verificar si ya tiene pregunta configurada
            try
            {
                var pregunta = await _recuperacionServicio.ObtenerPreguntaAsync(sesion.Email);
                TienePreguntaConfigurada = !string.IsNullOrEmpty(pregunta);
            }
            catch
            {
                TienePreguntaConfigurada = false;
            }
        }

        // ── Datos personales ──────────────────────────────────────────────────
        public void AbrirEdicionDatos()
        {
            EditNombre        = Sesion.Nombre;
            EditApellido      = Sesion.Apellido;
            MensajeExito      = string.Empty;
            LimpiarError();
            CerrarTodosLosPaneles();
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
                await Task.Delay(300); // TODO: await _usuarioServicio.ActualizarDatosAsync(...)
                Sesion = new UsuarioSesionDto
                {
                    IdUsuario  = Sesion.IdUsuario,
                    Nombre     = EditNombre,
                    Apellido   = EditApellido,
                    Email      = Sesion.Email,
                    IdSucursal = Sesion.IdSucursal,
                    Sucursal   = Sesion.Sucursal,
                    IdEmpresa  = Sesion.IdEmpresa,
                    Empresa    = Sesion.Empresa,
                    Rol        = Sesion.Rol
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
            CerrarTodosLosPaneles();
            PanelPasswordVisible = true;
        }

        public void CerrarPanelPassword() => PanelPasswordVisible = false;

        public async Task GuardarPassword()
        {
            if (string.IsNullOrWhiteSpace(PassActual))  { MostrarError("Ingresá tu contraseña actual."); return; }
            if (string.IsNullOrWhiteSpace(PassNuevo))   { MostrarError("Ingresá la nueva contraseña."); return; }
            if (PassNuevo != PassConfirmar)             { MostrarError("Las contraseñas no coinciden."); return; }
            if (PassNuevo.Length < 8)                   { MostrarError("La contraseña debe tener al menos 8 caracteres."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                // Verificar contraseña actual
                var sesion = await _authServicio.LoginAsync(Sesion.Email, PassActual);
                if (sesion == null) { MostrarError("La contraseña actual es incorrecta."); return; }

                await _recuperacionServicio.CambiarContrasenaAsync(Sesion.Email, PassNuevo);
                MensajeExito         = "Contraseña actualizada correctamente.";
                PanelPasswordVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Pregunta secreta ──────────────────────────────────────────────────
        public void AbrirPreguntaSecreta()
        {
            PreguntaSeleccionada  = PreguntasDisponibles[0];
            RespuestaPregunta     = string.Empty;
            ConfirmarRespuesta    = string.Empty;
            MensajeExito          = string.Empty;
            LimpiarError();
            CerrarTodosLosPaneles();
            PanelPreguntaVisible  = true;
        }

        public void CerrarPanelPregunta() => PanelPreguntaVisible = false;

        public async Task GuardarPreguntaSecreta()
        {
            if (string.IsNullOrWhiteSpace(PreguntaSeleccionada)) { MostrarError("Seleccioná una pregunta."); return; }
            if (string.IsNullOrWhiteSpace(RespuestaPregunta))    { MostrarError("Ingresá tu respuesta."); return; }
            if (RespuestaPregunta != ConfirmarRespuesta)         { MostrarError("Las respuestas no coinciden."); return; }
            if (RespuestaPregunta.Length < 3)                    { MostrarError("La respuesta debe tener al menos 3 caracteres."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                await _recuperacionServicio.ConfigurarPreguntaAsync(
                    Sesion.Email,
                    PreguntaSeleccionada,
                    RespuestaPregunta);

                TienePreguntaConfigurada = true;
                MensajeExito             = "Pregunta secreta configurada correctamente.";
                PanelPreguntaVisible     = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private void CerrarTodosLosPaneles()
        {
            PanelDatosVisible     = false;
            PanelPasswordVisible  = false;
            PanelPreguntaVisible  = false;
        }
    }
}
