using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters;

/// <summary>
/// Converts a width value to a percentage of itself.
/// Usage: {Binding ActualWidth, Converter={StaticResource WidthToPercentConverter}, ConverterParameter=0.90}
/// </summary>
public class WidthToPercentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width && parameter is string percentStr && double.TryParse(percentStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var percent))
        {
            return width * percent;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
