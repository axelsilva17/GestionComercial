 using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte un porcentaje (0-100) en un ancho en píxeles relativo al contenedor.
    /// Uso en XAML:
    ///   Width="{MultiBinding Converter={StaticResource ProgressToWidthConverter}}"
    ///   con Binding Progreso (int 0-100) y Binding ActualWidth del contenedor.
    /// </summary>
    public class ProgressToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length < 2) return 0d;
            if (!double.TryParse(values[0]?.ToString(), out var progreso)) return 0d;
            if (!double.TryParse(values[1]?.ToString(), out var ancho))    return 0d;
            return Math.Max(0, ancho * progreso / 100.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
