using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class SucursalesViewModel : NavigableViewModel
    {
        private ObservableCollection<SucursalDto> _items = new();
        public ObservableCollection<SucursalDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private SucursalDto _seleccionada;
        public SucursalDto Seleccionada
        {
            get => _seleccionada;
            set { _seleccionada = value; NotifyOfPropertyChange(() => Seleccionada); }
        }

        // ── Formulario ───────────────────────────────────────────────────────
        private string _editNombre    = string.Empty;
        private string _editDireccion = string.Empty;
        private bool   _editActiva    = true;
        private bool   _esNueva;

        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public string EditDireccion
        {
            get => _editDireccion;
            set { _editDireccion = value; NotifyOfPropertyChange(() => EditDireccion); }
        }
        public bool EditActiva
        {
            get => _editActiva;
            set { _editActiva = value; NotifyOfPropertyChange(() => EditActiva); }
        }

        private bool _panelVisible;
        public bool PanelVisible
        {
            get => _panelVisible;
            set { _panelVisible = value; NotifyOfPropertyChange(() => PanelVisible); }
        }

        private string _tituloPanelPanel = "Nueva Sucursal";
        public string TituloPanel
        {
            get => _tituloPanelPanel;
            set { _tituloPanelPanel = value; NotifyOfPropertyChange(() => TituloPanel); }
        }

        public async Task CargarAsync()
        {
            await Task.Delay(100);
           
        }

        public void NuevaSucursal()
        {
            _esNueva      = true;
            TituloPanel   = "Nueva Sucursal";
            EditNombre    = string.Empty;
            EditDireccion = string.Empty;
            EditActiva    = true;
            PanelVisible  = true;
        }

        public void Editar(SucursalDto item)
        {
            _esNueva      = false;
            TituloPanel   = "Editar Sucursal";
            Seleccionada  = item;
            EditNombre    = item.Nombre;
            EditDireccion = item.Direccion;
            EditActiva    = item.Activa;
            PanelVisible  = true;
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
                if (_esNueva)
                {
                    Items.Add(new SucursalDto
                    {
                        IdSucursal = Items.Count + 1,
                        Nombre     = EditNombre,
                        Direccion  = EditDireccion,
                        Activa     = EditActiva
                    });
                }
                else if (Seleccionada != null)
                {
                    Seleccionada.Nombre    = EditNombre;
                    Seleccionada.Direccion = EditDireccion;
                    Seleccionada.Activa    = EditActiva;
                    var idx = Items.IndexOf(Seleccionada);
                    Items.RemoveAt(idx);
                    Items.Insert(idx, Seleccionada);
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task ToggleActiva(SucursalDto item)
        {
            item.Activa = !item.Activa;
            var idx = Items.IndexOf(item);
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
            await Task.Delay(100); // TODO: await servicio
        }
    }
}
