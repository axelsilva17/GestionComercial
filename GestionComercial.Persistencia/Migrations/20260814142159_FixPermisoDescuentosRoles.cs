using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Corrección: asigna el permiso Descuentos.Ver (Id=16) a Gerente (rol 1) y
    /// Administrador (rol 2). La migración AddDescuentoConfiguracion usaba UpdateData
    /// sobre RolPermiso Id=16/32 asumiendo IDs del seed inicial, que no existen en
    /// bases creadas antes de esa semilla. Esta migración inserta solo las filas
    /// faltantes, de forma idempotente (NOT EXISTS).
    /// </summary>
    public partial class FixPermisoDescuentosRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Gerente (rol 1) -> Descuentos.Ver (permiso 16), solo si falta
            migrationBuilder.Sql(
                "INSERT INTO RolPermiso (Id_rol, Id_permiso, FechaAlta, Activo) " +
                "SELECT 1, 16, datetime('now'), 1 " +
                "WHERE NOT EXISTS (SELECT 1 FROM RolPermiso WHERE Id_rol = 1 AND Id_permiso = 16);");

            // Administrador (rol 2) -> Descuentos.Ver (permiso 16), solo si falta
            migrationBuilder.Sql(
                "INSERT INTO RolPermiso (Id_rol, Id_permiso, FechaAlta, Activo) " +
                "SELECT 2, 16, datetime('now'), 1 " +
                "WHERE NOT EXISTS (SELECT 1 FROM RolPermiso WHERE Id_rol = 2 AND Id_permiso = 16);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE FROM RolPermiso WHERE Id_rol IN (1, 2) AND Id_permiso = 16;");
        }
    }
}
