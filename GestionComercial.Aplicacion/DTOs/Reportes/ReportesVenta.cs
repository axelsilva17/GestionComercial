namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    public class VentaPorDiaDto
    {
        public string Dia { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
    }

    public class VentaPorMetodoDto
    {
        public string MetodoNombre { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
        public string Icono { get; set; } = string.Empty;
    }
    public class ReporteProductoTopDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Ingresos { get; set; }
        public double Margen { get; set; }
    }

    public class ReporteVentaMensualDto
    {
        public string Mes { get; set; } = string.Empty;
        public decimal Ventas { get; set; }
        public decimal Compras { get; set; }
        public decimal Resultado { get; set; }
        public double Margen { get; set; }
    }
    public class VentaPorSucursalDto
    {
        public string SucursalNombre { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class KpiGeneralDto
    {
        public decimal TotalVentasPeriodo { get; set; }
        public decimal TotalVentasPeriodoAnterior { get; set; }
        public decimal VariacionVentas => TotalVentasPeriodoAnterior == 0 ? 0
            : ((TotalVentasPeriodo - TotalVentasPeriodoAnterior) / TotalVentasPeriodoAnterior) * 100;
        public decimal MargenPromedio { get; set; }
        public int ProductosBajoStock { get; set; }
        public int TotalTransacciones { get; set; }
        public decimal TicketPromedio { get; set; }
        public string MejorProducto { get; set; } = string.Empty;
        public string MejorVendedor { get; set; } = string.Empty;
    }

    public class ReporteClienteDto
    {
        public string ClienteNombre { get; set; } = string.Empty;
        public int CantidadCompras { get; set; }
        public decimal TotalComprado { get; set; }
        public decimal TicketPromedio { get; set; }
        public DateTime UltimaCompra { get; set; }
    }
}