using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriaToMetodoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop vista que depende de MetodoPago antes de dropear la tabla
            migrationBuilder.Sql("DROP VIEW IF EXISTS VistaVentasResumidas;");

            // Limpiar FK references de Pago a MetodoPago (seed data se reinserta al final del migration)
            migrationBuilder.Sql("DELETE FROM Pago;");

            // Rebuild MetodoPago via raw SQL (SQLite no permite ALTER COLUMN)
            // EsEfectivo → Categoria con migración de datos incluida
            migrationBuilder.Sql(@"
                CREATE TABLE ""tmp_MetodoPago_new"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_MetodoPago"" PRIMARY KEY AUTOINCREMENT,
                    ""Nombre"" TEXT NOT NULL,
                    ""Categoria"" TEXT NOT NULL DEFAULT 'Otro',
                    ""Activo"" INTEGER NOT NULL DEFAULT 1,
                    ""Id_empresa"" INTEGER NOT NULL,
                    ""FechaAlta"" TEXT NULL,
                    CONSTRAINT ""FK_MetodoPago_Empresa_Id_empresa"" FOREIGN KEY (""Id_empresa"") REFERENCES ""Empresa"" (""Id"") ON DELETE CASCADE
                );

                INSERT INTO ""tmp_MetodoPago_new"" (""Id"", ""Nombre"", ""Categoria"", ""Activo"", ""Id_empresa"", ""FechaAlta"")
                SELECT ""Id"", ""Nombre"",
                    CASE
                        WHEN ""EsEfectivo"" = 1 THEN 'Efectivo'
                        WHEN ""Nombre"" IN ('Débito', 'Crédito') THEN 'Tarjeta'
                        WHEN ""Nombre"" = 'Transferencia' THEN 'Transferencia'
                        ELSE 'Otro'
                    END,
                    ""Activo"", ""Id_empresa"", ""FechaAlta""
                FROM ""MetodoPago"";

                DROP TABLE ""MetodoPago"";
                ALTER TABLE ""tmp_MetodoPago_new"" RENAME TO ""MetodoPago"";
            ");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Empresa",
                type: "TEXT",
                nullable: true);

            // ProveedorProductoCostos ya fue creada en la migration ProveedorProductoCosto

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5357), "Mañana" });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5397), "Tarde" });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5402), "Mañana" });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5407), "Noche" });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5413), "Mañana" });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5418), "Tarde" });

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4996));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5001));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5003));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5020));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5163));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5166));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5169));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5171));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5174));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5982));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5986));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5998));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6005));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6012));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6015));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6021));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "LogoUrl" },
                values: new object[] { new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4803), null });

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Categoria",
                value: "Efectivo");

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Categoria",
                value: "Tarjeta");

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Categoria",
                value: "Tarjeta");

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Categoria",
                value: "Transferencia");

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Categoria",
                value: "Otro");

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5873));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5876));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5878));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5880));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5882));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5892));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5893));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5895));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5897));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5898));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5900));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5902));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5903));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5905));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5907));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5910));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5912));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5913));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5915));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5916));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5920));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5921));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5923));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5924));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5926));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4501));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4503));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4507));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4523));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4540));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4542));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4544));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4546));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4548));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4550));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4552));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4556));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4557));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5217));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5234));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5238));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5241));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5245));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5249));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5252));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5256));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5277));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5281));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5297));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5301));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5127));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5130));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4113));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4116));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4118));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4644));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4645));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4646));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4648));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4649));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4650));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4652));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4653));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4654));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4656));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4657));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4658));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4661));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4698));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4700));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4702));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4703));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4704));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4705));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4707));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4708));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4709));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4712));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4713));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4715));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4716));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4748));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4752));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4754));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4755));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4841));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4462));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4465));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4418));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FechaAlta" },
                values: new object[] { "admin@miempresa.com", new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4879) });

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FechaAlta", "PasswordHash" },
                values: new object[] { "gerente@miempresa.com", new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4906), "$2a$12$WDfsTRGXXJUgkt/2ETwMb.IHj4pLKMzg.uthmji/O8u7QFzv.kpw2" });

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FechaAlta", "PasswordHash" },
                values: new object[] { "vendedor@miempresa.com", new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4914), "$2a$12$zSKdkIO6EG0FVBmfvc8L9uSkHz35E5ENyP7v/JSElqAAAXuSZtnLG" });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5492));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5496));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5505));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5509));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5513));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5518));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5522));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5535));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5539));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5543));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5547));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5552));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5564));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5578));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5582));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5587));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5591));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5595));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5599));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5604));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5608));

            migrationBuilder.CreateIndex(
                name: "IX_ProveedorProductoCostos_ProductoId",
                table: "ProveedorProductoCostos",
                column: "ProductoId");

            // Index de ProveedorProductoCostos ya fue creado en ProveedorProductoCosto

            // Recrear vista que depende de MetodoPago (dropeada al inicio del Up)
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS VistaVentasResumidas;
                CREATE VIEW VistaVentasResumidas AS
                SELECT
                    v.Id,
                    v.Fecha,
                    v.TotalFinal AS Total,
                    v.Estado,
                    c.Nombre AS ClienteNombre,
                    (u.Nombre || ' ' || u.Apellido) AS UsuarioNombre,
                    s.Nombre AS SucursalNombre,
                    mp.Nombre AS MetodoPagoNombre
                FROM Venta v
                LEFT JOIN Cliente c ON v.Id_cliente = c.Id
                LEFT JOIN Usuario u ON v.Id_usuario = u.Id
                LEFT JOIN Sucursal s ON v.Id_sucursal = s.Id
                LEFT JOIN Pago p ON p.Id_venta = v.Id
                LEFT JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ProveedorProductoCostos no se dropea aquí (fue creada en ProveedorProductoCosto)

            // Rebuild MetodoPago via raw SQL (reversión: Categoria → EsEfectivo)
            migrationBuilder.Sql("DROP VIEW IF EXISTS VistaVentasResumidas;");
            migrationBuilder.Sql("DELETE FROM Pago;");
            migrationBuilder.Sql(@"
                CREATE TABLE tmp_MetodoPago_new (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_MetodoPago"" PRIMARY KEY AUTOINCREMENT,
                    ""Nombre"" TEXT NOT NULL,
                    ""EsEfectivo"" INTEGER NOT NULL DEFAULT 0,
                    ""Activo"" INTEGER NOT NULL DEFAULT 1,
                    ""Id_empresa"" INTEGER NOT NULL,
                    ""FechaAlta"" TEXT NULL,
                    CONSTRAINT ""FK_MetodoPago_Empresa_Id_empresa"" FOREIGN KEY (""Id_empresa"") REFERENCES ""Empresa"" (""Id"") ON DELETE CASCADE
                );

                INSERT INTO tmp_MetodoPago_new (""Id"", ""Nombre"", ""EsEfectivo"", ""Activo"", ""Id_empresa"", ""FechaAlta"")
                SELECT ""Id"", ""Nombre"",
                    CASE WHEN ""Categoria"" = 'Efectivo' THEN 1 ELSE 0 END,
                    ""Activo"", ""Id_empresa"", ""FechaAlta""
                FROM ""MetodoPago"";

                DROP TABLE ""MetodoPago"";
                ALTER TABLE tmp_MetodoPago_new RENAME TO ""MetodoPago"";
            ");

            // Pago se restaura via UpdateData al final del Down

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Empresa");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4225), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4240), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4245), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4249), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4254), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FechaAlta", "Turno" },
                values: new object[] { new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4258), null });

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3881));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3884));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3886));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3888));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3890));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3892));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3894));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3896));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3898));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4025));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4028));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4030));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4032));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4034));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4036));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4821));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4827));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4836));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4840));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4844));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4847));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4850));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4857));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4861));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "EsEfectivo",
                value: true);

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "EsEfectivo",
                value: false);

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "EsEfectivo",
                value: false);

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "EsEfectivo",
                value: false);

            migrationBuilder.UpdateData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "EsEfectivo",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4717));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4722));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4723));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4725));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4727));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4729));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4730));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4732));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4733));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4735));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4737));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4738));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4740));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4741));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4743));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4745));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4746));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4748));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4751));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4753));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4754));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4756));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4760));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4762));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4764));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3322));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3325));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3327));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3328));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3342));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3356));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3358));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3359));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3361));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3363));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3366));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3368));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3370));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3371));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4088));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4102));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4106));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4115));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4119));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4122));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4126));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4129));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4152));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4156));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4159));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4163));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4167));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4171));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4175));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3985));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3988));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3990));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3992));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(2981));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(2984));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(2996));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3462));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3463));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3464));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3466));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3467));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3468));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3469));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3470));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3471));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3472));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3473));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3474));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3475));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3477));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3478));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3517));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3519));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3521));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3522));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3523));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3524));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3525));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3526));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3527));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3529));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3530));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3531));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3532));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3533));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3565));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3568));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3570));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3571));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3674));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3278));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3233));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3235));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3237));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3238));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FechaAlta" },
                values: new object[] { "admin@sistema.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3712) });

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FechaAlta", "PasswordHash" },
                values: new object[] { "gerente@sistema.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3744), "$2a$12$NKA/6TaLtSB80UsdZUsZN.uO0IhAMH03WPDNeRQMOHrN/XRTECI9a" });

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FechaAlta", "PasswordHash" },
                values: new object[] { "vendedor@sistema.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3748), "$2a$12$v4qlp9oXiSIn8kCyfNdmU.fQJMAETzMpXvXVF9h5U.TnxOvq1yolu" });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4325));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4332));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4336));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4340));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4344));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4348));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4352));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4356));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4369));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4373));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4377));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4381));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4385));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4389));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4393));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4397));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4405));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4409));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4412));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4424));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4428));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4432));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4436));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4440));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4444));

            // Recrear vista que depende de MetodoPago
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS VistaVentasResumidas;
                CREATE VIEW VistaVentasResumidas AS
                SELECT
                    v.Id,
                    v.Fecha,
                    v.TotalFinal AS Total,
                    v.Estado,
                    c.Nombre AS ClienteNombre,
                    (u.Nombre || ' ' || u.Apellido) AS UsuarioNombre,
                    s.Nombre AS SucursalNombre,
                    mp.Nombre AS MetodoPagoNombre
                FROM Venta v
                LEFT JOIN Cliente c ON v.Id_cliente = c.Id
                LEFT JOIN Usuario u ON v.Id_usuario = u.Id
                LEFT JOIN Sucursal s ON v.Id_sucursal = s.Id
                LEFT JOIN Pago p ON p.Id_venta = v.Id
                LEFT JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id;
            ");
        }
    }
}
