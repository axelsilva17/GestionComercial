// ══════════════════════════════════════════════════════════════════════════════
// COMPRA DETALLE VIEW MODEL
// ══════════════════════════════════════════════════════════════════════════════
using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Compras;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;

public class CompraDetalleViewModel : NavigableViewModel
{
    private int _idCompra;
    public int IdCompra
    {
        get => _idCompra;
        set { _idCompra = value; NotifyOfPropertyChange(() => IdCompra); }
    }

    private DateTime _fecha;
    public DateTime Fecha
    {
        get => _fecha;
        set { _fecha = value; NotifyOfPropertyChange(() => Fecha); }
    }

    private string _proveedorNombre = string.Empty;
    public string ProveedorNombre
    {
        get => _proveedorNombre;
        set { _proveedorNombre = value; NotifyOfPropertyChange(() => ProveedorNombre); }
    }

    private string _proveedorTelefono = string.Empty;
    public string ProveedorTelefono
    {
        get => _proveedorTelefono;
        set { _proveedorTelefono = value; NotifyOfPropertyChange(() => ProveedorTelefono); }
    }

    private string _sucursalNombre = string.Empty;
    public string SucursalNombre
    {
        get => _sucursalNombre;
        set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
    }

    private string _usuarioNombre = string.Empty;
    public string UsuarioNombre
    {
        get => _usuarioNombre;
        set { _usuarioNombre = value; NotifyOfPropertyChange(() => UsuarioNombre); }
    }

    private int _cantidadProductos;
    public int CantidadProductos
    {
        get => _cantidadProductos;
        set { _cantidadProductos = value; NotifyOfPropertyChange(() => CantidadProductos); }
    }

    private int _unidadesTotales;
    public int UnidadesTotales
    {
        get => _unidadesTotales;
        set { _unidadesTotales = value; NotifyOfPropertyChange(() => UnidadesTotales); }
    }

    private decimal _total;
    public decimal Total
    {
        get => _total;
        set { _total = value; NotifyOfPropertyChange(() => Total); }
    }

    private string _observacion = string.Empty;
    public string Observacion
    {
        get => _observacion;
        set { _observacion = value; NotifyOfPropertyChange(() => Observacion); }
    }

    private ObservableCollection<CompraItemDto> _items = new();
    public ObservableCollection<CompraItemDto> Items
    {
        get => _items;
        set { _items = value; NotifyOfPropertyChange(() => Items); }
    }

    public void InicializarConCompra(CompraResumenDto compra)
    {
        IdCompra = compra.IdCompra;
        Fecha = compra.Fecha;
        ProveedorNombre = compra.ProveedorNombre;
        Total = compra.Total;
    }

    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (IdCompra == 0) return;
        IsLoading = true;
        try
        {
            // TODO: await _compraServicio.ObtenerDetalleAsync(IdCompra)
            await Task.Delay(200);
            Items = new ObservableCollection<CompraItemDto>();
        }
        finally { IsLoading = false; }
    }

    public async Task Volver()
    {
        await IoC.Get<ShellViewModel>()
                 .ActivateItemAsync(IoC.Get<CompraListadoViewModel>(), CancellationToken.None);
    }
}