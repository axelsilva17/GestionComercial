using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CUIT = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TablasAuditadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreTabla = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Habilitada = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CamposExcluidos = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablasAuditadas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoMovimientoStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMovimientoStock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Abreviatura = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CategoriaPadre_id = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categoria_Categoria_CategoriaPadre_id",
                        column: x => x.CategoriaPadre_id,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categoria_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Documento = table.Column<int>(type: "INTEGER", maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cliente_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetodoPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EsEfectivo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodoPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetodoPago_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CUIT = table.Column<string>(type: "TEXT", maxLength: 13, nullable: true),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proveedor_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sucursal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sucursal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sucursal_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id_rol = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_permiso = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Permiso_Id_permiso",
                        column: x => x.Id_permiso,
                        principalTable: "Permiso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Rol_Id_rol",
                        column: x => x.Id_rol,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CodigoBarra = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PrecioVentaActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioCostoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockActual = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_categoria = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_unidadMedida = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_Id_categoria",
                        column: x => x.Id_categoria,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Producto_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Producto_UnidadMedida_Id_unidadMedida",
                        column: x => x.Id_unidadMedida,
                        principalTable: "UnidadMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PreguntaSecreta = table.Column<string>(type: "TEXT", nullable: true),
                    RespuestaHash = table.Column<string>(type: "TEXT", nullable: true),
                    IntentosFallidos = table.Column<int>(type: "INTEGER", nullable: false),
                    BloqueadoHasta = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Id_sucursal = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_rol = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_Id_rol",
                        column: x => x.Id_rol,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Sucursal_Id_sucursal",
                        column: x => x.Id_sucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreTabla = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RegistroId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoOperacion = table.Column<int>(type: "INTEGER", nullable: false),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: true),
                    NombreUsuario = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FechaOperacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValoresAnteriores = table.Column<string>(type: "TEXT", nullable: true),
                    ValoresNuevos = table.Column<string>(type: "TEXT", nullable: true),
                    Workstation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IdEmpresa = table.Column<int>(type: "INTEGER", nullable: true),
                    IdSucursal = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Empresa_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Sucursal_IdSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Caja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaApertura = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MontoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    UsuarioApertura_id = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioCierre_id = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_sucursal = table.Column<int>(type: "INTEGER", nullable: false),
                    EsPrimaria = table.Column<bool>(type: "INTEGER", nullable: false),
                    Turno = table.Column<string>(type: "TEXT", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caja_Sucursal_Id_sucursal",
                        column: x => x.Id_sucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Caja_Usuario_UsuarioApertura_id",
                        column: x => x.UsuarioApertura_id,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Caja_Usuario_UsuarioCierre_id",
                        column: x => x.UsuarioCierre_id,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Id_proveedor = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_sucursal = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_usuario = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compra_Proveedor_Id_proveedor",
                        column: x => x.Id_proveedor,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Sucursal_Id_sucursal",
                        column: x => x.Id_sucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TipoMovimiento = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    StockAnterior = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    StockNuevo = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReferenciaId = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_sucursal = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_producto = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_usuario = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Producto_Id_producto",
                        column: x => x.Id_producto,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Sucursal_Id_sucursal",
                        column: x => x.Id_sucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Venta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Id_sucursal = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_cliente = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_usuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_caja = table.Column<int>(type: "INTEGER", nullable: true),
                    EfectivoRecibido = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MotivoAnulacion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FechaAnulacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UsuarioAnulacionId = table.Column<int>(type: "INTEGER", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venta_Caja_Id_caja",
                        column: x => x.Id_caja,
                        principalTable: "Caja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Venta_Cliente_Id_cliente",
                        column: x => x.Id_cliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Venta_Sucursal_Id_sucursal",
                        column: x => x.Id_sucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Venta_Usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompraDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id_compra = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_producto = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompraDetalle_Compra_Id_compra",
                        column: x => x.Id_compra,
                        principalTable: "Compra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraDetalle_Producto_Id_producto",
                        column: x => x.Id_producto,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Concepto = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ReferenciaId = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_caja = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_usuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_venta = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoCaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientoCaja_Caja_Id_caja",
                        column: x => x.Id_caja,
                        principalTable: "Caja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoCaja_Usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoCaja_Venta_Id_venta",
                        column: x => x.Id_venta,
                        principalTable: "Venta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargenUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id_venta = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_producto = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDetalle_Producto_Id_producto",
                        column: x => x.Id_producto,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VentaDetalle_Venta_Id_venta",
                        column: x => x.Id_venta,
                        principalTable: "Venta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Id_venta = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_metodoPago = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_movimientoCaja = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pago_MetodoPago_Id_metodoPago",
                        column: x => x.Id_metodoPago,
                        principalTable: "MetodoPago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pago_MovimientoCaja_Id_movimientoCaja",
                        column: x => x.Id_movimientoCaja,
                        principalTable: "MovimientoCaja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pago_Venta_Id_venta",
                        column: x => x.Id_venta,
                        principalTable: "Venta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalleDescuento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id_detalle = table.Column<int>(type: "INTEGER", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalleDescuento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDetalleDescuento_VentaDetalle_Id_detalle",
                        column: x => x.Id_detalle,
                        principalTable: "VentaDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalleImpuesto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id_detalle = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_tipoImpuesto = table.Column<int>(type: "INTEGER", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalleImpuesto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDetalleImpuesto_VentaDetalle_Id_detalle",
                        column: x => x.Id_detalle,
                        principalTable: "VentaDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Empresa",
                columns: new[] { "Id", "Activo", "CUIT", "Direccion", "Email", "FechaAlta", "Nombre", "Telefono" },
                values: new object[] { 1, true, "20-12345678-9", "Dirección Principal 123", "admin@miempresa.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3621), "Mi Empresa", "3794000000" });

            migrationBuilder.InsertData(
                table: "Permiso",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Ver ventas", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3322), "Ventas.Ver" },
                    { 2, true, "Crear ventas", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3325), "Ventas.Crear" },
                    { 3, true, "Anular ventas", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3327), "Ventas.Anular" },
                    { 4, true, "Ver compras", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3328), "Compras.Ver" },
                    { 5, true, "Crear compras", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3342), "Compras.Crear" },
                    { 6, true, "Ver productos", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3356), "Productos.Ver" },
                    { 7, true, "Crear productos", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3358), "Productos.Crear" },
                    { 8, true, "Editar productos", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3359), "Productos.Editar" },
                    { 9, true, "Ver clientes", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3361), "Clientes.Ver" },
                    { 10, true, "Crear clientes", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3363), "Clientes.Crear" },
                    { 11, true, "Ver reportes", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3365), "Reportes.Ver" },
                    { 12, true, "Abrir caja", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3366), "Caja.Abrir" },
                    { 13, true, "Cerrar caja", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3368), "Caja.Cerrar" },
                    { 14, true, "Ver configuración", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3370), "Configuracion.Ver" },
                    { 15, true, "Gestionar usuarios", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3371), "Usuarios.Gestionar" }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Acceso total al sistema", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(2981), "Gerente" },
                    { 2, true, "Administración general", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(2984), "Administrador" },
                    { 3, true, "Operaciones de venta", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(2996), "Vendedor" }
                });

            migrationBuilder.InsertData(
                table: "TipoDocumento",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Documento Nacional de Identidad", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3278), "DNI" },
                    { 2, true, "Clave Única de Identificación Tributaria", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3280), "CUIT" },
                    { 3, true, "Pasaporte", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3282), "Pasaporte" }
                });

            migrationBuilder.InsertData(
                table: "TipoMovimientoStock",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Ingreso de mercadería", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3233), "Entrada" },
                    { 2, true, "Egreso de mercadería", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3235), "Salida" },
                    { 3, true, "Ajuste positivo de stock", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3237), "Ajuste Positivo" },
                    { 4, true, "Ajuste negativo de stock", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3238), "Ajuste Negativo" }
                });

            migrationBuilder.InsertData(
                table: "UnidadMedida",
                columns: new[] { "Id", "Abreviatura", "Nombre" },
                values: new object[,]
                {
                    { 1, "UN", "Unidad" },
                    { 2, "KG", "Kilogramo" },
                    { 3, "MT", "Metro" },
                    { 4, "LT", "Litro" },
                    { 5, "CJA", "Caja" }
                });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Activo", "CategoriaPadre_id", "FechaAlta", "Id_empresa", "Nombre" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3881), 1, "Herramientas" },
                    { 2, true, null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3884), 1, "Materiales" },
                    { 3, true, null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3886), 1, "Electricidad" },
                    { 4, true, null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3888), 1, "Pintura" },
                    { 5, true, null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3890), 1, "Plomería" }
                });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "Id", "Activo", "Documento", "Email", "FechaAlta", "Id_empresa", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, true, 0, null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4025), 1, "Consumidor Final", null },
                    { 2, true, 30111111, "juan@gmail.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4028), 1, "Juan Pérez", "3794555001" },
                    { 3, true, 32222222, "maria@gmail.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4030), 1, "María González", "3794555002" },
                    { 4, true, 28333333, "carlos@empresa.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4032), 1, "Carlos Rodríguez", "3794555003" },
                    { 5, true, 35444444, "laura@gmail.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4034), 1, "Laura Martínez", "3794555004" },
                    { 6, true, 30555555, "compras@constructora.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4036), 1, "Constructora ABC", "3794555005" }
                });

            migrationBuilder.InsertData(
                table: "MetodoPago",
                columns: new[] { "Id", "Activo", "EsEfectivo", "Id_empresa", "Nombre" },
                values: new object[,]
                {
                    { 1, true, true, 1, "Efectivo" },
                    { 2, true, false, 1, "Débito" },
                    { 3, true, false, 1, "Crédito" },
                    { 4, true, false, 1, "Transferencia" },
                    { 5, true, false, 1, "Mercado Pago" }
                });

            migrationBuilder.InsertData(
                table: "Proveedor",
                columns: new[] { "Id", "Activo", "CUIT", "Email", "FechaAlta", "Id_empresa", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, true, "30-11111111-1", "norte@proveedor.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3985), 1, "Distribuidora Norte", "3794111111" },
                    { 2, true, "30-22222222-2", "sur@pinturerias.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3988), 1, "Pinturerias del Sur", "3794222222" },
                    { 3, true, "30-33333333-3", "ventas@electro.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3990), 1, "Electro Mayorista", "3794333333" },
                    { 4, true, "30-44444444-4", "info@constructor.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3992), 1, "Materiales El Constructor", "3794444444" }
                });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "Id", "Activo", "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3462), 1, 1 },
                    { 2, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3463), 2, 1 },
                    { 3, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3464), 3, 1 },
                    { 4, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3466), 4, 1 },
                    { 5, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3467), 5, 1 },
                    { 6, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3468), 6, 1 },
                    { 7, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3469), 7, 1 },
                    { 8, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3470), 8, 1 },
                    { 9, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3471), 9, 1 },
                    { 10, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3472), 10, 1 },
                    { 11, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3473), 11, 1 },
                    { 12, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3474), 12, 1 },
                    { 13, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3475), 13, 1 },
                    { 14, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3477), 14, 1 },
                    { 15, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3478), 15, 1 },
                    { 16, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3517), 1, 2 },
                    { 17, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3519), 2, 2 },
                    { 18, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3521), 3, 2 },
                    { 19, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3522), 4, 2 },
                    { 20, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3523), 5, 2 },
                    { 21, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3524), 6, 2 },
                    { 22, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3525), 7, 2 },
                    { 23, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3526), 8, 2 },
                    { 24, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3527), 9, 2 },
                    { 25, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3529), 10, 2 },
                    { 26, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3530), 11, 2 },
                    { 27, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3531), 12, 2 },
                    { 28, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3532), 13, 2 },
                    { 29, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3533), 14, 2 },
                    { 30, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3565), 1, 3 },
                    { 31, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3567), 2, 3 },
                    { 32, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3568), 6, 3 },
                    { 33, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3570), 9, 3 },
                    { 34, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3571), 10, 3 },
                    { 35, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3573), 12, 3 },
                    { 36, true, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3574), 13, 3 }
                });

            migrationBuilder.InsertData(
                table: "Sucursal",
                columns: new[] { "Id", "Activo", "Direccion", "FechaAlta", "Id_empresa", "Nombre", "Telefono" },
                values: new object[] { 1, true, "Dirección Principal 123", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3674), 1, "Casa Central", "3794000000" });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Activo", "CategoriaPadre_id", "FechaAlta", "Id_empresa", "Nombre" },
                values: new object[,]
                {
                    { 6, true, 1, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3892), 1, "Manuales" },
                    { 7, true, 1, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3894), 1, "Eléctricas" },
                    { 8, true, 4, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3896), 1, "Látex" },
                    { 9, true, 4, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3898), 1, "Esmalte" }
                });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "Activo", "CodigoBarra", "Descripcion", "FechaAlta", "Id_categoria", "Id_empresa", "Id_unidadMedida", "Nombre", "PrecioCostoActual", "PrecioVentaActual", "StockActual", "StockMinimo" },
                values: new object[,]
                {
                    { 11, true, "7790001000011", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4159), 3, 1, 3, "Cable Unipolar 1.5mm x5m", 3500m, 5500m, 50m, 10m },
                    { 12, true, "7790001000012", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4163), 3, 1, 1, "Tomacorriente Doble", 2000m, 3200m, 35m, 8m },
                    { 13, true, "7790001000013", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4167), 3, 1, 1, "Disyuntor 16A", 5100m, 7800m, 22m, 5m },
                    { 14, true, "7790001000014", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4171), 5, 1, 3, "Caño PVC 1\" x3m", 3100m, 4800m, 40m, 8m },
                    { 15, true, "7790001000015", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4175), 5, 1, 1, "Codo PVC 90° 1\"", 520m, 850m, 3m, 10m }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Activo", "Apellido", "BloqueadoHasta", "Email", "FechaAlta", "Id_rol", "Id_sucursal", "IntentosFallidos", "Nombre", "PasswordHash", "PreguntaSecreta", "RespuestaHash", "UltimoAcceso" },
                values: new object[,]
                {
                    { 1, true, "Sistema", null, "admin@sistema.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3712), 2, 1, 0, "Administrador", "$2a$12$1afFAY7Q1dY9UOpV5EboqOM9P1IO41RZz4F01zEqC918SeOU0qaRy", null, null, null },
                    { 2, true, "General", null, "gerente@sistema.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3744), 1, 1, 0, "Gerente", "$2a$12$NKA/6TaLtSB80UsdZUsZN.uO0IhAMH03WPDNeRQMOHrN/XRTECI9a", null, null, null },
                    { 3, true, "Sistema", null, "vendedor@sistema.com", new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(3748), 3, 1, 0, "Vendedor", "$2a$12$v4qlp9oXiSIn8kCyfNdmU.fQJMAETzMpXvXVF9h5U.TnxOvq1yolu", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Caja",
                columns: new[] { "Id", "Activo", "EsPrimaria", "Estado", "FechaAlta", "FechaApertura", "FechaCierre", "Id_sucursal", "MontoFinal", "MontoInicial", "Observacion", "Turno", "UsuarioApertura_id", "UsuarioCierre_id" },
                values: new object[,]
                {
                    { 1, true, false, 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4225), new DateTime(2025, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 285000m, 50000m, null, null, 1, 1 },
                    { 2, true, false, 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4240), new DateTime(2025, 11, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 320000m, 50000m, null, null, 1, 1 },
                    { 3, true, false, 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4245), new DateTime(2025, 12, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 410000m, 50000m, null, null, 1, 1 },
                    { 4, true, false, 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4249), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 380000m, 50000m, null, null, 1, 1 },
                    { 5, true, false, 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4254), new DateTime(2026, 2, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 450000m, 50000m, null, null, 1, 1 },
                    { 6, true, false, 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4258), new DateTime(2026, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 520000m, 50000m, null, null, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Compra",
                columns: new[] { "Id", "Activo", "Estado", "Fecha", "FechaAlta", "Id_proveedor", "Id_sucursal", "Id_usuario", "Observacion", "Total" },
                values: new object[,]
                {
                    { 1, true, 2, new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4821), 1, 1, 1, null, 145000m },
                    { 2, true, 2, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4827), 2, 1, 1, null, 84000m },
                    { 3, true, 2, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4836), 3, 1, 1, null, 108000m },
                    { 4, true, 2, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4840), 4, 1, 1, null, 62000m },
                    { 5, true, 2, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4844), 1, 1, 1, null, 174000m },
                    { 6, true, 2, new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4847), 2, 1, 1, null, 96000m },
                    { 7, true, 2, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4850), 3, 1, 1, null, 132000m },
                    { 8, true, 2, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4854), 4, 1, 1, null, 78000m },
                    { 9, true, 2, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4857), 1, 1, 1, null, 158000m },
                    { 10, true, 2, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4861), 2, 1, 1, null, 91000m },
                    { 11, true, 2, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4864), 1, 1, 1, null, 165000m },
                    { 12, true, 2, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4868), 3, 1, 1, null, 88000m }
                });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "Activo", "CodigoBarra", "Descripcion", "FechaAlta", "Id_categoria", "Id_empresa", "Id_unidadMedida", "Nombre", "PrecioCostoActual", "PrecioVentaActual", "StockActual", "StockMinimo" },
                values: new object[,]
                {
                    { 1, true, "7790001000001", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4088), 6, 1, 1, "Martillo 500g", 5500m, 8500m, 25m, 5m },
                    { 2, true, "7790001000002", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4102), 6, 1, 1, "Destornillador Philips", 2600m, 4200m, 40m, 8m },
                    { 3, true, "7790001000003", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4106), 6, 1, 1, "Alicate Universal", 4200m, 6800m, 18m, 5m },
                    { 4, true, "7790001000004", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4115), 6, 1, 1, "Sierra Arco", 8000m, 12500m, 12m, 3m },
                    { 5, true, "7790001000005", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4119), 7, 1, 1, "Taladro 650W", 58000m, 85000m, 8m, 2m },
                    { 6, true, "7790001000006", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4122), 7, 1, 1, "Amoladora 115mm", 50000m, 72000m, 6m, 2m },
                    { 7, true, "7790001000007", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4126), 8, 1, 4, "Látex Interior Blanco 4L", 12000m, 18500m, 30m, 6m },
                    { 8, true, "7790001000008", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4129), 8, 1, 4, "Látex Exterior 10L", 28000m, 42000m, 15m, 4m },
                    { 9, true, "7790001000009", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4152), 9, 1, 4, "Esmalte Sintético 1L", 6400m, 9800m, 20m, 5m },
                    { 10, true, "7790001000010", null, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4156), 9, 1, 4, "Esmalte Negro 4L", 21000m, 32000m, 10m, 3m }
                });

            migrationBuilder.InsertData(
                table: "CompraDetalle",
                columns: new[] { "Id", "Cantidad", "Id_compra", "Id_producto", "PrecioCosto", "Subtotal" },
                values: new object[,]
                {
                    { 1, 10m, 1, 1, 5500m, 55000m },
                    { 2, 15m, 1, 2, 2600m, 39000m },
                    { 3, 8m, 1, 3, 4200m, 33600m },
                    { 4, 4m, 1, 4, 8000m, 32000m },
                    { 5, 10m, 2, 7, 12000m, 120000m },
                    { 6, 8m, 2, 9, 6400m, 51200m },
                    { 7, 1m, 3, 5, 58000m, 58000m },
                    { 8, 10m, 3, 11, 3500m, 35000m },
                    { 9, 10m, 3, 12, 2000m, 20000m },
                    { 10, 10m, 4, 14, 3100m, 31000m },
                    { 11, 20m, 4, 15, 520m, 10400m },
                    { 12, 8m, 4, 13, 5100m, 40800m },
                    { 13, 1m, 5, 5, 58000m, 58000m },
                    { 14, 1m, 5, 6, 50000m, 50000m },
                    { 15, 12m, 5, 1, 5500m, 66000m },
                    { 16, 2m, 6, 8, 28000m, 56000m },
                    { 17, 2m, 6, 10, 21000m, 42000m },
                    { 18, 8m, 7, 1, 5500m, 44000m },
                    { 19, 10m, 7, 3, 4200m, 42000m },
                    { 20, 6m, 7, 4, 8000m, 48000m },
                    { 21, 12m, 8, 11, 3500m, 42000m },
                    { 22, 12m, 8, 12, 2000m, 24000m },
                    { 23, 1m, 9, 5, 58000m, 58000m },
                    { 24, 1m, 9, 6, 50000m, 50000m },
                    { 25, 4m, 9, 7, 12000m, 48000m },
                    { 26, 2m, 10, 8, 28000m, 56000m },
                    { 27, 5m, 10, 9, 6400m, 32000m },
                    { 28, 15m, 11, 1, 5500m, 82500m },
                    { 29, 20m, 11, 2, 2600m, 52000m },
                    { 30, 8m, 11, 3, 4200m, 33600m },
                    { 31, 1m, 12, 5, 58000m, 58000m },
                    { 32, 1m, 12, 10, 21000m, 21000m }
                });

            migrationBuilder.InsertData(
                table: "Venta",
                columns: new[] { "Id", "Activo", "EfectivoRecibido", "Estado", "Fecha", "FechaAlta", "FechaAnulacion", "Id_caja", "Id_cliente", "Id_sucursal", "Id_usuario", "MotivoAnulacion", "Observacion", "TotalBruto", "TotalDescuento", "TotalFinal", "UsuarioAnulacionId" },
                values: new object[,]
                {
                    { 1, true, null, 2, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4325), null, 1, 2, 1, 1, null, null, 25700m, 0m, 25700m, null },
                    { 2, true, null, 2, new DateTime(2025, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4332), null, 1, 1, 1, 1, null, null, 85000m, 0m, 85000m, null },
                    { 3, true, null, 2, new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4336), null, 1, 3, 1, 1, null, null, 18500m, 925m, 17575m, null },
                    { 4, true, null, 2, new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4340), null, 1, 6, 1, 1, null, null, 54800m, 0m, 54800m, null },
                    { 5, true, null, 2, new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4344), null, 1, 4, 1, 1, null, null, 32000m, 1600m, 30400m, null },
                    { 6, true, null, 2, new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4348), null, 2, 1, 1, 1, null, null, 42000m, 0m, 42000m, null },
                    { 7, true, null, 2, new DateTime(2025, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4352), null, 2, 2, 1, 1, null, null, 12500m, 0m, 12500m, null },
                    { 8, true, null, 2, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4356), null, 2, 6, 1, 1, null, null, 93600m, 4680m, 88920m, null },
                    { 9, true, null, 2, new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4369), null, 2, 5, 1, 1, null, null, 27300m, 0m, 27300m, null },
                    { 10, true, null, 2, new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4373), null, 2, 3, 1, 1, null, null, 16800m, 0m, 16800m, null },
                    { 11, true, null, 2, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4377), null, 3, 6, 1, 1, null, null, 72000m, 3600m, 68400m, null },
                    { 12, true, null, 2, new DateTime(2025, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4381), null, 3, 1, 1, 1, null, null, 9800m, 0m, 9800m, null },
                    { 13, true, null, 2, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4385), null, 3, 4, 1, 1, null, null, 47500m, 0m, 47500m, null },
                    { 14, true, null, 2, new DateTime(2025, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4389), null, 3, 6, 1, 1, null, null, 156000m, 7800m, 148200m, null },
                    { 15, true, null, 2, new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4393), null, 3, 2, 1, 1, null, null, 22400m, 0m, 22400m, null },
                    { 16, true, null, 2, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4397), null, 4, 1, 1, 1, null, null, 38500m, 0m, 38500m, null },
                    { 17, true, null, 2, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4401), null, 4, 6, 1, 1, null, null, 92000m, 4600m, 87400m, null },
                    { 18, true, null, 2, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4405), null, 4, 3, 1, 1, null, null, 27300m, 0m, 27300m, null },
                    { 19, true, null, 2, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4409), null, 4, 4, 1, 1, null, null, 54000m, 0m, 54000m, null },
                    { 20, true, null, 2, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4412), null, 4, 2, 1, 1, null, null, 18500m, 925m, 17575m, null },
                    { 21, true, null, 2, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4416), null, 5, 6, 1, 1, null, null, 85000m, 0m, 85000m, null },
                    { 22, true, null, 2, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4420), null, 5, 1, 1, 1, null, null, 42000m, 2100m, 39900m, null },
                    { 23, true, null, 2, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4424), null, 5, 5, 1, 1, null, null, 32000m, 0m, 32000m, null },
                    { 24, true, null, 2, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4428), null, 5, 6, 1, 1, null, null, 72000m, 3600m, 68400m, null },
                    { 25, true, null, 2, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4432), null, 5, 3, 1, 1, null, null, 16800m, 0m, 16800m, null },
                    { 26, true, null, 2, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4436), null, 6, 4, 1, 1, null, null, 54800m, 0m, 54800m, null },
                    { 27, true, null, 2, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4440), null, 6, 6, 1, 1, null, null, 93600m, 4680m, 88920m, null },
                    { 28, true, null, 2, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4444), null, 6, 2, 1, 1, null, null, 25700m, 0m, 25700m, null }
                });

            migrationBuilder.InsertData(
                table: "Pago",
                columns: new[] { "Id", "Fecha", "Id_metodoPago", "Id_movimientoCaja", "Id_venta", "Monto" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4717), 1, null, 1, 25700m },
                    { 2, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4722), 3, null, 2, 85000m },
                    { 3, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4723), 1, null, 3, 17575m },
                    { 4, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4725), 4, null, 4, 54800m },
                    { 5, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4727), 2, null, 5, 30400m },
                    { 6, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4729), 1, null, 6, 42000m },
                    { 7, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4730), 5, null, 7, 12500m },
                    { 8, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4732), 4, null, 8, 88920m },
                    { 9, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4733), 1, null, 9, 27300m },
                    { 10, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4735), 2, null, 10, 16800m },
                    { 11, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4737), 3, null, 11, 68400m },
                    { 12, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4738), 1, null, 12, 9800m },
                    { 13, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4740), 4, null, 13, 47500m },
                    { 14, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4741), 3, null, 14, 148200m },
                    { 15, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4743), 1, null, 15, 22400m },
                    { 16, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4745), 1, null, 16, 38500m },
                    { 17, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4746), 3, null, 17, 87400m },
                    { 18, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4748), 2, null, 18, 27300m },
                    { 19, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4749), 4, null, 19, 54000m },
                    { 20, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4751), 1, null, 20, 17575m },
                    { 21, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4753), 3, null, 21, 85000m },
                    { 22, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4754), 1, null, 22, 39900m },
                    { 23, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4756), 2, null, 23, 32000m },
                    { 24, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4757), 4, null, 24, 68400m },
                    { 25, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4759), 1, null, 25, 16800m },
                    { 26, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4760), 5, null, 26, 54800m },
                    { 27, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4762), 3, null, 27, 88920m },
                    { 28, new DateTime(2026, 4, 26, 17, 44, 55, 979, DateTimeKind.Local).AddTicks(4764), 1, null, 28, 25700m }
                });

            migrationBuilder.InsertData(
                table: "VentaDetalle",
                columns: new[] { "Id", "Cantidad", "CostoUnitario", "Descuento", "Id_producto", "Id_venta", "MargenUnitario", "PrecioUnitario", "Subtotal" },
                values: new object[,]
                {
                    { 1, 1m, 5500m, 0m, 1, 1, 3000m, 8500m, 8500m },
                    { 2, 2m, 2600m, 0m, 2, 1, 1600m, 4200m, 8400m },
                    { 3, 2m, 2000m, 0m, 12, 1, 1200m, 3200m, 6400m },
                    { 4, 1m, 5100m, 0m, 13, 1, 2700m, 7800m, 7800m },
                    { 5, 1m, 58000m, 0m, 5, 2, 27000m, 85000m, 85000m },
                    { 6, 1m, 12000m, 925m, 7, 3, 5575m, 18500m, 17575m },
                    { 7, 1m, 28000m, 0m, 8, 4, 14000m, 42000m, 42000m },
                    { 8, 1m, 6400m, 0m, 9, 4, 3400m, 9800m, 9800m },
                    { 9, 1m, 4200m, 0m, 3, 4, 2600m, 6800m, 6800m },
                    { 10, 1m, 21000m, 1600m, 10, 5, 9400m, 32000m, 30400m },
                    { 11, 1m, 28000m, 0m, 8, 6, 14000m, 42000m, 42000m },
                    { 12, 1m, 8000m, 0m, 4, 7, 4500m, 12500m, 12500m },
                    { 13, 1m, 58000m, 4680m, 5, 8, 22320m, 85000m, 80320m },
                    { 14, 1m, 50000m, 0m, 6, 8, 22000m, 72000m, 72000m },
                    { 15, 1m, 12000m, 0m, 7, 9, 6500m, 18500m, 18500m },
                    { 16, 1m, 3500m, 0m, 11, 9, 2000m, 5500m, 5500m },
                    { 17, 1m, 2600m, 0m, 2, 9, 1600m, 4200m, 4200m },
                    { 18, 1m, 6400m, 0m, 9, 10, 3400m, 9800m, 9800m },
                    { 19, 1m, 4200m, 0m, 3, 10, 2600m, 6800m, 6800m },
                    { 20, 1m, 50000m, 3600m, 6, 11, 18400m, 72000m, 68400m },
                    { 21, 1m, 6400m, 0m, 9, 12, 3400m, 9800m, 9800m },
                    { 22, 1m, 28000m, 0m, 8, 13, 14000m, 42000m, 42000m },
                    { 23, 1m, 5500m, 0m, 1, 13, 3000m, 8500m, 8500m },
                    { 24, 1m, 58000m, 7800m, 5, 14, 19200m, 85000m, 77200m },
                    { 25, 1m, 50000m, 0m, 6, 14, 22000m, 72000m, 72000m },
                    { 26, 1m, 12000m, 0m, 7, 15, 6500m, 18500m, 18500m },
                    { 27, 1m, 3100m, 0m, 14, 15, 1700m, 4800m, 4800m },
                    { 28, 2m, 5500m, 0m, 1, 16, 3000m, 8500m, 17000m },
                    { 29, 3m, 2600m, 0m, 2, 16, 1600m, 4200m, 12600m },
                    { 30, 1m, 4200m, 0m, 3, 16, 2600m, 6800m, 6800m },
                    { 31, 1m, 58000m, 4600m, 5, 17, 22400m, 85000m, 80400m },
                    { 32, 1m, 12000m, 0m, 7, 18, 6500m, 18500m, 18500m },
                    { 33, 2m, 3500m, 0m, 11, 18, 2000m, 5500m, 11000m },
                    { 34, 1m, 28000m, 0m, 8, 19, 14000m, 42000m, 42000m },
                    { 35, 1m, 50000m, 0m, 6, 19, 22000m, 72000m, 72000m },
                    { 36, 1m, 12000m, 925m, 7, 20, 5575m, 18500m, 17575m },
                    { 37, 1m, 58000m, 0m, 5, 21, 27000m, 85000m, 85000m },
                    { 38, 1m, 28000m, 2100m, 8, 22, 11900m, 42000m, 39900m },
                    { 39, 1m, 21000m, 0m, 10, 23, 11000m, 32000m, 32000m },
                    { 40, 1m, 50000m, 3600m, 6, 24, 18400m, 72000m, 68400m },
                    { 41, 1m, 6400m, 0m, 9, 25, 3400m, 9800m, 9800m },
                    { 42, 1m, 4200m, 0m, 3, 25, 2600m, 6800m, 6800m },
                    { 43, 1m, 28000m, 0m, 8, 26, 14000m, 42000m, 42000m },
                    { 44, 1m, 6400m, 0m, 9, 26, 3400m, 9800m, 9800m },
                    { 45, 1m, 58000m, 4680m, 5, 27, 22320m, 85000m, 80320m },
                    { 46, 2m, 2600m, 0m, 2, 27, 1600m, 4200m, 8400m },
                    { 47, 1m, 5500m, 0m, 1, 28, 3000m, 8500m, 8500m },
                    { 48, 3m, 2000m, 0m, 12, 28, 1200m, 3200m, 9600m },
                    { 49, 1m, 5100m, 0m, 13, 28, 2700m, 7800m, 7800m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_FechaOperacion",
                table: "AuditoriaLogs",
                column: "FechaOperacion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_IdEmpresa",
                table: "AuditoriaLogs",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_IdSucursal",
                table: "AuditoriaLogs",
                column: "IdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_IdUsuario",
                table: "AuditoriaLogs",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_NombreTabla_RegistroId",
                table: "AuditoriaLogs",
                columns: new[] { "NombreTabla", "RegistroId" });

            migrationBuilder.CreateIndex(
                name: "IX_Caja_Id_sucursal_Estado",
                table: "Caja",
                columns: new[] { "Id_sucursal", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Caja_UsuarioApertura_id",
                table: "Caja",
                column: "UsuarioApertura_id");

            migrationBuilder.CreateIndex(
                name: "IX_Caja_UsuarioCierre_id",
                table: "Caja",
                column: "UsuarioCierre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_CategoriaPadre_id",
                table: "Categoria",
                column: "CategoriaPadre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Id_empresa",
                table: "Categoria",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_IdEmpresa",
                table: "Cliente",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_Id_proveedor",
                table: "Compra",
                column: "Id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_Id_sucursal",
                table: "Compra",
                column: "Id_sucursal");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_Id_usuario",
                table: "Compra",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDetalle_Id_compra",
                table: "CompraDetalle",
                column: "Id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDetalle_Id_producto",
                table: "CompraDetalle",
                column: "Id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_CUIT",
                table: "Empresa",
                column: "CUIT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetodoPago_Id_empresa",
                table: "MetodoPago",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoCaja_Id_caja",
                table: "MovimientoCaja",
                column: "Id_caja");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoCaja_Id_usuario",
                table: "MovimientoCaja",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoCaja_Id_venta",
                table: "MovimientoCaja",
                column: "Id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_Id_producto_Fecha",
                table: "MovimientoStock",
                columns: new[] { "Id_producto", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_Id_sucursal",
                table: "MovimientoStock",
                column: "Id_sucursal");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_Id_usuario",
                table: "MovimientoStock",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Id_metodoPago",
                table: "Pago",
                column: "Id_metodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Id_movimientoCaja",
                table: "Pago",
                column: "Id_movimientoCaja");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Id_venta",
                table: "Pago",
                column: "Id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_Permiso_Nombre",
                table: "Permiso",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CodigoBarra",
                table: "Producto",
                column: "CodigoBarra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CodigoBarra_Empresa",
                table: "Producto",
                columns: new[] { "CodigoBarra", "Id_empresa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Id_unidadMedida",
                table: "Producto",
                column: "Id_unidadMedida");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto",
                column: "Id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdEmpresa",
                table: "Producto",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_Id_empresa",
                table: "Proveedor",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_Id_permiso",
                table: "RolPermiso",
                column: "Id_permiso");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_Id_rol",
                table: "RolPermiso",
                column: "Id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Sucursal_Id_empresa",
                table: "Sucursal",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_TablasAuditadas_NombreTabla",
                table: "TablasAuditadas",
                column: "NombreTabla",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Id_rol",
                table: "Usuario",
                column: "Id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Id_sucursal",
                table: "Usuario",
                column: "Id_sucursal");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Fecha",
                table: "Venta",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Id_caja",
                table: "Venta",
                column: "Id_caja");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Id_cliente",
                table: "Venta",
                column: "Id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Id_sucursal_Fecha",
                table: "Venta",
                columns: new[] { "Id_sucursal", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Id_usuario",
                table: "Venta",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalle_Id_producto",
                table: "VentaDetalle",
                column: "Id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalle_Id_venta",
                table: "VentaDetalle",
                column: "Id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalleDescuento_Id_detalle",
                table: "VentaDetalleDescuento",
                column: "Id_detalle");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalleImpuesto_Id_detalle",
                table: "VentaDetalleImpuesto",
                column: "Id_detalle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaLogs");

            migrationBuilder.DropTable(
                name: "CompraDetalle");

            migrationBuilder.DropTable(
                name: "MovimientoStock");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "TablasAuditadas");

            migrationBuilder.DropTable(
                name: "TipoDocumento");

            migrationBuilder.DropTable(
                name: "TipoMovimientoStock");

            migrationBuilder.DropTable(
                name: "VentaDetalleDescuento");

            migrationBuilder.DropTable(
                name: "VentaDetalleImpuesto");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "MetodoPago");

            migrationBuilder.DropTable(
                name: "MovimientoCaja");

            migrationBuilder.DropTable(
                name: "Permiso");

            migrationBuilder.DropTable(
                name: "VentaDetalle");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Venta");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "UnidadMedida");

            migrationBuilder.DropTable(
                name: "Caja");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Sucursal");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
