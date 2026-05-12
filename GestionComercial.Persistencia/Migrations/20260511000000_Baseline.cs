using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class Baseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Crear tabla de historial de migraciones (si no existe) ──
            // Esta migración permite usar MigrateAsync() en DBs creadas con EnsureCreated.
            // Idempotente: puede ejecutarse múltiples veces sin efectos colaterales.

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (
                    MigrationId TEXT NOT NULL CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY,
                    ProductVersion TEXT NOT NULL
                );
            ");

            // ── Insertar migraciones existentes como aplicadas ──
            // Las migraciones InitialCreate y ProveedorProductoCosto ya fueron ejecutadas
            // en DBs creadas con EnsureCreated. Las marcamos como aplicadas para que
            // MigrateAsync() no intente recrear las tablas.

            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO __EFMigrationsHistory (MigrationId, ProductVersion)
                VALUES ('20260426204457_InitialCreate', '8.0.0');
            ");

            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO __EFMigrationsHistory (MigrationId, ProductVersion)
                VALUES ('20260427020000_ProveedorProductoCosto', '8.0.0');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No revertimos esta migración — las DBs existentes podrían necesitar
            // mantener estos registros para coherencia de estado.
        }
    }
}