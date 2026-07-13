using System;
using System.Windows.Input;

namespace GestionComercial.UI.Views.Comandos
{
    ///     /// Comando sincrónico reutilizable para bindear acciones en la UI.
    /// Uso: new RelayCommand(() => HacerAlgo(), () => PuedoHacerlo)
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        /// Fuerza re-evaluación de CanExecute en la UI.
        public void RaiseCanExecuteChanged() =>
            CommandManager.InvalidateRequerySuggested();
    }

    ///     /// Versión genérica: acepta un parámetro tipado.
    /// Uso: new RelayCommand&lt;Producto&gt;(p => Editar(p))
    
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) =>
            _canExecute?.Invoke(parameter is T t ? t : default) ?? true;

        public void Execute(object? parameter) =>
            _execute(parameter is T t ? t : default);

        public void RaiseCanExecuteChanged() =>
            CommandManager.InvalidateRequerySuggested();
    }
}
