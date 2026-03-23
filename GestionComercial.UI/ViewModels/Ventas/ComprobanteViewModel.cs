using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class ComprobanteItemVm
    {
        public string  Descripcion    { get; set; } = string.Empty;
        public int     Cantidad       { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal       { get; set; }
    }

    public class ComprobanteViewModel : NavigableViewModel
    {
        private readonly IVentaServicio _ventaServicio;
        private readonly SesionServicio _sesion;

        public ComprobanteViewModel(IVentaServicio ventaServicio, SesionServicio sesion)
        {
            _ventaServicio = ventaServicio;
            _sesion        = sesion;
            Titulo         = "Comprobante";
        }

        private int _idVenta;
        public int IdVenta
        {
            get => _idVenta;
            set { _idVenta = value; NotifyOfPropertyChange(() => IdVenta); NotifyOfPropertyChange(() => NumeroComprobante); }
        }
        public string NumeroComprobante => $"#{IdVenta:D6}";

        private string _clienteNombre = string.Empty;
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); }
        }

        private string _vendedorNombre = string.Empty;
        public string VendedorNombre
        {
            get => _vendedorNombre;
            set { _vendedorNombre = value; NotifyOfPropertyChange(() => VendedorNombre); }
        }

        private DateTime _fecha;
        public DateTime Fecha
        {
            get => _fecha;
            set { _fecha = value; NotifyOfPropertyChange(() => Fecha); }
        }

        private decimal _totalFinal;
        public decimal TotalFinal
        {
            get => _totalFinal;
            set { _totalFinal = value; NotifyOfPropertyChange(() => TotalFinal); }
        }

        private decimal _totalDescuento;
        public decimal TotalDescuento
        {
            get => _totalDescuento;
            set { _totalDescuento = value; NotifyOfPropertyChange(() => TotalDescuento); NotifyOfPropertyChange(() => HayDescuento); }
        }
        public bool HayDescuento => TotalDescuento > 0;

        private decimal _vuelto;
        public decimal Vuelto
        {
            get => _vuelto;
            set { _vuelto = value; NotifyOfPropertyChange(() => Vuelto); NotifyOfPropertyChange(() => HayVuelto); }
        }
        public bool HayVuelto => Vuelto > 0;

        private string _estado = string.Empty;
        public string Estado
        {
            get => _estado;
            set { _estado = value; NotifyOfPropertyChange(() => Estado); }
        }

        private ObservableCollection<ComprobanteItemVm> _items = new();
        public ObservableCollection<ComprobanteItemVm> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        // ── Carga ─────────────────────────────────────────────────────────────
        public async Task CargarAsync(int idVenta, decimal vuelto)
        {
            Vuelto = vuelto;
            IsLoading = true;
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ComprobanteVM-Cargar] Iniciando carga para venta #{idVenta}, Vuelto: {vuelto}");
                
                var venta = await _ventaServicio.ObtenerPorIdAsync(idVenta);
                if (venta == null || venta.Items == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[ComprobanteVM-Cargar] ERROR: Venta #{idVenta} no encontrada o sin items");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[ComprobanteVM-Cargar] Venta encontrada: {venta.IdVenta}, Items: {venta.Items.Count}");

                IdVenta        = venta.IdVenta;
                ClienteNombre  = venta.ClienteNombre;
                VendedorNombre = venta.UsuarioNombre;
                Fecha          = venta.Fecha;
                TotalFinal     = venta.TotalFinal;
                TotalDescuento = venta.TotalDescuento;
                Estado         = venta.Estado;

                Items = new ObservableCollection<ComprobanteItemVm>(
                    venta.Items.Select(i => new ComprobanteItemVm
                    {
                        Descripcion    = i.ProductoNombre,
                        Cantidad       = i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario,
                        Subtotal       = i.Subtotal,
                    }));
                    
                System.Diagnostics.Debug.WriteLine("[ComprobanteVM-Cargar] Carga completada exitosamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ComprobanteVM-Cargar] ERROR: {ex}");
                MostrarError(ex.Message);
            }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task NuevaVenta()
        {
            var vm = IoC.Get<VentaViewModel>();
            vm.NuevaVenta();
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task VerHistorial()
            => await IoC.Get<ShellViewModel>()
                        .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);

        /// <summary>
        /// Genera un HTML del comprobante y lo abre en el navegador.
        /// El botón 🖨️ Imprimir del HTML usa window.print().
        ///
        /// Para impresora térmica (58mm o 80mm):
        ///   El CSS @media print restringe max-width a 58mm/80mm automáticamente.
        ///   No necesitás driver especial: el navegador imprime a cualquier impresora
        ///   instalada en Windows, incluyendo térmica.
        ///
        /// Para probar sin impresora:
        ///   Ctrl+P en el navegador → "Guardar como PDF"
        /// </summary>
        public async Task GenerarComprobante()
        {
            IsLoading = true;
            try
            {
                var html = GenerarHtml();
                var ruta = Path.Combine(Path.GetTempPath(), $"comprobante_venta_{IdVenta}.html");
                await File.WriteAllTextAsync(ruta, html, System.Text.Encoding.UTF8);
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch (Exception ex) { MostrarError($"No se pudo generar el comprobante: {ex.Message}"); }
            finally { IsLoading = false; }
        }

        private string GenerarHtml()
        {
            var filas = string.Join("\n", Items.Select(i =>
                $@"<tr>
                     <td>{HtmlEnc(i.Descripcion)}</td>
                     <td class='c'>{i.Cantidad}</td>
                     <td class='r'>${i.PrecioUnitario:N2}</td>
                     <td class='r'>${i.Subtotal:N2}</td>
                   </tr>"));

            var descuentoFila = HayDescuento
                ? $"<tr class='desc'><td colspan='3'>Descuento</td><td class='r'>-${TotalDescuento:N2}</td></tr>"
                : string.Empty;

            var vueltoBadge = HayVuelto
                ? $"<div class='vuelto'>🔄 Vuelto: <strong>${Vuelto:N2}</strong></div>"
                : string.Empty;

            var estadoBadge = Estado == "Anulada"
                ? "<div class='anulado'>⚠ VENTA ANULADA</div>"
                : string.Empty;

            return $@"<!DOCTYPE html>
<html lang='es'>
<head>
  <meta charset='UTF-8'/>
  <title>Comprobante {NumeroComprobante}</title>
  <style>
    * {{ margin:0; padding:0; box-sizing:border-box; }}
    body {{
      font-family: 'Courier New', monospace;
      font-size: 13px;
      max-width: 320px;
      margin: 0 auto;
      padding: 16px;
      background: #fff;
      color: #000;
    }}
    h1  {{ font-size: 17px; text-align: center; margin-bottom: 3px; }}
    .n  {{ text-align: center; font-size: 12px; color: #555; margin-bottom: 10px; }}
    .ln {{ border-top: 1px dashed #000; margin: 10px 0; }}
    p   {{ font-size: 12px; margin-bottom: 3px; }}
    table {{ width: 100%; border-collapse: collapse; margin: 8px 0; }}
    th  {{ font-size: 10px; border-bottom: 1px solid #000; padding: 3px 2px; text-align: left; }}
    td  {{ padding: 4px 2px; font-size: 12px; vertical-align: top; }}
    .c  {{ text-align: center; }}
    .r  {{ text-align: right; }}
    .total td {{ font-weight: bold; font-size: 15px; border-top: 1px solid #000; padding-top: 6px; }}
    .desc td  {{ color: #c00; }}
    .vuelto  {{ margin: 10px 0 0; padding: 8px; background: #e8f5e9;
                font-size: 14px; font-weight: bold; text-align: center;
                border-radius: 6px; }}
    .anulado {{ margin: 10px 0 0; padding: 8px; background: #ffeaea;
                color: #c00; font-size: 14px; font-weight: bold;
                text-align: center; border-radius: 6px; }}
    .footer  {{ text-align: center; font-size: 10px; color: #777; margin-top: 14px; }}
    .actions {{ text-align: center; margin-top: 18px; }}
    .actions button {{
      padding: 9px 22px; font-size: 14px; cursor: pointer;
      background: #1a1a2e; color: #fff; border: none; border-radius: 6px;
    }}
    @media print {{
      body {{ max-width: 58mm; padding: 4px; font-size: 11px; }}
      .actions {{ display: none; }}
    }}
  </style>
</head>
<body>
  <h1>🧾 Comprobante de Venta</h1>
  <div class='n'>{NumeroComprobante}</div>
  <div class='ln'></div>
  <p><strong>Fecha:</strong> {Fecha:dd/MM/yyyy HH:mm}</p>
  <p><strong>Cliente:</strong> {HtmlEnc(ClienteNombre)}</p>
  <p><strong>Vendedor:</strong> {HtmlEnc(VendedorNombre)}</p>
  <div class='ln'></div>
  <table>
    <thead>
      <tr><th>Descripción</th><th class='c'>Cant</th><th class='r'>P.U.</th><th class='r'>Sub.</th></tr>
    </thead>
    <tbody>
      {filas}
      {descuentoFila}
    </tbody>
    <tfoot>
      <tr class='total'><td colspan='3'>TOTAL</td><td class='r'>${TotalFinal:N2}</td></tr>
    </tfoot>
  </table>
  {vueltoBadge}
  {estadoBadge}
  <div class='ln'></div>
  <div class='footer'>Gracias por su compra · {DateTime.Now:dd/MM/yyyy HH:mm}</div>
  <div class='actions'>
    <button onclick='window.print()'>🖨️ Imprimir</button>
  </div>
</body>
</html>";
        }

        private static string HtmlEnc(string s)
            => System.Net.WebUtility.HtmlEncode(s);
    }
}
