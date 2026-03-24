// NOTA: Este archivo requiere el paquete NuGet ClosedXML.
// Install-Package ClosedXML
// Una vez instalado, eliminá el #if y el #endif al final.

#if true  // ← Habilitado para exportación

using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.DTOs.Caja;
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

        public static void ExportarTopProductos(IEnumerable<ReporteTopProductoDto> datos, DateTime desde, DateTime hasta)
        {
            Exportar("Top Productos", $"TopProductos_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Top Productos");
                var headers = new[] { "Producto", "Categoría", "Cant. Vendida", "Ingresos", "Margen Total", "Margen %" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.ProductoNombre;
                    ws.Cell(fila, 2).Value = d.Categoria;
                    ws.Cell(fila, 3).Value = d.CantidadVendida;
                    ws.Cell(fila, 4).Value = (double)d.Ingresos;
                    ws.Cell(fila, 5).Value = (double)d.MargenTotal;
                    ws.Cell(fila, 6).Value = d.MargenPorcentaje / 100;

                    ws.Cell(fila, 4).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 5).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 6).Style.NumberFormat.Format = "0.0%";
                    fila++;
                }

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Top Productos Más Vendidos", desde, hasta);
            });
        }

        public static void ExportarMetodosPago(IEnumerable<ReporteMetodosPagoDto> datos, DateTime desde, DateTime hasta)
        {
            Exportar("Métodos de Pago", $"MetodosPago_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Métodos de Pago");
                var headers = new[] { "Método", "Total", "Cantidad", "Porcentaje" };
                AgregarHeaders(ws, headers);

                int fila = 2;
                foreach (var d in datos)
                {
                    ws.Cell(fila, 1).Value = d.Metodo;
                    ws.Cell(fila, 2).Value = (double)d.Total;
                    ws.Cell(fila, 3).Value = d.Cantidad;
                    ws.Cell(fila, 4).Value = d.Porcentaje / 100;

                    ws.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 4).Style.NumberFormat.Format = "0.0%";
                    fila++;
                }

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Resumen Métodos de Pago", desde, hasta);
            });
        }

        public static void ExportarAuditoria(
            IEnumerable<AuditoriaLogDto> auditoriaCajas,
            IEnumerable<AuditoriaLogDto> auditoriaMovimientos,
            DateTime desde,
            DateTime hasta)
        {
            Exportar("Auditoría", $"Auditoria_{Fecha()}", wb =>
            {
                // ── Hoja 1: Auditoría de Cajas ──────────────────────────────
                var wsCajas = wb.Worksheets.Add("Auditoría Cajas");
                var headersCajas = new[]
                {
                    "Fecha", "Usuario", "Operación", "Caja",
                    "Monto", "Valor Anterior", "Valor Nuevo", "Diferencia", "Cambios"
                };
                AgregarHeaders(wsCajas, headersCajas);

                int filaC = 2;
                foreach (var d in auditoriaCajas)
                {
                    wsCajas.Cell(filaC, 1).Value = d.FechaOperacion.ToString("dd/MM/yyyy HH:mm");
                    wsCajas.Cell(filaC, 2).Value = d.Usuario;
                    wsCajas.Cell(filaC, 3).Value = d.TipoOperacionCaja;
                    wsCajas.Cell(filaC, 4).Value = d.NumeroCaja ?? "—";

                    if (d.MontoMostrar.HasValue) wsCajas.Cell(filaC, 5).Value = (double)d.MontoMostrar.Value; else wsCajas.Cell(filaC, 5).Value = "—";
                    if (d.ValorAnteriorMostrar.HasValue) wsCajas.Cell(filaC, 6).Value = (double)d.ValorAnteriorMostrar.Value; else wsCajas.Cell(filaC, 6).Value = "—";
                    if (d.ValorNuevoMostrar.HasValue) wsCajas.Cell(filaC, 7).Value = (double)d.ValorNuevoMostrar.Value; else wsCajas.Cell(filaC, 7).Value = "—";
                    if (d.DiferenciaMonetaria.HasValue) wsCajas.Cell(filaC, 8).Value = (double)d.DiferenciaMonetaria.Value; else wsCajas.Cell(filaC, 8).Value = "—";

                    wsCajas.Cell(filaC, 9).Value = d.DetalleCambios;

                    wsCajas.Cell(filaC, 5).Style.NumberFormat.Format = "$ #,##0";
                    wsCajas.Cell(filaC, 6).Style.NumberFormat.Format = "$ #,##0";
                    wsCajas.Cell(filaC, 7).Style.NumberFormat.Format = "$ #,##0";
                    wsCajas.Cell(filaC, 8).Style.NumberFormat.Format = "$ #,##0";

                    // Color rojo para diferencias sospechosas
                    if (d.EsDiferenciaSospechosa)
                        wsCajas.Cell(filaC, 8).Style.Font.FontColor = XLColor.Red;

                    filaC++;
                }

                FormatearHoja(wsCajas, headersCajas.Length);
                AgregarMetadatos(wsCajas, "Auditoría de Cajas", desde, hasta);

                // ── Hoja 2: Auditoría de Movimientos ─────────────────────────
                var wsMovs = wb.Worksheets.Add("Auditoría Movimientos");
                var headersMovs = new[]
                {
                    "Fecha", "Usuario", "Operación", "Caja",
                    "Monto", "Valor Anterior", "Valor Nuevo", "Diferencia", "Cambios"
                };
                AgregarHeaders(wsMovs, headersMovs);

                int filaM = 2;
                foreach (var d in auditoriaMovimientos)
                {
                    wsMovs.Cell(filaM, 1).Value = d.FechaOperacion.ToString("dd/MM/yyyy HH:mm");
                    wsMovs.Cell(filaM, 2).Value = d.Usuario;
                    wsMovs.Cell(filaM, 3).Value = d.TipoOperacionCaja;
                    wsMovs.Cell(filaM, 4).Value = d.NumeroCaja ?? "—";

                    if (d.MontoMostrar.HasValue) wsMovs.Cell(filaM, 5).Value = (double)d.MontoMostrar.Value; else wsMovs.Cell(filaM, 5).Value = "—";
                    if (d.ValorAnteriorMostrar.HasValue) wsMovs.Cell(filaM, 6).Value = (double)d.ValorAnteriorMostrar.Value; else wsMovs.Cell(filaM, 6).Value = "—";
                    if (d.ValorNuevoMostrar.HasValue) wsMovs.Cell(filaM, 7).Value = (double)d.ValorNuevoMostrar.Value; else wsMovs.Cell(filaM, 7).Value = "—";
                    if (d.DiferenciaMonetaria.HasValue) wsMovs.Cell(filaM, 8).Value = (double)d.DiferenciaMonetaria.Value; else wsMovs.Cell(filaM, 8).Value = "—";

                    wsMovs.Cell(filaM, 9).Value = d.DetalleCambios;

                    wsMovs.Cell(filaM, 5).Style.NumberFormat.Format = "$ #,##0";
                    wsMovs.Cell(filaM, 6).Style.NumberFormat.Format = "$ #,##0";
                    wsMovs.Cell(filaM, 7).Style.NumberFormat.Format = "$ #,##0";
                    wsMovs.Cell(filaM, 8).Style.NumberFormat.Format = "$ #,##0";

                    if (d.EsDiferenciaSospechosa)
                        wsMovs.Cell(filaM, 8).Style.Font.FontColor = XLColor.Red;

                    filaM++;
                }

                FormatearHoja(wsMovs, headersMovs.Length);
                AgregarMetadatos(wsMovs, "Auditoría de Movimientos", desde, hasta);
            });
        }

        public static void ExportarCierre(ResumenCierreDto resumen)
        {
            Exportar("Informe de Cierre", $"CierreCaja_{resumen.FechaApertura:yyyyMMdd_HHmm}", wb =>
            {
                var ws = wb.Worksheets.Add("Informe de Cierre");

                // ── Encabezado del informe ─────────────────────────────────────
                ws.Cell(1, 1).Value = "INFORME DE CIERRE DE CAJA";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 16;
                ws.Range(1, 1, 1, 3).Merge();
                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // ── Datos del turno ─────────────────────────────────────────────
                ws.Cell(3, 1).Value = "Fecha de Apertura:";
                ws.Cell(3, 2).Value = resumen.FechaApertura.ToString("dd/MM/yyyy HH:mm");
                ws.Cell(3, 1).Style.Font.Bold = true;

                ws.Cell(4, 1).Value = "Fecha de Cierre:";
                ws.Cell(4, 2).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ws.Cell(4, 1).Style.Font.Bold = true;

                ws.Cell(5, 1).Value = "Transacciones:";
                ws.Cell(5, 2).Value = resumen.CantidadVentas;
                ws.Cell(5, 1).Style.Font.Bold = true;

                // ── Sección Efectivo ───────────────────────────────────────────
                ws.Cell(7, 1).Value = "MOVIMIENTOS DE EFECTIVO";
                ws.Cell(7, 1).Style.Font.Bold = true;
                ws.Cell(7, 1).Style.Font.FontSize = 12;
                ws.Cell(7, 1).Style.Font.FontColor = XLColor.FromHtml("#1E3A5F");

                var headersEfectivo = new[] { "Concepto", "Monto" };
                AgregarHeaders(ws, headersEfectivo);
                ws.Cell(8, 1).Value = "Monto Inicial";
                ws.Cell(8, 2).Value = (double)resumen.MontoInicial;
                ws.Cell(8, 2).Style.NumberFormat.Format = "$ #,##0";

                ws.Cell(9, 1).Value = "Ventas en Efectivo";
                ws.Cell(9, 2).Value = (double)resumen.VentasEfectivo;
                ws.Cell(9, 2).Style.NumberFormat.Format = "$ #,##0";

                ws.Cell(10, 1).Value = "Ingresos";
                ws.Cell(10, 2).Value = (double)resumen.IngresosEfectivo;
                ws.Cell(10, 2).Style.NumberFormat.Format = "$ #,##0";

                ws.Cell(11, 1).Value = "Egresos";
                ws.Cell(11, 2).Value = (double)resumen.EgresosEfectivo;
                ws.Cell(11, 2).Style.NumberFormat.Format = "$ #,##0";

                ws.Cell(12, 1).Value = "SALDO ESPERADO";
                ws.Cell(12, 1).Style.Font.Bold = true;
                ws.Cell(12, 2).Value = (double)resumen.SaldoEsperado;
                ws.Cell(12, 2).Style.NumberFormat.Format = "$ #,##0";
                ws.Cell(12, 2).Style.Font.Bold = true;
                ws.Cell(12, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");

                // ── Sección Otros Métodos ───────────────────────────────────────
                ws.Cell(14, 1).Value = "OTROS MÉTODOS DE PAGO";
                ws.Cell(14, 1).Style.Font.Bold = true;
                ws.Cell(14, 1).Style.Font.FontSize = 12;
                ws.Cell(14, 1).Style.Font.FontColor = XLColor.FromHtml("#1E3A5F");

                var headersOtros = new[] { "Método", "Total" };
                AgregarHeaders(ws, headersOtros);
                int filaOtros = 15;

                if (resumen.VentasTarjeta > 0)
                {
                    ws.Cell(filaOtros, 1).Value = "Tarjeta";
                    ws.Cell(filaOtros, 2).Value = (double)resumen.VentasTarjeta;
                    ws.Cell(filaOtros, 2).Style.NumberFormat.Format = "$ #,##0";
                    filaOtros++;
                }
                if (resumen.VentasTransferencia > 0)
                {
                    ws.Cell(filaOtros, 1).Value = "Transferencia";
                    ws.Cell(filaOtros, 2).Value = (double)resumen.VentasTransferencia;
                    ws.Cell(filaOtros, 2).Style.NumberFormat.Format = "$ #,##0";
                    filaOtros++;
                }
                if (resumen.VentasQR > 0)
                {
                    ws.Cell(filaOtros, 1).Value = "QR";
                    ws.Cell(filaOtros, 2).Value = (double)resumen.VentasQR;
                    ws.Cell(filaOtros, 2).Style.NumberFormat.Format = "$ #,##0";
                    filaOtros++;
                }
                if (resumen.VentasCuentaCte > 0)
                {
                    ws.Cell(filaOtros, 1).Value = "Cuenta Corriente";
                    ws.Cell(filaOtros, 2).Value = (double)resumen.VentasCuentaCte;
                    ws.Cell(filaOtros, 2).Style.NumberFormat.Format = "$ #,##0";
                    filaOtros++;
                }
                if (resumen.VentasOtros > 0)
                {
                    ws.Cell(filaOtros, 1).Value = "Otros";
                    ws.Cell(filaOtros, 2).Value = (double)resumen.VentasOtros;
                    ws.Cell(filaOtros, 2).Style.NumberFormat.Format = "$ #,##0";
                    filaOtros++;
                }

                // ── Total General ───────────────────────────────────────────────
                ws.Cell(filaOtros + 1, 1).Value = "TOTAL VENDIDO";
                ws.Cell(filaOtros + 1, 1).Style.Font.Bold = true;
                ws.Cell(filaOtros + 1, 1).Style.Font.FontSize = 12;
                ws.Cell(filaOtros + 1, 2).Value = (double)resumen.TotalVendido;
                ws.Cell(filaOtros + 1, 2).Style.NumberFormat.Format = "$ #,##0";
                ws.Cell(filaOtros + 1, 2).Style.Font.Bold = true;
                ws.Cell(filaOtros + 1, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#E3F2FD");

                // ── Desglose detallado ─────────────────────────────────────────
                if (resumen.DesglosePorMetodo != null && resumen.DesglosePorMetodo.Count > 0)
                {
                    int filaDesglose = filaOtros + 3;
                    ws.Cell(filaDesglose, 1).Value = "DETALLE POR MÉTODO";
                    ws.Cell(filaDesglose, 1).Style.Font.Bold = true;
                    ws.Cell(filaDesglose, 1).Style.Font.FontSize = 12;
                    ws.Cell(filaDesglose, 1).Style.Font.FontColor = XLColor.FromHtml("#1E3A5F");

                    var headersDetalle = new[] { "Método", "Total", "Cantidad" };
                    AgregarHeaders(ws, headersDetalle);
                    filaDesglose++;

                    foreach (var item in resumen.DesglosePorMetodo)
                    {
                        ws.Cell(filaDesglose, 1).Value = item.Metodo;
                        ws.Cell(filaDesglose, 2).Value = (double)item.Total;
                        ws.Cell(filaDesglose, 3).Value = item.Cantidad > 0 ? item.Cantidad : 1;
                        ws.Cell(filaDesglose, 2).Style.NumberFormat.Format = "$ #,##0";
                        filaDesglose++;
                    }
                }

                // ── Formatear ───────────────────────────────────────────────────
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 18;
                ws.Column(3).Width = 12;

                // ── Metadatos al pie ───────────────────────────────────────────
                int ultimaFila = (ws.RangeUsed()?.LastRowUsed()?.RowNumber() ?? 1) + 2;
                ws.Cell(ultimaFila, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
                ws.Cell(ultimaFila, 1).Style.Font.Italic = true;
                ws.Cell(ultimaFila, 1).Style.Font.FontColor = XLColor.Gray;
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
