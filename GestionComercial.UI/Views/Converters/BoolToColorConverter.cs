using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte un booleano a color:
    /// true → Verde (#10B981)
    /// false → Rojo (#EF4444)
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush Verde = new(Color.FromRgb(0x10, 0xB9, 0x81));
        private static readonly SolidColorBrush Rojo = new(Color.FromRgb(0xEF, 0x44, 0x44));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool esIngreso)
            {
                return esIngreso ? Verde : Rojo;
            }
            return Verde;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}