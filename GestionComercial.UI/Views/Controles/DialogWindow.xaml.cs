using GestionComercial.UI.Views.Servicios;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestionComercial.UI.Views.Controles
{
    public partial class DialogWindow : Window
    {
        public string Titulo
        {
            get => Title;
            set => Title = value;
        }

        private string _mensaje = string.Empty;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                MensajeTextBlock.Text = value;
            }
        }

        private DialogTipo _tipoMensaje = DialogTipo.Info;
        public DialogTipo TipoMensaje
        {
            get => _tipoMensaje;
            set
            {
                _tipoMensaje = value;
                AplicarEstiloPorTipo();
            }
        }

        public DialogResultado Resultado { get; private set; } = DialogResultado.Cancelar;

        public DialogWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AplicarEstiloPorTipo()
        {
            var (icono, colorTitulo, colorIcono) = TipoMensaje switch
            {
                DialogTipo.Info         => ("ℹ️",  Brushes.DeepSkyBlue, Brushes.DeepSkyBlue),
                DialogTipo.Error        => ("❌",  (Brush)FindResource("ErrorBrush"), (Brush)FindResource("ErrorBrush")),
                DialogTipo.Confirmar    => ("❓",  (Brush)FindResource("PrimaryBrush"), (Brush)FindResource("PrimaryBrush")),
                DialogTipo.Advertencia  => ("⚠️",  (Brush)FindResource("WarningBrush"), (Brush)FindResource("WarningBrush")),
                _                       => ("ℹ️",  Brushes.Gray, Brushes.Gray)
            };

            IconoTextBlock.Text = icono;
            IconoTextBlock.Foreground = colorIcono;
            TituloTextBlock.Text = Titulo;
            TituloTextBlock.Foreground = colorTitulo;

            PanelBotones.Children.Clear();

            if (TipoMensaje is DialogTipo.Confirmar or DialogTipo.Advertencia)
            {
                // Dos botones: Cancelar + Aceptar
                var cancelarBtn = new Button
                {
                    Content = "Cancelar",
                    Style = (Style)FindResource("BotonSecundario"),
                    Margin = new Thickness(0, 0, 8, 0),
                    Foreground = (Brush)FindResource("TextPrimaryBrush")
                };
                cancelarBtn.Click += (s, e) =>
                {
                    Resultado = DialogResultado.Cancelar;
                    Close();
                };

                var aceptarBtn = new Button
                {
                    Content = "Aceptar",
                    Style = (Style)FindResource("BotonPrimario"),
                    Foreground = Brushes.White
                };
                aceptarBtn.Click += (s, e) =>
                {
                    Resultado = DialogResultado.Aceptar;
                    Close();
                };

                PanelBotones.Children.Add(cancelarBtn);
                PanelBotones.Children.Add(aceptarBtn);
            }
            else
            {
                // Un solo botón: Aceptar
                var aceptarBtn = new Button
                {
                    Content = "Aceptar",
                    Style = (Style)FindResource("BotonPrimario"),
                    MinWidth = 120,
                    Foreground = Brushes.White
                };
                aceptarBtn.Click += (s, e) =>
                {
                    Resultado = DialogResultado.Aceptar;
                    Close();
                };

                PanelBotones.Children.Add(aceptarBtn);
            }
        }

        protected override void OnActivated(System.EventArgs e)
        {
            base.OnActivated(e);
            // Aplicar estilo después de que el Window esté completamente inicializado
            AplicarEstiloPorTipo();
        }
    }
}
