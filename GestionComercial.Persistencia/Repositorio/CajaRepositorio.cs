using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class CajaRepositorio : RepositorioBase<Caja>, ICajaRepositorio
    {
        public CajaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(c => c.UsuarioApertura)
                .FirstOrDefaultAsync(c => c.Id_sucursal == idSucursal && c.Estado == 1, ct);

        public async Task<bool> ExisteCajaAbiertaAsync(int idSucursal, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.Id_sucursal == idSucursal && c.Estado == 1, ct);

        public async Task<Caja?> ObtenerConMovimientosAsync(int idCaja, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(c => c.Movimientos)
                .Include(c => c.UsuarioApertura)
                .Include(c => c.UsuarioCierre)
                .FirstOrDefaultAsync(c => c.Id == idCaja, ct);

        public async Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(c => c.Ventas)
                .Include(c => c.Movimientos)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.UsuarioApertura)
                .Include(c => c.UsuarioCierre)
                .Where(c => c.Id_sucursal == idSucursal && c.FechaApertura >= desde && c.FechaApertura <= hasta && c.Activo)
                .OrderByDescending(c => c.FechaApertura)
                .ToListAsync(ct);

        // ── Nuevo: historial con proyección ligera y Take en SQL ────────────────
        public async Task<List<CajaHistorialDto>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default)
        {
            var rows = await _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal 
                         && c.FechaApertura >= desde 
                         && c.FechaApertura <= hasta 
                         && c.Activo)
                .OrderByDescending(c => c.FechaApertura)
                .Take(take)
                .Select(c => new CajaHistorialDto
                {
                    Id = c.Id,
                    FechaApertura = c.FechaApertura,
                    FechaCierre = c.FechaCierre,
                    Estado = c.Estado,
                    SaldoInicial = c.MontoInicial,
                    SaldoFinal = c.MontoFinal,
                    SucursalNombre = c.Sucursal!.Nombre,
                    UsuarioApertura = c.UsuarioApertura!.Nombre
                })
                .ToListAsync(ct);

            return rows;
        }

        public async Task<List<Caja>> ObtenerCajasPorTurnoAsync(int idSucursal, string turno, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_sucursal == idSucursal && c.Turno == turno)
                .OrderBy(c => c.FechaApertura)
                .ToListAsync(ct);

        public async Task<bool> ExisteCajaAbiertaEnTurnoAsync(int idSucursal, string turno, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.Id_sucursal == idSucursal && c.Turno == turno && c.Estado == 1, ct);

        public async Task<Caja?> ObtenerCajaAbiertaPorSucursYTurnoAsync(int idSucursal, string turno, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(c => c.UsuarioApertura)
                .FirstOrDefaultAsync(c => c.Id_sucursal == idSucursal && c.Turno == turno && c.Estado == 1, ct);
    }
}
