using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression: Ajuste sign semantics — user picks Ajuste Positivo / Negativo
    /// and enters a delta magnitude (always > 0). The service computes
    /// delta = +cantidad or delta = -cantidad accordingly.
    /// </summary>
    public class InventarioAjusteRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public InventarioAjusteRegresionTests(SqliteTestDatabase db) => _db = db;

        // ── Entity-level tests (MovimientoStock.Ajuste) ────────────────

        [Fact]
        public void MovimientoStock_Ajuste_DeltaCero_LanzaArgumentException()
        {
            // Act + Assert: delta == 0 should throw ArgumentException
            var act = () => MovimientoStock.Ajuste(
                delta: 0,
                stockAnterior: 10,
                idProducto: 1,
                idSucursal: 1,
                idUsuario: 1);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*ajuste no puede ser cero*");
        }

        [Fact]
        public void MovimientoStock_Ajuste_DeltaPositivo_CalculaStockCorrecto()
        {
            // Arrange + Act
            var mov = MovimientoStock.Ajuste(
                delta: 5,
                stockAnterior: 10,
                idProducto: 1,
                idSucursal: 1,
                idUsuario: 1);

            // Assert
            mov.EsAjustePositivo.Should().BeTrue();
            mov.StockNuevo.Should().Be(15);
            mov.Cantidad.Should().Be(5); // magnitude
        }

        [Fact]
        public void MovimientoStock_Ajuste_DeltaNegativo_CalculaStockCorrecto()
        {
            // Arrange + Act
            var mov = MovimientoStock.Ajuste(
                delta: -3,
                stockAnterior: 10,
                idProducto: 1,
                idSucursal: 1,
                idUsuario: 1);

            // Assert
            mov.EsAjusteNegativo.Should().BeTrue();
            mov.StockNuevo.Should().Be(7);
            mov.Cantidad.Should().Be(3); // absolute magnitude
        }

        [Fact]
        public void MovimientoStock_AjusteNegativo_StockInsuficiente_LanzaInvalidOperationException()
        {
            // delta = -20 on stock 10 → insufficient
            var act = () => MovimientoStock.Ajuste(
                delta: -20,
                stockAnterior: 10,
                idProducto: 1,
                idSucursal: 1,
                idUsuario: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Stock insuficiente*");
        }

        // ── Service-level tests (InventarioServicio) ───────────────────

        [Fact]
        public async Task InventarioServicio_AjustePositivo_AgregaStock()
        {
            // Arrange: seed a product with stock 10
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear("Ajuste Pos Test", 100m, 60m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "AJT-POS-001");
                producto.StockActual = 10;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: Ajuste Positivo +5 → stock should become 15
            using var uow = _db.CreateUnitOfWork();
            var servicio = new InventarioServicio(uow);

            await servicio.RegistrarMovimientoAsync(
                idProducto: productoId,
                tipoMovimiento: "Ajuste",
                cantidad: 5,
                observacion: "Test positivo",
                idSucursal: sucursalId,
                idUsuario: usuarioId,
                guardarCambios: true,
                esAjustePositivo: true);

            // Assert: verify stock updated
            await using var ctxAssert = _db.CreateContext();
            var prod = ctxAssert.Productos.First(p => p.Id == productoId);
            prod.StockActual.Should().Be(15);
        }

        [Fact]
        public async Task InventarioServicio_AjusteNegativo_RestaStock()
        {
            // Arrange: seed a product with stock 10
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear("Ajuste Neg Test", 100m, 60m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "AJT-NEG-001");
                producto.StockActual = 10;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: Ajuste Negativo -3 → stock should become 7
            using var uow = _db.CreateUnitOfWork();
            var servicio = new InventarioServicio(uow);

            await servicio.RegistrarMovimientoAsync(
                idProducto: productoId,
                tipoMovimiento: "Ajuste",
                cantidad: 3,
                observacion: "Test negativo",
                idSucursal: sucursalId,
                idUsuario: usuarioId,
                guardarCambios: true,
                esAjustePositivo: false);

            // Assert: verify stock updated
            await using var ctxAssert = _db.CreateContext();
            var prod = ctxAssert.Productos.First(p => p.Id == productoId);
            prod.StockActual.Should().Be(7);
        }

        [Fact]
        public async Task InventarioServicio_AjusteNegativo_StockInsuficiente_LanzaExcepcion()
        {
            // Arrange: seed a product with stock 5
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear("Ajuste Insuf Test", 100m, 60m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "AJT-INS-001");
                producto.StockActual = 5;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: Ajuste Negativo -10 on stock 5 → should fail
            using var uow = _db.CreateUnitOfWork();
            var servicio = new InventarioServicio(uow);

            var act = () => servicio.RegistrarMovimientoAsync(
                idProducto: productoId,
                tipoMovimiento: "Ajuste",
                cantidad: 10,
                observacion: "Test insuficiente",
                idSucursal: sucursalId,
                idUsuario: usuarioId,
                guardarCambios: true,
                esAjustePositivo: false);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Stock insuficiente*");
        }

        [Fact]
        public async Task InventarioServicio_Ajuste_CantidadCero_LanzaArgumentException()
        {
            // Arrange: seed a product
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear("Ajuste Zero Test", 100m, 60m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "AJT-ZERO-001");
                producto.StockActual = 10;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: cantidad 0 → should fail (service validates cantidad <= 0)
            using var uow = _db.CreateUnitOfWork();
            var servicio = new InventarioServicio(uow);

            var act = () => servicio.RegistrarMovimientoAsync(
                idProducto: productoId,
                tipoMovimiento: "Ajuste",
                cantidad: 0,
                observacion: "Test cero",
                idSucursal: sucursalId,
                idUsuario: usuarioId,
                guardarCambios: true,
                esAjustePositivo: true);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*cantidad*");
        }

        [Fact]
        public async Task InventarioServicio_AjusteNegativo_CantidadMayorQueStock_LanzaExcepcion()
        {
            // Arrange: seed a product with stock 3
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear("Ajuste MayorStock Test", 100m, 60m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "AJT-MAY-001");
                producto.StockActual = 3;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: Ajuste Negativo -5 on stock 3 → should fail
            using var uow = _db.CreateUnitOfWork();
            var servicio = new InventarioServicio(uow);

            var act = () => servicio.RegistrarMovimientoAsync(
                idProducto: productoId,
                tipoMovimiento: "Ajuste",
                cantidad: 5,
                observacion: "Test mayor que stock",
                idSucursal: sucursalId,
                idUsuario: usuarioId,
                guardarCambios: true,
                esAjustePositivo: false);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Stock insuficiente*");
        }
    }
}
