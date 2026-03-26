using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Controls
{
    public partial class HistorialControl : UserControl
    {
        public HistorialControl()
        {
            InitializeComponent();
        }

        private void CerrarHistorial_Click(object sender, RoutedEventArgs e)
        {
            // Try to find a CerrarHistorialCommand on the DataContext
            if (DataContext is ViewModels.Ventas.VentaViewModel vm)
            {
                vm.CerrarHistorialCommand?.Execute(null);
            }
        }

        private void FechaDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ViewModels.Ventas.VentaViewModel vm)
            {
                vm.FiltrarHistorialCommand?.Execute(null);
            }
        }

        private void FechaHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ViewModels.Ventas.VentaViewModel vm)
            {
                vm.FiltrarHistorialCommand?.Execute(null);
            }
        }

        private void EstadoFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ViewModels.Ventas.VentaViewModel vm)
            {
                if (sender is ComboBox combo && combo.SelectedItem is ComboBoxItem item)
                {
                    if (item.Tag is string tagString && int.TryParse(tagString, out int intValue))
                    {
                        vm.EstadoVentaFiltro = intValue;
                    }
                    else
                    {
                        vm.EstadoVentaFiltro = null;
                    }
                }
                vm.FiltrarHistorialCommand?.Execute(null);
            }
        }

        private void DniClienteFiltro_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (DataContext is ViewModels.Ventas.VentaViewModel vm)
                {
                    vm.FiltrarHistorialCommand?.Execute(null);
                }
                e.Handled = true;
            }
        }
    }
}