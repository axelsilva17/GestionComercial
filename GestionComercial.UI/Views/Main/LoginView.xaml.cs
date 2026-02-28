using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Main
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => DragMove();
        }

        // Acceso tipado al ViewModel inyectado por Caliburn
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeIn");
            MainContainer.BeginStoryboard(storyboard);
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel?.SetPassword(PasswordBox.Password);
        }

        private void UsuarioBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
         
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.CanLoginCommand == true)
                _ = ViewModel.LoginCommand();
        }
    }
}