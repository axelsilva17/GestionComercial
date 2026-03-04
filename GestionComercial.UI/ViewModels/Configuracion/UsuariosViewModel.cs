using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class UsuariosViewModel : NavigableViewModel
    {
        private ObservableCollection<UsuarioDto> _items = new();
        public ObservableCollection<UsuarioDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private ObservableCollection<SucursalDto> _sucursales = new();
        public ObservableCollection<SucursalDto> Sucursales
        {
            get => _sucursales;
            set { _sucursales = value; NotifyOfPropertyChange(() => Sucursales); }
        }

        private ObservableCollection<RolDto> _roles = new();
        public ObservableCollection<RolDto> Roles
        {
            get => _roles;
            set { _roles = value; NotifyOfPropertyChange(() => Roles); }
        }

        private UsuarioDto _seleccionado;
        public UsuarioDto Seleccionado
        {
            get => _seleccionado;
            set { _seleccionado = value; NotifyOfPropertyChange(() => Seleccionado); }
        }

        // ── Formulario ───────────────────────────────────────────────────────
        private string _editNombre    = string.Empty;
        private string _editApellido  = string.Empty;
        private string _editPassword  = string.Empty;
        private bool   _editActivo    = true;
        private bool   _esNuevo;
        private SucursalDto _editSucursal;
        private RolDto      _editRol;

        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public string EditApellido
        {
            get => _editApellido;
            set { _editApellido = value; NotifyOfPropertyChange(() => EditApellido); }
        }
        public string EditPassword
        {
            get => _editPassword;
            set { _editPassword = value; NotifyOfPropertyChange(() => EditPassword); }
        }
        public bool EditActivo
        {
            get => _editActivo;
            set { _editActivo = value; NotifyOfPropertyChange(() => EditActivo); }
        }
        public SucursalDto EditSucursal
        {
            get => _editSucursal;
            set { _editSucursal = value; NotifyOfPropertyChange(() => EditSucursal); }
        }
        public RolDto EditRol
        {
            get => _editRol;
            set { _editRol = value; NotifyOfPropertyChange(() => EditRol); }
        }

        private bool   _panelVisible;
        private string _tituloPanel = "Nuevo Usuario";
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
            Sucursales = new ObservableCollection<SucursalDto>
            {
                new() { IdSucursal = 1, Nombre = "Casa Central"   },
                new() { IdSucursal = 2, Nombre = "Sucursal Norte" },
                new() { IdSucursal = 3, Nombre = "Sucursal Sur"   },
            };
            Roles = new ObservableCollection<RolDto>
            {
                new() { IdRol = 1, Nombre = "Administrador" },
                new() { IdRol = 2, Nombre = "Vendedor"      },
                new() { IdRol = 3, Nombre = "Cajero"        },
            };
            Items = new ObservableCollection<UsuarioDto>
            {
                new() { IdUsuario = 1, Nombre = "Juan",   Apellido = "García",    Activo = true,  IdSucursal = 1, SucursalNombre = "Casa Central",   Rol = "Administrador" },
                new() { IdUsuario = 2, Nombre = "María",  Apellido = "López",     Activo = true,  IdSucursal = 2, SucursalNombre = "Sucursal Norte",  Rol = "Vendedor"      },
                new() { IdUsuario = 3, Nombre = "Carlos", Apellido = "Martínez",  Activo = true,  IdSucursal = 1, SucursalNombre = "Casa Central",    Rol = "Cajero"        },
                new() { IdUsuario = 4, Nombre = "Ana",    Apellido = "Rodríguez", Activo = false, IdSucursal = 3, SucursalNombre = "Sucursal Sur",    Rol = "Vendedor"      },
            };
        }

        public void NuevoUsuario()
        {
            _esNuevo     = true;
            TituloPanel  = "Nuevo Usuario";
            EditNombre   = string.Empty;
            EditApellido = string.Empty;
            EditPassword = string.Empty;
            EditActivo   = true;
            EditSucursal = null;
            EditRol      = null;
            PanelVisible = true;
        }

        public void Editar(UsuarioDto item)
        {
            _esNuevo     = false;
            TituloPanel  = "Editar Usuario";
            Seleccionado = item;
            EditNombre   = item.Nombre;
            EditApellido = item.Apellido;
            EditPassword = string.Empty;
            EditActivo   = item.Activo;
            PanelVisible = true;
        }

        public void CerrarPanel() => PanelVisible = false;

        public async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(EditNombre)) { MostrarError("El nombre es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(EditApellido)) { MostrarError("El apellido es obligatorio."); return; }
            if (_esNuevo && string.IsNullOrWhiteSpace(EditPassword)) { MostrarError("La contraseña es obligatoria."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(300);
                if (_esNuevo)
                {
                    Items.Add(new UsuarioDto
                    {
                        IdUsuario      = Items.Count + 1,
                        Nombre         = EditNombre,
                        Apellido       = EditApellido,
                        Activo         = EditActivo,
                        IdSucursal     = EditSucursal?.IdSucursal ?? 0,
                        SucursalNombre = EditSucursal?.Nombre ?? string.Empty,
                        Rol      = EditRol?.Nombre ?? string.Empty
                    });
                }
                else if (Seleccionado != null)
                {
                    Seleccionado.Nombre   = EditNombre;
                    Seleccionado.Apellido = EditApellido;
                    Seleccionado.Activo   = EditActivo;
                    var idx = Items.IndexOf(Seleccionado);
                    Items.RemoveAt(idx);
                    Items.Insert(idx, Seleccionado);
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task ToggleActivo(UsuarioDto item)
        {
            item.Activo = !item.Activo;
            var idx = Items.IndexOf(item);
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
            await Task.Delay(100);
        }
    }
}
