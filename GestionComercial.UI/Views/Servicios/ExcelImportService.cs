// REQUIERE: Install-Package ClosedXML
// Una vez instalado, quitá el #if y el #endif

#if true

using ClosedXML.Excel;
using GestionComercial.UI.ViewModels.Productos;
using System;
using System.Collections.Generic;
using System.IO;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Lee un archivo Excel y retorna filas para previsualizar/importar.
    ///
    /// Columnas esperadas en la planilla (en cualquier orden, por nombre de encabezado):
    ///   Nombre         → obligatorio
    ///   CodigoBarra    → obligatorio
    ///   PrecioVenta    → obligatorio
    ///   PrecioCosto    → opcional
    ///   StockActual    → opcional (default 0)
    ///   StockMinimo    → opcional (default 0)
    ///   Categoria      → opcional
    ///   UnidadMedida   → opcional (default "Unidad")
    /// </summary>
    public static class ExcelImportService
    {
        public static List<FilaImportacionDto> LeerProductos(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException("No se encontró el archivo.", rutaArchivo);

            var resultado = new List<FilaImportacionDto>();

            using var wb = new XLWorkbook(rutaArchivo);
            var ws = wb.Worksheet(1); // Primera hoja

            // Leer encabezados de la primera fila (case-insensitive)
            var columnas = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var headerRow = ws.Row(1);
            foreach (var cell in headerRow.CellsUsed())
            {
                var header = cell.GetString().Trim();
                if (!string.IsNullOrEmpty(header))
                    columnas[header] = cell.Address.ColumnNumber;
            }

            // Validar que tenga las columnas mínimas
            var requeridas = new[] { "Nombre", "CodigoBarra", "PrecioVenta" };
            foreach (var col in requeridas)
            {
                if (!columnas.ContainsKey(col))
                    throw new InvalidOperationException(
                        $"No se encontró la columna requerida '{col}'. " +
                        "Descargá la plantilla para ver el formato correcto.");
            }

            // Leer filas de datos (desde la 2)
            int ultimaFila = ws.LastRowUsed()?.RowNumber() ?? 1;

            for (int nFila = 2; nFila <= ultimaFila; nFila++)
            {
                var row = ws.Row(nFila);

                // Saltar filas completamente vacías
                if (row.IsEmpty()) continue;

                var fila = new FilaImportacionDto { Fila = nFila };
                var errores = new List<string>();

                // Nombre
                fila.Nombre = ObtenerString(row, columnas, "Nombre");
                if (string.IsNullOrWhiteSpace(fila.Nombre))
                    errores.Add("Nombre obligatorio");

                // Código de barra
                fila.CodigoBarra = ObtenerString(row, columnas, "CodigoBarra");
                if (string.IsNullOrWhiteSpace(fila.CodigoBarra))
                    errores.Add("Código de barra obligatorio");
                else if (!long.TryParse(fila.CodigoBarra, out _))
                    errores.Add("Código de barra debe ser numérico");

                // Precio venta
                fila.PrecioVenta = ObtenerDecimal(row, columnas, "PrecioVenta");
                if (fila.PrecioVenta <= 0)
                    errores.Add("Precio de venta debe ser mayor a 0");

                // Opcionales
                fila.PrecioCosto  = ObtenerDecimal(row, columnas, "PrecioCosto");
                fila.Stock        = ObtenerInt(row, columnas, "StockActual");
                fila.StockMinimo  = ObtenerInt(row, columnas, "StockMinimo");
                fila.Categoria    = ObtenerString(row, columnas, "Categoria", "Sin categoría");
                fila.UnidadMedida = ObtenerString(row, columnas, "UnidadMedida", "Unidad");

                fila.EsValida         = errores.Count == 0;
                fila.ErrorDescripcion = string.Join(", ", errores);
                fila.EsNuevo          = true; // TODO: consultar BD por CodigoBarra

                resultado.Add(fila);
            }

            return resultado;
        }

        // ── Generador de plantilla ────────────────────────────────────────────
        public static void GenerarPlantilla(string rutaDestino)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Productos");

            // Encabezados
            var headers = new[]
            {
                "Nombre", "CodigoBarra", "PrecioVenta", "PrecioCosto",
                "StockActual", "StockMinimo", "Categoria", "UnidadMedida"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Filas de ejemplo
            var ejemplos = new object[,]
            {
                { "Auriculares Pro X",  "7890001", 24000, 15000, 10, 3, "Electrónica", "Unidad" },
                { "Mouse Inalámbrico",  "7890002", 12500,  8000,  5, 2, "Periféricos", "Unidad" },
                { "Cable HDMI 2m",      "7890003",  3500,  1800, 20, 5, "Accesorios",  "Unidad" },
            };

            for (int f = 0; f < ejemplos.GetLength(0); f++)
            {
                for (int c = 0; c < ejemplos.GetLength(1); c++)
                {
                    ws.Cell(f + 2, c + 1).Value = XLCellValue.FromObject(ejemplos[f, c]);
                }
                // Formato moneda para precio
                ws.Cell(f + 2, 3).Style.NumberFormat.Format = "$ #,##0";
                ws.Cell(f + 2, 4).Style.NumberFormat.Format = "$ #,##0";
            }

            // Nota en la parte inferior
            var notaFila = ejemplos.GetLength(0) + 3;
            ws.Cell(notaFila, 1).Value = "* Las columnas Nombre, CodigoBarra y PrecioVenta son obligatorias.";
            ws.Cell(notaFila, 1).Style.Font.Italic = true;
            ws.Cell(notaFila, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Range(notaFila, 1, notaFila, headers.Length).Merge();

            // Ajuste de columnas
            for (int c = 1; c <= headers.Length; c++)
                ws.Column(c).AdjustToContents();

            wb.SaveAs(rutaDestino);
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private static string ObtenerString(IXLRow row, Dictionary<string, int> cols,
            string nombre, string defecto = "")
        {
            if (!cols.TryGetValue(nombre, out int col)) return defecto;
            return row.Cell(col).GetString().Trim() is { Length: > 0 } v ? v : defecto;
        }

        private static decimal ObtenerDecimal(IXLRow row, Dictionary<string, int> cols, string nombre)
        {
            if (!cols.TryGetValue(nombre, out int col)) return 0;
            var cell = row.Cell(col);
            if (cell.TryGetValue(out decimal d)) return d;
            if (decimal.TryParse(cell.GetString().Replace("$", "").Replace(".", "").Replace(",", ".").Trim(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal d2)) return d2;
            return 0;
        }

        private static int ObtenerInt(IXLRow row, Dictionary<string, int> cols, string nombre)
        {
            if (!cols.TryGetValue(nombre, out int col)) return 0;
            var cell = row.Cell(col);
            if (cell.TryGetValue(out int i)) return i;
            if (int.TryParse(cell.GetString().Trim(), out int i2)) return i2;
            return 0;
        }
    }
}

#endif

// ── Versión SIN ClosedXML (siempre disponible) ────────────────────────────────
// Podés usar esto mientras instalás ClosedXML, lee CSV básico como fallback.
namespace GestionComercial.Aplicacion.Servicios
{
    public static class CsvImportService
    {
        /// <summary>
        /// Lee un CSV simple como alternativa al Excel.
        /// Formato esperado: Nombre;CodigoBarra;PrecioVenta;PrecioCosto;StockActual;StockMinimo;Categoria;UnidadMedida
        /// </summary>
        public static System.Collections.Generic.List<GestionComercial.UI.ViewModels.Productos.FilaImportacionDto>
            LeerCsv(string rutaArchivo, char separador = ';')
        {
            var resultado = new System.Collections.Generic.List<GestionComercial.UI.ViewModels.Productos.FilaImportacionDto>();
            var lineas    = System.IO.File.ReadAllLines(rutaArchivo, System.Text.Encoding.UTF8);

            if (lineas.Length < 2) return resultado;

            // Leer encabezados
            var headers = lineas[0].Split(separador);
            var idx     = new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
                idx[headers[i].Trim()] = i;

            for (int n = 1; n < lineas.Length; n++)
            {
                var cols   = lineas[n].Split(separador);
                var errores = new System.Collections.Generic.List<string>();

                string Get(string nombre, string def = "") =>
                    idx.TryGetValue(nombre, out int i) && i < cols.Length
                        ? cols[i].Trim() : def;

                decimal GetDec(string nombre) =>
                    decimal.TryParse(Get(nombre).Replace(",", "."),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : 0;

                int GetInt(string nombre) =>
                    int.TryParse(Get(nombre), out var v) ? v : 0;

                var fila = new GestionComercial.UI.ViewModels.Productos.FilaImportacionDto
                {
                    Fila         = n + 1,
                    Nombre       = Get("Nombre"),
                    CodigoBarra  = Get("CodigoBarra"),
                    PrecioVenta  = GetDec("PrecioVenta"),
                    PrecioCosto  = GetDec("PrecioCosto"),
                    Stock        = GetInt("StockActual"),
                    StockMinimo  = GetInt("StockMinimo"),
                    Categoria    = Get("Categoria", "Sin categoría"),
                    UnidadMedida = Get("UnidadMedida", "Unidad"),
                    EsNuevo      = true,
                };

                if (string.IsNullOrWhiteSpace(fila.Nombre))     errores.Add("Nombre obligatorio");
                if (string.IsNullOrWhiteSpace(fila.CodigoBarra)) errores.Add("Código de barra obligatorio");
                if (fila.PrecioVenta <= 0)                       errores.Add("Precio de venta requerido");

                fila.EsValida         = errores.Count == 0;
                fila.ErrorDescripcion = string.Join(", ", errores);

                resultado.Add(fila);
            }
            return resultado;
        }
    }
}
