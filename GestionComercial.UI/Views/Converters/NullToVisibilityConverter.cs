using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte null a Collapsed y no-null a Visible (y viceversa con InvertirValor).
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public bool InvertirValor { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool esNull = value == null;
            
            // Parameter puede ser "Inverse" para invertir
            bool invertir = parameter is string paramStr && paramStr.Equals("Inverse", StringComparison.OrdinalIgnoreCase);
            
            if (InvertirValor || invertir)
                esNull = !esNull;

            return esNull ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}