namespace GestionComercial.Aplicacion.DTOs.Caja
{
    /// <summary>
    /// Resumen completo de un turno de caja para mostrar en el cierre.
    /// Solo VentasEfectivo e Ingresos/Egresos afectan el SaldoEsperado físico.
    /// Los demás métodos son informativos.
    /// </summary>
    public class ResumenCierreDto
    {
        // ── Efectivo (afecta saldo físico) ────────────────────────────────────
        public decimal MontoInicial      { get; set; }
        public decimal VentasEfectivo    { get; set; }  // pagos con EsEfectivo = true
        public decimal IngresosEfectivo  { get; set; }  // movimientos manuales tipo Ingreso
        public decimal EgresosEfectivo   { get; set; }  // movimientos manuales tipo Egreso

        /// <summary>Saldo que debería haber físicamente en caja.</summary>
        public decimal SaldoEsperado =>
            MontoInicial + VentasEfectivo + IngresosEfectivo - EgresosEfectivo;

        // ── Otros métodos (informativos, no afectan caja física) ──────────────
        public decimal VentasTarjeta       { get; set; }
        public decimal VentasTransferencia { get; set; }
        public decimal VentasQR            { get; set; }
        public decimal VentasCuentaCte     { get; set; }
        public decimal VentasOtros         { get; set; }

        /// <summary>Total vendido en el turno (todos los métodos).</summary>
        public decimal TotalVendido =>
            VentasEfectivo + VentasTarjeta + VentasTransferencia +
            VentasQR + VentasCuentaCte + VentasOtros;

        /// <summary>Cantidad de transacciones del turno.</summary>
        public int CantidadVentas { get; set; }

        // ── Desglose libre (para mostrar cada método) ─────────────────────────
        public List<DesglosePagoDto> DesglosePorMetodo { get; set; } = new();

        // ── Timestamps ───────────────────────────────────────────────────────
        public DateTime FechaApertura { get; set; }
    }

    public class DesglosePagoDto
    {
        public string  Metodo     { get; set; } = string.Empty;
        public decimal Total      { get; set; }
        public int     Cantidad   { get; set; }
        public bool    EsEfectivo { get; set; }
    }
}
