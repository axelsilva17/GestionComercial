// NOTA: Este archivo requiere el paquete NuGet ClosedXML.
// Install-Package ClosedXML
// Una vez instalado, eliminá el #if y el #endif al final.

#if true  // ← Habilitado para exportación

using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Reportes;
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
            ExportarTopProductosCompleto(datos, desde, hasta);
        }

        public static void ExportarTopProductosCompleto(IEnumerable<ReporteTopProductoDto> datos, DateTime desde, DateTime hasta, bool shouldOpenAfterDownload = false)
        {
            Exportar("Top Productos", $"TopProductos_{Fecha()}", wb =>
            {
                var ws = wb.Worksheets.Add("Top Productos");
                var headers = new[] { "#", "Producto", "Código", "Categoría", "Cant. Vendida", "Precio Unit.", "Subtotal", "Descuentos", "Margen Unit.", "Margen Total", "Margen %" };
                AgregarHeaders(ws, headers);

                // Convertir a lista para poder agregar ranking
                var datosList = datos.ToList();

                int fila = 2;
                int ranking = 1;
                decimal totalIngresos = 0;
                decimal totalDescuentos = 0;
                decimal totalMargen = 0;

                foreach (var d in datosList)
                {
                    // Calcular valores derivados
                    var precioUnitario = d.CantidadVendida > 0 ? d.Ingresos / d.CantidadVendida : 0;
                    var subtotal = d.Ingresos;
                    var descuento = 0m; // Por ahora 0, luego puede venir del DTO
                    var margenUnitario = d.CantidadVendida > 0 ? d.MargenTotal / d.CantidadVendida : 0;

                    ws.Cell(fila, 1).Value = ranking;
                    ws.Cell(fila, 2).Value = d.ProductoNombre;
                    ws.Cell(fila, 3).Value = d.IdProducto.ToString(); // Código producto
                    ws.Cell(fila, 4).Value = d.Categoria;
                    ws.Cell(fila, 5).Value = d.CantidadVendida;
                    ws.Cell(fila, 6).Value = (double)precioUnitario;
                    ws.Cell(fila, 7).Value = (double)subtotal;
                    ws.Cell(fila, 8).Value = (double)descuento;
                    ws.Cell(fila, 9).Value = (double)margenUnitario;
                    ws.Cell(fila, 10).Value = (double)d.MargenTotal;
                    ws.Cell(fila, 11).Value = d.MargenPorcentaje / 100;

                    ws.Cell(fila, 6).Style.NumberFormat.Format = "$ #,##0.00";
                    ws.Cell(fila, 7).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 8).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 9).Style.NumberFormat.Format = "$ #,##0.00";
                    ws.Cell(fila, 10).Style.NumberFormat.Format = "$ #,##0";
                    ws.Cell(fila, 11).Style.NumberFormat.Format = "0.0%";

                    totalIngresos += subtotal;
                    totalDescuentos += descuento;
                    totalMargen += d.MargenTotal;
                    ranking++;
                    fila++;
                }

                // Fila de totales
                ws.Cell(fila, 1).Value = "";
                ws.Cell(fila, 2).Value = "TOTALES:";
                ws.Cell(fila, 2).Style.Font.Bold = true;
                ws.Cell(fila, 5).Value = datosList.Sum(d => d.CantidadVendida);
                ws.Cell(fila, 7).Value = (double)totalIngresos;
                ws.Cell(fila, 8).Value = (double)totalDescuentos;
                ws.Cell(fila, 10).Value = (double)totalMargen;
                
                // Calcular margen total %
                var margenTotalPorcentaje = totalIngresos > 0 ? (double)(totalMargen / totalIngresos * 100) : 0;
                ws.Cell(fila, 11).Value = margenTotalPorcentaje / 100;

                ws.Cell(fila, 7).Style.NumberFormat.Format = "$ #,##0";
                ws.Cell(fila, 8).Style.NumberFormat.Format = "$ #,##0";
                ws.Cell(fila, 10).Style.NumberFormat.Format = "$ #,##0";
                ws.Cell(fila, 11).Style.NumberFormat.Format = "0.0%";
                
                // Estilar fila de totales
                ws.Cell(fila, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");
                ws.Cell(fila, 5).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");
                ws.Cell(fila, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");
                ws.Cell(fila, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");
                ws.Cell(fila, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");
                ws.Cell(fila, 11).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");

                for (int c = 1; c <= 11; c++)
                    ws.Cell(fila, c).Style.Font.Bold = true;

                FormatearHoja(ws, headers.Length);
                AgregarMetadatos(ws, "Top Productos Más Vendidos", desde, hasta);
            }, shouldOpenAfterDownload);
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

        // Nuevo: exportar reporte de gerencia completo a un solo archivo
        public static void ExportarReporteGerenciaCompleto(
            IEnumerable<VentaPorDiaDto> ventaPorDia,
            IEnumerable<ReporteMargenDto> margen,
            IEnumerable<ReporteTopProductoDto> topProductos,
            IEnumerable<ReporteVendedorDto> vendedores,
            IEnumerable<ReporteRotacionDto> rotacion,
            IEnumerable<ReporteMetodosPagoDto> metodosPago,
            DateTime desde,
            DateTime hasta,
            decimal ventasAcumuladas = 0,
            decimal comprasAcumuladas = 0,
            decimal resultadoNeto = 0,
            double margenPromedio = 0,
            IEnumerable<ReporteVentaMensualDto>? ventasMensuales = null,
            IEnumerable<MetodosPagoMesDto>? metodosPagoMensual = null)
        {
            Exportar("Reporte Gerencia", $"ReporteGerencia_{Fecha()}", wb =>
            {
                // ── Hoja 0: Resumen KPIs ─────────────────────────────────────────
                var wsResumen = wb.Worksheets.Add("Resumen");
                wsResumen.Cell(1, 1).Value = "REPORTE DE GERENCIA";
                wsResumen.Cell(1, 1).Style.Font.Bold = true;
                wsResumen.Cell(1, 1).Style.Font.FontSize = 16;
                wsResumen.Range(1, 1, 1, 3).Merge();
                wsResumen.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                wsResumen.Cell(3, 1).Value = "Período:";
                wsResumen.Cell(3, 2).Value = $"{desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}";
                wsResumen.Cell(3, 1).Style.Font.Bold = true;

                wsResumen.Cell(5, 1).Value = "INDICADORES CLAVE";
                wsResumen.Cell(5, 1).Style.Font.Bold = true;
                wsResumen.Cell(5, 1).Style.Font.FontSize = 12;

                wsResumen.Cell(6, 1).Value = "Ventas Acumuladas:";
                wsResumen.Cell(6, 2).Value = (double)ventasAcumuladas;
                wsResumen.Cell(6, 2).Style.NumberFormat.Format = "$ #,##0";

                wsResumen.Cell(7, 1).Value = "Compras Acumuladas:";
                wsResumen.Cell(7, 2).Value = (double)comprasAcumuladas;
                wsResumen.Cell(7, 2).Style.NumberFormat.Format = "$ #,##0";

                wsResumen.Cell(8, 1).Value = "Resultado Neto:";
                wsResumen.Cell(8, 2).Value = (double)resultadoNeto;
                wsResumen.Cell(8, 2).Style.NumberFormat.Format = "$ #,##0";
                if (resultadoNeto < 0)
                    wsResumen.Cell(8, 2).Style.Font.FontColor = XLColor.Red;
                else
                    wsResumen.Cell(8, 2).Style.Font.FontColor = XLColor.Green;

                wsResumen.Cell(9, 1).Value = "Margen Promedio:";
                wsResumen.Cell(9, 2).Value = margenPromedio / 100;
                wsResumen.Cell(9, 2).Style.NumberFormat.Format = "0.0%";

                wsResumen.Cell(10, 1).Value = "Total Ventas (registros):";
                wsResumen.Cell(10, 2).Value = ventaPorDia.Sum(v => v.Cantidad);

                wsResumen.Cell(11, 1).Value = "Ticket Promedio:";
                var totalVentas = ventaPorDia.Sum(v => v.Total);
                var totalCantidad = ventaPorDia.Sum(v => v.Cantidad);
                wsResumen.Cell(11, 2).Value = totalCantidad > 0 ? (double)(totalVentas / totalCantidad) : 0;
                wsResumen.Cell(11, 2).Style.NumberFormat.Format = "$ #,##0";

                // Formato
                wsResumen.Column(1).Width = 25;
                wsResumen.Column(2).Width = 18;
                wsResumen.Range(6, 1, 11, 1).Style.Font.Bold = true;

                // ── Hoja 1: Ventas Mensuales (si se proporcionan) ────────────────
                if (ventasMensuales != null && ventasMensuales.Any())
                {
                    var wsMensual = wb.Worksheets.Add("Ventas Mensuales");
                    var headersMensual = new[] { "Mes", "Ventas", "Compras", "Resultado", "Margen %" };
                    AgregarHeaders(wsMensual, headersMensual);
                    int filaMen = 2;
                    foreach (var m in ventasMensuales)
                    {
                        wsMensual.Cell(filaMen, 1).Value = m.Mes;
                        wsMensual.Cell(filaMen, 2).Value = (double)m.Ventas;
                        wsMensual.Cell(filaMen, 3).Value = (double)m.Compras;
                        wsMensual.Cell(filaMen, 4).Value = (double)m.Resultado;
                        wsMensual.Cell(filaMen, 5).Value = m.Margen / 100;
                        wsMensual.Cell(filaMen, 2).Style.NumberFormat.Format = "$ #,##0";
                        wsMensual.Cell(filaMen, 3).Style.NumberFormat.Format = "$ #,##0";
                        wsMensual.Cell(filaMen, 4).Style.NumberFormat.Format = "$ #,##0";
                        wsMensual.Cell(filaMen, 5).Style.NumberFormat.Format = "0.0%";
                        filaMen++;
                    }
                    FormatearHoja(wsMensual, headersMensual.Length);
                    AgregarMetadatos(wsMensual, "Ventas Mensuales", desde, hasta);
                }

                // Hoja 1: Ventas por Día
                var wsVentas = wb.Worksheets.Add("Ventas por Día");
                var headersVentas = new[] { "Día", "Total Ventas", "Cantidad" };
                AgregarHeaders(wsVentas, headersVentas);
                int filaV = 2;
                foreach (var d in ventaPorDia)
                {
                    wsVentas.Cell(filaV, 1).Value = d.Dia;
                    wsVentas.Cell(filaV, 2).Value = (double)d.Total;
                    wsVentas.Cell(filaV, 3).Value = d.Cantidad;
                    wsVentas.Cell(filaV, 2).Style.NumberFormat.Format = "$ #,##0";
                    filaV++;
                }
                FormatearHoja(wsVentas, headersVentas.Length);
                AgregarMetadatos(wsVentas, "Ventas por Día", desde, hasta);

                // Hoja 2: Margen
                var wsMargen = wb.Worksheets.Add("Margen");
                var headersMargen = new[] { "Producto", "Categoría", "Costo", "Precio Venta", "Margen Unit.", "Margen %", "Cant. Vendida", "Ganancia Total" };
                AgregarHeaders(wsMargen, headersMargen);
                int filaM = 2;
                foreach (var d in margen)
                {
                    wsMargen.Cell(filaM, 1).Value = d.ProductoNombre;
                    wsMargen.Cell(filaM, 2).Value = d.Categoria;
                    wsMargen.Cell(filaM, 3).Value = (double)d.PrecioCosto;
                    wsMargen.Cell(filaM, 4).Value = (double)d.PrecioVenta;
                    wsMargen.Cell(filaM, 5).Value = (double)d.MargenUnitario;
                    wsMargen.Cell(filaM, 6).Value = (double)d.MargenPorcentaje / 100;
                    wsMargen.Cell(filaM, 7).Value = d.CantidadVendida;
                    wsMargen.Cell(filaM, 8).Value = (double)d.MargenTotal;
                    wsMargen.Cell(filaM, 3).Style.NumberFormat.Format = "$ #,##0";
                    wsMargen.Cell(filaM, 4).Style.NumberFormat.Format = "$ #,##0";
                    wsMargen.Cell(filaM, 5).Style.NumberFormat.Format = "$ #,##0";
                    wsMargen.Cell(filaM, 6).Style.NumberFormat.Format = "0.0%";
                    wsMargen.Cell(filaM, 8).Style.NumberFormat.Format = "$ #,##0";
                    filaM++;
                }
                FormatearHoja(wsMargen, headersMargen.Length);
                AgregarMetadatos(wsMargen, "Margen por Producto", desde, hasta);

                // Hoja 3: Top Productos
                var wsTop = wb.Worksheets.Add("Top Productos");
                var headersTop = new[] { "#", "Producto", "Código", "Categoría", "Cant. Vendida", "Precio Unit.", "Subtotal", "Margen Total", "Margen %" };
                AgregarHeaders(wsTop, headersTop);
                int filaTop = 2;
                int ranking = 1;
                foreach (var d in topProductos)
                {
                    var precioUnitario = d.CantidadVendida > 0 ? d.Ingresos / d.CantidadVendida : 0;
                    wsTop.Cell(filaTop, 1).Value = ranking;
                    wsTop.Cell(filaTop, 2).Value = d.ProductoNombre;
                    wsTop.Cell(filaTop, 3).Value = d.IdProducto.ToString();
                    wsTop.Cell(filaTop, 4).Value = d.Categoria;
                    wsTop.Cell(filaTop, 5).Value = d.CantidadVendida;
                    wsTop.Cell(filaTop, 6).Value = (double)precioUnitario;
                    wsTop.Cell(filaTop, 7).Value = (double)d.Ingresos;
                    wsTop.Cell(filaTop, 8).Value = (double)d.MargenTotal;
                    wsTop.Cell(filaTop, 9).Value = d.MargenPorcentaje / 100;
                    wsTop.Cell(filaTop, 6).Style.NumberFormat.Format = "$ #,##0.00";
                    wsTop.Cell(filaTop, 7).Style.NumberFormat.Format = "$ #,##0";
                    wsTop.Cell(filaTop, 8).Style.NumberFormat.Format = "$ #,##0";
                    wsTop.Cell(filaTop, 9).Style.NumberFormat.Format = "0.0%";
                    ranking++;
                    filaTop++;
                }
                FormatearHoja(wsTop, headersTop.Length);
                AgregarMetadatos(wsTop, "Top Productos", desde, hasta);

                // Hoja 4: Vendedores
                var wsVendedores = wb.Worksheets.Add("Vendedores");
                var headersVend = new[] { "Vendedor", "Sucursal", "Cant. Ventas", "Total Vendido", "Ticket Promedio", "Descuentos" };
                AgregarHeaders(wsVendedores, headersVend);
                int filaVend = 2;
                foreach (var d in vendedores)
                {
                    wsVendedores.Cell(filaVend, 1).Value = d.UsuarioNombre;
                    wsVendedores.Cell(filaVend, 2).Value = d.Sucursal;
                    wsVendedores.Cell(filaVend, 3).Value = d.CantidadVentas;
                    wsVendedores.Cell(filaVend, 4).Value = (double)d.TotalVendido;
                    wsVendedores.Cell(filaVend, 5).Value = (double)d.PromedioVenta;
                    wsVendedores.Cell(filaVend, 6).Value = (double)d.TotalDescuentos;
                    wsVendedores.Cell(filaVend, 4).Style.NumberFormat.Format = "$ #,##0";
                    wsVendedores.Cell(filaVend, 5).Style.NumberFormat.Format = "$ #,##0";
                    wsVendedores.Cell(filaVend, 6).Style.NumberFormat.Format = "$ #,##0";
                    filaVend++;
                }
                FormatearHoja(wsVendedores, headersVend.Length);
                AgregarMetadatos(wsVendedores, "Rendimiento Vendedores", desde, hasta);

                // Hoja 5: Rotación
                var wsRotacion = wb.Worksheets.Add("Rotación");
                var headersRot = new[] { "Producto", "Categoría", "Stock Actual", "Cant. Vendida", "Cant. Comprada", "Índice Rotación", "Última Venta", "Última Compra" };
                AgregarHeaders(wsRotacion, headersRot);
                int filaRot = 2;
                foreach (var d in rotacion)
                {
                    wsRotacion.Cell(filaRot, 1).Value = d.ProductoNombre;
                    wsRotacion.Cell(filaRot, 2).Value = d.Categoria;
                    wsRotacion.Cell(filaRot, 3).Value = d.StockActual;
                    wsRotacion.Cell(filaRot, 4).Value = d.CantidadVendida;
                    wsRotacion.Cell(filaRot, 5).Value = d.CantidadComprada;
                    wsRotacion.Cell(filaRot, 6).Value = (double)d.IndiceRotacion;
                    wsRotacion.Cell(filaRot, 7).Value = d.UltimaVenta != DateTime.MinValue ? d.UltimaVenta.ToString("dd/MM/yyyy") : "-";
                    wsRotacion.Cell(filaRot, 8).Value = d.UltimaCompra != DateTime.MinValue ? d.UltimaCompra.ToString("dd/MM/yyyy") : "-";
                    wsRotacion.Cell(filaRot, 6).Style.NumberFormat.Format = "0.0";
                    filaRot++;
                }
                FormatearHoja(wsRotacion, headersRot.Length);
                AgregarMetadatos(wsRotacion, "Rotación de Stock", desde, hasta);

                // Hoja 6: Métodos de Pago
                var wsMetodos = wb.Worksheets.Add("Métodos de Pago");
                var headersMet = new[] { "Método", "Total", "Cantidad", "Porcentaje" };
                AgregarHeaders(wsMetodos, headersMet);
                int filaMet = 2;
                foreach (var d in metodosPago)
                {
                    wsMetodos.Cell(filaMet, 1).Value = d.Metodo;
                    wsMetodos.Cell(filaMet, 2).Value = (double)d.Total;
                    wsMetodos.Cell(filaMet, 3).Value = d.Cantidad;
                    wsMetodos.Cell(filaMet, 4).Value = d.Porcentaje / 100;
                    wsMetodos.Cell(filaMet, 2).Style.NumberFormat.Format = "$ #,##0";
                    wsMetodos.Cell(filaMet, 4).Style.NumberFormat.Format = "0.0%";
                    filaMet++;
                }
                FormatearHoja(wsMetodos, headersMet.Length);
                AgregarMetadatos(wsMetodos, "Métodos de Pago", desde, hasta);

                // ── Hoja 7: Distribución Mensual de Métodos de Pago (si se proporciona) ─
                if (metodosPagoMensual != null && metodosPagoMensual.Any())
                {
                    var wsMetodosMensual = wb.Worksheets.Add("Metodos Pago x Mes");
                    var headersMetMen = new[] { "Mes", "Método", "Total", "Cant." };
                    AgregarHeaders(wsMetodosMensual, headersMetMen);
                    int filaMetMen = 2;
                    foreach (var m in metodosPagoMensual)
                    {
                        wsMetodosMensual.Cell(filaMetMen, 1).Value = m.Mes;
                        wsMetodosMensual.Cell(filaMetMen, 2).Value = m.Metodo;
                        wsMetodosMensual.Cell(filaMetMen, 3).Value = (double)m.Total;
                        wsMetodosMensual.Cell(filaMetMen, 4).Value = m.Cantidad;
                        wsMetodosMensual.Cell(filaMetMen, 3).Style.NumberFormat.Format = "$ #,##0";
                        filaMetMen++;
                    }
                    FormatearHoja(wsMetodosMensual, headersMetMen.Length);
                    AgregarMetadatos(wsMetodosMensual, "Distribución Mensual de Métodos de Pago", desde, hasta);
                }
            });
        }

        public static void ExportarAuditoria(
            IEnumerable<AuditoriaLogDto> auditoriaCajas,
            IEnumerable<AuditoriaLogDto> auditoriaMovimientos,
            DateTime desde,
            DateTime hasta)
        {
            ExportarAuditoriaCompleto(auditoriaCajas, auditoriaMovimientos, null, null, desde, hasta);
        }

        public static void ExportarAuditoriaCompleto(
            IEnumerable<AuditoriaLogDto> auditoriaCajas,
            IEnumerable<AuditoriaLogDto> auditoriaMovimientos,
            IEnumerable<GestionComercial.UI.ViewModels.Reportes.CajaHistorialDto>? historialCajas,
            IEnumerable<GestionComercial.UI.ViewModels.Reportes.VentaResumenCajaDto>? ventasPorCaja,
            DateTime desde,
            DateTime hasta,
            bool shouldOpenAfterDownload = false)
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

                // ── Hoja 3: Resumen de Cajas (nueva) ───────────────────────────
                if (historialCajas != null && historialCajas.Any())
                {
                    var wsResumen = wb.Worksheets.Add("Resumen Cajas");
                    var headersResumen = new[]
                    {
                        "Caja", "Fecha Apertura", "Fecha Cierre", "Monto Inicial", "Monto Final", "Diferencia", "Tipo Diferencia", "Usuario Apertura", "Usuario Cierre", "Estado"
                    };
                    AgregarHeaders(wsResumen, headersResumen);

                    int filaR = 2;
                    decimal totalDiferencia = 0;
                    foreach (var c in historialCajas)
                    {
                        wsResumen.Cell(filaR, 1).Value = $"Caja {c.Id}";
                        wsResumen.Cell(filaR, 2).Value = c.FechaApertura;
                        wsResumen.Cell(filaR, 3).Value = c.FechaCierre ?? "—";
                        wsResumen.Cell(filaR, 4).Value = (double)c.MontoInicial;
                        wsResumen.Cell(filaR, 5).Value = c.MontoFinal.HasValue ? (double)c.MontoFinal.Value : 0;
                        wsResumen.Cell(filaR, 6).Value = c.Diferencia.HasValue ? (double)c.Diferencia.Value : 0;
                        wsResumen.Cell(filaR, 7).Value = c.TipoDiferencia;
                        wsResumen.Cell(filaR, 8).Value = c.UsuarioApertura;
                        wsResumen.Cell(filaR, 9).Value = c.UsuarioCierre ?? "—";
                        wsResumen.Cell(filaR, 10).Value = c.Estado;

                        wsResumen.Cell(filaR, 4).Style.NumberFormat.Format = "$ #,##0";
                        wsResumen.Cell(filaR, 5).Style.NumberFormat.Format = "$ #,##0";
                        wsResumen.Cell(filaR, 6).Style.NumberFormat.Format = "$ #,##0";

                        // Color por tipo de diferencia
                        if (c.TipoDiferencia == "Negativo")
                            wsResumen.Cell(filaR, 6).Style.Font.FontColor = XLColor.Red;
                        else if (c.TipoDiferencia == "Positivo")
                            wsResumen.Cell(filaR, 6).Style.Font.FontColor = XLColor.Green;

                        if (c.Diferencia.HasValue) totalDiferencia += c.Diferencia.Value;
                        filaR++;
                    }

                    // Fila de totales
                    wsResumen.Cell(filaR, 5).Value = "TOTALES:";
                    wsResumen.Cell(filaR, 5).Style.Font.Bold = true;
                    wsResumen.Cell(filaR, 6).Value = (double)totalDiferencia;
                    wsResumen.Cell(filaR, 6).Style.NumberFormat.Format = "$ #,##0";
                    wsResumen.Cell(filaR, 6).Style.Font.Bold = true;

                    FormatearHoja(wsResumen, headersResumen.Length);
                    AgregarMetadatos(wsResumen, "Resumen de Cajas", desde, hasta);
                }

                // ── Hoja 4: Ventas por Caja (nueva) ───────────────────────────
                if (ventasPorCaja != null && ventasPorCaja.Any())
                {
                    var wsVentas = wb.Worksheets.Add("Ventas por Caja");
                    var headersVentas = new[]
                    {
                        "Caja", "Fecha Venta", "Total", "Método Pago", "Vendedor"
                    };
                    AgregarHeaders(wsVentas, headersVentas);

                    int filaV = 2;
                    decimal totalVentas = 0;
                    foreach (var v in ventasPorCaja)
                    {
                        wsVentas.Cell(filaV, 1).Value = v.NumeroCaja;
                        wsVentas.Cell(filaV, 2).Value = v.FechaVenta;
                        wsVentas.Cell(filaV, 3).Value = (double)v.Total;
                        wsVentas.Cell(filaV, 4).Value = v.MetodoPago;
                        wsVentas.Cell(filaV, 5).Value = v.Vendedor;

                        wsVentas.Cell(filaV, 3).Style.NumberFormat.Format = "$ #,##0";
                        totalVentas += v.Total;
                        filaV++;
                    }

                    // Fila de totales
                    wsVentas.Cell(filaV, 2).Value = "TOTALES:";
                    wsVentas.Cell(filaV, 2).Style.Font.Bold = true;
                    wsVentas.Cell(filaV, 3).Value = (double)totalVentas;
                    wsVentas.Cell(filaV, 3).Style.NumberFormat.Format = "$ #,##0";
                    wsVentas.Cell(filaV, 3).Style.Font.Bold = true;

                    FormatearHoja(wsVentas, headersVentas.Length);
                AgregarMetadatos(wsVentas, "Ventas por Caja", desde, hasta);
                }
            }, shouldOpenAfterDownload);
        }

        // ── Exportar Informe Admin Completo ─────────────────────────────────────
        public static void ExportarInformeAdmin(
            IEnumerable<AuditoriaLogDto> auditoriaCajas,
            IEnumerable<AuditoriaLogDto> auditoriaMovimientos,
            IEnumerable<GestionComercial.UI.ViewModels.Reportes.CajaHistorialDto>? historialCajas,
            IEnumerable<GestionComercial.UI.ViewModels.Reportes.VentaResumenCajaDto>? ventasPorCaja,
            GestionComercial.UI.ViewModels.Reportes.ReporteAdminViewModel.ResumenAdminKpiDto? kpis,
            IEnumerable<GestionComercial.UI.ViewModels.Reportes.ReporteAdminViewModel.ResumenMetodoPagoDto>? metodosPago,
            IEnumerable<GestionComercial.UI.ViewModels.Reportes.ReporteAdminViewModel.ResumenProductoDto>? topProductos,
            DateTime desde,
            DateTime hasta,
            bool shouldOpenAfterDownload = false)
        {
            Exportar("Informe Admin", $"InformeAdmin_{Fecha()}", wb =>
            {
                // ── Hoja 1: Resumen de KPIs ──────────────────────────────────────
                var wsKpi = wb.Worksheets.Add("Resumen KPIs");
                wsKpi.Cell(1, 1).Value = "INFORME ADMINISTRATIVO";
                wsKpi.Cell(1, 1).Style.Font.Bold = true;
                wsKpi.Cell(1, 1).Style.Font.FontSize = 16;
                wsKpi.Range(1, 1, 1, 4).Merge();
                wsKpi.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                wsKpi.Cell(3, 1).Value = "Período:";
                wsKpi.Cell(3, 2).Value = $"{desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}";

                if (kpis != null)
                {
                    wsKpi.Cell(5, 1).Value = "RESUMEN DE OPERACIONES";
                    wsKpi.Cell(5, 1).Style.Font.Bold = true;
                    wsKpi.Range(5, 1, 5, 2).Merge();

                    wsKpi.Cell(6, 1).Value = "Total Ventas:";
                    wsKpi.Cell(6, 2).Value = (double)kpis.TotalVentas;
                    wsKpi.Cell(6, 2).Style.NumberFormat.Format = "$ #,##0";

                    wsKpi.Cell(7, 1).Value = "Cantidad de Ventas:";
                    wsKpi.Cell(7, 2).Value = kpis.CantidadVentas;

                    wsKpi.Cell(8, 1).Value = "Promedio por Venta:";
                    wsKpi.Cell(8, 2).Value = (double)kpis.PromedioVenta;
                    wsKpi.Cell(8, 2).Style.NumberFormat.Format = "$ #,##0.00";

                    wsKpi.Cell(9, 1).Value = "Clientes Únicos:";
                    wsKpi.Cell(9, 2).Value = kpis.ClientesUnicos;

                    wsKpi.Cell(10, 1).Value = "Clientes Nuevos:";
                    wsKpi.Cell(10, 2).Value = kpis.ClientesNuevos;

                    wsKpi.Cell(12, 1).Value = "RESUMEN DE CAJAS";
                    wsKpi.Cell(12, 1).Style.Font.Bold = true;
                    wsKpi.Range(12, 1, 12, 2).Merge();

                    wsKpi.Cell(13, 1).Value = "Total Cajas:";
                    wsKpi.Cell(13, 2).Value = kpis.TotalCajas;

                    wsKpi.Cell(14, 1).Value = "Cajas Cerradas:";
                    wsKpi.Cell(14, 2).Value = kpis.CajasCerradas;

                    wsKpi.Cell(15, 1).Value = "Total Ingresos (aperturas):";
                    wsKpi.Cell(15, 2).Value = (double)kpis.TotalIngresos;
                    wsKpi.Cell(15, 2).Style.NumberFormat.Format = "$ #,##0";

                    wsKpi.Cell(16, 1).Value = "Total Diferencias:";
                    wsKpi.Cell(16, 2).Value = (double)kpis.TotalDiferencias;
                    wsKpi.Cell(16, 2).Style.NumberFormat.Format = "$ #,##0";
                }

                wsKpi.Column(1).Width = 30;
                wsKpi.Column(2).Width = 20;
                FormatearHoja(wsKpi, 2);

                // ── Hoja 2: Métodos de Pago ─────────────────────────────────────
                if (metodosPago != null && metodosPago.Any())
                {
                    var wsMet = wb.Worksheets.Add("Métodos de Pago");
                    var headersMet = new[] { "Método", "Total", "Cantidad" };
                    AgregarHeaders(wsMet, headersMet);

                    int fila = 2;
                    decimal totalGeneral = 0;
                    foreach (var m in metodosPago)
                    {
                        wsMet.Cell(fila, 1).Value = m.Metodo;
                        wsMet.Cell(fila, 2).Value = (double)m.Total;
                        wsMet.Cell(fila, 3).Value = m.Cantidad;
                        wsMet.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0";
                        totalGeneral += m.Total;
                        fila++;
                    }

                    wsMet.Cell(fila, 1).Value = "TOTAL";
                    wsMet.Cell(fila, 1).Style.Font.Bold = true;
                    wsMet.Cell(fila, 2).Value = (double)totalGeneral;
                    wsMet.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0";
                    wsMet.Cell(fila, 2).Style.Font.Bold = true;

                    FormatearHoja(wsMet, headersMet.Length);
                    AgregarMetadatos(wsMet, "Métodos de Pago", desde, hasta);
                }

                // ── Hoja 3: Top Productos ────────────────────────────────────────
                if (topProductos != null && topProductos.Any())
                {
                    var wsProd = wb.Worksheets.Add("Top Productos");
                    var headersProd = new[] { "Producto", "Cantidad Vendida", "Total" };
                    AgregarHeaders(wsProd, headersProd);

                    int fila = 2;
                    foreach (var p in topProductos)
                    {
                        wsProd.Cell(fila, 1).Value = p.Nombre;
                        wsProd.Cell(fila, 2).Value = p.Cantidad;
                        wsProd.Cell(fila, 3).Value = (double)p.Total;
                        wsProd.Cell(fila, 3).Style.NumberFormat.Format = "$ #,##0";
                        fila++;
                    }

                    FormatearHoja(wsProd, headersProd.Length);
                    AgregarMetadatos(wsProd, "Top Productos", desde, hasta);
                }

                // ── Hoja 4: Resumen de Cajas ─────────────────────────────────────
                if (historialCajas != null && historialCajas.Any())
                {
                    var wsResumen = wb.Worksheets.Add("Resumen Cajas");
                    var headersResumen = new[]
                    {
                        "Caja", "Fecha Apertura", "Fecha Cierre", "Monto Inicial", "Monto Final", "Diferencia", "Tipo Diferencia", "Usuario Apertura", "Usuario Cierre", "Estado"
                    };
                    AgregarHeaders(wsResumen, headersResumen);

                    int filaR = 2;
                    decimal totalDiferencia = 0;
                    foreach (var c in historialCajas)
                    {
                        wsResumen.Cell(filaR, 1).Value = $"Caja {c.Id}";
                        wsResumen.Cell(filaR, 2).Value = c.FechaApertura;
                        wsResumen.Cell(filaR, 3).Value = c.FechaCierre ?? "—";
                        wsResumen.Cell(filaR, 4).Value = (double)c.MontoInicial;
                        wsResumen.Cell(filaR, 5).Value = c.MontoFinal.HasValue ? (double)c.MontoFinal.Value : 0;
                        wsResumen.Cell(filaR, 6).Value = c.Diferencia.HasValue ? (double)c.Diferencia.Value : 0;
                        wsResumen.Cell(filaR, 7).Value = c.TipoDiferencia;
                        wsResumen.Cell(filaR, 8).Value = c.UsuarioApertura;
                        wsResumen.Cell(filaR, 9).Value = c.UsuarioCierre ?? "—";
                        wsResumen.Cell(filaR, 10).Value = c.Estado;

                        wsResumen.Cell(filaR, 4).Style.NumberFormat.Format = "$ #,##0";
                        wsResumen.Cell(filaR, 5).Style.NumberFormat.Format = "$ #,##0";
                        wsResumen.Cell(filaR, 6).Style.NumberFormat.Format = "$ #,##0";

                        if (c.Diferencia.HasValue) totalDiferencia += c.Diferencia.Value;
                        filaR++;
                    }

                    wsResumen.Cell(filaR, 5).Value = "TOTALES:";
                    wsResumen.Cell(filaR, 5).Style.Font.Bold = true;
                    wsResumen.Cell(filaR, 6).Value = (double)totalDiferencia;
                    wsResumen.Cell(filaR, 6).Style.NumberFormat.Format = "$ #,##0";
                    wsResumen.Cell(filaR, 6).Style.Font.Bold = true;

                    FormatearHoja(wsResumen, headersResumen.Length);
                    AgregarMetadatos(wsResumen, "Resumen de Cajas", desde, hasta);
                }

                // ── Hoja 5: Auditoría de Cajas (reducida) ─────────────────────────
                var wsAud = wb.Worksheets.Add("Auditoría");
                var headersAud = new[] { "Fecha", "Usuario", "Operación", "Caja", "Monto", "Cambios" };
                AgregarHeaders(wsAud, headersAud);

                int filaAud = 2;
                foreach (var d in auditoriaCajas.Take(500)) // Limitar a 500 registros
                {
                    wsAud.Cell(filaAud, 1).Value = d.FechaOperacion.ToString("dd/MM/yyyy HH:mm");
                    wsAud.Cell(filaAud, 2).Value = d.Usuario;
                    wsAud.Cell(filaAud, 3).Value = d.TipoOperacionCaja;
                    wsAud.Cell(filaAud, 4).Value = d.NumeroCaja ?? "—";
                    if (d.MontoMostrar.HasValue) wsAud.Cell(filaAud, 5).Value = (double)d.MontoMostrar.Value;
                    wsAud.Cell(filaAud, 6).Value = d.DetalleCambios;
                    if (d.MontoMostrar.HasValue) wsAud.Cell(filaAud, 5).Style.NumberFormat.Format = "$ #,##0";
                    filaAud++;
                }

                FormatearHoja(wsAud, headersAud.Length);
                AgregarMetadatos(wsAud, "Auditoría de Cajas", desde, hasta);

            }, shouldOpenAfterDownload);
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

        private static void Exportar(string titulo, string nombreArchivo, Action<XLWorkbook> construir, bool shouldOpenAfterDownload = false)
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

                // Si shouldOpenAfterDownload es true, abrir automáticamente
                if (shouldOpenAfterDownload)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName        = dialogo.FileName,
                        UseShellExecute = true
                    });
                }
                else
                {
                    var resultado = MessageBox.Show(
                        $"Archivo exportado correctamente.\n¿Deseá abrirlo ahora?",
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

        /// <summary>
        /// Exporta auditoría de caja a Excel con todos los datos relevantes.
        /// </summary>
        public static void ExportarAuditoriaCaja(List<CajaAuditoriaItemDto> cajas)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Auditoría Caja");

            // Título
            ws.Cell(1, 1).Value = "Auditoría de Caja";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Range(1, 1, 1, 8).Merge();

            // Headers
            string[] headers = { "Fecha", "Hora", "Usuario Apertura", "Monto Inicial", "Ventas Efectivo", "Efectivo en Caja", "Monto Final", "Diff s/Efectivo", "Diff c/Efectivo", "Estado" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(3, i + 1).Value = headers[i];
                ws.Cell(3, i + 1).Style.Font.Bold = true;
                ws.Cell(3, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#1E293B");
                ws.Cell(3, i + 1).Style.Font.FontColor = XLColor.White;
            }

            // Datos
            for (int i = 0; i < cajas.Count; i++)
            {
                var c = cajas[i];
                int row = 4 + i;
                ws.Cell(row, 1).Value = c.FechaApertura;
                ws.Cell(row, 2).Value = c.HoraApertura;
                ws.Cell(row, 3).Value = c.UsuarioApertura;
                ws.Cell(row, 4).Value = (double)c.MontoInicial;
                ws.Cell(row, 4).Style.NumberFormat.Format = "$ #,##0.00";
                ws.Cell(row, 5).Value = (double)c.VentasEfectivo;
                ws.Cell(row, 5).Style.NumberFormat.Format = "$ #,##0.00";
                ws.Cell(row, 6).Value = (double)c.EfectivoEnCaja;
                ws.Cell(row, 6).Style.NumberFormat.Format = "$ #,##0.00";
                ws.Cell(row, 7).Value = c.MontoFinal.HasValue ? (double)c.MontoFinal.Value : 0;
                ws.Cell(row, 7).Style.NumberFormat.Format = "$ #,##0.00";
                ws.Cell(row, 8).Value = (double)c.DiferenciaSinEfectivo;
                ws.Cell(row, 8).Style.NumberFormat.Format = "$ #,##0.00";
                ws.Cell(row, 9).Value = (double)c.DiferenciaConEfectivo;
                ws.Cell(row, 9).Style.NumberFormat.Format = "$ #,##0.00";
                ws.Cell(row, 10).Value = c.Estado;

                // Color diferencia
                if (c.DiferenciaConEfectivo != 0)
                {
                    ws.Cell(row, 9).Style.Font.FontColor = XLColor.FromHtml("#EF4444");
                }
            }

            // Auto-ajustar columnas
            ws.Columns().AdjustToContents();

            // Guardar
            var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"AuditoriaCaja_{Fecha()}.xlsx");
            wb.SaveAs(path);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
        }
    }
}

#endif
