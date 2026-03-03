using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace GestionComercial.Persistencia.Contexto
{
    /// <summary>
    /// Permite a "dotnet ef migrations add" crear el contexto desde la CLI
    /// sin necesidad de levantar la aplicación completa.
    /// </summary>
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
                ?? "Server=localhost;Database=GestionComercial;Trusted_Connection=True;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<GestionComercialContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new GestionComercialContext(options);
        }
    }
}
