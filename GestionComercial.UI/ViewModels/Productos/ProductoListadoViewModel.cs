using Caliburn.Micro;
using GestionComercial.Dominio.Entidades.Proveedores;
using ClosedXML.Excel;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ProductoListadoViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly IInventarioServicio _inventarioServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<ProductoListadoViewModel>? _logger;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public ProductoListadoViewModel(
            IProductoServicio productoServicio,
            IInventarioServicio inventarioServicio,
            ShellViewModel shell,
            ILogger<ProductoListadoViewModel>? logger = null)
        {
            _productoServicio = productoServicio;
            _inventarioServicio = inventarioServicio;
            _shell = shell;
            _logger = logger;
            Titulo    = "Productos";
            Subtitulo = "Catálogo de productos";
        }


        // ── Métricas ──────────────────────────────────────────────────
        private int _totalProductos;
        public int TotalProductos
        {
            get => _totalProductos;
            set { _totalProductos = value; NotifyOfPropertyChange(() => TotalProductos); }
        }

        private string _status = string.Empty;
        public string Status
        {
            get => _status;
            set { _status = value; NotifyOfPropertyChange(() => Status); }
        }

        private int _productosActivos;
        public int ProductosActivos
        {
            get => _productosActivos;
            set { _productosActivos = value; NotifyOfPropertyChange(() => ProductosActivos); }
        }

        private int _productosStockBajo;
        public int ProductosStockBajo
        {
            get => _productosStockBajo;
            set { _productosStockBajo = value; NotifyOfPropertyChange(() => ProductosStockBajo); }
        }

        private int _productosSinStock;
        public int ProductosSinStock
        {
            get => _productosSinStock;
            set { _productosSinStock = value; NotifyOfPropertyChange(() => ProductosSinStock); }
        }

        // ── Listas ────────────────────────────────────────────────────
        private ObservableCollection<ProductoListadoDto> _productos = new();
        public ObservableCollection<ProductoListadoDto> Productos
        {
            get => _productos;
            set { _productos = value; NotifyOfPropertyChange(() => Productos); }
        }

        private ObservableCollection<CategoriaItemDto> _categorias = new();
        public ObservableCollection<CategoriaItemDto> Categorias
        {
            get => _categorias;
            set { _categorias = value; NotifyOfPropertyChange(() => Categorias); }
        }

        // ── Selección (sidebar) ───────────────────────────────────────
        private ProductoListadoDto _productoSeleccionado;
        public ProductoListadoDto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set { _productoSeleccionado = value; NotifyOfPropertyChange(() => ProductoSeleccionado); }
        }

        // ── Filtros ───────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private CategoriaItemDto _categoriaSeleccionada;
        public CategoriaItemDto CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                NotifyOfPropertyChange(() => CategoriaSeleccionada);
                _ = BuscarConCatch(); // Filtrar automaticamente al cambiar categoria
            }
        }

        private int _filtroActivo;
        public int FiltroActivo
        {
            get => _filtroActivo;
            set
            {
                _filtroActivo = value;
                NotifyOfPropertyChange(() => FiltroActivo);
                _ = BuscarConCatch();
            }
        }

        // ── Paginación ────────────────────────────────────────────────
        private int _productosMostrados;
        public int ProductosMostrados
        {
            get => _productosMostrados;
            set { _productosMostrados = value; NotifyOfPropertyChange(() => ProductosMostrados); }
        }

        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; NotifyOfPropertyChange(() => PaginaActual); }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; NotifyOfPropertyChange(() => TotalPaginas); }
        }

        // ── Ajuste Masivo de Precios ───────────────────────────────────────────
        private bool _mostrarPopupAjuste;

        // ── Ajuste Masivo - Filtro por categoría ──
        private CategoriaItemDto _categoriaAjuste;
        public CategoriaItemDto CategoriaAjuste
        {
            get => _categoriaAjuste;
            set
            {
                _categoriaAjuste = value;
                NotifyOfPropertyChange(() => CategoriaAjuste);
                if (MostrarPopupAjuste)
                    GenerarPreviewAjuste();
            }
        }
        public bool MostrarPopupAjuste
        {
            get => _mostrarPopupAjuste;
            set { _mostrarPopupAjuste = value; NotifyOfPropertyChange(() => MostrarPopupAjuste); }
        }

        private string _tipoAjuste = "porcentaje"; // "porcentaje" | "ganancia" | "fijo"
        public string TipoAjuste
        {
            get => _tipoAjuste;
            set { _tipoAjuste = value; NotifyOfPropertyChange(() => TipoAjuste); }
        }

        private decimal _porcentajeAjuste = 0;
        public decimal PorcentajeAjuste
        {
            get => _porcentajeAjuste;
            set { _porcentajeAjuste = value; NotifyOfPropertyChange(() => PorcentajeAjuste); }
        }

        private decimal _montoFijo = 0;
        public decimal MontoFijo
        {
            get => _montoFijo;
            set { _montoFijo = value; NotifyOfPropertyChange(() => MontoFijo); }
        }

        private ObservableCollection<ProductoListadoDto> _productosPreview = new();
        public ObservableCollection<ProductoListadoDto> ProductosPreview
        {
            get => _productosPreview;
            set { _productosPreview = value; NotifyOfPropertyChange(() => ProductosPreview); }
        }

        private bool _aplicarAPrecioVenta = true;
        public bool AplicarAPrecioVenta
        {
            get => _aplicarAPrecioVenta;
            set { _aplicarAPrecioVenta = value; NotifyOfPropertyChange(() => AplicarAPrecioVenta); }
        }

        private bool _aplicarAPrecioCosto = true;
        public bool AplicarAPrecioCosto
        {
            get => _aplicarAPrecioCosto;
            set { _aplicarAPrecioCosto = value; NotifyOfPropertyChange(() => AplicarAPrecioCosto); }
        }

        private int _productosActualizados;
        public int ProductosActualizados
        {
            get => _productosActualizados;
            set { _productosActualizados = value; NotifyOfPropertyChange(() => ProductosActualizados); }
        }

        // ── Flag para stock crítico (desde dashboard) ──────────────────
        private bool _mostrarSoloStockCritico;
        public bool MostrarSoloStockCritico
        {
            get => _mostrarSoloStockCritico;
            set
            {
                _mostrarSoloStockCritico = value;
                NotifyOfPropertyChange(() => MostrarSoloStockCritico);
            }
        }

        // ── Lifecycle ─────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await CargarCategoriasAsync();
            await CargarAsync();
            await CargarProveedoresAjusteAsync();
        }

        private async Task CargarCategoriasAsync()
        {
            try
            {
                var categorias = (await _productoServicio.ObtenerCategoriasAsync(_shell.IdEmpresaActual)).ToList();

                // Insertar opción "Todas las categorías" al inicio
                var lista = new List<CategoriaItemDto>(categorias.Count + 1);
                lista.Add(new CategoriaItemDto { IdCategoria = 0, Nombre = "Todos" });
                lista.AddRange(categorias);

                Categorias = new ObservableCollection<CategoriaItemDto>(lista);

                // Seleccionar "Todos" por defecto
                CategoriaSeleccionada = lista[0];
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cargando categorías");
                MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarAsync()
        {
            try
            {
                IsLoading = true;
                var productos = await _productoServicio.ObtenerTodosAsync(_shell.IdEmpresaActual);
                var productosList = productos.ToList();

                // Aplicar filtros
                var filtrados = productosList.AsEnumerable();

                // Filtro por texto de búsqueda
                if (!string.IsNullOrWhiteSpace(TextoBusqueda))
                {
                    var busqueda = TextoBusqueda.Trim().ToLower();
                    filtrados = filtrados.Where(p =>
                        (p.Nombre?.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.CodigoBarra?.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ?? false));
                }

                // Filtro por categoría (IdCategoria == 0 = "Todos")
                if (CategoriaSeleccionada?.IdCategoria > 0)
                {
                    filtrados = filtrados.Where(p => p.IdCategoria == CategoriaSeleccionada.IdCategoria);
                }

                // Filtro por estado activo/inactivo
                if (FiltroActivo == 1)
                    filtrados = filtrados.Where(p => p.Activo);
                else if (FiltroActivo == 2)
                    filtrados = filtrados.Where(p => !p.Activo);

                // Filtro stock crítico (desde dashboard "Ver todos")
                if (MostrarSoloStockCritico)
                    filtrados = filtrados.Where(p => p.StockActual <= p.StockMinimo);

                var filtradosList = filtrados.ToList();
                Productos = new ObservableCollection<ProductoListadoDto>(filtradosList);
                TotalProductos = filtradosList.Count;

                // Métricas (sobre toda la lista, no filtrada)
                ProductosActivos = productosList.Count(p => p.Activo);
                ProductosStockBajo = productosList.Count(p => p.StockActual > 0 && p.StockActual <= 10);
                ProductosSinStock = productosList.Count(p => p.StockActual <= 0);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cargando productos");
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Acciones ──────────────────────────────────────────────────
        public async Task Buscar()
        {
            if (!await _lock.WaitAsync(0)) return;
            try
            {
                PaginaActual = 1;
                await CargarAsync();
            }
            finally
            {
                _lock.Release();
            }
        }

        ///         /// Wrapper fire-and-forget seguro para Buscar() desde setters de propiedades.
        /// Engulle cualquier excepción para evitar unobserved task exceptions.
        private async Task BuscarConCatch()
        {
            try
            {
                await Buscar();
            }
            catch
            {
                // Buscar() ya maneja errores internamente via CargarAsync();
                // este catch solo protege contra unobserved task exceptions.
            }
        }

        public async Task NuevoProducto()
        {
            // ProductoFormularioViewModel sirve tanto para crear como para editar
            var vm = IoC.Get<ProductoFormularioViewModel>();
            vm.InicializarParaCrear();
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        // ── Popup: Crear / Eliminar Categoría ──────────────────────────
        private bool _mostrarPopupCategoria;
        public bool MostrarPopupCategoria
        {
            get => _mostrarPopupCategoria;
            set { _mostrarPopupCategoria = value; NotifyOfPropertyChange(() => MostrarPopupCategoria); }
        }

        private string _nombreNuevaCategoria = string.Empty;
        public string NombreNuevaCategoria
        {
            get => _nombreNuevaCategoria;
            set { _nombreNuevaCategoria = value; NotifyOfPropertyChange(() => NombreNuevaCategoria); }
        }

        private ObservableCollection<CategoriaItemDto> _categoriasGestion = new();
        public ObservableCollection<CategoriaItemDto> CategoriasGestion
        {
            get => _categoriasGestion;
            set { _categoriasGestion = value; NotifyOfPropertyChange(() => CategoriasGestion); }
        }

        public async Task AbrirPopupCategorias()
        {
            NombreNuevaCategoria = string.Empty;
            MostrarPopupCategoria = true;
            await CargarCategoriasGestionAsync();
        }

        private async Task CargarCategoriasGestionAsync()
        {
            var categorias = (await _productoServicio.ObtenerCategoriasAsync(_shell.IdEmpresaActual)).ToList();
            CategoriasGestion = new ObservableCollection<CategoriaItemDto>(categorias);
        }

        public async Task CrearCategoria()
        {
            if (string.IsNullOrWhiteSpace(NombreNuevaCategoria)) return;

            try
            {
                await _productoServicio.CrearCategoriaAsync(_shell.IdEmpresaActual, NombreNuevaCategoria);
                NombreNuevaCategoria = string.Empty;
                await CargarCategoriasGestionAsync();
                await CargarCategoriasAsync();
                await Buscar();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al crear categoría");
                MessageBox.Show($"Error al crear categoría: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Renombrar categoría ──────────────────────────────────────────
        public async Task RenombrarCategoria(CategoriaItemDto categoria)
        {
            if (categoria == null || categoria.IdCategoria <= 0) return;

            var nuevoNombre = Microsoft.VisualBasic.Interaction.InputBox(
                "Nuevo nombre para la categoría:",
                "Renombrar categoría",
                categoria.Nombre);

            if (string.IsNullOrWhiteSpace(nuevoNombre) || nuevoNombre.Trim() == categoria.Nombre)
                return;

            try
            {
                await _productoServicio.ActualizarCategoriaAsync(categoria.IdCategoria, nuevoNombre.Trim());
                await CargarCategoriasGestionAsync();
                await CargarCategoriasAsync();
                await Buscar();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al renombrar categoría");
                MessageBox.Show($"Error al renombrar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Borrar todos los productos de una categoría ──────────────────
        public async Task EliminarTodosLosProductos(CategoriaItemDto categoria)
        {
            if (categoria == null || categoria.IdCategoria <= 0) return;

            var confirm = MessageBox.Show(
                $"¿Estás SEGURO de eliminar TODOS los productos de la categoría \"{categoria.Nombre}\"?\n\n" +
                "Esta acción NO se puede deshacer.",
                "Eliminar todos los productos", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                var cantidad = await _productoServicio.EliminarProductosPorCategoriaAsync(categoria.IdCategoria);
                if (cantidad > 0)
                {
                    MessageBox.Show($"Se eliminaron {cantidad} producto(s) de la categoría \"{categoria.Nombre}\".",
                        "Productos eliminados", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarCategoriasGestionAsync();
                    await CargarCategoriasAsync();
                    await Buscar();
                }
                else
                {
                    MessageBox.Show("No hay productos en esta categoría.",
                        "Sin productos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al eliminar productos por categoría");
                MessageBox.Show($"Error al eliminar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task EliminarCategoria(CategoriaItemDto categoria)
        {
            if (categoria == null || categoria.IdCategoria <= 0) return;

            var confirm = MessageBox.Show(
                $"¿Estás seguro de eliminar la categoría \"{categoria.Nombre}\"?\n\n" +
                "Los productos se moverán a una nueva categoría \"Sin Categoría\" que podés renombrar después.",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                var resultado = await _productoServicio.EliminarCategoriaAsync(categoria.IdCategoria);
                if (resultado)
                {
                    await CargarCategoriasGestionAsync();
                    await CargarCategoriasAsync();
                    await Buscar();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "No se puede eliminar", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al eliminar categoría");
                var inner = ex.InnerException?.Message ?? "";
                var mensaje = inner != ""
                    ? $"Error al eliminar categoría:\n{ex.Message}\n\nDetalle: {inner}"
                    : $"Error al eliminar categoría: {ex.Message}";
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CerrarPopupCategorias()
        {
            NombreNuevaCategoria = string.Empty;
            MostrarPopupCategoria = false;
        }

        // ── Ajuste Masivo de Precios ─────────────────────────────────
        public void AbrirAjusteMasivo()
        {
            MostrarPopupAjuste = true;
            PorcentajeAjuste = 0;
            MontoFijo = 0;
            TipoAjuste = "porcentaje";
            CategoriaAjuste = Categorias.FirstOrDefault(); // "Todos" por defecto
            // Pre-cargar preview con todos los productos visibles
            _productosPreview = new ObservableCollection<ProductoListadoDto>(Productos);
            NotifyOfPropertyChange(() => ProductosPreview);
        }

        // Nueva: Ajuste por Proveedor (aplicación en modal de precios)
        public ObservableCollection<Proveedor> ProveedoresAjuste { get; set; } = new ObservableCollection<Proveedor>();
        public int? ProveedorAjusteId { get; set; }
        public decimal PorcentajeAjusteProveedor { get; set; }

        public async Task CargarProveedoresAjusteAsync()
        {
            var proveedores = await _productoServicio.ObtenerProveedoresAsync();
            ProveedoresAjuste = new ObservableCollection<Proveedor>(proveedores);
            NotifyOfPropertyChange(() => ProveedoresAjuste);
        }

        public async Task AplicarAjusteProveedorAsync()
        {
            if (ProveedorAjusteId == null || ProveedorAjusteId <= 0)
            {
                MessageBox.Show("Seleccione un proveedor para el ajuste", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (PorcentajeAjusteProveedor == 0)
            {
                MessageBox.Show("Indique un porcentaje distinto de 0 para el ajuste", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // Confirmación simple para el usuario final
            var confirm = MessageBox.Show($"¿Está seguro de aplicar el ajuste de {PorcentajeAjusteProveedor}% para el proveedor seleccionado?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            var (nuevos, actualizados) = await _productoServicio.AjustePreciosPorProveedorAsync(ProveedorAjusteId.Value, PorcentajeAjusteProveedor);
            ProductosActualizados = actualizados;
            Status = $"Ajuste aplicado: {actualizados} productos actualizados";
            NotifyOfPropertyChange(() => Status);
            await CargarAsync();
        }

        public void CancelarAjusteMasivo()
        {
            MostrarPopupAjuste = false;
            ProductosPreview.Clear();
        }

        public void GenerarPreviewAjuste()
        {
            // Generar preview sin modificar la base de datos
            var preview = new ObservableCollection<ProductoListadoDto>();

            // Filtrar por categoría si se eligió una específica en el popup
            var fuente = Productos.AsEnumerable();
            if (CategoriaAjuste?.IdCategoria > 0)
                fuente = fuente.Where(p => p.IdCategoria == CategoriaAjuste.IdCategoria);

            // Si no hay valor, mostrar todos los productos actuales (filtrados por categoría)
            if (PorcentajeAjuste == 0 && MontoFijo == 0)
            {
                foreach (var p in fuente)
                {
                    var copia = new ProductoListadoDto
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        CodigoBarra = p.CodigoBarra,
                        PrecioVentaActual = p.PrecioVentaActual,
                        PrecioCostoActual = p.PrecioCostoActual,
                        PrecioVentaNuevo = p.PrecioVentaActual,
                        PrecioCostoNuevo = p.PrecioCostoActual,
                    };
                    preview.Add(copia);
                }
                ProductosPreview = preview;
                ProductosActualizados = preview.Count;
                return;
            }
            
            foreach (var p in fuente)
            {
                decimal nuevoVenta = p.PrecioVentaActual;
                decimal nuevoCosto = p.PrecioCostoActual;

                if (TipoAjuste == "porcentaje")
                {
                    if (AplicarAPrecioVenta)
                        nuevoVenta = Math.Round(p.PrecioVentaActual * (1 + PorcentajeAjuste / 100m), 2);
                    if (AplicarAPrecioCosto)
                        nuevoCosto = Math.Round(p.PrecioCostoActual * (1 + PorcentajeAjuste / 100m), 2);
                }
                else if (TipoAjuste == "fijo")
                {
                    if (AplicarAPrecioVenta)
                        nuevoVenta = p.PrecioVentaActual + MontoFijo;
                    if (AplicarAPrecioCosto)
                        nuevoCosto = p.PrecioCostoActual + MontoFijo;
                }

                // Siempre agregar paraver el cambio
                var copia = new ProductoListadoDto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    CodigoBarra = p.CodigoBarra,
                    PrecioVentaActual = p.PrecioVentaActual,
                    PrecioCostoActual = p.PrecioCostoActual,
                    PrecioVentaNuevo = nuevoVenta,
                    PrecioCostoNuevo = nuevoCosto,
                };
                preview.Add(copia);
            }

            ProductosPreview = preview;
            ProductosActualizados = preview.Count;
        }

        public async Task ConfirmarAjusteMasivo()
        {
            if (ProductosPreview.Count == 0)
            {
                MessageBox.Show("No hay productos para actualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Se actualizarán {ProductosActualizados} productos.\n\n" +
                $"Precio venta: {(AplicarAPrecioVenta ? "✓ SÍ" : "✗ NO")}\n" +
                $"Precio costo: {(AplicarAPrecioCosto ? "✓ SÍ" : "✗ NO")}\n" +
                $"Tipo: {TipoAjuste}  Valor: {(TipoAjuste == "porcentaje" ? PorcentajeAjuste.ToString("0.#") + "%" : MontoFijo.ToString("C0"))}\n\n¿Continuar?",
                "Confirmar Ajuste Masivo",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                int actualizada = 0;

                foreach (var p in ProductosPreview)
                {
                    // Obtener producto actual de la DB para actualizar
                    var producto = await _productoServicio.ObtenerPorIdAsync(p.IdProducto);
                    if (producto != null)
                    {
                        if (AplicarAPrecioVenta)
                            producto.PrecioVentaActual = p.PrecioVentaNuevo ?? producto.PrecioVentaActual;
                        if (AplicarAPrecioCosto)
                            producto.PrecioCostoActual = p.PrecioCostoNuevo ?? producto.PrecioCostoActual;

                        // Map to DTO
                        var dto = new ProductoActualizarDto
                        {
                            IdProducto = producto.IdProducto,
                            Nombre = producto.Nombre,
                            CodigoBarra = producto.CodigoBarra,
                            PrecioVentaActual = producto.PrecioVentaActual,
                            PrecioCostoActual = producto.PrecioCostoActual,
                            StockMinimo = producto.StockMinimo,
                            Activo = producto.Activo,
                            IdCategoria = producto.IdCategoria,
                            IdUnidadMedida = producto.IdUnidadMedida
                        };
                        
                        await _productoServicio.ActualizarAsync(dto);
                        actualizada++;
                    }
                }

                MessageBox.Show($"Se actualizaron {actualizada} productos correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                
                MostrarPopupAjuste = false;
                await CargarAsync(); // Recargar lista
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error en ajuste masivo");
                MessageBox.Show("Error al aplicar ajuste: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task EditarProducto()
        {
            if (ProductoSeleccionado == null) return;
            var vm = IoC.Get<ProductoFormularioViewModel>();
            vm.InicializarParaEditar(ProductoSeleccionado.IdProducto);
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public void CerrarDetalle() => ProductoSeleccionado = null;

        public async Task VerMovimientos()
        {
            if (ProductoSeleccionado == null) return;

            try
            {
                var movimientos = await _inventarioServicio.ObtenerMovimientosPorProductoAsync(ProductoSeleccionado.IdProducto);
                var lista = movimientos.ToList();

                if (lista.Count == 0)
                {
                    MessageBox.Show("No hay movimientos registrados para este producto.", 
                        "Sin movimientos", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Mostrar resumen
                var entradas = lista.Count(m => m.TipoMovimiento == "Entrada");
                var salidas = lista.Count(m => m.TipoMovimiento == "Salida");
                var ajustes = lista.Count(m => m.TipoMovimiento == "Ajuste");

                var mensaje = $"Movimientos de '{ProductoSeleccionado.Nombre}':\n\n";
                mensaje += $"Entradas: {entradas}\n";
                mensaje += $"Salidas: {salidas}\n";
                mensaje += $"Ajustes: {ajustes}\n\n";
                mensaje += $"Total: {lista.Count} movimientos";

                MessageBox.Show(mensaje, "Historial de Stock", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error obteniendo movimientos");
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void DesactivarProducto()
        {
            if (ProductoSeleccionado == null) return;
            var result = MessageBox.Show($"¿Desactivar producto {ProductoSeleccionado.Nombre}?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                await _productoServicio.DesactivarAsync(ProductoSeleccionado.IdProducto);
                await CargarAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error desactivando producto");
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Paginación ────────────────────────────────────────────────
        public bool CanPaginaAnterior => PaginaActual > 1;
        public async Task PaginaAnterior()
        {
            if (PaginaActual > 1) { PaginaActual--; await CargarAsync(); }
            NotifyOfPropertyChange(() => CanPaginaAnterior);
            NotifyOfPropertyChange(() => CanPaginaSiguiente);
        }

        public bool CanPaginaSiguiente => PaginaActual < TotalPaginas;
        public async Task PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) { PaginaActual++; await CargarAsync(); }
            NotifyOfPropertyChange(() => CanPaginaAnterior);
            NotifyOfPropertyChange(() => CanPaginaSiguiente);
        }
    }
}
