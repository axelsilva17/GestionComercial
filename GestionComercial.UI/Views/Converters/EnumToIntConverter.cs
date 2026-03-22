using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return Enum.Parse(targetType, intValue.ToString());
            return Enum.Parse(targetType, value.ToString() ?? "0");
        }
    }
}