using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Caja
{
    public class AperturaCajaViewModel : NavigableViewModel
    {
        private readonly ICajaServicio  _cajaServicio;
        private readonly SesionServicio _sesion;

        public AperturaCajaViewModel(ICajaServicio cajaServicio, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _sesion       = sesion;
        }

        public DateTime FechaHoy => DateTime.Now;

        private string _sucursalNombre = "Casa Central";
        public string SucursalNombre
        {
            get => _sucursalNombre;
            set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
        }

        private DateTime _ultimoCierre;
        public DateTime UltimoCierre
        {
            get => _ultimoCierre;
            set { _ultimoCierre = value; NotifyOfPropertyChange(() => UltimoCierre); }
        }

        private decimal _saldoAnterior;
        public decimal SaldoAnterior
        {
            get => _saldoAnterior;
            set { _saldoAnterior = value; NotifyOfPropertyChange(() => SaldoAnterior); }
        }

        private string _montoInicial = string.Empty;
        public string MontoInicial
        {
            get => _montoInicial;
            set { _montoInicial = value; NotifyOfPropertyChange(() => MontoInicial); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // Verificar si ya hay una caja abierta — si sí, volver atrás
                var cajaAbierta = await _cajaServicio.ObtenerCajaAbiertaAsync(_sesion.IdSucursal);
                if (cajaAbierta != null)
                {
                    await Cancelar();
                    return;
                }

                // Cargar datos del último cierre para mostrar en pantalla
                // Si el servicio expone un método para esto, usarlo.
                // Por ahora usamos la fecha actual como referencia.
                UltimoCierre  = DateTime.Now;
                SaldoAnterior = 0;
            }
            catch (Exception ex)
            {
                // No bloqueamos la apertura si falla cargar datos históricos
                MostrarError($"No se pudo cargar el último cierre: {ex.Message}");
                UltimoCierre  = DateTime.Now;
                SaldoAnterior = 0;
            }
            finally { IsLoading = false; }
        }

        public async Task Confirmar()
        {
            if (!decimal.TryParse(
                    MontoInicial.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var monto) || monto < 0)
            {
                MostrarError("Ingresá un monto inicial válido.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                var caja = await _cajaServicio.AbrirCajaAsync(
                    _sesion.IdSucursal, _sesion.IdUsuario, monto);

                _sesion.IdCajaActual = caja.Id;
                await Cancelar();
            }
            catch (Exception ex) 
            { 
                // Mostrar error en MessageBox para debug
                var msg = $"ERROR:\n\n{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.InnerException?.Message}";
                System.Windows.MessageBox.Show(msg, "ERROR AL ABRIR CAJA", 
                    System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        public async Task Cancelar()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);
        }
    }
}
