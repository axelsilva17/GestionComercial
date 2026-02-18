using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte DateTime a string formateado.
    /// Por defecto: "dd/MM/yyyy"
    /// Pasá el formato como ConverterParameter si necesitás otro.
    /// Ej: ConverterParameter=dd/MM/yyyy HH:mm
    /// </summary>
    public class FechaToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime fecha)
            {
                string formato = parameter as string ?? "dd/MM/yyyy";
                return fecha.ToString(formato, new CultureInfo("es-AR"));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && DateTime.TryParse(s, out var fecha))
                return fecha;
            return DateTime.MinValue;
        }
    }
}
