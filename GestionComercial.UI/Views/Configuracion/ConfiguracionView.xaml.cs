using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.UI.ViewModels.Configuracion;
using GestionComercial.Dominio.DTOs.Infraestructura;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GestionComercial.UI.Views.Configuracion
{
    public partial class ConfiguracionView : UserControl
    {
        private ConfiguracionViewModel VM => DataContext as ConfiguracionViewModel;

        private Border _panelActivo;
        private TranslateTransform _transformActivo;

        public ConfiguracionView()
        {
            InitializeComponent();
        }

        // ══ HELPERS ANIMACIÓN ════════════════════════════════════════════════

        private void AbrirPanel(Border panel, TranslateTransform transform)
        {
            if (_panelActivo != null && _panelActivo != panel)
                CerrarPanelInterno(_panelActivo, _transformActivo, onComplete: null);

            _panelActivo = panel;
            _transformActivo = transform;

            panel.Visibility = Visibility.Visible;
            Overlay.IsHitTestVisible = true;

            var slideAnim = new DoubleAnimation
            {
                From = panel.Width,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(280)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            transform.BeginAnimation(TranslateTransform.XProperty, slideAnim);

            var fadeAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };
            Overlay.BeginAnimation(OpacityProperty, fadeAnim);
        }

        private void CerrarPanelInterno(Border panel, TranslateTransform transform, Action onComplete)
        {
            var slideAnim = new DoubleAnimation
            {
                From = 0,
                To = panel.Width,
                Duration = new Duration(TimeSpan.FromMilliseconds(220)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            slideAnim.Completed += (s, e) =>
            {
                panel.Visibility = Visibility.Collapsed;
                onComplete?.Invoke();
            };
            transform.BeginAnimation(TranslateTransform.XProperty, slideAnim);

            var fadeAnim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180))
            };
            fadeAnim.Completed += (s, e) => Overlay.IsHitTestVisible = false;
            Overlay.BeginAnimation(OpacityProperty, fadeAnim);
        }

        private void CerrarPanelActivo()
        {
            if (_panelActivo == null) return;
            CerrarPanelInterno(_panelActivo, _transformActivo, onComplete: () =>
            {
                _panelActivo = null;
                _transformActivo = null;
            });
        }

        // ══ CERRAR ═══════════════════════════════════════════════════════════
        private void CerrarPanel_Click(object sender, RoutedEventArgs e) => CerrarPanelActivo();

        // ══ EMPRESA ══════════════════════════════════════════════════════════
        private void AbrirPanel_Empresa_Click(object sender, RoutedEventArgs e)
        {
            VM?.Empresa.AbrirEdicion();
            AbrirPanel(PanelEmpresa, PanelEmpresaTransform);
        }

        private async void GuardarEmpresa_Click(object sender, RoutedEventArgs e)
        {
            await VM.Empresa.Guardar();
            if (!VM.Empresa.PanelVisible) CerrarPanelActivo();
        }

        // ══ SUCURSALES ═══════════════════════════════════════════════════════
        private void NuevaSucursal_Click(object sender, RoutedEventArgs e)
        {
            VM?.Sucursales.NuevaSucursal();
            AbrirPanel(PanelSucursal, PanelSucursalTransform);
        }

        private void EditarSucursal_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is SucursalDto item)
            {
                VM?.Sucursales.Editar(item);
                AbrirPanel(PanelSucursal, PanelSucursalTransform);
            }
        }

        private async void GuardarSucursal_Click(object sender, RoutedEventArgs e)
        {
            await VM.Sucursales.Guardar();
            if (!VM.Sucursales.PanelVisible) CerrarPanelActivo();
        }

        // ══ USUARIOS ═════════════════════════════════════════════════════════
        private void NuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            VM?.Usuarios.NuevoUsuario();
            PbUsuario.Clear();
            AbrirPanel(PanelUsuario, PanelUsuarioTransform);
        }

        private void EditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is GestionComercial.Aplicacion.DTOs.Usuarios.UsuarioDto item)
            {
                VM?.Usuarios.Editar(item);
                PbUsuario.Clear();
                AbrirPanel(PanelUsuario, PanelUsuarioTransform);
            }
        }

        private async void GuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            await VM.Usuarios.Guardar();
            if (!VM.Usuarios.PanelVisible) CerrarPanelActivo();
        }

        private void PbUsuario_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (VM?.Usuarios != null) VM.Usuarios.EditPassword = PbUsuario.Password;
        }

        // ══ ROLES ════════════════════════════════════════════════════════════
        private void NuevoRol_Click(object sender, RoutedEventArgs e)
        {
            VM?.Roles.NuevoRol();
            AbrirPanel(PanelRol, PanelRolTransform);
        }

        private void EditarRol_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is RolDto item)
            {
                VM?.Roles.Editar(item);
                AbrirPanel(PanelRol, PanelRolTransform);
            }
        }

        private async void GuardarRol_Click(object sender, RoutedEventArgs e)
        {
            await VM.Roles.Guardar();
            if (!VM.Roles.PanelVisible) CerrarPanelActivo();
        }

        private async void EliminarRol_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is RolDto item)
            {
                var confirm = MessageBox.Show(
                    $"¿Eliminar el rol \"{item.Nombre}\"?\nLos usuarios con este rol quedarán sin asignación.",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (confirm == MessageBoxResult.Yes)
                    await VM.Roles.Eliminar(item);
            }
        }

        // ══ MÉTODOS DE PAGO ══════════════════════════════════════════════════
        private void NuevoMetodo_Click(object sender, RoutedEventArgs e)
        {
            VM?.MetodosPago.NuevoMetodo();
            AbrirPanel(PanelMetodo, PanelMetodoTransform);
        }

        private void EditarMetodo_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is MetodoPagoDto item)
            {
                VM?.MetodosPago.Editar(item);
                AbrirPanel(PanelMetodo, PanelMetodoTransform);
            }
        }

        private async void GuardarMetodo_Click(object sender, RoutedEventArgs e)
        {
            await VM.MetodosPago.Guardar();
            if (!VM.MetodosPago.PanelVisible) CerrarPanelActivo();
        }

        private async void EliminarMetodo_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is MetodoPagoDto item)
            {
                var confirm = MessageBox.Show(
                    $"¿Eliminar el método de pago \"{item.Nombre}\"?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (confirm == MessageBoxResult.Yes)
                    await VM.MetodosPago.Eliminar(item);
            }
        }

        // ══ BACKUP ═══════════════════════════════════════════════════════════
        private async void BackupManual_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show(
                "¿Deseás realizar una copia de seguridad de la base de datos?\n\nEsta operación puede tardar unos segundos.",
                "Confirmar Backup",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                await VM.Backup.RealizarBackup();

                if (VM.Backup.ErrorVisible)
                {
                    MessageBox.Show(
                        $"Error al realizar backup:\n{VM.Backup.ErrorMessage}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(
                        "Backup realizado correctamente.",
                        "Éxito",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        // ══ CAMBIAR CARPETA DE BACKUP ═══════════════════════════════════════════
        private void CambiarCarpetaBackup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Seleccionar carpeta de backups",
                Multiselect = false
            };

            // Si hay una carpeta actual, intentar establecerla
            if (!string.IsNullOrEmpty(VM?.Backup?.CarpetaDestino) && 
                Directory.Exists(VM.Backup.CarpetaDestino))
            {
                dialog.InitialDirectory = VM.Backup.CarpetaDestino;
            }

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // El usuario seleccionó una carpeta
                // Actualizamos el ViewModel (solo en memoria, hasta que guarde)
                if (VM?.Backup != null)
                {
                    VM.Backup.CarpetaDestino = dialog.FolderName;
                }
            }
        }

        // ══ GUARDAR CONFIGURACIÓN DE BACKUP ════════════════════════════════════
        private async void GuardarConfigBackup_Click(object sender, RoutedEventArgs e)
        {
            if (VM?.Backup == null) return;

            try
            {
                await VM.Backup.GuardarConfiguracion();

                // Verificar si hubo error
                if (VM.Backup.ErrorVisible)
                {
                    MessageBox.Show(
                        $"Error al guardar la configuración:\n{VM.Backup.ErrorMessage}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(
                        "Configuración guardada correctamente.",
                        "Éxito",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Recargar para reflejar cambios
                    await VM.Backup.CargarAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al guardar la configuración:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void EliminarBackup_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is BackupInfo item)
            {
                var confirm = MessageBox.Show(
                    $"¿Deseás eliminar el backup?\n{item.FileName}",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    VM.Backup.BackupSeleccionado = item;
                    await VM.Backup.EliminarBackup();
                }
            }
        }
    }
}