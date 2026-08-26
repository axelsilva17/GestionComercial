using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Ventas;

namespace GestionComercial.UI.Views.Ventas
{
    public partial class SeleccionClienteView : UserControl
    {
        public SeleccionClienteView()
        {
            InitializeComponent();
        }

        private void ClientesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not SeleccionClienteViewModel vm) return;
            if (vm.ClienteSeleccionado != null)
            {
                _ = vm.Confirmar();
            }
        }

        ///         /// Busca al presionar Enter en el campo de búsqueda.
        private void TextoBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is SeleccionClienteViewModel vm)
            {
                _ = vm.Buscar();
                e.Handled = true;
            }
        }

        private void ClientesDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (DataContext is not SeleccionClienteViewModel vm) return;
            if (vm.ClienteSeleccionado != null)
            {
                _ = vm.Confirmar();
                e.Handled = true;
            }
        }
    }
}
