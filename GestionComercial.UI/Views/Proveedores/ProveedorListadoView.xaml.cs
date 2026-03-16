using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.UI.ViewModels.Proveedores;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Proveedores
{
    public partial class ProveedorListadoView : UserControl
    {
        private ProveedorListadoViewModel VM => DataContext as ProveedorListadoViewModel;

        public ProveedorListadoView() => InitializeComponent();

        private async void NuevoProveedor_Click(object sender, RoutedEventArgs e)
            => await VM?.NuevoProveedor();

        private async void Buscar_Click(object sender, RoutedEventArgs e)
            => await VM?.Buscar();

        private void CerrarDetalle_Click(object sender, RoutedEventArgs e)
            => VM?.CerrarDetalle();

        private async void EditarProveedor_Click(object sender, RoutedEventArgs e)
            => await VM?.EditarProveedor();

        private async void DesactivarProveedor_Click(object sender, RoutedEventArgs e)
            => await VM?.DesactivarProveedor();

        private void VerCompras_Click(object sender, RoutedEventArgs e)
        {
            // TODO: navegar a compras filtrado por proveedor seleccionado
        }

        private async void PaginaAnterior_Click(object sender, RoutedEventArgs e)
            => await VM?.PaginaAnterior();

        private async void PaginaSiguiente_Click(object sender, RoutedEventArgs e)
            => await VM?.PaginaSiguiente();

        private void Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM == null) return;
            VM.ProveedorSeleccionado = (sender as DataGrid)?.SelectedItem as ProveedorItemDto;
        }
    }
}
