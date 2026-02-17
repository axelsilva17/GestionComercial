using Caliburn.Micro;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Main
{
    public class LoginViewModel : Screen
    {
        private readonly ShellViewModel _shell;
        private string _usuario;
        private string _password;

        public LoginViewModel(ShellViewModel shell)
        {
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

        public bool CanLoginCommand
        {
            get => !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Password);
        }

        public async Task LoginCommand()
        {
            // Aqui ira tu logica de autenticacion
            // Por ahora solo mostramos un mensaje

            if (Usuario == "admin" && Password == "admin")
            {
                MessageBox.Show("Login exitoso!", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Aqui navegarias a la vista principal
                // await _shell.ActivateItemAsync(new MainViewModel());
            }
            else
            {
                MessageBox.Show("Usuario o contrasena incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}