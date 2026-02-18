using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Caja;
using GestionComercial.UI.ViewModels.Main;

public class CierreCajaViewModel : NavigableViewModel
{
    private string _sucursalNombre = "Casa Central";
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

    private decimal _totalVentas;
    public decimal TotalVentas
    {
        get => _totalVentas;
        set { _totalVentas = value; NotifyOfPropertyChange(() => TotalVentas); RecalcularSaldo(); }
    }

    private decimal _saldoEsperado;
    public decimal SaldoEsperado
    {
        get => _saldoEsperado;
        set { _saldoEsperado = value; NotifyOfPropertyChange(() => SaldoEsperado); }
    }

    private decimal _diferencia;
    public decimal Diferencia
    {
        get => _diferencia;
        set { _diferencia = value; NotifyOfPropertyChange(() => Diferencia); NotifyOfPropertyChange(() => DiferenciaNegativa); }
    }

    public bool DiferenciaNegativa => Diferencia < 0;

    private string _montoFinal = string.Empty;
    public string MontoFinal
    {
        get => _montoFinal;
        set
        {
            _montoFinal = value;
            NotifyOfPropertyChange(() => MontoFinal);
            if (decimal.TryParse(value, out var m))
                Diferencia = m - SaldoEsperado;
        }
    }

    private string _observacion = string.Empty;
    public string Observacion
    {
        get => _observacion;
        set { _observacion = value; NotifyOfPropertyChange(() => Observacion); }
    }

    public void InicializarConCaja(decimal montoInicial, decimal ingresos, decimal egresos, decimal ventas)
    {
        MontoInicial = montoInicial;
        TotalIngresos = ingresos;
        TotalEgresos = egresos;
        TotalVentas = ventas;
    }

    private void RecalcularSaldo()
        => SaldoEsperado = MontoInicial + TotalIngresos - TotalEgresos;

    public async Task Confirmar()
    {
        if (!decimal.TryParse(MontoFinal, out var monto) || monto < 0)
        {
            MostrarError("Ingresá el efectivo contado.");
            return;
        }

        IsLoading = true;
        LimpiarError();
        try
        {
            // TODO: await _cajaServicio.CerrarAsync(cajaId, usuarioId, monto, Observacion)
            await Task.Delay(300);
            await Cancelar();
        }
        catch (Exception ex) { MostrarError(ex.Message); }
        finally { IsLoading = false; }
    }

    public async Task Cancelar()
    {
        await IoC.Get<ShellViewModel>()
                 .ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);
    }
}