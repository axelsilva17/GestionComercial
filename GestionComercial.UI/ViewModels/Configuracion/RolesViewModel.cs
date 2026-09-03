using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class RolesViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;
        private readonly IRolServicio _rolServicio;
        private readonly SesionServicio _sesion;

        // Mapeo: nombre de módulo → TODOS los códigos de permiso que implica
        private static readonly (string Modulo, string[] Codigos)[] ModulosDefinidos =
        {
            ("Ventas",        new[] { "Ventas.Ver", "Ventas.Crear", "Ventas.Anular" }),
            ("Caja",          new[] { "Caja.Abrir", "Caja.Cerrar", "Caja.Auditoria" }),
            ("Compras",       new[] { "Compras.Ver", "Compras.Crear" }),
            ("Productos",     new[] { "Productos.Ver", "Productos.Crear", "Productos.Editar" }),
            ("Clientes",      new[] { "Clientes.Ver", "Clientes.Crear" }),
            ("Reportes",      new[] { "Reportes.Ver" }),
            ("Descuentos",    new[] { "Descuentos.Ver" }),
            ("Configuración", new[] { "Configuracion.Ver" }),
            ("Usuarios",      new[] { "Usuarios.Gestionar" }),
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
                var permisoMap = todosPermisos.ToDictionary(p => p.Codigo, p => p.Id);

                // Un módulo activo implica TODOS sus permisos
                var ids = new System.Collections.Generic.List<int>();
                foreach (var m in _modulos.Where(m => m.Activo))
                {
                    var modulo = ModulosDefinidos.FirstOrDefault(md => md.Modulo == m.Nombre);
                    if (modulo.Codigos == null) continue;
                    foreach (var cod in modulo.Codigos)
                    {
                        if (permisoMap.TryGetValue(cod, out var pid) && !ids.Contains(pid))
                            ids.Add(pid);
                    }
                }

                await _rolServicio.AsignarPermisosARolAsync(_seleccionado.Id, ids);

                // DEBUG: mostrar qué permisos se guardaron
                var nombresGuardados = ids.Select(id => todosPermisos.FirstOrDefault(p => p.Id == id)?.Codigo ?? $"?{id}");
                System.Diagnostics.Debug.WriteLine($"[RolesVM] Guardando rol '{_seleccionado.Nombre}' (Id={_seleccionado.Id}) con {ids.Count} permisos: {string.Join(", ", nombresGuardados)}");

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

        public async Task RemoverPermisoAsync(string nombreModulo)
        {
            if (_seleccionado == null) return;

            IsLoading = true;
            LimpiarError();
            try
            {
                var modulo = ModulosDefinidos.FirstOrDefault(md => md.Modulo == nombreModulo);
                if (modulo.Codigos == null) return;

                var todosPermisos = await _rolServicio.ObtenerPermisosDisponiblesAsync();
                var permisoMap = todosPermisos.ToDictionary(p => p.Codigo, p => p.Id);

                var permisosActuales = await _rolServicio.ObtenerPermisosPorRolAsync(_seleccionado.Id);
                var nuevosPermisos = permisosActuales
                    .Where(pid => !modulo.Codigos.Any(cod => permisoMap.TryGetValue(cod, out var pId) && pId == pid))
                    .ToList();

                await _rolServicio.AsignarPermisosARolAsync(_seleccionado.Id, nuevosPermisos);

                _seleccionado.CantidadPermisos = nuevosPermisos.Count;
                await CargarPermisosAsync(_seleccionado.Id);
                NotifyOfPropertyChange(() => ResumenPermisos);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
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

        public RolesViewModel(IUnitOfWork uow, IRolServicio rolServicio, SesionServicio sesion)
        {
            _uow = uow;
            _rolServicio = rolServicio;
            _sesion = sesion;
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
                    // Un módulo está activo si tiene AL MENOS UNO de sus permisos asignados
                    bool activo = m.Codigos.Any(codigo =>
                        permisoMap.TryGetValue(codigo, out var pid) && asignadosSet.Contains(pid));
                    return new ModuloToggleItem
                    {
                        Nombre = m.Modulo,
                        CodigoPermiso = m.Codigos.First(),
                        Activo = activo,
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
                if (item.Nombre == "Gerente" || item.Nombre == "Vendedor")
                {
                    MostrarError("No se pueden eliminar los roles base (Gerente, Vendedor).");
                    return;
                }

                var confirm = System.Windows.MessageBox.Show(
                    $"¿Eliminar el rol \"{item.Nombre}\"?\n\nLos permisos se transferirán al rol Gerente.\nLos usuarios con este rol quedarán sin asignación.",
                    "Confirmar eliminación",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (confirm != System.Windows.MessageBoxResult.Yes) return;

                var rol = await _uow.Roles.ObtenerPorIdAsync(item.Id);
                if (rol == null) return;

                // Transferir permisos al Gerente
                var gerente = await _uow.Roles.ObtenerPorIdAsync(1);
                if (gerente != null)
                {
                    var permisosARol = await _rolServicio.ObtenerPermisosPorRolAsync(item.Id);
                    var permisosGerente = await _rolServicio.ObtenerPermisosPorRolAsync(1);
                    var permisosGerenteSet = new System.Collections.Generic.HashSet<int>(permisosGerente);

                    var permisosAgregados = permisosARol
                        .Where(pid => !permisosGerenteSet.Contains(pid))
                        .ToList();

                    if (permisosAgregados.Count > 0)
                    {
                        var nuevosPermisosGerente = new System.Collections.Generic.List<int>(permisosGerente);
                        nuevosPermisosGerente.AddRange(permisosAgregados);
                        await _rolServicio.AsignarPermisosARolAsync(1, nuevosPermisosGerente);
                    }
                }

                // Reasignar usuarios del rol eliminado al Gerente
                var todosUsuarios = await _uow.Usuarios.Consultar()
                    .Where(u => u.Id_rol == item.Id)
                    .ToListAsync();

                foreach (var usuario in todosUsuarios)
                {
                    usuario.Id_rol = 1;
                    _uow.Usuarios.Actualizar(usuario);
                }

                _uow.Roles.Eliminar(rol);
                await _uow.GuardarCambiosAsync();
                Items.Remove(item);
                if (Seleccionado?.Id == item.Id)
                    Seleccionado = null;
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
