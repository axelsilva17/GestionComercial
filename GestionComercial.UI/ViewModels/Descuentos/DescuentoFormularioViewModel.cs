using Caliburn.Micro;
using GestionComercial.Aplicacion.Eventos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Descuentos
{
    public class DescuentoFormularioViewModel : NavigableViewModel
    {
        private readonly IDescuentoConfiguracionServicio _servicio;
        private readonly SesionServicio _sesion;
        private readonly IEventAggregator _eventAggregator;

        public DescuentoFormularioViewModel(
            IDescuentoConfiguracionServicio servicio,
            SesionServicio sesion,
            IEventAggregator eventAggregator)
        {
            _servicio = servicio;
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
            set { _tipoSeleccionado = value; NotifyOfPropertyChange(() => TipoSeleccionado); }
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
                    FechaDesde = descuento.FechaDesde;
                    FechaHasta = descuento.FechaHasta;
                    Prioridad = descuento.Prioridad;
                }
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
                        DescuentoId, Nombre, Valor, FechaDesde, FechaHasta, Prioridad);
                }
                else
                {
                    await _servicio.CrearAsync(
                        _sesion.IdEmpresa, Nombre, TipoSeleccionado, Valor,
                        IdProducto, IdCategoria, FechaDesde, FechaHasta, Prioridad);
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
