using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class DescuentoConfiguracionRepositorio : RepositorioBase<DescuentoConfiguracion>, IDescuentoConfiguracionRepositorio
    {
        public DescuentoConfiguracionRepositorio(GestionComercialContext context) : base(context)
        {
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerVigentesPorEmpresaAsync(int idEmpresa)
        {
            return await _dbSet.AsNoTracking()
                .Include(d => d.DescuentosMetodosPago)
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente)
                .ToListAsync();
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerConMetodosPagoAsync(int idEmpresa)
        {
            return await _dbSet.AsNoTracking()
                .Include(d => d.DescuentosMetodosPago).ThenInclude(dm => dm.MetodoPago)
                .Where(d => d.Id_empresa == idEmpresa && d.Activo)
                .ToListAsync();
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerConMetodosPagoPorIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .Include(d => d.DescuentosMetodosPago).ThenInclude(dm => dm.MetodoPago)
                .Where(d => d.Id == id)
                .ToListAsync();
        }

        public async Task<List<DescuentoConfiguracion>> BuscarAsync(int idEmpresa, string? texto, bool? activo)
        {
            var query = _dbSet.AsNoTracking()
                .Include(d => d.DescuentosMetodosPago).ThenInclude(dm => dm.MetodoPago)
                .Include(d => d.Producto)
                .Include(d => d.Categoria)
                .Where(d => d.Id_empresa == idEmpresa);

            if (!string.IsNullOrWhiteSpace(texto))
                query = query.Where(d => d.Nombre.Contains(texto));

            if (activo.HasValue)
                query = query.Where(d => d.Activo == activo.Value);

            return await query.OrderBy(d => d.Nombre).ToListAsync();
        }

        public async Task ActualizarMetodosPagoAsync(int idDescuento, List<int> idsMetodosPago)
        {
            var existing = await _context.DescuentoMetodosPago
                .Where(dm => dm.Id_descuentoConfiguracion == idDescuento)
                .ToListAsync();

            _context.DescuentoMetodosPago.RemoveRange(existing);

            var newRows = idsMetodosPago.Select(idMp => new DescuentoMetodoPago
            {
                Id_descuentoConfiguracion = idDescuento,
                Id_metodoPago = idMp
            }).ToList();

            await _context.DescuentoMetodosPago.AddRangeAsync(newRows);
        }
    }
}