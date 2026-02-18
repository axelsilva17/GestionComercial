using Caliburn.Micro;

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
    }
}
