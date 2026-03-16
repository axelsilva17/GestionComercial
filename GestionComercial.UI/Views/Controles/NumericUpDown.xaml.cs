using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.UI.Controles
{
    public partial class NumericUpDown : UserControl
    {
        // ── DependencyProperties ─────────────────────────────────────────────

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(decimal), typeof(NumericUpDown),
                new PropertyMetadata(0m));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(decimal), typeof(NumericUpDown),
                new PropertyMetadata(999999m));

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(nameof(Step), typeof(decimal), typeof(NumericUpDown),
                new PropertyMetadata(1m));

        public static readonly DependencyProperty DecimalesProperty =
            DependencyProperty.Register(nameof(Decimales), typeof(int), typeof(NumericUpDown),
                new PropertyMetadata(0));

        // ── Propiedades ──────────────────────────────────────────────────────

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public decimal MinValue
        {
            get => (decimal)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public decimal MaxValue
        {
            get => (decimal)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public decimal Step
        {
            get => (decimal)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public int Decimales
        {
            get => (int)GetValue(DecimalesProperty);
            set => SetValue(DecimalesProperty, value);
        }

        // ── Evento de cambio de valor ────────────────────────────────────────
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged;

        public NumericUpDown()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)d;
            control.ValueChanged?.Invoke(control, new RoutedPropertyChangedEventArgs<decimal>(
                (decimal)e.OldValue, (decimal)e.NewValue));
        }

        // ── Handlers ─────────────────────────────────────────────────────────

        private void Incrementar_Click(object sender, RoutedEventArgs e)
        {
            var nuevo = Value + Step;
            Value = nuevo > MaxValue ? MaxValue : nuevo;
        }

        private void Decrementar_Click(object sender, RoutedEventArgs e)
        {
            var nuevo = Value - Step;
            Value = nuevo < MinValue ? MinValue : nuevo;
        }

        // Solo permitir dígitos y coma/punto decimal
        private void TxtValor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Decimales == 0)
                e.Handled = !int.TryParse(e.Text, out _);
            else
                e.Handled = e.Text != "." && e.Text != "," && !int.TryParse(e.Text, out _);
        }

        private void TxtValor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxtValor.Text.Replace(',', '.'), out var val))
                val = MinValue;

            val = val < MinValue ? MinValue : val > MaxValue ? MaxValue : val;
            Value = Decimales > 0 ? decimal.Round(val, Decimales) : decimal.Round(val, 0);
        }
    }
}
