using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Main
{
    public partial class DashboardView : UserControl
    {
        public DashboardView() => InitializeComponent();

        private ShellViewModel Shell => IoC.Get<ShellViewModel>();

        private void NuevaVenta_Click(object sender, RoutedEventArgs e)
            => Shell?.IrVentas();

        private void IrVentasCompleto_Click(object sender, RoutedEventArgs e)
            => Shell?.IrVentas();

        private void IrCaja_Click(object sender, RoutedEventArgs e)
            => Shell?.IrCaja();

        private void IrClientes_Click(object sender, RoutedEventArgs e)
            => Shell?.IrClientes();

        private void IrCompras_Click(object sender, RoutedEventArgs e)
            => Shell?.IrCompras();

        private void IrProductos_Click(object sender, RoutedEventArgs e)
            => Shell?.IrProductos();

        private void IrProductosStock_Click(object sender, RoutedEventArgs e)
            => Shell?.IrProductos();

        private void IrInventario_Click(object sender, RoutedEventArgs e)
            => Shell?.IrInventario();

        private void IrReportes_Click(object sender, RoutedEventArgs e)
            => Shell?.IrReportes();

        private void IrConfiguracion_Click(object sender, RoutedEventArgs e)
            => Shell?.IrConfiguracion();
    }
}
