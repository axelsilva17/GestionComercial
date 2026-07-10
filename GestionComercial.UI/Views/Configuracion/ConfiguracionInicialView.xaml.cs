using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Configuracion;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Configuracion
{
    public partial class ConfiguracionInicialView : Window
    {
        public ConfiguracionInicialView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        private void PasswordAdminBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConfiguracionInicialViewModel vm)
                vm.SetPasswordAdmin(((PasswordBox)sender).Password);
        }

        private void PasswordGerenteBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConfiguracionInicialViewModel vm)
                vm.SetPasswordGerente(((PasswordBox)sender).Password);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["FadeIn"] is Storyboard sb)
                sb.Begin((FrameworkElement)sender);
        }
    }
}
