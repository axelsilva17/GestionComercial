using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionComercial.Aplicacion.DTOs.Productos
{
    public class FilaImportacionDto : INotifyPropertyChanged
    {
        private string _nombre = string.Empty;
        private string _codigoBarra = string.Empty;
        private decimal _precioVenta;
        private decimal _precioCosto;
        private int _stock;
        private int _stockMinimo;
        private string _categoria = string.Empty;
        private string _unidadMedida = string.Empty;
        private bool _esValida;
        private string _errorDescripcion = string.Empty;

        public int Fila { get; set; }
        public int? IdCategoria { get; set; }
        public bool EsNuevo { get; set; }

        public decimal PrecioVentaOriginal { get; set; }
        public decimal PrecioCostoOriginal { get; set; }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); Validar(); }
        }

        public string CodigoBarra
        {
            get => _codigoBarra;
            set { _codigoBarra = value; OnPropertyChanged(); }
        }

        public decimal PrecioVenta
        {
            get => _precioVenta;
            set { _precioVenta = value; OnPropertyChanged(); OnPropertyChanged(nameof(Margen)); Validar(); }
        }

        public decimal PrecioCosto
        {
            get => _precioCosto;
            set { _precioCosto = value; OnPropertyChanged(); OnPropertyChanged(nameof(Margen)); }
        }

        public void AplicarAjuste(decimal factor, bool ajustarVenta, bool ajustarCosto)
        {
            if (ajustarVenta)
                PrecioVenta = Math.Round(PrecioVentaOriginal * factor, 2);
            else
                PrecioVenta = PrecioVentaOriginal;

            if (ajustarCosto)
                PrecioCosto = Math.Round(PrecioCostoOriginal * factor, 2);
            else
                PrecioCosto = PrecioCostoOriginal;
        }

        public int Stock
        {
            get => _stock;
            set { _stock = value; OnPropertyChanged(); }
        }

        public int StockMinimo
        {
            get => _stockMinimo;
            set { _stockMinimo = value; OnPropertyChanged(); }
        }

        public string Categoria
        {
            get => _categoria;
            set { _categoria = value; OnPropertyChanged(); }
        }

        public string UnidadMedida
        {
            get => _unidadMedida;
            set { _unidadMedida = value; OnPropertyChanged(); }
        }

        public bool EsValida
        {
            get => _esValida;
            set { _esValida = value; OnPropertyChanged(); OnPropertyChanged(nameof(EstadoTexto)); }
        }

        public string ErrorDescripcion
        {
            get => _errorDescripcion;
            set { _errorDescripcion = value; OnPropertyChanged(); }
        }

        public string EstadoTexto => EsValida ? (EsNuevo ? "Nuevo" : "Actualizar") : "Error";

        public decimal Margen => PrecioVenta > 0 && PrecioCosto > 0
            ? Math.Round((PrecioVenta - PrecioCosto) / PrecioVenta * 100, 1)
            : 0;

        private void Validar()
        {
            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("Nombre vacío");
            if (PrecioVenta <= 0)
                errores.Add("Precio de venta inválido");
            EsValida = errores.Count == 0;
            ErrorDescripcion = string.Join("; ", errores);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
