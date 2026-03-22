using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.Extensions.Logging;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ProductoFormularioViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<ProductoFormularioViewModel> _logger;

        public ProductoFormularioViewModel(IProductoServicio productoServicio, ShellViewModel shell, ILogger<ProductoFormularioViewModel> logger)
        {
            _productoServicio = productoServicio;
            _shell = shell;
            _logger = logger;
        }

        // ── Modo ──────────────────────────────────────────────────────────────
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                NotifyOfPropertyChange(() => IsEditMode);
                NotifyOfPropertyChange(() => TituloFormulario);
                NotifyOfPropertyChange(() => SubtituloFormulario);
                // El botón Importar solo aparece al crear (no tiene sentido en edición)
                NotifyOfPropertyChange(() => MostrarBotonImportar);
            }
        }

        /// <summary>El botón "Importar desde Excel" solo se muestra en modo Crear.</summary>
        public bool MostrarBotonImportar => !IsEditMode;

        public string TituloFormulario    => IsEditMode ? "Editar Producto"  : "Nuevo Producto";
        public string SubtituloFormulario => IsEditMode
            ? "Modificá los datos del producto"
            : "Completá los datos para crear un nuevo producto";

        // ── ID (edición) ──────────────────────────────────────────────────────
        private int _idProducto;
        public int IdProducto
        {
            get => _idProducto;
            set { _idProducto = value; NotifyOfPropertyChange(() => IdProducto); }
        }

        // ── Selección de referencias ─────────────────────────────────────
        private CategoriaItemDto _categoriaSeleccionada;
        public CategoriaItemDto CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; NotifyOfPropertyChange(() => CategoriaSeleccionada); }
        }

        private UnidadMedidaItemDto _unidadMedidaSeleccionada;
        public UnidadMedidaItemDto UnidadMedidaSeleccionada
        {
            get => _unidadMedidaSeleccionada;
            set { _unidadMedidaSeleccionada = value; NotifyOfPropertyChange(() => UnidadMedidaSeleccionada); }
        }

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
            IsEditMode = false;
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
            IsEditMode = true;
            _idProducto   = idProducto;
            LimpiarError();
            _ = CargarProductoAsync(idProducto);
        }

        // ── Carga de datos ────────────────────────────────────────────────────
        public async Task CargarReferenciasAsync()
        {
            try
            {
                var categorias = await _productoServicio.ObtenerCategoriasAsync(_shell.IdEmpresaActual);
                Categorias = new ObservableCollection<CategoriaItemDto>(categorias);

                var unidades = await _productoServicio.ObtenerUnidadesMedidaAsync();
                UnidadesMedida = new ObservableCollection<UnidadMedidaItemDto>(unidades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando referencias");
            }
        }

        public async Task CargarProductoAsync(int idProducto)
        {
            try
            {
                IsLoading = true;
                var dto = await _productoServicio.ObtenerPorIdAsync(idProducto);
                if (dto != null)
                {
                    IdProducto = dto.IdProducto;
                    Nombre = dto.Nombre;
                    CodigoBarra = dto.CodigoBarra;
                    // Descripcion no existe en ProductoDto
                    PrecioCostoActual = dto.PrecioCostoActual;
                    PrecioVentaActual = dto.PrecioVentaActual;
                    StockMinimo = dto.StockMinimo;
                    CategoriaSeleccionada = Categorias?.FirstOrDefault(c => c.IdCategoria == dto.IdCategoria);
                    UnidadMedidaSeleccionada = UnidadesMedida?.FirstOrDefault(u => u.IdUnidadMedida == dto.IdUnidadMedida);
                    IsEditMode = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando producto");
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public void GenerarSKU()
        {
            // TODO: SKU = await _skuServicio.GenerarAsync();
            SKU = $"SKU-{System.DateTime.Now:yyMMddHHmm}";
        }

        public async Task<bool> Guardar()
        {
            try
            {
                IsLoading = true;
                if (IsEditMode)
                {
                    var dto = new ProductoActualizarDto
                    {
                        IdProducto = IdProducto,
                        Nombre = Nombre,
                        CodigoBarra = CodigoBarra,
                        PrecioCostoActual = PrecioCostoActual,
                        PrecioVentaActual = PrecioVentaActual,
                        StockMinimo = StockMinimo,
                        IdCategoria = CategoriaSeleccionada?.IdCategoria ?? 0,
                        IdUnidadMedida = UnidadMedidaSeleccionada?.IdUnidadMedida ?? 0,
                        Activo = Activo
                    };
                    await _productoServicio.ActualizarAsync(dto);
                }
                else
                {
                    var dto = new ProductoCrearDto
                    {
                        Nombre = Nombre,
                        CodigoBarra = CodigoBarra,
                        PrecioCostoActual = PrecioCostoActual,
                        PrecioVentaActual = PrecioVentaActual,
                        StockMinimo = StockMinimo,
                        IdCategoria = CategoriaSeleccionada?.IdCategoria ?? 0,
                        IdUnidadMedida = UnidadMedidaSeleccionada?.IdUnidadMedida ?? 0,
                        IdEmpresa = _shell.IdEmpresaActual
                    };
                    await _productoServicio.CrearAsync(dto);
                }
                MessageBox.Show("Producto guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando producto");
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
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
