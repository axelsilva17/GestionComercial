using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    public partial class AddMantenimientoLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MantenimientoLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Resultado = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Detalles = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientoLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MantenimientoLog");
        }
    }
}
