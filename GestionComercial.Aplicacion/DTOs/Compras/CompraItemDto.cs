using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionComercial.Aplicacion.DTOs.Compras
{
    /// <summary>
    /// Ítem de la compra en curso — usado en la UI para el carrito de compra.
    /// Implementa INotifyPropertyChanged para que los cambios de Cantidad/SubTotal
    /// se reflejen en el DataGrid sin necesidad de refrescar la colección.
    /// </summary>
    public class CompraItemDto : INotifyPropertyChanged
    {
        private int     _cantidad;
        private decimal _precioCosto;
        private decimal _subTotal;

        public int    ProductoId     { get; set; }
        public string? ProductoNombre { get; set; }
        public string? CodigoBarra    { get; set; }
        public string? CategoriaNombre { get; set; }

        public int Cantidad
        {
            get => _cantidad;
            set { _cantidad = value; OnPropertyChanged(); }
        }

        public decimal PrecioCosto
        {
            get => _precioCosto;
            set { _precioCosto = value; OnPropertyChanged(); }
        }

        public decimal SubTotal
        {
            get => _subTotal;
            set { _subTotal = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

namespace GestionComercial.Aplicacion.DTOs.Proveedores
{
    /// <summary>
    /// DTO liviano para mostrar proveedores en ComboBox.
    /// </summary>
   
}
