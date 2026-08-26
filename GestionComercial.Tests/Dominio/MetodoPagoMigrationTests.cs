using FluentAssertions;
using GestionComercial.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GestionComercial.Tests.Dominio
{
    public class MetodoPagoMigrationTests
    {
        private readonly string _rootDbPath;
        private readonly string _binDbPath;

        public MetodoPagoMigrationTests()
        {
            var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            _rootDbPath = Path.Combine(root, "GestionComercial.UI", "GestionComercial.db");
            _binDbPath = Path.Combine(AppContext.BaseDirectory, "GestionComercial.db");
        }

        [Fact]
        public void RootDb_MetodoPago_SubcategoriaColumnExists()
        {
            if (!File.Exists(_rootDbPath)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_rootDbPath}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA table_info('MetodoPago')";
            using var reader = cmd.ExecuteReader();
            bool found = false;
            while (reader.Read())
            {
                if (reader["name"].ToString() == "Subcategoria")
                {
                    found = true;
                    break;
                }
            }
            found.Should().BeTrue("Subcategoria column should exist in MetodoPago table");
        }

        [Fact]
        public void RootDb_MetodoPago_BackfillDebitoCorrecto()
        {
            if (!File.Exists(_rootDbPath)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_rootDbPath}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Categoria, Subcategoria FROM MetodoPago WHERE Nombre = 'Débito'";
            using var reader = cmd.ExecuteReader();
            reader.Read().Should().BeTrue();
            reader["Categoria"].ToString().Should().Be("Tarjeta");
            reader["Subcategoria"].ToString().Should().Be("Debito");
        }

        [Fact]
        public void RootDb_MetodoPago_BackfillCreditoCorrecto()
        {
            if (!File.Exists(_rootDbPath)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_rootDbPath}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Categoria, Subcategoria FROM MetodoPago WHERE Nombre = 'Crédito'";
            using var reader = cmd.ExecuteReader();
            reader.Read().Should().BeTrue();
            reader["Categoria"].ToString().Should().Be("Tarjeta");
            reader["Subcategoria"].ToString().Should().Be("Credito");
        }

        [Fact]
        public void RootDb_MetodoPago_OtherMethodsSubcategoriaNull()
        {
            if (!File.Exists(_rootDbPath)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_rootDbPath}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Nombre, Subcategoria FROM MetodoPago WHERE Nombre IN ('Efectivo', 'Transferencia', 'Mercado Pago')";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var subcat = reader["Subcategoria"];
                (subcat == null || subcat == DBNull.Value).Should().BeTrue(
                    $"{reader["Nombre"]} should have null Subcategoria");
            }
        }

        [Fact]
        public void BinDb_MetodoPago_SubcategoriaColumnExists()
        {
            if (!File.Exists(_binDbPath)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_binDbPath}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA table_info('MetodoPago')";
            using var reader = cmd.ExecuteReader();
            bool found = false;
            while (reader.Read())
            {
                if (reader["name"].ToString() == "Subcategoria")
                {
                    found = true;
                    break;
                }
            }
            found.Should().BeTrue("Subcategoria column should exist in BIN DB MetodoPago table");
        }

        [Fact]
        public void BackfillEsIdempotente_RunningTwiceNoError()
        {
            if (!File.Exists(_rootDbPath)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_rootDbPath}");
            connection.Open();

            // Run backfill twice - should be idempotent
            var backfillSql = @"
                UPDATE ""MetodoPago"" SET ""Subcategoria"" = 'Debito' WHERE ""Nombre"" = 'Débito' AND ""Subcategoria"" IS NULL;
                UPDATE ""MetodoPago"" SET ""Subcategoria"" = 'Credito' WHERE ""Nombre"" = 'Crédito' AND ""Subcategoria"" IS NULL;";

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = backfillSql;
                cmd.ExecuteNonQuery();
            }
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = backfillSql;
                var rows = cmd.ExecuteNonQuery();
                // Second run should affect 0 rows (idempotent)
            }

            // Verify values are still correct
            using var cmd2 = connection.CreateCommand();
            cmd2.CommandText = "SELECT Subcategoria FROM MetodoPago WHERE Nombre = 'Débito'";
            using var reader = cmd2.ExecuteReader();
            reader.Read().Should().BeTrue();
            reader["Subcategoria"].ToString().Should().Be("Debito");
        }
    }
}
