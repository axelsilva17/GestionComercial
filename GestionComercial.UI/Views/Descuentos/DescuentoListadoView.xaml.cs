using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.Aplicacion.DTOs.Descuentos;
using GestionComercial.UI.ViewModels.Descuentos;

namespace GestionComercial.UI.Views.Descuentos
{
    public partial class DescuentoListadoView : UserControl
    {
        public DescuentoListadoView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DescuentoListadoViewModel vm && sender is DataGrid grid && grid.SelectedItem is DescuentoListadoDto item)
            {
                vm.EditarDescuento(item);
            }
        }
    }
}
