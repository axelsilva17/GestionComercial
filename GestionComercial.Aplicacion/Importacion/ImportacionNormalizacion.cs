using System.Globalization;
using System.Text.RegularExpressions;

namespace GestionComercial.Aplicacion.Importacion
{
    /// <summary>
    /// Culture-tolerant parsing helpers shared by every Excel import path.
    /// Accepts invariant ("1234.56"), en-US currency ("$1,234.56") and
    /// es-AR ("1.234,56" or "12.500,99") formats, plus optional currency
    /// symbols and thousand separators made of spaces.
    /// </summary>
    public static class ImportacionNormalizacion
    {
        private static readonly Regex GrupoMilesPunto = new(@"^\d{1,3}(\.\d{3})+$", RegexOptions.Compiled);
        private static readonly Regex GrupoMilesComa = new(@"^\d{1,3}(,\d{3})+$", RegexOptions.Compiled);

        /// <summary>
        /// Parses a decimal value regardless of the locale that produced it.
        /// </summary>
        /// <param name="valor">Raw cell value (may include currency symbols and spaces).</param>
        /// <param name="result">Parsed value; 0 when parsing fails.</param>
        /// <returns>True when the value could be parsed as a valid decimal.</returns>
        public static bool TryParseDecimal(string? valor, out decimal result)
        {
            result = 0;
            var texto = (valor ?? string.Empty).Trim();
            if (texto.Length == 0) return false;

            // Drop currency symbols and internal spaces ("$ 1.234,56" -> "1.234,56").
            var limpio = new string(texto
                .Where(c => !char.IsWhiteSpace(c) && !EsSimboloMoneda(c))
                .ToArray());
            if (limpio.Length == 0) return false;

            var tienePunto = limpio.Contains('.');
            var tieneComa = limpio.Contains(',');

            string unificado;
            if (tienePunto && tieneComa)
            {
                // Ambiguous formats: the LAST '.' or ',' is the decimal separator;
                // the other one is the thousands separator.
                //   "$1,234.56" -> "1234.56"   (en-US)
                //   "1.234,56"  -> "1234.56"   (es-AR)
                unificado = limpio.LastIndexOf('.') > limpio.LastIndexOf(',')
                    ? limpio.Replace(",", "")
                    : limpio.Replace(".", "").Replace(",", ".");
            }
            else if (tieneComa)
            {
                unificado = limpio.Replace(",", ".");
            }
            else
            {
                unificado = limpio;
            }

            return decimal.TryParse(unificado, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Parses an integer value accepting es-AR ("1.234") and en-US ("1,234")
        /// thousands separators. Values like "1.5" are rejected instead of being
        /// silently mangled into 15.
        /// </summary>
        public static bool TryParseInt(string? valor, out int result)
        {
            result = 0;
            var texto = (valor ?? string.Empty).Trim();
            if (texto.Length == 0) return false;

            if (int.TryParse(texto, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                return true;

            if (GrupoMilesPunto.IsMatch(texto) || GrupoMilesComa.IsMatch(texto))
            {
                var limpio = texto.Replace(".", "").Replace(",", "");
                return int.TryParse(limpio, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
            }

            return false;
        }

        /// <summary>
        /// Parses a date accepting ISO ("2024-05-01"), "dd-MM-yyyy", "dd/MM/yyyy"
        /// and the current culture format as a last resort.
        /// </summary>
        public static bool TryParseFecha(string? valor, out DateTime result)
        {
            result = default;
            var texto = (valor ?? string.Empty).Trim();
            if (texto.Length == 0) return false;

            var formatos = new[] { "yyyy-MM-dd", "dd-MM-yyyy", "dd/MM/yyyy", "yyyy/MM/dd" };
            if (DateTime.TryParseExact(texto, formatos, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out result))
                return true;

            return DateTime.TryParse(texto, CultureInfo.CurrentCulture, DateTimeStyles.None, out result)
                || DateTime.TryParse(texto, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        private static bool EsSimboloMoneda(char c)
            => c is '$' or '€' or '£' or '¥' or '¤';
    }
}