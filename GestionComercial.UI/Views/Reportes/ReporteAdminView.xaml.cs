using GestionComercial.UI.ViewModels.Reportes;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Main.Reportes
{
    public partial class ReporteAdminView : UserControl
    {
        private ReporteAdminViewModel VM => DataContext as ReporteAdminViewModel;
        public ReporteAdminView() => InitializeComponent();
        private async void Actualizar_Click(object sender, RoutedEventArgs e)
            => await VM?.Actualizar();
    }
}
