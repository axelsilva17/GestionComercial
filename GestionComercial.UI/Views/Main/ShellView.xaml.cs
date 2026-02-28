using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Windows.Input;

namespace GestionComercial.UI.Views.Main
{
    public partial class ShellView : Window
    {
        public ShellView() => InitializeComponent();

        // Arrastrar ventana sin borde
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => DragMove();

        private void Minimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void Maximize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;

        private void Close_Click(object sender, RoutedEventArgs e)
            => Close();

        // Botón Inventario alternativo para Administrador
        private async void IrInventario_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrInventario();
    }
}
