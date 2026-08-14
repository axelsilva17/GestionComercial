using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Compara el valor enlazado (string) contra el ConverterParameter.
    /// Permite enlazar RadioButton.IsChecked a una propiedad string compartida.
    /// </summary>
    public class StringEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.Equals(value?.ToString(), parameter?.ToString(), StringComparison.Ordinal);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is true ? parameter?.ToString() : Binding.DoNothing;
    }
}