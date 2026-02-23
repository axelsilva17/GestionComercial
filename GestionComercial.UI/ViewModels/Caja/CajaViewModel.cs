using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.DTOs.Caja;


namespace GestionComercial.UI.ViewModels.Caja
{
    // ══════════════════════════════════════════════════════════════════════════════
    // CAJA VIEW MODEL — pantalla principal
    // ══════════════════════════════════════════════════════════════════════════════
    public class CajaViewModel : NavigableViewModel
    {
        private string _sucursalNombre = "Casa Central";
        public string SucursalNombre
        {
            get => _sucursalNombre;
            set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
        }

        private bool _cajaAbierta;
        public bool CajaAbierta
        {
            get => _cajaAbierta;
            set
            {
                _cajaAbierta = value;
                NotifyOfPropertyChange(() => CajaAbierta);
                NotifyOfPropertyChange(() => CajaCerrada);
            }
        }
        public bool CajaCerrada => !CajaAbierta;

        private decimal _montoInicial;
        public decimal MontoInicial
        {
            get => _montoInicial;
            set { _montoInicial = value; NotifyOfPropertyChange(() => MontoInicial); }
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

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
     

    
        
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            try
            {
                // TODO: await _cajaServicio.ObtenerCajaActualAsync(sucursalId)
                await Task.Delay(200);
                CajaAbierta = false;
            }
            finally { IsLoading = false; }
        }
        public async Task AbrirCaja()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[AbrirCaja] Iniciando...");
                var vm = IoC.Get<AperturaCajaViewModel>();
                System.Diagnostics.Debug.WriteLine($"[AbrirCaja] VM obtenido: {vm?.GetType().Name ?? "NULL"}");
                var shell = IoC.Get<ShellViewModel>();
                System.Diagnostics.Debug.WriteLine("[AbrirCaja] Shell obtenido");
                await shell.ActivateItemAsync(vm, CancellationToken.None);
                System.Diagnostics.Debug.WriteLine("[AbrirCaja] ActivateItem completado");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AbrirCaja] Error: {ex}");
            }
        }
        public async Task CerrarCaja()
        {
            var vm = IoC.Get<CierreCajaViewModel>();
            vm.InicializarConCaja(MontoInicial, TotalIngresos, TotalEgresos, TotalVentasDia);
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task RegistrarIngreso()
        {
            // TODO: abrir diálogo de ingreso manual
        }



        private void RecalcularSaldo() => SaldoActual = MontoInicial + TotalIngresos - TotalEgresos;
    }
}


