using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    /// <summary>
    /// DTO para reporte administrativo completo (exportable a Excel).
    /// </summary>
    public class ReporteAdminDto
    {
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        // Resumen de caja
        public ReporteCajaResumenDto? ResumenCaja { get; set; }

        // Ventas por día
        public List<ReporteCajaVentasPorDiaDto> VentasPorDia { get; set; } = new();

        // Formas de pago
        public List<ReporteCajaFormaPagoDto> FormasPago { get; set; } = new();

        // Movimientos manuales
        public List<ReporteCajaMovimientoManualDto> MovimientosManuales { get; set; } = new();

        // Top productos
        public List<ReporteCajaTopProductoDto> TopProductos { get; set; } = new();

        // Totales
        public decimal TotalVentas { get; set; }
        public decimal TotalEfectivo { get; set; }
        public decimal TotalTarjeta { get; set; }
        public decimal TotalTransferencia { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal TotalIngresos { get; set; }
    }

    public class ReporteCajaResumenDto
    {
        public int CantidadCajas { get; set; }
        public int CajasAbiertas { get; set; }
        public int CajasCerradas { get; set; }
        public decimal MontoInicialTotal { get; set; }
        public decimal MontoFinalTotal { get; set; }
    }

    public class ReporteCajaVentasPorDiaDto
    {
        public DateTime Fecha { get; set; }
        public int CantidadVentas { get; set; }
        public decimal Total { get; set; }
    }

    public class ReporteCajaFormaPagoDto
    {
        public string FormaPago { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class ReporteCajaMovimientoManualDto
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty; // Ingreso | Egreso
        public decimal Monto { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
    }

    public class ReporteCajaTopProductoDto
    {
        public int Posicion { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal Total { get; set; }
    }
}
