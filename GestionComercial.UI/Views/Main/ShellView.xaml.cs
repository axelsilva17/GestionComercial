using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Main
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FadeIn");
            var border = sender as System.Windows.Controls.Border;
            border?.BeginStoryboard(storyboard);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.CerrarSesion();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}