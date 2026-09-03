using System.Data.Common;
using System.Threading.Tasks;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IDiagnosticoContext
    {
        Task<string> EjecutarRawSqlAsync(string sql);
        Task<DbDataReader> EjecutarReaderAsync(string sql);
        Task<long> ObtenerTamanoDbAsync();
    }
}
