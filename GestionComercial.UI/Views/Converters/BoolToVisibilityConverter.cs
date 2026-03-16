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
            bool visible = value is bool b && b;
            if (InvertirValor) visible = !visible;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = value is Visibility v && v == Visibility.Visible;
            return InvertirValor ? !visible : visible;
        }
    }
}
