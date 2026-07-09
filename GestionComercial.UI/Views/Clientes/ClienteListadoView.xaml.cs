using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.UI.ViewModels.Clientes;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.ViewModels.Ventas;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Clientes
{
    public partial class ClienteListadoView : UserControl
    {
        private ClienteListadoViewModel VM => DataContext as ClienteListadoViewModel;

        public ClienteListadoView() => InitializeComponent();

        private async void NuevoCliente_Click(object sender, RoutedEventArgs e) => await VM?.NuevoCliente();

        private async void TextoBusqueda_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && VM != null)
                await VM.Buscar();
        }

        private void CerrarDetalle_Click(object sender, RoutedEventArgs e)
            => VM?.CerrarDetalle();

        private async void EditarCliente_Click(object sender, RoutedEventArgs e)
            => await VM?.EditarCliente();

        private async void DesactivarCliente_Click(object sender, RoutedEventArgs e)
            => await VM?.DesactivarCliente();

        private async void VerVentas_Click(object sender, RoutedEventArgs e)
        {
            if (VM?.ClienteSeleccionado == null) return;
            var shell = IoC.Get<ShellViewModel>();
            var listado = IoC.Get<VentaListadoViewModel>();
            listado.ClienteId = VM.ClienteSeleccionado.IdCliente;
            listado.ClienteNombre = VM.ClienteSeleccionado.Nombre;
            listado.FechaDesde = DateTime.Today.AddYears(-1);
            listado.FechaHasta = DateTime.Now;
            await shell.ActivateItemAsync(listado, CancellationToken.None);
        }

        private async void PaginaAnterior_Click(object sender, RoutedEventArgs e)
            => await VM?.PaginaAnterior();

        private async void PaginaSiguiente_Click(object sender, RoutedEventArgs e)
            => await VM?.PaginaSiguiente();

        private void Clientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM == null) return;
            VM.ClienteSeleccionado = (sender as DataGrid)?.SelectedItem as ClienteDto;
        }
    }
}
