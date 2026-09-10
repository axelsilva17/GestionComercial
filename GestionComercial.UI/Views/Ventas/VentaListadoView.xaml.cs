using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Ventas;

namespace GestionComercial.UI.Views.Ventas
{
    public partial class VentaListadoView : UserControl
    {
        public VentaListadoView()
        {
            InitializeComponent();
        }

        // Abre (o cierra, si se vuelve a seleccionar la misma) el drawer lateral de detalle.
        // Patrón espejo de ClienteListadoView: la selección del grid se reenvía al VM.
        private void Ventas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not VentaListadoViewModel vm) return;
            _ = vm.CargarDetalleVentaAsync(vm.VentaSeleccionada);
        }

        private void VentaListadoView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is not VentaListadoViewModel vm) return;

            // Solo procesar si el foco NO está en un TextBox/DatePicker
            if (e.OriginalSource is System.Windows.Controls.TextBox or System.Windows.Controls.DatePicker)
            {
                if (e.Key != Key.Escape && e.Key != Key.Enter) return;
            }

            vm.HandleKeyDown(e.Key, Keyboard.Modifiers);
            e.Handled = true;
        }
    }
}
