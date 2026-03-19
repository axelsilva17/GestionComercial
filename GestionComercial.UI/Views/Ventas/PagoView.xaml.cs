using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Ventas;

namespace GestionComercial.UI.Views.Ventas
{
    public partial class PagoView : UserControl
    {
        public PagoView()
        {
            InitializeComponent();
        }

        private void PagoView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is not PagoViewModel vm) return;

            // Solo procesar si el foco NO está en el TextBox de monto
            if (e.OriginalSource is System.Windows.Controls.TextBox)
            {
                if (e.Key != Key.Escape && e.Key != Key.Enter) return;
            }

            vm.HandleKeyDown(e.Key, Keyboard.Modifiers);
            e.Handled = true;
        }
    }
}
