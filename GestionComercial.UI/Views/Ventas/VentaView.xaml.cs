using System.Windows;
using System.Windows.Input;
using GestionComercial.UI.ViewModels.Ventas;

namespace GestionComercial.UI.Views.Ventas
{
    public partial class VentaView : UserControl
    {
        public VentaView()
        {
            InitializeComponent();
        }

        private void VentaView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is not VentaViewModel vm) return;

            // Solo procesar si el foco NO está en un TextBox de edición
            // (permitir que el usuario escriba normalmente en el buscador)
            if (e.OriginalSource is System.Windows.Controls.TextBox)
            {
                // Permitir Escape siempre desde TextBox
                if (e.Key != Key.Escape) return;
            }

            vm.HandleKeyDown(e.Key, Keyboard.Modifiers);
            e.Handled = true;
        }
    }
}
