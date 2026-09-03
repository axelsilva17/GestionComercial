using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

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

        public async Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, ct);

        public async Task<IEnumerable<T>> ObtenerTodosAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> criterio, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(criterio).ToListAsync(ct);

        public async Task<T?> PrimerODefaultAsync(Expression<Func<T, bool>> criterio, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().FirstOrDefaultAsync(criterio, ct);

        public async Task<bool> ExisteAsync(Expression<Func<T, bool>> criterio, CancellationToken ct = default)
            => await _dbSet.AnyAsync(criterio, ct);

        public async Task<int> ContarAsync(Expression<Func<T, bool>>? criterio = null, CancellationToken ct = default)
            => criterio is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(criterio, ct);

        public async Task<T> AgregarAsync(T entidad, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entidad, ct);
            return entidad;
        }

        public async Task AgregarRangoAsync(IEnumerable<T> entidades, CancellationToken ct = default)
            => await _dbSet.AddRangeAsync(entidades, ct);

        public void Actualizar(T entidad)
            => _dbSet.Update(entidad);

        public void Eliminar(T entidad)
            => _dbSet.Remove(entidad);

        public void EliminarRango(IEnumerable<T> entidades)
            => _dbSet.RemoveRange(entidades);

        public IQueryable<T> Consultar()
            => _dbSet.AsQueryable();

        public IQueryable<T> ConsultarSinTracking()
            => _dbSet.AsNoTracking();

        public void Desadjuntar(T entidad)
            => _context.Entry(entidad).State = EntityState.Detached;
    }
}