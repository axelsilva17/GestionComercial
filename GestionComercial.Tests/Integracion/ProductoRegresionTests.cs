using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regresión E2E — Productos: el ajuste de precios por proveedor no debe duplicar
    /// Categoría/UnidadMedida. Bug original: "The instance of entity type 'Categoria' cannot
    /// be tracked because another instance with the same key value for {'Id'} is already
    /// being tracked" (dos productos comparten categoría).
    /// </summary>
    public class ProductoRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public ProductoRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task AjustePreciosPorProveedorAsync_ConProductosMismaCategoria_NoDuplicaCategoriaYActualizaPrecios()
        {
            // Arrange: 3 productos propios — 2 comparten categoría (reproduce el bug)
            int empresaId, categoriaAId, unidadId, proveedorId;
            int[] productoIds;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;
                categoriaAId = escenario.CategoriaId;
                unidadId = escenario.UnidadMedidaId;

                var categoriaB = Categoria.Crear("Categoría B Regresión", empresaId);
                ctx.Categorias.Add(categoriaB);
                await ctx.SaveChangesAsync();

                var proveedor = Proveedor.Crear("Proveedor Regresión", empresaId);
                ctx.Proveedores.Add(proveedor);

                var p1 = Producto.Crear("Reg A1", 100m, 60m, empresaId, categoriaAId, unidadId, codigoBarra: "REG-P-001");
                p1.StockActual = 10;
                var p2 = Producto.Crear("Reg A2", 200m, 120m, empresaId, categoriaAId, unidadId, codigoBarra: "REG-P-002");
                p2.StockActual = 5;
                var p3 = Producto.Crear("Reg B1", 300m, 200m, empresaId, categoriaB.Id, unidadId, codigoBarra: "REG-P-003");
                p3.StockActual = 7;
                ctx.Productos.AddRange(p1, p2, p3);

                await ctx.SaveChangesAsync();
                proveedorId = proveedor.Id;
                productoIds = new[] { p1.Id, p2.Id, p3.Id };
            }

            // Act: +10% sobre venta y costo
            using var uow = _db.CreateUnitOfWork();
            var servicio = new ProductoServicio(uow);
            var (nuevos, actualizados) = await servicio.AjustePreciosPorProveedorAsync(proveedorId, 10m);

            // Assert
            actualizados.Should().Be(3);
            nuevos.Should().Be(3);

            using var ctxVerif = _db.CreateContext();
            var productos = ctxVerif.Productos.Where(p => productoIds.Contains(p.Id)).ToDictionary(p => p.Id);
            productos[productoIds[0]].PrecioVentaActual.Should().Be(110m);
            productos[productoIds[0]].PrecioCostoActual.Should().Be(66m);
            productos[productoIds[1]].PrecioVentaActual.Should().Be(220m);
            productos[productoIds[1]].PrecioCostoActual.Should().Be(132m);
            productos[productoIds[2]].PrecioVentaActual.Should().Be(330m);
            productos[productoIds[2]].PrecioCostoActual.Should().Be(220m);

            // Sin duplicados ni borrados colaterales
            ctxVerif.Categorias.Count(c => c.Id_empresa == empresaId).Should().Be(2);
            ctxVerif.Productos.Count(p => p.Id_empresa == empresaId).Should().Be(3);
        }
    }
}