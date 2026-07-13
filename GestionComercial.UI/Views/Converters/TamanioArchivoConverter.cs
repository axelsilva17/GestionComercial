using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    ///     /// Convierte un long (bytes) a un string legible (B, KB, MB, GB, TB).
    public class TamanioArchivoConverter : IValueConverter
    {
        private static readonly string[] Sufijos = { "B", "KB", "MB", "GB", "TB" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not long bytes || bytes < 0)
                return "0 B";

            int i = 0;
            decimal tam = bytes;

            while (tam >= 1024 && i < Sufijos.Length - 1)
            {
                tam /= 1024;
                i++;
            }

            return $"{tam:F1} {Sufijos[i]}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
