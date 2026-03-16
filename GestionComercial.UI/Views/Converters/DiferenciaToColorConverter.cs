using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Returns a SolidColorBrush based on the sign of a decimal value.
    /// Positive = Green, Negative = Red, Zero/Null = Gray
    /// </summary>
    public class DiferenciaToColorConverter : IValueConverter
    {
        // Static brushes to avoid creating new instances
        private static readonly SolidColorBrush Verde = new SolidColorBrush(Color.FromRgb(16, 185, 129));
        private static readonly SolidColorBrush Rojo = new SolidColorBrush(Color.FromRgb(239, 68, 68));
        private static readonly SolidColorBrush Gris = new SolidColorBrush(Color.FromRgb(100, 116, 139));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Gris;

            if (value is decimal diff)
            {
                if (diff > 0)
                    return Verde;
                if (diff < 0)
                    return Rojo;
            }
            return Gris;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
