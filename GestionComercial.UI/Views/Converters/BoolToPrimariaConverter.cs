using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    ///     /// Convierte un bool (EsPrimaria) a texto descriptivo.
    /// true → "⭐ Principal"
    /// false → "Secundaria"
    public class BoolToPrimariaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool esPrimaria)
            {
                return esPrimaria ? "⭐ Principal" : "Secundaria";
            }
            return "Secundaria";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string texto)
            {
                return texto.Contains("Principal") || texto.Contains("⭐");
            }
            return false;
        }
    }
}
