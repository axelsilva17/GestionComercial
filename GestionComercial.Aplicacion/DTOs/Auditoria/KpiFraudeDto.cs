using System;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    ///     /// DTO para KPIs de prevención de fraude en caja.
    public class KpiFraudeDto
    {
        ///         /// Caja con mayor diferencia entre ventas registradas y cobrado.
        public CajaMayorDiferenciaDto? CajaMayorDiferencia { get; set; }

        ///         /// Ventas anuladas agrupadas por usuario.
        public List<VentaAnuladaDto> VentasAnuladasPorUsuario { get; set; } = new();

        ///         /// Movimientos registrados fuera del horario laboral (antes 8am, después 10pm).
        public List<MovimientoFueraHorarioDto> MovimientosFueraHorario { get; set; } = new();

        ///         /// Desglose de formas de pago por vendedor.
        public List<FormaPagoVendedorDto> FormasPagoPorVendedor { get; set; } = new();

        ///         /// Resumen general de alertas.
        public int TotalAlertas => (CajaMayorDiferencia != null ? 1 : 0)
                                 + VentasAnuladasPorUsuario.Sum(v => v.Cantidad)
                                 + MovimientosFueraHorario.Count;
    }

    ///     /// Información de la caja con mayor diferencia.
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

    ///     /// Ventas anuladas por usuario.
    public class VentaAnuladaDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal TotalAnulado { get; set; }
        public string PorcentajeDelTotal { get; set; } = "0%";
    }

    ///     /// Movimiento fuera del horario laboral.
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

    ///     /// Forma de pago utilizada por cada vendedor.
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
