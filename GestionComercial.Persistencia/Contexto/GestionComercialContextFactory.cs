using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace GestionComercial.Persistencia.Contexto
{
    ///     /// Permite a "dotnet ef migrations add" crear el contexto desde la CLI
    /// sin necesidad de levantar la aplicación completa.
    public class GestionComercialContextFactory : IDesignTimeDbContextFactory<GestionComercialContext>
    {
        public GestionComercialContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "GestionComercial.UI");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString =
                config.GetConnectionString("DefaultConnection")
                ?? "Data Source=GestionComercial.db";

            // ── Resolver ruta relativa de SQLite ──────────────────────────────
            var connBuilder = new SqliteConnectionStringBuilder(connectionString);
            if (!Path.IsPathRooted(connBuilder.DataSource))
            {
                connBuilder.DataSource = Path.Combine(basePath, connBuilder.DataSource);
            }
            connectionString = connBuilder.ConnectionString;

            var options = new DbContextOptionsBuilder<GestionComercialContext>()
                .UseSqlite(connectionString)
                .Options;

            return new GestionComercialContext(options);
        }
    }
}
