using FluentAssertions;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Persistencia.Repositorio;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression: BuscarProductosAsync (CONTAINS) must find products by infix substring
    /// and be case-insensitive. Also tests that BuscarProductosContieneAsync delegates
    /// to the same implementation (deduplication).
    /// </summary>
    public class ProductoBusquedaRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public ProductoBusquedaRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task BuscarProductosAsync_ConSubcadenaEncuentraProducto()
        {
            // Arrange: seed a product "Aceite Girasol"
            int empresaId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;

                var producto = Producto.Crear("Aceite Girasol 900ml", 500m, 300m,
                    empresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "BUSQ-001");
                producto.StockActual = 20;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();
            }

            // Act + Assert
            using var ctx2 = _db.CreateContext();
            var repo = new ProductoRepositorio(ctx2);

            // Search for infix "Girasol" — should match "Aceite Girasol 900ml"
            var resultados = await repo.BuscarProductosAsync(empresaId, "Girasol", null, true, 10);
            resultados.Should().HaveCount(1);
            resultados.First().Nombre.Should().Contain("Girasol");

            // Case-insensitive: "girasol" (lowercase) must also match
            var resultadosLower = await repo.BuscarProductosAsync(empresaId, "girasol", null, true, 10);
            resultadosLower.Should().HaveCount(1);

            // Case-insensitive: "GIRASOL" (uppercase) must also match
            var resultadosUpper = await repo.BuscarProductosAsync(empresaId, "GIRASOL", null, true, 10);
            resultadosUpper.Should().HaveCount(1);

            // Non-matching term
            var resultadosNoMatch = await repo.BuscarProductosAsync(empresaId, "Leche", null, true, 10);
            resultadosNoMatch.Should().BeEmpty();
        }

        [Fact]
        public async Task BuscarProductosContieneAsync_DelegaAMismaImplementacion()
        {
            // Arrange
            int empresaId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;

                var p1 = Producto.Crear("Girasol 900ml", 500m, 300m,
                    empresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "DELEG-001");
                p1.StockActual = 10;
                var p2 = Producto.Crear("Girasol 1500ml", 800m, 500m,
                    empresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "DELEG-002");
                p2.StockActual = 5;
                ctx.Productos.AddRange(p1, p2);
                await ctx.SaveChangesAsync();
            }

            // Act: both methods should return the same results for the same query
            using var ctx2 = _db.CreateContext();
            var repo = new ProductoRepositorio(ctx2);

            var viaBuscar = await repo.BuscarProductosAsync(empresaId, "Girasol", null, true, 10);
            var viaContiene = await repo.BuscarProductosContieneAsync(empresaId, "Girasol", null, true, 10);

            viaBuscar.Should().HaveCount(2);
            viaContiene.Should().HaveCount(2);
            viaBuscar.Select(p => p.Id).OrderBy(x => x).Should()
                .Equal(viaContiene.Select(p => p.Id).OrderBy(x => x));
        }

        [Fact]
        public async Task BuscarProductosAsync_ProductoSinCoincidencia_RetornaListaVacia()
        {
            // A matching term is case-insensitively found; a non-existent
            // product name returns an empty list.
            int empresaId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;

                var p = Producto.Crear("Leche Entera", 300m, 150m,
                    empresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "BUSQ-002");
                p.StockActual = 10;
                ctx.Productos.Add(p);
                await ctx.SaveChangesAsync();
            }

            using var ctx2 = _db.CreateContext();
            var repo = new ProductoRepositorio(ctx2);

            // "Leche" → matches
            var resultados = await repo.BuscarProductosAsync(empresaId, "Leche", null, true, 10);
            resultados.Should().HaveCount(1);

            // "Agua" → no match
            var noMatch = await repo.BuscarProductosAsync(empresaId, "Agua", null, true, 10);
            noMatch.Should().BeEmpty();
        }
    }
}
