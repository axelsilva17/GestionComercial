using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;

namespace GestionComercial.UI.ViewModel.Main
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AutenticacionServicio _authServicio;
        private readonly IWindowManager _windowManager;

        public LoginViewModel(AutenticacionServicio authServicio, IWindowManager windowManager)
        {
            _authServicio = authServicio;
            _windowManager = windowManager;
        }

        // ── Propiedades ───────────────────────────────────────────────────────
        private string _usuario = string.Empty;
        public string Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                NotifyOfPropertyChange(() => Usuario);
                NotifyOfPropertyChange(() => CanLoginCommand);
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLoginCommand);
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => ErrorVisible);
            }
        }

        public bool ErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

        // Llamado desde el code-behind cuando cambia el PasswordBox
        public void SetPassword(string password)
        {
            Password = password;
        }

        // Caliburn usa esto para habilitar/deshabilitar el botón LoginCommand
        public bool CanLoginCommand =>
            !string.IsNullOrWhiteSpace(Usuario) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !IsLoading;

        // ── Login ─────────────────────────────────────────────────────────────
        public async Task LoginCommand()
        {
            IsLoading = true;
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

                var shell = IoC.Get<ShellViewModel>();
                shell.UsuarioNombre = sesion.NombreCompleto;
                shell.UsuarioRol = sesion.Rol;
                shell.UsuarioSucursal = sesion.Sucursal;
                shell.IdEmpresaActual = sesion.IdEmpresa;
                shell.IdSucursalActual = sesion.IdSucursal;

                await _windowManager.ShowWindowAsync(shell);
                await TryCloseAsync();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error al conectar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => CanLoginCommand);
            }
        }
    }
}