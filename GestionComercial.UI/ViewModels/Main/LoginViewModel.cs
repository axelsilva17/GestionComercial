using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Usuarios;

namespace GestionComercial.UI.ViewModel.Main
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AutenticacionServicio _authServicio;
        private readonly IWindowManager        _windowManager;
        private readonly SesionServicio        _sesionServicio;  // <-- campo declarado

        public LoginViewModel(AutenticacionServicio authServicio, IWindowManager windowManager, SesionServicio sesionServicio)
        {
            _authServicio   = authServicio;
            _windowManager  = windowManager;
            _sesionServicio = sesionServicio;
        }

        private string _usuario = string.Empty;
        public string Usuario
        {
            get => _usuario;
            set { _usuario = value; NotifyOfPropertyChange(() => Usuario); NotifyOfPropertyChange(() => CanLoginCommand); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; NotifyOfPropertyChange(() => Password); NotifyOfPropertyChange(() => CanLoginCommand); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; NotifyOfPropertyChange(() => ErrorMessage); NotifyOfPropertyChange(() => ErrorVisible); }
        }

        public bool ErrorVisible => !string.IsNullOrEmpty(ErrorMessage);
        public void SetPassword(string password) => Password = password;
        public bool CanLoginCommand =>
            !string.IsNullOrWhiteSpace(Usuario) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !IsLoading;

        public async Task LoginCommand()
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;
            NotifyOfPropertyChange(() => CanLoginCommand);
            try
            {
                var sesion = await _authServicio.LoginAsync(Usuario, Password);

                if (sesion == null)  // <-- verificar ANTES de IniciarSesion
                {
                    ErrorMessage = "Email o contraseña incorrectos.";
                    return;
                }

                _sesionServicio.IniciarSesion(sesion);  // <-- después de verificar

                var shell = IoC.Get<ShellViewModel>();
                shell.IdEmpresaActual  = sesion.IdEmpresa;
                shell.IdSucursalActual = sesion.IdSucursal;
                shell.SesionActual     = sesion;
                shell.ConfigurarSesion(sesion.NombreCompleto, sesion.Rol, sesion.Sucursal, sesion);
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
