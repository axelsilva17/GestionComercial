using GestionComercial.UI.ViewModels.Clientes;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Clientes
{
    public partial class ClienteFormularioView : UserControl
    {
        private ClienteFormularioViewModel VM => DataContext as ClienteFormularioViewModel;

        public ClienteFormularioView() => InitializeComponent();

        private async void Volver_Click(object sender, RoutedEventArgs e)
            => await VM?.Volver();

        private void Guardar_Click(object sender, RoutedEventArgs e)
            => VM?.Guardar();

        private async void Cancelar_Click(object sender, RoutedEventArgs e)
            => await VM?.Volver();
    }
}
