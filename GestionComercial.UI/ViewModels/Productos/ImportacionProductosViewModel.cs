using Caliburn.Micro;
using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // ── Filas preview ────────────────────────────────────────────────────
        private ObservableCollection<FilaImportacionDto> _filas = new();
        public ObservableCollection<FilaImportacionDto> Filas
        {
            get => _filas;
            set { _filas = value; NotifyOfPropertyChange(() => Filas); }
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

        // Ajuste de precio por porcentaje (%): +10 = +10%, -15 = -15%
        private decimal _ajustePorcentaje = 0;
        public decimal AjustePorcentaje
        {
            get => _ajustePorcentaje;
            set { _ajustePorcentaje = value; NotifyOfPropertyChange(() => AjustePorcentaje); }
        }

        // Margen sobre costo (1.30 = 30% de margen)
        private decimal _margen = 0;
        public decimal Margen
        {
            get => _margen;
            set { _margen = value; NotifyOfPropertyChange(() => Margen); }
        }

        // ── Error visible en dropzone ─────────────────────────────────────────
        private bool _tieneError;
        public bool TieneError
        {
            get => _tieneError;
            set { _tieneError = value; NotifyOfPropertyChange(() => TieneError); }
        }

        private string _mensajeError = string.Empty;
        public string MensajeError
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
                Filas         = new ObservableCollection<FilaImportacionDto>(filas);

                Estado = EstadoImportacion.Previsualizando;
            }
            catch (Exception ex)
            {
                TieneError   = true;
                MensajeError = $"Error al leer el archivo: {ex.Message}";
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
                var filasUsadas = hoja.RangeUsed()?.RowsUsed().ToList() ?? new List<IXLRangeRow>();

                if (filasUsadas.Count < 2)
                    throw new Exception("El archivo debe tener al menos una fila de encabezado y una fila de datos.");

                var encabezado = filasUsadas[0];
                var mapaColumnas = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                int colIdx = 1;
                foreach (var celda in encabezado.CellsUsed())
                {
                    var nombreCol = celda.GetString().Trim().ToLower();
                    mapaColumnas[nombreCol] = colIdx;
                    colIdx++;
                }

                if (!mapaColumnas.ContainsKey("nombre"))
                    throw new Exception("Falta columna 'Nombre'. Asegurate de usar la plantilla oficial.");

                for (int i = 1; i < filasUsadas.Count; i++)
                {
                    var filaExcel = filasUsadas[i];
                    int numeroFila = i + 1;

                    var nombre       = ObtenerValorCelda(filaExcel, mapaColumnas, "nombre");
                    var codigoBarra  = ObtenerValorCelda(filaExcel, mapaColumnas, "codigobarra");
                    var pVentaStr    = ObtenerValorCelda(filaExcel, mapaColumnas, "precioventa");
                    var pCostoStr    = ObtenerValorCelda(filaExcel, mapaColumnas, "preciocosto");
                    var stockStr     = ObtenerValorCelda(filaExcel, mapaColumnas, "stockactual");
                    var stockMinStr  = ObtenerValorCelda(filaExcel, mapaColumnas, "stockminimo");
                    var categoria    = ObtenerValorCelda(filaExcel, mapaColumnas, "categoria");
                    var unidadMedida = ObtenerValorCelda(filaExcel, mapaColumnas, "unidadmedida");

                    var errores = new List<string>();

                    if (string.IsNullOrWhiteSpace(nombre))
                        errores.Add("Nombre vacío");

                    decimal.TryParse(pVentaStr, out decimal precioVenta);
                    decimal.TryParse(pCostoStr, out decimal precioCosto);
                    int.TryParse(stockStr, out int stock);
                    int.TryParse(stockMinStr, out int stockMinimo);

                    if (!string.IsNullOrWhiteSpace(pVentaStr) && precioVenta <= 0)
                        errores.Add("Precio de venta inválido");

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
                        Fila       = numeroFila,
                        Nombre     = nombre,
                        CodigoBarra = codigoBarra,
                        PrecioVenta = precioVenta,
                        PrecioCosto = precioCosto,
                        Stock       = stock,
                        StockMinimo = stockMinimo,
                        Categoria   = categoria,
                        UnidadMedida = string.IsNullOrWhiteSpace(unidadMedida) ? "Unidad" : unidadMedida,
                        EsValida    = errores.Count == 0,
                        ErrorDescripcion = string.Join("; ", errores),
                        IdCategoria = idCategoria
                    });
                }

                return filas;
            });
        }

        private static string ObtenerValorCelda(IXLRangeRow fila, Dictionary<string, int> mapa, string nombreCol)
        {
            if (!mapa.TryGetValue(nombreCol, out int colIdx)) return string.Empty;
            try
            {
                var celda = fila.Cell(colIdx);
                return celda?.GetString()?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
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
                // Calcular factores de ajuste
                decimal factorPorcentaje = 1 + (AjustePorcentaje / 100m);  // +10% -> 1.10
                decimal factorMargen = Margen > 0 ? 1 / (1 - Margen / 100m) : 0;  // 30% margen -> 1/0.7

                // Convertir filas a DTOs (aplicando ajustes)
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

                    // Aplicar ajustes de precio
                    decimal precioVenta = fila.PrecioVenta;
                    decimal precioCosto = fila.PrecioCosto;

                    // Prioridad: Margen > Porcentaje > ninguno
                    if (Margen != 0 && precioCosto > 0)
                    {
                        // Precio = costo / (1 - margen/100)
                        precioVenta = precioCosto * factorMargen;
                    }
                    else if (AjustePorcentaje != 0)
                    {
                        precioVenta = precioVenta * factorPorcentaje;
                    }

                    dtos.Add(new ProductoImportarDto
                    {
                        Nombre = fila.Nombre,
                        CodigoBarra = fila.CodigoBarra ?? string.Empty,
                        Categoria = fila.Categoria,
                        PrecioVentaActual = Math.Round(precioVenta, 2),
                        PrecioCostoActual = precioCosto,
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

                // Importar en bulk (más rápido)
                var (nuevos, actualizados, omitidos) = await _productoServicio.ImportarMasivoAsync(
                    dtos,
                    ActualizarExistentes,
                    progress);

                Importados = nuevos;
                Actualizados = actualizados;
                Omitidos = omitidos + FilasConError;

                Estado = Omitidos > 0
                    ? EstadoImportacion.ConErrores
                    : EstadoImportacion.Completado;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error durante la importacion bulk");
                TieneError = true;
                MensajeError = $"Error durante la importacion: {ex.Message}";
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
            Filas         = new ObservableCollection<FilaImportacionDto>();
            FilasTotales  = 0;
            FilasValidas  = 0;
            FilasConError = 0;
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

        // ── Descargar plantilla ───────────────────────────────────────────────
        public async Task DescargarPlantilla()
        {
            // Usar carpeta temp del sistema
            var tempPath = System.IO.Path.GetTempPath();
            var fileName = $"PlantillaImportacionProductos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var filePath = System.IO.Path.Combine(tempPath, fileName);

            await Task.Run(() =>
            {
                using var workbook = new XLWorkbook();
                var hoja = workbook.Worksheets.Add("Productos");

                var encabezados = new[] { "Nombre", "CodigoBarra", "PrecioVenta", "PrecioCosto", "StockActual", "StockMinimo", "Categoria", "UnidadMedida" };
                for (int i = 0; i < encabezados.Length; i++)
                {
                    var celda = hoja.Cell(1, i + 1);
                    celda.Value = encabezados[i];
                    celda.Style.Font.Bold = true;
                    celda.Style.Fill.BackgroundColor = XLColor.FromHtml("#2D5A8A");
                    celda.Style.Font.FontColor = XLColor.White;
                }

                var ejemplos = new (string Nombre, string CodBarra, decimal PVenta, decimal PCosto, int Stock, int StockMin, string Categoria, string Unidad)[]
                {
                    ("Auriculares Pro X", "7890001", 24000, 15000, 8,  3, "Electronica", "Unidad"),
                    ("Mouse Inalambrico", "7890002", 12500, 8000,  3,  2, "Perifericos", "Unidad"),
                    ("Teclado Mecanico",  "7890003", 34000, 22000, 15, 5, "Perifericos", "Unidad"),
                    ("Webcam HD 1080p",   "7890004", 18000, 11000, 12, 4, "Perifericos", "Unidad"),
                    ("Cable HDMI 2m",     "7890005", 3500,  1800,  25, 5, "Accesorios",  "Unidad"),
                };

                for (int f = 0; f < ejemplos.Length; f++)
                {
                    hoja.Cell(f + 2, 1).Value = ejemplos[f].Nombre;
                    hoja.Cell(f + 2, 2).Value = ejemplos[f].CodBarra;
                    hoja.Cell(f + 2, 3).Value = ejemplos[f].PVenta;
                    hoja.Cell(f + 2, 4).Value = ejemplos[f].PCosto;
                    hoja.Cell(f + 2, 5).Value = ejemplos[f].Stock;
                    hoja.Cell(f + 2, 6).Value = ejemplos[f].StockMin;
                    hoja.Cell(f + 2, 7).Value = ejemplos[f].Categoria;
                    hoja.Cell(f + 2, 8).Value = ejemplos[f].Unidad;
                }

                hoja.Column(1).Width = 25;
                hoja.Column(2).Width = 15;
                hoja.Column(3).Width = 15;
                hoja.Column(4).Width = 15;
                hoja.Column(5).Width = 12;
                hoja.Column(6).Width = 12;
                hoja.Column(7).Width = 15;
                hoja.Column(8).Width = 15;

                workbook.SaveAs(filePath);
            });

            // Guardar ruta para re-importar
            RutaPlantillaTemporal = filePath;
            ArchivoRuta = filePath;
            ArchivoNombre = System.IO.Path.GetFileName(filePath);

            // Abrir automáticamente
            try
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al abrir Excel");
            }

            // Previsualizar
            await PrevisualizarArchivo();
        }

        /// <summary>
        /// Re-importa la última plantilla descargada sin pedir archivo.
        /// </summary>
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

    // ── DTO fila con edición inline y INotifyPropertyChanged ──────────────────
    public class FilaImportacionDto : System.ComponentModel.INotifyPropertyChanged
    {
        private string  _nombre = string.Empty;
        private string  _codigoBarra = string.Empty;
        private decimal _precioVenta;
        private decimal _precioCosto;
        private int     _stock;
        private int     _stockMinimo;
        private string  _categoria = string.Empty;
        private string  _unidadMedida = string.Empty;
        private bool    _esValida;
        private string  _errorDescripcion = string.Empty;

        public int     Fila          { get; set; }
        public int?    IdCategoria   { get; set; }
        public bool    EsNuevo       { get; set; }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); Validar(); }
        }

        public string CodigoBarra
        {
            get => _codigoBarra;
            set { _codigoBarra = value; OnPropertyChanged(); }
        }

        public decimal PrecioVenta
        {
            get => _precioVenta;
            set { _precioVenta = value; OnPropertyChanged(); OnPropertyChanged(nameof(Margen)); Validar(); }
        }

        public decimal PrecioCosto
        {
            get => _precioCosto;
            set { _precioCosto = value; OnPropertyChanged(); OnPropertyChanged(nameof(Margen)); }
        }

        public int Stock
        {
            get => _stock;
            set { _stock = value; OnPropertyChanged(); }
        }

        public int StockMinimo
        {
            get => _stockMinimo;
            set { _stockMinimo = value; OnPropertyChanged(); }
        }

        public string Categoria
        {
            get => _categoria;
            set { _categoria = value; OnPropertyChanged(); }
        }

        public string UnidadMedida
        {
            get => _unidadMedida;
            set { _unidadMedida = value; OnPropertyChanged(); }
        }

        public bool EsValida
        {
            get => _esValida;
            set { _esValida = value; OnPropertyChanged(); OnPropertyChanged(nameof(EstadoTexto)); }
        }

        public string ErrorDescripcion
        {
            get => _errorDescripcion;
            set { _errorDescripcion = value; OnPropertyChanged(); }
        }

        public string EstadoTexto => EsValida ? (EsNuevo ? "Nuevo" : "Actualizar") : "Error";

        public decimal Margen => PrecioVenta > 0 && PrecioCosto > 0
            ? Math.Round((PrecioVenta - PrecioCosto) / PrecioVenta * 100, 1)
            : 0;

        private void Validar()
        {
            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("Nombre vacío");
            if (PrecioVenta <= 0)
                errores.Add("Precio de venta inválido");
            EsValida = errores.Count == 0;
            ErrorDescripcion = string.Join("; ", errores);
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
    }
}