using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class SeleccionClienteViewModel : NavigableViewModel
    {
        // VM de origen para volver con el cliente elegido
        public VentaViewModel VentaOrigen { get; set; }

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private ObservableCollection<ClienteItemDto> _clientes = new();
        public ObservableCollection<ClienteItemDto> Clientes
        {
            get => _clientes;
            set { _clientes = value; NotifyOfPropertyChange(() => Clientes); }
        }

        // Bindeado al DataGrid x:Name="ClienteSeleccionado"
        private ClienteItemDto _clienteSeleccionado;
        public ClienteItemDto ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set { _clienteSeleccionado = value; NotifyOfPropertyChange(() => ClienteSeleccionado); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await Buscar();
        }

        public async Task Buscar()
        {
            IsLoading = true;
            try
            {
                // TODO: await _clienteServicio.BuscarAsync(TextoBusqueda, empresaId)
                await Task.Delay(200);
                Clientes = new ObservableCollection<ClienteItemDto>();
            }
            finally { IsLoading = false; }
        }

        public async Task Confirmar()
        {
            if (ClienteSeleccionado == null || VentaOrigen == null) return;

            VentaOrigen.ClienteId     = ClienteSeleccionado.Id;
            VentaOrigen.ClienteNombre = ClienteSeleccionado.Nombre;

            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(VentaOrigen, CancellationToken.None);
        }

        public async Task SeleccionarConsumidorFinal()
        {
            if (VentaOrigen == null) return;
            VentaOrigen.ClienteId     = 0;
            VentaOrigen.ClienteNombre = "Consumidor Final";

            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(VentaOrigen, CancellationToken.None);
        }

        public async Task NuevoCliente()
        {
            // TODO: navegar a ClienteFormularioViewModel en modo creación
        }

        public async Task Volver()
        {
            if (VentaOrigen != null)
                await IoC.Get<ShellViewModel>()
                         .ActivateItemAsync(VentaOrigen, CancellationToken.None);
            else
                await IoC.Get<ShellViewModel>()
                         .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
        }
    }

    public class ClienteItemDto
    {
        public int    Id       { get; set; }
        public string Nombre   { get; set; }
        public string Inicial  => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
        public string Documento { get; set; }
        public string Telefono  { get; set; }
        public string Email     { get; set; }
    }
}
