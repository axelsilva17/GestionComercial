using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace GestionComercial.Persistencia.Repositorio
{
    public class DiagnosticoContext : IDiagnosticoContext
    {
        private readonly GestionComercialContext _context;
        private readonly string _connectionString;

        public DiagnosticoContext(GestionComercialContext context)
        {
            _context = context;
            _connectionString = context.Database.GetConnectionString() ?? string.Empty;
        }

        public async Task<string> EjecutarRawSqlAsync(string sql)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString() ?? string.Empty;
        }

        public async Task<DbDataReader> EjecutarReaderAsync(string sql)
        {
            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            return reader;
        }

        public async Task<long> ObtenerTamanoDbAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT page_count * page_size FROM pragma_page_count(), pragma_page_size();";
            var result = await cmd.ExecuteScalarAsync();
            return result is long bytes ? bytes : 0;
        }
    }
}
