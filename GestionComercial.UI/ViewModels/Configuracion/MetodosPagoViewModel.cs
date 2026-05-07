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
        private bool   _editEfectivo  = false;
        private bool   _esNuevo;

        public string EditNombre
        {
            get => _editNombre;
            set { _editNombre = value; NotifyOfPropertyChange(() => EditNombre); }
        }
        public bool EditEfectivo
        {
            get => _editEfectivo;
            set { _editEfectivo = value; NotifyOfPropertyChange(() => EditEfectivo); }
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
                var metodos = await _uow.MetodosPago.ObtenerTodasAsync();
                Items = new ObservableCollection<MetodoPagoDto>(
                    metodos.Select(m => new MetodoPagoDto
                    {
                        IdMetodoPago = m.Id,
                        Nombre       = m.Nombre,
                        EsEfectivo   = m.EsEfectivo,
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
            EditEfectivo  = false;
            PanelVisible  = true;
        }

        public void Editar(MetodoPagoDto item)
        {
            _esNuevo     = false;
            TituloPanel  = "Editar Método de Pago";
            Seleccionado = item;
            EditNombre   = item.Nombre;
            EditEfectivo = item.EsEfectivo;
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
                        EsEfectivo = EditEfectivo,
                        Activo     = true,
                        Id_empresa = empresa.Id
                    };

                    await _uow.MetodosPago.AgregarAsync(metodo);
                    await _uow.GuardarCambiosAsync();

                    Items.Add(new MetodoPagoDto
                    {
                        IdMetodoPago = metodo.Id,
                        Nombre       = metodo.Nombre,
                        EsEfectivo   = metodo.EsEfectivo,
                        IdEmpresa    = metodo.Id_empresa
                    });
                }
                else if (Seleccionado != null)
                {
                    var metodo = await _uow.MetodosPago.ObtenerPorIdAsync(Seleccionado.IdMetodoPago);
                    if (metodo != null)
                    {
                        metodo.Nombre     = EditNombre;
                        metodo.EsEfectivo = EditEfectivo;
                        _uow.MetodosPago.Actualizar(metodo);
                        await _uow.GuardarCambiosAsync();

                        Seleccionado.Nombre     = metodo.Nombre;
                        Seleccionado.EsEfectivo = metodo.EsEfectivo;
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
