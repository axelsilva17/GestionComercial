using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ClienteRepositorio : RepositorioBase<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<Cliente>> ObtenerPorEmpresaAsync(int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_empresa == idEmpresa)
                .OrderBy(c => c.Nombre)
                .ToListAsync(ct);

        public async Task<(IEnumerable<Cliente> Items, int TotalCount)> ObtenerPorEmpresaPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, bool? soloActivos = null, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking().Where(c => c.Id_empresa == idEmpresa);

            if (soloActivos.HasValue)
                query = query.Where(c => c.Activo == soloActivos.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                // StartsWith para uso de índices
                query = query.Where(c =>
                    EF.Functions.Like(c.Nombre, term + "%") ||
                    EF.Functions.Like(c.Email, term + "%") ||
                    c.Documento.ToString().StartsWith(term));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<Cliente?> ObtenerPorDocumentoAsync(int documento, int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Documento == documento && c.Id_empresa == idEmpresa, ct);

        public async Task<bool> ExisteDocumentoAsync(int documento, int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.Documento == documento && c.Id_empresa == idEmpresa, ct);

        public async Task<bool> ExisteEmailAsync(string email, int idEmpresa, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.Email == email && c.Id_empresa == idEmpresa, ct);

        // ── Búsqueda con StartsWith (prefijo) ──────────────────────────────────
        public async Task<List<Cliente>> BuscarPorNombreAsync(string nombre, int idEmpresa, int take = 10, CancellationToken ct = default)
        {
            var term = nombre.Trim();
            return await _dbSet.AsNoTracking()
                .Where(c => c.Id_empresa == idEmpresa
                         && EF.Functions.Like(c.Nombre, term + "%"))
                .OrderBy(c => c.Nombre)
                .Take(take)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Cliente>> ObtenerPorEmpresaYFechaAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(c => c.Id_empresa == idEmpresa && c.FechaAlta >= desde && c.FechaAlta <= hasta)
                .OrderByDescending(c => c.FechaAlta)
                .ToListAsync(ct);

        public async Task<int> ContarClientesConVentasAsync(int idEmpresa, CancellationToken ct = default)
            => await _context.Ventas
                .Where(v => v.Cliente.Id_empresa == idEmpresa)
                .Select(v => v.Id_cliente)
                .Distinct()
                .CountAsync(ct);

        // Conteo en SQL: mismas filas que ObtenerPorEmpresaYFechaAsync sin materializarlas.
        public async Task<int> ContarClientesNuevosAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .CountAsync(c => c.Id_empresa == idEmpresa && c.FechaAlta >= desde && c.FechaAlta <= hasta, ct);
    }
}
