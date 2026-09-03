using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data;
using System.Data.Common;

namespace GestionComercial.Persistencia.Contexto
{
    /// <summary>
    /// Interceptor that sets SQLite PRAGMAs on every new connection.
    /// EF Core's connection pool may return connections without WAL/cache settings,
    /// so this ensures every connection is properly configured.
    /// </summary>
    public class SqlitePragmaInterceptor : DbConnectionInterceptor
    {
        private readonly int _cacheSize;
        private readonly string _journalMode;
        private readonly string _synchronous;
        private readonly string _tempStore;
        private readonly long _mmapSize;

        public SqlitePragmaInterceptor(
            int cacheSize = -16000,
            string journalMode = "WAL",
            string synchronous = "NORMAL",
            string tempStore = "MEMORY",
            long mmapSize = 268435456)
        {
            _cacheSize = cacheSize;
            _journalMode = journalMode;
            _synchronous = synchronous;
            _tempStore = tempStore;
            _mmapSize = mmapSize;
        }

        public override InterceptionResult ConnectionOpening(
            DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
                ApplyPragmas(connection);
            }
            return result;
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection, ConnectionEventData eventData, InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
                ApplyPragmas(connection);
            }
            return result;
        }

        private void ApplyPragmas(DbConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $@"
                PRAGMA journal_mode={_journalMode};
                PRAGMA synchronous={_synchronous};
                PRAGMA cache_size={_cacheSize};
                PRAGMA temp_store={_tempStore};
                PRAGMA mmap_size={_mmapSize};
            ";
            cmd.ExecuteNonQuery();
        }
    }
}
