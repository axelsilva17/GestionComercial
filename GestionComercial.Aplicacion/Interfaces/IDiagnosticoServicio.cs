using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Servicios
{
    public interface IDiagnosticoServicio
    {
        Task<DiagnosticoResultDto> EjecutarAuditoriaCompletaAsync(int idEmpresa);
        Task<DiagnosticoResultDto> VerificarIntegridadAsync();
        Task OptimizarAsync();
        Task ReindexarAsync();
        Task LimpiarAsync();
    }

    public class DiagnosticoResultDto
    {
        public bool IntegridadOk { get; set; }
        public string MensajeIntegridad { get; set; } = string.Empty;
        public string DetalleIntegridad { get; set; } = string.Empty;
        public StatsDbDto? Stats { get; set; }
        public List<LogItemDto> Warnings { get; set; } = new();
    }

    public class StatsDbDto
    {
        public int TotalVentas { get; set; }
        public int TotalProductos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalCompras { get; set; }
        public int TotalProveedores { get; set; }
        public int TotalMetodosPago { get; set; }
        public int TotalCategorias { get; set; }
        public long TamanoDbBytes { get; set; }
        public string TamanoFormateado { get; set; } = string.Empty;
    }

    public class LogItemDto
    {
        public string Tipo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
