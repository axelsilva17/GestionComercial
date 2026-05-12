using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GestionComercial.Infraestructura.Utilidades
{
    /// <summary>
    /// Validador de CUIT/CUIL argentino.
    /// Valida formato y dígito verificador.
    /// </summary>
    public static class ValidadorCUIT
    {
        private static readonly Regex FormatoCUIT = new(
            @"^\d{2}-\d{8}-\d{1}$",
            RegexOptions.Compiled);

        private static readonly Regex FormatoCUITSinGuiones = new(
            @"^\d{11}$",
            RegexOptions.Compiled);

        // Serie para el cálculo del dígito verificador (de derecha a izquierda)
        // Cuando se lee de izquierda a derecha, la serie es: 5,4,3,2,7,6,5,4,3,2
        private static readonly int[] Serie = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        /// <summary>
        /// Valida un CUIT con o sin guiones.
        /// </summary>
        /// <param name="cuit">CUIT en formato XX-XXXXXXXX-X o XXXXXXXXXXX</param>
        /// <returns>True si el CUIT es válido</returns>
        public static bool EsValido(string? cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            string cuitLimpio = cuit.Trim();

            // Verificar formato (con o sin guiones)
            if (!FormatoCUIT.IsMatch(cuitLimpio) && !FormatoCUITSinGuiones.IsMatch(cuitLimpio))
                return false;

            // Obtener solo los dígitos
            string soloDigitos = new string(cuitLimpio.Where(char.IsDigit).ToArray());

            if (soloDigitos.Length != 11)
                return false;

            // Convertir a array de enteros
            int[] digitos = soloDigitos.Select(c => int.Parse(c.ToString())).ToArray();

            // Calcular dígito verificador esperado
            int digitoEsperado = CalcularDigitoVerificador(digitos.Take(10).ToArray());

            // Comparar con el último dígito
            return digitos[10] == digitoEsperado;
        }

        /// <summary>
        /// Formatea un CUIT sin guiones al formato XX-XXXXXXXX-X.
        /// </summary>
        /// <param name="cuitSinGuiones">CUIT de 11 dígitos</param>
        /// <returns>CUIT formateado, o el valor original si no es válido</returns>
        public static string Formatear(string? cuitSinGuiones)
        {
            if (string.IsNullOrWhiteSpace(cuitSinGuiones))
                return string.Empty;

            string soloDigitos = new string(cuitSinGuiones.Where(char.IsDigit).ToArray());

            if (soloDigitos.Length != 11)
                return cuitSinGuiones ?? string.Empty;

            return $"{soloDigitos.Substring(0, 2)}-{soloDigitos.Substring(2, 8)}-{soloDigitos.Substring(10, 1)}";
        }

        /// <summary>
        /// Calcula el dígito verificador de un CUIT.
        /// Algoritmo: Módulo 11 con la serie 5,4,3,2,7,6,5,4,3,2
        /// </summary>
        /// <param name="primerosDiez">Primeros 10 dígitos del CUIT</param>
        /// <returns>Dígito verificador</returns>
        private static int CalcularDigitoVerificador(int[] primerosDiez)
        {
            if (primerosDiez.Length != 10)
                throw new ArgumentException("Se requieren exactamente 10 dígitos", nameof(primerosDiez));

            int suma = 0;

            for (int i = 0; i < 10; i++)
            {
                suma += primerosDiez[i] * Serie[i];
            }

            int resto = suma % 11;

            return resto switch
            {
                0 => 0,
                1 => 9,  // Si el resto es 1, el dígito es 9 (para CUIT/CUIL)
                _ => 11 - resto
            };
        }
    }
}
