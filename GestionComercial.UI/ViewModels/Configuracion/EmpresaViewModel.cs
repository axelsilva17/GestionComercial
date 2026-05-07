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
        private string _editNombre    = string.Empty;
        private string _editCuit      = string.Empty;
        private string _editDireccion = string.Empty;

        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public string EditCuit
        {
            get => _editCuit;
            set { _editCuit = value; NotifyOfPropertyChange(() => EditCuit); }
        }
        public string EditDireccion
        {
            get => _editDireccion;
            set { _editDireccion = value; NotifyOfPropertyChange(() => EditDireccion); }
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
            EditCuit      = Empresa.CUIT;
            EditDireccion = Empresa.Direccion;
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
                    empresa.Nombre    = EditNombre;
                    empresa.CUIT      = EditCuit;
                    empresa.Direccion = EditDireccion;
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
                    Activa    = empresa.Activo
                };
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
