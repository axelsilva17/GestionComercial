using System.Linq.Expressions;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioBase<T> where T : class
    {
        // Lectura
        Task<T?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> criterio);
        Task<T?> PrimerODefaultAsync(Expression<Func<T, bool>> criterio);
        Task<bool> ExisteAsync(Expression<Func<T, bool>> criterio);
        Task<int> ContarAsync(Expression<Func<T, bool>>? criterio = null);

        // Escritura
        Task<T> AgregarAsync(T entidad);
        Task AgregarRangoAsync(IEnumerable<T> entidades);
        void Actualizar(T entidad);
        void Eliminar(T entidad);
        void EliminarRango(IEnumerable<T> entidades);

        // Consultas avanzadas
        IQueryable<T> Consultar();
        IQueryable<T> ConsultarSinTracking();
    }
}