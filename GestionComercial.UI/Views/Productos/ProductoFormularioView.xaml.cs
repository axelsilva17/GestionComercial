using System.Windows;
using System.Windows.Controls;
using GestionComercial.UI.ViewModels.Productos;

namespace GestionComercial.UI.Views.Productos
{
    public partial class ProductoFormularioView : UserControl
    {
        private ProductoFormularioViewModel ViewModel => DataContext as ProductoFormularioViewModel;

        public ProductoFormularioView()
        {
            InitializeComponent();
        }

        private async void Volver_Click(object sender, RoutedEventArgs e)
            => await ViewModel?.Volver();

        private void Guardar_Click(object sender, RoutedEventArgs e)
            => ViewModel?.Guardar();

        private async void Cancelar_Click(object sender, RoutedEventArgs e)
            => await ViewModel?.Volver();

        private void GenerarSKU_Click(object sender, RoutedEventArgs e)
            => ViewModel?.GenerarSKU();

        // Navega a ImportacionProductosViewModel
        private async void ImportarExcel_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.IrAImportacion();
        }
    }
}
