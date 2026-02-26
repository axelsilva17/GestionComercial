using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.UI.Controles
{
    public partial class CodigoBarraInput : UserControl
    {
        // ── DependencyProperties ─────────────────────────────────────────────

        public static readonly DependencyProperty CodigoProperty =
            DependencyProperty.Register(nameof(Codigo), typeof(string), typeof(CodigoBarraInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnCodigoChanged));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(CodigoBarraInput),
                new PropertyMetadata("Escanear o ingresar código..."));

        public string Codigo
        {
            get => (string)GetValue(CodigoProperty);
            set => SetValue(CodigoProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        // ── Eventos ──────────────────────────────────────────────────────────

        /// <summary>
        /// Se dispara cuando el usuario presiona Enter o Tab (lector de barras).
        /// </summary>
        public event RoutedEventHandler CodigoIngresado;

        public CodigoBarraInput()
        {
            InitializeComponent();
        }

        public void Enfocar() => TxtCodigo.Focus();

        public void Limpiar()
        {
            TxtCodigo.Clear();
            BtnLimpiar.Visibility = Visibility.Collapsed;
        }

        // ── Handlers ─────────────────────────────────────────────────────────

        private static void OnCodigoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (CodigoBarraInput)d;
            if (ctrl.TxtCodigo.Text != (string)e.NewValue)
                ctrl.TxtCodigo.Text = (string)e.NewValue ?? string.Empty;
        }

        private void TxtCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            Codigo = TxtCodigo.Text;
            BtnLimpiar.Visibility = string.IsNullOrEmpty(TxtCodigo.Text)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void TxtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            // Los lectores de código de barras terminan con Enter o Tab
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (!string.IsNullOrWhiteSpace(Codigo))
                    CodigoIngresado?.Invoke(this, new RoutedEventArgs());

                e.Handled = e.Key == Key.Enter;
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e) => Limpiar();
    }
}
