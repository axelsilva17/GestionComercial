using Caliburn.Micro;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Dominio.DTOs.Infraestructura;
using GestionComercial.Dominio.Entidades.Configuracion;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
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

        private FrecuenciaBackupEnum _frecuenciaSeleccionada = FrecuenciaBackupEnum.Desactivado;
        public FrecuenciaBackupEnum FrecuenciaSeleccionada
        {
            get => _frecuenciaSeleccionada;
            set
            {
                _frecuenciaSeleccionada = value;
                NotifyOfPropertyChange(() => FrecuenciaSeleccionada);
                NotifyOfPropertyChange(() => DiaSemanaHabilitado);
            }
        }

        private DayOfWeek? _diaSemanaSeleccionado;
        public DayOfWeek? DiaSemanaSeleccionado
        {
            get => _diaSemanaSeleccionado;
            set { _diaSemanaSeleccionado = value; NotifyOfPropertyChange(() => DiaSemanaSeleccionado); }
        }

        private TimeOnly? _horaProgramada;
        public TimeOnly? HoraProgramada
        {
            get => _horaProgramada;
            set
            {
                _horaProgramada = value;
                NotifyOfPropertyChange(() => HoraProgramada);
                NotifyOfPropertyChange(() => HoraSeleccionada);
                NotifyOfPropertyChange(() => MinutoSeleccionado);
            }
        }

        public IEnumerable<int> Horas => Enumerable.Range(0, 24);
        public IEnumerable<int> Minutos => new[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

        public int HoraSeleccionada
        {
            get => HoraProgramada?.Hour ?? 0;
            set
            {
                var min = HoraProgramada?.Minute ?? 0;
                HoraProgramada = new TimeOnly(value, min);
                NotifyOfPropertyChange(() => HoraSeleccionada);
            }
        }

        public int MinutoSeleccionado
        {
            get => HoraProgramada?.Minute ?? 0;
            set
            {
                var h = HoraProgramada?.Hour ?? 0;
                HoraProgramada = new TimeOnly(h, value);
                NotifyOfPropertyChange(() => MinutoSeleccionado);
            }
        }

        private int _maxBackups = 10;
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

        private DateTime? _ultimoBackup;
        public DateTime? UltimoBackup
        {
            get => _ultimoBackup;
            set
            {
                _ultimoBackup = value;
                NotifyOfPropertyChange(() => UltimoBackup);
                NotifyOfPropertyChange(() => UltimoBackupFormato);
            }
        }

        public string UltimoBackupFormato => UltimoBackup?.ToString("dd/MM/yyyy HH:mm") ?? "Nunca";

        public bool DiaSemanaHabilitado => FrecuenciaSeleccionada == FrecuenciaBackupEnum.Semanal;

        public IEnumerable<FrecuenciaBackupEnum> FrecuenciasDisponibles =>
            Enum.GetValues<FrecuenciaBackupEnum>();

        public IEnumerable<DiaSemanaItem> DiasSemana => new[]
        {
            new DiaSemanaItem(DayOfWeek.Monday,    "Lunes"),
            new DiaSemanaItem(DayOfWeek.Tuesday,   "Martes"),
            new DiaSemanaItem(DayOfWeek.Wednesday, "Miércoles"),
            new DiaSemanaItem(DayOfWeek.Thursday,  "Jueves"),
            new DiaSemanaItem(DayOfWeek.Friday,    "Viernes"),
            new DiaSemanaItem(DayOfWeek.Saturday,  "Sábado"),
            new DiaSemanaItem(DayOfWeek.Sunday,    "Domingo"),
        };

        private DiaSemanaItem? _diaSemanaSeleccionadoItem;
        public DiaSemanaItem? DiaSemanaSeleccionadoItem
        {
            get => _diaSemanaSeleccionadoItem;
            set
            {
                _diaSemanaSeleccionadoItem = value;
                DiaSemanaSeleccionado = value?.DayOfWeek;
                NotifyOfPropertyChange(() => DiaSemanaSeleccionadoItem);
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
                FrecuenciaSeleccionada = config.Frecuencia;
                DiaSemanaSeleccionado = config.DiaSemana;
                DiaSemanaSeleccionadoItem = DiasSemana.FirstOrDefault(d => d.DayOfWeek == config.DiaSemana);
                HoraProgramada = config.HoraProgramada;
                MaxBackups = config.MaxBackups;
                CarpetaDestino = config.CarpetaDestino;
                UltimoBackup = config.UltimoBackup;

                await CargarBackupsAsync();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task RealizarBackup()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var resultado = await _backupService.GenerarBackupAsync("manual");
                if (resultado.Success)
                {
                    UltimoBackup = DateTime.Now;
                    await CargarBackupsAsync();
                }
                else
                {
                    MostrarError(resultado.ErrorMessage ?? "Error al realizar backup");
                }
            }
            catch (Exception ex) { MostrarError(ex.Message); }
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
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task GuardarConfiguracion()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var config = new BackupConfig
                {
                    Frecuencia = FrecuenciaSeleccionada,
                    DiaSemana = DiaSemanaSeleccionado,
                    HoraProgramada = HoraProgramada,
                    MaxBackups = MaxBackups,
                    CarpetaDestino = CarpetaDestino
                };
                await _backupService.GuardarConfiguracionAsync(config);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
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

    public class DiaSemanaItem
    {
        public DayOfWeek DayOfWeek { get; }
        public string Nombre { get; }

        public DiaSemanaItem(DayOfWeek dayOfWeek, string nombre)
        {
            DayOfWeek = dayOfWeek;
            Nombre = nombre;
        }

        public override string ToString() => Nombre;
    }
}
