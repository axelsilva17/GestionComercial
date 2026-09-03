using System.Linq.Expressions;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioBase<T> where T : class
    {
        // Lectura
        Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<T>> ObtenerTodosAsync(CancellationToken ct = default);
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> criterio, CancellationToken ct = default);
        Task<T?> PrimerODefaultAsync(Expression<Func<T, bool>> criterio, CancellationToken ct = default);
        Task<bool> ExisteAsync(Expression<Func<T, bool>> criterio, CancellationToken ct = default);
        Task<int> ContarAsync(Expression<Func<T, bool>>? criterio = null, CancellationToken ct = default);

        // Escritura
        Task<T> AgregarAsync(T entidad, CancellationToken ct = default);
        Task AgregarRangoAsync(IEnumerable<T> entidades, CancellationToken ct = default);
        void Actualizar(T entidad);
        void Eliminar(T entidad);
        void EliminarRango(IEnumerable<T> entidades);

        // Consultas avanzadas
        IQueryable<T> Consultar();
        IQueryable<T> ConsultarSinTracking();

        // Tracking
        void Desadjuntar(T entidad);
    }
}