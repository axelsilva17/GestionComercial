using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Usuarios;

namespace GestionComercial.UI.ViewModels.Main
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AutenticacionServicio _authServicio;
        private readonly IWindowManager        _windowManager;
        private readonly SesionServicio        _sesionServicio;
        private readonly DemoService?          _demoService;

        public LoginViewModel(AutenticacionServicio authServicio, IWindowManager windowManager,
            SesionServicio sesionServicio, DemoService? demoService = null)
        {
            _authServicio   = authServicio;
            _windowManager  = windowManager;
            _sesionServicio = sesionServicio;
            _demoService    = demoService;

            // Mostrar showcase si es la primera vez
            if (_demoService != null && _demoService.MostrarShowcasePendiente)
            {
                IniciarShowcase();
            }
        }

        private string _usuario = string.Empty;
        public string Usuario
        {
            get => _usuario;
            set { _usuario = value; if (!string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = string.Empty; NotifyOfPropertyChange(() => Usuario); NotifyOfPropertyChange(() => CanLoginCommand); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; if (!string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = string.Empty; NotifyOfPropertyChange(() => Password); NotifyOfPropertyChange(() => CanLoginCommand); }
        }

        private string _errorMessage = string.Empty;
        public new string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; NotifyOfPropertyChange(() => ErrorMessage); NotifyOfPropertyChange(() => ErrorVisible); }
        }

        public new bool ErrorVisible => !string.IsNullOrEmpty(ErrorMessage);
        public void SetPassword(string password) => Password = password;
        public bool CanLoginCommand =>
            !string.IsNullOrWhiteSpace(Usuario) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !IsLoading;

        // ── Showcase ───────────────────────────────────────────────────────
        private bool _mostrarShowcase;
        public bool MostrarShowcase
        {
            get => _mostrarShowcase;
            set { _mostrarShowcase = value; NotifyOfPropertyChange(() => MostrarShowcase); }
        }

        private FeatureShowcaseViewModel? _showcase;
        public FeatureShowcaseViewModel? Showcase
        {
            get => _showcase;
            set { _showcase = value; NotifyOfPropertyChange(() => Showcase); }
        }

        public void IniciarShowcase()
        {
            Showcase = new FeatureShowcaseViewModel();
            Showcase.CloseRequested += CerrarShowcase;
            MostrarShowcase = true;
        }

        public void CerrarShowcase()
        {
            MostrarShowcase = false;
            Showcase = null;
            _demoService?.MarcarShowcaseMostrado();
        }

        // ── Login ──────────────────────────────────────────────────────────
        public async Task LoginCommand()
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;
            NotifyOfPropertyChange(() => CanLoginCommand);
            try
            {
                var sesion = await _authServicio.LoginAsync(Usuario, Password);

                if (sesion == null)
                {
                    ErrorMessage = "Email o contraseña incorrectos.";
                    return;
                }

                _sesionServicio.IniciarSesion(sesion);

                var shell = IoC.Get<ShellViewModel>();
                shell.IdEmpresaActual  = sesion.IdEmpresa;
                shell.IdSucursalActual = sesion.IdSucursal;
                shell.SesionActual     = sesion;
                await shell.ConfigurarSesion(sesion.NombreCompleto, sesion.Rol, sesion.Sucursal, sesion);
                await _windowManager.ShowWindowAsync(shell);
                await TryCloseAsync();
            }
            catch (GestionComercial.Aplicacion.Excepciones.NegocioException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (System.Exception)
            {
                ErrorMessage = "Error al conectar con la base de datos. Intentá de nuevo.";
            }
            finally
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => CanLoginCommand);
            }
        }
    }
}
