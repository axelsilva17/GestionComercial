using GestionComercial.Persistencia.Contexto;
using GestionComercial.Persistencia.Repositorio;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Base de datos SQLite en memoria con el schema completo del modelo (incluyendo las
    /// seeds HasData). Una única conexión compartida por todos los DbContext de la clase de
    /// test: los datos persisten mientras la conexión siga abierta.
    /// Cada clase de test con IClassFixture&lt;SqliteTestDatabase&gt; recibe su propia
    /// instancia (aislamiento total entre clases; xUnit corre las clases en paralelo).
    /// </summary>
    public sealed class SqliteTestDatabase : IDisposable
    {
        private readonly SqliteConnection _connection;

        public SqliteTestDatabase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            using var ctx = CreateContext();
            ctx.Database.EnsureCreated();
        }

        public GestionComercialContext CreateContext()
            => new(new DbContextOptionsBuilder<GestionComercialContext>()
                .UseSqlite(_connection)
                .Options);

        public UnitOfWork CreateUnitOfWork()
            => new(CreateContext());

        public void Dispose() => _connection.Dispose();
    }
}