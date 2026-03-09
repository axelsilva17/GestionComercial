using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class RolesViewModel : NavigableViewModel
    {
        private ObservableCollection<RolDto> _items = new();
        public ObservableCollection<RolDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private RolDto _seleccionado;
        public RolDto Seleccionado
        {
            get => _seleccionado;
            set { _seleccionado = value; NotifyOfPropertyChange(() => Seleccionado); }
        }

        private string _editNombre = string.Empty;
        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }

        private bool   _panelVisible;
        private string _tituloPanel = "Nuevo Rol";
        private bool   _esNuevo;

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
            Items = new ObservableCollection<RolDto>
            {
                new() { IdRol = 1, Nombre = "Gerente" },
                new() { IdRol = 2, Nombre = "Administrador"      },
                new() { IdRol = 3, Nombre = "Vendedor"        },
            };
        }

        public void NuevoRol()
        {
            _esNuevo     = true;
            TituloPanel  = "Nuevo Rol";
            EditNombre   = string.Empty;
            PanelVisible = true;
        }

        public void Editar(RolDto item)
        {
            _esNuevo     = false;
            TituloPanel  = "Editar Rol";
            Seleccionado = item;
            EditNombre   = item.Nombre;
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
                    Items.Add(new RolDto { IdRol = Items.Count + 1, Nombre = EditNombre });
                }
                else if (Seleccionado != null)
                {
                    Seleccionado.Nombre = EditNombre;
                    var idx = Items.IndexOf(Seleccionado);
                    Items.RemoveAt(idx);
                    Items.Insert(idx, Seleccionado);
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Eliminar(RolDto item)
        {
            IsLoading = true;
            try
            {
                await Task.Delay(200); // TODO: await servicio
                Items.Remove(item);
            }
            finally { IsLoading = false; }
        }
    }
}
