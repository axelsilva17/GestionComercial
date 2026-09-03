using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Descuentos;
using GestionComercial.Aplicacion.Eventos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class DescuentoConfigViewModel : PropertyChangedBase
    {
        private readonly IDescuentoConfiguracionServicio _servicio;
        private readonly IProductoServicio _productoServicio;
        private readonly IUnitOfWork _uow;
        private readonly SesionServicio _sesion;
        private readonly IEventAggregator _eventAggregator;

        public DescuentoConfigViewModel(
            IDescuentoConfiguracionServicio servicio,
            IProductoServicio productoServicio,
            IUnitOfWork uow,
            SesionServicio sesion,
            IEventAggregator eventAggregator)
        {
            _servicio = servicio;
            _productoServicio = productoServicio;
            _uow = uow;
            _sesion = sesion;
            _eventAggregator = eventAggregator;
        }

        private ObservableCollection<DescuentoListadoDto> _items = new();
        public ObservableCollection<DescuentoListadoDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                _textoBusqueda = value;
                NotifyOfPropertyChange(() => TextoBusqueda);
                _ = CargarAsync();
            }
        }

        // ── Filtros ──────────────────────────────────────────────────────────
        private List<DescuentoListadoDto> _todosLosItems = new();

        public List<string> TiposDescuento { get; } = new() { "Todos", "Producto", "Categoría", "Método de Pago", "Compra Mayor" };

        private List<string> _metodosPago = new();
        public List<string> MetodosPago
        {
            get => _metodosPago;
            set { _metodosPago = value; NotifyOfPropertyChange(() => MetodosPago); }
        }

        private string _filtroTipo = "Todos";
        public string FiltroTipo
        {
            get => _filtroTipo;
            set
            {
                _filtroTipo = value;
                NotifyOfPropertyChange(() => FiltroTipo);
                AplicarFiltros();
            }
        }

        private string _filtroMetodoPago = string.Empty;
        public string FiltroMetodoPago
        {
            get => _filtroMetodoPago;
            set
            {
                _filtroMetodoPago = value;
                NotifyOfPropertyChange(() => FiltroMetodoPago);
                AplicarFiltros();
            }
        }

        public void LimpiarFiltros()
        {
            FiltroTipo = "Todos";
            FiltroMetodoPago = string.Empty;
            TextoBusqueda = string.Empty;
        }

        public async Task CargarMetodosPagoAsync()
        {
            if (_sesion.IdEmpresa <= 0) return;

            var metodos = await _uow.MetodosPago.ObtenerTodosPorEmpresaAsync(_sesion.IdEmpresa);
            MetodosPago = metodos
                .Where(m => m.Activo)
                .Select(m => m.Nombre)
                .OrderBy(n => n)
                .ToList();
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; NotifyOfPropertyChange(() => IsLoading); }
        }

        private bool _mostrarFormulario;
        public bool MostrarFormulario
        {
            get => _mostrarFormulario;
            set { _mostrarFormulario = value; NotifyOfPropertyChange(() => MostrarFormulario); }
        }

        private DescuentoFormularioInnerViewModel? _formulario;
        public DescuentoFormularioInnerViewModel? Formulario
        {
            get => _formulario;
            set { _formulario = value; NotifyOfPropertyChange(() => Formulario); }
        }

        public async Task CargarAsync()
        {
            if (_sesion.IdEmpresa <= 0) return;

            IsLoading = true;
            try
            {
                var lista = await _servicio.ObtenerTodosAsync(
                    _sesion.IdEmpresa, null, TextoBusqueda);

                _todosLosItems = lista.Select(d => new DescuentoListadoDto
                {
                    Id = d.Id,
                    Nombre = d.Nombre,
                    Valor = d.Valor,
                    Id_producto = d.Id_producto,
                    Id_categoria = d.Id_categoria,
                    ProductoNombre = d.Producto?.Nombre,
                    CategoriaNombre = d.Categoria?.Nombre,
                    AplicaCualquierMetodoPago = d.AplicaCualquierMetodoPago,
                    MetodosPagoNombres = string.Join(", ",
                        d.DescuentosMetodosPago
                            .Where(dm => dm.MetodoPago != null)
                            .Select(dm => dm.MetodoPago!.Nombre)),
                    MetodosPagoNombresLista = d.DescuentosMetodosPago
                        .Where(dm => dm.MetodoPago != null)
                        .Select(dm => dm.MetodoPago!.Nombre)
                        .ToList(),
                    FechaDesde = d.FechaDesde,
                    FechaHasta = d.FechaHasta,
                    Activo = d.Activo,
                    Alcance = d.Alcance.ToString(),
                    MontoMinimoCompra = d.MontoMinimoCompra
                }).ToList();

                await CargarMetodosPagoAsync();
                AplicarFiltros();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AplicarFiltros()
        {
            var filtered = _todosLosItems.AsEnumerable();

            if (FiltroTipo != "Todos")
            {
                filtered = filtered.Where(d => d.Tipo == FiltroTipo);
            }

            if (!string.IsNullOrWhiteSpace(FiltroMetodoPago))
            {
                // Coincidencia EXACTA por nombre de método (no substring): evita que
                // un método cuyo nombre es subcadena de otro filtre por error.
                // Los descuentos que aplican a cualquier método no se asocian a uno
                // específico, por lo que quedan excluidos de este filtro.
                filtered = filtered.Where(d =>
                    !d.AplicaCualquierMetodoPago &&
                    d.MetodosPagoNombresLista.Contains(
                        FiltroMetodoPago, StringComparer.OrdinalIgnoreCase));
            }

            Items = new ObservableCollection<DescuentoListadoDto>(filtered);
        }

        public void NuevoDescuento()
        {
            var vm = IoC.Get<DescuentoFormularioInnerViewModel>();
            vm.ParentRef = this;
            vm.EsModoEdicion = false;
            vm.DescuentoId = 0;
            Formulario = vm;
            MostrarFormulario = true;
            _ = vm.CargarAsync();
        }

        public void EditarDescuento(DescuentoListadoDto? item)
        {
            if (item == null) return;
            var vm = IoC.Get<DescuentoFormularioInnerViewModel>();
            vm.ParentRef = this;
            vm.EsModoEdicion = true;
            vm.DescuentoId = item.Id;
            Formulario = vm;
            MostrarFormulario = true;
            _ = vm.CargarAsync();
        }

        public async Task ToggleActivoAsync(DescuentoListadoDto item)
        {
            try
            {
                var descuento = await _servicio.ObtenerPorIdAsync(item.Id);
                if (descuento == null) return;

                if (descuento.Activo)
                    await _servicio.EliminarAsync(item.Id);
                else
                    await _servicio.ActivarAsync(item.Id);

                await _eventAggregator.PublishOnUIThreadAsync(new DescuentosActualizadosEvent());
                await CargarAsync();
            }
            catch (Exception ex)
            {
                // Log or surface the error — don't swallow silently
                System.Diagnostics.Debug.WriteLine($"Error toggling discount: {ex.Message}");
                throw;
            }
        }

    }
}
