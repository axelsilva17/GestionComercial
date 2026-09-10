using FluentAssertions;
using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — migración 20260909000000_LimpiarRolPermisoDuplicados: ejecuta las
    /// sentencias Up reales contra el schema (misma huella que el MigrateAsync de
    /// arranque) y verifica los tres invariantes: deduplicación de pares (Id_rol,
    /// Id_permiso) conservando la fila de menor Id, inserción del par (1,17) Gerente →
    /// Caja.Auditoria (que ninguna migración siembra), y el índice UNIQUE bloqueando
    /// futuros duplicados.
    /// Clase separada a propósito: crea el índice UNIQUE en la BD del fixture, así que
    /// no puede convivir con RolRegresionTests (que necesita insertar duplicados).
    /// </summary>
    public class RolMigracionRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;
        private static int _contador;

        public RolMigracionRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task MigracionLimpiarRolPermisoDuplicados_DeduplicaInsertaGerenteAuditoriaYBloqueaDuplicados()
        {
            // Arrange: rol propio con un par (Id_rol, Id_permiso) duplicado
            int rolId;

            await using (var setup = _db.CreateContext())
            {
                var n = Interlocked.Increment(ref _contador);
                var rol = new Rol { Nombre = $"Rol Migración Regresión {n}" };
                setup.Roles.Add(rol);
                await setup.SaveChangesAsync();
                rolId = rol.Id;

                setup.RolPermisos.AddRange(
                    new RolPermiso { Id_rol = rolId, Id_permiso = 5 },
                    new RolPermiso { Id_rol = rolId, Id_permiso = 5 });
                await setup.SaveChangesAsync();

                // Pre-condición: EnsureCreated() siembra Gerente con permisos 1..16,
                // así que (1,17) NO existe todavía (mismo estado que una BD migrada antes
                // de esta limpieza).
                setup.RolPermisos.Any(rp => rp.Id_rol == 1 && rp.Id_permiso == 17).Should().BeFalse();
            }

            // Act: replicar las sentencias Up de 20260909000000_LimpiarRolPermisoDuplicados
            using (var ctx = _db.CreateContext())
            {
                ctx.Database.ExecuteSqlRaw("""
                    DELETE FROM "RolPermiso"
                    WHERE "Id" NOT IN (
                        SELECT MIN("Id")
                        FROM "RolPermiso"
                        GROUP BY "Id_rol", "Id_permiso"
                    );
                    """);

                ctx.Database.ExecuteSqlRaw("""
                    INSERT INTO "RolPermiso" ("Id_rol", "Id_permiso", "FechaAlta", "Activo")
                    SELECT 1, 17, datetime('now'), 1
                    WHERE NOT EXISTS (SELECT 1 FROM "RolPermiso" WHERE "Id_rol" = 1 AND "Id_permiso" = 17);
                    """);

                ctx.Database.ExecuteSqlRaw("""
                    CREATE UNIQUE INDEX IF NOT EXISTS "IX_RolPermiso_Id_rol_Id_permiso"
                    ON "RolPermiso" ("Id_rol", "Id_permiso");
                    """);
            }

            // Assert: duplicado eliminado (quedó 1 fila), (1,17) insertado y el índice
            // UNIQUE rechaza cualquier inserción duplicada posterior.
            using (var verif = _db.CreateContext())
            {
                verif.RolPermisos.Count(rp => rp.Id_rol == rolId).Should().Be(1);
                verif.RolPermisos.Should().Contain(rp => rp.Id_rol == 1 && rp.Id_permiso == 17);

                var saveDuplicado = async () =>
                {
                    verif.RolPermisos.Add(new RolPermiso { Id_rol = 1, Id_permiso = 17 });
                    await verif.SaveChangesAsync();
                };
                // EF envuelve la SqliteException (ERROR 19 UNIQUE constraint) en DbUpdateException
                await saveDuplicado.Should().ThrowAsync<DbUpdateException>();
            }
        }
    }
}