using System.Globalization;
using System.Text;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación del servicio de impresión térmica.
    /// Soporta dos modos:
    /// - ModoTest=true: Guarda tickets en archivos de texto legible en DirectorioTest
    /// - ModoTest=false: Envía comandos ESCPOS a la impresora térmica
    /// </summary>
    public class ServicioImpresionTermica : IServicioImpresion
    {
        private ImpresoraConfig _config = new();

        public void Configurar(ImpresoraConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool VerificarConexion()
        {
            // En modo test siempre está "conectado"
            if (_config.ModoTest) return true;

            // En producción intentamos verificar el puerto de la impresora
            try
            {
                // Intentar abrir el puerto LPT o COM
                // RawPrinterHelper.SendToPrinter(_config.NombreImpresora, new byte[] { 0x10, 0x04, 0x01 });
                return true; // Placeholder - implementar según necesidad
            }
            catch
            {
                return false;
            }
        }

        public void ImprimirTicket(VentaDto venta, List<PagoDto> pagos)
        {
            if (_config.ModoTest)
            {
                var contenido = GenerarContenidoTicket(venta, pagos);
                var nombreArchivo = $"ticket_{venta.IdVenta}_{DateTime.Now:yyyyMMddHHmmss}.txt";
                var ruta = Path.Combine(_config.DirectorioTest, nombreArchivo);

                Directory.CreateDirectory(_config.DirectorioTest);
                File.WriteAllText(ruta, contenido, Encoding.UTF8);
            }
            else
            {
                var bytes = GenerarBytesTicket(venta, pagos);
                EnviarAImpresora(bytes);
            }
        }

        /// <summary>
        /// Genera el contenido de texto legible para el ticket (modo test).
        /// </summary>
        private string GenerarContenidoTicket(VentaDto venta, List<PagoDto> pagos)
        {
            var sb = new StringBuilder();
            var separador = new string('=', _config.AnchoCaracteres);
            var linea = new string('-', _config.AnchoCaracteres);

            sb.AppendLine(separador);
            sb.AppendLine("            TICKET DE VENTA");
            sb.AppendLine(separador);
            sb.AppendLine($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Ticket Nro: {venta.IdVenta}");
            sb.AppendLine($"Caja: #{venta.IdCaja}");
            sb.AppendLine($"Vendedor: {venta.UsuarioNombre}");
            sb.AppendLine(linea);

            // Detalle de items
            sb.AppendLine("              DETALLE DE COMPRA");
            sb.AppendLine(linea);
            foreach (var item in venta.Items)
            {
                sb.AppendLine($"{item.Cantidad} x {item.ProductoNombre}");
                sb.AppendLine($"   {item.PrecioUnitario:N2} c/u ............ {item.Subtotal:N2}");
            }

            sb.AppendLine(linea);
            sb.AppendLine("              RESUMEN");
            sb.AppendLine(linea);
            sb.AppendLine($"  SUBTOTAL:        ${venta.TotalBruto:N2}");
            sb.AppendLine($"  DESCUENTOS:     -${venta.TotalDescuento:N2}");
            sb.AppendLine(linea);
            sb.AppendLine($"  TOTAL:           ${venta.TotalFinal:N2}");
            sb.AppendLine(separador);

            // Detalle de pagos
            sb.AppendLine("              PAGOS");
            sb.AppendLine(linea);
            foreach (var pago in pagos)
            {
                sb.AppendLine($"  {pago.MetodoNombre} ............. ${pago.Monto:N2}");
            }

            sb.AppendLine(linea);
            sb.AppendLine("    ¡Gracias por su compra!");
            sb.AppendLine(separador);

            return sb.ToString();
        }

        /// <summary>
        /// Genera bytes ESCPOS para enviar a impresora térmica.
        /// </summary>
        private byte[] GenerarBytesTicket(VentaDto venta, List<PagoDto> pagos)
        {
            var sb = new List<byte>();

            // Inicializar impresora
            sb.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @

            // Centrar texto
            sb.AddRange(new byte[] { 0x1B, 0x61, 0x01 }); // ESC a 1 = centrado

            // Negrita ON
            sb.AddRange(new byte[] { 0x1B, 0x45, 0x01 }); // ESC E 1

            sb.AddRange(Encoding.ASCII.GetBytes("TIKET DE VENTA\n"));
            sb.AddRange(new byte[] { 0x1B, 0x45, 0x00 }); // Negrita OFF

            // Alineación izquierda
            sb.AddRange(new byte[] { 0x1B, 0x61, 0x00 }); // ESC a 0 = izquierda

            sb.AddRange(Encoding.ASCII.GetBytes($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}\n"));
            sb.AddRange(Encoding.ASCII.GetBytes($"Ticket #: {venta.IdVenta}\n"));
            sb.AddRange(Encoding.ASCII.GetBytes("----------------------------\n"));

            foreach (var item in venta.Items)
            {
                sb.AddRange(Encoding.ASCII.GetBytes($"{item.Cantidad} x {item.ProductoNombre}\n"));
                sb.AddRange(Encoding.ASCII.GetBytes($"   ${item.PrecioUnitario:N2} ---- ${item.Subtotal:N2}\n"));
            }

            sb.AddRange(Encoding.ASCII.GetBytes("----------------------------\n"));
            sb.AddRange(Encoding.ASCII.GetBytes($"TOTAL: ${venta.TotalFinal:N2}\n"));
            sb.AddRange(new byte[] { 0x1B, 0x45, 0x01 }); // Negrita ON
            sb.AddRange(Encoding.ASCII.GetBytes($"PAGADO: ${pagos.Sum(p => p.Monto):N2}\n"));
            sb.AddRange(new byte[] { 0x1B, 0x45, 0x00 }); // Negrita OFF

            sb.AddRange(Encoding.ASCII.GetBytes("----------------------------\n"));
            sb.AddRange(Encoding.ASCII.GetBytes("Gracias por su compra!\n\n\n"));

            // Cortar papel
            sb.AddRange(new byte[] { 0x1D, 0x56, 0x00 }); // GS V 0

            return sb.ToArray();
        }

        /// <summary>
        /// Envía bytes raw a la impresora (requiere P/Invoke o biblioteca ESCPOS).
        /// </summary>
        private void EnviarAImpresora(byte[] datos)
        {
            // Placeholder: implementar con RawPrinterHelper o similar
            // RawPrinterHelper.SendBytesToPrinter(_config.NombreImpresora, datos);
            throw new NotImplementedException("Envío a impresora física requiere implementación de RawPrinterHelper o similar.");
        }
    }
}
