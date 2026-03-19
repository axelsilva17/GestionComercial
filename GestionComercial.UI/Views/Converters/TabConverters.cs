using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte el índice de tab activo (int) a IsChecked para RadioButtons.
    /// Parameter indica el índice de este tab.
    /// </summary>
    public class TabIndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && parameter is string paramStr && int.TryParse(paramStr, out int tabIndex))
                return index == tabIndex;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string paramStr && int.TryParse(paramStr, out int tabIndex))
                return tabIndex;
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// Muestra el contenido si el tab activo coincide con el parámetro.
    /// </summary>
    public class TabIndexToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && parameter is string paramStr && int.TryParse(paramStr, out int tabIndex))
                return index == tabIndex ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
