using System;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    /// <summary>
    /// DTO para KPIs de prevención de fraude en caja.
    /// </summary>
    public class KpiFraudeDto
    {
        /// <summary>
        /// Caja con mayor diferencia entre ventas registradas y cobrado.
        /// </summary>
        public CajaMayorDiferenciaDto? CajaMayorDiferencia { get; set; }

        /// <summary>
        /// Ventas anuladas agrupadas por usuario.
        /// </summary>
        public List<VentaAnuladaDto> VentasAnuladasPorUsuario { get; set; } = new();

        /// <summary>
        /// Movimientos registrados fuera del horario laboral (antes 8am, después 10pm).
        /// </summary>
        public List<MovimientoFueraHorarioDto> MovimientosFueraHorario { get; set; } = new();

        /// <summary>
        /// Desglose de formas de pago por vendedor.
        /// </summary>
        public List<FormaPagoVendedorDto> FormasPagoPorVendedor { get; set; } = new();

        /// <summary>
        /// Resumen general de alertas.
        /// </summary>
        public int TotalAlertas => (CajaMayorDiferencia != null ? 1 : 0)
                                 + VentasAnuladasPorUsuario.Sum(v => v.Cantidad)
                                 + MovimientosFueraHorario.Count;
    }

    /// <summary>
    /// Información de la caja con mayor diferencia.
    /// </summary>
    public class CajaMayorDiferenciaDto
    {
        public int IdCaja { get; set; }
        public DateTime FechaApertura { get; set; }
        public string UsuarioCierre { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
        public decimal TotalCobrado { get; set; }
        public decimal Diferencia { get; set; }
        public decimal PorcentajeDiferencia { get; set; }
        public string NivelAlerta { get; set; } = "Normal"; // Normal | Warning | Critical
    }

    /// <summary>
    /// Ventas anuladas por usuario.
    /// </summary>
    public class VentaAnuladaDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal TotalAnulado { get; set; }
        public string PorcentajeDelTotal { get; set; } = "0%";
    }

    /// <summary>
    /// Movimiento fuera del horario laboral.
    /// </summary>
    public class MovimientoFueraHorarioDto
    {
        public int Id { get; set; }
        public int IdCaja { get; set; }
        public DateTime Fecha { get; set; }
        public int Hora { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Concepto { get; set; } = string.Empty;
        public string Razon { get; set; } = string.Empty; // "Antes de 8am" | "Después de 10pm"
    }

    /// <summary>
    /// Forma de pago utilizada por cada vendedor.
    /// </summary>
    public class FormaPagoVendedorDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public decimal TotalEfectivo { get; set; }
        public decimal TotalTarjeta { get; set; }
        public decimal TotalTransferencia { get; set; }
        public decimal TotalOtro { get; set; }
        public int CantidadVentas { get; set; }
    }
}
