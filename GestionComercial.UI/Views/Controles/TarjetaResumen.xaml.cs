using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestionComercial.UI.Controles
{
    /// <summary>
    /// Tarjeta KPI reutilizable. Uso en XAML:
    ///
    ///   <controles:TarjetaResumen
    ///       Icono="💰" Titulo="VENTAS"
    ///       Valor="$1.250.000"
    ///       Subtitulo="▲ 27.5% vs anterior"
    ///       AcentoFondo="#1A3B82F6"
    ///       ColorValor="{DynamicResource TextPrimaryBrush}"/>
    /// </summary>
    public partial class TarjetaResumen : UserControl
    {
        public static readonly DependencyProperty IconoProperty =
            DependencyProperty.Register(nameof(Icono), typeof(string), typeof(TarjetaResumen),
                new PropertyMetadata("📊"));

        public static readonly DependencyProperty TituloProperty =
            DependencyProperty.Register(nameof(Titulo), typeof(string), typeof(TarjetaResumen),
                new PropertyMetadata("TÍTULO"));

        public static readonly DependencyProperty ValorProperty =
            DependencyProperty.Register(nameof(Valor), typeof(string), typeof(TarjetaResumen),
                new PropertyMetadata("—"));

        public static readonly DependencyProperty SubtituloProperty =
            DependencyProperty.Register(nameof(Subtitulo), typeof(string), typeof(TarjetaResumen),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AcentoFondoProperty =
            DependencyProperty.Register(nameof(AcentoFondo), typeof(Brush), typeof(TarjetaResumen),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(26, 59, 130, 246))));

        public static readonly DependencyProperty ColorValorProperty =
            DependencyProperty.Register(nameof(ColorValor), typeof(Brush), typeof(TarjetaResumen),
                new PropertyMetadata(null));

        public string Icono
        {
            get => (string)GetValue(IconoProperty);
            set => SetValue(IconoProperty, value);
        }

        public string Titulo
        {
            get => (string)GetValue(TituloProperty);
            set => SetValue(TituloProperty, value);
        }

        public string Valor
        {
            get => (string)GetValue(ValorProperty);
            set => SetValue(ValorProperty, value);
        }

        public string Subtitulo
        {
            get => (string)GetValue(SubtituloProperty);
            set => SetValue(SubtituloProperty, value);
        }

        public Brush AcentoFondo
        {
            get => (Brush)GetValue(AcentoFondoProperty);
            set => SetValue(AcentoFondoProperty, value);
        }

        public Brush ColorValor
        {
            get => (Brush)GetValue(ColorValorProperty);
            set => SetValue(ColorValorProperty, value);
        }

        public TarjetaResumen()
        {
            InitializeComponent();

            // Default ColorValor al TextPrimaryBrush del tema si no se especifica
            Loaded += (s, e) =>
            {
                if (ColorValor == null && TryFindResource("TextPrimaryBrush") is Brush b)
                    ColorValor = b;
            };
        }
    }
}
