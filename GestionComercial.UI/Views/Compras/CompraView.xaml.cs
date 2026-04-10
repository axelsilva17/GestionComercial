using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Compras;

namespace GestionComercial.UI.Views.Compras
{
    public partial class CompraView : UserControl
    {
        private CompraViewModel? VM => DataContext as CompraViewModel;

        public CompraView() => InitializeComponent();

        private void BuscarProducto_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            VM?.BuscarProducto();
        }

        private void BusquedaProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VM?.BuscarProducto();
            }
        }
    }
}