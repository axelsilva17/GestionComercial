using Caliburn.Micro;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Compras
{


    // ══════════════════════════════════════════════════════════════════════════════
    // COMPRA VIEW MODEL — nueva compra
    // ══════════════════════════════════════════════════════════════════════════════
    public class CompraViewModel : NavigableViewModel
    {
        private ObservableCollection<ProveedorItemDto> _proveedores = new();
        public ObservableCollection<ProveedorItemDto> Proveedores
        {
            get => _proveedores;
            set { _proveedores = value; NotifyOfPropertyChange(() => Proveedores); }
        }

        private ProveedorItemDto _proveedorSeleccionado;
        public ProveedorItemDto ProveedorSeleccionado
        {
            get => _proveedorSeleccionado;
            set
            {
                _proveedorSeleccionado = value;
                NotifyOfPropertyChange(() => ProveedorSeleccionado);
                NotifyOfPropertyChange(() => ProveedorNombre);
            }
        }

        public string ProveedorNombre => ProveedorSeleccionado?.Nombre;

        private string _busquedaProducto = string.Empty;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set { _busquedaProducto = value; NotifyOfPropertyChange(() => BusquedaProducto); }
        }

        private ObservableCollection<CompraItemDto> _items = new();
        public ObservableCollection<CompraItemDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
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

        public RelayCommand<CompraItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<CompraItemDto> RestarCantidadCommand { get; }
        public RelayCommand<CompraItemDto> QuitarItemCommand     { get; }

        public CompraViewModel()
        {
            SumarCantidadCommand  = new RelayCommand<CompraItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<CompraItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<CompraItemDto>(QuitarItem);
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            // TODO: await _proveedorServicio.ObtenerTodosAsync(empresaId)
            Proveedores = new ObservableCollection<ProveedorItemDto>();
        }

        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;

            // TODO: buscar en _productoServicio
            Items.Add(new CompraItemDto
            {
                ProductoId     = 1,
                ProductoNombre = BusquedaProducto,
                CodigoBarra    = "0000000",
                Cantidad       = 1,
                PrecioCosto    = 0,
                SubTotal       = 0
            });

            BusquedaProducto = string.Empty;
            RecalcularTotal();
        }

        public async Task ConfirmarCompra()
        {
            if (ProveedorSeleccionado == null) { MostrarError("Seleccioná un proveedor."); return; }
            if (Items.Count == 0) { MostrarError("Agregá al menos un producto."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                // TODO: await _compraServicio.CrearAsync(dto)
                await Task.Delay(300);
                await Volver();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CompraListadoViewModel>(), CancellationToken.None);
        }

        private void SumarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            item.Cantidad++;
            item.SubTotal = item.Cantidad * item.PrecioCosto;
            RefrescarItem(item);
            RecalcularTotal();
        }

        private void RestarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad <= 1) { QuitarItem(item); return; }
            item.Cantidad--;
            item.SubTotal = item.Cantidad * item.PrecioCosto;
            RefrescarItem(item);
            RecalcularTotal();
        }

        private void QuitarItem(CompraItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotal();
        }

        private void RefrescarItem(CompraItemDto item)
        {
            var idx = Items.IndexOf(item);
            if (idx < 0) return;
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
        }

        private void RecalcularTotal()
        {
            decimal t = 0;
            foreach (var i in Items) t += i.SubTotal;
            Total = t;
        }
    }

    

    // ── DTOs locales ──────────────────────────────────────────────────────────────
    public class CompraResumenDto
    {
        public int      IdCompra         { get; set; }
        public DateTime Fecha            { get; set; }
        public string   ProveedorNombre  { get; set; }
        public string   ProveedorInicial => string.IsNullOrEmpty(ProveedorNombre) ? "?" : ProveedorNombre[0].ToString().ToUpper();
        public string   UsuarioNombre    { get; set; }
        public int      CantidadProductos { get; set; }
        public decimal  Total            { get; set; }
        public ObservableCollection<CompraItemDto> Items { get; set; } = new();
    }

    public class CompraItemDto
    {
        public int     ProductoId     { get; set; }
        public string  ProductoNombre { get; set; }
        public string  CodigoBarra    { get; set; }
        public string  CategoriaNombre { get; set; }
        public int     Cantidad       { get; set; }
        public decimal PrecioCosto    { get; set; }
        public decimal SubTotal       { get; set; }
    }

    public class ProveedorItemDto
    {
        public int    Id       { get; set; }
        public string Nombre   { get; set; }
        public string Telefono { get; set; }
        public string Email    { get; set; }
    }
}
