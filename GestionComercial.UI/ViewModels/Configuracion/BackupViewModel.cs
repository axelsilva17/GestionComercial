using Caliburn.Micro;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Dominio.DTOs.Infraestructura;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class BackupViewModel : NavigableViewModel
    {
        private readonly IBackupService _backupService;

        private ObservableCollection<BackupInfo> _backups = new();
        public ObservableCollection<BackupInfo> Backups
        {
            get => _backups;
            set { _backups = value; NotifyOfPropertyChange(() => Backups); }
        }

        private bool _backupHabilitado;
        public bool BackupHabilitado
        {
            get => _backupHabilitado;
            set
            {
                _backupHabilitado = value;
                NotifyOfPropertyChange(() => BackupHabilitado);
            }
        }

        private int _maxBackups = 7;
        public int MaxBackups
        {
            get => _maxBackups;
            set
            {
                _maxBackups = value;
                NotifyOfPropertyChange(() => MaxBackups);
            }
        }

        private string? _carpetaDestino;
        public string? CarpetaDestino
        {
            get => _carpetaDestino;
            set
            {
                _carpetaDestino = value;
                NotifyOfPropertyChange(() => CarpetaDestino);
            }
        }

        private BackupInfo? _backupSeleccionado;
        public BackupInfo? BackupSeleccionado
        {
            get => _backupSeleccionado;
            set
            {
                _backupSeleccionado = value;
                NotifyOfPropertyChange(() => BackupSeleccionado);
            }
        }

        public BackupViewModel(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var config = await _backupService.ObtenerConfiguracionAsync();
                BackupHabilitado = config.Enabled;
                MaxBackups = config.CantidadMaximaBackups;
                CarpetaDestino = config.CarpetaDestino;

                await CargarBackupsAsync();
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task RealizarBackup()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var resultado = await _backupService.RealizarBackupAsync("manual");
                if (resultado.Success)
                {
                    // Recargar lista
                    await CargarBackupsAsync();
                }
                else
                {
                    MostrarError(resultado.ErrorMessage ?? "Error al realizar backup");
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task EliminarBackup()
        {
            if (BackupSeleccionado == null)
                return;

            IsLoading = true;
            LimpiarError();
            try
            {
                var ok = await _backupService.EliminarBackupAsync(BackupSeleccionado.FullPath);
                if (ok)
                {
                    Backups.Remove(BackupSeleccionado);
                    BackupSeleccionado = null;
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task GuardarConfiguracion()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                await _backupService.GuardarConfiguracionAsync(new BackupAutoConfig
                {
                    Enabled = BackupHabilitado,
                    CantidadMaximaBackups = MaxBackups,
                    CarpetaDestino = CarpetaDestino
                });
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        private async Task CargarBackupsAsync()
        {
            var backups = await _backupService.ObtenerBackupsAsync();
            Backups = new ObservableCollection<BackupInfo>(backups);
        }

        public static string FormatearTamanio(long bytes)
        {
            string[] sufijos = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double tam = bytes;

            while (tam >= 1024 && i < sufijos.Length - 1)
            {
                tam /= 1024;
                i++;
            }

            return $"{tam:F1} {sufijos[i]}";
        }
    }
}
