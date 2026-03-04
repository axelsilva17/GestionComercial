using GestionComercial.UI.ViewModel.Main;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Main
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

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
            if (Resources["FadeIn"] is System.Windows.Media.Animation.Storyboard sb)
                sb.Begin((FrameworkElement)sender);
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                // Mostrar texto plano
                PasswordVisible.Text       = PasswordBox.Password;
                PasswordBox.Visibility     = Visibility.Collapsed;
                PasswordVisible.Visibility = Visibility.Visible;
                TogglePassword.Content     = "●";
            }
            else
            {
                // Volver a ocultar
                PasswordBox.Visibility     = Visibility.Visible;
                PasswordVisible.Visibility = Visibility.Collapsed;
                TogglePassword.Content     = "○";
            }
        }
    }
}
