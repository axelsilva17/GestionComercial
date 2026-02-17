using System.Windows;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Main
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            
            // Permitir arrastrar la ventana
            MouseLeftButtonDown += (s, e) => DragMove();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Ejecutar animación de entrada
            var storyboard = (Storyboard)FindResource("FadeIn");
            MainContainer.BeginStoryboard(storyboard);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Ocultar error cuando el usuario empieza a escribir
            ErrorContainer.Visibility = Visibility.Collapsed;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string usuario = UsuarioBox.Text.Trim();
            string password = PasswordBox.Password;

            // Validación básica
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MostrarError("Por favor completá todos los campos");
                return;
            }

            // TODO: Aquí va tu lógica de autenticación
            // Por ahora, validación de ejemplo
            if (usuario == "admin" && password == "admin")
            {
                // Login exitoso - abrir ShellView
                var shellView = new ShellView();
                shellView.Show();
                this.Close();
            }
            else
            {
                MostrarError("Usuario o contraseña incorrectos");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MostrarError(string mensaje)
        {
            ErrorText.Text = mensaje;
            ErrorContainer.Visibility = Visibility.Visible;
            
            // Animar el mensaje de error
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            ErrorContainer.BeginAnimation(OpacityProperty, fadeIn);
        }
    }
}
