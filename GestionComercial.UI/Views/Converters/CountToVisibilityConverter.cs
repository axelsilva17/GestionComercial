using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte una colección (Count) a Visibility. 
    /// Count > 0 = Visible, Count == 0 = Collapsed
    /// Usa ConverterParameter="Inverse" para invertir la lógica
    /// </summary>
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            int count = 0;
            if (value is int intVal)
                count = intVal;
            else if (value is System.Collections.ICollection coll)
                count = coll.Count;

            bool tieneItems = count > 0;
            
            // Parameter puede ser "Inverse" para invertir
            bool invertir = parameter is string paramStr && paramStr.Equals("Inverse", StringComparison.OrdinalIgnoreCase);
            
            if (invertir)
                tieneItems = !tieneItems;

            return tieneItems ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}