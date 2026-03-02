using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Main
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AutenticacionServicio _authServicio;
        private readonly IWindowManager        _windowManager;

        public LoginViewModel(AutenticacionServicio authServicio, IWindowManager windowManager)
        {
            _authServicio  = authServicio;
            _windowManager = windowManager;
        }

        // ── Propiedades ───────────────────────────────────────────────────────
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; NotifyOfPropertyChange(() => Email); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; NotifyOfPropertyChange(() => Password); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; NotifyOfPropertyChange(() => ErrorMessage); NotifyOfPropertyChange(() => TieneError); }
        }

        public bool TieneError => !string.IsNullOrEmpty(ErrorMessage);

        // ── Login ─────────────────────────────────────────────────────────────
        public async Task Ingresar()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Ingresá email y contraseña.";
                return;
            }

            IsLoading    = true;
            ErrorMessage = string.Empty;

            try
            {
                var sesion = await _authServicio.LoginAsync(Email, Password);

                if (sesion == null)
                {
                    ErrorMessage = "Email o contraseña incorrectos.";
                    return;
                }

                // Abrir ShellViewModel con los datos de sesión
                var shell = IoC.Get<ShellViewModel>();
                shell.UsuarioNombre   = sesion.NombreCompleto;
                shell.UsuarioRol      = sesion.Rol;
                shell.UsuarioSucursal = sesion.Sucursal;
                shell.IdEmpresaActual = sesion.IdEmpresa;
                shell.IdSucursalActual = sesion.IdSucursal;

                await _windowManager.ShowWindowAsync(shell);

                // Cerrar ventana de login
                await TryCloseAsync();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error al conectar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
