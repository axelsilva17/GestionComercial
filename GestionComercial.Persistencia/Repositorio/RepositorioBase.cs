using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionComercial.Persistencia.Repositorio
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        protected readonly GestionComercialContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositorioBase(GestionComercialContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // ── Lectura ───────────────────────────────────────────────────────────

        public async Task<T?> ObtenerPorIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> ObtenerTodosAsync()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> criterio)
            => await _dbSet.Where(criterio).ToListAsync();

        public async Task<T?> PrimerODefaultAsync(Expression<Func<T, bool>> criterio)
            => await _dbSet.FirstOrDefaultAsync(criterio);

        public async Task<bool> ExisteAsync(Expression<Func<T, bool>> criterio)
            => await _dbSet.AnyAsync(criterio);

        public async Task<int> ContarAsync(Expression<Func<T, bool>>? criterio = null)
            => criterio is null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(criterio);

        // ── Escritura ─────────────────────────────────────────────────────────

        public async Task<T> AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            return entidad;
        }

        public async Task AgregarRangoAsync(IEnumerable<T> entidades)
            => await _dbSet.AddRangeAsync(entidades);

        public void Actualizar(T entidad)
            => _dbSet.Update(entidad);

        public void Eliminar(T entidad)
            => _dbSet.Remove(entidad);

        public void EliminarRango(IEnumerable<T> entidades)
            => _dbSet.RemoveRange(entidades);

        // ── Consultas con includes ────────────────────────────────────────────

        public IQueryable<T> Consultar()
            => _dbSet.AsQueryable();

        public IQueryable<T> ConsultarSinTracking()
            => _dbSet.AsNoTracking();
    }
}