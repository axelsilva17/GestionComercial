using System.Collections.Generic;
using System.Threading.Tasks;
using GestionComercial.Dominio.Entidades.Configuracion;

namespace GestionComercial.Aplicacion.Servicios
{
    public interface IReporteMantenimientoServicio
    {
        Task<string> GenerarReporteAsync(DiagnosticoResultDto diagnostico, string nombreEmpresa, int idEmpresa, BackupConfig? backupConfig = null, List<string>? accionesRealizadas = null);
        byte[] GenerarPdf(DiagnosticoResultDto diagnostico, string nombreEmpresa, int idEmpresa, BackupConfig? backupConfig = null, List<string>? accionesRealizadas = null);
    }
}
