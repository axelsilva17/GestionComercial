using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.UI.ViewModels.Inventario;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Inventario
{
    public partial class InventarioView : UserControl
    {
        private InventarioViewModel VM => DataContext as InventarioViewModel;

        public InventarioView()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (VM == null) return;
                VM.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(VM.PanelVisible))
                    {
                        if (VM.PanelVisible) AbrirPanel();
                        else                  CerrarPanel();
                    }
                    if (args.PropertyName == nameof(VM.NuevoTipo))
                        ActualizarBotonesTipo(VM.NuevoTipo);
                };
            };
        }

        // ── Animación slide ───────────────────────────────────────────────────

        private void AbrirPanel()
        {
            PanelMovimiento.Visibility   = Visibility.Visible;
            Overlay.IsHitTestVisible     = true;

            var slide = new DoubleAnimation
            {
                From           = PanelMovimiento.Width,
                To             = 0,
                Duration       = new Duration(TimeSpan.FromMilliseconds(280)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            PanelTransform.BeginAnimation(TranslateTransform.XProperty, slide);

            var fade = new DoubleAnimation
            {
                From     = 0, To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };
            Overlay.BeginAnimation(OpacityProperty, fade);

            // Inicializar selección visual del tipo
            ActualizarBotonesTipo(VM?.NuevoTipo ?? "Entrada");
        }

        private void CerrarPanel()
        {
            var slide = new DoubleAnimation
            {
                From           = 0,
                To             = PanelMovimiento.Width,
                Duration       = new Duration(TimeSpan.FromMilliseconds(220)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            slide.Completed += (_, _) =>
            {
                PanelMovimiento.Visibility = Visibility.Collapsed;
            };
            PanelTransform.BeginAnimation(TranslateTransform.XProperty, slide);

            var fade = new DoubleAnimation
            {
                From     = 1, To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180))
            };
            fade.Completed += (_, _) => Overlay.IsHitTestVisible = false;
            Overlay.BeginAnimation(OpacityProperty, fade);
        }

        private void CerrarPanel_Click(object sender, RoutedEventArgs e)
            => VM?.CerrarPanel();

        // ── Selector visual de tipo (Entrada / Salida / Ajuste) ───────────────

        private void TipoBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tipo && VM != null)
                VM.NuevoTipo = tipo;
        }

        private void ActualizarBotonesTipo(string tipo)
        {
            // Resetear todos
            EstiloBotonTipo(BtnEntrada, false, "#22C55E");
            EstiloBotonTipo(BtnSalida,  false, "#EF4444");
            EstiloBotonTipo(BtnAjuste,  false, "#F59E0B");

            // Activar el seleccionado
            switch (tipo)
            {
                case "Entrada": EstiloBotonTipo(BtnEntrada, true, "#22C55E"); break;
                case "Salida":  EstiloBotonTipo(BtnSalida,  true, "#EF4444"); break;
                case "Ajuste":  EstiloBotonTipo(BtnAjuste,  true, "#F59E0B"); break;
            }
        }

        private static void EstiloBotonTipo(Button btn, bool activo, string colorHex)
        {
            var color = (Color)ColorConverter.ConvertFromString(colorHex);
            if (activo)
            {
                btn.Background   = new SolidColorBrush(Color.FromArgb(30, color.R, color.G, color.B));
                btn.BorderBrush  = new SolidColorBrush(color);
                btn.Foreground   = new SolidColorBrush(color);
            }
            else
            {
                btn.ClearValue(Button.BackgroundProperty);
                btn.ClearValue(Button.BorderBrushProperty);
                btn.ClearValue(Button.ForegroundProperty);
            }
        }

        // ── BuscadorProducto → ViewModel ─────────────────────────────────────

        private void BuscadorProducto_ProductoElegido(object sender, ProductoDto producto)
        {
            if (VM != null) VM.ProductoSeleccionado = producto;
        }
    }
}
