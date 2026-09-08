// REQUIERE: Install-Package ClosedXML
// Una vez instalado, quitá el #if y el #endif

#if true

using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Importacion;
using System;
using System.Collections.Generic;
using System.IO;

namespace GestionComercial.Aplicacion.Servicios
{
    ///     /// Lee un archivo Excel y retorna filas para previsualizar/importar.
    ///
    /// Columnas esperadas en la planilla (en cualquier orden, por nombre de encabezado):
    ///   Nombre         → obligatorio
    ///   CodigoBarra    → opcional (vacío = sin código; si se provee debe ser numérico)
    ///   PrecioVenta    → obligatorio
    ///   PrecioCosto    → opcional
    ///   StockActual    → opcional (default 0)
    ///   StockMinimo    → opcional (default 0)
    ///   Categoria      → opcional
    ///   UnidadMedida   → opcional (default "Unidad")
    public static class ExcelImportService
    {
        public static List<FilaImportacionDto> LeerProductos(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException("No se encontró el archivo.", rutaArchivo);

            var resultado = new List<FilaImportacionDto>();

            using var wb = new XLWorkbook(rutaArchivo);
            var ws = wb.Worksheet(1); // Primera hoja

            // Leer encabezados de la primera fila (case-insensitive), mapeando por número de columna real
            var columnas = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var headerRow = ws.Row(1);
            foreach (var cell in headerRow.CellsUsed())
            {
                var header = cell.GetString().Trim();
                if (!string.IsNullOrEmpty(header))
                    columnas[header] = cell.Address.ColumnNumber;
            }

            // Validar que tenga las columnas mínimas
            // Nota: CodigoBarra puede tener valores vacíos pero la columna debe existir.
            var requeridas = new[] { "Nombre", "CodigoBarra", "PrecioVenta" };
            foreach (var col in requeridas)
            {
                if (!columnas.ContainsKey(col))
                    throw new InvalidOperationException(
                        $"No se encontró la columna requerida '{col}'. " +
                        "Descargá la plantilla para ver el formato correcto.");
            }

            // Leer filas de datos en UNA pasada sobre el rango usado
            foreach (var row in ws.RangeUsed()?.RowsUsed() ?? Enumerable.Empty<IXLRangeRow>())
            {
                if (row.RowNumber() == 1) continue; // encabezado

                string Celda(string nombre)
                    => columnas.TryGetValue(nombre, out int col) ? row.Cell(col).GetString().Trim() : string.Empty;

                var nombre = Celda("Nombre");
                var codigoBarra = Celda("CodigoBarra");
                var precioVentaStr = Celda("PrecioVenta");
                var precioCostoStr = Celda("PrecioCosto");
                var stockStr = Celda("StockActual");
                var stockMinStr = Celda("StockMinimo");
                var categoria = Celda("Categoria");
                var unidadMedida = Celda("UnidadMedida");

                // Normalización: omitir filas vacías o con solo espacios en las columnas mapeadas
                if (string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(codigoBarra)
                    && string.IsNullOrEmpty(precioVentaStr) && string.IsNullOrEmpty(precioCostoStr)
                    && string.IsNullOrEmpty(stockStr) && string.IsNullOrEmpty(stockMinStr)
                    && string.IsNullOrEmpty(categoria) && string.IsNullOrEmpty(unidadMedida))
                    continue;

                var fila = new FilaImportacionDto { Fila = row.RowNumber() };
                var errores = new List<string>();

                // Nombre
                fila.Nombre = nombre;
                if (string.IsNullOrWhiteSpace(fila.Nombre))
                    errores.Add("Nombre obligatorio");

                // Código de barra — OPTIONAL: empty/whitespace is valid (no barcode).
                // Only validate numeric format when a value IS provided.
                fila.CodigoBarra = codigoBarra;
                if (!string.IsNullOrWhiteSpace(fila.CodigoBarra) && !long.TryParse(fila.CodigoBarra, out _))
                    errores.Add("Código de barra debe ser numérico");

                // Precio venta (formato tolerante: "24000.50", "$1,234.56", "1.234,56")
                if (!ImportacionNormalizacion.TryParseDecimal(precioVentaStr, out var precioVenta))
                    errores.Add("Precio de venta inválido");
                else if (precioVenta <= 0)
                    errores.Add("Precio de venta debe ser mayor a 0");
                fila.PrecioVenta = precioVenta;

                // Opcionales
                fila.PrecioCosto = ImportacionNormalizacion.TryParseDecimal(precioCostoStr, out var precioCosto)
                    ? precioCosto : 0;
                fila.Stock = ImportacionNormalizacion.TryParseInt(stockStr, out var stock) ? stock : 0;
                fila.StockMinimo = ImportacionNormalizacion.TryParseInt(stockMinStr, out var stockMinimo)
                    ? stockMinimo : 0;

                fila.Categoria = categoria.Length > 0 ? categoria : "Sin categoría";
                fila.UnidadMedida = unidadMedida.Length > 0 ? unidadMedida : "Unidad";

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
            ws.Cell(notaFila, 1).Value = "* Las columnas Nombre y PrecioVenta son obligatorias. CodigoBarra es opcional (vacío = sin código).";
            ws.Cell(notaFila, 1).Style.Font.Italic = true;
            ws.Cell(notaFila, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Range(notaFila, 1, notaFila, headers.Length).Merge();

            // Ajuste de columnas
            for (int c = 1; c <= headers.Length; c++)
                ws.Column(c).AdjustToContents();

            wb.SaveAs(rutaDestino, new SaveOptions { ValidatePackage = false });
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
        ///         /// Lee un CSV simple como alternativa al Excel.
        /// Formato esperado: Nombre;CodigoBarra;PrecioVenta;PrecioCosto;StockActual;StockMinimo;Categoria;UnidadMedida
        public static System.Collections.Generic.List<GestionComercial.Aplicacion.DTOs.Productos.FilaImportacionDto>
            LeerCsv(string rutaArchivo, char separador = ';')
        {
            var resultado = new System.Collections.Generic.List<GestionComercial.Aplicacion.DTOs.Productos.FilaImportacionDto>();
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

                var fila = new GestionComercial.Aplicacion.DTOs.Productos.FilaImportacionDto
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

                if (string.IsNullOrWhiteSpace(fila.Nombre)) errores.Add("Nombre obligatorio");
                // CodigoBarra is OPTIONAL — empty/whitespace means no barcode.
                if (fila.PrecioVenta <= 0) errores.Add("Precio de venta requerido");

                fila.EsValida         = errores.Count == 0;
                fila.ErrorDescripcion = string.Join(", ", errores);

                resultado.Add(fila);
            }
            return resultado;
        }
    }
}
