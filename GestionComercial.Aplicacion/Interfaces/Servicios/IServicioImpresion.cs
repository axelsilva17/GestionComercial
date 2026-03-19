namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    /// <summary>
    /// Define las operaciones disponibles para el servicio de impresión térmica.
    /// Soporta modo test (archivo) y modo producción (ESCPOS).
    /// </summary>
    public interface IServicioImpresion
    {
        /// <summary>
        /// Imprime un ticket de venta con los pagos realizados.
        /// En modo test: guarda en archivo. En producción: envía a impresora ESCPOS.
        /// </summary>
        void ImprimirTicket(DTOs.Ventas.VentaDto venta, List<DTOs.Ventas.PagoDto> pagos);

        /// <summary>
        /// Configura los parámetros de la impresora.
        /// </summary>
        void Configurar(ImpresoraConfig config);

        /// <summary>
        /// Verifica si la impresora está conectada y disponible.
        /// En modo test siempre retorna true.
        /// </summary>
        bool VerificarConexion();
    }

    /// <summary>
    /// Configuración para el servicio de impresión térmica.
    /// </summary>
    public class ImpresoraConfig
    {
        public string NombreImpresora  { get; set; } = "POS-58";
        public string DirectorioTest   { get; set; } = "tickets_test";
        public bool   ModoTest         { get; set; } = true;  // Default TRUE por seguridad
        public int    AnchoCaracteres   { get; set; } = 48;
    }
}
