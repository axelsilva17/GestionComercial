using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.Helpers;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Caja
{
    public class CierreCajaViewModel : NavigableViewModel
    {
        private readonly ICajaServicio  _cajaServicio;
        private readonly SesionServicio _sesion;

        private int _idCaja;

        public CierreCajaViewModel(ICajaServicio cajaServicio, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _sesion       = sesion;
        }

        // ── Props display ─────────────────────────────────────────────────────
        private string _sucursalNombre = "Casa Central";
        public string SucursalNombre
        {
            get => _sucursalNombre;
            set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
        }

        private DateTime _fechaApertura;
        public DateTime FechaApertura
        {
            get => _fechaApertura;
            set { _fechaApertura = value; NotifyOfPropertyChange(() => FechaApertura); }
        }

        private DateTime _fechaCierre;
        public DateTime FechaCierre
        {
            get => _fechaCierre;
            set { _fechaCierre = value; NotifyOfPropertyChange(() => FechaCierre); }
        }

        private int _cantidadVentas;
        public int CantidadVentas
        {
            get => _cantidadVentas;
            set { _cantidadVentas = value; NotifyOfPropertyChange(() => CantidadVentas); }
        }

        // ── Efectivo (afecta saldo físico) ────────────────────────────────────
        private decimal _montoInicial;
        public decimal MontoInicial
        {
            get => _montoInicial;
            set { _montoInicial = value; NotifyOfPropertyChange(() => MontoInicial); RecalcularSaldo(); }
        }

        private decimal _ventasEfectivo;
        public decimal VentasEfectivo
        {
            get => _ventasEfectivo;
            set { _ventasEfectivo = value; NotifyOfPropertyChange(() => VentasEfectivo); RecalcularSaldo(); }
        }

        private decimal _ingresosEfectivo;
        public decimal IngresosEfectivo
        {
            get => _ingresosEfectivo;
            set { _ingresosEfectivo = value; NotifyOfPropertyChange(() => IngresosEfectivo); RecalcularSaldo(); }
        }

        private decimal _egresosEfectivo;
        public decimal EgresosEfectivo
        {
            get => _egresosEfectivo;
            set { _egresosEfectivo = value; NotifyOfPropertyChange(() => EgresosEfectivo); RecalcularSaldo(); }
        }

        private decimal _saldoEsperado;
        public decimal SaldoEsperado
        {
            get => _saldoEsperado;
            set { _saldoEsperado = value; NotifyOfPropertyChange(() => SaldoEsperado); RecalcularDiferencia(); }
        }

        // ── Otros métodos (informativos) ──────────────────────────────────────
        private decimal _ventasTarjeta;
        public decimal VentasTarjeta
        {
            get => _ventasTarjeta;
            set { _ventasTarjeta = value; NotifyOfPropertyChange(() => VentasTarjeta); NotifyOfPropertyChange(() => TotalVendido); NotifyOfPropertyChange(() => HayOtrosMetodos); }
        }

        private decimal _ventasTransferencia;
        public decimal VentasTransferencia
        {
            get => _ventasTransferencia;
            set { _ventasTransferencia = value; NotifyOfPropertyChange(() => VentasTransferencia); NotifyOfPropertyChange(() => TotalVendido); NotifyOfPropertyChange(() => HayOtrosMetodos); }
        }

        private decimal _ventasQR;
        public decimal VentasQR
        {
            get => _ventasQR;
            set { _ventasQR = value; NotifyOfPropertyChange(() => VentasQR); NotifyOfPropertyChange(() => TotalVendido); NotifyOfPropertyChange(() => HayOtrosMetodos); }
        }

        private decimal _ventasCuentaCte;
        public decimal VentasCuentaCte
        {
            get => _ventasCuentaCte;
            set { _ventasCuentaCte = value; NotifyOfPropertyChange(() => VentasCuentaCte); NotifyOfPropertyChange(() => TotalVendido); NotifyOfPropertyChange(() => HayOtrosMetodos); }
        }

        private decimal _ventasOtros;
        public decimal VentasOtros
        {
            get => _ventasOtros;
            set { _ventasOtros = value; NotifyOfPropertyChange(() => VentasOtros); NotifyOfPropertyChange(() => TotalVendido); NotifyOfPropertyChange(() => HayOtrosMetodos); }
        }

        public decimal TotalVendido =>
            VentasEfectivo + VentasTarjeta + VentasTransferencia + VentasQR + VentasCuentaCte + VentasOtros;

        public bool HayOtrosMetodos =>
            VentasTarjeta > 0 || VentasTransferencia > 0 || VentasQR > 0 || VentasCuentaCte > 0 || VentasOtros > 0;

        private ObservableCollection<DesglosePagoDto> _desglose = new();
        public ObservableCollection<DesglosePagoDto> Desglose
        {
            get => _desglose;
            set { _desglose = value; NotifyOfPropertyChange(() => Desglose); }
        }

        // ── Monto contado y diferencia ────────────────────────────────────────
        private decimal _diferencia;
        public decimal Diferencia
        {
            get => _diferencia;
            set { _diferencia = value; NotifyOfPropertyChange(() => Diferencia); NotifyOfPropertyChange(() => DiferenciaNegativa); NotifyOfPropertyChange(() => DiferenciaTexto); }
        }
        public bool   DiferenciaNegativa => Diferencia < 0;
        public string DiferenciaTexto    => Diferencia < 0 ? "Falta efectivo en caja" : Diferencia > 0 ? "Sobra efectivo en caja" : "Todo cuadra ✓";

        private string _montoFinal = string.Empty;
        public string MontoFinal
        {
            get => _montoFinal;
            set { _montoFinal = value; NotifyOfPropertyChange(() => MontoFinal); RecalcularDiferencia(); }
        }

        private string _observacion = string.Empty;
        public string Observacion
        {
            get => _observacion;
            set { _observacion = value; NotifyOfPropertyChange(() => Observacion); }
        }

        // ── Inicialización desde CajaViewModel ───────────────────────────────
        // Recibe solo el ID — el resumen lo carga solo al activarse
        public void InicializarConCaja(int idCaja, DateTime fechaApertura)
        {
            _idCaja       = idCaja;
            FechaApertura = fechaApertura;
        }

        // Sobrecarga de compatibilidad con la firma anterior
        public void InicializarConCaja(int idCaja, decimal montoInicial, decimal ingresos,
                                       decimal egresos, decimal ventas, DateTime fechaApertura)
        {
            _idCaja        = idCaja;
            FechaApertura  = fechaApertura;
            MontoInicial   = montoInicial;
            // El resto se sobreescribe al activar el VM con datos reales
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (_idCaja == 0) return;
            await CargarResumenAsync();
        }

        private async Task CargarResumenAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var resumen = await _cajaServicio.ObtenerResumenCierreAsync(_idCaja);

                // Efectivo
                MontoInicial      = resumen.MontoInicial;
                VentasEfectivo    = resumen.VentasEfectivo;
                IngresosEfectivo  = resumen.IngresosEfectivo;
                EgresosEfectivo   = resumen.EgresosEfectivo;

                // Otros métodos
                VentasTarjeta       = resumen.VentasTarjeta;
                VentasTransferencia = resumen.VentasTransferencia;
                VentasQR            = resumen.VentasQR;
                VentasCuentaCte     = resumen.VentasCuentaCte;
                VentasOtros         = resumen.VentasOtros;

                // Desglose libre para lista
                Desglose = new ObservableCollection<DesglosePagoDto>(resumen.DesglosePorMetodo);

                FechaApertura = resumen.FechaApertura;
                FechaCierre   = DateTime.Now;
                CantidadVentas = resumen.CantidadVentas;

                NotifyOfPropertyChange(() => TotalVendido);
                NotifyOfPropertyChange(() => HayOtrosMetodos);
            }
            catch (Exception ex) { MostrarError($"Error al cargar resumen: {ex.Message}"); }
            finally { IsLoading = false; }
        }

        private void RecalcularSaldo()
        {
            SaldoEsperado = MontoInicial + VentasEfectivo + IngresosEfectivo - EgresosEfectivo;
        }

        private void RecalcularDiferencia()
        {
            if (decimal.TryParse(
                    MontoFinal.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var m))
                Diferencia = m - SaldoEsperado;
            else
                Diferencia = 0;
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        
        /// <summary>
        /// Cierre de caja en modo "Ciego" - sin input manual del usuario.
        /// El sistema usa el saldo calculado internamente y guarda la diferencia en auditoría.
        /// </summary>
        public async Task ConfirmarCiego()
        {
            if (_idCaja == 0)
            {
                MostrarError("No se encontró el ID de caja. Volvé al módulo de Caja e intentá de nuevo.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                // En modo Ciego: el monto final = saldo calculado por el sistema
                // La diferencia se calcula internamente (siempre 0 en este modo)
                // pero se guarda en auditoría para seguridad
                var montoCalculado = SaldoEsperado;
                
                System.Diagnostics.Debug.WriteLine($"[CierreCajaVM] Modo CIEGO: montoCalculado={montoCalculado}, diferencia=0 (calculada internamente)");
                
                // Guardar en auditoría la diferencia (aunque sea 0, queda registro)
                await GuardarAuditoriaCierreCiegoAsync(montoCalculado, diferencia: 0);
                
                await _cajaServicio.CerrarCajaAsync(_idCaja, _sesion.IdUsuario, montoCalculado);
                _sesion.IdCajaActual = null;
                await Cancelar();
            }
            catch (Exception ex) 
            { 
                var msg = $"ERROR:\n\n{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.InnerException?.Message}";
                System.Windows.MessageBox.Show(msg, "ERROR AL CERRAR CAJA", 
                    System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        /// <summary>
        /// Registra la diferencia del cierre ciego en auditoría para seguridad.
        /// </summary>
        private async Task GuardarAuditoriaCierreCiegoAsync(decimal montoFinal, decimal diferencia)
        {
            try
            {
                var valoresAuditoria = new
                {
                    IdCaja = _idCaja,
                    FechaCierre = DateTime.Now,
                    MontoInicial,
                    VentasEfectivo,
                    IngresosEfectivo,
                    EgresosEfectivo,
                    SaldoEsperado,
                    MontoFinal = montoFinal,
                    Diferencia = diferencia,
                    ModoCierre = "CIEGO", // Indica que fue cierre ciego (sin verificación manual)
                    UsuarioCierreId = _sesion.IdUsuario
                };

                var json = System.Text.Json.JsonSerializer.Serialize(valoresAuditoria);
                
                // Registrar en auditoría del sistema
                await _cajaServicio.RegistrarAuditoriaCierreAsync(
                    _idCaja,
                    _sesion.IdUsuario,
                    json,
                    montoFinal,
                    diferencia);
                    
                System.Diagnostics.Debug.WriteLine($"[CierreCajaVM] Auditoría de cierre ciego guardada: diferencia={diferencia}");
            }
            catch (Exception ex)
            {
                // No fallar el cierre por error de auditoría - solo loguear
                System.Diagnostics.Debug.WriteLine($"[CierreCajaVM] Error guardando auditoría: {ex.Message}");
            }
        }

        /// <summary>
        /// Confirmar original (para compatibilidad) - ahora redirect a ConfirmarCiego.
        /// </summary>
        public async Task Confirmar()
        {
            await ConfirmarCiego();
        }

        public async Task Cancelar()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);
        }

        // ── Exportar a Excel ─────────────────────────────────────────────────
        public async Task ExportarExcel()
        {
            try
            {
                IsLoading = true;
                LimpiarError();

                // Crear un DTO con los datos actuales para exportar
                var resumen = new ResumenCierreDto
                {
                    MontoInicial       = MontoInicial,
                    VentasEfectivo     = VentasEfectivo,
                    IngresosEfectivo   = IngresosEfectivo,
                    EgresosEfectivo    = EgresosEfectivo,
                    VentasTarjeta      = VentasTarjeta,
                    VentasTransferencia= VentasTransferencia,
                    VentasQR           = VentasQR,
                    VentasCuentaCte    = VentasCuentaCte,
                    VentasOtros        = VentasOtros,
                    FechaApertura      = FechaApertura,
                    CantidadVentas     = CantidadVentas,
                    DesglosePorMetodo  = new List<DesglosePagoDto>(Desglose)
                };

                ExportHelper.ExportarCierre(resumen);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al exportar: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
