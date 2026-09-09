using FluentAssertions;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regresión — Costos por proveedor: crear y consultar un ProveedorProductoCosto debe
    /// funcionar mapeando las FKs a las columnas físicas IdProveedor/IdProducto.
    /// Bug original: el nav Producto estaba tipado como self-reference, EF creó columnas
    /// shadow ProveedorId/ProductoId y toda query/insert fallaba con "no such column" contra
    /// el schema físico (migración 20260908000000) que solo tiene IdProveedor/IdProducto.
    /// </summary>
    public class ProveedorProductoCostoRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;
        private static int _contador;

        public ProveedorProductoCostoRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task CrearYConsultarCostoProveedor_MapeaIdProveedorEIdProducto()
        {
            // Arrange: proveedor y producto propios, aislados de las seeds HasData
            int n = Interlocked.Increment(ref _contador);
            int proveedorId, productoId;

            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);

                var proveedor = Proveedor.Crear(
                    $"Proveedor Regresión {n}",
                    escenario.EmpresaId,
                    cuit: $"20-{n:D8}-3");
                setup.Proveedores.Add(proveedor);

                var producto = Producto.Crear(
                    $"Producto Regresión {n}",
                    precioVenta: 100m,
                    precioCosto: 60m,
                    idEmpresa: escenario.EmpresaId,
                    idCategoria: escenario.CategoriaId,
                    idUnidadMedida: escenario.UnidadMedidaId,
                    codigoBarra: $"REG-PPC-{n:D6}");
                setup.Productos.Add(producto);

                await setup.SaveChangesAsync();
                proveedorId = proveedor.Id;
                productoId = producto.Id;
            }

            // El schema del fixture se crea con EnsureCreated() (mismo modelo), por lo que
            // nunca reproduciría el bug. Se recrea la tabla con la forma física real definida
            // por la migración 20260908000000: solo IdProveedor/IdProducto, sin columnas shadow.
            // Con el modelo roto, EF generaría SELECT/INSERT con "ProveedorId"/"ProductoId" y
            // fallaría con "no such column" — exactamente el fallo de producción.
            using (var schema = _db.CreateContext())
            {
                schema.Database.ExecuteSqlRaw("""
                    DROP TABLE "ProveedorProductoCostos";
                    CREATE TABLE "ProveedorProductoCostos" (
                        "Id" INTEGER NOT NULL CONSTRAINT "PK_ProveedorProductoCostos" PRIMARY KEY AUTOINCREMENT,
                        "IdProveedor" INTEGER NOT NULL,
                        "IdProducto" INTEGER NOT NULL,
                        "Costo" TEXT NOT NULL,
                        "FechaAlta" TEXT NOT NULL,
                        "Activo" INTEGER NOT NULL,
                        CONSTRAINT "FK_ProveedorProductoCostos_Proveedor_IdProveedor" FOREIGN KEY ("IdProveedor") REFERENCES "Proveedor" ("Id") ON DELETE CASCADE,
                        CONSTRAINT "FK_ProveedorProductoCostos_Producto_IdProducto" FOREIGN KEY ("IdProducto") REFERENCES "Producto" ("Id") ON DELETE CASCADE
                    );
                    CREATE UNIQUE INDEX "IX_ProveedorProductoCostos_ProveedorProducto" ON "ProveedorProductoCostos" ("IdProveedor", "IdProducto");
                    """);
            }

            // El modelo EF debe usar IdProveedor/IdProducto como FKs y NO exponer columnas shadow
            using (var modelo = _db.CreateContext())
            {
                var tipo = modelo.Model.FindEntityType(typeof(ProveedorProductoCosto))!;
                tipo.GetProperties().Select(p => p.Name).Should().NotContain(new[] { "ProveedorId", "ProductoId" });
                tipo.GetProperties().Select(p => p.Name).Should().Contain(new[] { "IdProveedor", "IdProducto" });
                tipo.GetForeignKeys().SelectMany(fk => fk.Properties.Select(p => p.Name))
                    .Should().BeEquivalentTo(new[] { "IdProveedor", "IdProducto" });
            }

            // Act: crear el costo a través del repo en el unit of work
            using (var uow = _db.CreateUnitOfWork())
            {
                var costo = ProveedorProductoCosto.Crear(proveedorId, productoId, costo: 58.50m);
                await uow.ProveedoresCostos.AgregarAsync(costo);
                await uow.GuardarCambiosAsync();

                // Query por proveedor: debe traer el registro sin errores de columna
                var porProveedor = await uow.ProveedoresCostos.ObtenerPorProveedorAsync(proveedorId);
                var unico = porProveedor.Should().ContainSingle().Subject;
                unico.IdProveedor.Should().Be(proveedorId);
                unico.IdProducto.Should().Be(productoId);
                unico.Costo.Should().Be(58.50m);

                // Query por proveedor + producto
                var porPar = await uow.ProveedoresCostos.ObtenerPorProveedorYProductoAsync(proveedorId, productoId);
                porPar.Should().NotBeNull();
                porPar!.IdProveedor.Should().Be(proveedorId);
                porPar.IdProducto.Should().Be(productoId);

                // Flujo real de la capa de aplicación (Alta de Compra): ajuste de costos
                var servicio = new GestionComercial.Aplicacion.Servicios.ProveedorCostoService(uow);
                var resultado = await servicio.AjusteCostoProveedorAsync(proveedorId, 10m);
                resultado.Nuevos.Should().Be(0);
                resultado.Actualizados.Should().Be(1);
            }

            // Assert: persistencia real con los valores mapeados en las columnas físicas
            using var verif = _db.CreateContext();
            var costoAjustado = await verif.ProveedorProductoCostos
                .AsNoTracking()
                .SingleAsync(c => c.IdProveedor == proveedorId && c.IdProducto == productoId);
            costoAjustado.Costo.Should().Be(64.35m); // 58.50 * 1.10

            // Linkage de navegaciones a las entidades reales (Proveedor y Producto)
            var conNavegaciones = await verif.ProveedorProductoCostos
                .AsNoTracking()
                .Include(c => c.Proveedor)
                .Include(c => c.Producto)
                .SingleAsync(c => c.IdProveedor == proveedorId && c.IdProducto == productoId);
            conNavegaciones.Proveedor.Id.Should().Be(proveedorId);
            conNavegaciones.Producto.Nombre.Should().Be($"Producto Regresión {n}");
        }
    }
}