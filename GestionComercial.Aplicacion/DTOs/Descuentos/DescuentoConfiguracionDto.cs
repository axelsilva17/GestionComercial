namespace GestionComercial.Aplicacion.DTOs.Descuentos
{
    public class DescuentoConfiguracionDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int? Id_producto { get; set; }
        public int? Id_categoria { get; set; }
        public bool AplicaCualquierMetodoPago { get; set; } = true;
        public List<int> MetodosPagoIds { get; set; } = new();
        public int Id_empresa { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal? MontoMinimoCompra { get; set; }
    }

    public class DescuentoListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int? Id_producto { get; set; }
        public string? ProductoNombre { get; set; }
        public int? Id_categoria { get; set; }
        public string? CategoriaNombre { get; set; }
        public bool AplicaCualquierMetodoPago { get; set; } = true;
        public string MetodosPagoNombres { get; set; } = string.Empty;
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool Activo { get; set; }
        public string Alcance { get; set; } = string.Empty;
        public decimal? MontoMinimoCompra { get; set; }

        public string Tipo => Alcance switch
        {
            "Producto" => "Producto",
            "Categoria" => "Categoría",
            "MetodoPago" => "Método de Pago",
            "CompraMayor" => "Compra Mayor",
            _ => "Global"
        };

        public string ObjetoNombre => Alcance switch
        {
            "Producto" => ProductoNombre ?? "Todos",
            "Categoria" => CategoriaNombre ?? "Todos",
            "CompraMayor" => MontoMinimoCompra.HasValue
                ? $">= ${MontoMinimoCompra.Value:N2}"
                : "Todos",
            _ => "Todos"
        };

        public string MetodoPagoNombre => AplicaCualquierMetodoPago
            ? "Todos"
            : MetodosPagoNombres;
    }
}