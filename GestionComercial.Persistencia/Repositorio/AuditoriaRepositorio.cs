using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class AuditoriaRepositorio : IAuditoriaRepositorio
    {
        private readonly GestionComercialContext _context;

        public AuditoriaRepositorio(GestionComercialContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorRangoFechasAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .Include(a => a.Usuario)
                .Where(a => a.Fecha >= fechaDesde && a.Fecha <= fechaHasta)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync(cancellationToken);
        }

        public async Task<AuditoriaLog> AgregarAsync(
            AuditoriaLog auditoriaLog,
            CancellationToken cancellationToken = default)
        {
            await _context.AuditoriaLogs.AddAsync(auditoriaLog, cancellationToken);
            return auditoriaLog;
        }
    }
}
