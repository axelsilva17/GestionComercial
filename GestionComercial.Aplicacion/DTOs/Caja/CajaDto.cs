using System;

namespace GestionComercial.Aplicacion.DTOs.Caja
{
    /// <summary>
    /// Representa un movimiento individual de caja (ingreso o egreso manual, o venta).
    /// </summary>
    public class MovimientoCajaDto
    {
        public int IdMovimiento { get; set; }
        public string Tipo { get; set; }  // "Ingreso" | "Egreso" | "Venta"
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public int? ReferenciaId { get; set; }  // IdVenta si aplica

        // Para mostrar en UI
        public bool EsIngreso => Tipo == "Ingreso" || Tipo == "Venta";
        public bool EsApertura => Tipo == "Apertura";
        public string TipoIcono => Tipo switch
        {
            "Apertura" => "◯",
            "Cierre" => "◉",
            "Venta" => "◈",
            "Ingreso" => "↑",
            "Egreso" => "↓",
            _ => "·"
        };
        public string TipoDisplay => Tipo switch
        {
            "Apertura" => "INICIO",
            "Cierre" => "CIERRE",
            "Venta" => "VENTA",
            "Ingreso" => "INGRESO",
            "Egreso" => "EGRESO",
            _ => "OTRO"
        };
    }
    public class MovimientoAuditoriaDto
    {
        public string TipoOperacion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Fecha { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;
        public bool EsIngreso { get; set; }
    }

    /// <summary>
    /// DTO para mostrar un cierre de caja en la auditoría.
    /// </summary>
    public class CajaAuditoriaItemDto
    {
        public int Id { get; set; }
        public string FechaApertura { get; set; } = string.Empty;
        public string HoraApertura { get; set; } = string.Empty;
        public string UsuarioApertura { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public decimal MontoInicial { get; set; }
        public decimal VentasEfectivo { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal EfectivoEnCaja { get; set; } // MontoInicial + VentasEfectivo + Ingresos - Egresos
        public decimal? MontoFinal { get; set; }
        public decimal DiferenciaSinEfectivo { get; set; } // Conteo físico - MontoInicial (sin ventas)
        public decimal DiferenciaConEfectivo { get; set; } // Conteo físico - (MontoInicial + VentasEfectivo + Ingresos - Egresos)
        public string? FechaCierre { get; set; }
        public string? UsuarioCierre { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string EstadoColor { get; set; } = "#10B981"; // Verde por defecto
    }

}