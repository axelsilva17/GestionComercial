using System.Windows;

namespace GestionComercial.UI.Views.Caja
{
    public partial class RegistrarIngresoEgresoWindow : Window
    {
        public RegistrarIngresoEgresoWindow()
        {
            InitializeComponent();
        }

        public RegistrarIngresoEgresoWindow(object dataContext) : this()
        {
            DialogContent.DataContext = dataContext;
        }
    }
}
