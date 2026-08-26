namespace GestionComercial.Dominio.DTOs.Inventario
{
    public class ResumenMovimientoStockDto
    {
        public int TotalEntradas { get; set; }
        public int TotalSalidas { get; set; }
        public int TotalAjustes { get; set; }
        public int UnidadesIngresadas { get; set; }
        public int UnidadesEgresadas { get; set; }
        public int BalanceNeto => UnidadesIngresadas - UnidadesEgresadas;
    }
}
