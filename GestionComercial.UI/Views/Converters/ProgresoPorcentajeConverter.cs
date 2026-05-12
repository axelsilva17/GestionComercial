using System;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte porcentaje (0-100) a ancho fijo en pixeles para barra de progreso.
    /// </summary>
    public class ProgresoPorcentajeConverter : IValueConverter
    {
        // Ancho máximo de la barra (aproximado, se ajusta en XAML si es necesario)
        public double MaxWidth { get; set; } = 400;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0d;

            if (!int.TryParse(value.ToString(), out int progreso))
                return 0d;

            // Calcular ancho proporcional (el contenedor tiene ~400px disponibles)
            var maxAncho = parameter != null && double.TryParse(parameter.ToString(), out var CustomMax)
                ? CustomMax
                : 400;

            return Math.Max(0, maxAncho * progreso / 100.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}