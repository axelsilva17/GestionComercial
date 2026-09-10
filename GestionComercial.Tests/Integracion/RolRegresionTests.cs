using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — Roles: CantidadPermisos must count DISTINCT permiso ids per role.
    /// Original bug: the count used r.RolPermisos.Count (rows), so duplicated pairs
    /// (produced by the full reconcile list re-inserting pairs) inflated the UI number.
    /// Fix: RolServicio counts .Select(rp => rp.Id_permiso).Distinct() and
    /// ObtenerPermisosPorRolAsync returns distinct ids. Production is also protected by
    /// the UNIQUE index created in migration 20260909000000 (raw SQL, NOT in the model:
    /// EnsureCreated() here has no such index, so the duplicate rows can be inserted to
    /// prove the service counts distinct).
    /// </summary>
    public class RolRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;
        private static int _contador;

        public RolRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerRolesAsync_RolConPermisosDuplicados_CuentaDistintos()
        {
            // Arrange: rol propio + 3 filas con solo 2 permisos distintos (1 duplicado)
            int n = Interlocked.Increment(ref _contador);
            int rolId;

            await using (var setup = _db.CreateContext())
            {
                var rol = new Rol { Nombre = $"Rol Permisos Regresión {n}" };
                setup.Roles.Add(rol);
                await setup.SaveChangesAsync();
                rolId = rol.Id;

                setup.RolPermisos.AddRange(
                    new RolPermiso { Id_rol = rolId, Id_permiso = 1 },
                    new RolPermiso { Id_rol = rolId, Id_permiso = 1 },
                    new RolPermiso { Id_rol = rolId, Id_permiso = 2 });
                await setup.SaveChangesAsync();
            }

            // Act: el servicio listado usado por RolesViewModel
            using var uow = _db.CreateUnitOfWork();
            var servicio = new RolServicio(uow);
            var roles = await servicio.ObtenerRolesAsync();

            // Assert: 3 filas pero 2 permisos distintos → el conteo debe ser 2
            var dto = roles.Should().ContainSingle(r => r.Id == rolId).Subject;
            dto.CantidadPermisos.Should().Be(2);
        }
    }
}