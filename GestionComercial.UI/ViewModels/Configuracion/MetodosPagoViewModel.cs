using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class MetodosPagoViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;

        private ObservableCollection<MetodoPagoDto> _items = new();
        public ObservableCollection<MetodoPagoDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private MetodoPagoDto _seleccionado;
        public MetodoPagoDto Seleccionado
        {
            get => _seleccionado;
            set { _seleccionado = value; NotifyOfPropertyChange(() => Seleccionado); }
        }

        private string _editNombre    = string.Empty;
        private string _editCategoria = "Otro";
        private bool   _esNuevo;

        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public string EditCategoria
        {
            get => _editCategoria;
            set { _editCategoria = value; NotifyOfPropertyChange(() => EditCategoria); }
        }

        private bool   _panelVisible;
        private string _tituloPanel = "Nuevo Método";
        public bool PanelVisible
        {
            get => _panelVisible;
            set { _panelVisible = value; NotifyOfPropertyChange(() => PanelVisible); }
        }
        public string TituloPanel
        {
            get => _tituloPanel;
            set { _tituloPanel = value; NotifyOfPropertyChange(() => TituloPanel); }
        }

        public MetodosPagoViewModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var metodos = await _uow.MetodosPago.ObtenerTodosAsync();
                Items = new ObservableCollection<MetodoPagoDto>(
                    metodos.Select(m => new MetodoPagoDto
                    {
                        IdMetodoPago = m.Id,
                        Nombre       = m.Nombre,
                        Categoria    = m.Categoria ?? "Otro",
                        IdEmpresa    = m.Id_empresa
                    })
                );
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public void NuevoMetodo()
        {
            _esNuevo      = true;
            TituloPanel   = "Nuevo Método de Pago";
            EditNombre    = string.Empty;
            EditCategoria = "Otro";
            PanelVisible  = true;
        }

        public void Editar(MetodoPagoDto item)
        {
            _esNuevo     = false;
            TituloPanel  = "Editar Método de Pago";
            Seleccionado = item;
            EditNombre   = item.Nombre;
            EditCategoria = item.Categoria;
            PanelVisible = true;
        }

        public void CerrarPanel() => PanelVisible = false;

        public async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(EditNombre)) { MostrarError("El nombre es obligatorio."); return; }
            IsLoading = true;
            LimpiarError();
            try
            {
                if (_esNuevo)
                {
                    var empresa = await _uow.Empresas.PrimerODefaultAsync(e => e.Activo);
                    if (empresa == null)
                    {
                        MostrarError("No hay una empresa activa. Cree la empresa primero.");
                        return;
                    }

                    var metodo = new MetodoPago
                    {
                        Nombre     = EditNombre,
                        Categoria  = EditCategoria,
                        Activo     = true,
                        Id_empresa = empresa.Id
                    };

                    await _uow.MetodosPago.AgregarAsync(metodo);
                    await _uow.GuardarCambiosAsync();

                    Items.Add(new MetodoPagoDto
                    {
                        IdMetodoPago = metodo.Id,
                        Nombre       = metodo.Nombre,
                        Categoria    = metodo.Categoria ?? "Otro",
                        IdEmpresa    = metodo.Id_empresa
                    });
                }
                else if (Seleccionado != null)
                {
                    var metodo = await _uow.MetodosPago.ObtenerPorIdAsync(Seleccionado.IdMetodoPago);
                    if (metodo != null)
                    {
                        metodo.Nombre    = EditNombre;
                        metodo.Categoria = EditCategoria;
                        _uow.MetodosPago.Actualizar(metodo);
                        await _uow.GuardarCambiosAsync();

                        Seleccionado.Nombre    = metodo.Nombre;
                        Seleccionado.Categoria = metodo.Categoria ?? "Otro";
                        var idx = Items.IndexOf(Seleccionado);
                        Items.RemoveAt(idx);
                        Items.Insert(idx, Seleccionado);
                    }
                }
                PanelVisible = false;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Eliminar(MetodoPagoDto item)
        {
            IsLoading = true;
            try
            {
                var metodo = await _uow.MetodosPago.ObtenerPorIdAsync(item.IdMetodoPago);
                if (metodo != null)
                {
                    _uow.MetodosPago.Eliminar(metodo);
                    await _uow.GuardarCambiosAsync();
                    Items.Remove(item);
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
