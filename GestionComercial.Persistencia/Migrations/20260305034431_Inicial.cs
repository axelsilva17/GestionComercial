using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CUIT = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoMovimientoStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMovimientoStock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoriaPadre_id = table.Column<int>(type: "int", nullable: true),
                    Id_empresa = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Id_empresa = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EsEfectivo = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Id_empresa = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CUIT = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Id_empresa = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Id_empresa = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_rol = table.Column<int>(type: "int", nullable: false),
                    Id_permiso = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoBarra = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioVentaActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioCostoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockActual = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Id_empresa = table.Column<int>(type: "int", nullable: false),
                    Id_categoria = table.Column<int>(type: "int", nullable: false),
                    Id_unidadMedida = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id_sucursal = table.Column<int>(type: "int", nullable: false),
                    Id_rol = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                name: "Caja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsuarioApertura_id = table.Column<int>(type: "int", nullable: false),
                    UsuarioCierre_id = table.Column<int>(type: "int", nullable: true),
                    Id_sucursal = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id_proveedor = table.Column<int>(type: "int", nullable: false),
                    Id_sucursal = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    StockAnterior = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    StockNuevo = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenciaId = table.Column<int>(type: "int", nullable: true),
                    Id_sucursal = table.Column<int>(type: "int", nullable: false),
                    Id_producto = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false)
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
                name: "MovimientoCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReferenciaId = table.Column<int>(type: "int", nullable: true),
                    Id_caja = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Venta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id_sucursal = table.Column<int>(type: "int", nullable: false),
                    Id_cliente = table.Column<int>(type: "int", nullable: false),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    Id_caja = table.Column<int>(type: "int", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id_compra = table.Column<int>(type: "int", nullable: false),
                    Id_producto = table.Column<int>(type: "int", nullable: false)
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
                name: "Pago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_venta = table.Column<int>(type: "int", nullable: false),
                    Id_metodoPago = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Pago_Venta_Id_venta",
                        column: x => x.Id_venta,
                        principalTable: "Venta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargenUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id_venta = table.Column<int>(type: "int", nullable: false),
                    Id_producto = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Empresa",
                columns: new[] { "Id", "Activo", "CUIT", "Direccion", "Email", "FechaAlta", "Nombre", "Telefono" },
                values: new object[] { 1, true, "20-12345678-9", "Dirección Principal 123", "admin@miempresa.com", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4424), "Mi Empresa", "3794000000" });

            migrationBuilder.InsertData(
                table: "Permiso",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Ver ventas", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4215), "Ventas.Ver" },
                    { 2, true, "Crear ventas", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4217), "Ventas.Crear" },
                    { 3, true, "Anular ventas", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4218), "Ventas.Anular" },
                    { 4, true, "Ver compras", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4219), "Compras.Ver" },
                    { 5, true, "Crear compras", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4221), "Compras.Crear" },
                    { 6, true, "Ver productos", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4222), "Productos.Ver" },
                    { 7, true, "Crear productos", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4223), "Productos.Crear" },
                    { 8, true, "Editar productos", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4224), "Productos.Editar" },
                    { 9, true, "Ver clientes", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4225), "Clientes.Ver" },
                    { 10, true, "Crear clientes", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4226), "Clientes.Crear" },
                    { 11, true, "Ver reportes", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4228), "Reportes.Ver" },
                    { 12, true, "Abrir caja", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4229), "Caja.Abrir" },
                    { 13, true, "Cerrar caja", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4230), "Caja.Cerrar" },
                    { 14, true, "Ver configuración", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4232), "Configuracion.Ver" },
                    { 15, true, "Gestionar usuarios", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4233), "Usuarios.Gestionar" }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Acceso total al sistema", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(3964), "Gerente" },
                    { 2, true, "Administración general", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(3976), "Administrador" },
                    { 3, true, "Operaciones de venta", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(3977), "Vendedor" }
                });

            migrationBuilder.InsertData(
                table: "TipoDocumento",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Documento Nacional de Identidad", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4178), "DNI" },
                    { 2, true, "Clave Única de Identificación Tributaria", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4179), "CUIT" },
                    { 3, true, "Pasaporte", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4181), "Pasaporte" }
                });

            migrationBuilder.InsertData(
                table: "TipoMovimientoStock",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Ingreso de mercadería", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4139), "Entrada" },
                    { 2, true, "Egreso de mercadería", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4140), "Salida" },
                    { 3, true, "Ajuste positivo de stock", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4141), "Ajuste Positivo" },
                    { 4, true, "Ajuste negativo de stock", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4142), "Ajuste Negativo" }
                });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "Id", "Activo", "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4291), 1, 1 },
                    { 2, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4293), 2, 1 },
                    { 3, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4293), 3, 1 },
                    { 4, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4294), 4, 1 },
                    { 5, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4295), 5, 1 },
                    { 6, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4295), 6, 1 },
                    { 7, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4296), 7, 1 },
                    { 8, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4297), 8, 1 },
                    { 9, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4297), 9, 1 },
                    { 10, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4298), 10, 1 },
                    { 11, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4298), 11, 1 },
                    { 12, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4299), 12, 1 },
                    { 13, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4300), 13, 1 },
                    { 14, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4300), 14, 1 },
                    { 15, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4301), 15, 1 },
                    { 16, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4342), 1, 2 },
                    { 17, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4344), 2, 2 },
                    { 18, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4344), 3, 2 },
                    { 19, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4345), 4, 2 },
                    { 20, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4346), 5, 2 },
                    { 21, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4346), 6, 2 },
                    { 22, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4347), 7, 2 },
                    { 23, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4348), 8, 2 },
                    { 24, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4348), 9, 2 },
                    { 25, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4349), 10, 2 },
                    { 26, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4350), 11, 2 },
                    { 27, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4350), 12, 2 },
                    { 28, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4351), 13, 2 },
                    { 29, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4351), 14, 2 },
                    { 30, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4379), 1, 3 },
                    { 31, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4380), 2, 3 },
                    { 32, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4382), 6, 3 },
                    { 33, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4383), 9, 3 },
                    { 34, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4384), 10, 3 },
                    { 35, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4385), 12, 3 },
                    { 36, true, new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4386), 13, 3 }
                });

            migrationBuilder.InsertData(
                table: "Sucursal",
                columns: new[] { "Id", "Activo", "Direccion", "FechaAlta", "Id_empresa", "Nombre", "Telefono" },
                values: new object[] { 1, true, "Dirección Principal 123", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4459), 1, "Casa Central", "3794000000" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Activo", "Apellido", "Email", "FechaAlta", "Id_rol", "Id_sucursal", "Nombre", "PasswordHash", "UltimoAcceso" },
                values: new object[] { 1, true, "Sistema", "admin@sistema.com", new DateTime(2026, 3, 5, 0, 44, 31, 461, DateTimeKind.Local).AddTicks(4487), 1, 1, "Admin", "$2a$12$xTXms.c66F43LdABY4LZjeXhAJmZSwEeYircoRBHDnBy4cqkklYFu", null });

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
                name: "IX_Cliente_Id_empresa",
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
                unique: true,
                filter: "[CodigoBarra] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Id_categoria",
                table: "Producto",
                column: "Id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Id_empresa",
                table: "Producto",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Id_unidadMedida",
                table: "Producto",
                column: "Id_unidadMedida");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraDetalle");

            migrationBuilder.DropTable(
                name: "MovimientoCaja");

            migrationBuilder.DropTable(
                name: "MovimientoStock");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "TipoDocumento");

            migrationBuilder.DropTable(
                name: "TipoMovimientoStock");

            migrationBuilder.DropTable(
                name: "VentaDetalle");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "MetodoPago");

            migrationBuilder.DropTable(
                name: "Permiso");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Venta");

            migrationBuilder.DropTable(
                name: "Proveedor");

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
