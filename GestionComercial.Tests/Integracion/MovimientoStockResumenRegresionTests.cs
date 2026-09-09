using FluentAssertions;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Producto;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regresión E2E — Resumen de período: la agregación Sum sobre Cantidad (decimal) debe
    /// traducirse en SQLite. Bug original: EF Core no traduce Sum de decimal en SQLite
    /// ("cannot apply aggregate operator 'Sum' on expressions of type 'decimal'").
    /// </summary>
    public class MovimientoStockResumenRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public MovimientoStockResumenRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerResumenPeriodoAsync_ConMovimientos_AgregaUnidadesSinErrorDeTraduccion()
        {
            // Arrange: entidades propias + 3 movimientos (2 entradas, 1 salida)
            int empresaId, sucursalId, usuarioId, productoId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var producto = Producto.Crear(
                    "Producto Regresión", 100m, 60m, empresaId,
                    escenario.CategoriaId, escenario.UnidadMedidaId, codigoBarra: "REG-RES-001");
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
                productoId = producto.Id;
            }

            using (var uow = _db.CreateUnitOfWork())
            {
                await uow.MovimientosStock.AgregarAsync(MovimientoStock.Entrada(3, 0, productoId, sucursalId, usuarioId, "Entrada 1"));
                await uow.MovimientosStock.AgregarAsync(MovimientoStock.Entrada(5, 3, productoId, sucursalId, usuarioId, "Entrada 2"));
                await uow.MovimientosStock.AgregarAsync(MovimientoStock.Salida(2, 8, productoId, sucursalId, usuarioId, "Salida 1"));
                await uow.MovimientosStock.AgregarAsync(MovimientoStock.Ajuste(+4, 6, productoId, sucursalId, usuarioId, "Ajuste +"));
                await uow.MovimientosStock.AgregarAsync(MovimientoStock.Ajuste(-1, 10, productoId, sucursalId, usuarioId, "Ajuste -"));
                await uow.GuardarCambiosAsync();
            }

            // Act: contexto nuevo — consulta con agregación Sum en SQL
            using var uow2 = _db.CreateUnitOfWork();
            var resumen = await uow2.MovimientosStock.ObtenerResumenPeriodoAsync(
                DateTime.Now.AddDays(-1), DateTime.Now, empresaId);

            // Assert
            resumen.Should().NotBeNull();
            resumen!.TotalEntradas.Should().Be(2);
            resumen.UnidadesIngresadas.Should().Be(8);
            resumen.TotalSalidas.Should().Be(1);
            resumen.UnidadesEgresadas.Should().Be(2);
            resumen.TotalAjustes.Should().Be(2);
        }
    }
}