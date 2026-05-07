using BCrypt.Net;
using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class UsuariosViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;

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
        private string _editEmail     = string.Empty;
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
        public string EditEmail
        {
            get => _editEmail;
            set { _editEmail = value; NotifyOfPropertyChange(() => EditEmail); }
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

        public UsuariosViewModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // Cargar usuarios con Sucursal y Rol incluidos
                var usuarios = await _uow.Usuarios.Consultar()
                    .Include(u => u.Sucursal)
                    .Include(u => u.Rol)
                    .ToListAsync();

                Items = new ObservableCollection<UsuarioDto>(
                    usuarios.Select(u => new UsuarioDto
                    {
                        IdUsuario      = u.Id,
                        Nombre         = u.Nombre,
                        Apellido       = u.Apellido,
                        Email          = u.Email,
                        Activo         = u.Activo,
                        IdSucursal     = u.Id_sucursal,
                        SucursalNombre = u.Sucursal?.Nombre ?? string.Empty,
                        Rol            = u.Rol?.Nombre ?? string.Empty,
                        IdRol          = u.Id_rol
                    })
                );

                // Cargar sucursales para el combo
                var sucursales = await _uow.Sucursales.ObtenerTodosAsync();
                Sucursales = new ObservableCollection<SucursalDto>(
                    sucursales.Select(s => new SucursalDto
                    {
                        IdSucursal = s.Id,
                        Nombre     = s.Nombre,
                        Direccion  = s.Direccion,
                        Activa     = s.Activo,
                        IdEmpresa  = s.Id_empresa
                    })
                );

                // Cargar roles para el combo
                var roles = await _uow.Roles.ObtenerTodosAsync();
                Roles = new ObservableCollection<RolDto>(
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

        public void NuevoUsuario()
        {
            _esNuevo     = true;
            TituloPanel  = "Nuevo Usuario";
            EditNombre   = string.Empty;
            EditApellido = string.Empty;
            EditEmail    = string.Empty;
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
            EditEmail    = item.Email;
            EditPassword = string.Empty;
            EditActivo   = item.Activo;
            EditSucursal = Sucursales.FirstOrDefault(s => s.IdSucursal == item.IdSucursal);
            EditRol      = Roles.FirstOrDefault(r => r.IdRol == item.IdRol);
            PanelVisible = true;
        }

        public void CerrarPanel() => PanelVisible = false;

        public async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(EditNombre)) { MostrarError("El nombre es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(EditApellido)) { MostrarError("El apellido es obligatorio."); return; }
            if (_esNuevo && string.IsNullOrWhiteSpace(EditPassword)) { MostrarError("La contraseña es obligatoria."); return; }
            if (EditSucursal == null) { MostrarError("Debe seleccionar una sucursal."); return; }
            if (EditRol == null) { MostrarError("Debe seleccionar un rol."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                if (_esNuevo)
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(EditPassword, workFactor: 12);
                    var usuario = GestionComercial.Dominio.Entidades.Seguridad.Usuario.Crear(
                        EditNombre, EditApellido, EditEmail, passwordHash,
                        EditSucursal.IdSucursal, EditRol.IdRol);

                    await _uow.Usuarios.AgregarAsync(usuario);
                    await _uow.GuardarCambiosAsync();

                    // Recargar con includes para obtener datos de navegación
                    var usuarioCompleto = await _uow.Usuarios.Consultar()
                        .Include(u => u.Sucursal)
                        .Include(u => u.Rol)
                        .FirstOrDefaultAsync(u => u.Id == usuario.Id);

                    Items.Add(new UsuarioDto
                    {
                        IdUsuario      = usuario.Id,
                        Nombre         = usuario.Nombre,
                        Apellido       = usuario.Apellido,
                        Email          = usuario.Email,
                        Activo         = usuario.Activo,
                        IdSucursal     = usuario.Id_sucursal,
                        SucursalNombre = usuarioCompleto?.Sucursal?.Nombre ?? string.Empty,
                        Rol            = usuarioCompleto?.Rol?.Nombre ?? string.Empty,
                        IdRol          = usuario.Id_rol
                    });
                }
                else if (Seleccionado != null)
                {
                    var usuario = await _uow.Usuarios.Consultar()
                        .Include(u => u.Sucursal)
                        .Include(u => u.Rol)
                        .FirstOrDefaultAsync(u => u.Id == Seleccionado.IdUsuario);

                    if (usuario != null)
                    {
                        usuario.Nombre     = EditNombre;
                        usuario.Apellido   = EditApellido;
                        usuario.Email      = EditEmail;
                        usuario.Id_sucursal = EditSucursal.IdSucursal;
                        usuario.Id_rol     = EditRol.IdRol;

                        if (!string.IsNullOrWhiteSpace(EditPassword))
                        {
                            var passwordHash = BCrypt.Net.BCrypt.HashPassword(EditPassword, workFactor: 12);
                            usuario.ActualizarPassword(passwordHash);
                        }

                        _uow.Usuarios.Actualizar(usuario);
                        await _uow.GuardarCambiosAsync();

                        Seleccionado.Nombre         = usuario.Nombre;
                        Seleccionado.Apellido       = usuario.Apellido;
                        Seleccionado.Email          = usuario.Email;
                        Seleccionado.Activo         = usuario.Activo;
                        Seleccionado.IdSucursal     = usuario.Id_sucursal;
                        Seleccionado.SucursalNombre = usuario.Sucursal?.Nombre ?? string.Empty;
                        Seleccionado.Rol            = usuario.Rol?.Nombre ?? string.Empty;
                        Seleccionado.IdRol          = usuario.Id_rol;

                        var idx = Items.IndexOf(Seleccionado);
                        if (idx >= 0)
                        {
                            Items.RemoveAt(idx);
                            Items.Insert(idx, Seleccionado);
                        }
                    }
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task ToggleActivo(UsuarioDto item)
        {
            IsLoading = true;
            try
            {
                var usuario = await _uow.Usuarios.ObtenerPorIdAsync(item.IdUsuario);
                if (usuario != null)
                {
                    usuario.Activo = !usuario.Activo;
                    _uow.Usuarios.Actualizar(usuario);
                    await _uow.GuardarCambiosAsync();

                    item.Activo = usuario.Activo;
                    var idx = Items.IndexOf(item);
                    if (idx >= 0)
                    {
                        Items.RemoveAt(idx);
                        Items.Insert(idx, item);
                    }
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
