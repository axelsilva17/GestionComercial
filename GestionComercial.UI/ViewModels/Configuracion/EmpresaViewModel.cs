using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.UI.ViewModels.Base;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class EmpresaViewModel : NavigableViewModel
    {
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

        public async Task CargarAsync()
        {
            await Task.Delay(100); // TODO: await _empresaServicio.ObtenerAsync()
      
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
                await Task.Delay(300); // TODO: await _empresaServicio.ActualizarAsync(...)
                Empresa = new EmpresaDto
                {
                    IdEmpresa = Empresa.IdEmpresa,
                    Nombre    = EditNombre,
                    CUIT      = EditCuit,
                    Direccion = EditDireccion,
                    Activa    = Empresa.Activa
                };
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
