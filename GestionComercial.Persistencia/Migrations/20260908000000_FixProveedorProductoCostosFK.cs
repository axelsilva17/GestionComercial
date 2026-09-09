using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(GestionComercialContext))]
    [Migration("20260908000000_FixProveedorProductoCostosFK")]
    public partial class FixProveedorProductoCostosFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // La migración original (20260427020000_ProveedorProductoCosto) declaró FKs
            // a tablas inexistentes (plurales "Proveedores"/"Productos"), que SQLite aceptó
            // al crearlas pero nunca enforzó. Las tablas reales son "Proveedor"/"Producto"
            // (singular). SQLite no permite ALTER TABLE ... DROP CONSTRAINT, así que se
            // reconstruye la tabla con el patrón create_nuevo → copia → drop → rename,
            // corrigiendo los FKs a las tablas singulares reales.
            //
            // Nota: no se usa PRAGMA foreign_keys=OFF (EF Core envuelve la migración en una
            // transacción y SQLite no permite PRAGMA dentro de una transacción; ver convención
            // en OptimizacionIndices.cs). El rebuild es seguro dentro de la transacción única.
            migrationBuilder.Sql(@"
                CREATE TABLE ""tmp_ProveedorProductoCostos_new"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_ProveedorProductoCostos"" PRIMARY KEY AUTOINCREMENT,
                    ""IdProveedor"" INTEGER NOT NULL,
                    ""IdProducto"" INTEGER NOT NULL,
                    ""Costo"" TEXT NOT NULL,
                    ""FechaAlta"" TEXT NOT NULL,
                    ""Activo"" INTEGER NOT NULL,
                    CONSTRAINT ""FK_ProveedorProductoCostos_Proveedor_IdProveedor"" FOREIGN KEY (""IdProveedor"") REFERENCES ""Proveedor"" (""Id"") ON DELETE CASCADE,
                    CONSTRAINT ""FK_ProveedorProductoCostos_Producto_IdProducto"" FOREIGN KEY (""IdProducto"") REFERENCES ""Producto"" (""Id"") ON DELETE CASCADE
                );

                INSERT INTO ""tmp_ProveedorProductoCostos_new"" (""Id"", ""IdProveedor"", ""IdProducto"", ""Costo"", ""FechaAlta"", ""Activo"")
                SELECT ""Id"", ""IdProveedor"", ""IdProducto"", ""Costo"", ""FechaAlta"", ""Activo""
                FROM ""ProveedorProductoCostos"";

                DROP TABLE ""ProveedorProductoCostos"";
                ALTER TABLE ""tmp_ProveedorProductoCostos_new"" RENAME TO ""ProveedorProductoCostos"";
            ");

            // Recrear los índices que existían en la tabla (los tres definidos en la
            // migración original; ninguno apunta a columnas inexistentes).
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS ""IX_ProveedorProductoCostos_IdProveedor"" ON ""ProveedorProductoCostos"" (""IdProveedor"");
            ");
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS ""IX_ProveedorProductoCostos_IdProducto"" ON ""ProveedorProductoCostos"" (""IdProducto"");
            ");
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IF NOT EXISTS ""IX_ProveedorProductoCostos_ProveedorProducto"" ON ""ProveedorProductoCostos"" (""IdProveedor"", ""IdProducto"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restaurar el estado roto original: FKs a las tablas plurales inexistentes.
            migrationBuilder.Sql(@"
                CREATE TABLE ""tmp_ProveedorProductoCostos_new"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_ProveedorProductoCostos"" PRIMARY KEY AUTOINCREMENT,
                    ""IdProveedor"" INTEGER NOT NULL,
                    ""IdProducto"" INTEGER NOT NULL,
                    ""Costo"" TEXT NOT NULL,
                    ""FechaAlta"" TEXT NOT NULL,
                    ""Activo"" INTEGER NOT NULL,
                    CONSTRAINT ""FK_ProveedorProductoCostos_Proveedores_IdProveedor"" FOREIGN KEY (""IdProveedor"") REFERENCES ""Proveedores"" (""Id"") ON DELETE CASCADE,
                    CONSTRAINT ""FK_ProveedorProductoCostos_Productos_IdProducto"" FOREIGN KEY (""IdProducto"") REFERENCES ""Productos"" (""Id"") ON DELETE CASCADE
                );

                INSERT INTO ""tmp_ProveedorProductoCostos_new"" (""Id"", ""IdProveedor"", ""IdProducto"", ""Costo"", ""FechaAlta"", ""Activo"")
                SELECT ""Id"", ""IdProveedor"", ""IdProducto"", ""Costo"", ""FechaAlta"", ""Activo""
                FROM ""ProveedorProductoCostos"";

                DROP TABLE ""ProveedorProductoCostos"";
                ALTER TABLE ""tmp_ProveedorProductoCostos_new"" RENAME TO ""ProveedorProductoCostos"";
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS ""IX_ProveedorProductoCostos_IdProveedor"" ON ""ProveedorProductoCostos"" (""IdProveedor"");
            ");
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS ""IX_ProveedorProductoCostos_IdProducto"" ON ""ProveedorProductoCostos"" (""IdProducto"");
            ");
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IF NOT EXISTS ""IX_ProveedorProductoCostos_ProveedorProducto"" ON ""ProveedorProductoCostos"" (""IdProveedor"", ""IdProducto"");
            ");
        }
    }
}
