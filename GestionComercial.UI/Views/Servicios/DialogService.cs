using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GestionComercial.UI.Views.Servicios
{
    /// <summary>
    /// Implementación de IDialogService usando DialogWindow estilizado.
    /// </summary>
    public class DialogService : IDialogService
    {
        private readonly DispatcherTimer _debounceTimer;
        private System.Action? _debounceAction;

        public DialogService()
        {
            _debounceTimer = new DispatcherTimer
            {
                Interval = System.TimeSpan.FromMilliseconds(300)
            };
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                _debounceAction?.Invoke();
            };
        }

        public Task MostrarInfoAsync(string titulo, string mensaje)
        {
            return MostrarAsync(titulo, mensaje, DialogTipo.Info);
        }

        public Task MostrarErrorAsync(string titulo, string mensaje)
        {
            return MostrarAsync(titulo, mensaje, DialogTipo.Error);
        }

        public Task<bool> ConfirmarAsync(string titulo, string mensaje)
        {
            return Task.FromResult(MostrarSync(titulo, mensaje, DialogTipo.Confirmar));
        }

        public Task<bool> AdvertirAsync(string titulo, string mensaje)
        {
            return Task.FromResult(MostrarSync(titulo, mensaje, DialogTipo.Advertencia));
        }

        private Task MostrarAsync(string titulo, string mensaje, DialogTipo tipo)
        {
            var result = MostrarSync(titulo, mensaje, tipo);
            return Task.FromResult(result);
        }

        private bool MostrarSync(string titulo, string mensaje, DialogTipo tipo)
        {
            var dialog = new Controles.DialogWindow
            {
                Titulo       = titulo,
                Mensaje      = mensaje,
                TipoMensaje  = tipo,
                Owner        = Application.Current.MainWindow
            };

            dialog.ShowDialog();
            return dialog.Resultado == DialogResultado.Aceptar;
        }

        /// <summary>
        /// Ejecuta una acción con debounce de 300ms.
        /// Si se llama múltiples veces antes de 300ms, solo se ejecuta la última.
        /// </summary>
        public void EjecutarConDebounce(System.Action accion)
        {
            _debounceAction = accion;
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }
    }

    public enum DialogTipo
    {
        Info,
        Error,
        Confirmar,
        Advertencia
    }

    public enum DialogResultado
    {
        Cancelar,
        Aceptar
    }
}
