// NOTA: Este archivo requiere el paquete NuGet ClosedXML.
// Install-Package ClosedXML
// Una vez instalado, eliminá el #if y el #endif al final.

#if true  // ← Habilitado para exportación

using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace GestionComercial.UI.Helpers
{
    /// <summary>
    /// Helper de exportación a Excel usando ClosedXML.
    /// NuGet: Install-Package ClosedXML
    ///
    /// Uso desde ViewModel:
    ///   ExportHelper.ExportarVentas(misVentas);
    ///   ExportHelper.ExportarMargen(misMargen);
    /// </summary>
    public static class ExportHelper
    {
        // ── Exportadores específicos por reporte ─────────────────────────────

        public static void ExportarVentasPorDia(IEnumerable<VentaPorDiaDto> datos, DateTime desde, DateTime hasta)
        {
            Exportar("Ventas por Día", $"VentasPorDia_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Ventas por Día");
                var headers = new[] { "Día", "Total Ventas", "Cantidad" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.Dia;
                    ws.Cell(fila, 2).Value = (double)d.Total;
                    ws.Cell(fila, 3).Value = d.Cantidad;
                    ws.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0";
                    fila++;
                }

                AgregarTotales(ws, fila, new[] { "", "Total", "" });
                ws.Cell(fila, 2).FormulaA1 = $"=SUM(B2:B{fila - 1})";
                ws.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0";

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Ventas por Día", desde, hasta);
            });
        }

        public static void ExportarMargen(IEnumerable<ReporteMargenDto> datos, DateTime desde, DateTime hasta)
        {
            Exportar("Margen por Producto", $"Margen_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Margen");
                var headers = new[] { "Producto", "Categoría", "Costo", "Precio Venta", "Margen Unit.", "Margen %", "Cant. Vendida", "Ganancia Total" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.ProductoNombre;
                    ws.Cell(fila, 2).Value = d.Categoria;
                    ws.Cell(fila, 3).Value = (double)d.PrecioCosto;
                    ws.Cell(fila, 4).Value = (double)d.PrecioVenta;
                    ws.Cell(fila, 5).Value = (double)d.MargenUnitario;
                    ws.Cell(fila, 6).Value = (double)d.MargenPorcentaje / 100;
                    ws.Cell(fila, 7).Value = d.CantidadVendida;
                    ws.Cell(fila, 8).Value = (double)d.MargenTotal;

                    ws.Cell(fila, 3).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 4).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 5).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 6).Style.NumberFormat.Format = "0.0%";
                    ws.Cell(fila, 8).Style.NumberFormat.Format = "$ #,##0";
                    fila++;
                }

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Reporte de Márgenes", desde, hasta);
            });
        }

        public static void ExportarRotacion(IEnumerable<ReporteRotacionDto> datos, DateTime desde, DateTime hasta)
        {
            Exportar("Rotación de Stock", $"Rotacion_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Rotación");
                var headers = new[] { "Producto", "Categoría", "Stock Actual", "Cant. Vendida", "Cant. Comprada", "Índice Rotación", "Última Venta", "Última Compra" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.ProductoNombre;
                    ws.Cell(fila, 2).Value = d.Categoria;
                    ws.Cell(fila, 3).Value = d.StockActual;
                    ws.Cell(fila, 4).Value = d.CantidadVendida;
                    ws.Cell(fila, 5).Value = d.CantidadComprada;
                    ws.Cell(fila, 6).Value = (double)d.IndiceRotacion;
                    ws.Cell(fila, 7).Value = d.UltimaVenta != DateTime.MinValue ? d.UltimaVenta.ToString("dd/MM/yyyy") : "-";
                    ws.Cell(fila, 8).Value = d.UltimaCompra != DateTime.MinValue ? d.UltimaCompra.ToString("dd/MM/yyyy") : "-";
                    ws.Cell(fila, 6).Style.NumberFormat.Format = "0.0";
                    fila++;
                }

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Rotación de Stock", desde, hasta);
            });
        }

        public static void ExportarStock(IEnumerable<ReportesStockDto> datos)
        {
            Exportar("Estado de Stock", $"Stock_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Stock");
                var headers = new[] { "Producto", "Categoría", "Sucursal", "Stock Actual", "Stock Mínimo", "Estado" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.ProductoNombre;
                    ws.Cell(fila, 2).Value = d.Categoria;
                    ws.Cell(fila, 3).Value = d.Sucursal;
                    ws.Cell(fila, 4).Value = d.StockActual;
                    ws.Cell(fila, 5).Value = d.StockMinimo;
                    ws.Cell(fila, 6).Value = d.Estado;

                    // Color por estado
                    if (d.SinStock)
                        ws.Cell(fila, 6).Style.Font.FontColor = XLColor.Red;
                    else if (d.StockBajo)
                        ws.Cell(fila, 6).Style.Font.FontColor = XLColor.Orange;
                    else
                        ws.Cell(fila, 6).Style.Font.FontColor = XLColor.Green;

                    fila++;
                }

                FormatearHoja(ws, headers.Length);
            });
        }

        public static void ExportarVendedores(IEnumerable<ReporteVendedorDto> datos, DateTime desde, DateTime hasta)
        {
            Exportar("Rendimiento Vendedores", $"Vendedores_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Vendedores");
                var headers = new[] { "Vendedor", "Sucursal", "Cant. Ventas", "Total Vendido", "Ticket Promedio", "Descuentos" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.UsuarioNombre;
                    ws.Cell(fila, 2).Value = d.Sucursal;
                    ws.Cell(fila, 3).Value = d.CantidadVentas;
                    ws.Cell(fila, 4).Value = (double)d.TotalVendido;
                    ws.Cell(fila, 5).Value = (double)d.PromedioVenta;
                    ws.Cell(fila, 6).Value = (double)d.TotalDescuentos;

                    ws.Cell(fila, 4).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 5).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 6).Style.NumberFormat.Format = "$ #,##0";
                    fila++;
                }

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Rendimiento de Vendedores", desde, hasta);
            });
        }

        // ── Motor genérico ───────────────────────────────────────────────────

        private static void Exportar(string titulo, string nombreArchivo, Action<XLWorkbook> construir)
        {
            try
            {
                var dialogo = new Microsoft.Win32.SaveFileDialog
                {
                    Title            = $"Exportar {titulo}",
                    FileName         = nombreArchivo,
                    DefaultExt       = ".xlsx",
                    Filter           = "Excel (*.xlsx)|*.xlsx"
                };

                if (dialogo.ShowDialog() != true) return;

                using var wb = new XLWorkbook();
                construir(wb);
                wb.SaveAs(dialogo.FileName);

                var resultado = MessageBox.Show(
                    $"Archivo exportado correctamente.\n¿Deseás abrirlo ahora?",
                    "Exportación exitosa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (resultado == MessageBoxResult.Yes)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName        = dialogo.FileName,
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Helpers de formato ───────────────────────────────────────────────

        private static void AgregarHeaders(IXLWorksheet ws, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }
        }

        private static void AgregarTotales(IXLWorksheet ws, int fila, string[] etiquetas)
        {
            for (int i = 0; i < etiquetas.Length; i++)
            {
                var cell = ws.Cell(fila, i + 1);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0F4FF");
                if (!string.IsNullOrEmpty(etiquetas[i]))
                    cell.Value = etiquetas[i];
            }
        }

        private static void FormatearHoja(IXLWorksheet ws, int columnas)
        {
            // Filas alternas
            var rango = ws.RangeUsed();
            int totalFilas = rango?.RowCount() ?? 1;
            for (int f = 2; f <= totalFilas; f++)
            {
                if (f % 2 == 0)
                    ws.Row(f).Style.Fill.BackgroundColor = XLColor.FromHtml("#F8FAFF");
            }

            // Auto-ajustar columnas
            for (int c = 1; c <= columnas; c++)
                ws.Column(c).AdjustToContents();

            // Borde general
            var range = ws.RangeUsed();
            if (range != null)
            {
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.OutsideBorderColor = XLColor.FromHtml("#CBD5E1");
            }
        }

        private static void AgregarMetadatos(IXLWorksheet ws, string titulo, DateTime desde, DateTime hasta)
        {
            // Agregar info del período al pie
            int ultimaFila = (ws.RangeUsed()?.LastRowUsed()?.RowNumber() ?? 1) + 2;
            ws.Cell(ultimaFila, 1).Value = $"Período: {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}";
            ws.Cell(ultimaFila, 1).Style.Font.Italic = true;
            ws.Cell(ultimaFila, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Cell(ultimaFila + 1, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Cell(ultimaFila + 1, 1).Style.Font.Italic = true;
            ws.Cell(ultimaFila + 1, 1).Style.Font.FontColor = XLColor.Gray;
        }

        private static string Fecha() => DateTime.Now.ToString("yyyyMMdd_HHmm");
    }
}

#endif
