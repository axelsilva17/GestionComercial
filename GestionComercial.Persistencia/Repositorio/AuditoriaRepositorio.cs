using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    /// <summary>
    /// Implementación del repositorio de auditoría.
    /// </summary>
    public class AuditoriaRepositorio : IAuditoriaRepositorio
    {
        private readonly GestionComercialContext _context;

        public AuditoriaRepositorio(GestionComercialContext context)
        {
            _context = context;
        }

        public async Task RegistrarAuditoriaAsync(
            string nombreTabla,
            int registroId,
            OperacionAuditoriaEnum tipoOperacion,
            int? idUsuario,
            string? nombreUsuario,
            string? valoresAnteriores,
            string? valoresNuevos,
            string? workstation = null,
            int? idEmpresa = null,
            int? idSucursal = null)
        {
            var auditoriaLog = new AuditoriaLog
            {
                NombreTabla = nombreTabla,
                RegistroId = registroId,
                TipoOperacion = (int)tipoOperacion,
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                FechaOperacion = DateTime.Now,
                ValoresAnteriores = valoresAnteriores,
                ValoresNuevos = valoresNuevos,
                Workstation = workstation,
                IdEmpresa = idEmpresa,
                IdSucursal = idSucursal
            };

            await _context.AuditoriaLogs.AddAsync(auditoriaLog);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorTablaYRegistroAsync(
            string nombreTabla,
            int registroId)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.NombreTabla == nombreTabla && a.RegistroId == registroId)
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorUsuarioAsync(
            int idUsuario,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AuditoriaLogs
                .Where(a => a.IdUsuario == idUsuario);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorEmpresaAsync(
            int idEmpresa,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AuditoriaLogs
                .Where(a => a.IdEmpresa == idEmpresa);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorSucursalAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AuditoriaLogs
                .Where(a => a.IdSucursal == idSucursal);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaCajaAsync(
            int? idCaja = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AuditoriaLogs
                .Where(a => a.NombreTabla == "Cajas");

            if (idCaja.HasValue)
                query = query.Where(a => a.RegistroId == idCaja.Value);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaMovimientoCajaAsync(
            int? idMovimiento = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AuditoriaLogs
                .Where(a => a.NombreTabla == "MovimientosCaja");

            if (idMovimiento.HasValue)
                query = query.Where(a => a.RegistroId == idMovimiento.Value);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaFiltradaAsync(
            int? idUsuario = null,
            int? tipoOperacion = null,
            string? nombreTabla = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AuditoriaLogs.AsQueryable();

            if (idUsuario.HasValue)
                query = query.Where(a => a.IdUsuario == idUsuario.Value);

            if (tipoOperacion.HasValue)
                query = query.Where(a => a.TipoOperacion == tipoOperacion.Value);

            if (!string.IsNullOrWhiteSpace(nombreTabla))
                query = query.Where(a => a.NombreTabla == nombreTabla);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync();
        }
    }
}
