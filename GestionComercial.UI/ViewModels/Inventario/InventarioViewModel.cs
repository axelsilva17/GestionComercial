using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GestionComercial.Aplicacion.DTOs.Inventario;  
using GestionComercial.Aplicacion.DTOs.Productos;  

namespace GestionComercial.UI.ViewModels.Inventario
{
    public class InventarioViewModel : NavigableViewModel
    {
        public InventarioViewModel()
        {
            Titulo    = "Inventario";
            Subtitulo = "Movimientos de stock";
        }

        private ObservableCollection<MovimientoStockDto> _movimientos = new();
        public ObservableCollection<MovimientoStockDto> Movimientos
        {
            get => _movimientos;
            set { _movimientos = value; NotifyOfPropertyChange(() => Movimientos); }
        }

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; NotifyOfPropertyChange(() => PaginaActual); }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; NotifyOfPropertyChange(() => TotalPaginas); }
        }

        private int _totalMovimientos;
        public int TotalMovimientos
        {
            get => _totalMovimientos;
            set { _totalMovimientos = value; NotifyOfPropertyChange(() => TotalMovimientos); }
        }

        private int _movimientosMostrados;
        public int MovimientosMostrados
        {
            get => _movimientosMostrados;
            set { _movimientosMostrados = value; NotifyOfPropertyChange(() => MovimientosMostrados); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(200);
                Movimientos = new ObservableCollection<MovimientoStockDto>();
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Buscar()
        {
            PaginaActual = 1;
            await CargarAsync();
        }

        public async Task PaginaAnterior()
        {
            if (PaginaActual > 1) { PaginaActual--; await CargarAsync(); }
        }

        public async Task PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) { PaginaActual++; await CargarAsync(); }
        }
    }


}
