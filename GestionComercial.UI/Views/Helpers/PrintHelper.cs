using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.DTOs.Ventas;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GestionComercial.UI.Helpers
{
    /// <summary>
    /// Helper de impresión para tickets de venta y reportes básicos.
    /// Usa el sistema de impresión nativo de WPF (System.Printing).
    ///
    /// Uso desde ViewModel:
    ///   PrintHelper.ImprimirTicket(venta, sesion);
    ///   PrintHelper.ImprimirResumenCaja(movimientos, montoInicial, montoFinal);
    /// </summary>
    public static class PrintHelper
    {
        // ── Ticket de venta ──────────────────────────────────────────────────

        public static void ImprimirTicket(VentaDto venta, UsuarioSesionDto sesion)
        {
            var doc = new FlowDocument
            {
                PageWidth       = 280,   // ~80mm ticket
                PagePadding     = new Thickness(8),
                ColumnWidth     = double.MaxValue,
                FontFamily      = new FontFamily("Consolas"),
                FontSize        = 11
            };

            // Encabezado empresa
            doc.Blocks.Add(CentradoBold(sesion.EmpresaNombre, 13));
            doc.Blocks.Add(Centrado(sesion.SucursalNombre));
            doc.Blocks.Add(Separador());

            // Datos ticket
            doc.Blocks.Add(ParLinea("Ticket N°:", venta.IdVenta.ToString("D6")));
            doc.Blocks.Add(ParLinea("Fecha:",     venta.Fecha.ToString("dd/MM/yyyy HH:mm")));
            doc.Blocks.Add(ParLinea("Vendedor:",  sesion.NombreCompleto));
            if (!string.IsNullOrEmpty(venta.ClienteNombre))
                doc.Blocks.Add(ParLinea("Cliente:", venta.ClienteNombre));
            doc.Blocks.Add(Separador());

            // Items
            doc.Blocks.Add(CabecerItems());
            foreach (var item in venta.Detalle)
            {
                doc.Blocks.Add(LineaItem(
                    item.ProductoNombre,
                    item.Cantidad,
                    item.PrecioUnitario,
                    item.Subtotal));
            }

            doc.Blocks.Add(Separador());

            // Totales
            doc.Blocks.Add(ParLinea("Subtotal:",   $"$ {venta.TotalBruto:N0}"));
            if (venta.TotalDescuento > 0)
                doc.Blocks.Add(ParLinea("Descuento:", $"- $ {venta.TotalDescuento:N0}"));
            doc.Blocks.Add(ParLineaBold("TOTAL:", $"$ {venta.TotalFinal:N0}"));
            doc.Blocks.Add(Separador());

            // Métodos de pago
            foreach (var pago in venta.Pagos)
                doc.Blocks.Add(ParLinea($"  {pago.MetodoNombre}:", $"$ {pago.Monto:N0}"));

            doc.Blocks.Add(Separador());
            doc.Blocks.Add(Centrado("¡Gracias por su compra!"));
            doc.Blocks.Add(Centrado(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));

            Imprimir(doc, $"Ticket #{venta.IdVenta:D6}");
        }

        // ── Resumen de caja ──────────────────────────────────────────────────

        public static void ImprimirResumenCaja(
            string empresaNombre,
            string sucursalNombre,
            string usuarioNombre,
            DateTime apertura,
            DateTime cierre,
            decimal montoInicial,
            decimal totalVentas,
            decimal totalEfectivo,
            decimal totalElectronico,
            decimal montoFinal)
        {
            var doc = new FlowDocument
            {
                PageWidth   = 280,
                PagePadding = new Thickness(8),
                ColumnWidth = double.MaxValue,
                FontFamily  = new FontFamily("Consolas"),
                FontSize    = 11
            };

            doc.Blocks.Add(CentradoBold(empresaNombre, 13));
            doc.Blocks.Add(Centrado(sucursalNombre));
            doc.Blocks.Add(CentradoBold("CIERRE DE CAJA", 12));
            doc.Blocks.Add(Separador());

            doc.Blocks.Add(ParLinea("Operador:", usuarioNombre));
            doc.Blocks.Add(ParLinea("Apertura:", apertura.ToString("dd/MM/yyyy HH:mm")));
            doc.Blocks.Add(ParLinea("Cierre:",   cierre.ToString("dd/MM/yyyy HH:mm")));
            doc.Blocks.Add(Separador());

            doc.Blocks.Add(ParLinea("Monto inicial:",     $"$ {montoInicial:N0}"));
            doc.Blocks.Add(ParLinea("Total ventas:",      $"$ {totalVentas:N0}"));
            doc.Blocks.Add(Separador());
            doc.Blocks.Add(ParLinea("  Efectivo:",        $"$ {totalEfectivo:N0}"));
            doc.Blocks.Add(ParLinea("  Electrónico:",     $"$ {totalElectronico:N0}"));
            doc.Blocks.Add(Separador());
            doc.Blocks.Add(ParLineaBold("MONTO FINAL:",   $"$ {montoFinal:N0}"));
            doc.Blocks.Add(Separador());
            doc.Blocks.Add(Centrado(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));

            Imprimir(doc, "Cierre de Caja");
        }

        // ── Motor de impresión ────────────────────────────────────────────────

        private static void Imprimir(FlowDocument doc, string titulo)
        {
            try
            {
                var dialogo = new PrintDialog();
                if (dialogo.ShowDialog() != true) return;

                doc.PageWidth = dialogo.PrintableAreaWidth > 0
                    ? dialogo.PrintableAreaWidth
                    : 280;

                var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                dialogo.PrintDocument(paginator, titulo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir: {ex.Message}", "Error de impresión",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Helpers de bloques ────────────────────────────────────────────────

        private static Paragraph Centrado(string texto, double size = 11) =>
            new Paragraph(new Run(texto))
            {
                TextAlignment = TextAlignment.Center,
                FontSize      = size,
                Margin        = new Thickness(0, 1, 0, 1)
            };

        private static Paragraph CentradoBold(string texto, double size = 11) =>
            new Paragraph(new Run(texto))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight    = FontWeights.Bold,
                FontSize      = size,
                Margin        = new Thickness(0, 2, 0, 2)
            };

        private static Paragraph Separador() =>
            new Paragraph(new Run(new string('-', 36)))
            {
                FontFamily = new FontFamily("Consolas"),
                FontSize   = 10,
                Margin     = new Thickness(0, 2, 0, 2)
            };

        private static Paragraph ParLinea(string etiqueta, string valor)
        {
            var p = new Paragraph { Margin = new Thickness(0, 1, 0, 1) };
            p.Inlines.Add(new Run(etiqueta.PadRight(16)));
            p.Inlines.Add(new Run(valor));
            return p;
        }

        private static Paragraph ParLineaBold(string etiqueta, string valor)
        {
            var p = new Paragraph { Margin = new Thickness(0, 2, 0, 2), FontWeight = FontWeights.Bold };
            p.Inlines.Add(new Run(etiqueta.PadRight(16)));
            p.Inlines.Add(new Run(valor));
            return p;
        }

        private static Paragraph CabecerItems()
        {
            var p = new Paragraph { Margin = new Thickness(0, 2, 0, 2), FontWeight = FontWeights.Bold };
            p.Inlines.Add(new Run("Producto".PadRight(16)));
            p.Inlines.Add(new Run("Cant".PadRight(6)));
            p.Inlines.Add(new Run("P.Unit.".PadRight(9)));
            p.Inlines.Add(new Run("Total"));
            return p;
        }

        private static Paragraph LineaItem(string nombre, int cantidad, decimal precioUnit, decimal subtotal)
        {
            // Truncar nombre largo
            var nomCorto = nombre.Length > 14 ? nombre[..14] : nombre.PadRight(14);
            var p = new Paragraph { Margin = new Thickness(0, 1, 0, 1) };
            p.Inlines.Add(new Run($"{nomCorto}  {cantidad,-5} {precioUnit,8:N0}  {subtotal,8:N0}"));
            return p;
        }
    }

    // ── DTOs locales mínimos para no acoplar con lógica de negocio ────────────
    // Reemplazar por los DTOs reales cuando estén disponibles

    public class VentaDto
    {
        public int      IdVenta         { get; set; }
        public DateTime Fecha           { get; set; }
        public string   ClienteNombre   { get; set; }
        public decimal  TotalBruto      { get; set; }
        public decimal  TotalDescuento  { get; set; }
        public decimal  TotalFinal      { get; set; }
        public List<VentaDetalleDto> Detalle { get; set; } = new();
        public List<PagoDto>         Pagos   { get; set; } = new();
    }

    public class VentaDetalleDto
    {
        public string  ProductoNombre  { get; set; }
        public int     Cantidad        { get; set; }
        public decimal PrecioUnitario  { get; set; }
        public decimal Subtotal        { get; set; }
    }

    public class PagoDto
    {
        public string  MetodoNombre { get; set; }
        public decimal Monto        { get; set; }
    }
}
