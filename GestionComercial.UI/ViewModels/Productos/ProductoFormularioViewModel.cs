using Caliburn.Micro;
using FluentValidation;
using FluentValidation.Results;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.Helpers;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.Generic;
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
        private readonly ILogger<ProductoFormularioViewModel>? _logger;
        private readonly DemoFeatureService? _demoFeatures;

        public ProductoFormularioViewModel(IProductoServicio productoServicio, ShellViewModel shell, ILogger<ProductoFormularioViewModel>? logger = null, DemoFeatureService? demoFeatures = null)
        {
            _productoServicio = productoServicio;
            _shell = shell;
            _logger = logger;
            _demoFeatures = demoFeatures;
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

        /// El botón "Importar desde Excel" solo se muestra en modo Crear y si no es demo.
        public bool MostrarBotonImportar => !IsEditMode && (_demoFeatures == null || _demoFeatures.PuedeEjecutarAccion("productos", "importar"));

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

        /// Configura el formulario en modo Crear (campos vacíos).
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

        /// Configura el formulario en modo Editar cargando el producto indicado.
        public async void InicializarParaEditar(int idProducto)
        {
            IsEditMode = true;
            _idProducto   = idProducto;
            LimpiarError();
            await CargarReferenciasAsync();
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
                _logger?.LogError(ex, "Error cargando referencias");
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
                _logger?.LogError(ex, "Error cargando producto");
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

        public void GenerarCodigoBarra()
        {
            var rng = new Random();
            var digits = new char[13];
            // Prefijo fijo 779 (Argentina) + 10 dígitos aleatorios
            digits[0] = '7';
            digits[1] = '7';
            digits[2] = '9';
            for (int i = 3; i < 12; i++)
                digits[i] = (char)('0' + rng.Next(0, 10));
            // Dígito verificador simple
            int suma = 0;
            for (int i = 0; i < 12; i++)
                suma += (i % 2 == 0) ? (digits[i] - '0') : (digits[i] - '0') * 3;
            int digitoVerificador = (10 - (suma % 10)) % 10;
            digits[12] = (char)('0' + digitoVerificador);
            CodigoBarra = new string(digits);
        }

        public async Task<bool> Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MostrarError("El nombre del producto es obligatorio.");
                return false;
            }
            if (PrecioVentaActual <= 0)
            {
                MostrarError("El precio de venta debe ser mayor a 0.");
                return false;
            }
            if (PrecioCostoActual < 0)
            {
                MostrarError("El precio de costo no puede ser negativo.");
                return false;
            }
            if (StockActual < 0)
            {
                MostrarError("El stock no puede ser negativo.");
                return false;
            }

            try
            {
                IsLoading = true;
                LimpiarError();
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
                        StockActual = StockActual,
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
            catch (ValidationException ex)
            {
                // Validation failures are business errors: show them inline in the
                // form error surface (human-readable bullets) instead of exposing
                // the raw "Validation failed: * CodigoBarra: ..." exception text.
                _logger?.LogError(ex, "Error de validación guardando producto");
                MostrarError(FormatearErroresValidacion(ex));
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error guardando producto");
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// Formats a FluentValidation ValidationException into human-readable
        /// bullet lines ("No se pudo guardar el producto:" + "• Campo: mensaje")
        /// instead of exposing the raw exception text.
        private static string FormatearErroresValidacion(ValidationException ex)
        {
            var errores = ex.Errors?.ToList() ?? new List<ValidationFailure>();
            if (errores.Count == 0)
                return "No se pudo guardar el producto: " + ex.Message;

            var lineas = errores.Select(e => $"• {NombreCampoLegible(e.PropertyName)}: {e.ErrorMessage}");
            return "No se pudo guardar el producto:\n" + string.Join("\n", lineas);
        }

        private static string NombreCampoLegible(string nombrePropiedad)
            => NombresCampos.TryGetValue(nombrePropiedad, out var legible) ? legible : nombrePropiedad;

        private static readonly Dictionary<string, string> NombresCampos = new()
        {
            ["Nombre"]           = "Nombre",
            ["CodigoBarra"]      = "Código de barra",
            ["PrecioVentaActual"]= "Precio de venta",
            ["PrecioCostoActual"]= "Precio de costo",
            ["StockActual"]      = "Stock actual",
            ["StockMinimo"]      = "Stock mínimo",
            ["IdCategoria"]      = "Categoría",
            ["IdUnidadMedida"]   = "Unidad de medida",
            ["IdEmpresa"]        = "Empresa",
            ["IdProducto"]       = "Producto",
        };

        public async Task Volver()
        {
            var listado = IoC.Get<ProductoListadoViewModel>();
            await _shell.ActivateItemAsync(listado, CancellationToken.None);
        }

        /// Navega a la vista de importación masiva desde Excel.
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
