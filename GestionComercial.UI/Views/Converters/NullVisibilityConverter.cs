using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// null   → Collapsed
    /// no-null → Visible
    /// Con InvertirValor = true: al revés (null → Visible, útil para placeholders).
    /// </summary>
    public class NullVisibilityConverter : IValueConverter
    {
        public bool InvertirValor { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool tieneValor = value != null && value.ToString() != string.Empty;
            if (InvertirValor) tieneValor = !tieneValor;
            return tieneValor ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
