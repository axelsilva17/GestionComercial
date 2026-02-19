using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class SeleccionClienteViewModel : NavigableViewModel
    {
        public VentaViewModel VentaOrigen { get; set; }

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private ObservableCollection<ClienteDto> _clientes = new();
        public ObservableCollection<ClienteDto> Clientes
        {
            get => _clientes;
            set { _clientes = value; NotifyOfPropertyChange(() => Clientes); }
        }

        private ClienteDto _clienteSeleccionado;
        public ClienteDto ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set { _clienteSeleccionado = value; NotifyOfPropertyChange(() => ClienteSeleccionado); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await Buscar();

        public async Task Buscar()
        {
            IsLoading = true;
            try
            {
                // TODO: await _clienteServicio.BuscarAsync(TextoBusqueda, empresaId)
                await Task.Delay(200);
                Clientes = new ObservableCollection<ClienteDto>();
            }
            finally { IsLoading = false; }
        }

        public async Task Confirmar()
        {
            if (ClienteSeleccionado == null || VentaOrigen == null) return;
            VentaOrigen.ClienteId     = ClienteSeleccionado.Id;
            VentaOrigen.ClienteNombre = ClienteSeleccionado.Nombre;
            await IoC.Get<ShellViewModel>().ActivateItemAsync(VentaOrigen, CancellationToken.None);
        }

        public async Task SeleccionarConsumidorFinal()
        {
            if (VentaOrigen == null) return;
            VentaOrigen.ClienteId     = 0;
            VentaOrigen.ClienteNombre = "Consumidor Final";
            await IoC.Get<ShellViewModel>().ActivateItemAsync(VentaOrigen, CancellationToken.None);
        }

        public async Task NuevoCliente()
        {
            // TODO: navegar a ClienteFormularioViewModel
            await Task.CompletedTask;
        }

        public async Task Volver()
        {
            if (VentaOrigen != null)
                await IoC.Get<ShellViewModel>().ActivateItemAsync(VentaOrigen, CancellationToken.None);
            else
                await IoC.Get<ShellViewModel>().ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
        }
    }
}
