using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Returns "Positivo", "Negativo", or "Cero" based on the sign of the decimal value.
    /// Used for DataTrigger in XAML.
    /// </summary>
    public class DiferenciaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Cero";

            if (value is decimal diff)
            {
                if (diff > 0)
                    return "Positivo";
                if (diff < 0)
                    return "Negativo";
            }
            return "Cero";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
