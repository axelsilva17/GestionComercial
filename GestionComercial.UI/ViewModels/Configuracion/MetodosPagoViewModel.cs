using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class MetodosPagoViewModel : NavigableViewModel
    {
        private ObservableCollection<MetodoPagoDto> _items = new();
        public ObservableCollection<MetodoPagoDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private MetodoPagoDto _seleccionado;
        public MetodoPagoDto Seleccionado
        {
            get => _seleccionado;
            set { _seleccionado = value; NotifyOfPropertyChange(() => Seleccionado); }
        }

        private string _editNombre    = string.Empty;
        private bool   _editEfectivo  = false;
        private bool   _esNuevo;

        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public bool EditEfectivo
        {
            get => _editEfectivo;
            set { _editEfectivo = value; NotifyOfPropertyChange(() => EditEfectivo); }
        }

        private bool   _panelVisible;
        private string _tituloPanel = "Nuevo Método";
        public bool PanelVisible
        {
            get => _panelVisible;
            set { _panelVisible = value; NotifyOfPropertyChange(() => PanelVisible); }
        }
        public string TituloPanel
        {
            get => _tituloPanel;
            set { _tituloPanel = value; NotifyOfPropertyChange(() => TituloPanel); }
        }

        public async Task CargarAsync()
        {
            await Task.Delay(100);
            Items = new ObservableCollection<MetodoPagoDto>
            {
                new() { IdMetodoPago = 1, Nombre = "Efectivo",      EsEfectivo = true  },
                new() { IdMetodoPago = 2, Nombre = "Débito",        EsEfectivo = false },
                new() { IdMetodoPago = 3, Nombre = "Crédito",       EsEfectivo = false },
                new() { IdMetodoPago = 4, Nombre = "Transferencia", EsEfectivo = false },
            };
        }

        public void NuevoMetodo()
        {
            _esNuevo      = true;
            TituloPanel   = "Nuevo Método de Pago";
            EditNombre    = string.Empty;
            EditEfectivo  = false;
            PanelVisible  = true;
        }

        public void Editar(MetodoPagoDto item)
        {
            _esNuevo     = false;
            TituloPanel  = "Editar Método de Pago";
            Seleccionado = item;
            EditNombre   = item.Nombre;
            EditEfectivo = item.EsEfectivo;
            PanelVisible = true;
        }

        public void CerrarPanel() => PanelVisible = false;

        public async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(EditNombre)) { MostrarError("El nombre es obligatorio."); return; }
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(300);
                if (_esNuevo)
                {
                    Items.Add(new MetodoPagoDto
                    {
                        IdMetodoPago = Items.Count + 1,
                        Nombre       = EditNombre,
                        EsEfectivo   = EditEfectivo
                    });
                }
                else if (Seleccionado != null)
                {
                    Seleccionado.Nombre     = EditNombre;
                    Seleccionado.EsEfectivo = EditEfectivo;
                    var idx = Items.IndexOf(Seleccionado);
                    Items.RemoveAt(idx);
                    Items.Insert(idx, Seleccionado);
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Eliminar(MetodoPagoDto item)
        {
            IsLoading = true;
            try
            {
                await Task.Delay(200);
                Items.Remove(item);
            }
            finally { IsLoading = false; }
        }
    }
}
