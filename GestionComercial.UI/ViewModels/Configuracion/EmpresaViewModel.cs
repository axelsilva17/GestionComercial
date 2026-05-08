using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using System.IO;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class EmpresaViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;

        // ── Datos empresa ────────────────────────────────────────────────────
        private EmpresaDto _empresa = new();
        public EmpresaDto Empresa
        {
            get => _empresa;
            set { _empresa = value; NotifyOfPropertyChange(() => Empresa); }
        }

        // ── Formulario edición ───────────────────────────────────────────────
        private string _editNombre     = string.Empty;
        private string _editDireccion  = string.Empty;
        private string _editEmail      = string.Empty;
        private string _editTelefono   = string.Empty;

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
        public string EditEmail
        {
            get => _editEmail;
            set { _editEmail = value; NotifyOfPropertyChange(() => EditEmail); }
        }
        public string EditTelefono
        {
            get => _editTelefono;
            set { _editTelefono = value; NotifyOfPropertyChange(() => EditTelefono); }
        }

        // ── Logo ─────────────────────────────────────────────────────────────
        private string? _logoPath;
        /// <summary>Ruta temporal del logo seleccionado (antes de guardar).</summary>
        public string? LogoPath
        {
            get => _logoPath;
            set
            {
                _logoPath = value;
                NotifyOfPropertyChange(() => LogoPath);
                NotifyOfPropertyChange(() => LogoVisible);
            }
        }
        public bool LogoVisible => !string.IsNullOrEmpty(LogoPath) || !string.IsNullOrEmpty(Empresa.LogoUrl);

        // ── Panel slide ──────────────────────────────────────────────────────
        private bool _panelVisible;
        public bool PanelVisible
        {
            get => _panelVisible;
            set { _panelVisible = value; NotifyOfPropertyChange(() => PanelVisible); }
        }

        public EmpresaViewModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var empresa = await _uow.Empresas.PrimerODefaultAsync(e => e.Activo);
                if (empresa != null)
                {
                    Empresa = new EmpresaDto
                    {
                        IdEmpresa = empresa.Id,
                        Nombre    = empresa.Nombre,
                        CUIT      = empresa.CUIT,
                        Direccion = empresa.Direccion,
                        Email     = empresa.Email,
                        Telefono  = empresa.Telefono,
                        LogoUrl   = empresa.LogoUrl,
                        Activa    = empresa.Activo
                    };
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public void AbrirEdicion()
        {
            EditNombre    = Empresa.Nombre;
            EditDireccion = Empresa.Direccion;
            EditEmail     = Empresa.Email ?? string.Empty;
            EditTelefono  = Empresa.Telefono ?? string.Empty;
            LogoPath      = null;
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
                var empresa = await _uow.Empresas.PrimerODefaultAsync(e => e.Id == Empresa.IdEmpresa);
                if (empresa != null)
                {
                    // Actualizar campos editables (CUIT NO se modifica)
                    empresa.Nombre    = EditNombre;
                    empresa.Direccion = EditDireccion;
                    empresa.Email     = string.IsNullOrWhiteSpace(EditEmail) ? null : EditEmail.Trim().ToLower();
                    empresa.Telefono  = string.IsNullOrWhiteSpace(EditTelefono) ? null : EditTelefono.Trim();

                    // Copiar logo si se seleccionó uno nuevo
                    if (!string.IsNullOrEmpty(LogoPath) && File.Exists(LogoPath))
                    {
                        var dir = System.IO.Path.Combine(
                            System.AppDomain.CurrentDomain.BaseDirectory, "Assets", "Logos");
                        Directory.CreateDirectory(dir);
                        var ext  = System.IO.Path.GetExtension(LogoPath);
                        var dest = System.IO.Path.Combine(dir, $"empresa_logo{ext}");
                        File.Copy(LogoPath, dest, overwrite: true);
                        empresa.LogoUrl = dest;
                    }

                    _uow.Empresas.Actualizar(empresa);
                }
                else
                {
                    MostrarError("No se encontró la empresa en la base de datos.");
                    return;
                }

                await _uow.GuardarCambiosAsync();

                Empresa = new EmpresaDto
                {
                    IdEmpresa = empresa.Id,
                    Nombre    = empresa.Nombre,
                    CUIT      = empresa.CUIT,
                    Direccion = empresa.Direccion,
                    Email     = empresa.Email,
                    Telefono  = empresa.Telefono,
                    LogoUrl   = empresa.LogoUrl,
                    Activa    = empresa.Activo
                };
                LogoPath = null;
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
