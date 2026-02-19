using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Caja;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Productos;

public class AperturaCajaViewModel : NavigableViewModel
{
    private string _sucursalNombre = "Casa Central";
    public string SucursalNombre
    {
        get => _sucursalNombre;
        set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
    }

    public DateTime FechaHoy => DateTime.Now;

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
        // TODO: cargar último cierre desde servicio
        UltimoCierre = DateTime.Now.AddHours(-8);
        SaldoAnterior = 0;
    }

    public async Task Confirmar()
    {
        if (!decimal.TryParse(MontoInicial, out var monto) || monto < 0)
        {
            MostrarError("Ingresá un monto inicial válido.");
            return;
        }

        IsLoading = true;
        LimpiarError();
        try
        {
            // TODO: await _cajaServicio.AbrirAsync(sucursalId, usuarioId, monto)
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