using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorCostoViewModel : Screen
    {
        private readonly IProveedorCostoServicio _servicio;
        private readonly IUnitOfWork _uow;
        private readonly IConfirmService _confirmService;

        public ProveedorCostoViewModel(
            IProveedorCostoServicio servicio,
            IUnitOfWork uow,
            IConfirmService confirmService)
        {
            _servicio = servicio;
            _uow = uow;
            _confirmService = confirmService;
            Proveedores = new ObservableCollection<Proveedor>();
            CostosPreview = new ObservableCollection<CostoProveedorPreview>();
            Porcentaje = 0;
        }

        public ObservableCollection<Proveedor> Proveedores { get; set; }
        public Proveedor? ProveedorSeleccionado { get; set; }
        public decimal Porcentaje { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool EstaTrabajando { get; set; }
        public bool MostrarPreview { get; set; }

        public ObservableCollection<CostoProveedorPreview> CostosPreview { get; set; }

        public async Task CargarProveedoresAsync()
        {
            EstaTrabajando = true;
            var items = await _uow.Proveedores.ObtenerTodosAsync();
            var activos = items.Where(p => p.Activo).ToList();
            Proveedores = new ObservableCollection<Proveedor>(activos);
            NotifyOfPropertyChange(() => Proveedores);
            EstaTrabajando = false;
        }

        public async Task GenerarPreviewAsync()
        {
            if (ProveedorSeleccionado == null)
            {
                Status = "Seleccione un proveedor";
                NotifyOfPropertyChange(() => Status);
                return;
            }

            if (Porcentaje < -99m || Porcentaje > 999m)
            {
                Status = "El porcentaje debe estar entre -99% y 999%";
                NotifyOfPropertyChange(() => Status);
                return;
            }

            EstaTrabajando = true;
            Status = "Generando vista previa...";
            NotifyOfPropertyChange(() => Status);

            var existentes = await _uow.ProveedoresCostos.ObtenerPorProveedorAsync(ProveedorSeleccionado.Id);
            var preview = new List<CostoProveedorPreview>();

            if (existentes != null && existentes.Any())
            {
                foreach (var c in existentes)
                {
                    var producto = await _uow.Productos.ObtenerPorIdAsync(c.IdProducto);
                    preview.Add(new CostoProveedorPreview
                    {
                        NombreProducto = producto?.Nombre ?? $"Producto {c.IdProducto}",
                        CostoAnterior = c.Costo,
                        CostoNuevo = Math.Round(c.Costo * (1 + Porcentaje / 100m), 2)
                    });
                }
            }
            else
            {
                var productos = await _uow.Productos.ObtenerPorEmpresaAsync(ProveedorSeleccionado.Id_empresa);
                foreach (var p in productos)
                {
                    preview.Add(new CostoProveedorPreview
                    {
                        NombreProducto = p.Nombre,
                        CostoAnterior = p.PrecioCostoActual,
                        CostoNuevo = Math.Round(p.PrecioCostoActual * (1 + Porcentaje / 100m), 2)
                    });
                }
            }

            CostosPreview = new ObservableCollection<CostoProveedorPreview>(preview);
            NotifyOfPropertyChange(() => CostosPreview);
            MostrarPreview = preview.Count > 0;
            NotifyOfPropertyChange(() => MostrarPreview);
            EstaTrabajando = false;
            Status = $"Vista previa: {preview.Count} productos afectados";
            NotifyOfPropertyChange(() => Status);
        }

        public async Task AplicarAsync()
        {
            if (ProveedorSeleccionado == null)
            {
                Status = "Seleccione un proveedor";
                NotifyOfPropertyChange(() => Status);
                return;
            }

            if (Porcentaje < -99m || Porcentaje > 999m)
            {
                Status = "El porcentaje debe estar entre -99% y 999%";
                NotifyOfPropertyChange(() => Status);
                return;
            }

            if (CostosPreview.Count == 0)
            {
                Status = "Genere la vista previa primero";
                NotifyOfPropertyChange(() => Status);
                return;
            }

            var confirmado = await _confirmService.ConfirmAsync(
                "Confirmar ajuste de costo",
                $"¿Está seguro de aplicar el ajuste de {Porcentaje}% para '{ProveedorSeleccionado.Nombre}'?\n\nSe modificarán {CostosPreview.Count} productos.");

            if (!confirmado)
            {
                Status = "Operación cancelada";
                NotifyOfPropertyChange(() => Status);
                return;
            }

            EstaTrabajando = true;
            Status = "Aplicando ajustes...";
            NotifyOfPropertyChange(() => Status);

            try
            {
                var (nuevos, actualizados) = await _servicio.AjusteCostoProveedorAsync(
                    ProveedorSeleccionado.Id, Porcentaje);

                Status = $"✓ Aplicado: {nuevos} nuevos, {actualizados} actualizados";
                NotifyOfPropertyChange(() => Status);

                await GenerarPreviewAsync();
            }
            catch (System.Exception ex)
            {
                Status = $"Error: {ex.Message}";
                NotifyOfPropertyChange(() => Status);
            }
            finally
            {
                EstaTrabajando = false;
            }
        }
    }

    public class CostoProveedorPreview
    {
        public string NombreProducto { get; set; } = string.Empty;
        public decimal CostoAnterior { get; set; }
        public decimal CostoNuevo { get; set; }
        public decimal Diferencia => CostoNuevo - CostoAnterior;
    }
}