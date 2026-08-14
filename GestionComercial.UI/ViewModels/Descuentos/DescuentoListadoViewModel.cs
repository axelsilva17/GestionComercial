using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Descuentos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Descuentos
{
    public class DescuentoListadoViewModel : NavigableViewModel
    {
        private readonly IDescuentoConfiguracionServicio _servicio;
        private readonly SesionServicio _sesion;

        public DescuentoListadoViewModel(
            IDescuentoConfiguracionServicio servicio,
            SesionServicio sesion)
        {
            _servicio = servicio;
            _sesion = sesion;
            Titulo = "Descuentos";
            Subtitulo = "Gestión de descuentos por producto, categoría y método de pago";
        }

        private ObservableCollection<DescuentoListadoDto> _items = new();
        public ObservableCollection<DescuentoListadoDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private TipoDescuentoEnum? _filtroTipo;
        public TipoDescuentoEnum? FiltroTipo
        {
            get => _filtroTipo;
            set { _filtroTipo = value; NotifyOfPropertyChange(() => FiltroTipo); _ = BuscarAsync(); }
        }

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); _ = BuscarAsync(); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await BuscarAsync();
        }

        public async Task BuscarAsync()
        {
            if (_sesion.IdEmpresa <= 0) return;

            IsLoading = true;
            try
            {
                var lista = await _servicio.ObtenerTodosAsync(
                    _sesion.IdEmpresa, FiltroTipo, null, TextoBusqueda);

                Items = new ObservableCollection<DescuentoListadoDto>(
                    lista.Select(d => new DescuentoListadoDto
                    {
                        Id = d.Id,
                        Nombre = d.Nombre,
                        Tipo = d.Tipo,
                        Valor = d.Valor,
                        Id_producto = d.Id_producto,
                        Id_categoria = d.Id_categoria,
                        Id_metodoPago = d.Id_metodoPago,
                        MetodoPagoNombre = d.MetodoPago?.Nombre,
                        FechaDesde = d.FechaDesde,
                        FechaHasta = d.FechaHasta,
                        Prioridad = d.Prioridad,
                        Activo = d.Activo
                    }));
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void NuevoDescuento()
        {
            var vm = IoC.Get<DescuentoFormularioViewModel>();
            vm.EsModoEdicion = false;
            vm.ListadoRef = this;
            IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public void EditarDescuento(DescuentoListadoDto? item)
        {
            if (item == null) return;
            var vm = IoC.Get<DescuentoFormularioViewModel>();
            vm.EsModoEdicion = true;
            vm.DescuentoId = item.Id;
            vm.ListadoRef = this;
            IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }
    }
}
