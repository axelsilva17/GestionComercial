using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Configuracion;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Main
{
    public partial class ShellView : Window
    {
        private ShellViewModel VM => DataContext as ShellViewModel;

        // ── Estado sidebar ────────────────────────────────────────────────────
        private bool _sidebarCollapsed = false;
        private const double SidebarExpandedWidth  = 220;
        private const double SidebarCollapsedWidth = 56;

        // ── Estado panel perfil ───────────────────────────────────────────────
        private bool   _perfilAbierto        = false;
        private string _passActual           = string.Empty;
        private string _passNuevo            = string.Empty;
        private string _passConfirmar        = string.Empty;
        private string _respuesta            = string.Empty;
        private string _confirmarRespuesta   = string.Empty;

        private RecuperacionContrasenaServicio _recuperacionServicio =>
            IoC.Get<RecuperacionContrasenaServicio>();

        public ShellView()
        {
            InitializeComponent();
            Loaded += ShellView_Loaded;
        }

        private void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            // Cargar preguntas en el ComboBox
            CbPreguntas.ItemsSource = PreguntasSecretas.Lista;
            if (CbPreguntas.Items.Count > 0)
                CbPreguntas.SelectedIndex = 0;

            // Actualizar estado pregunta secreta
            _ = ActualizarEstadoPreguntaAsync();
        }

        // ══ VENTANA ══════════════════════════════════════════════════════════

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Solo arrastra si no se hace clic sobre el panel de perfil
            if (!IsPuntoDentroDePanelPerfil(e.GetPosition(this)))
                DragMove();
        }

        private bool IsPuntoDentroDePanelPerfil(Point p)
        {
            if (PanelPerfil.Visibility != Visibility.Visible) return false;
            var pos = PanelPerfil.TranslatePoint(new Point(0, 0), this);
            return p.X >= pos.X && p.Y >= pos.Y
                && p.X <= pos.X + PanelPerfil.ActualWidth
                && p.Y <= pos.Y + PanelPerfil.ActualHeight;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void Maximize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        // ══ RESPONSIVE SIDEBAR ═══════════════════════════════════════════════

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Auto-colapsar bajo 1100px, expandir sobre 1300px
            if (e.NewSize.Width < 1100 && !_sidebarCollapsed)
                ColapsarSidebar(animate: false);
            else if (e.NewSize.Width >= 1300 && _sidebarCollapsed)
                ExpandirSidebar(animate: false);
        }

        private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            if (_sidebarCollapsed) ExpandirSidebar(animate: true);
            else                   ColapsarSidebar(animate: true);
        }

        private void ColapsarSidebar(bool animate)
        {
            _sidebarCollapsed = true;
            OcultarTextosSidebar();

            if (animate)
            {
                var anim = new GridLengthAnimation
                {
                    From     = new GridLength(SidebarExpandedWidth),
                    To       = new GridLength(SidebarCollapsedWidth),
                    Duration = new Duration(TimeSpan.FromMilliseconds(220))
                };
                SidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, anim);
            }
            else
            {
                SidebarColumn.Width = new GridLength(SidebarCollapsedWidth);
            }
        }

        private void ExpandirSidebar(bool animate)
        {
            _sidebarCollapsed = false;
            MostrarTextosSidebar();

            if (animate)
            {
                var anim = new GridLengthAnimation
                {
                    From     = new GridLength(SidebarCollapsedWidth),
                    To       = new GridLength(SidebarExpandedWidth),
                    Duration = new Duration(TimeSpan.FromMilliseconds(220))
                };
                SidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, anim);
            }
            else
            {
                SidebarColumn.Width = new GridLength(SidebarExpandedWidth);
            }
        }

        private void OcultarTextosSidebar()
        {
            TxtSistema.Visibility     = Visibility.Collapsed;
            TxtSucursal.Visibility    = Visibility.Collapsed;
            LblPrincipal.Visibility   = Visibility.Collapsed;
            LblCatalogo.Visibility    = Visibility.Collapsed;
            LblGestion.Visibility     = Visibility.Collapsed;
            FooterNombre.Visibility   = Visibility.Collapsed;

            TxtDashboard.Visibility   = Visibility.Collapsed;
            TxtVentas.Visibility      = Visibility.Collapsed;
            TxtCaja.Visibility        = Visibility.Collapsed;
            TxtCompras.Visibility     = Visibility.Collapsed;
            TxtProductos.Visibility   = Visibility.Collapsed;
            TxtInventario.Visibility  = Visibility.Collapsed;
            TxtClientes.Visibility    = Visibility.Collapsed;
            TxtProveedores.Visibility = Visibility.Collapsed;
            TxtReportes.Visibility    = Visibility.Collapsed;
            TxtConfiguracion.Visibility = Visibility.Collapsed;
        }

        private void MostrarTextosSidebar()
        {
            TxtSistema.Visibility     = Visibility.Visible;
            TxtSucursal.Visibility    = Visibility.Visible;
            LblPrincipal.Visibility   = Visibility.Visible;
            LblCatalogo.Visibility    = Visibility.Visible;
            LblGestion.Visibility     = Visibility.Visible;
            FooterNombre.Visibility   = Visibility.Visible;

            TxtDashboard.Visibility   = Visibility.Visible;
            TxtVentas.Visibility      = Visibility.Visible;
            TxtCaja.Visibility        = Visibility.Visible;
            TxtCompras.Visibility     = Visibility.Visible;
            TxtProductos.Visibility   = Visibility.Visible;
            TxtInventario.Visibility  = Visibility.Visible;
            TxtClientes.Visibility    = Visibility.Visible;
            TxtProveedores.Visibility = Visibility.Visible;
            TxtReportes.Visibility    = Visibility.Visible;
            TxtConfiguracion.Visibility = Visibility.Visible;
        }

        // ══ PANEL PERFIL — APERTURA/CIERRE ══════════════════════════════════

        private void AbrirPerfil_Click(object sender, RoutedEventArgs e)
        {
            if (_perfilAbierto) { CerrarPanelPerfil(); return; }
            AbrirPanelPerfil();
        }

        private void CerrarPerfil_Click(object sender, RoutedEventArgs e)
            => CerrarPanelPerfil();

        private void AbrirPanelPerfil()
        {
            _perfilAbierto               = true;
            OcultarTodosLosFormularios();
            MensajeExito.Visibility      = Visibility.Collapsed;
            PanelPerfil.Visibility       = Visibility.Visible;
            Overlay.IsHitTestVisible     = true;

            _ = ActualizarEstadoPreguntaAsync();

            var slide = new DoubleAnimation
            {
                From           = PanelPerfil.Width,
                To             = 0,
                Duration       = new Duration(TimeSpan.FromMilliseconds(280)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            PanelPerfilTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, slide);

            var fade = new DoubleAnimation
            {
                From     = 0, To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };
            Overlay.BeginAnimation(OpacityProperty, fade);
        }

        private void CerrarPanelPerfil()
        {
            _perfilAbierto = false;

            var slide = new DoubleAnimation
            {
                From           = 0,
                To             = PanelPerfil.Width,
                Duration       = new Duration(TimeSpan.FromMilliseconds(220)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            slide.Completed += (s, e) =>
            {
                PanelPerfil.Visibility   = Visibility.Collapsed;
                Overlay.IsHitTestVisible = false;
            };
            PanelPerfilTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, slide);

            var fade = new DoubleAnimation
            {
                From = 1, To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180))
            };
            Overlay.BeginAnimation(OpacityProperty, fade);
        }

        private void OcultarTodosLosFormularios()
        {
            FormDatos.Visibility     = Visibility.Collapsed;
            FormPassword.Visibility  = Visibility.Collapsed;
            FormPregunta.Visibility  = Visibility.Collapsed;
            ErrorDatos.Visibility    = Visibility.Collapsed;
            ErrorPassword.Visibility = Visibility.Collapsed;
            ErrorPregunta.Visibility = Visibility.Collapsed;
        }

        private async Task ActualizarEstadoPreguntaAsync()
        {
            try
            {
                var email    = VM?.SesionActual?.Email;
                if (string.IsNullOrEmpty(email)) return;
                var pregunta = await _recuperacionServicio.ObtenerPreguntaAsync(email);
                var tieneConfigurada = !string.IsNullOrEmpty(pregunta);

                TxtEstadoPregunta.Text = tieneConfigurada
                    ? "✅ Configurada — podés cambiarla cuando quieras"
                    : "⚠️ Sin configurar — necesaria para recuperar contraseña";
                BtnEditarPregunta.Content = tieneConfigurada ? "🔐 Cambiar" : "🔐 Configurar";
            }
            catch { /* silencioso */ }
        }

        // ══ DATOS PERSONALES ═════════════════════════════════════════════════

        private void AbrirEditarDatos_Click(object sender, RoutedEventArgs e)
        {
            OcultarTodosLosFormularios();
            TxtNombre.Text    = VM?.SesionActual?.Nombre   ?? "";
            TxtApellido.Text  = VM?.SesionActual?.Apellido ?? "";
            FormDatos.Visibility = Visibility.Visible;
        }

        private void CancelarDatos_Click(object sender, RoutedEventArgs e)
            => FormDatos.Visibility = Visibility.Collapsed;

        private async void GuardarDatos_Click(object sender, RoutedEventArgs e)
        {
            ErrorDatos.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(TxtNombre.Text))
            { MostrarError(ErrorDatos, TxtErrorDatos, "El nombre es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(TxtApellido.Text))
            { MostrarError(ErrorDatos, TxtErrorDatos, "El apellido es obligatorio."); return; }

            try
            {
                // TODO: await usuarioServicio.ActualizarNombreAsync(...)
                await Task.Delay(200);

                if (VM?.SesionActual != null)
                {
                    VM.SesionActual.Nombre   = TxtNombre.Text.Trim();
                    VM.SesionActual.Apellido = TxtApellido.Text.Trim();
                    VM.UsuarioNombre = $"{VM.SesionActual.Nombre} {VM.SesionActual.Apellido}";
                }

                FormDatos.Visibility = Visibility.Collapsed;
                MostrarExito("Datos actualizados correctamente.");
            }
            catch (Exception ex)
            { MostrarError(ErrorDatos, TxtErrorDatos, ex.Message); }
        }

        // ══ CONTRASEÑA ═══════════════════════════════════════════════════════

        private void AbrirCambiarPassword_Click(object sender, RoutedEventArgs e)
        {
            OcultarTodosLosFormularios();
            PbActual.Clear(); PbNuevo.Clear(); PbConfirmar.Clear();
            _passActual = _passNuevo = _passConfirmar = string.Empty;
            FormPassword.Visibility = Visibility.Visible;
        }

        private void CancelarPassword_Click(object sender, RoutedEventArgs e)
            => FormPassword.Visibility = Visibility.Collapsed;

        private async void GuardarPassword_Click(object sender, RoutedEventArgs e)
        {
            ErrorPassword.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(_passActual))
            { MostrarError(ErrorPassword, TxtErrorPassword, "Ingresá tu contraseña actual."); return; }
            if (string.IsNullOrWhiteSpace(_passNuevo))
            { MostrarError(ErrorPassword, TxtErrorPassword, "Ingresá la nueva contraseña."); return; }
            if (_passNuevo != _passConfirmar)
            { MostrarError(ErrorPassword, TxtErrorPassword, "Las contraseñas no coinciden."); return; }
            if (_passNuevo.Length < 8)
            { MostrarError(ErrorPassword, TxtErrorPassword, "La contraseña debe tener al menos 8 caracteres."); return; }

            try
            {
                var authServicio = IoC.Get<AutenticacionServicio>();
                var sesion = await authServicio.LoginAsync(VM.SesionActual.Email, _passActual);
                if (sesion == null)
                { MostrarError(ErrorPassword, TxtErrorPassword, "La contraseña actual es incorrecta."); return; }

                await _recuperacionServicio.CambiarContrasenaAsync(VM.SesionActual.Email, _passNuevo);

                FormPassword.Visibility = Visibility.Collapsed;
                PbActual.Clear(); PbNuevo.Clear(); PbConfirmar.Clear();
                MostrarExito("Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            { MostrarError(ErrorPassword, TxtErrorPassword, ex.Message); }
        }

        private void PbActual_PasswordChanged(object sender, RoutedEventArgs e)
            => _passActual = PbActual.Password;
        private void PbNuevo_PasswordChanged(object sender, RoutedEventArgs e)
            => _passNuevo = PbNuevo.Password;
        private void PbConfirmar_PasswordChanged(object sender, RoutedEventArgs e)
            => _passConfirmar = PbConfirmar.Password;

        // ══ PREGUNTA SECRETA ══════════════════════════════════════════════════

        private void AbrirPreguntaSecreta_Click(object sender, RoutedEventArgs e)
        {
            OcultarTodosLosFormularios();
            PbRespuesta.Clear(); PbConfirmarRespuesta.Clear();
            _respuesta = _confirmarRespuesta = string.Empty;
            if (CbPreguntas.Items.Count > 0) CbPreguntas.SelectedIndex = 0;
            FormPregunta.Visibility = Visibility.Visible;
        }

        private void CancelarPregunta_Click(object sender, RoutedEventArgs e)
            => FormPregunta.Visibility = Visibility.Collapsed;

        private async void GuardarPregunta_Click(object sender, RoutedEventArgs e)
        {
            ErrorPregunta.Visibility = Visibility.Collapsed;

            if (CbPreguntas.SelectedItem == null)
            { MostrarError(ErrorPregunta, TxtErrorPregunta, "Seleccioná una pregunta."); return; }
            if (string.IsNullOrWhiteSpace(_respuesta))
            { MostrarError(ErrorPregunta, TxtErrorPregunta, "Ingresá tu respuesta."); return; }
            if (_respuesta != _confirmarRespuesta)
            { MostrarError(ErrorPregunta, TxtErrorPregunta, "Las respuestas no coinciden."); return; }
            if (_respuesta.Length < 3)
            { MostrarError(ErrorPregunta, TxtErrorPregunta, "La respuesta debe tener al menos 3 caracteres."); return; }

            try
            {
                await _recuperacionServicio.ConfigurarPreguntaAsync(
                    VM.SesionActual.Email,
                    CbPreguntas.SelectedItem.ToString(),
                    _respuesta);

                FormPregunta.Visibility = Visibility.Collapsed;
                PbRespuesta.Clear(); PbConfirmarRespuesta.Clear();
                await ActualizarEstadoPreguntaAsync();
                MostrarExito("Pregunta secreta configurada correctamente.");
            }
            catch (Exception ex)
            { MostrarError(ErrorPregunta, TxtErrorPregunta, ex.Message); }
        }

        private void PbRespuesta_PasswordChanged(object sender, RoutedEventArgs e)
            => _respuesta = PbRespuesta.Password;
        private void PbConfirmarRespuesta_PasswordChanged(object sender, RoutedEventArgs e)
            => _confirmarRespuesta = PbConfirmarRespuesta.Password;

        // ══ HELPERS ══════════════════════════════════════════════════════════

        private void MostrarError(System.Windows.Controls.Border errorBorder,
                                  System.Windows.Controls.TextBlock errorText, string mensaje)
        {
            errorText.Text            = mensaje;
            errorBorder.Visibility    = Visibility.Visible;
            MensajeExito.Visibility   = Visibility.Collapsed;
        }

        private async void MostrarExito(string mensaje)
        {
            TxtExito.Text             = mensaje;
            MensajeExito.Visibility   = Visibility.Visible;
            await Task.Delay(3000);
            MensajeExito.Visibility   = Visibility.Collapsed;
        }
    }

    // ── GridLengthAnimation para animar el ancho del sidebar ─────────────────
    public class GridLengthAnimation : AnimationTimeline
    {
        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(nameof(From), typeof(GridLength), typeof(GridLengthAnimation));

        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(nameof(To), typeof(GridLength), typeof(GridLengthAnimation));

        public override Type TargetPropertyType => typeof(GridLength);

        protected override System.Windows.Freezable CreateInstanceCore()
            => new GridLengthAnimation();

        public override object GetCurrentValue(object defaultOriginValue,
                                               object defaultDestinationValue,
                                               AnimationClock animationClock)
        {
            double progress = animationClock.CurrentProgress ?? 0;
            double from     = From.Value;
            double to       = To.Value;
            // EaseInOut suave
            progress = progress < 0.5
                ? 2 * progress * progress
                : 1 - Math.Pow(-2 * progress + 2, 2) / 2;
            return new GridLength(from + (to - from) * progress);
        }
    }
}
