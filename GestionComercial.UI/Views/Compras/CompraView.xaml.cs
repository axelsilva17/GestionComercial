using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Compras;

namespace GestionComercial.UI.Views.Compras
{
    public partial class CompraView : UserControl
    {
        private CompraViewModel? VM => DataContext as CompraViewModel;

        public CompraView() => InitializeComponent();

        private async void BuscarProducto_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await VM?.BuscarProductoAsync();
            
            // Si hay exactamente 1 producto, agregarlo automáticamente
            if (VM?.ProductosEncontrados.Count == 1)
            {
                VM.ProductoSeleccionado = VM.ProductosEncontrados[0];
                VM.AgregarProducto();
            }
        }

        private async void BusquedaProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await VM?.BuscarProductoAsync();
                
                // Si hay exactamente 1 producto, agregarlo automáticamente
                if (VM?.ProductosEncontrados.Count == 1)
                {
                    VM.ProductoSeleccionado = VM.ProductosEncontrados[0];
                    VM.AgregarProducto();
                }
            }
        }

        private void ResultadosBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (VM?.ProductoSeleccionado != null)
            {
                VM.AgregarProducto();
            }
        }
    }
}