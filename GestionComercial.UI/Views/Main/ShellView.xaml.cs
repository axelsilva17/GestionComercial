using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Main
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();

            // Cargar vista inicial
            LoadDashboard();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Animar entrada de la ventana
            var storyboard = (Storyboard)FindResource("FadeIn");
            var border = sender as Border;
            border?.BeginStoryboard(storyboard);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Permitir arrastrar la ventana desde el header
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar selección de todos los botones
            ClearMenuSelection();

            // Marcar el botón clickeado como activo
            var button = sender as Button;
            if (button != null)
            {
                button.Tag = "Active";

                // Cambiar título y subtítulo según el botón
                switch (button.Name)
                {
                    case "BtnDashboard":
                        PageTitle.Text = "Dashboard";
                        PageSubtitle.Text = "Resumen general del sistema";
                        LoadDashboard();
                        break;
                    case "BtnVentas":
                        PageTitle.Text = "Ventas";
                        PageSubtitle.Text = "Gestión de ventas y facturación";
                        LoadVentas();
                        break;
                    case "BtnCompras":
                        PageTitle.Text = "Compras";
                        PageSubtitle.Text = "Gestión de órdenes de compra";
                        LoadCompras();
                        break;
                    case "BtnCaja":
                        PageTitle.Text = "Caja";
                        PageSubtitle.Text = "Control de movimientos de caja";
                        LoadCaja();
                        break;
                    case "BtnProductos":
                        PageTitle.Text = "Productos";
                        PageSubtitle.Text = "Catálogo de productos";
                        LoadProductos();
                        break;
                    case "BtnInventario":
                        PageTitle.Text = "Inventario";
                        PageSubtitle.Text = "Control de stock y movimientos";
                        LoadInventario();
                        break;
                    case "BtnClientes":
                        PageTitle.Text = "Clientes";
                        PageSubtitle.Text = "Base de datos de clientes";
                        LoadClientes();
                        break;
                    case "BtnProveedores":
                        PageTitle.Text = "Proveedores";
                        PageSubtitle.Text = "Gestión de proveedores";
                        LoadProveedores();
                        break;
                    case "BtnReportes":
                        PageTitle.Text = "Reportes";
                        PageSubtitle.Text = "Análisis y reportes estadísticos";
                        LoadReportes();
                        break;
                    case "BtnConfiguracion":
                        PageTitle.Text = "Configuración";
                        PageSubtitle.Text = "Ajustes del sistema";
                        LoadConfiguracion();
                        break;
                }
            }
        }

        private void ClearMenuSelection()
        {
            BtnDashboard.Tag = null;
            BtnVentas.Tag = null;
            BtnCompras.Tag = null;
            BtnCaja.Tag = null;
            BtnProductos.Tag = null;
            BtnInventario.Tag = null;
            BtnClientes.Tag = null;
            BtnProveedores.Tag = null;
            BtnReportes.Tag = null;
            BtnConfiguracion.Tag = null;
        }

        // Métodos para cargar vistas (placeholder - reemplazá con tus vistas reales)
        private void LoadDashboard()
        {
            var content = new Border
            {
                Background = (System.Windows.Media.Brush)FindResource("CardBrush"),
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(40),
                Child = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "📊",
                            FontSize = 48,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 20)
                        },
                        new TextBlock
                        {
                            Text = "Panel de Control",
                            FontSize = 28,
                            FontWeight = FontWeights.SemiBold,
                            Foreground = (System.Windows.Media.Brush)FindResource("TextPrimaryBrush"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 12)
                        },
                        new TextBlock
                        {
                            Text = "Aquí se mostrará el dashboard con métricas y estadísticas del negocio",
                            FontSize = 16,
                            Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        }
                    }
                }
            };
            PageContent.Content = content;
        }

        private void LoadVentas()
        {
            CreatePlaceholder("💳", "Módulo de Ventas", "Sistema de punto de venta y facturación");
        }

        private void LoadCompras()
        {
            CreatePlaceholder("🛒", "Módulo de Compras", "Gestión de órdenes de compra a proveedores");
        }

        private void LoadCaja()
        {
            CreatePlaceholder("💰", "Módulo de Caja", "Control de ingresos, egresos y arqueos");
        }

        private void LoadProductos()
        {
            CreatePlaceholder("📦", "Módulo de Productos", "Gestión del catálogo de productos y servicios");
        }

        private void LoadInventario()
        {
            CreatePlaceholder("📋", "Módulo de Inventario", "Control de stock y movimientos de mercadería");
        }

        private void LoadClientes()
        {
            CreatePlaceholder("👥", "Módulo de Clientes", "Gestión de la base de datos de clientes");
        }

        private void LoadProveedores()
        {
            CreatePlaceholder("🏢", "Módulo de Proveedores", "Administración de proveedores y contactos");
        }

        private void LoadReportes()
        {
            CreatePlaceholder("📈", "Módulo de Reportes", "Análisis estadístico y reportes del negocio");
        }

        private void LoadConfiguracion()
        {
            CreatePlaceholder("⚙️", "Configuración", "Ajustes generales del sistema");
        }

        private void CreatePlaceholder(string emoji, string title, string description)
        {
            var content = new Border
            {
                Background = (System.Windows.Media.Brush)FindResource("CardBrush"),
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(40),
                Child = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = emoji,
                            FontSize = 48,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 20)
                        },
                        new TextBlock
                        {
                            Text = title,
                            FontSize = 28,
                            FontWeight = FontWeights.SemiBold,
                            Foreground = (System.Windows.Media.Brush)FindResource("TextPrimaryBrush"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 12)
                        },
                        new TextBlock
                        {
                            Text = description,
                            FontSize = 16,
                            Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        }
                    }
                }
            };
            PageContent.Content = content;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Volver al login
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
