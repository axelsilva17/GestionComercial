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

        private TipoDescuentoEnum _tipoSeleccionado = TipoDescuentoEnum.Producto;
        public TipoDescuentoEnum TipoSeleccionado
        {
            get => _tipoSeleccionado;
            set
            {
                _tipoSeleccionado = value;
                NotifyOfPropertyChange(() => TipoSeleccionado);
                NotifyOfPropertyChange(() => TipoSeleccionadoStr);
                NotifyOfPropertyChange(() => EsTipoProducto);
                NotifyOfPropertyChange(() => EsTipoCategoria);
                NotifyOfPropertyChange(() => EsTipoMetodoPago);
                if (value != TipoDescuentoEnum.MetodoPago)
                    IdMetodoPago = null;
            }
        }

        public string TipoSeleccionadoStr
        {
            get => TipoSeleccionado.ToString();
            set
            {
                if (System.Enum.TryParse<TipoDescuentoEnum>(value, out var parsed))
                    TipoSeleccionado = parsed;
            }
        }

        public bool EsTipoProducto => TipoSeleccionado == TipoDescuentoEnum.Producto;
        public bool EsTipoCategoria => TipoSeleccionado == TipoDescuentoEnum.Categoria;
        public bool EsTipoMetodoPago => TipoSeleccionado == TipoDescuentoEnum.MetodoPago;

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

        private int? _idMetodoPago;
        public int? IdMetodoPago
        {
            get => _idMetodoPago;
            set { _idMetodoPago = value; NotifyOfPropertyChange(() => IdMetodoPago); }
        }

        // ── Métodos de pago para selector ───────────────────────────────
        private ObservableCollection<MetodoPago> _metodosPago = new();
        public ObservableCollection<MetodoPago> MetodosPago
        {
            get => _metodosPago;
            set { _metodosPago = value; NotifyOfPropertyChange(() => MetodosPago); }
        }

        private MetodoPago? _metodoPagoSeleccionado;
        public MetodoPago? MetodoPagoSeleccionado
        {
            get => _metodoPagoSeleccionado;
            set
            {
                _metodoPagoSeleccionado = value;
                IdMetodoPago = value?.Id;
                NotifyOfPropertyChange(() => MetodoPagoSeleccionado);
            }
        }

        // ── Productos / Categorías para selectores ───────────────────────
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

        private int _prioridad;
        public int Prioridad
        {
            get => _prioridad;
            set { _prioridad = value; NotifyOfPropertyChange(() => Prioridad); }
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
                    Nombre = descuento.Nombre;
                    Valor = descuento.Valor;
                    TipoSeleccionado = descuento.Tipo;
                    IdProducto = descuento.Id_producto;
                    IdCategoria = descuento.Id_categoria;
                    IdMetodoPago = descuento.Id_metodoPago;
                    FechaDesde = descuento.FechaDesde;
                    FechaHasta = descuento.FechaHasta;
                    Prioridad = descuento.Prioridad;

                    // Preseleccionar producto/categoría/método de pago en los combos
                    if (IdProducto.HasValue)
                        ProductoSeleccionado = Productos.FirstOrDefault(p => p.IdProducto == IdProducto.Value);
                    if (IdCategoria.HasValue)
                        CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.IdCategoria == IdCategoria.Value);
                    if (IdMetodoPago.HasValue)
                        MetodoPagoSeleccionado = MetodosPago.FirstOrDefault(m => m.Id == IdMetodoPago.Value);
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
                MetodosPago = new ObservableCollection<MetodoPago>(
                    metodos.Where(m => m.Activo).OrderBy(m => m.Nombre));
            }
            catch
            {
                // Silently fail — selectors will be empty, validation will catch on save
            }
        }

        public async Task GuardarAsync()
        {
            LimpiarError();

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MostrarError("El nombre es requerido.");
                return;
            }
            if (Valor <= 0 || Valor > 100)
            {
                MostrarError("El valor debe ser entre 1 y 100.");
                return;
            }
            if (TipoSeleccionado == TipoDescuentoEnum.Producto && IdProducto == null)
            {
                MostrarError("Debe seleccionar un producto.");
                return;
            }
            if (TipoSeleccionado == TipoDescuentoEnum.Categoria && IdCategoria == null)
            {
                MostrarError("Debe seleccionar una categoría.");
                return;
            }
            if (TipoSeleccionado == TipoDescuentoEnum.MetodoPago && IdMetodoPago == null)
            {
                MostrarError("Debe seleccionar un método de pago.");
                return;
            }
            if (TipoSeleccionado != TipoDescuentoEnum.MetodoPago && IdMetodoPago != null)
            {
                MostrarError("El método de pago solo puede asignarse para tipo Método de Pago.");
                return;
            }
            if (FechaDesde.HasValue && FechaHasta.HasValue && FechaHasta < FechaDesde)
            {
                MostrarError("FechaHasta debe ser >= FechaDesde.");
                return;
            }

            try
            {
                if (EsModoEdicion)
                {
                    await _servicio.ActualizarAsync(
                        DescuentoId, Nombre, TipoSeleccionado, Valor,
                        IdProducto, IdCategoria, IdMetodoPago, FechaDesde, FechaHasta, Prioridad);
                }
                else
                {
                    await _servicio.CrearAsync(
                        _sesion.IdEmpresa, Nombre, TipoSeleccionado, Valor,
                        IdProducto, IdCategoria, IdMetodoPago, FechaDesde, FechaHasta, Prioridad);
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
}
