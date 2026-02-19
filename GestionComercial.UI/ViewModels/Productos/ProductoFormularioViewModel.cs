using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;

using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.UI.ViewModels.Productos;

public class ProductoFormularioViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        private bool _esModoEdicion;
        private string _nombre;
        private string _sku;
        private string _codigoBarra;
        private decimal _precioVentaActual;
        private decimal _precioCostoActual;
        private int _stockActual;
        private int _stockMinimo;
        private bool _activo = true;
        private string _descripcion;

        public ProductoFormularioViewModel(ShellViewModel shell)
        {
            _shell = shell;
        }

        public bool EsModoEdicion
        {
            get => _esModoEdicion;
            set
            {
                _esModoEdicion = value;
                NotifyOfPropertyChange(() => EsModoEdicion);
                NotifyOfPropertyChange(() => TituloFormulario);
                NotifyOfPropertyChange(() => SubtituloFormulario);
            }
        }

        public string TituloFormulario => EsModoEdicion ? "Editar Producto" : "Nuevo Producto";
        public string SubtituloFormulario => EsModoEdicion
            ? "Modificá los datos del producto"
            : "Completá los datos para crear un nuevo producto";

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; NotifyOfPropertyChange(() => Nombre); NotifyOfPropertyChange(() => CanGuardar); }
        }
        public string SKU
        {
            get => _sku;
            set { _sku = value; NotifyOfPropertyChange(() => SKU); }
        }
        public string CodigoBarra
        {
            get => _codigoBarra;
            set { _codigoBarra = value; NotifyOfPropertyChange(() => CodigoBarra); }
        }
        public decimal PrecioVentaActual
        {
            get => _precioVentaActual;
            set { _precioVentaActual = value; NotifyOfPropertyChange(() => PrecioVentaActual); NotifyOfPropertyChange(() => MargenCalculado); }
        }
        public decimal PrecioCostoActual
        {
            get => _precioCostoActual;
            set { _precioCostoActual = value; NotifyOfPropertyChange(() => PrecioCostoActual); NotifyOfPropertyChange(() => MargenCalculado); }
        }
        public int StockActual
        {
            get => _stockActual;
            set { _stockActual = value; NotifyOfPropertyChange(() => StockActual); }
        }
        public int StockMinimo
        {
            get => _stockMinimo;
            set { _stockMinimo = value; NotifyOfPropertyChange(() => StockMinimo); }
        }
        public bool Activo
        {
            get => _activo;
            set { _activo = value; NotifyOfPropertyChange(() => Activo); }
        }
        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; NotifyOfPropertyChange(() => Descripcion); }
        }

        public string MargenCalculado
        {
            get
            {
                if (PrecioCostoActual <= 0) return "—";
                var margen = ((PrecioVentaActual - PrecioCostoActual) / PrecioCostoActual) * 100;
                return $"{margen:N1}%";
            }
        }

        public bool CanGuardar => !string.IsNullOrWhiteSpace(Nombre) && !IsLoading;

        public async void Guardar()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // TODO: llamar al servicio para guardar en BD
                await System.Threading.Tasks.Task.Delay(500); // simula guardado
                await Volver();
            }
            catch (System.Exception ex)
            {
                MostrarError($"Error al guardar: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void Cancelar() => await Volver();

        public async System.Threading.Tasks.Task Volver()
        {
            await _shell.ActivateItemAsync(IoC.Get<ProductoListadoViewModel>(), CancellationToken.None);
        }

    internal void InicializarParaCrear()
    {
        throw new NotImplementedException();
    }

    internal void InicializarParaEditar(int idProducto)
    {
        throw new NotImplementedException();
    }
}
