using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
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
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
