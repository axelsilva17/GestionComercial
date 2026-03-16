using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.UI.Views.Converters
{
    /// <summary>
    /// Convierte un bool (Activo) a color de fondo o texto de estado.
    /// true  → verde  / "Activo"
    /// false → rojo   / "Inactivo"
    /// </summary>
    public class EstadoToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush VerdeActivo  = new SolidColorBrush(Color.FromRgb(34, 197, 94));   // #22C55E
        private static readonly SolidColorBrush RojoInactivo = new SolidColorBrush(Color.FromRgb(239, 68, 68));   // #EF4444

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool activo = value is bool b && b;

            // Si piden string (para el TextBlock de texto)
            if (targetType == typeof(string))
                return activo ? "Activo" : "Inactivo";

            // Si piden Brush (para el fondo del badge)
            return activo ? VerdeActivo : RojoInactivo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
