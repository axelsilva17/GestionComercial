using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    ///     /// decimal > 0 → Visible; otherwise Collapsed.
    /// InvertirValor: decimal > 0 → Collapsed.
    public class DecimalToVisibilityConverter : IValueConverter
    {
        public bool InvertirValor { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = false;

            if (value is decimal d)
                visible = d > 0;
            else if (value is int i)
                visible = i > 0;

            if (parameter is string paramStr && paramStr.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                visible = !visible;
            else if (InvertirValor)
                visible = !visible;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility v && v == Visibility.Visible;
        }
    }
}
