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
            _shell         = shell;
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

        // =====================================================================
        // CREDENCIALES DE PRUEBA
        // usuario: gerente       password: gerente123
        // usuario: administrador password: admin123
        // usuario: vendedor      password: venta123
        // =====================================================================
        public async Task LoginCommand()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(500); // simula llamada al servidor

                var (ok, nombre, rol) = (Usuario?.ToLower(), _passwordValue) switch
                {
                    ("gerente",       "gerente123") => (true,  "Carlos Rodriguez", "Gerente"),
                    ("administrador", "admin123")   => (true,  "Maria Gonzalez",   "Administrador"),
                    ("vendedor",      "venta123")   => (true,  "Juan Perez",       "Vendedor"),
                    _                               => (false, "",                 ""),
                };

                if (ok)
                {
                    _shell.ConfigurarSesion(nombre, rol, "Casa Central");
                    await _windowManager.ShowWindowAsync(_shell);
                    await TryCloseAsync();
                }
                else
                {
                    MostrarError("Usuario o contrasena incorrectos.");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al iniciar sesion: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
