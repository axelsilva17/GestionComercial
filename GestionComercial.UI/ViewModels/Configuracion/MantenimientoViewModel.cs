using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class MantenimientoViewModel : Screen
    {
        public string Titulo    => "Mantenimiento";
        public string Subtitulo => "Herramientas de diagnóstico y mantenimiento";

        private readonly IDiagnosticoServicio _diagnostico;
        private readonly IActualizacionServicio _actualizacion;
        private readonly IReporteMantenimientoServicio _reporte;
        private readonly IBackupService _backupService;
        private readonly SesionServicio _sesion;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; NotifyOfPropertyChange(() => IsLoading); }
        }

        private string _resultadoAuditoria = string.Empty;
        public string ResultadoAuditoria
        {
            get => _resultadoAuditoria;
            set { _resultadoAuditoria = value; NotifyOfPropertyChange(() => ResultadoAuditoria); }
        }

        private bool _integridadOk;
        public bool IntegridadOk
        {
            get => _integridadOk;
            set { _integridadOk = value; NotifyOfPropertyChange(() => IntegridadOk); }
        }

        private StatsDbDto? _stats;
        public StatsDbDto? Stats
        {
            get => _stats;
            set { _stats = value; NotifyOfPropertyChange(() => Stats); }
        }

        private ObservableCollection<LogItemDto> _warnings = new();
        public ObservableCollection<LogItemDto> Warnings
        {
            get => _warnings;
            set { _warnings = value; NotifyOfPropertyChange(() => Warnings); }
        }

        private string _versionActual = string.Empty;
        public string VersionActual
        {
            get => _versionActual;
            set { _versionActual = value; NotifyOfPropertyChange(() => VersionActual); }
        }

        private string? _rutaActualizacion;
        public string? RutaActualizacion
        {
            get => _rutaActualizacion;
            set { _rutaActualizacion = value; NotifyOfPropertyChange(() => RutaActualizacion); }
        }

        private string _mensajeEstado = string.Empty;
        public string MensajeEstado
        {
            get => _mensajeEstado;
            set { _mensajeEstado = value; NotifyOfPropertyChange(() => MensajeEstado); }
        }

        private string _reporteGenerado = string.Empty;
        public string ReporteGenerado
        {
            get => _reporteGenerado;
            set { _reporteGenerado = value; NotifyOfPropertyChange(() => ReporteGenerado); }
        }

        private DiagnosticoResultDto? _ultimoDiagnostico;

        public MantenimientoViewModel(
            IDiagnosticoServicio diagnostico,
            IActualizacionServicio actualizacion,
            IReporteMantenimientoServicio reporte,
            IBackupService backupService,
            SesionServicio sesion)
        {
            _diagnostico = diagnostico;
            _actualizacion = actualizacion;
            _reporte = reporte;
            _backupService = backupService;
            _sesion = sesion;
            VersionActual = _actualizacion.ObtenerVersionActual();
        }

        public async Task EjecutarAuditoriaAsync()
        {
            IsLoading = true;
            MensajeEstado = "Ejecutando auditoría completa...";
            try
            {
                var resultado = await _diagnostico.EjecutarAuditoriaCompletaAsync(_sesion.IdEmpresa);
                _ultimoDiagnostico = resultado;
                IntegridadOk = resultado.IntegridadOk;
                Stats = resultado.Stats;
                Warnings = new ObservableCollection<LogItemDto>(resultado.Warnings);

                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Integridad: {(resultado.IntegridadOk ? "OK" : "ERROR")}");
                if (resultado.Stats != null)
                {
                    sb.AppendLine($"Ventas: {resultado.Stats.TotalVentas:N0}");
                    sb.AppendLine($"Productos: {resultado.Stats.TotalProductos:N0}");
                    sb.AppendLine($"Clientes: {resultado.Stats.TotalClientes:N0}");
                    sb.AppendLine($"Tamaño DB: {resultado.Stats.TamanoFormateado}");
                }
                sb.AppendLine($"Advertencias: {resultado.Warnings.Count}");
                ResultadoAuditoria = sb.ToString();
                MensajeEstado = "Auditoría completada.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        public async Task OptimizarDbAsync()
        {
            IsLoading = true;
            MensajeEstado = "Optimizando base de datos...";
            try
            {
                await _diagnostico.OptimizarAsync();
                MensajeEstado = "Base de datos optimizada correctamente.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al optimizar: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        public async Task ReindexarAsync()
        {
            IsLoading = true;
            MensajeEstado = "Reindexando base de datos...";
            try
            {
                await _diagnostico.ReindexarAsync();
                MensajeEstado = "Base de datos reindexada correctamente.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al reindexar: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        public async Task VerificarBackupsAsync()
        {
            IsLoading = true;
            MensajeEstado = "Verificando backups...";
            try
            {
                var config = await _backupService.ObtenerConfiguracionAsync();
                var ultimo = config.UltimoBackup?.ToString("dd/MM/yyyy HH:mm") ?? "Nunca";
                MensajeEstado = $"Último backup: {ultimo}. Frecuencia: {config.Frecuencia}.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al verificar backups: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        public async Task GenerarReporteAsync()
        {
            if (_ultimoDiagnostico == null)
            {
                MensajeEstado = "Primero ejecutá la auditoría.";
                return;
            }

            IsLoading = true;
            MensajeEstado = "Generando reporte...";
            try
            {
                GestionComercial.Dominio.Entidades.Configuracion.BackupConfig? backupConfig = null;
                try
                {
                    backupConfig = await _backupService.ObtenerConfiguracionAsync();
                }
                catch { }

                var acciones = new List<string>
                {
                    "Auditoría de integridad",
                    "Optimización de base de datos",
                    "Verificación de backups",
                    "Generación de reporte"
                };

                ReporteGenerado = await _reporte.GenerarReporteAsync(_ultimoDiagnostico, "Empresa", _sesion.IdEmpresa, backupConfig, acciones);
                MensajeEstado = "Reporte generado correctamente.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al generar reporte: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        public void ExportarPdf()
        {
            if (_ultimoDiagnostico == null)
            {
                MensajeEstado = "Primero ejecutá la auditoría.";
                return;
            }

            try
            {
                GestionComercial.Dominio.Entidades.Configuracion.BackupConfig? backupConfig = null;
                try
                {
                    backupConfig = _backupService.ObtenerConfiguracionAsync().GetAwaiter().GetResult();
                }
                catch { }

                var acciones = new List<string>
                {
                    "Auditoría de integridad",
                    "Optimización de base de datos",
                    "Verificación de backups",
                    "Generación de reporte"
                };

                var pdfBytes = _reporte.GenerarPdf(_ultimoDiagnostico, "Empresa", _sesion.IdEmpresa, backupConfig, acciones);

                var dialog = new SaveFileDialog
                {
                    Filter = "Archivos PDF (*.pdf)|*.pdf",
                    DefaultExt = ".pdf",
                    FileName = $"Mantenimiento_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(dialog.FileName, pdfBytes);
                    MensajeEstado = $"PDF exportado: {dialog.FileName}";
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al exportar PDF: {ex.Message}";
            }
        }

        public void BuscarActualizacion()
        {
            var ruta = RutaActualizacion;
            if (string.IsNullOrWhiteSpace(ruta))
            {
                MensajeEstado = "Ingresá una ruta para buscar actualizaciones.";
                return;
            }

            var archivo = _actualizacion.BuscarActualizacionEnRuta(ruta);
            if (archivo != null)
            {
                var cambios = _actualizacion.ObtenerCambios(archivo);
                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Actualización encontrada: {Path.GetFileName(archivo)}");
                foreach (var c in cambios)
                    sb.AppendLine($"  - {c}");
                MensajeEstado = sb.ToString();
            }
            else
            {
                MensajeEstado = "No se encontraron actualizaciones en la ruta indicada.";
            }
        }

        public async Task LimpiarLogsAsync()
        {
            IsLoading = true;
            MensajeEstado = "Limpiando logs antiguos...";
            try
            {
                await _diagnostico.LimpiarAsync();
                MensajeEstado = "Logs antiguos eliminados correctamente.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al limpiar: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        public async Task AplicarActualizacionAsync()
        {
            if (string.IsNullOrWhiteSpace(RutaActualizacion))
            {
                MensajeEstado = "Ingresá una ruta válida.";
                return;
            }

            IsLoading = true;
            MensajeEstado = "Aplicando actualización...";
            try
            {
                var archivo = _actualizacion.BuscarActualizacionEnRuta(RutaActualizacion);
                if (archivo == null)
                {
                    MensajeEstado = "No se encontró archivo de actualización.";
                    return;
                }

                var resultado = await _actualizacion.AplicarActualizacionAsync(archivo);
                MensajeEstado = resultado
                    ? "Actualización aplicada correctamente."
                    : "Error al aplicar la actualización.";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error: {ex.Message}";
            }
            finally { IsLoading = false; }
        }
    }
}
