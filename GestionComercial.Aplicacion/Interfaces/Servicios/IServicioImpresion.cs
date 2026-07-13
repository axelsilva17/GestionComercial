namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    ///     /// Define las operaciones disponibles para el servicio de impresión térmica.
    /// Soporta modo test (archivo) y modo producción (ESCPOS).
    public interface IServicioImpresion
    {
        ///         /// Imprime un ticket de venta con los pagos realizados.
        /// En modo test: guarda en archivo. En producción: envía a impresora ESCPOS.
        void ImprimirTicket(DTOs.Ventas.VentaDto venta, List<DTOs.Ventas.PagoDto> pagos);

        ///         /// Configura los parámetros de la impresora.
        void Configurar(ImpresoraConfig config);

        ///         /// Verifica si la impresora está conectada y disponible.
        /// En modo test siempre retorna true.
        bool VerificarConexion();
    }

    ///     /// Configuración para el servicio de impresión térmica.
    public class ImpresoraConfig
    {
        public string NombreImpresora  { get; set; } = "POS-58";
        public string DirectorioTest   { get; set; } = "tickets_test";
        public bool   ModoTest         { get; set; } = true;  // Default TRUE por seguridad
        public int    AnchoCaracteres   { get; set; } = 48;
    }
}
