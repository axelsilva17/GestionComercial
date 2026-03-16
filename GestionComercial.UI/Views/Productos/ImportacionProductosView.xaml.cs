using GestionComercial.UI.ViewModels.Productos;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Productos
{
    public partial class ImportacionProductosView : UserControl
    {
        private ImportacionProductosViewModel VM => DataContext as ImportacionProductosViewModel;

        public ImportacionProductosView()
        {
            InitializeComponent();
        }

        // ── Dropzone ──────────────────────────────────────────────────────────
        private async void DropZone_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => await VM?.SeleccionarArchivo();

        private void DropZone_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
            e.Handled = true;
        }

        private async void DropZone_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var archivos = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (archivos?.Length > 0 && VM != null)
            {
                var archivo = archivos[0];
                var ext = System.IO.Path.GetExtension(archivo).ToLower();
                if (ext == ".xlsx" || ext == ".xls")
                {
                    VM.ArchivoRuta   = archivo;
                    VM.ArchivoNombre = System.IO.Path.GetFileName(archivo);
                    await VM.PrevisualizarArchivo();
                }
                else
                {
                    MessageBox.Show("Solo se aceptan archivos .xlsx o .xls",
                        "Formato inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // ── Botón principal: Ejecutar importación ─────────────────────────────
        private void EjecutarImportacion_Click(object sender, RoutedEventArgs e)
            => VM?.EjecutarImportacion();

        // ── Cancelar (vuelve al estado anterior) ──────────────────────────────
        private void Cancelar_Click(object sender, RoutedEventArgs e)
            => VM?.Cancelar();

        // ── Cambiar archivo (vuelve al paso 1) ───────────────────────────────
        private void Reiniciar_Click(object sender, RoutedEventArgs e)
            => VM?.Reiniciar();

        // ── Resultado: importar otro archivo ─────────────────────────────────
        private void ReiniciarArchivo_Click(object sender, RoutedEventArgs e)
            => VM?.Reiniciar();

        // ── Resultado: ir a listado de productos ──────────────────────────────
        private void Finalizar_Click(object sender, RoutedEventArgs e)
            => VM?.Finalizar();

        // ── Descargar plantilla Excel ─────────────────────────────────────────
        private async void DescargarPlantilla_Click(object sender, RoutedEventArgs e)
            => await VM?.DescargarPlantilla();
    }
}
