using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    ///     /// Implementación del repositorio de auditoría.
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
            int? idSucursal = null,
            CancellationToken ct = default)
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

            await _context.AuditoriaLogs.AddAsync(auditoriaLog, ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorTablaYRegistroAsync(
            string nombreTabla,
            int registroId,
            CancellationToken ct = default)
        {
            return await _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.NombreTabla == nombreTabla && a.RegistroId == registroId)
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorUsuarioAsync(
            int idUsuario,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.IdUsuario == idUsuario);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorEmpresaAsync(
            int idEmpresa,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.IdEmpresa == idEmpresa);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorSucursalAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.IdSucursal == idSucursal);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaCajaAsync(
            int? idCaja = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.NombreTabla == "Cajas");

            if (idCaja.HasValue)
                query = query.Where(a => a.RegistroId == idCaja.Value);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaMovimientoCajaAsync(
            int? idMovimiento = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.NombreTabla == "MovimientosCaja");

            if (idMovimiento.HasValue)
                query = query.Where(a => a.RegistroId == idMovimiento.Value);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaOperacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaOperacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaOperacion)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaFiltradaAsync(
            int? idUsuario = null,
            int? tipoOperacion = null,
            string? nombreTabla = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking().AsQueryable();

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
                .ToListAsync(ct);
        }

        public async Task<(IEnumerable<AuditoriaLog> Items, int Total)> ObtenerAuditoriaPaginadaAsync(
            int? idUsuario,
            int? tipoOperacion,
            string? nombreTabla,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina,
            int tamanioPagina,
            CancellationToken ct = default)
        {
            var query = _context.AuditoriaLogs.AsNoTracking().AsQueryable();

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

            var ordenada = query.OrderByDescending(a => a.FechaOperacion);

            // ── Count en SQL + página en SQL ──
            var total = await ordenada.CountAsync(ct);
            var items = await ordenada
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync(ct);

            return (items, total);
        }

        // ── Totales agregados en SQL (sin materializar filas) ─────────────────────
        public async Task<(int TotalRegistros, decimal DiferenciaTotal)> ObtenerAuditoriaCajaTotalesAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct = default)
        {
            // Replica el cálculo de ExportHelper (MontoFinal - MontoInicial del JSON):
            // las filas sin clave o con JSON inválido contribuyen 0 en ambos caminos.
            var rows = await _context.Database
                .SqlQueryRaw<AuditoriaCajaTotalesRaw>(
                    @"SELECT COUNT(*) AS TotalRegistros,
                             COALESCE(SUM(CAST(json_extract(ValoresNuevos, '$.MontoFinal') AS REAL)
                                        - CAST(json_extract(ValoresNuevos, '$.MontoInicial') AS REAL)), 0) AS DiferenciaTotal
                      FROM AuditoriaLogs
                      WHERE NombreTabla = 'Cajas'
                        AND FechaOperacion >= {0}
                        AND FechaOperacion <= {1}",
                    fechaDesde, fechaHasta)
                .ToListAsync(ct);

            var r = rows.FirstOrDefault() ?? new AuditoriaCajaTotalesRaw(0, 0);
            return (r.TotalRegistros, r.DiferenciaTotal);
        }

        // ── Últimos registros en SQL (proyección limitada, sin materializar todo) ──
        public async Task<List<AuditoriaLog>> ObtenerAuditoriaCajaRecienteAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int take,
            CancellationToken ct = default)
            => await _context.AuditoriaLogs.AsNoTracking()
                .Where(a => a.NombreTabla == "Cajas"
                         && a.FechaOperacion >= fechaDesde
                         && a.FechaOperacion <= fechaHasta)
                .OrderByDescending(a => a.FechaOperacion)
                .Take(take)
                .ToListAsync(ct);
    }
}