using Caliburn.Micro;

using System.Collections.Generic;
using System.ComponentModel;

namespace GestionComercial.UI.ViewModels.Base
{
    public class ViewModelBase : Screen
    {
        private bool _isLoading;
        private string _errorMessage;
        private bool _errorVisible;

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; NotifyOfPropertyChange(() => IsLoading); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; NotifyOfPropertyChange(() => ErrorMessage); }
        }

        public bool ErrorVisible
        {
            get => _errorVisible;
            set { _errorVisible = value; NotifyOfPropertyChange(() => ErrorVisible); }
        }

        protected void MostrarError(string mensaje)
        {
            ErrorMessage = mensaje;
            ErrorVisible = true;
        }

        protected void LimpiarError()
        {
            ErrorMessage = string.Empty;
            ErrorVisible = false;
        }

        // SetProperty helper para simplificación
        protected bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            NotifyOfPropertyChange(propertyName);
            return true;
        }
    }
}
