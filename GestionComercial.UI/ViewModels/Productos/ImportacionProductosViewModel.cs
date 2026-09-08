using Caliburn.Micro;
using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Importacion;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ImportacionProductosViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<ImportacionProductosViewModel>? _logger;

        public ImportacionProductosViewModel(
            IProductoServicio productoServicio,
            ShellViewModel shell,
            ILogger<ImportacionProductosViewModel>? logger = null)
        {
            _productoServicio = productoServicio;
            _shell = shell;
            _logger = logger;
        }

        // ── Estado ───────────────────────────────────────────────────────────
        public enum EstadoImportacion { Inicial, Previsualizando, Importando, Completado, ConErrores }

        private EstadoImportacion _estado = EstadoImportacion.Inicial;
        public EstadoImportacion Estado
        {
            get => _estado;
            set
            {
                _estado = value;
                NotifyOfPropertyChange(() => Estado);
                NotifyOfPropertyChange(() => MostrarDropzone);
                NotifyOfPropertyChange(() => MostrarPreview);
                NotifyOfPropertyChange(() => MostrarResultado);
                NotifyOfPropertyChange(() => PuedeImportar);
                NotifyOfPropertyChange(() => PuedeSeleccionarArchivo);
                NotifyOfPropertyChange(() => MostrarBotonReimportar);
            }
        }

        public bool MostrarDropzone => Estado == EstadoImportacion.Inicial;
        public bool MostrarPreview => Estado == EstadoImportacion.Previsualizando
                                     || Estado == EstadoImportacion.Importando;
        public bool MostrarResultado => Estado == EstadoImportacion.Completado
                                      || Estado == EstadoImportacion.ConErrores;
        public bool MostrarBotonReimportar => Estado == EstadoImportacion.Completado
                                             || Estado == EstadoImportacion.ConErrores;

        public bool PuedeImportar => Estado == EstadoImportacion.Previsualizando && FilasValidas > 0;
        public bool PuedeSeleccionarArchivo => Estado == EstadoImportacion.Inicial;

        // ── Archivo ──────────────────────────────────────────────────────────
        private string _archivoNombre = string.Empty;
        public string ArchivoNombre
        {
            get => _archivoNombre;
            set { _archivoNombre = value; NotifyOfPropertyChange(() => ArchivoNombre); }
        }

        private string _archivoRuta = string.Empty;
        public string ArchivoRuta
        {
            get => _archivoRuta;
            set { _archivoRuta = value; NotifyOfPropertyChange(() => ArchivoRuta); }
        }

        // ── Full rows (import source; NOT bound to the UI) ────────────────────
        // Plain List on purpose: avoids materializing a 10k+ ObservableCollection
        // nobody binds to. The UI binds exclusively to FilasPreview.
        private List<FilaImportacionDto> _filas = new();
        public List<FilaImportacionDto> Filas
        {
            get => _filas;
            set { _filas = value; NotifyOfPropertyChange(() => Filas); }
        }

        // ── Filas preview (capped for UI performance) ─────────────────────────
        private const int LimiteFilasPreview = 500;

        private ObservableCollection<FilaImportacionDto> _filasPreview = new();
        public ObservableCollection<FilaImportacionDto> FilasPreview
        {
            get => _filasPreview;
            set { _filasPreview = value; NotifyOfPropertyChange(() => FilasPreview); }
        }

        private string _avisoPreview = string.Empty;
        public string AvisoPreview
        {
            get => _avisoPreview;
            set { _avisoPreview = value; NotifyOfPropertyChange(() => AvisoPreview); }
        }

        // ── Collapsible preview list ──────────────────────────────────────────
        private bool _mostrarLista = false;
        public bool MostrarLista
        {
            get => _mostrarLista;
            set
            {
                _mostrarLista = value;
                NotifyOfPropertyChange(() => MostrarLista);
                NotifyOfPropertyChange(() => FlechaListaTexto);
            }
        }

        public string FlechaListaTexto => MostrarLista ? "▲ Ocultar lista" : "▼ Mostrar lista";

        public void ToggleLista()
        {
            MostrarLista = !MostrarLista;
        }

        private int _filasValidas;
        public int FilasValidas
        {
            get => _filasValidas;
            set
            {
                _filasValidas = value;
                NotifyOfPropertyChange(() => FilasValidas);
                NotifyOfPropertyChange(() => PuedeImportar);
            }
        }

        private int _filasConError;
        public int FilasConError
        {
            get => _filasConError;
            set { _filasConError = value; NotifyOfPropertyChange(() => FilasConError); }
        }

        private int _filasTotales;
        public int FilasTotales
        {
            get => _filasTotales;
            set { _filasTotales = value; NotifyOfPropertyChange(() => FilasTotales); }
        }

        // ── Resultado ────────────────────────────────────────────────────────
        private int _importados;
        public int Importados
        {
            get => _importados;
            set { _importados = value; NotifyOfPropertyChange(() => Importados); }
        }

        private int _omitidos;
        public int Omitidos
        {
            get => _omitidos;
            set { _omitidos = value; NotifyOfPropertyChange(() => Omitidos); }
        }

        private int _actualizados;
        public int Actualizados
        {
            get => _actualizados;
            set { _actualizados = value; NotifyOfPropertyChange(() => Actualizados); }
        }

        // ── Progreso ─────────────────────────────────────────────────────────
        private int _progreso;
        public int Progreso
        {
            get => _progreso;
            set { _progreso = value; NotifyOfPropertyChange(() => Progreso); }
        }

        private string _textoProgreso = string.Empty;
        public string TextoProgreso
        {
            get => _textoProgreso;
            set { _textoProgreso = value; NotifyOfPropertyChange(() => TextoProgreso); }
        }

        // ── Opciones ─────────────────────────────────────────────────────────
        private bool _actualizarExistentes = true;
        public bool ActualizarExistentes
        {
            get => _actualizarExistentes;
            set { _actualizarExistentes = value; NotifyOfPropertyChange(() => ActualizarExistentes); }
        }

        private bool _crearCategorias = true;
        public bool CrearCategorias
        {
            get => _crearCategorias;
            set { _crearCategorias = value; NotifyOfPropertyChange(() => CrearCategorias); }
        }

        // ── Error visible en dropzone ─────────────────────────────────────────
        private bool _tieneError;
        public new bool TieneError
        {
            get => _tieneError;
            set { _tieneError = value; NotifyOfPropertyChange(() => TieneError); }
        }

        private string _mensajeError = string.Empty;
        public new string MensajeError
        {
            get => _mensajeError;
            set { _mensajeError = value; NotifyOfPropertyChange(() => MensajeError); }
        }

        // ── Eventos para el formulario padre ─────────────────────────────────
        public event System.Action ImportacionCompletada;
        public event System.Action Cancelado;

        // ── Mapeo de categorías detectadas en el Excel ────────────────────────
        private Dictionary<string, int> _mapaCategoriasExistentes = new();
        private List<string> _categoriasPorCrear = new();
        public IReadOnlyList<string> CategoriasPorCrear => _categoriasPorCrear;

        // ── Ruta temporal para re-importar ────────────────────────────────────────
        private string _rutaPlantillaTemporal = string.Empty;
        public string RutaPlantillaTemporal
        {
            get => _rutaPlantillaTemporal;
            private set { _rutaPlantillaTemporal = value; NotifyOfPropertyChange(() => TienePlantillaTemporal); }
        }

        public bool TienePlantillaTemporal => !string.IsNullOrEmpty(_rutaPlantillaTemporal);

        // ── Seleccionar archivo ───────────────────────────────────────────────
        public async Task SeleccionarArchivo()
        {
            var dialogo = new Microsoft.Win32.OpenFileDialog
            {
                Title  = "Seleccionar archivo Excel",
                Filter = "Excel (*.xlsx;*.xls)|*.xlsx;*.xls",
            };

            if (dialogo.ShowDialog() != true) return;

            ArchivoRuta   = dialogo.FileName;
            ArchivoNombre = System.IO.Path.GetFileName(dialogo.FileName);
            TieneError    = false;
            MensajeError  = string.Empty;

            await PrevisualizarArchivo();
        }

        // ── Previsualizar ─────────────────────────────────────────────────────
        public async Task PrevisualizarArchivo()
        {
            if (string.IsNullOrEmpty(ArchivoRuta)) return;

            IsLoading = true;
            TieneError = false;

            try
            {
                var idEmpresa = _shell.IdEmpresaActual;
                var catsExistentes = await _productoServicio.ObtenerCategoriasAsync(idEmpresa);
                _mapaCategoriasExistentes = catsExistentes.ToDictionary(c => c.Nombre.ToLower().Trim(), c => c.IdCategoria);

                var filas = await LeerExcelYValidar(ArchivoRuta);

                FilasTotales  = filas.Count;
                FilasValidas  = filas.Count(f => f.EsValida);
                FilasConError = filas.Count(f => !f.EsValida);
                Filas = filas;

                // Cap preview collection for UI performance (500 rows max)
                FilasPreview = new ObservableCollection<FilaImportacionDto>(
                    filas.Take(LimiteFilasPreview));
                AvisoPreview = FilasTotales > LimiteFilasPreview
                    ? $"Mostrando {FilasPreview.Count} de {FilasTotales} filas (vista previa)"
                    : string.Empty;

                Estado = EstadoImportacion.Previsualizando;
            }
            catch (Exception ex)
            {
                TieneError   = true;
                MensajeError = ex.Message.Contains("same key", StringComparison.OrdinalIgnoreCase)
                    ? $"El archivo tiene categorías duplicadas. Revisá que no haya nombres de categorías con diferencias de mayúsculas/minúsculas o espacios extra."
                    : $"Error al leer el archivo: {ex.Message}";
                Estado       = EstadoImportacion.Inicial;
            }
            finally
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => PuedeImportar);
            }
        }

        private async Task<List<FilaImportacionDto>> LeerExcelYValidar(string ruta)
        {
            return await Task.Run(() =>
            {
                var filas = new List<FilaImportacionDto>();

                using var workbook = new XLWorkbook(ruta);
                var hoja = workbook.Worksheet(1);
                // RowsUsed() directly (avoids building the whole-sheet RangeUsed
                // object, which is slower on large files). It yields the same row
                // set: only rows with at least one used cell; all-empty rows are
                // dropped anyway by FilaVacia below.
                var filasUsadas = hoja.RowsUsed().ToList();

                if (filasUsadas.Count < 2)
                    throw new Exception("El archivo debe tener al menos una fila de encabezado y una fila de datos.");

                var encabezado = filasUsadas[0];
                var mapaColumnas = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Mapear por NÚMERO DE COLUMNA REAL del archivo (no por índice de enumeración)
                foreach (var celda in encabezado.CellsUsed())
                {
                    var nombreCol = celda.GetString().Trim().ToLower();
                    if (nombreCol.Length > 0)
                        mapaColumnas[nombreCol] = celda.Address.ColumnNumber;
                }

                string ValorCelda(IXLRow filaExcel, string nombre)
                    => mapaColumnas.TryGetValue(nombre, out int col)
                        ? (filaExcel.Cell(col).GetString()?.Trim() ?? string.Empty)
                        : string.Empty;

                bool FilaVacia(string nombre, string codigo, string venta, string costo,
                    string stock, string stockMin)
                    => string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(codigo)
                       && string.IsNullOrEmpty(venta) && string.IsNullOrEmpty(costo)
                       && string.IsNullOrEmpty(stock) && string.IsNullOrEmpty(stockMin);

                // Guard de esquema: DataTable solo con las primeras 100 filas NO vacías
                // (el guard valida internamente ese mismo límite). Evita materializar
                // todo el archivo a string dos veces.
                var table = new DataTable();
                foreach (var col in mapaColumnas)
                    table.Columns.Add(col.Key, typeof(string));

                var guardrails = new ProductoImportGuardrails();
                var dtosImportacion = new List<ProductoImportarDto>();
                int filasParaGuard = 0;

                foreach (var filaExcel in filasUsadas.Skip(1))
                {
                    var nombre = ValorCelda(filaExcel, "nombre");
                    var codigoBarra = ValorCelda(filaExcel, "codigobarra");
                    var pVentaStr = ValorCelda(filaExcel, "precioventa");
                    var pCostoStr = ValorCelda(filaExcel, "preciocosto");
                    var stockStr = ValorCelda(filaExcel, "stockactual");
                    var stockMinStr = ValorCelda(filaExcel, "stockminimo");
                    var categoria = ValorCelda(filaExcel, "categoria");
                    var unidadMedida = ValorCelda(filaExcel, "unidadmedida");

                    // Normalización: omitir filas vacías o con solo espacios
                    if (FilaVacia(nombre, codigoBarra, pVentaStr, pCostoStr, stockStr, stockMinStr))
                        continue;

                    if (filasParaGuard < 100)
                    {
                        var newRow = table.NewRow();
                        newRow["nombre"] = nombre;
                        newRow["codigobarra"] = codigoBarra;
                        newRow["precioventa"] = pVentaStr;
                        newRow["preciocosto"] = pCostoStr;
                        newRow["stockactual"] = stockStr;
                        newRow["stockminimo"] = stockMinStr;
                        newRow["categoria"] = categoria;
                        newRow["unidadmedida"] = unidadMedida;
                        table.Rows.Add(newRow);
                        filasParaGuard++;
                    }

                    // Parsing tolerante a la cultura ("24000.50", "$1,234.56", "1.234,56")
                    ImportacionNormalizacion.TryParseDecimal(pVentaStr, out decimal precioVenta);
                    ImportacionNormalizacion.TryParseDecimal(pCostoStr, out decimal precioCosto);
                    ImportacionNormalizacion.TryParseInt(stockStr, out int stock);
                    ImportacionNormalizacion.TryParseInt(stockMinStr, out int stockMinimo);

                    int? idCategoria = null;
                    if (!string.IsNullOrWhiteSpace(categoria))
                    {
                        var catNorm = categoria.ToLower().Trim();
                        if (_mapaCategoriasExistentes.TryGetValue(catNorm, out int idCat))
                            idCategoria = idCat;
                        else if (!_categoriasPorCrear.Contains(categoria.Trim()))
                            _categoriasPorCrear.Add(categoria.Trim());
                    }

                    filas.Add(new FilaImportacionDto
                    {
                        Fila = filaExcel.RowNumber(), // número de fila REAL del Excel
                        Nombre = nombre,
                        CodigoBarra = codigoBarra,
                        PrecioVenta = precioVenta,
                        PrecioVentaOriginal = precioVenta,
                        PrecioCosto = precioCosto,
                        PrecioCostoOriginal = precioCosto,
                        Stock = stock,
                        StockMinimo = stockMinimo,
                        Categoria = categoria,
                        UnidadMedida = string.IsNullOrWhiteSpace(unidadMedida) ? "Unidad" : unidadMedida,
                        IdCategoria = idCategoria,
                        EsValida = true,
                        ErrorDescripcion = string.Empty,
                    });

                    dtosImportacion.Add(new ProductoImportarDto
                    {
                        Nombre = nombre,
                        CodigoBarra = codigoBarra,
                        PrecioVentaActual = precioVenta,
                        PrecioCostoActual = precioCosto,
                        StockActual = stock,
                        StockMinimo = stockMinimo,
                        Categoria = categoria,
                        UnidadMedida = string.IsNullOrWhiteSpace(unidadMedida) ? "Unidad" : unidadMedida,
                        IdEmpresa = _shell.IdEmpresaActual,
                        IdCategoria = idCategoria ?? 0,
                        IdUnidadMedida = 1,
                    });
                }

                var schemaResult = ImportacionSchemaGuard.Validate(table);
                if (schemaResult.HasFatalErrors)
                {
                    var fatalErrors = string.Join("\n", schemaResult.Errors.Where(e => e.IsFatal).Select(e => e.Message));
                    throw new Exception($"Error de esquema:\n{fatalErrors}");
                }

                // Ejecutar guardrails de negocio
                var guardResults = guardrails.ValidateBatch(dtosImportacion);
                for (int i = 0; i < guardResults.Count; i++)
                {
                    var (rowDto, results) = guardResults[i];
                    var errors = results.Where(r => !r.Passed).ToList();
                    if (errors.Any())
                    {
                        filas[i].EsValida = !errors.Any(r => r.Severity == GuardSeverity.Error);
                        filas[i].ErrorDescripcion = string.Join("; ", errors.Select(r => r.Message));
                    }
                }

                return filas;
            });
        }

        // ── Ejecutar importación con bulk ───────────────────────────────────────────
        public async void EjecutarImportacion()
        {
            if (!PuedeImportar) return;

            Estado    = EstadoImportacion.Importando;
            IsLoading = true;
            Progreso = 0;
            TextoProgreso = "Iniciando importación...";

            // Allow UI to render before starting heavy work
            await Task.Delay(50);

            var filasAImportar = Filas.Where(f => f.EsValida).ToList();

            try
            {
                // Convertir filas a DTOs
                var dtos = new List<ProductoImportarDto>();
                foreach (var fila in filasAImportar)
                {
                    int idCategoria = 1;
                    if (!string.IsNullOrWhiteSpace(fila.Categoria))
                    {
                        var catNorm = fila.Categoria.ToLower().Trim();
                        if (_mapaCategoriasExistentes.TryGetValue(catNorm, out int idCat))
                            idCategoria = idCat;
                    }

                    dtos.Add(new ProductoImportarDto
                    {
                        Nombre = fila.Nombre,
                        CodigoBarra = fila.CodigoBarra ?? string.Empty,
                        Categoria = fila.Categoria,
                        PrecioVentaActual = Math.Round(fila.PrecioVenta, 2),
                        PrecioCostoActual = fila.PrecioCosto,
                        StockActual = fila.Stock,
                        StockMinimo = fila.StockMinimo > 0 ? fila.StockMinimo : 10,
                        IdCategoria = idCategoria,
                        IdUnidadMedida = 1,
                        IdEmpresa = _shell.IdEmpresaActual,
                    });
                }

                // Progress callback - use BeginInvoke to update UI from background thread without blocking
                var progress = new Progress<(int current, int total, string message)>(p =>
                {
                    // Marshal to UI thread without blocking
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Progreso = p.total > 0 ? (int)((p.current / (double)p.total) * 100) : 0;
                        TextoProgreso = p.message;
                    });
                });

                // Importar en bulk con guardrails y commit parcial
                var importResult = await _productoServicio.ImportarMasivoAsync(
                    dtos,
                    ActualizarExistentes,
                    progress);

                Importados = importResult.Inserted;
                Actualizados = importResult.Updated;
                Omitidos = importResult.Skipped + FilasConError;

                Estado = Omitidos > 0
                    ? EstadoImportacion.ConErrores
                    : EstadoImportacion.Completado;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error durante la importacion bulk");
                TieneError = true;
                var innerMsg = ex.InnerException?.Message;
                MensajeError = $"Error durante la importación: {ex.Message}";
                if (!string.IsNullOrEmpty(innerMsg))
                    MensajeError += $"\nDetalle: {innerMsg}";
                Estado = EstadoImportacion.ConErrores;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CrearCategoriasAsync(IReadOnlyList<string> categorias)
        {
            foreach (var _ in categorias)
                await Task.Delay(10);
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public void Reiniciar()
        {
            ArchivoRuta   = string.Empty;
            ArchivoNombre = string.Empty;
            Filas         = new List<FilaImportacionDto>();
            FilasPreview  = new ObservableCollection<FilaImportacionDto>();
            AvisoPreview  = string.Empty;
            FilasTotales  = 0;
            FilasValidas  = 0;
            FilasConError = 0;
            MostrarLista  = false;
            Importados    = 0;
            Actualizados  = 0;
            Omitidos      = 0;
            Progreso      = 0;
            TextoProgreso = string.Empty;
            TieneError    = false;
            MensajeError  = string.Empty;
            _categoriasPorCrear.Clear();
            _mapaCategoriasExistentes.Clear();
            Estado = EstadoImportacion.Inicial;
        }

        public void Reimportar() => Reiniciar();
        public void Finalizar()  => ImportacionCompletada?.Invoke();
        public void Cancelar()   => Cancelado?.Invoke();

        // ── Volver al listado de productos ────────────────────────────────────
        public async Task Volver()
        {
            var listado = IoC.Get<ProductoListadoViewModel>();
            await _shell.ActivateItemAsync(listado, CancellationToken.None);
        }

        // ── Descargar plantilla ───────────────────────────────────────────────
        private bool _descargandoPlantilla;

        public async Task DescargarPlantilla()
        {
            // Guard against concurrent download requests.
            if (_descargandoPlantilla) return;
            _descargandoPlantilla = true;

            try
            {
                var dialogo = new Microsoft.Win32.SaveFileDialog
                {
                    Title      = "Guardar plantilla de importación",
                    FileName   = "Plantilla_Productos.xlsx",
                    DefaultExt = ".xlsx",
                    Filter     = "Excel (*.xlsx)|*.xlsx",
                };

                if (dialogo.ShowDialog() != true) return;

                var filePath = dialogo.FileName;

                await Task.Run(() =>
                {
                    using var workbook = new XLWorkbook();
                    var hoja = workbook.Worksheets.Add("Productos");

                    // Exact schema headers: same names and order as ImportacionSchema.
                    var encabezados = ImportacionSchema.SchemaDefinicion
                        .Select(c => c.Name)
                        .ToArray();

                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        var celda = hoja.Cell(1, i + 1);
                        celda.Value = encabezados[i];
                        celda.Style.Font.Bold = true;
                        celda.Style.Font.FontColor = XLColor.White;
                        celda.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
                        celda.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    // Sensible column widths for the schema columns.
                    var anchos = new[] { 25, 35, 15, 15, 22, 18, 13, 13, 16 };
                    for (int i = 0; i < anchos.Length && i < encabezados.Length; i++)
                        hoja.Column(i + 1).Width = anchos[i];

                    // Force CodigoBarra (column 5) to text so long codes and
                    // leading zeros are preserved exactly as typed.
                    hoja.Column(5).Style.NumberFormat.Format = "@";

                    // Freeze the header row and add an auto-filter on the header.
                    hoja.SheetView.FreezeRows(1);
                    hoja.RangeUsed()?.SetAutoFilter();

                    workbook.SaveAs(filePath, new SaveOptions { ValidatePackage = false });
                });
            }
            finally
            {
                _descargandoPlantilla = false;
            }
        }

        ///         /// Re-importa la última plantilla descargada sin pedir archivo.
        public async Task ReimportarUltimaPlantilla()
        {
            if (!TienePlantillaTemporal || !System.IO.File.Exists(RutaPlantillaTemporal))
            {
                await DescargarPlantilla();
                return;
            }

            ArchivoRuta = RutaPlantillaTemporal;
            ArchivoNombre = System.IO.Path.GetFileName(RutaPlantillaTemporal);
            await PrevisualizarArchivo();
        }
    }

}