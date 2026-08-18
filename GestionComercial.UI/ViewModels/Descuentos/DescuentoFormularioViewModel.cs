using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Eventos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Descuentos
{
    public class DescuentoFormularioViewModel : NavigableViewModel
    {
        private readonly IDescuentoConfiguracionServicio _servicio;
        private readonly IProductoServicio _productoServicio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SesionServicio _sesion;
        private readonly IEventAggregator _eventAggregator;

        public DescuentoFormularioViewModel(
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

        public DescuentoListadoViewModel? ListadoRef { get; set; }
        public bool EsModoEdicion { get; set; }
        public int DescuentoId { get; set; }

        // ── Ámbito (qué se compra): Producto o Categoría ─────────────────
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
                if (value == "Producto") IdCategoria = null;
                else IdProducto = null;
            }
        }

        public bool MuestraSelectorProducto => AmbitoSeleccionado == "Producto";
        public bool MuestraSelectorCategoria => AmbitoSeleccionado == "Categoría";

        // ── Condición de pago ────────────────────────────────────────────
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

        public bool MuestraSelectorMetodosPago => !AplicaCualquierMetodoPago;

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

        private string GenerarNombre()
        {
            if (IdProducto != null)
                return $"{ProductoNombre} {Valor.ToString("0.##", CultureInfo.InvariantCulture)}%";
            if (IdCategoria != null)
                return $"Categoría {CategoriaNombre} {Valor.ToString("0.##", CultureInfo.InvariantCulture)}%";
            return string.Empty;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            Titulo = EsModoEdicion ? "Editar Descuento" : "Nuevo Descuento";

            await CargarSelectoresAsync();

            if (EsModoEdicion && DescuentoId > 0)
            {
                var descuento = await _servicio.ObtenerPorIdAsync(DescuentoId);
                if (descuento != null)
                {
                    Valor = descuento.Valor;
                    IdProducto = descuento.Id_producto;
                    IdCategoria = descuento.Id_categoria;
                    AplicaCualquierMetodoPago = descuento.AplicaCualquierMetodoPago;
                    FechaDesde = descuento.FechaDesde;
                    FechaHasta = descuento.FechaHasta;

                    // Inferir ámbito desde la FK seteada
                    AmbitoSeleccionado = descuento.Id_producto.HasValue ? "Producto" : "Categoría";

                    if (IdProducto.HasValue)
                        ProductoSeleccionado = Productos.FirstOrDefault(p => p.IdProducto == IdProducto.Value);
                    if (IdCategoria.HasValue)
                        CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.IdCategoria == IdCategoria.Value);

                    // Preseleccionar métodos de pago
                    foreach (var item in MetodosPagoDisponibles)
                        item.EstaSeleccionado = descuento.DescuentosMetodosPago.Any(dm => dm.Id_metodoPago == item.MetodoPago.Id);
                }
            }
        }

        private async Task CargarSelectoresAsync()
        {
            try
            {
                var productos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                Productos = new ObservableCollection<ProductoListadoDto>(productos);

                var categorias = await _productoServicio.ObtenerCategoriasAsync(_sesion.IdEmpresa);
                Categorias = new ObservableCollection<CategoriaItemDto>(categorias);

                var metodos = await _unitOfWork.MetodosPago.ObtenerTodosPorEmpresaAsync(_sesion.IdEmpresa);
                var lista = metodos
                    .Where(m => m.Activo && m.Categoria == "Tarjeta" && m.Subcategoria != null)
                    .OrderBy(m => m.Subcategoria == "Credito" ? 0 : 1)
                    .ThenBy(m => m.Nombre)
                    .Select(m => new MetodoPagoCheckItem(m))
                    .ToList();
                MetodosPagoDisponibles = new ObservableCollection<MetodoPagoCheckItem>(lista);

                // Configurar agrupación por Subcategoria
                var view = System.Windows.Data.CollectionViewSource.GetDefaultView(MetodosPagoDisponibles);
                view?.GroupDescriptions?.Clear();
                view?.GroupDescriptions?.Add(new System.Windows.Data.PropertyGroupDescription("MetodoPago.Subcategoria"));
            }
            catch
            {
                // Selectores vacíos; la validación de guardado cubre los casos inválidos
            }
        }

        public async Task GuardarAsync()
        {
            LimpiarError();

            if (Valor <= 0 || Valor > 100)
            {
                MostrarError("El valor debe ser entre 1 y 100.");
                return;
            }
            if (IdProducto == null && IdCategoria == null)
            {
                MostrarError("Debe seleccionar un producto o una categoría.");
                return;
            }
            if (IdProducto != null && IdCategoria != null)
            {
                MostrarError("No puede seleccionar producto y categoría a la vez.");
                return;
            }
            if (!AplicaCualquierMetodoPago
                && !MetodosPagoDisponibles.Any(m => m.EstaSeleccionado))
            {
                MostrarError("Debe indicar cualquier método o seleccionar al menos una tarjeta.");
                return;
            }
            if (FechaDesde.HasValue && FechaHasta.HasValue && FechaHasta < FechaDesde)
            {
                MostrarError("FechaHasta debe ser >= FechaDesde.");
                return;
            }

            var idsMetodosPago = MetodosPagoDisponibles
                .Where(m => m.EstaSeleccionado)
                .Select(m => m.MetodoPago.Id)
                .ToList();

            Nombre = GenerarNombre();

            try
            {
                if (EsModoEdicion)
                {
                    await _servicio.ActualizarAsync(
                        DescuentoId, Nombre, Valor,
                        IdProducto, IdCategoria, AplicaCualquierMetodoPago,
                        AplicaCualquierMetodoPago ? null : idsMetodosPago,
                        FechaDesde, FechaHasta);
                }
                else
                {
                    await _servicio.CrearAsync(
                        _sesion.IdEmpresa, Nombre, Valor,
                        IdProducto, IdCategoria, AplicaCualquierMetodoPago,
                        AplicaCualquierMetodoPago ? null : idsMetodosPago,
                        FechaDesde, FechaHasta);
                }

                await _eventAggregator.PublishOnUIThreadAsync(new DescuentosActualizadosEvent());

                if (ListadoRef != null)
                    await ListadoRef.BuscarAsync();

                await IoC.Get<ShellViewModel>().ActivateItemAsync(ListadoRef!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        public async Task EliminarAsync()
        {
            try
            {
                await _servicio.EliminarAsync(DescuentoId);
                await _eventAggregator.PublishOnUIThreadAsync(new DescuentosActualizadosEvent());

                if (ListadoRef != null)
                    await ListadoRef.BuscarAsync();

                await IoC.Get<ShellViewModel>().ActivateItemAsync(ListadoRef!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        public void Cancelar()
        {
            if (ListadoRef != null)
                IoC.Get<ShellViewModel>().ActivateItemAsync(ListadoRef, CancellationToken.None);
        }
    }

    // ── Item para checkbox de tarjeta ─────────────────────────────────────
    public class MetodoPagoCheckItem : PropertyChangedBase
    {
        public MetodoPago MetodoPago { get; }

        private bool _estaSeleccionado;
        public bool EstaSeleccionado
        {
            get => _estaSeleccionado;
            set { _estaSeleccionado = value; NotifyOfPropertyChange(() => EstaSeleccionado); }
        }

        public MetodoPagoCheckItem(MetodoPago metodoPago)
        {
            MetodoPago = metodoPago;
        }
    }
}