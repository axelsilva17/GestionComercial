using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class RolesViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;

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

        public RolesViewModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var roles = await _uow.Roles.ObtenerTodasAsync();
                Items = new ObservableCollection<RolDto>(
                    roles.Select(r => new RolDto
                    {
                        IdRol  = r.Id,
                        Nombre = r.Nombre
                    })
                );
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
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
                if (_esNuevo)
                {
                    var rol = new Rol { Nombre = EditNombre };
                    await _uow.Roles.AgregarAsync(rol);
                    await _uow.GuardarCambiosAsync();

                    Items.Add(new RolDto { IdRol = rol.Id, Nombre = rol.Nombre });
                }
                else if (Seleccionado != null)
                {
                    var rol = await _uow.Roles.ObtenerPorIdAsync(Seleccionado.IdRol);
                    if (rol != null)
                    {
                        rol.Nombre = EditNombre;
                        _uow.Roles.Actualizar(rol);
                        await _uow.GuardarCambiosAsync();

                        Seleccionado.Nombre = rol.Nombre;
                        var idx = Items.IndexOf(Seleccionado);
                        Items.RemoveAt(idx);
                        Items.Insert(idx, Seleccionado);
                    }
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
                var rol = await _uow.Roles.ObtenerPorIdAsync(item.IdRol);
                if (rol != null)
                {
                    _uow.Roles.Eliminar(rol);
                    await _uow.GuardarCambiosAsync();
                    Items.Remove(item);
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
