using GestionComercial.UI.ViewModels.Reportes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.UI.Views.Reportes
{
    public partial class ReporteAdminView : UserControl
    {
        private ReporteAdminViewModel VM => DataContext as ReporteAdminViewModel;
        public ReporteAdminView() => InitializeComponent();
        private async void Actualizar_Click(object sender, RoutedEventArgs e)
            => await VM?.Actualizar();

        private void OverlayBackdrop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VM?.OcultarAuditoria();
        }
    }
}
