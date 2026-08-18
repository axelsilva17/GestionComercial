using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcategoriaMetodoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subcategoria",
                table: "MetodoPago",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            // Backfill: set Subcategoria for existing Débito/Crédito rows and normalize Categoria to Tarjeta
            migrationBuilder.Sql(@"
                UPDATE ""MetodoPago"" SET ""Subcategoria"" = 'Debito' WHERE ""Nombre"" = 'Débito' AND ""Subcategoria"" IS NULL;
                UPDATE ""MetodoPago"" SET ""Subcategoria"" = 'Credito' WHERE ""Nombre"" = 'Crédito' AND ""Subcategoria"" IS NULL;
                UPDATE ""MetodoPago"" SET ""Categoria"" = 'Tarjeta' WHERE ""Nombre"" IN ('Débito','Crédito') AND ""Categoria"" != 'Tarjeta' AND ""Subcategoria"" IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subcategoria",
                table: "MetodoPago");
        }
    }
}
