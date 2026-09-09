using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regresión E2E — Inventario: el movimiento manual de stock (modo standalone,
    /// guardarCambios=true) debe persistir sin re-insertar Sucursal/Usuario.
    /// Bug original: "UNIQUE constraint failed: Sucursal.Id".
    /// </summary>
    public class InventarioRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public InventarioRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task RegistrarMovimientoAsync_GuardarCambiosTrue_PersisteMovimientoSinDuplicarSucursalNiUsuario()
        {
            // Arrange: entidades propias en un contexto descartado (el servicio usa uno nuevo)
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear(
                    "Producto Regresión", 100m, 60m, escenario.EmpresaId,
                    escenario.CategoriaId, escenario.UnidadMedidaId, codigoBarra: "REG-MOV-001");
                producto.StockActual = 10;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: contexto nuevo sin tracking previo — reproduce el bug original
            using (var uow = _db.CreateUnitOfWork())
            {
                var servicio = new InventarioServicio(uow);
                await servicio.RegistrarMovimientoAsync(
                    idProducto: productoId,
                    tipoMovimiento: "Salida",
                    cantidad: 2,
                    observacion: "Regresión E2E",
                    idSucursal: sucursalId,
                    idUsuario: usuarioId,
                    guardarCambios: true);
            }

            // Assert
            using var ctxVerif = _db.CreateContext();
            ctxVerif.Sucursales.Count(s => s.Id == sucursalId).Should().Be(1);
            ctxVerif.Usuarios.Count(u => u.Id == usuarioId).Should().Be(1);

            var movimiento = ctxVerif.MovimientosStock.SingleOrDefault(m => m.Id_producto == productoId);
            movimiento.Should().NotBeNull();
            movimiento!.TipoMovimiento.Should().Be((int)TipoMovimientoStockEnum.Salida);
            movimiento.Cantidad.Should().Be(2);
            movimiento.Id_sucursal.Should().Be(sucursalId);
            movimiento.Id_usuario.Should().Be(usuarioId);
            movimiento.StockNuevo.Should().Be(8);

            ctxVerif.Productos.Single(p => p.Id == productoId).StockActual.Should().Be(8);
        }
    }
}