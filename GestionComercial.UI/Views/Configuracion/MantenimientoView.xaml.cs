using GestionComercial.UI.ViewModels.Configuracion;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Configuracion
{
    public partial class MantenimientoView : UserControl
    {
        private MantenimientoViewModel VM => DataContext as MantenimientoViewModel;

        public MantenimientoView()
        {
            InitializeComponent();
        }

        private async void EjecutarAuditoria_Click(object sender, RoutedEventArgs e)
        {
            if (VM != null) await VM.EjecutarAuditoriaAsync();
        }

        private async void Optimizar_Click(object sender, RoutedEventArgs e)
        {
            if (VM != null) await VM.OptimizarDbAsync();
        }

        private async void Reindexar_Click(object sender, RoutedEventArgs e)
        {
            if (VM != null) await VM.ReindexarAsync();
        }

        private async void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null) return;
            var confirm = MessageBox.Show(
                "¿Eliminar logs de mantenimiento mayores a 90 días?",
                "Confirmar limpieza",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
                await VM.LimpiarLogsAsync();
        }

        private async void VerificarBackup_Click(object sender, RoutedEventArgs e)
        {
            if (VM != null) await VM.VerificarBackupsAsync();
        }

        private void BuscarActualizacion_Click(object sender, RoutedEventArgs e)
        {
            VM?.BuscarActualizacion();
        }

        private async void AplicarActualizacion_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null) return;
            var confirm = MessageBox.Show(
                "¿Aplicar la actualización? Se recomienda hacer un backup primero.",
                "Confirmar actualización",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
                await VM.AplicarActualizacionAsync();
        }

        private async void GenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            if (VM != null) await VM.GenerarReporteAsync();
        }

        private void ExportarReporte_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null || string.IsNullOrEmpty(VM.ReporteGenerado)) return;

            var dialog = new SaveFileDialog
            {
                Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*",
                DefaultExt = ".txt",
                FileName = $"ReporteMantenimiento_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, VM.ReporteGenerado);
                MessageBox.Show(
                    $"Reporte exportado a:\n{dialog.FileName}",
                    "Exportación exitosa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void ExportarPdf_Click(object sender, RoutedEventArgs e)
        {
            VM?.ExportarPdf();
        }
    }
}
