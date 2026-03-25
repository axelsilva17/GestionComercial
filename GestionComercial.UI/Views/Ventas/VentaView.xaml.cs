using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Ventas;

namespace GestionComercial.UI.Views.Ventas
{
    public partial class VentaView : UserControl
    {
        private VentaViewModel ViewModel => DataContext as VentaViewModel;

        public VentaView()
        {
            InitializeComponent();
        }

        private void VentaView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is not VentaViewModel vm) return;

            // Solo procesar si el foco NO está en un TextBox de edición
            // (permitir que el usuario escriba normalmente en el buscador)
            if (e.OriginalSource is System.Windows.Controls.TextBox)
            {
                // Permitir Escape y Enter siempre desde TextBox (Enter para código de barras)
                if (e.Key != Key.Escape && e.Key != Key.Enter) return;
            }

            vm.HandleKeyDown(e.Key, Keyboard.Modifiers);
            e.Handled = true;
        }

        /// <summary>
        /// Maneja el doble click en un producto del popup de autocompletado.
        /// </summary>
        private void ResultadosBusquedaList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not VentaViewModel vm) return;

            if (sender is ListBox list && list.SelectedItem is GestionComercial.Aplicacion.DTOs.Productos.ProductoListadoDto producto)
            {
                vm.SeleccionarProductoDelPopup(producto);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Maneja la selección con Enter en el popup de autocompletado.
        /// </summary>
        private void ResultadosBusquedaList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (DataContext is not VentaViewModel vm) return;

            if (sender is ListBox list && list.SelectedItem is GestionComercial.Aplicacion.DTOs.Productos.ProductoListadoDto producto)
            {
                vm.SeleccionarProductoDelPopup(producto);
                e.Handled = true;
            }
        }

        private void VerHistorial_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.VerHistorialCommand?.Execute(null);
        }

        private void CerrarHistorial_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.CerrarHistorialCommand?.Execute(null);
        }

        /// <summary>
        /// Maneja Enter en el input de test de código de barras (Feature 7).
        /// </summary>
        private void TestBarcodeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ViewModel != null)
            {
                ViewModel.TestBarcodeCommand.Execute(null);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Maneja el cambio de selección en el ComboBox de categorías para auto-refrescar productos.
        /// </summary>
        private void CategoriaFiltroCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ViewModels.Ventas.VentaViewModel vm)
            {
                _ = vm.RefrescarProductosPorCategoriaAsync();
            }
        }

        /// <summary>
        /// Maneja el cambio de fecha "Desde" en los filtros del historial.
        /// </summary>
        private void FechaDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel?.FiltrarHistorialCommand?.Execute(null);
        }

        /// <summary>
        /// Maneja el cambio de fecha "Hasta" en los filtros del historial.
        /// </summary>
        private void FechaHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel?.FiltrarHistorialCommand?.Execute(null);
        }

        /// <summary>
        /// Maneja el cambio de estado en los filtros del historial.
        /// </summary>
        private void EstadoFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel?.FiltrarHistorialCommand?.Execute(null);
        }
    }
}
