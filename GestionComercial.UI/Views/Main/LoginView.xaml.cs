using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModel.Main;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Main
{
    public partial class LoginView : Window
    {
        private readonly RecuperacionContrasenaServicio _recuperacionServicio;
        private string _emailRecuperacion = string.Empty;

        public LoginView()
        {
            InitializeComponent();
            _recuperacionServicio = IoC.Get<RecuperacionContrasenaServicio>();
        }

        // ── Handlers login ────────────────────────────────────────────────────
        private void CloseButton_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.SetPassword(((PasswordBox)sender).Password);
        }

        private void UsuarioBox_TextChanged(object sender, TextChangedEventArgs e) { }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["FadeIn"] is Storyboard sb)
                sb.Begin((FrameworkElement)sender);
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordVisible.Text       = PasswordBox.Password;
                PasswordBox.Visibility     = Visibility.Collapsed;
                PasswordVisible.Visibility = Visibility.Visible;
                TogglePassword.Content     = "●";
            }
            else
            {
                PasswordBox.Visibility     = Visibility.Visible;
                PasswordVisible.Visibility = Visibility.Collapsed;
                TogglePassword.Content     = "○";
            }
        }

        // ── Navegación entre paneles ──────────────────────────────────────────
        private void OlvideContrasena_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => MostrarPanel(PanelRecupEmail);

        private void VolverAlLogin_Click(object sender, RoutedEventArgs e)
            => MostrarPanel(PanelLogin);

        private void VolverAEmail_Click(object sender, RoutedEventArgs e)
            => MostrarPanel(PanelRecupEmail);

        private void MostrarPanel(StackPanel panel)
        {
            PanelLogin.Visibility              = Visibility.Collapsed;
            PanelRecupEmail.Visibility         = Visibility.Collapsed;
            PanelRecupPregunta.Visibility      = Visibility.Collapsed;
            PanelRecupNuevaContrasena.Visibility = Visibility.Collapsed;

            panel.Visibility = Visibility.Visible;
            panel.Opacity    = 0;

            if (Resources["SlideIn"] is Storyboard sb)
                sb.Begin(panel);
        }

        // ── Paso 1: Verificar email ───────────────────────────────────────────
        private async void RecupEmail_Continuar(object sender, RoutedEventArgs e)
        {
            ErrorRecupEmail.Visibility = Visibility.Collapsed;
            var email = RecupEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MostrarError(ErrorRecupEmail, ErrorRecupEmailText, "Ingresá tu email.");
                return;
            }

            try
            {
                var pregunta = await _recuperacionServicio.ObtenerPreguntaAsync(email);
                _emailRecuperacion  = email;
                TextPregunta.Text   = pregunta;
                RecupRespuesta.Clear();
                MostrarPanel(PanelRecupPregunta);
            }
            catch (Exception ex)
            {
                MostrarError(ErrorRecupEmail, ErrorRecupEmailText, ex.Message);
            }
        }

        // ── Paso 2: Verificar respuesta ───────────────────────────────────────
        private async void RecupPregunta_Verificar(object sender, RoutedEventArgs e)
        {
            ErrorRecupPregunta.Visibility = Visibility.Collapsed;
            var respuesta = RecupRespuesta.Password;

            if (string.IsNullOrEmpty(respuesta))
            {
                MostrarError(ErrorRecupPregunta, ErrorRecupPreguntaText, "Ingresá tu respuesta.");
                return;
            }

            try
            {
                await _recuperacionServicio.ValidarRespuestaAsync(_emailRecuperacion, respuesta);
                NuevaContrasena.Clear();
                ConfirmarContrasena.Clear();
                MostrarPanel(PanelRecupNuevaContrasena);
            }
            catch (Exception ex)
            {
                MostrarError(ErrorRecupPregunta, ErrorRecupPreguntaText, ex.Message);
            }
        }

        // ── Paso 3: Cambiar contraseña ────────────────────────────────────────
        private async void CambiarContrasena_Click(object sender, RoutedEventArgs e)
        {
            ErrorNuevaContrasena.Visibility = Visibility.Collapsed;

            var nueva     = NuevaContrasena.Password;
            var confirmar = ConfirmarContrasena.Password;

            if (string.IsNullOrEmpty(nueva))
            {
                MostrarError(ErrorNuevaContrasena, ErrorNuevaContrasenaText, "Ingresá la nueva contraseña.");
                return;
            }

            if (nueva != confirmar)
            {
                MostrarError(ErrorNuevaContrasena, ErrorNuevaContrasenaText, "Las contraseñas no coinciden.");
                return;
            }

            if (nueva.Length < 8)
            {
                MostrarError(ErrorNuevaContrasena, ErrorNuevaContrasenaText, "La contraseña debe tener al menos 8 caracteres.");
                return;
            }

            try
            {
                await _recuperacionServicio.CambiarContrasenaAsync(_emailRecuperacion, nueva);
                MessageBox.Show("¡Contraseña actualizada correctamente! Ya podés iniciar sesión.",
                    "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                MostrarPanel(PanelLogin);
            }
            catch (Exception ex)
            {
                MostrarError(ErrorNuevaContrasena, ErrorNuevaContrasenaText, ex.Message);
            }
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static void MostrarError(Border border, TextBlock text, string mensaje)
        {
            text.Text          = mensaje;
            border.Visibility  = Visibility.Visible;
        }
    }
}
