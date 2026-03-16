using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ProductoFormularioViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        public ProductoFormularioViewModel(ShellViewModel shell)
        {
            _shell = shell;
        }

        // ── Modo ──────────────────────────────────────────────────────────────
        private bool _esModoEdicion;
        public bool EsModoEdicion
        {
            get => _esModoEdicion;
            set
            {
                _esModoEdicion = value;
                NotifyOfPropertyChange(() => EsModoEdicion);
                NotifyOfPropertyChange(() => TituloFormulario);
                NotifyOfPropertyChange(() => SubtituloFormulario);
                // El botón Importar solo aparece al crear (no tiene sentido en edición)
                NotifyOfPropertyChange(() => MostrarBotonImportar);
            }
        }

        /// <summary>El botón "Importar desde Excel" solo se muestra en modo Crear.</summary>
        public bool MostrarBotonImportar => !EsModoEdicion;

        public string TituloFormulario    => EsModoEdicion ? "Editar Producto"  : "Nuevo Producto";
        public string SubtituloFormulario => EsModoEdicion
            ? "Modificá los datos del producto"
            : "Completá los datos para crear un nuevo producto";

        // ── ID (edición) ──────────────────────────────────────────────────────
        private int _idProducto;

        // ── Campos ───────────────────────────────────────────────────────────
        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; NotifyOfPropertyChange(() => Nombre); NotifyOfPropertyChange(() => CanGuardar); }
        }

        private string _sku = string.Empty;
        public string SKU
        {
            get => _sku;
            set { _sku = value; NotifyOfPropertyChange(() => SKU); }
        }

        private string _codigoBarra = string.Empty;
        public string CodigoBarra
        {
            get => _codigoBarra;
            set { _codigoBarra = value; NotifyOfPropertyChange(() => CodigoBarra); }
        }

        private decimal _precioVentaActual;
        public decimal PrecioVentaActual
        {
            get => _precioVentaActual;
            set { _precioVentaActual = value; NotifyOfPropertyChange(() => PrecioVentaActual); NotifyOfPropertyChange(() => MargenCalculado); }
        }

        private decimal _precioCostoActual;
        public decimal PrecioCostoActual
        {
            get => _precioCostoActual;
            set { _precioCostoActual = value; NotifyOfPropertyChange(() => PrecioCostoActual); NotifyOfPropertyChange(() => MargenCalculado); }
        }

        private int _stockActual;
        public int StockActual
        {
            get => _stockActual;
            set { _stockActual = value; NotifyOfPropertyChange(() => StockActual); }
        }

        private int _stockMinimo;
        public int StockMinimo
        {
            get => _stockMinimo;
            set { _stockMinimo = value; NotifyOfPropertyChange(() => StockMinimo); }
        }

        private bool _activo = true;
        public bool Activo
        {
            get => _activo;
            set { _activo = value; NotifyOfPropertyChange(() => Activo); }
        }

        private string _descripcion = string.Empty;
        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; NotifyOfPropertyChange(() => Descripcion); }
        }

        // ── Listas de referencia ──────────────────────────────────────────────
        private ObservableCollection<CategoriaItemDto> _categorias = new();
        public ObservableCollection<CategoriaItemDto> Categorias
        {
            get => _categorias;
            set { _categorias = value; NotifyOfPropertyChange(() => Categorias); }
        }

        private ObservableCollection<UnidadMedidaItemDto> _unidadesMedida = new();
        public ObservableCollection<UnidadMedidaItemDto> UnidadesMedida
        {
            get => _unidadesMedida;
            set { _unidadesMedida = value; NotifyOfPropertyChange(() => UnidadesMedida); }
        }

        // ── Computed ──────────────────────────────────────────────────────────
        public string MargenCalculado
        {
            get
            {
                if (PrecioCostoActual <= 0) return "—";
                var margen = ((PrecioVentaActual - PrecioCostoActual) / PrecioCostoActual) * 100;
                return $"{margen:N1}%";
            }
        }

        public bool CanGuardar => !string.IsNullOrWhiteSpace(Nombre) && !IsLoading;

        // ── Inicialización ────────────────────────────────────────────────────

        /// <summary>Configura el formulario en modo Crear (campos vacíos).</summary>
        public void InicializarParaCrear()
        {
            EsModoEdicion    = false;
            _idProducto      = 0;
            Nombre           = string.Empty;
            SKU              = string.Empty;
            CodigoBarra      = string.Empty;
            PrecioVentaActual = 0;
            PrecioCostoActual = 0;
            StockActual      = 0;
            StockMinimo      = 0;
            Activo           = true;
            Descripcion      = string.Empty;
            LimpiarError();
            _ = CargarReferenciasAsync();
        }

        /// <summary>Configura el formulario en modo Editar cargando el producto indicado.</summary>
        public void InicializarParaEditar(int idProducto)
        {
            EsModoEdicion = true;
            _idProducto   = idProducto;
            LimpiarError();
            _ = CargarProductoAsync(idProducto);
        }

        // ── Carga de datos ────────────────────────────────────────────────────
        private async Task CargarReferenciasAsync()
        {
            IsLoading = true;
            try
            {
                await Task.Delay(100); // TODO: Categorias = await _categoriaServicio.ListarAsync();
                Categorias    = new ObservableCollection<CategoriaItemDto>();
                UnidadesMedida = new ObservableCollection<UnidadMedidaItemDto>();
            }
            finally { IsLoading = false; }
        }

        private async Task CargarProductoAsync(int idProducto)
        {
            IsLoading = true;
            try
            {
                await CargarReferenciasAsync();
                await Task.Delay(150); // TODO: var dto = await _productoServicio.ObtenerAsync(idProducto);
                // Nombre = dto.Nombre; etc.
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public void GenerarSKU()
        {
            // TODO: SKU = await _skuServicio.GenerarAsync();
            SKU = $"SKU-{System.DateTime.Now:yyMMddHHmm}";
        }

        public async void Guardar()
        {
            if (!CanGuardar) return;
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(500); // TODO: llamar al servicio
                await Volver();
            }
            catch (System.Exception ex) { MostrarError($"Error al guardar: {ex.Message}"); }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            var listado = IoC.Get<ProductoListadoViewModel>();
            await _shell.ActivateItemAsync(listado, CancellationToken.None);
        }

        /// <summary>Navega a la vista de importación masiva desde Excel.</summary>
        public async Task IrAImportacion()
        {
            var importacionVm = IoC.Get<ImportacionProductosViewModel>();

            // Al terminar la importación → volver al listado
            importacionVm.ImportacionCompletada += async () =>
            {
                var listado = IoC.Get<ProductoListadoViewModel>();
                await _shell.ActivateItemAsync(listado, CancellationToken.None);
            };

            // Al cancelar → volver al formulario
            importacionVm.Cancelado += async () =>
            {
                await _shell.ActivateItemAsync(this, CancellationToken.None);
            };

            await _shell.ActivateItemAsync(importacionVm, CancellationToken.None);
        }
    }
}
