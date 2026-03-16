using GestionComercial.Aplicacion.DTOs.Clientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.UI.Controles
{
    public partial class SelectorCliente : UserControl
    {
        // ── DependencyProperties ─────────────────────────────────────────────

        public static readonly DependencyProperty ClienteSeleccionadoProperty =
            DependencyProperty.Register(nameof(ClienteSeleccionado), typeof(ClienteDto), typeof(SelectorCliente),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty BuscarFuncProperty =
            DependencyProperty.Register(nameof(BuscarFunc), typeof(Func<string, Task<IEnumerable<ClienteDto>>>),
                typeof(SelectorCliente), new PropertyMetadata(null));

        public ClienteDto ClienteSeleccionado
        {
            get => (ClienteDto)GetValue(ClienteSeleccionadoProperty);
            set => SetValue(ClienteSeleccionadoProperty, value);
        }

        public Func<string, Task<IEnumerable<ClienteDto>>> BuscarFunc
        {
            get => (Func<string, Task<IEnumerable<ClienteDto>>>)GetValue(BuscarFuncProperty);
            set => SetValue(BuscarFuncProperty, value);
        }

        // ── Evento ───────────────────────────────────────────────────────────
        public event EventHandler<ClienteDto> ClienteElegido;

        private System.Threading.CancellationTokenSource _cts;
        private bool _seleccionandoItem;

        public SelectorCliente()
        {
            InitializeComponent();
        }

        public void Limpiar()
        {
            TxtBusqueda.Clear();
            ClienteSeleccionado = null;
            BtnLimpiar.Visibility = Visibility.Collapsed;
            CerrarDropdown();
        }

        // ── Búsqueda ──────────────────────────────────────────────────────────

        private async void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = TxtBusqueda.Text.Trim();
            BtnLimpiar.Visibility = string.IsNullOrEmpty(texto) ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrWhiteSpace(texto) || texto.Length < 2)
            {
                CerrarDropdown();
                return;
            }

            _cts?.Cancel();
            _cts = new System.Threading.CancellationTokenSource();
            var token = _cts.Token;

            MostrarCargando();
            await Task.Delay(300);
            if (token.IsCancellationRequested) return;

            try
            {
                var resultados = BuscarFunc != null
                    ? await BuscarFunc(texto)
                    : BusquedaMock(texto);

                if (!token.IsCancellationRequested)
                    MostrarResultados(resultados);
            }
            catch { CerrarDropdown(); }
        }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Limpiar();
        }

        private void TxtBusqueda_LostFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_seleccionandoItem) CerrarDropdown();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void ItemResultado_Click(object sender, MouseButtonEventArgs e)
        {
            _seleccionandoItem = true;
            if ((sender as Border)?.DataContext is ClienteDto cliente)
                SeleccionarCliente(cliente);
            _seleccionandoItem = false;
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e) => Limpiar();

        // ── Helpers UI ────────────────────────────────────────────────────────

        private void MostrarCargando()
        {
            Dropdown.Visibility         = Visibility.Visible;
            PnlCargando.Visibility      = Visibility.Visible;
            ListaResultados.Visibility  = Visibility.Collapsed;
            TxtSinResultados.Visibility = Visibility.Collapsed;
        }

        private void MostrarResultados(IEnumerable<ClienteDto> resultados)
        {
            PnlCargando.Visibility = Visibility.Collapsed;
            var lista = new List<ClienteDto>(resultados);

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

        private void CerrarDropdown() => Dropdown.Visibility = Visibility.Collapsed;

        private void SeleccionarCliente(ClienteDto cliente)
        {
            ClienteSeleccionado   = cliente;
            TxtBusqueda.Text      = cliente.Nombre;
            BtnLimpiar.Visibility = Visibility.Visible;
            CerrarDropdown();
            ClienteElegido?.Invoke(this, cliente);
        }

        // ── Mock para desarrollo ───────────────────────────────────────────────

        private IEnumerable<ClienteDto> BusquedaMock(string texto)
        {
            var mock = new List<ClienteDto>
            {
                new() { IdCliente = 1, Nombre = "Tech Solutions SRL",   Documento = 30123456, Telefono = 3794001122, Activo = true },
                new() { IdCliente = 2, Nombre = "Fernández, Roberto",   Documento = 25678901, Telefono = 3794003344, Activo = true },
                new() { IdCliente = 3, Nombre = "Oficinas Modernas SA", Documento = 30987654, Telefono = 3794005566, Activo = true },
                new() { IdCliente = 4, Nombre = "Gómez, Patricia",      Documento = 28345678, Telefono = 3794007788, Activo = true },
            };
            return mock.FindAll(c =>
                c.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                c.Documento.ToString().Contains(texto));
        }
    }
}
