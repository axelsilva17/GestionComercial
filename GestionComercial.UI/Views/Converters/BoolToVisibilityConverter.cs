using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// true  → Visible
    /// false → Collapsed
    /// Con InvertirValor = true: al revés.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool InvertirValor { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible;
            
            // Si es bool, usar el valor directo
            if (value is bool b)
            {
                visible = b;
            }
            // Si es cualquier otro objeto (no-bool), es visible si no es null
            else
            {
                visible = value != null;
            }
            
            // Leer ConverterParameter="Inverse" desde XAML para invertir el valor
            if (parameter is string paramStr && paramStr.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                visible = !visible;
            // Si no hay parameter, usar la propiedad InvertirValor (backwards compatible)
            else if (InvertirValor)
                visible = !visible;
                
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = value is Visibility v && v == Visibility.Visible;
            // Leer ConverterParameter="Inverse" desde XAML para invertir el valor
            if (parameter is string paramStr && paramStr.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                visible = !visible;
            else if (InvertirValor)
                visible = !visible;
            return visible;
        }
    }
}
