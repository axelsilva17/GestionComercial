using GestionComercial.Aplicacion.DTOs.Productos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.UI.Controles
{
    public partial class BuscadorProducto : UserControl
    {
        // ── DependencyProperties ─────────────────────────────────────────────

        public static readonly DependencyProperty ProductoSeleccionadoProperty =
            DependencyProperty.Register(nameof(ProductoSeleccionado), typeof(ProductoDto), typeof(BuscadorProducto),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Función de búsqueda que el ViewModel padre debe proveer.
        /// Recibe el texto y retorna la lista de productos.
        /// Ejemplo: BuscarProductos="{Binding BuscarProductosAsync}"
        /// </summary>
        public static readonly DependencyProperty BuscarFuncProperty =
            DependencyProperty.Register(nameof(BuscarFunc), typeof(Func<string, Task<IEnumerable<ProductoDto>>>),
                typeof(BuscadorProducto), new PropertyMetadata(null));

        public ProductoDto ProductoSeleccionado
        {
            get => (ProductoDto)GetValue(ProductoSeleccionadoProperty);
            set => SetValue(ProductoSeleccionadoProperty, value);
        }

        public Func<string, Task<IEnumerable<ProductoDto>>> BuscarFunc
        {
            get => (Func<string, Task<IEnumerable<ProductoDto>>>)GetValue(BuscarFuncProperty);
            set => SetValue(BuscarFuncProperty, value);
        }

        // ── Evento ───────────────────────────────────────────────────────────

        public event EventHandler<ProductoDto> ProductoElegido;

        // ── Estado interno ───────────────────────────────────────────────────
        private System.Threading.CancellationTokenSource _cts;
        private bool _seleccionandoItem;

        public BuscadorProducto()
        {
            InitializeComponent();
        }

        // ── Búsqueda con debounce ─────────────────────────────────────────────

        private async void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = TxtBusqueda.Text.Trim();
            BtnLimpiar.Visibility = string.IsNullOrEmpty(texto) ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrWhiteSpace(texto) || texto.Length < 2)
            {
                CerrarDropdown();
                return;
            }

            // Cancelar búsqueda anterior
            _cts?.Cancel();
            _cts = new System.Threading.CancellationTokenSource();
            var token = _cts.Token;

            MostrarCargando();

            // Debounce 300ms
            await Task.Delay(300);
            if (token.IsCancellationRequested) return;

            try
            {
                IEnumerable<ProductoDto> resultados;

                if (BuscarFunc != null)
                    resultados = await BuscarFunc(texto);
                else
                    resultados = BusquedaMock(texto); // fallback para desarrollo

                if (token.IsCancellationRequested) return;
                MostrarResultados(resultados);
            }
            catch { CerrarDropdown(); }
        }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { CerrarDropdown(); BtnLimpiar_Click(null, null); }
        }

        private void TxtBusqueda_LostFocus(object sender, RoutedEventArgs e)
        {
            // Pequeño delay para que el click en el item se procese antes de cerrar
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_seleccionandoItem) CerrarDropdown();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void ItemResultado_Click(object sender, MouseButtonEventArgs e)
        {
            _seleccionandoItem = true;
            if ((sender as Border)?.DataContext is ProductoDto producto)
                SeleccionarProducto(producto);
            _seleccionandoItem = false;
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            TxtBusqueda.Clear();
            ProductoSeleccionado = null;
            CerrarDropdown();
        }

        // ── Helpers UI ────────────────────────────────────────────────────────

        private void MostrarCargando()
        {
            Dropdown.Visibility      = Visibility.Visible;
            PnlCargando.Visibility   = Visibility.Visible;
            ListaResultados.Visibility = Visibility.Collapsed;
            TxtSinResultados.Visibility = Visibility.Collapsed;
        }

        private void MostrarResultados(IEnumerable<ProductoDto> resultados)
        {
            PnlCargando.Visibility = Visibility.Collapsed;
            var lista = new List<ProductoDto>(resultados);

            if (lista.Count == 0)
            {
                ListaResultados.Visibility  = Visibility.Collapsed;
                TxtSinResultados.Visibility = Visibility.Visible;
            }
            else
            {
                ListaResultados.ItemsSource = lista;
                ListaResultados.Visibility  = Visibility.Visible;
                TxtSinResultados.Visibility = Visibility.Collapsed;
            }
            Dropdown.Visibility = Visibility.Visible;
        }

        private void CerrarDropdown()
        {
            Dropdown.Visibility = Visibility.Collapsed;
            PnlCargando.Visibility = Visibility.Collapsed;
        }

        private void SeleccionarProducto(ProductoDto producto)
        {
            ProductoSeleccionado = producto;
            TxtBusqueda.Text     = producto.Nombre;
            BtnLimpiar.Visibility = Visibility.Visible;
            CerrarDropdown();
            ProductoElegido?.Invoke(this, producto);
        }

        // ── Mock para desarrollo (reemplazar con servicio real) ───────────────

        private IEnumerable<ProductoDto> BusquedaMock(string texto)
        {
            var mock = new List<ProductoDto>
            {
                new() { IdProducto = 1, Nombre = "Auriculares Pro X",   CodigoBarra = "7890001", PrecioVentaActual = 24000, StockActual = 8  },
                new() { IdProducto = 2, Nombre = "Mouse Inalámbrico",   CodigoBarra = "7890002", PrecioVentaActual = 12500, StockActual = 3  },
                new() { IdProducto = 3, Nombre = "Teclado Mecánico",    CodigoBarra = "7890003", PrecioVentaActual = 34000, StockActual = 15 },
                new() { IdProducto = 4, Nombre = "Monitor 24\"",        CodigoBarra = "7890004", PrecioVentaActual = 120000,StockActual = 5  },
                new() { IdProducto = 5, Nombre = "Webcam HD 1080p",     CodigoBarra = "7890005", PrecioVentaActual = 18000, StockActual = 12 },
            };
            return mock.FindAll(p =>
                p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.CodigoBarra.ToString().Contains(texto));
        }
    }
}
