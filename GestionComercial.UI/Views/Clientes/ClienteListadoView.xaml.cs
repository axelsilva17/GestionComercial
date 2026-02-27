using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.UI.ViewModels.Clientes;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Clientes
{
    public partial class ClienteListadoView : UserControl
    {
        private ClienteListadoViewModel VM => DataContext as ClienteListadoViewModel;

        public ClienteListadoView() => InitializeComponent();

        private async void NuevoCliente_Click(object sender, RoutedEventArgs e)
            => await VM?.NuevoCliente();

        private async void Buscar_Click(object sender, RoutedEventArgs e)
            => await VM?.Buscar();

        private void CerrarDetalle_Click(object sender, RoutedEventArgs e)
            => VM?.CerrarDetalle();

        private async void EditarCliente_Click(object sender, RoutedEventArgs e)
            => await VM?.EditarCliente();

        private async void DesactivarCliente_Click(object sender, RoutedEventArgs e)
            => await VM?.DesactivarCliente();

        private void VerVentas_Click(object sender, RoutedEventArgs e)
        {
            // TODO: navegar a ventas filtrado por cliente seleccionado
        }

        private async void PaginaAnterior_Click(object sender, RoutedEventArgs e)
            => await VM?.PaginaAnterior();

        private async void PaginaSiguiente_Click(object sender, RoutedEventArgs e)
            => await VM?.PaginaSiguiente();

        private void Clientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM == null) return;
            VM.ClienteSeleccionado = (sender as DataGrid)?.SelectedItem as ClienteItemDto;
        }
    }
}
