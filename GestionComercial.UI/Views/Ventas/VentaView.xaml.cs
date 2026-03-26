using System;
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
                System.Diagnostics.Debug.WriteLine($"[VentaView] SelectionChanged fired!");
                System.Diagnostics.Debug.WriteLine($"[VentaView] Categorias count: {vm.Categorias?.Count ?? 0}");
                System.Diagnostics.Debug.WriteLine($"[VentaView] SelectedItem: {vm.CategoriaFiltro?.Nombre ?? "NULL"}");
                
                if (vm.Categorias == null || vm.Categorias.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[VentaView] ERROR: Categorias está vacío!");
                    return;
                }
                
                _ = vm.RefrescarProductosPorCategoriaAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[VentaView] ERROR: DataContext no es VentaViewModel, es: {DataContext?.GetType().Name}");
            }
        }

        /// <summary>
        /// Debug: cuando el ComboBox recibe foco.
        /// </summary>
        private void CategoriaFiltroCombo_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("[VentaView] CategoriaFiltroCombo_GotFocus!");
            if (DataContext is ViewModels.Ventas.VentaViewModel vm)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaView] GotFocus - Categorias count: {vm.Categorias?.Count ?? 0}");
                foreach (var cat in vm.Categorias)
                {
                    System.Diagnostics.Debug.WriteLine($"[VentaView]   - {cat.Nombre}");
                }
            }
        }

        /// <summary>
        /// Debug: cuando se abre el dropdown.
        /// </summary>
        private void CategoriaFiltroCombo_DropDownOpened(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("[VentaView] CategoriaFiltroCombo_DropDownOpened!");
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
            if (ViewModel == null) return; // Protección: DataContext puede no estar asignado aún

            if (sender is System.Windows.Controls.ComboBox combo && combo.SelectedItem is System.Windows.Controls.ComboBoxItem item)
            {
                // Convertir Tag string a int? para el filtro
                if (item.Tag is string tagString && int.TryParse(tagString, out int intValue))
                {
                    ViewModel.EstadoVentaFiltro = intValue;
                }
                else
                {
                    ViewModel.EstadoVentaFiltro = null;
                }
            }
            ViewModel?.FiltrarHistorialCommand?.Execute(null);
        }

        /// <summary>
        /// Maneja Enter en el filtro de DNI del historial.
        /// </summary>
        private void DniClienteFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel?.FiltrarHistorialCommand?.Execute(null);
                e.Handled = true;
            }
        }
    }
}
