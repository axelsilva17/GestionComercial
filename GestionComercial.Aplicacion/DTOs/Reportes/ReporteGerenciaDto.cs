using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    /// <summary>
    /// DTO para reporte gerencial con métricas KPIs y tendencias.
    /// </summary>
    public class ReporteGerenciaDto
    {
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        // Período
        public int DiasDelPeriodo => (FechaHasta - FechaDesde).Days + 1;

        // Resumen ejecutivo
        public ResumenEjecutivoDto? ResumenEjecutivo { get; set; }

        // KPIs de ventas
        public List<KpiVentaDto> KpisVentas { get; set; } = new();

        // Tendencias diarias
        public List<TendenciaDiariaDto> TendenciasDiarias { get; set; } = new();

        // Comparativa por vendedor
        public List<ComparativaVendedorDto> ComparativaVendedores { get; set; } = new();

        // Productos más vendidos y con mayor margen
        public List<ProductoRendimientoDto> ProductosMayorMargen { get; set; } = new();
        public List<ProductoRendimientoDto> ProductosMasVendidos { get; set; } = new();

        // Análisis de horarios
        public AnalisisHorariosDto? AnalisisHorarios { get; set; }
    }

    public class ResumenEjecutivoDto
    {
        public decimal TotalVentas { get; set; }
        public decimal TotalCosto { get; set; }
        public decimal GananciaBruta { get; set; }
        public decimal MargenBrutoPorcentaje { get; set; }
        public int CantidadTransacciones { get; set; }
        public decimal TicketPromedio { get; set; }
        public decimal DescuentosOtorgados { get; set; }
        public decimal PorcentajeDescuentos { get; set; }
    }

    public class KpiVentaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal ValorAnterior { get; set; }
        public decimal VariacionPorcentaje { get; set; }
        public string Tendencia { get; set; } = "Stable"; // Up | Down | Stable
        public string Formato { get; set; } = "Currency"; // Currency | Percentage | Number
    }

    public class TendenciaDiariaDto
    {
        public DateTime Fecha { get; set; }
        public decimal Ventas { get; set; }
        public decimal Costo { get; set; }
        public decimal Ganancia { get; set; }
        public int Transacciones { get; set; }
    }

    public class ComparativaVendedorDto
    {
        public int Posicion { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
        public int CantidadVentas { get; set; }
        public decimal TicketPromedio { get; set; }
        public decimal MargenPromedio { get; set; }
        public decimal PorcentajeDelTotal { get; set; }
    }

    public class ProductoRendimientoDto
    {
        public int Posicion { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
        public decimal MargenPromedio { get; set; }
    }

    public class AnalisisHorariosDto
    {
        public HorarioPicoDto? HorarioPico { get; set; }
        public List<ResumenHorarioDto> ResumenPorHorario { get; set; } = new();
    }

    public class HorarioPicoDto
    {
        public TimeSpan Hora { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
        public int Transacciones { get; set; }
    }

    public class ResumenHorarioDto
    {
        public string Rango { get; set; } = string.Empty; // "08:00 - 10:00"
        public decimal Total { get; set; }
        public int Transacciones { get; set; }
        public decimal PorcentajeDelTotal { get; set; }
    }
}
