using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
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
        private readonly IRolServicio _rolServicio;

        // Mapeo: nombre de módulo → código de permiso
        private static readonly (string Modulo, string Codigo)[] ModulosDefinidos =
        {
            ("Ventas",        "Ventas.Ver"),
            ("Caja",          "Caja.Abrir"),
            ("Compras",       "Compras.Ver"),
            ("Productos",     "Productos.Ver"),
            ("Clientes",      "Clientes.Ver"),
            ("Reportes",      "Reportes.Ver"),
            ("Configuración", "Configuracion.Ver"),
            ("Usuarios",      "Usuarios.Gestionar"),
        };

        // ── Lista de roles ────────────────────────────────────────────────────
        private ObservableCollection<RolListDto> _items = new();
        public ObservableCollection<RolListDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private RolListDto _seleccionado;
        public RolListDto Seleccionado
        {
            get => _seleccionado;
            set
            {
                _seleccionado = value;
                NotifyOfPropertyChange(() => Seleccionado);
                NotifyOfPropertyChange(() => MostrarPermisos);
                if (value != null)
                    _ = CargarPermisosAsync(value.Id);
            }
        }

        // ── Toggles de módulos ───────────────────────────────────────────────
        private ObservableCollection<ModuloToggleItem> _modulos = new();
        public ObservableCollection<ModuloToggleItem> Modulos
        {
            get => _modulos;
            set { _modulos = value; NotifyOfPropertyChange(() => Modulos); }
        }

        public bool MostrarPermisos => _seleccionado != null;
        public bool PuedeGuardarPermisos => _seleccionado != null && !IsLoading;

        // Calcula cantidad de módulos activos
        public string ResumenPermisos
        {
            get
            {
                if (_seleccionado == null) return "";
                int activos = _modulos.Count(m => m.Activo);
                return $"{activos} de {_modulos.Count} módulos activos";
            }
        }

        public async Task GuardarPermisos()
        {
            if (_seleccionado == null) return;

            IsLoading = true;
            LimpiarError();
            NotifyOfPropertyChange(() => PuedeGuardarPermisos);
            try
            {
                // Obtener IDs de permisos correspondientes a módulos activos
                var todosPermisos = await _rolServicio.ObtenerPermisosDisponiblesAsync();
                var codigosActivos = _modulos
                    .Where(m => m.Activo)
                    .Select(m => m.CodigoPermiso)
                    .ToHashSet();

                var ids = todosPermisos
                    .Where(p => codigosActivos.Contains(p.Codigo))
                    .Select(p => p.Id)
                    .ToList();

                await _rolServicio.AsignarPermisosARolAsync(_seleccionado.Id, ids);

                // Recargar para actualizar cantidad de permisos
                await CargarAsync();

                System.Windows.MessageBox.Show(
                    "Permisos guardados correctamente.",
                    "Éxito",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => PuedeGuardarPermisos);
            }
        }

        // ── Panel de edición de nombre (existente) ──────────────────────────
        private string _editNombre = string.Empty;
        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }

        private bool _panelVisible;
        private string _tituloPanel = "Nuevo Rol";
        private bool _esNuevo;

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

        public RolesViewModel(IUnitOfWork uow, IRolServicio rolServicio)
        {
            _uow = uow;
            _rolServicio = rolServicio;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                Items = new ObservableCollection<RolListDto>(
                    await _rolServicio.ObtenerRolesAsync());

                if (Items.Count > 0)
                    Seleccionado = Items[0];
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        private async Task CargarPermisosAsync(int rolId)
        {
            IsLoading = true;
            NotifyOfPropertyChange(() => PuedeGuardarPermisos);
            try
            {
                var todosPermisos = await _rolServicio.ObtenerPermisosDisponiblesAsync();
                var idsAsignados = await _rolServicio.ObtenerPermisosPorRolAsync(rolId);
                var asignadosSet = new System.Collections.Generic.HashSet<int>(idsAsignados);

                // Mapa: código de permiso → ID
                var permisoMap = todosPermisos.ToDictionary(p => p.Codigo, p => p.Id);

                var modulos = ModulosDefinidos.Select(m =>
                {
                    permisoMap.TryGetValue(m.Codigo, out var permisoId);
                    return new ModuloToggleItem
                    {
                        Nombre = m.Modulo,
                        CodigoPermiso = m.Codigo,
                        Activo = asignadosSet.Contains(permisoId),
                    };
                }).ToList();

                Modulos = new ObservableCollection<ModuloToggleItem>(modulos);
                NotifyOfPropertyChange(() => ResumenPermisos);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => PuedeGuardarPermisos);
            }
        }

        // ── CRUD de nombres de rol (existente) ───────────────────────────────
        public void NuevoRol()
        {
            _esNuevo = true;
            TituloPanel = "Nuevo Rol";
            EditNombre = string.Empty;
            PanelVisible = true;
        }

        public void Editar(RolListDto item)
        {
            _esNuevo = false;
            TituloPanel = "Editar Rol";
            Seleccionado = item;
            EditNombre = item.Nombre;
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
                    Items.Add(new RolListDto { Id = rol.Id, Nombre = rol.Nombre });
                }
                else if (Seleccionado != null)
                {
                    var rol = await _uow.Roles.ObtenerPorIdAsync(Seleccionado.Id);
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

        public async Task Eliminar(RolListDto item)
        {
            IsLoading = true;
            try
            {
                var rol = await _uow.Roles.ObtenerPorIdAsync(item.Id);
                if (rol != null)
                {
                    _uow.Roles.Eliminar(rol);
                    await _uow.GuardarCambiosAsync();
                    Items.Remove(item);
                    if (Seleccionado?.Id == item.Id)
                        Seleccionado = null;
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }

    // ── Modelo para toggle de módulo ─────────────────────────────────────────
    public class ModuloToggleItem : PropertyChangedBase
    {
        private bool _activo;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoPermiso { get; set; } = string.Empty;
        public bool Activo
        {
            get => _activo;
            set { _activo = value; NotifyOfPropertyChange(() => Activo); }
        }
    }
}
