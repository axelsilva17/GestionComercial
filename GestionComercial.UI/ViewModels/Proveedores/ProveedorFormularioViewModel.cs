using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorFormularioViewModel : FormularioEntidadViewModel
    {
        private readonly IProveedorServicio _proveedorServicio;
        private readonly SesionServicio _sesion;

        public ProveedorFormularioViewModel(ShellViewModel shell, IProveedorServicio proveedorServicio, SesionServicio sesion)
            : base(shell)
        {
            _proveedorServicio = proveedorServicio;
            _sesion = sesion;
        }

        // ── Títulos ───────────────────────────────────────────────────────────
        public override string TituloFormulario    => EsModoEdicion ? "Editar Proveedor"           : "Nuevo Proveedor";
        public override string SubtituloFormulario => EsModoEdicion ? "Modificá los datos del proveedor" : "Completá los datos para registrar un nuevo proveedor";

        // ── Validación ────────────────────────────────────────────────────────
        public override bool CanGuardar => !string.IsNullOrWhiteSpace(Nombre) && EmailValido && !IsLoading;

        // ── Campos propios ────────────────────────────────────────────────────
        private int _idProveedor;

        // ── Inicialización ────────────────────────────────────────────────────
        public void InicializarParaCrear()
        {
            EsModoEdicion = false;
            _idProveedor  = 0;
            LimpiarCamposComunes();
        }

        public void InicializarParaEditar(int idProveedor)
        {
            EsModoEdicion = true;
            _idProveedor  = idProveedor;
            LimpiarError();
            _ = CargarProveedorAsync(idProveedor);
        }

        private async Task CargarProveedorAsync(int idProveedor)
        {
            IsLoading = true;
            try
            {
                var dto = await _proveedorServicio.ObtenerPorIdAsync(idProveedor);
                if (dto != null)
                {
                    Nombre   = dto.Nombre;
                    Telefono = dto.Telefono ?? string.Empty;
                    Email    = dto.Email ?? string.Empty;
                    Activo   = dto.Activo;
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Guardar ───────────────────────────────────────────────────────────
        public async void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MostrarError("El nombre del proveedor es obligatorio.");
                return;
            }
            if (!EmailValido)
            {
                MostrarError("Ingresá un email válido.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                if (EsModoEdicion)
                {
                    var proveedor = await _proveedorServicio.ObtenerPorIdAsync(_idProveedor);
                    if (proveedor != null)
                    {
                        proveedor.Nombre   = Nombre;
                        proveedor.Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono;
                        proveedor.Email    = string.IsNullOrWhiteSpace(Email) ? null : Email;
                        proveedor.Activo   = Activo;
                        await _proveedorServicio.ActualizarAsync(proveedor);
                    }
                }
                else
                {
                    var nuevoProveedor = new Proveedor
                    {
                        Nombre     = Nombre,
                        Telefono   = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono,
                        Email      = string.IsNullOrWhiteSpace(Email) ? null : Email,
                        Id_empresa = _sesion.IdEmpresa,
                        Activo     = true
                    };
                    await _proveedorServicio.CrearAsync(nuevoProveedor);
                }
                await Volver();
            }
            catch (System.Exception ex) { MostrarError($"Error al guardar: {ex.Message}"); }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            var listado = IoC.Get<ProveedorListadoViewModel>();
            await Shell.ActivateItemAsync(listado, CancellationToken.None);
        }
    }
}
