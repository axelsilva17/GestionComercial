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
