using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class SucursalesViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;

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

        public SucursalesViewModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var sucursales = await _uow.Sucursales.ObtenerTodosAsync();
                Items = new ObservableCollection<SucursalDto>(
                    sucursales.Select(s => new SucursalDto
                    {
                        IdSucursal = s.Id,
                        Nombre     = s.Nombre,
                        Direccion  = s.Direccion,
                        Activa     = s.Activo,
                        IdEmpresa  = s.Id_empresa
                    })
                );
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
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
                if (_esNueva)
                {
                    var empresa = await _uow.Empresas.PrimerODefaultAsync(e => e.Activo);
                    if (empresa == null)
                    {
                        MostrarError("No hay una empresa activa. Cree la empresa primero.");
                        return;
                    }

                    var sucursal = Sucursal.Crear(EditNombre, EditDireccion, empresa.Id);
                    await _uow.Sucursales.AgregarAsync(sucursal);
                    await _uow.GuardarCambiosAsync();

                    Items.Add(new SucursalDto
                    {
                        IdSucursal = sucursal.Id,
                        Nombre     = sucursal.Nombre,
                        Direccion  = sucursal.Direccion,
                        Activa     = sucursal.Activo,
                        IdEmpresa  = sucursal.Id_empresa
                    });
                }
                else if (Seleccionada != null)
                {
                    var sucursal = await _uow.Sucursales.ObtenerPorIdAsync(Seleccionada.IdSucursal);
                    if (sucursal != null)
                    {
                        sucursal.Nombre    = EditNombre;
                        sucursal.Direccion = EditDireccion;
                        sucursal.Activo    = EditActiva;
                        _uow.Sucursales.Actualizar(sucursal);
                        await _uow.GuardarCambiosAsync();

                        Seleccionada.Nombre    = sucursal.Nombre;
                        Seleccionada.Direccion = sucursal.Direccion;
                        Seleccionada.Activa    = sucursal.Activo;
                        var idx = Items.IndexOf(Seleccionada);
                        Items.RemoveAt(idx);
                        Items.Insert(idx, Seleccionada);
                    }
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task ToggleActiva(SucursalDto item)
        {
            IsLoading = true;
            try
            {
                var sucursal = await _uow.Sucursales.ObtenerPorIdAsync(item.IdSucursal);
                if (sucursal != null)
                {
                    sucursal.Activo = !sucursal.Activo;
                    _uow.Sucursales.Actualizar(sucursal);
                    await _uow.GuardarCambiosAsync();

                    item.Activa = sucursal.Activo;
                    var idx = Items.IndexOf(item);
                    Items.RemoveAt(idx);
                    Items.Insert(idx, item);
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
