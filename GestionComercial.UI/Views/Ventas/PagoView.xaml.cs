using System;
using System.Linq;
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

        /// <summary>
        /// Maneja el clic en el botón "Ver Historial" para mostrar el popup de historial.
        /// </summary>
        private void VerHistorial_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PagoViewModel vm)
            {
                // Cargar historial y mostrar popup
                _ = vm.CargarHistorialAsync();
                vm.MostrarHistorial = true;
            }
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

        /// <summary>
        /// Valida que solo se permitan dígitos y separadores decimales (coma/punto).
        /// Esto evita que caracteres inválidos entren en el campo de monto.
        /// </summary>
        private void MontoIngresado_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return text.All(c => char.IsDigit(c) || c == ',' || c == '.');
        }

        /// <summary>
        /// Maneja KeyDown en el campo de monto - solo agrega pago si es Enter.
        /// </summary>
        private void MontoIngresado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is PagoViewModel vm)
            {
                vm.AgregarPago();
                e.Handled = true;
            }
        }
    }
}
