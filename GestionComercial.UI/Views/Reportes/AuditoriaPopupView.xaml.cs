using GestionComercial.UI.ViewModels.Reportes;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Reportes
{
    public partial class AuditoriaPopupView : UserControl
    {
        private AuditoriaPopupViewModel VM => DataContext as AuditoriaPopupViewModel;
        public AuditoriaPopupView() => InitializeComponent();
    }
}
