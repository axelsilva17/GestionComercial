using GestionComercial.Aplicacion.DTOs.Ventas;

namespace GestionComercial.Infraestructura.Servicios.Impresion
{
    public interface IServicioImpresion
    {
        void ImprimirTicket(VentaDto venta);
        void ConfigurarImpresora(string nombreImpresora);
        bool VerificarConexion();
    }
}