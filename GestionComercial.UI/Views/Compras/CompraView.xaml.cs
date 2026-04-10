using System.Windows.Controls;
using GestionComercial.UI.ViewModels.Compras;

namespace GestionComercial.UI.Views.Compras
{
    public partial class CompraView : UserControl
    {
        public CompraView()
        {
            InitializeComponent();
        }

        private void BuscarProducto_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is CompraViewModel vm)
            {
                vm.BuscarProducto();
            }
        }
    }
}