using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using FluentValidation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class VentaViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly IVentaServicio    _ventaServicio;
        private readonly SesionServicio    _sesion;
        private readonly IValidator<VentaCrearDto> _validator;

        // ── Timer para debounce de búsqueda ──────────────────────────────────────
        private readonly DispatcherTimer _debounceTimer;
        private CancellationTokenSource?  _debounceCts;
        private List<ProductoListadoDto> _productosCache = new(); // Cache de productos precargados

        // ── Scanner Fast-Entry Detection ──────────────────────────────────────────
        // Un escáner de código de barras tipea muy rápido (>8 chars en <500ms)
        // Typing manual es más lento
        private readonly DispatcherTimer _scannerTimer;
        private DateTime _typingStartTime;
        private string _scannerBuffer = string.Empty;
        private const int ScannerMinLength = 8;
        private const int ScannerMaxMs = 500;

        public VentaViewModel(
            IProductoServicio productoServicio,
            IVentaServicio    ventaServicio,
            SesionServicio    sesion,
            IValidator<VentaCrearDto> validator)
        {
            System.Diagnostics.Debug.WriteLine("[VentaVM] Constructor INICIO");

            try
            {
                _productoServicio     = productoServicio;
                _ventaServicio        = ventaServicio;
                _sesion               = sesion;
                _validator            = validator;
                Titulo                = "Nueva Venta";
                Items                 = new ObservableCollection<VentaItemDto>();
                ResultadosBusqueda    = new ObservableCollection<ProductoListadoDto>();
                SumarCantidadCommand  = new RelayCommand<VentaItemDto>(SumarCantidad);
                RestarCantidadCommand = new RelayCommand<VentaItemDto>(RestarCantidad);
                QuitarItemCommand     = new RelayCommand<VentaItemDto>(QuitarItem);
                AgregarItemDescuentoCommand = new RelayCommand<DescuentoItemParam>(AgregarItemConDescuento);
                AnularVentaCommand    = new RelayCommand(async () => await AnularVentaAsync(), () => _ventaActualId > 0);
                MostrarPopupAnulacionCommand = new RelayCommand(() => MostrarPopupAnulacion = true);
                CerrarPopupAnulacionCommand = new RelayCommand(() =>
                {
                    MotivoAnulacion = string.Empty;
                    MostrarPopupAnulacion = false;
                });
                SeleccionarProductoCommand = new RelayCommand<ProductoListadoDto>(SeleccionarProductoDelPopup);
                CerrarPopupBusquedaCommand  = new RelayCommand(CerrarPopupBusqueda);
                VerHistorialCommand = new RelayCommand(() => { _ = CargarHistorialAsync(); MostrarHistorial = true; });
                CerrarHistorialCommand = new RelayCommand(() => MostrarHistorial = false);
                FiltrarHistorialCommand = new RelayCommand(() => FiltrarHistorial());
                TestBarcodeCommand = new RelayCommand(TestBarcodeKeyDown);

                System.Diagnostics.Debug.WriteLine($"[VentaVM] Constructor: _sesion={_sesion?.GetType().Name ?? "NULL"}, Rol={_sesion?.Rol ?? "NULL"}");
                LimiteDescuento = _sesion?.Rol?.ToLowerInvariant() switch
                {
                    "gerente"       => 30m,
                    "administrador" => 15m,
                    _               => 5m,
                };

                // Configurar debounce de 300ms
                _debounceTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(300)
                };
                _debounceTimer.Tick += async (s, e) =>
                {
                    _debounceTimer.Stop();
                    _debounceCts?.Cancel();
                    _debounceCts = new CancellationTokenSource();
                    await BuscarProductosAsync(_textoDebounce, _debounceCts.Token);
                };

                // Configurar timer para detección de escáner (500ms para detectar typing lento)
                _scannerTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(ScannerMaxMs)
                };
                _scannerTimer.Tick += (s, e) =>
                {
                    _scannerTimer.Stop();
                    // Si pasaron 500ms y no se activó el scanner, es typing manual
                    // Limpiar el buffer
                    _scannerBuffer = string.Empty;
                };

                System.Diagnostics.Debug.WriteLine("[VentaVM] Constructor FIN OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Constructor ERROR: {ex.Message}\n{ex.StackTrace}");
                // No lanzar - permitir que el ViewModel se cree aunque haya error
                Titulo = "Nueva Venta";
                Items = new ObservableCollection<VentaItemDto>();
                ResultadosBusqueda = new ObservableCollection<ProductoListadoDto>();
                LimiteDescuento = 5m;
            }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            System.Diagnostics.Debug.WriteLine("[VentaVM] OnActivateAsync INICIO");

            // Precargar productos al abrir la vista de ventas
            if (_sesion.IdEmpresa > 0)
            {
                try
                {
                    var productos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                    // Guardar en cache para búsquedas rápidas
                    _productosCache = productos.ToList();
                    System.Diagnostics.Debug.WriteLine($"[VentaVM] OnActivateAsync: Cargados {_productosCache.Count} productos para IdEmpresa={_sesion.IdEmpresa}");


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[VentaVM] OnActivateAsync Error: {ex.Message}");
                    // No lanzar la excepción para que el ViewModel no quede en estado null
                    // El usuario podrá usar la vista aunque los productos no se carguen
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM] OnActivateAsync: IdEmpresa = {_sesion.IdEmpresa} (inválido)");
            }

            System.Diagnostics.Debug.WriteLine("[VentaVM] OnActivateAsync FIN");
        }

        private string _textoDebounce = string.Empty;

        // ── Límite de descuento según rol ──────────────────────────────────────
        public decimal LimiteDescuento { get; }


        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { SetProperty(ref _fechaDesde, value); }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { SetProperty(ref _fechaHasta, value); }
        }

        private string _dniClienteFiltro = string.Empty;
        public string DniClienteFiltro
        {
            get => _dniClienteFiltro;
            set { SetProperty(ref _dniClienteFiltro, value); }
        }

        private int? _estadoVentaFiltro;
        public int? EstadoVentaFiltro
        {
            get => _estadoVentaFiltro;
            set { SetProperty(ref _estadoVentaFiltro, value); }
        }

        // ── Feature 7: Test barcode ─────────────────────────────────────────────
        private string _testBarcodeInput = string.Empty;
        public string TestBarcodeInput
        {
            get => _testBarcodeInput;
            set { SetProperty(ref _testBarcodeInput, value); }
        }

        public RelayCommand TestBarcodeCommand { get; }

        /// <summary>
        /// Maneja atajos de teclado globales en la vista de venta.
        /// </summary>
        public void HandleKeyDown(Key key, ModifierKeys modifiers)
        {
            // Ignorar si hay ctrl/alt/shift presionados (excepto Ctrl+N para nueva venta)
            if (modifiers == ModifierKeys.Control && key == Key.N)
            {
                NuevaVenta();
                return;
            }
            if (modifiers != ModifierKeys.None) return;

            switch (key)
            {
                case Key.Enter:
                    _ = AgregarProducto();
                    break;
                case Key.F2:
                    if (Items.Count > 0) _ = IrACobrar();
                    break;
                case Key.F4:
                    _ = SeleccionarCliente();
                    break;
                case Key.F6:
                    _ = AgregarProducto();
                    break;
                case Key.Escape:
                    if (MostrarPopupAnulacion)
                        MostrarPopupAnulacion = false;
                    else
                        _ = Volver();
                    break;
            }
        }

        // ── Venta actual (para anulación) ───────────────────────────────────────
        private int _ventaActualId;
        public int VentaActualId
        {
            get => _ventaActualId;
            set { _ventaActualId = value; NotifyOfPropertyChange(() => VentaActualId); NotifyOfPropertyChange(() => HayVentaActiva); }
        }
        public bool HayVentaActiva => _ventaActualId > 0;

        // ── Popup de anulación ───────────────────────────────────────────────────
        private bool _mostrarPopupAnulacion;
        public bool MostrarPopupAnulacion
        {
            get => _mostrarPopupAnulacion;
            set { _mostrarPopupAnulacion = value; NotifyOfPropertyChange(() => MostrarPopupAnulacion); }
        }

        private string _motivoAnulacion = string.Empty;
        public string MotivoAnulacion
        {
            get => _motivoAnulacion;
            set { _motivoAnulacion = value; NotifyOfPropertyChange(() => MotivoAnulacion); }
        }

        public RelayCommand MostrarPopupAnulacionCommand { get; }
        public RelayCommand CerrarPopupAnulacionCommand { get; }
        public RelayCommand AnularVentaCommand { get; }

        /// <summary>
        /// Agrega un ítem con descuento por porcentaje.
        /// El descuento se calcula sobre el subtotal del ítem.
        /// </summary>
        public RelayCommand<DescuentoItemParam> AgregarItemDescuentoCommand { get; }

        /// <summary>
        /// Registra un pago y abre el popup de anulación si corresponde.
        /// </summary>
        public async Task AnularVentaAsync()
        {
            if (string.IsNullOrWhiteSpace(MotivoAnulacion))
            {
                MostrarError("Debe ingresar el motivo de anulación.");
                return;
            }

            if (VentaActualId <= 0)
            {
                MostrarError("No hay venta activa para anular.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                await _ventaServicio.CancelarAsync(VentaActualId, MotivoAnulacion);
                MotivoAnulacion = string.Empty;
                MostrarPopupAnulacion = false;
                MostrarMensaje("Venta anulada correctamente.");
                NuevaVenta();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al anular: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Feature 4: Anula una venta desde el historial.
        /// Muestra confirmación y luego el popup para ingresar el motivo.
        /// </summary>
        public void AnularVentaDesdeHistorial(VentaResumenDto? ventaResumen)
        {
            if (ventaResumen == null) return;

            // Verificar que la venta no esté ya anulada
            if (ventaResumen.Estado == "Anulada")
            {
                MessageBox.Show("Esta venta ya está anulada.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Mostrar confirmación
            var result = MessageBox.Show(
                $"¿Está seguro que desea anular la venta #{ventaResumen.IdVenta}?\n\nEsta acción no se puede deshacer.",
                "Confirmar anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            // Setear el ID de la venta activa y mostrar popup de motivo
            VentaActualId = ventaResumen.IdVenta;
            MotivoAnulacion = string.Empty;
            MostrarPopupAnulacion = true;
        }

        private void AgregarItemConDescuento(DescuentoItemParam? param)
        {
            if (param?.Producto == null || param.Cantidad <= 0) return;

            var existente = Items.FirstOrDefault(i => i.ProductoId == param.Producto.IdProducto);
            if (existente != null)
            {
                // Actualizar cantidad y recalcular con descuento
                var idx = Items.IndexOf(existente);
                existente.Cantidad += param.Cantidad;
                existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;

                // Aplicar descuento por ítem
                if (param.PorcentajeDescuento > 0)
                {
                    existente.DescuentoPorItem = Math.Round(existente.Subtotal * param.PorcentajeDescuento / 100, 2);
                }

                Items.RemoveAt(idx);
                Items.Insert(idx, existente);
            }
            else
            {
                var subtotal = param.Cantidad * param.Producto.PrecioVentaActual;
                var descuento = param.PorcentajeDescuento > 0
                    ? Math.Round(subtotal * param.PorcentajeDescuento / 100, 2)
                    : 0m;

                Items.Add(new VentaItemDto
                {
                    ProductoId          = param.Producto.IdProducto,
                    ProductoNombre      = param.Producto.Nombre,
                    CodigoBarra         = param.Producto.CodigoBarra ?? string.Empty,
                    Cantidad            = param.Cantidad,
                    PrecioUnitario      = param.Producto.PrecioVentaActual,
                    CostoUnitario       = param.Producto.PrecioCostoActual,
                    Subtotal            = subtotal,
                    DescuentoPorItem    = descuento,
                });
            }

            RecalcularTotales();
        }

        private void MostrarMensaje(string mensaje)
        {
            // TODO: Integrar con sistema de notificaciones/snackbar cuando esté disponible
            System.Diagnostics.Debug.WriteLine($"[VentaVM] {mensaje}");
        }

        // ── Cliente ───────────────────────────────────────────────────────────
        private string _clienteNombre = string.Empty;
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); }
        }

        private int _clienteId;
        public int ClienteId
        {
            get => _clienteId;
            set { _clienteId = value; NotifyOfPropertyChange(() => ClienteId); }
        }

        // ── Búsqueda (texto libre O código de barras del escáner) ─────────────
        private string _busquedaProducto = string.Empty;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set
            {
                if (SetProperty(ref _busquedaProducto, value) && !string.IsNullOrWhiteSpace(value))
                {
                    // ── Scanner Fast-Entry Detection ───────────────────────────────
                    // Detectar si es输入 rápida (escáner) o typing manual
                    var now = DateTime.Now;
                    
                    if (value.Length >= ScannerMinLength)
                    {
                        // Primer caracter del buffer o después de un timeout
                        if (string.IsNullOrEmpty(_scannerBuffer))
                        {
                            _typingStartTime = now;
                            _scannerBuffer = value;
                            _scannerTimer.Stop();
                            _scannerTimer.Start();
                        }
                        else
                        {
                            // Ya tenemos caracteres previos - verificar si es entrada rápida
                            var elapsed = (now - _typingStartTime).TotalMilliseconds;
                            
                            // Si vienen más de 8 chars en menos de 500ms desde el inicio → ESCÁNER
                            if (elapsed < ScannerMaxMs && value.Length >= ScannerMinLength)
                            {
                                // ¡ESCÁNER DETECTADO! → Buscar por código de barras directamente
                                _scannerTimer.Stop();
                                System.Diagnostics.Debug.WriteLine($"[VentaVM] ESCÁNER DETECTADO: {value} en {elapsed}ms");
                                _scannerBuffer = string.Empty;
                                _ = ProcesarBarcodeEscaneadoAsync(value);
                                return; // No hacer debounce normal
                            }
                            
                            // Actualizar buffer y tiempo
                            _scannerBuffer = value;
                            _typingStartTime = now;
                        }
                    }
                    else
                    {
                        // Menos de 8 chars - es typing manual normal
                        _scannerBuffer = string.Empty;
                        _scannerTimer.Stop();
                    }

                    // ── Debounce normal para búsqueda ───────────────────────────────
                    if (!string.IsNullOrWhiteSpace(value) && value.Length >= 3)
                    {
                        _textoDebounce = value;
                        _debounceTimer.Stop();
                        _debounceTimer.Start();
                    }
                    else
                    {
                        // Ocultar popup si el texto es muy corto
                        MostrarPopupBusqueda = false;
                    }
                }
            }
        }

        /// <summary>
        /// Procesa un código de barras escaneado rápidamente.
        /// Busca por código exacto y agrega el producto si existe.
        /// </summary>
        private async Task ProcesarBarcodeEscaneadoAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return;

            try
            {
                // Limpiar campo de búsqueda
                _busquedaProducto = string.Empty;
                MostrarPopupBusqueda = false;

                // Buscar por código de barras exacto en cache
                var producto = _productosCache.FirstOrDefault(p =>
                    p.CodigoBarra != null &&
                    p.CodigoBarra.Trim().Equals(barcode.Trim(), StringComparison.OrdinalIgnoreCase));

                if (producto == null)
                {
                    // Si no está en cache, buscar en servicio
                    var todos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                    producto = todos.FirstOrDefault(p =>
                        p.CodigoBarra != null &&
                        p.CodigoBarra.Trim().Equals(barcode.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (producto == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[VentaVM] Escáner: código {barcode} no encontrado");
                    return;
                }

                if (producto.StockActual <= 0)
                {
                    MostrarError($"'{producto.Nombre}' no tiene stock disponible.");
                    return;
                }

                // Agregar el producto directamente (como si fuera el escáner físico)
                var dtoParaAgregar = new ProductoListadoDto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    CodigoBarra = producto.CodigoBarra,
                    PrecioVentaActual = producto.PrecioVentaActual,
                    PrecioCostoActual = producto.PrecioCostoActual,
                    StockActual = producto.StockActual,
                };

                SeleccionarProductoDelPopup(dtoParaAgregar);
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Escáner: agregado {producto.Nombre}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Error procesando barcode escaneado: {ex.Message}");
            }
        }

        // ── Autocompletado de productos ─────────────────────────────────────────
        private ObservableCollection<ProductoListadoDto> _resultadosBusqueda;
        public ObservableCollection<ProductoListadoDto> ResultadosBusqueda
        {
            get => _resultadosBusqueda;
            set => SetProperty(ref _resultadosBusqueda, value);
        }

        private bool _mostrarPopupBusqueda;
        public bool MostrarPopupBusqueda
        {
            get => _mostrarPopupBusqueda;
            set => SetProperty(ref _mostrarPopupBusqueda, value);
        }

        private bool _haySinResultados;
        public bool HaySinResultados
        {
            get => _haySinResultados;
            set => SetProperty(ref _haySinResultados, value);
        }

        private bool _buscandoProductos;
        public bool BuscandoProductos
        {
            get => _buscandoProductos;
            set => SetProperty(ref _buscandoProductos, value);
        }

        public RelayCommand<ProductoListadoDto> SeleccionarProductoCommand { get; }
        public RelayCommand CerrarPopupBusquedaCommand { get; }

        /// <summary>
        /// Busca productos con debounce de 300ms para autocompletado.
        /// Primero usa cache local, luego consulta servicio si no hay cache.
        /// </summary>
        private async Task BuscarProductosAsync(string texto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(texto) || texto.Length < 3)
            {
                MostrarPopupBusqueda = false;
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[VentaVM] BuscarProductosAsync called. Texto: {texto}, Cache count: {_productosCache.Count}");

            BuscandoProductos = true;
            try
            {
                IEnumerable<ProductoListadoDto> todos;

                // Si tenemos productos en cache, buscar ahí primero
                if (_productosCache.Count > 0 && _sesion.IdEmpresa > 0)
                {
                    var busqueda = texto.Trim().ToLowerInvariant();
                    var resultadosCache = _productosCache
                        .Where(p => (p.Nombre?.ToLowerInvariant().Contains(busqueda) ?? false) ||
                                   (p.CodigoBarra?.ToLowerInvariant().Contains(busqueda) ?? false))
                        .Take(8)
                        .ToList();

                    if (resultadosCache.Count > 0)
                    {
                        ResultadosBusqueda = new ObservableCollection<ProductoListadoDto>(resultadosCache);
                        HaySinResultados = false;
                        MostrarPopupBusqueda = true;
                        System.Diagnostics.Debug.WriteLine($"[VentaVM] Resultados desde cache: {resultadosCache.Count}");
                        return; // Usar cache
                    }
                }

                // Si no hay resultados en cache o no hay cache, consultar servicio
                todos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var textoBusqueda = texto.Trim().ToLowerInvariant();

                // Buscar por nombre que contenga el texto
                var resultados = todos
                    .Where(p => (p.Nombre?.ToLowerInvariant().Contains(textoBusqueda) ?? false) ||
                               (p.CodigoBarra?.ToLowerInvariant().Contains(textoBusqueda) ?? false))
                    .Take(8) // Limitar a 8 resultados para el popup
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"[VentaVM] Resultados: {resultados.Count}");
                ResultadosBusqueda = new ObservableCollection<ProductoListadoDto>(resultados);
                HaySinResultados = resultados.Count == 0;
                MostrarPopupBusqueda = true;
                System.Diagnostics.Debug.WriteLine($"[VentaVM] MostrarPopupBusqueda set to true");
            }
            catch (OperationCanceledException)
            {
                // Ignorar si se canceló por nuevo debounce
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Error en búsqueda: {ex.Message}");
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BuscandoProductos = false;
            }
        }

        /// <summary>
        /// Selecciona un producto del popup de autocompletado y lo agrega al carrito.
        /// </summary>
        public void SeleccionarProductoDelPopup(ProductoListadoDto? producto)
        {
            if (producto == null) return;

            if (producto.StockActual <= 0)
            {
                MostrarError($"'{producto.Nombre}' no tiene stock disponible.");
                return;
            }

            var existente = Items.FirstOrDefault(i => i.ProductoId == producto.IdProducto);
            if (existente != null)
            {
                if (existente.Cantidad >= producto.StockActual)
                {
                    MostrarError($"Stock máximo: {producto.StockActual}");
                    return;
                }
                var idx = Items.IndexOf(existente);
                existente.Cantidad++;
                existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
                Items.RemoveAt(idx);
                Items.Insert(idx, existente);
            }
            else
            {
                Items.Add(new VentaItemDto
                {
                    ProductoId     = producto.IdProducto,
                    ProductoNombre = producto.Nombre,
                    CodigoBarra    = producto.CodigoBarra ?? string.Empty,
                    Cantidad       = 1,
                    PrecioUnitario = producto.PrecioVentaActual,
                    CostoUnitario  = producto.PrecioCostoActual,
                    Subtotal       = producto.PrecioVentaActual,
                });
            }

            BusquedaProducto = string.Empty;
            MostrarPopupBusqueda = false;
            RecalcularTotales();
            NotificarCanIrACobrar();
        }

        /// <summary>
        /// Cierra el popup de búsqueda y limpia el campo.
        /// </summary>
        public void CerrarPopupBusqueda()
        {
            MostrarPopupBusqueda = false;
            ResultadosBusqueda?.Clear();
        }

        /// <summary>
        /// Refresca los productos según el filtro de categoría seleccionado.
        /// Llamado desde el SelectionChanged del ComboBox de categorías.
        /// </summary>
        public Task RefrescarProductosPorCategoriaAsync()
        {
            // Feature 2 disabled - category filter removed per change cierre-caja-sin-estres
            return Task.CompletedTask;
        }

        /// <summary>
        /// Refresca el cache de productos desde el servidor.
        /// </summary>
        private async Task RefrescarCacheProductosAsync()
        {
            try
            {
                var productos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                _productosCache.Clear();
                foreach (var p in productos)
                {
                    _productosCache.Add(p);
                }
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Cache de productos refrescado: {_productosCache.Count} productos");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Error refrescando cache: {ex.Message}");
            }
        }

        // ── Items del carrito ─────────────────────────────────────────────────
        private ObservableCollection<VentaItemDto> _items = new();
        public ObservableCollection<VentaItemDto> Items
        {
            get => _items;
            set { SetProperty(ref _items, value); NotifyOfPropertyChange(() => CanIrACobrar); }
        }

        public bool CanIrACobrar => Items.Count > 0;

        // ── Totales ───────────────────────────────────────────────────────────
        private decimal _totalBruto;
        public decimal TotalBruto
        {
            get => _totalBruto;
            set { _totalBruto = value; NotifyOfPropertyChange(() => TotalBruto); }
        }

        private decimal _totalDescuento;
        public decimal TotalDescuento
        {
            get => _totalDescuento;
            set { _totalDescuento = value; NotifyOfPropertyChange(() => TotalDescuento); }
        }

        private decimal _totalFinal;
        public decimal TotalFinal
        {
            get => _totalFinal;
            set { _totalFinal = value; NotifyOfPropertyChange(() => TotalFinal); }
        }

        private string _descuentoManual = string.Empty;
        public string DescuentoManual
        {
            get => _descuentoManual;
            set { _descuentoManual = value; NotifyOfPropertyChange(() => DescuentoManual); RecalcularTotales(); }
        }

        // ── Commands para botones dentro del DataGrid ─────────────────────────
        public RelayCommand<VentaItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<VentaItemDto> RestarCantidadCommand { get; }
        public RelayCommand<VentaItemDto> QuitarItemCommand     { get; }

        // ── Notificar CanIrACobrar cuando cambian los items ─────────────────────
        private void NotificarCanIrACobrar()
        {
            NotifyOfPropertyChange(() => CanIrACobrar);
        }

        // ── Acciones públicas (bindeadas desde la View) ───────────────────────

        public async Task SeleccionarCliente()
        {
            var vm = IoC.Get<SeleccionClienteViewModel>();
            vm.VentaOrigen = this;
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        /// <summary>
        /// Busca por nombre O por código de barras exacto.
        /// El escáner físico envía el código como texto + Enter,
        /// así que este método se llama igual desde el botón o desde KeyDown Enter.
        /// </summary>
        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;
            IsLoading = true;
            LimpiarError();
            try
            {
                var todos    = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var busqueda = BusquedaProducto.Trim().ToLower();

                // Prioridad 1: código de barras exacto (escáner)
                var producto = todos.FirstOrDefault(p =>
                        p.CodigoBarra != null &&
                        p.CodigoBarra.Trim().ToLower() == busqueda)
                    // Prioridad 2: nombre contiene el texto
                    ?? todos.FirstOrDefault(p =>
                        p.Nombre.ToLower().Contains(busqueda));

                if (producto == null)
                {
                    MostrarError($"No se encontró ningún producto con '{BusquedaProducto}'.");
                    return;
                }

                if (producto.StockActual <= 0)
                {
                    MostrarError($"'{producto.Nombre}' no tiene stock disponible.");
                    return;
                }

                var existente = Items.FirstOrDefault(i => i.ProductoId == producto.IdProducto);
                if (existente != null)
                {
                    if (existente.Cantidad >= producto.StockActual)
                    {
                        MostrarError($"Stock máximo: {producto.StockActual}");
                        return;
                    }
                    var idx = Items.IndexOf(existente);
                    existente.Cantidad++;
                    existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
                    Items.RemoveAt(idx);
                    Items.Insert(idx, existente);
                }
                else
                {
                    Items.Add(new VentaItemDto
                    {
                        ProductoId     = producto.IdProducto,
                        ProductoNombre = producto.Nombre,
                        CodigoBarra    = producto.CodigoBarra ?? string.Empty,
                        Cantidad       = 1,
                        PrecioUnitario = producto.PrecioVentaActual,
                        CostoUnitario  = producto.PrecioCostoActual,
                        Subtotal       = producto.PrecioVentaActual,
                    });
                }

                BusquedaProducto = string.Empty;
                RecalcularTotales();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        /// <summary>
        /// Crea la venta como Pendiente en BD y navega a PagoViewModel.
        /// El stock ya se descuenta al crear — si el pago falla/cancela,
        /// el operador puede anular la venta y el stock se repone.
        /// </summary>
        public async Task IrACobrar()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] =======================================");
                System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] Iniciando proceso de cobro...");
                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] Validación: IdCajaActual={_sesion.IdCajaActual}, IdSucursal={_sesion.IdSucursal}");
                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] Validación: Items.Count={Items.Count}, ClienteId={ClienteId}");

                // Verificar que hay una caja abierta
                if (!_sesion.IdCajaActual.HasValue)
                {
                    System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] VALIDACIÓN FALLIDA: Caja no abierta (IdCajaActual es null)");
                    MessageBox.Show("Debe abrir una caja antes de realizar ventas.", "Caja cerrada", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] VALIDACIÓN OK: Caja abierta");

                if (Items.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] VALIDACIÓN FALLIDA: Carrito vacío (Items.Count=0)");
                    MostrarError("Agregá al menos un producto."); 
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] VALIDACIÓN OK: Carrito con {Items.Count} items");

                if (TotalFinal <= 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] VALIDACIÓN FALLIDA: TotalFinal={TotalFinal} <= 0");
                    MessageBox.Show("El total debe ser mayor a cero.", "Total inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] VALIDACIÓN OK: TotalFinal={TotalFinal} > 0");

                IsLoading = true;
                LimpiarError();

                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] Items count: {Items.Count}, Total: {TotalFinal}");

                var dto = new VentaCrearDto
                {
                    IdSucursal     = _sesion.IdSucursal,
                    IdCliente      = ClienteId > 0 ? ClienteId : 1, // 1 = consumidor final
                    IdUsuario      = _sesion.IdUsuario,
                    IdCaja         = _sesion.IdCajaActual ?? 0,
                    TotalDescuento = TotalDescuento,
                    TotalFinal     = TotalFinal,
                    Items          = Items.Select(i => new VentaDetalleCrearDto
                    {
                        IdProducto     = i.ProductoId,
                        Cantidad       = (int)i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario,
                        CostoUnitario  = i.CostoUnitario,
                        // Descuentos por ítem: enviar si hay alguno
                        Descuentos = i.DescuentoPorItem > 0
                            ? new List<DescuentoItemDto>
                            {
                                new DescuentoItemDto
                                {
                                    Porcentaje = 0, // Ya calculado el monto
                                    Monto      = i.DescuentoPorItem,
                                    Descripcion = "Descuento por ítem"
                                }
                            }
                            : new List<DescuentoItemDto>(),
                    }).ToList(),
                };

                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] DTO creado con {dto.Items.Count} items");

                // Validar con FluentValidation antes de crear
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
                    System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] ERROR Validación: {errores}");
                    MostrarError(errores);
                    return;
                }

                System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] Llamando a VentaServicio.CrearAsync...");
                var venta = await _ventaServicio.CrearAsync(dto);
                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] Venta creada con ID: {venta.IdVenta}");

                // Guardar ID para posible anulación
                VentaActualId = venta.IdVenta;

                System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] Navegando a PagoViewModel...");
                var vm = IoC.Get<PagoViewModel>();
                vm.InicializarConVenta(
                    venta.IdVenta,
                    ClienteNombre.Length > 0 ? ClienteNombre : "Consumidor Final",
                    venta.TotalFinal);

                await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
                System.Diagnostics.Debug.WriteLine("[VentaVM-IrACobrar] Proceso completado");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM-IrACobrar] ERROR: {ex}");
                MessageBox.Show($"Error al procesar: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        /// <summary>Resetea el formulario para una nueva venta.</summary>
        public void NuevaVenta()
        {
            Items            = new();
            ClienteId        = 0;
            ClienteNombre    = string.Empty;
            DescuentoManual  = string.Empty;
            BusquedaProducto = string.Empty;
            ResultadosBusqueda?.Clear();
            MostrarPopupBusqueda = false;
            RecalcularTotales();
            LimpiarError();
        }

        public async Task Volver()
        {
            // Stay on Venta screen - show recent sales instead
            await CargarHistorialAsync();
            MostrarHistorial = true;
        }

        // ── Lógica interna ────────────────────────────────────────────────────
        private void SumarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            var idx = Items.IndexOf(item);
            item.Cantidad++;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
            RecalcularTotales();
            NotificarCanIrACobrar();
        }

        private void RestarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad <= 1) { QuitarItem(item); return; }
            var idx = Items.IndexOf(item);
            item.Cantidad--;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
            RecalcularTotales();
            NotificarCanIrACobrar();
        }

        private void QuitarItem(VentaItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotales();
            NotificarCanIrACobrar();
        }

        private void RecalcularTotales()
        {
            TotalBruto = Items.Sum(i => i.Subtotal);
            // Incluir descuentos por ítem + descuento general
            var descuentoPorItem = Items.Sum(i => i.DescuentoPorItem);
            decimal pct = 0;
            string? errorDescuento = null;

            if (decimal.TryParse(DescuentoManual, out var d))
            {
                // Validar límite según rol - ELIMINAR EL TOPE PARA PERMITIR CUALQUIER %
                // El usuario puede poner el % que quiera (ej: 50%)
                pct = Math.Clamp(d, 0, 100);
                System.Diagnostics.Debug.WriteLine($"[VentaVM] Descuento aplicado: {pct}% sobre TotalBruto={TotalBruto}");
            }

            var descuentoGeneral = Math.Round(TotalBruto * pct / 100, 2);
            TotalDescuento = descuentoPorItem + descuentoGeneral;
            TotalFinal     = TotalBruto - TotalDescuento;

            // Mostrar error de descuento si corresponde
            if (errorDescuento != null)
                MostrarError(errorDescuento);
            else
                LimpiarError();

            NotifyOfPropertyChange(() => CanIrACobrar);
        }

        // ── Popup Historial ──────────────────────────────────────────────────
        private bool _mostrarHistorial;
        public bool MostrarHistorial
        {
            get => _mostrarHistorial;
            set { _mostrarHistorial = value; NotifyOfPropertyChange(() => MostrarHistorial); }
        }

        private ObservableCollection<VentaResumenDto> _historialVentas = new();
        public ObservableCollection<VentaResumenDto> HistorialVentas
        {
            get => _historialVentas;
            set { _historialVentas = value; NotifyOfPropertyChange(() => HistorialVentas); }
        }

        public RelayCommand VerHistorialCommand { get; }
        public RelayCommand CerrarHistorialCommand { get; }
        public RelayCommand FiltrarHistorialCommand { get; }

        public async Task CargarHistorialAsync()
        {
            try
            {
                // Por defecto: últimos 30 días (spec dice 30 días, no solo hoy)
                var desde = FechaDesde ?? DateTime.Today.AddDays(-30);
                var hasta = FechaHasta ?? DateTime.Today.AddDays(1).AddSeconds(-1);

                System.Diagnostics.Debug.WriteLine($"[VentaVM] CargarHistorialAsync: IdSucursal={_sesion.IdSucursal}, desde={desde:yyyy-MM-dd}, hasta={hasta:yyyy-MM-dd HH:mm:ss}");
                System.Diagnostics.Debug.WriteLine($"[VentaVM] CargarHistorialAsync: Filtros - FechaDesde={FechaDesde}, FechaHasta={FechaHasta}, Dni={DniClienteFiltro}, Estado={EstadoVentaFiltro}");

                IEnumerable<VentaResumenDto> ventas;

                // Si hay filtros activos, usar el método con filtros
                if (FechaDesde.HasValue || FechaHasta.HasValue || !string.IsNullOrWhiteSpace(DniClienteFiltro) || EstadoVentaFiltro.HasValue)
                {
                    ventas = await _ventaServicio.ObtenerVentasAsync(
                        _sesion.IdSucursal, FechaDesde, FechaHasta, DniClienteFiltro, EstadoVentaFiltro);
                }
                else
                {
                    ventas = await _ventaServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal, desde, hasta);
                }

                var listaVentas = ventas.Take(50).ToList();
                System.Diagnostics.Debug.WriteLine($"[VentaVM] CargarHistorialAsync: {listaVentas.Count} ventas encontradas");

                HistorialVentas = new ObservableCollection<VentaResumenDto>(listaVentas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VentaVM] CargarHistorialAsync ERROR: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Feature 3: Filtra el historial aplicando los filtros activos.
        /// </summary>
        public void FiltrarHistorial()
        {
            _ = CargarHistorialAsync();
        }

        /// <summary>
        /// Feature 7: Test barcode - procesa el código de barras del input de test.
        /// </summary>
        public async void TestBarcodeKeyDown()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TestBarcodeInput)) return;

                var codigo = TestBarcodeInput.Trim();
                TestBarcodeInput = string.Empty;

                System.Diagnostics.Debug.WriteLine($"[VentaVM] TestBarcodeKeyDown: {codigo}");

                // Buscar por código de barras exacto
                var todos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var producto = todos.FirstOrDefault(p =>
                    p.CodigoBarra != null &&
                    p.CodigoBarra.Trim().Equals(codigo, StringComparison.OrdinalIgnoreCase));

                if (producto == null)
                {
                    MessageBox.Show($"No se encontró ningún producto con código de barras '{codigo}'.", 
                        "Código no encontrado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (producto.StockActual <= 0)
                {
                    MessageBox.Show($"'{producto.Nombre}' no tiene stock disponible.", 
                        "Sin stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Agregar el producto al carrito (usar el método existente de selección)
                var dtoParaAgregar = new ProductoListadoDto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    CodigoBarra = producto.CodigoBarra,
                    PrecioVentaActual = producto.PrecioVentaActual,
                    PrecioCostoActual = producto.PrecioCostoActual,
                    StockActual = producto.StockActual,
                };

                SeleccionarProductoDelPopup(dtoParaAgregar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// Parámetros para agregar un ítem con descuento.
    /// </summary>
    public class DescuentoItemParam
    {
        public ProductoDto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PorcentajeDescuento { get; set; }
    }
}
