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

        private void Sidebar_Loaded(object sender, RoutedEventArgs e)
        {
            // Animar entrada del sidebar
            var storyboard = (Storyboard)FindResource("SidebarIn");
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
                
                // Cambiar título de página según el botón
                switch (button.Name)
                {
                    case "BtnDashboard":
                        PageTitle.Text = "Dashboard";
                        LoadDashboard();
                        break;
                    case "BtnVentas":
                        PageTitle.Text = "Ventas";
                        LoadVentas();
                        break;
                    case "BtnProductos":
                        PageTitle.Text = "Productos";
                        LoadProductos();
                        break;
                    case "BtnClientes":
                        PageTitle.Text = "Clientes";
                        LoadClientes();
                        break;
                    case "BtnProveedores":
                        PageTitle.Text = "Proveedores";
                        LoadProveedores();
                        break;
                    case "BtnInventario":
                        PageTitle.Text = "Inventario";
                        LoadInventario();
                        break;
                    case "BtnReportes":
                        PageTitle.Text = "Reportes";
                        LoadReportes();
                        break;
                    case "BtnConfiguracion":
                        PageTitle.Text = "Configuración";
                        LoadConfiguracion();
                        break;
                }
            }
        }

        private void ClearMenuSelection()
        {
            BtnDashboard.Tag = null;
            BtnVentas.Tag = null;
            BtnProductos.Tag = null;
            BtnClientes.Tag = null;
            BtnProveedores.Tag = null;
            BtnInventario.Tag = null;
            BtnReportes.Tag = null;
            BtnConfiguracion.Tag = null;
        }

        // Métodos para cargar vistas (placeholder)
        private void LoadDashboard()
        {
            var content = new TextBlock
            {
                Text = "📊 Dashboard\n\nBienvenido al sistema de gestión comercial",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadVentas()
        {
            var content = new TextBlock
            {
                Text = "💰 Ventas\n\nMódulo de ventas en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadProductos()
        {
            var content = new TextBlock
            {
                Text = "📦 Productos\n\nMódulo de productos en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadClientes()
        {
            var content = new TextBlock
            {
                Text = "👥 Clientes\n\nMódulo de clientes en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadProveedores()
        {
            var content = new TextBlock
            {
                Text = "🏢 Proveedores\n\nMódulo de proveedores en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadInventario()
        {
            var content = new TextBlock
            {
                Text = "📋 Inventario\n\nMódulo de inventario en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadReportes()
        {
            var content = new TextBlock
            {
                Text = "📈 Reportes\n\nMódulo de reportes en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };
            PageContent.Content = content;
        }

        private void LoadConfiguracion()
        {
            var content = new TextBlock
            {
                Text = "⚙️ Configuración\n\nMódulo de configuración en desarrollo",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
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
