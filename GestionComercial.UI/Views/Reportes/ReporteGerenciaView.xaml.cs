using GestionComercial.UI.ViewModels.Reportes;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Main.Reportes
{
    public partial class ReporteGerenciaView : UserControl
    {
        private ReporteGerenciaViewModel VM => DataContext as ReporteGerenciaViewModel;
        public ReporteGerenciaView() => InitializeComponent();
        private async void Actualizar_Click(object sender, RoutedEventArgs e)
            => await VM?.Actualizar();
    }
}
