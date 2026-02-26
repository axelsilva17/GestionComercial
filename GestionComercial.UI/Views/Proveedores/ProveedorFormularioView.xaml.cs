using GestionComercial.UI.ViewModels.Proveedores;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Proveedores
{
    public partial class ProveedorFormularioView : UserControl
    {
        private ProveedorFormularioViewModel VM => DataContext as ProveedorFormularioViewModel;

        public ProveedorFormularioView() => InitializeComponent();

        private async void Volver_Click(object sender, RoutedEventArgs e)
            => await VM?.Volver();

        private void Guardar_Click(object sender, RoutedEventArgs e)
            => VM?.Guardar();

        private async void Cancelar_Click(object sender, RoutedEventArgs e)
            => await VM?.Volver();
    }
}
