using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Eventos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Descuentos;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class DescuentoFormularioInnerViewModel : PropertyChangedBase
    {
        private readonly IDescuentoConfiguracionServicio _servicio;
        private readonly IProductoServicio _productoServicio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SesionServicio _sesion;
        private readonly IEventAggregator _eventAggregator;

        public DescuentoFormularioInnerViewModel(
            IDescuentoConfiguracionServicio servicio,
            IProductoServicio productoServicio,
            IUnitOfWork unitOfWork,
            SesionServicio sesion,
            IEventAggregator eventAggregator)
        {
            _servicio = servicio;
            _productoServicio = productoServicio;
            _unitOfWork = unitOfWork;
            _sesion = sesion;
            _eventAggregator = eventAggregator;
        }

        public DescuentoConfigViewModel? ParentRef { get; set; }
        public bool EsModoEdicion { get; set; }
        public int DescuentoId { get; set; }

        // ── Ambito ──
        private string _ambitoSeleccionado = "Producto";
        public string AmbitoSeleccionado
        {
            get => _ambitoSeleccionado;
            set
            {
                _ambitoSeleccionado = value;
                NotifyOfPropertyChange(() => AmbitoSeleccionado);
                NotifyOfPropertyChange(() => MuestraSelectorProducto);
                NotifyOfPropertyChange(() => MuestraSelectorCategoria);
                NotifyOfPropertyChange(() => MuestraAmbitoMetodoPago);
                NotifyOfPropertyChange(() => MuestraSelectorMetodosPago);
                NotifyOfPropertyChange(() => MuestraCompraMinima);
                if (value == "Producto")
                {
                    IdCategoria = null;
                    AplicaCualquierMetodoPago = true;
                }
                else if (value == "Categoría")
                {
                    IdProducto = null;
                    AplicaCualquierMetodoPago = true;
                }
                else if (value == "Método de Pago")
                {
                    IdProducto = null;
                    IdCategoria = null;
                    AplicaCualquierMetodoPago = false;
                }
                else if (value == "Compra Mayor")
                {
                    IdProducto = null;
                    IdCategoria = null;
                    AplicaCualquierMetodoPago = true;
                }
            }
        }

        public bool MuestraSelectorProducto => AmbitoSeleccionado == "Producto";
        public bool MuestraSelectorCategoria => AmbitoSeleccionado == "Categoría";
        public bool MuestraAmbitoMetodoPago => AmbitoSeleccionado == "Método de Pago";
        public bool MuestraCompraMinima => AmbitoSeleccionado == "Compra Mayor";

        // ── Condición de pago ──
        private bool _aplicaCualquierMetodoPago = true;
        public bool AplicaCualquierMetodoPago
        {
            get => _aplicaCualquierMetodoPago;
            set
            {
                _aplicaCualquierMetodoPago = value;
                NotifyOfPropertyChange(() => AplicaCualquierMetodoPago);
                NotifyOfPropertyChange(() => MuestraSelectorMetodosPago);
            }
        }

        public bool MuestraSelectorMetodosPago =>
            AmbitoSeleccionado == "Método de Pago"
            || (!AplicaCualquierMetodoPago && AmbitoSeleccionado != "Método de Pago");

        private ObservableCollection<MetodoPagoCheckItem> _metodosPagoDisponibles = new();
        public ObservableCollection<MetodoPagoCheckItem> MetodosPagoDisponibles
        {
            get => _metodosPagoDisponibles;
            set { _metodosPagoDisponibles = value; NotifyOfPropertyChange(() => MetodosPagoDisponibles); }
        }

        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; NotifyOfPropertyChange(() => Nombre); }
        }

        private decimal _valor;
        public decimal Valor
        {
            get => _valor;
            set { _valor = value; NotifyOfPropertyChange(() => Valor); }
        }

        private decimal? _montoMinimoCompra;
        public decimal? MontoMinimoCompra
        {
            get => _montoMinimoCompra;
            set { _montoMinimoCompra = value; NotifyOfPropertyChange(() => MontoMinimoCompra); }
        }

        private int? _idProducto;
        public int? IdProducto
        {
            get => _idProducto;
            set { _idProducto = value; NotifyOfPropertyChange(() => IdProducto); }
        }

        private string _productoNombre = string.Empty;
        public string ProductoNombre
        {
            get => _productoNombre;
            set { _productoNombre = value; NotifyOfPropertyChange(() => ProductoNombre); }
        }

        private int? _idCategoria;
        public int? IdCategoria
        {
            get => _idCategoria;
            set { _idCategoria = value; NotifyOfPropertyChange(() => IdCategoria); }
        }

        private string _categoriaNombre = string.Empty;
        public string CategoriaNombre
        {
            get => _categoriaNombre;
            set { _categoriaNombre = value; NotifyOfPropertyChange(() => CategoriaNombre); }
        }

        private ObservableCollection<ProductoListadoDto> _productos = new();
        public ObservableCollection<ProductoListadoDto> Productos
        {
            get => _productos;
            set { _productos = value; NotifyOfPropertyChange(() => Productos); }
        }

        // ── Búsqueda Producto ──────────────────────────────────────────────
        private string _textoBusquedaProducto = string.Empty;
        public string TextoBusquedaProducto
        {
            get => _textoBusquedaProducto;
            set
            {
                _textoBusquedaProducto = value;
                NotifyOfPropertyChange(() => TextoBusquedaProducto);
                FiltrarProductos();
            }
        }

        private ObservableCollection<ProductoListadoDto> _productosFiltrados = new();
        public ObservableCollection<ProductoListadoDto> ProductosFiltrados
        {
            get => _productosFiltrados;
            set { _productosFiltrados = value; NotifyOfPropertyChange(() => ProductosFiltrados); }
        }

        private bool _isUpdating;
        private ProductoListadoDto? _productoSeleccionadoBusqueda;
        public ProductoListadoDto? ProductoSeleccionadoBusqueda
        {
            get => _productoSeleccionadoBusqueda;
            set
            {
                if (_isUpdating) return;
                _isUpdating = true;
                try
                {
                    _productoSeleccionadoBusqueda = value;
                    if (value != null)
                    {
                        ProductoSeleccionado = value;
                        TextoBusquedaProducto = value.Nombre;
                        MostrarListaProductos = false;
                    }
                    NotifyOfPropertyChange(() => ProductoSeleccionadoBusqueda);
                }
                finally { _isUpdating = false; }
            }
        }

        private bool _mostrarListaProductos;
        public bool MostrarListaProductos
        {
            get => _mostrarListaProductos;
            set { _mostrarListaProductos = value; NotifyOfPropertyChange(() => MostrarListaProductos); }
        }

        // ── Búsqueda Categoría ─────────────────────────────────────────────
        private string _textoBusquedaCategoria = string.Empty;
        public string TextoBusquedaCategoria
        {
            get => _textoBusquedaCategoria;
            set
            {
                _textoBusquedaCategoria = value;
                NotifyOfPropertyChange(() => TextoBusquedaCategoria);
                FiltrarCategorias();
            }
        }

        private ObservableCollection<CategoriaItemDto> _categoriasFiltradas = new();
        public ObservableCollection<CategoriaItemDto> CategoriasFiltradas
        {
            get => _categoriasFiltradas;
            set { _categoriasFiltradas = value; NotifyOfPropertyChange(() => CategoriasFiltradas); }
        }

        private CategoriaItemDto? _categoriaSeleccionadaBusqueda;
        public CategoriaItemDto? CategoriaSeleccionadaBusqueda
        {
            get => _categoriaSeleccionadaBusqueda;
            set
            {
                if (_isUpdating) return;
                _isUpdating = true;
                try
                {
                    _categoriaSeleccionadaBusqueda = value;
                    if (value != null)
                    {
                        CategoriaSeleccionada = value;
                        TextoBusquedaCategoria = value.Nombre;
                        MostrarListaCategorias = false;
                    }
                    NotifyOfPropertyChange(() => CategoriaSeleccionadaBusqueda);
                }
                finally { _isUpdating = false; }
            }
        }

        private bool _mostrarListaCategorias;
        public bool MostrarListaCategorias
        {
            get => _mostrarListaCategorias;
            set { _mostrarListaCategorias = value; NotifyOfPropertyChange(() => MostrarListaCategorias); }
        }

        private ProductoListadoDto? _productoSeleccionado;
        public ProductoListadoDto? ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                IdProducto = value?.IdProducto;
                ProductoNombre = value?.Nombre ?? string.Empty;
                NotifyOfPropertyChange(() => ProductoSeleccionado);
            }
        }

        private ObservableCollection<CategoriaItemDto> _categorias = new();
        public ObservableCollection<CategoriaItemDto> Categorias
        {
            get => _categorias;
            set { _categorias = value; NotifyOfPropertyChange(() => Categorias); }
        }

        private CategoriaItemDto? _categoriaSeleccionada;
        public CategoriaItemDto? CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                IdCategoria = value?.IdCategoria;
                CategoriaNombre = value?.Nombre ?? string.Empty;
                NotifyOfPropertyChange(() => CategoriaSeleccionada);
            }
        }

        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        private string _mensajeError = string.Empty;
        public string MensajeError
        {
            get => _mensajeError;
            set { _mensajeError = value; NotifyOfPropertyChange(() => MensajeError); NotifyOfPropertyChange(() => TieneError); }
        }

        private bool _errorVisible;
        public bool TieneError
        {
            get => _errorVisible;
            set { _errorVisible = value; NotifyOfPropertyChange(() => TieneError); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; NotifyOfPropertyChange(() => IsLoading); }
        }

        // ── Filtrado de productos ─────────────────────────────────────────
        public void FiltrarProductos()
        {
            if (Productos == null) return;

            var texto = TextoBusquedaProducto?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(texto))
            {
                ProductosFiltrados = new ObservableCollection<ProductoListadoDto>(Productos);
                MostrarListaProductos = false;
            }
            else
            {
                ProductosFiltrados = new ObservableCollection<ProductoListadoDto>(
                    Productos.Where(p => p.Nombre.Contains(texto, System.StringComparison.OrdinalIgnoreCase)
                                      || (p.CodigoBarra != null && p.CodigoBarra.Contains(texto, System.StringComparison.OrdinalIgnoreCase))));
                if (ProductosFiltrados.Count > 0)
                    MostrarListaProductos = true;
            }
        }

        // ── Filtrado de categorías ───────────────────────────────────────
        public void FiltrarCategorias()
        {
            if (Categorias == null) return;

            var texto = TextoBusquedaCategoria?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(texto))
            {
                CategoriasFiltradas = new ObservableCollection<CategoriaItemDto>(Categorias);
            }
            else
            {
                CategoriasFiltradas = new ObservableCollection<CategoriaItemDto>(
                    Categorias.Where(c => c.Nombre.Contains(texto, System.StringComparison.OrdinalIgnoreCase)));
            }
            MostrarListaCategorias = CategoriasFiltradas.Count > 0 && !string.IsNullOrEmpty(texto);
        }

        private string GenerarNombre()
        {
            if (AmbitoSeleccionado == "Compra Mayor")
            {
                var monto = MontoMinimoCompra ?? 0;
                return $"Compra Mayor >= ${monto.ToString("N2", CultureInfo.InvariantCulture)} {Valor.ToString("0.##", CultureInfo.InvariantCulture)}%";
            }
            if (AmbitoSeleccionado == "Método de Pago")
            {
                var nombres = MetodosPagoDisponibles
                    .Where(m => m.EstaSeleccionado)
                    .Select(m => m.MetodoPago.Nombre)
                    .ToList();
                return $"Método {string.Join("/", nombres)} {Valor.ToString("0.##", CultureInfo.InvariantCulture)}%";
            }
            if (IdProducto != null)
                return $"{ProductoNombre} {Valor.ToString("0.##", CultureInfo.InvariantCulture)}%";
            if (IdCategoria != null)
                return $"Categoría {CategoriaNombre} {Valor.ToString("0.##", CultureInfo.InvariantCulture)}%";
            return string.Empty;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            try
            {
                var productos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                Productos = new ObservableCollection<ProductoListadoDto>(productos);
                ProductosFiltrados = new ObservableCollection<ProductoListadoDto>(productos);

                var categorias = await _productoServicio.ObtenerCategoriasAsync(_sesion.IdEmpresa);
                Categorias = new ObservableCollection<CategoriaItemDto>(categorias);
                CategoriasFiltradas = new ObservableCollection<CategoriaItemDto>(categorias);

                var metodos = await _unitOfWork.MetodosPago.ObtenerTodosPorEmpresaAsync(_sesion.IdEmpresa);
                var todosMetodos = metodos
                    .Where(m => m.Activo)
                    .OrderBy(m => m.Categoria)
                    .ThenBy(m => m.Nombre)
                    .Select(m => new MetodoPagoCheckItem(m))
                    .ToList();
                MetodosPagoDisponibles = new ObservableCollection<MetodoPagoCheckItem>(todosMetodos);

                var view = System.Windows.Data.CollectionViewSource.GetDefaultView(MetodosPagoDisponibles);
                view?.GroupDescriptions?.Clear();
                view?.GroupDescriptions?.Add(
                    new System.Windows.Data.PropertyGroupDescription("MetodoPago.Categoria"));

                if (EsModoEdicion && DescuentoId > 0)
                    await CargarDescuentoAsync();
            }
            catch { }
            finally { IsLoading = false; }
        }

        private async Task CargarDescuentoAsync()
        {
            var descuento = await _servicio.ObtenerPorIdAsync(DescuentoId);
            if (descuento == null) return;

            Valor = descuento.Valor;
            IdProducto = descuento.Id_producto;
            IdCategoria = descuento.Id_categoria;
            FechaDesde = descuento.FechaDesde;
            FechaHasta = descuento.FechaHasta;
            Nombre = descuento.Nombre;

            MontoMinimoCompra = descuento.MontoMinimoCompra;

            // IMPORTANTE: Set AmbitoSeleccionado PRIMERO, porque su setter sobrescribe AplicaCualquierMetodoPago
            AmbitoSeleccionado = descuento.Alcance switch
            {
                AlcanceDescuentoEnum.MetodoPago => "Método de Pago",
                AlcanceDescuentoEnum.Categoria => "Categoría",
                AlcanceDescuentoEnum.CompraMayor => "Compra Mayor",
                _ => "Producto"
            };

            // AHORA restauramos el valor real de AplicaCualquierMetodoPago desde la entidad
            AplicaCualquierMetodoPago = descuento.AplicaCualquierMetodoPago;

            if (IdProducto.HasValue)
                ProductoSeleccionado = Productos.FirstOrDefault(p => p.IdProducto == IdProducto.Value);
            if (IdCategoria.HasValue)
                CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.IdCategoria == IdCategoria.Value);

            foreach (var item in MetodosPagoDisponibles)
                item.EstaSeleccionado = descuento.DescuentosMetodosPago.Any(dm => dm.Id_metodoPago == item.MetodoPago.Id);
        }

        public async Task GuardarAsync()
        {
            MensajeError = string.Empty;
            TieneError = false;

            if (Valor <= 0 || Valor > 100)
            {
                MensajeError = "El valor debe ser entre 1 y 100.";
                TieneError = true;
                return;
            }

            var alcance = AmbitoSeleccionado switch
            {
                "Método de Pago" => AlcanceDescuentoEnum.MetodoPago,
                "Categoría" => AlcanceDescuentoEnum.Categoria,
                "Compra Mayor" => AlcanceDescuentoEnum.CompraMayor,
                _ => AlcanceDescuentoEnum.Producto
            };

            if (alcance == AlcanceDescuentoEnum.CompraMayor)
            {
                if (!MontoMinimoCompra.HasValue || MontoMinimoCompra <= 0)
                {
                    MensajeError = "Debe indicar el monto mínimo de compra.";
                    TieneError = true;
                    return;
                }
            }
            else if (alcance == AlcanceDescuentoEnum.MetodoPago)
            {
                if (AplicaCualquierMetodoPago)
                {
                    MensajeError = "Para descuentos por método de pago, no puede aplicar a cualquier método.";
                    TieneError = true;
                    return;
                }
                if (!MetodosPagoDisponibles.Any(m => m.EstaSeleccionado))
                {
                    MensajeError = "Debe seleccionar al menos un método de pago.";
                    TieneError = true;
                    return;
                }
            }
            else
            {
                if (IdProducto == null && IdCategoria == null)
                {
                    MensajeError = "Debe seleccionar un producto o una categoría.";
                    TieneError = true;
                    return;
                }
                if (IdProducto != null && IdCategoria != null)
                {
                    MensajeError = "No puede seleccionar producto y categoría a la vez.";
                    TieneError = true;
                    return;
                }
                if (!AplicaCualquierMetodoPago && !MetodosPagoDisponibles.Any(m => m.EstaSeleccionado))
                {
                    MensajeError = "Debe indicar cualquier método o seleccionar al menos una tarjeta.";
                    TieneError = true;
                    return;
                }
            }

            if (FechaDesde.HasValue && FechaHasta.HasValue && FechaHasta < FechaDesde)
            {
                MensajeError = "FechaHasta debe ser >= FechaDesde.";
                TieneError = true;
                return;
            }

            var idsMetodosPago = MetodosPagoDisponibles
                .Where(m => m.EstaSeleccionado)
                .Select(m => m.MetodoPago.Id)
                .ToList();

            if (!EsModoEdicion)
            {
                Nombre = GenerarNombre();
            }
            else if (string.IsNullOrWhiteSpace(Nombre))
            {
                // En edición: si el usuario dejó el nombre vacío, regenerarlo
                Nombre = GenerarNombre();
            }

            try
            {
                if (EsModoEdicion)
                {
                    await _servicio.ActualizarAsync(
                        DescuentoId, Nombre, Valor,
                        IdProducto, IdCategoria, AplicaCualquierMetodoPago,
                        AplicaCualquierMetodoPago ? null : idsMetodosPago,
                        FechaDesde, FechaHasta, alcance, MontoMinimoCompra);
                }
                else
                {
                    await _servicio.CrearAsync(
                        _sesion.IdEmpresa, Nombre, Valor,
                        IdProducto, IdCategoria, AplicaCualquierMetodoPago,
                        AplicaCualquierMetodoPago ? null : idsMetodosPago,
                        FechaDesde, FechaHasta, alcance, MontoMinimoCompra);
                }

                await _eventAggregator.PublishOnUIThreadAsync(new DescuentosActualizadosEvent());

                if (ParentRef != null)
                {
                    ParentRef.MostrarFormulario = false;
                    ParentRef.Formulario = null;
                    await ParentRef.CargarAsync();
                }
            }
            catch (System.Exception ex)
            {
                MensajeError = ex.Message;
                TieneError = true;
            }
        }

        public void Cancelar()
        {
            if (ParentRef != null)
            {
                ParentRef.MostrarFormulario = false;
                ParentRef.Formulario = null;
            }
        }
    }
}
