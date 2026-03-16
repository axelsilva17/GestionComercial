using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte un decimal a formato moneda argentina: $ 1.234,56
    /// </summary>
    public class DecimalToMonedaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d)
                return d.ToString("C2", new CultureInfo("es-AR"));

            if (value is double dbl)
                return ((decimal)dbl).ToString("C2", new CultureInfo("es-AR"));

            if (value is int i)
                return ((decimal)i).ToString("C2", new CultureInfo("es-AR"));

            return "$ 0,00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                s = s.Replace("$", "").Trim();
                if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("es-AR"), out var result))
                    return result;
            }
            return 0m;
        }
    }
}
