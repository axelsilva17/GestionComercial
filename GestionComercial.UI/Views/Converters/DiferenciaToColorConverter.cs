using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte un valor decimal a color según su signo:
    /// - Positivo = Verde (Success)
    /// - Negativo = Rojo (Error)
    /// - Cero/Null = Gris (TextSecondary)
    /// </summary>
    public class DiferenciaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal diff)
            {
                if (diff > 0)
                    return new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Success - verde
                if (diff < 0)
                    return new SolidColorBrush(Color.FromRgb(239, 68, 68));  // Error - rojo
            }
            return new SolidColorBrush(Color.FromRgb(100, 116, 139)); // TextSecondary - gris
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
