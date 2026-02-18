using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Main
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IWindowManager _windowManager;
        private readonly ShellViewModel _shell;
        private string _usuario;
        private string _passwordValue;

        public LoginViewModel(IWindowManager windowManager, ShellViewModel shell)
        {
            _windowManager = windowManager;
            _shell = shell;
        }

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

        public void SetPassword(string password)
        {
            _passwordValue = password;
            NotifyOfPropertyChange(() => CanLoginCommand);
        }

        public bool CanLoginCommand =>
            !string.IsNullOrWhiteSpace(Usuario) &&
            !string.IsNullOrWhiteSpace(_passwordValue) &&
            !IsLoading;

        public async Task LoginCommand()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(500);
                if (Usuario == "admin" && _passwordValue == "admin")
                {
                    await _windowManager.ShowWindowAsync(_shell);
                    await TryCloseAsync();
                }
                else
                {
                    MostrarError("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al iniciar sesión: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
