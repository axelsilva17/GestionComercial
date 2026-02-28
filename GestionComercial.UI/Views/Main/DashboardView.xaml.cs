using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Main
{
    public partial class DashboardView : UserControl
    {
        private DashboardViewModel VM => DataContext as DashboardViewModel;

        public DashboardView() => InitializeComponent();

        private async void NuevaVenta_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrVentas();

        private async void IrCaja_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrCaja();

        private async void IrClientes_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrClientes();

        private async void IrCompras_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrCompras();

        private async void IrReportes_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrReportes();

        private async void IrProductos_Click(object sender, RoutedEventArgs e)
            => await IoC.Get<ShellViewModel>().IrProductos();

        private void IrVentasCompleto_Click(object sender, RoutedEventArgs e)
            => VM?.IrVentasCompleto();

        private void IrProductosStock_Click(object sender, RoutedEventArgs e)
            => VM?.IrProductosStock();
    }
}
