using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression: MovimientoStock.Ajuste with delta==0 must throw ArgumentException
    /// (not InvalidOperationException) with a descriptive message. The InventarioViewModel
    /// catch block must handle this via the ArgumentException path.
    /// </summary>
    public class InventarioAjusteRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public InventarioAjusteRegresionTests(SqliteTestDatabase db) => _db = db;

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
        public async Task InventarioServicio_RegistrarMovimiento_AjusteDeltaCero_LanzaArgumentException()
        {
            // Arrange: seed a product with stock 10
            int sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear("Ajuste Test", 100m, 60m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "AJT-001");
                producto.StockActual = 10;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            // Act: Ajuste with NuevaCantidad == stock actual → delta == 0
            using var uow = _db.CreateUnitOfWork();
            var servicio = new InventarioServicio(uow);

            var act = () => servicio.RegistrarMovimientoAsync(
                idProducto: productoId,
                tipoMovimiento: "Ajuste",
                cantidad: 10,          // same as current stock → delta 0
                observacion: "Test",
                idSucursal: sucursalId,
                idUsuario: usuarioId,
                guardarCambios: true);

            // Assert: should throw ArgumentException from MovimientoStock.Ajuste
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*ajuste no puede ser cero*");
        }
    }
}
