using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Caja
{
    public class CajaViewModel : NavigableViewModel
    {
        private readonly ICajaServicio  _cajaServicio;
        private readonly SesionServicio _sesion;

        public CajaViewModel(ICajaServicio cajaServicio, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _sesion       = sesion;
        }

        // ── Id interno ────────────────────────────────────────────────────────
        private int? _idCajaActual;

        // ── Props ─────────────────────────────────────────────────────────────
        private bool _cajaAbierta;
        public bool CajaAbierta
        {
            get => _cajaAbierta;
            set { _cajaAbierta = value; NotifyOfPropertyChange(() => CajaAbierta); NotifyOfPropertyChange(() => CajaCerrada); }
        }
        public bool CajaCerrada => !CajaAbierta;

        private string _sucursalNombre = string.Empty;
        public string SucursalNombre
        {
            get => _sucursalNombre;
            set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
        }

        private decimal _montoInicial;
        public decimal MontoInicial
        {
            get => _montoInicial;
            set { _montoInicial = value; NotifyOfPropertyChange(() => MontoInicial); RecalcularSaldo(); }
        }

        private decimal _totalIngresos;
        public decimal TotalIngresos
        {
            get => _totalIngresos;
            set { _totalIngresos = value; NotifyOfPropertyChange(() => TotalIngresos); RecalcularSaldo(); }
        }

        private decimal _totalEgresos;
        public decimal TotalEgresos
        {
            get => _totalEgresos;
            set { _totalEgresos = value; NotifyOfPropertyChange(() => TotalEgresos); RecalcularSaldo(); }
        }

        private decimal _totalVentasDia;
        public decimal TotalVentasDia
        {
            get => _totalVentasDia;
            set { _totalVentasDia = value; NotifyOfPropertyChange(() => TotalVentasDia); }
        }

        private int _cantidadIngresos;
        public int CantidadIngresos
        {
            get => _cantidadIngresos;
            set { _cantidadIngresos = value; NotifyOfPropertyChange(() => CantidadIngresos); }
        }

        private int _cantidadEgresos;
        public int CantidadEgresos
        {
            get => _cantidadEgresos;
            set { _cantidadEgresos = value; NotifyOfPropertyChange(() => CantidadEgresos); }
        }

        private int _cantidadVentasDia;
        public int CantidadVentasDia
        {
            get => _cantidadVentasDia;
            set { _cantidadVentasDia = value; NotifyOfPropertyChange(() => CantidadVentasDia); }
        }

        private decimal _saldoActual;
        public decimal SaldoActual
        {
            get => _saldoActual;
            set { _saldoActual = value; NotifyOfPropertyChange(() => SaldoActual); }
        }

        private DateTime _fechaApertura;
        public DateTime FechaApertura
        {
            get => _fechaApertura;
            set { _fechaApertura = value; NotifyOfPropertyChange(() => FechaApertura); }
        }

        private string _observacion = string.Empty;
        public string Observacion
        {
            get => _observacion;
            set { _observacion = value; NotifyOfPropertyChange(() => Observacion); }
        }

        private ObservableCollection<MovimientoCajaDto> _movimientos = new();
        public ObservableCollection<MovimientoCajaDto> Movimientos
        {
            get => _movimientos;
            set { _movimientos = value; NotifyOfPropertyChange(() => Movimientos); }
        }

        private ObservableCollection<DesglosePagoDto> _desglosePorMetodo = new();
        public ObservableCollection<DesglosePagoDto> DesglosePorMetodo
        {
            get => _desglosePorMetodo;
            set { _desglosePorMetodo = value; NotifyOfPropertyChange(() => DesglosePorMetodo); }
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var caja = await _cajaServicio.ObtenerCajaAbiertaAsync(_sesion.IdSucursal);

                if (caja == null)
                {
                    CajaAbierta          = false;
                    _idCajaActual        = null;
                    _sesion.IdCajaActual = null;
                    MontoInicial         = 0;
                    TotalIngresos        = 0;
                    TotalEgresos         = 0;
                    TotalVentasDia       = 0;
                    SaldoActual          = 0;
                    Movimientos          = new();
                    DesglosePorMetodo    = new();
                    return;
                }

                CajaAbierta          = true;
                _idCajaActual        = caja.Id;
                _sesion.IdCajaActual = caja.Id;
                MontoInicial         = caja.MontoInicial;
                FechaApertura        = caja.FechaApertura;

                // Cargar movimientos y calcular totales
                var movimientos = await _cajaServicio.ObtenerMovimientosAsync(caja.Id);
                Movimientos = new ObservableCollection<MovimientoCajaDto>(movimientos);

                // Calcular ingresos y egresos desde los movimientos (excluir apertura que es neutral)
                TotalIngresos = movimientos.Where(m => m.EsIngreso && !m.EsApertura).Sum(m => m.Monto);
                TotalEgresos  = movimientos.Where(m => !m.EsIngreso && !m.EsApertura).Sum(m => m.Monto);
                CantidadIngresos = movimientos.Count(m => m.EsIngreso && !m.EsApertura);
                CantidadEgresos  = movimientos.Count(m => !m.EsIngreso && !m.EsApertura);

                // Calcular ventas del día
                var ventasDia = await _cajaServicio.ObtenerVentasDelDiaAsync(caja.Id);
                TotalVentasDia = ventasDia.Sum(v => v.Total);
                CantidadVentasDia = ventasDia.Count();

                // Desglose por método de pago
                var desglose = await _cajaServicio.ObtenerDesglosePorMetodoAsync(caja.Id);
                DesglosePorMetodo = new ObservableCollection<DesglosePagoDto>(desglose);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar caja: {ex.Message}");
                CajaAbierta = false;
            }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task AbrirCaja()
        {
            var vm = IoC.Get<AperturaCajaViewModel>();
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task CerrarCaja()
        {
            if (_idCajaActual == null)
            {
                MostrarError("No hay caja abierta para cerrar.");
                return;
            }
            var vm = IoC.Get<CierreCajaViewModel>();
            vm.InicializarConCaja(_idCajaActual.Value, FechaApertura);
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task RegistrarIngreso() { /* TODO */ }

        public async Task Actualizar() => await CargarAsync();

        private void RecalcularSaldo() => SaldoActual = MontoInicial + TotalIngresos - TotalEgresos;
    }
}
