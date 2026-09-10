using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(GestionComercialContext))]
    [Migration("20260909000000_LimpiarRolPermisoDuplicados")]
    public partial class LimpiarRolPermisoDuplicados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── 1. Deduplicar pares (Id_rol, Id_permiso) manteniendo la fila de menor Id ──
            // Los duplicados venían del reconcile de SemillaPermisos (que con la lista
            // completa de pares re-insertaba filas ya presentes) y de asignaciones
            // previas sin Distinct. Se conserva la fila de menor Id: las filas con Id
            // explícito de las migraciones (1..32, 37-43) prevalecen sobre las réplicas
            // con Id autogenerado.
            migrationBuilder.Sql(@"
                DELETE FROM ""RolPermiso""
                WHERE ""Id"" NOT IN (
                    SELECT MIN(""Id"")
                    FROM ""RolPermiso""
                    GROUP BY ""Id_rol"", ""Id_permiso""
                );
            ");

            // ── 2. Garantizar el par (1,17): Gerente → Caja.Auditoria ──
            // El permiso 17 (Caja.Auditoria) se insertó en 20260903231401, pero ninguna
            // migración inserta su RolPermiso. SemillaPermisos.ReconciliarPermisosSemillaAsync
            // (ya reducida a solo este par) también lo garantiza en runtime; acá se asegura
            // el mismo invariante a nivel de datos para que el índice UNIQUE no falle.
            migrationBuilder.Sql(@"
                INSERT INTO ""RolPermiso"" (""Id_rol"", ""Id_permiso"", ""FechaAlta"", ""Activo"")
                SELECT 1, 17, datetime('now'), 1
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""RolPermiso"" WHERE ""Id_rol"" = 1 AND ""Id_permiso"" = 17
                );
            ");

            // ── 3. Índice UNIQUE (Id_rol, Id_permiso) para impedir futuros duplicados ──
            // Se mantiene a nivel SQL (no en el modelo/snapshot) a propósito: los tests de
            // integración crear el schema con EnsureCreated() y necesitan poder insertar
            // pares duplicados para probar el conteo DISTINCT del servicio.
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IF NOT EXISTS ""IX_RolPermiso_Id_rol_Id_permiso""
                ON ""RolPermiso"" (""Id_rol"", ""Id_permiso"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No se revierten los datos: la deduplicación y el par (1,17) son invariantes
            // deseados. Solo se quita el índice UNIQUE para volver al estado pre-migración.
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_RolPermiso_Id_rol_Id_permiso\";");
        }
    }
}