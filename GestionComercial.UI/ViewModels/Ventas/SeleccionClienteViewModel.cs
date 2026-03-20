using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
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
        private readonly IClienteServicio _clienteServicio;
        private readonly SesionServicio _sesion;

        public VentaViewModel VentaOrigen { get; set; }

        public SeleccionClienteViewModel(IClienteServicio clienteServicio, SesionServicio sesion)
        {
            _clienteServicio = clienteServicio;
            _sesion          = sesion;
            Titulo           = "Seleccionar Cliente";
        }

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
                var todos = await _clienteServicio.ObtenerTodosAsync(_sesion.IdEmpresa);

                if (todos == null || !todos.Any())
                {
                    Clientes.Clear();
                    return;
                }

                var filtrados = string.IsNullOrWhiteSpace(TextoBusqueda)
                    ? todos
                    : todos.Where(c => 
                        (c.Nombre?.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        c.Documento.ToString().Contains(TextoBusqueda));

                Clientes.Clear();
                foreach (var c in filtrados) Clientes.Add(c);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally { IsLoading = false; }
        }

        public async Task Confirmar()
        {
            if (ClienteSeleccionado == null || VentaOrigen == null) return;
            VentaOrigen.ClienteId     = ClienteSeleccionado.IdCliente;
            VentaOrigen.ClienteNombre = ClienteSeleccionado.Nombre;
            await IoC.Get<ShellViewModel>().ActivateItemAsync(VentaOrigen, CancellationToken.None);
        }

        public async Task SeleccionarConsumidorFinal()
        {
            if (VentaOrigen == null) return;
            VentaOrigen.ClienteId     = 1;
            VentaOrigen.ClienteNombre = "Consumidor Final";
            await IoC.Get<ShellViewModel>().ActivateItemAsync(VentaOrigen, CancellationToken.None);
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
