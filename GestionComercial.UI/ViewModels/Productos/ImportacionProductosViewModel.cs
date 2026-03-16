using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ImportacionProductosViewModel : NavigableViewModel
    {
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
            }
        }

        public bool MostrarDropzone  => Estado == EstadoImportacion.Inicial;
        public bool MostrarPreview   => Estado == EstadoImportacion.Previsualizando
                                     || Estado == EstadoImportacion.Importando;
        public bool MostrarResultado => Estado == EstadoImportacion.Completado
                                     || Estado == EstadoImportacion.ConErrores;

        // ── PuedeImportar: SIN chequear IsLoading para evitar que quede bloqueado ──
        public bool PuedeImportar => Estado == EstadoImportacion.Previsualizando && FilasValidas > 0;

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
                await Task.Delay(300);

                // TODO: reemplazar con lector real (ClosedXML / NPOI)
                var filas = GenerarPreviewMock();

                FilasTotales  = filas.Count;
                FilasValidas  = filas.Count(f => f.EsValida);
                FilasConError = filas.Count(f => !f.EsValida);
                Filas         = new ObservableCollection<FilaImportacionDto>(filas);

                // IMPORTANTE: cambiar Estado DESPUÉS de setear FilasValidas
                // para que PuedeImportar ya sea true cuando se notifica
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
                // IsLoading = false ANTES de notificar PuedeImportar
                // para que el botón quede habilitado
                IsLoading = false;
                NotifyOfPropertyChange(() => PuedeImportar);
            }
        }

        // ── Ejecutar importación ──────────────────────────────────────────────
        // async void para que Caliburn.Micro pueda conectarlo por convención de nombre
        public async void EjecutarImportacion()
        {
            if (!PuedeImportar) return;

            Estado    = EstadoImportacion.Importando;
            IsLoading = true;

            var filasAImportar = Filas.Where(f => f.EsValida).ToList();
            int total          = filasAImportar.Count;
            Importados         = 0;
            Omitidos           = 0;
            Actualizados       = 0;

            try
            {
                for (int i = 0; i < filasAImportar.Count; i++)
                {
                    var fila = filasAImportar[i];

                    Progreso      = (int)((i + 1) / (double)total * 100);
                    TextoProgreso = $"Procesando {i + 1} de {total}...";

                    await Task.Delay(30); // TODO: await _productoServicio.CrearOActualizar(fila)

                    if (fila.EsNuevo) Importados++;
                    else              Actualizados++;
                }

                Omitidos = FilasConError;
                Estado   = Omitidos > 0
                    ? EstadoImportacion.ConErrores
                    : EstadoImportacion.Completado;
            }
            catch (Exception ex)
            {
                MostrarError($"Error durante la importación: {ex.Message}");
                Estado = EstadoImportacion.ConErrores;
            }
            finally
            {
                IsLoading = false;
            }
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
            LimpiarError();
            Estado = EstadoImportacion.Inicial;
        }

        public void Finalizar() => ImportacionCompletada?.Invoke();

        public void Cancelar()  => Cancelado?.Invoke();

        // ── Descargar plantilla ───────────────────────────────────────────────
        public async Task DescargarPlantilla()
        {
            var dialogo = new Microsoft.Win32.SaveFileDialog
            {
                Title      = "Guardar plantilla",
                FileName   = "PlantillaImportacionProductos",
                DefaultExt = ".xlsx",
                Filter     = "Excel (*.xlsx)|*.xlsx"
            };

            if (dialogo.ShowDialog() != true) return;

            await Task.Delay(100);

            System.Windows.MessageBox.Show(
                "Columnas requeridas:\n\n" +
                "• Nombre (obligatorio)\n• CodigoBarra\n• PrecioVenta (obligatorio)\n" +
                "• PrecioCosto\n• StockActual\n• StockMinimo\n• Categoria\n• UnidadMedida\n\n" +
                "La primera fila debe ser el encabezado.",
                "Plantilla de importación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        // ── Mock preview ──────────────────────────────────────────────────────
        private static System.Collections.Generic.List<FilaImportacionDto> GenerarPreviewMock()
        {
            return new()
            {
                new() { Fila=2, Nombre="Auriculares Pro X",  CodigoBarra="7890001", PrecioVenta=24000, PrecioCosto=15000, Stock=8,  StockMinimo=3, Categoria="Electrónica", UnidadMedida="Unidad", EsValida=true,  EsNuevo=true  },
                new() { Fila=3, Nombre="Mouse Inalámbrico",  CodigoBarra="7890002", PrecioVenta=12500, PrecioCosto=8000,  Stock=3,  StockMinimo=2, Categoria="Periféricos",  UnidadMedida="Unidad", EsValida=true,  EsNuevo=false },
                new() { Fila=4, Nombre="Teclado Mecánico",   CodigoBarra="7890003", PrecioVenta=34000, PrecioCosto=22000, Stock=15, StockMinimo=5, Categoria="Periféricos",  UnidadMedida="Unidad", EsValida=true,  EsNuevo=true  },
                new() { Fila=5, Nombre="",                   CodigoBarra="7890004", PrecioVenta=0,     PrecioCosto=0,     Stock=0,  StockMinimo=0, Categoria="",             UnidadMedida="",       EsValida=false, EsNuevo=false, ErrorDescripcion="Nombre y precio de venta obligatorios" },
                new() { Fila=6, Nombre="Webcam HD 1080p",    CodigoBarra="7890005", PrecioVenta=18000, PrecioCosto=11000, Stock=12, StockMinimo=4, Categoria="Periféricos",  UnidadMedida="Unidad", EsValida=true,  EsNuevo=true  },
                new() { Fila=7, Nombre="Cable HDMI 2m",      CodigoBarra="7890006", PrecioVenta=3500,  PrecioCosto=1800,  Stock=25, StockMinimo=5, Categoria="Accesorios",   UnidadMedida="Unidad", EsValida=true,  EsNuevo=true  },
                new() { Fila=8, Nombre="Hub USB",            CodigoBarra="ABC",     PrecioVenta=8900,  PrecioCosto=5500,  Stock=7,  StockMinimo=2, Categoria="Accesorios",   UnidadMedida="Unidad", EsValida=false, EsNuevo=false, ErrorDescripcion="Código de barra inválido (debe ser numérico)" },
            };
        }
    }

    // ── DTO fila ──────────────────────────────────────────────────────────────
    public class FilaImportacionDto
    {
        public int     Fila             { get; set; }
        public string  Nombre           { get; set; } = string.Empty;
        public string  CodigoBarra      { get; set; } = string.Empty;
        public decimal PrecioVenta      { get; set; }
        public decimal PrecioCosto      { get; set; }
        public int     Stock            { get; set; }
        public int     StockMinimo      { get; set; }
        public string  Categoria        { get; set; } = string.Empty;
        public string  UnidadMedida     { get; set; } = string.Empty;
        public bool    EsValida         { get; set; }
        public bool    EsNuevo          { get; set; }
        public string  ErrorDescripcion { get; set; } = string.Empty;

        public string  EstadoTexto => EsValida ? (EsNuevo ? "Nuevo" : "Actualizar") : "Error";
        public decimal Margen      => PrecioVenta > 0 && PrecioCosto > 0
            ? Math.Round((PrecioVenta - PrecioCosto) / PrecioVenta * 100, 1)
            : 0;
    }
}
