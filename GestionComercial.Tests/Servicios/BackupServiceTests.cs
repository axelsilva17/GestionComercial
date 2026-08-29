using FluentAssertions;
using GestionComercial.Dominio.Entidades.Configuracion;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Infraestructura.Servicios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace GestionComercial.Tests.Servicios
{
    public class BackupServiceTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _dbPath;
        private readonly string _connectionString;
        private readonly GestionComercialContext _context;

        public BackupServiceTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), $"backup_test_{Guid.NewGuid():N}");
            Directory.CreateDirectory(_tempDir);

            _dbPath = Path.Combine(_tempDir, "test.db");
            _connectionString = $"Data Source={_dbPath}";

            // Create database with just the BackupConfig table
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS BackupConfig (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FechaAlta TEXT NOT NULL DEFAULT '0001-01-01 00:00:00',
                    Activo INTEGER NOT NULL DEFAULT 1,
                    Frecuencia INTEGER NOT NULL DEFAULT 0,
                    DiaSemana INTEGER NULL,
                    HoraProgramada TEXT NULL,
                    MaxBackups INTEGER NOT NULL DEFAULT 10,
                    CarpetaDestino TEXT NOT NULL DEFAULT '',
                    UltimoBackup TEXT NULL
                );";
            cmd.ExecuteNonQuery();
            conn.Close();

            var options = new DbContextOptionsBuilder<GestionComercialContext>()
                .UseSqlite(_connectionString)
                .Options;
            _context = new GestionComercialContext(options);
        }

        public void Dispose()
        {
            _context?.Dispose();
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        [Fact]
        public void RotarBackups_EliminaArchivosExcedentes()
        {
            // Arrange
            var backupDir = Path.Combine(_tempDir, "Backups");
            Directory.CreateDirectory(backupDir);

            var config = new BackupConfig
            {
                Frecuencia = FrecuenciaBackupEnum.Desactivado,
                MaxBackups = 5,
                CarpetaDestino = backupDir
            };
            _context.BackupConfigs.Add(config);
            _context.SaveChanges();

            // Create 12 .db files with different creation times
            for (int i = 0; i < 12; i++)
            {
                var filePath = Path.Combine(backupDir, $"GestionComercial_Backup_{i:D2}-01-2026_12-00.db");
                File.WriteAllText(filePath, $"backup content {i}");
                File.SetCreationTime(filePath, DateTime.Now.AddMinutes(-i));
            }

            var service = new BackupService(_connectionString, _context);

            // Act
            service.RotarBackups();

            // Assert
            var remaining = new DirectoryInfo(backupDir)
                .GetFiles("GestionComercial_Backup_*.db");
            remaining.Should().HaveCount(5);
        }

        [Fact]
        public void RotarBackups_NoEliminaCuandoDentroDelLimite()
        {
            // Arrange
            var backupDir = Path.Combine(_tempDir, "Backups2");
            Directory.CreateDirectory(backupDir);

            var config = new BackupConfig
            {
                Frecuencia = FrecuenciaBackupEnum.Desactivado,
                MaxBackups = 5,
                CarpetaDestino = backupDir
            };
            _context.BackupConfigs.Add(config);
            _context.SaveChanges();

            for (int i = 0; i < 3; i++)
            {
                var filePath = Path.Combine(backupDir, $"GestionComercial_Backup_{i:D2}-01-2026_12-00.db");
                File.WriteAllText(filePath, $"backup content {i}");
            }

            var service = new BackupService(_connectionString, _context);

            // Act
            service.RotarBackups();

            // Assert
            var remaining = new DirectoryInfo(backupDir)
                .GetFiles("GestionComercial_Backup_*.db");
            remaining.Should().HaveCount(3);
        }

        [Fact]
        public void RotarBackups_CarpetaNoExiste_NoLanzaExcepcion()
        {
            var config = new BackupConfig
            {
                Frecuencia = FrecuenciaBackupEnum.Desactivado,
                MaxBackups = 5,
                CarpetaDestino = Path.Combine(_tempDir, "nonexistent")
            };
            _context.BackupConfigs.Add(config);
            _context.SaveChanges();

            var service = new BackupService(_connectionString, _context);

            // Act & Assert - should not throw
            var act = () => service.RotarBackups();
            act.Should().NotThrow();
        }

        [Fact]
        public void HayEspacioDisponible_DevuelveTrueCuandoHayEspacio()
        {
            var config = new BackupConfig
            {
                Frecuencia = FrecuenciaBackupEnum.Desactivado,
                MaxBackups = 10,
                CarpetaDestino = _tempDir
            };
            _context.BackupConfigs.Add(config);
            _context.SaveChanges();

            var service = new BackupService(_connectionString, _context);

            // Act - request 1 byte (any drive should have this)
            var result = service.HayEspacioDisponible(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void HayEspacioDisponible_DevuelveFalseCuandoNoHayEspacio()
        {
            var config = new BackupConfig
            {
                Frecuencia = FrecuenciaBackupEnum.Desactivado,
                MaxBackups = 10,
                CarpetaDestino = _tempDir
            };
            _context.BackupConfigs.Add(config);
            _context.SaveChanges();

            var service = new BackupService(_connectionString, _context);

            // Act - request impossibly large amount
            var result = service.HayEspacioDisponible(long.MaxValue);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HayEspacioDisponible_CarpetaInvalida_DevuelveFalse()
        {
            var config = new BackupConfig
            {
                Frecuencia = FrecuenciaBackupEnum.Desactivado,
                MaxBackups = 10,
                CarpetaDestino = @"\\invalid\unc\path"
            };
            _context.BackupConfigs.Add(config);
            _context.SaveChanges();

            var service = new BackupService(_connectionString, _context);

            // Act
            var result = service.HayEspacioDisponible(1);

            // Assert
            result.Should().BeFalse();
        }
    }
}
