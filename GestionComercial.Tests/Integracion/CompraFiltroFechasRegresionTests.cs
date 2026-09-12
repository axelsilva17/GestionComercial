using FluentAssertions;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — CompraFiltroFechas: date filtering must work for ALL providers
    /// (null/"Todos") AND for a specific provider. The date range (Desde/Hasta) must
    /// always apply regardless of whether a provider is selected.
    ///
    /// Original bug: when a specific provider was selected, the date range was only
    /// applied when the user toggled an explicit "Filtrar por fecha" checkbox; with
    /// "Todos" the dates always applied but the checkbox was disabled so the user
    /// couldn't even enable it. The fix makes date filtering unconditional in the
    /// repository and removes the dead checkbox / state from the ViewModel.
    /// </summary>
    public class CompraFiltroFechasRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public CompraFiltroFechasRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerPorSucursalPaginadoAsync_Todos_Fecha_FiltraCorrectamente()
        {
            // Arrange: 2 proveedores, cada uno con 1 compra dentro y 1 fuera del rango
            int sucursalId, proveedorAId, proveedorBId;

            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);

                var proveedorA = Proveedor.Crear("Proveedor A FiltroFecha", escenario.EmpresaId, cuit: "20-11111111-1");
                var proveedorB = Proveedor.Crear("Proveedor B FiltroFecha", escenario.EmpresaId, cuit: "20-22222222-2");
                setup.Proveedores.AddRange(proveedorA, proveedorB);
                await setup.SaveChangesAsync();
                proveedorAId = proveedorA.Id;
                proveedorBId = proveedorB.Id;
                sucursalId = escenario.SucursalId;

                // Producto mínimo para poder crear detalles
                var unidad = setup.UnidadesMedida.First();
                var categoria = setup.Categorias.First(c => c.Id_empresa == escenario.EmpresaId);
                var producto = Producto.Crear("Prod FiltroFecha", 100m, 50m,
                    escenario.EmpresaId, categoria.Id, unidad.Id, codigoBarra: $"FF-{Guid.NewGuid():N}");
                setup.Productos.Add(producto);
                await setup.SaveChangesAsync();

                // Compras dentro del rango: 2026-01-15 y 2026-01-20
                var compraA_dentro = Compra.Crear(proveedorAId, sucursalId, escenario.UsuarioId, "A dentro");
                compraA_dentro.Fecha = new DateTime(2026, 1, 15, 10, 0, 0);
                compraA_dentro.AgregarDetalle(CompraDetalle.Crear(producto, 1, 50m));
                setup.Compras.Add(compraA_dentro);

                var compraB_dentro = Compra.Crear(proveedorBId, sucursalId, escenario.UsuarioId, "B dentro");
                compraB_dentro.Fecha = new DateTime(2026, 1, 20, 10, 0, 0);
                compraB_dentro.AgregarDetalle(CompraDetalle.Crear(producto, 2, 50m));
                setup.Compras.Add(compraB_dentro);

                // Compras fuera del rango: 2026-03-10 y 2026-03-15
                var compraA_fuera = Compra.Crear(proveedorAId, sucursalId, escenario.UsuarioId, "A fuera");
                compraA_fuera.Fecha = new DateTime(2026, 3, 10, 10, 0, 0);
                compraA_fuera.AgregarDetalle(CompraDetalle.Crear(producto, 1, 50m));
                setup.Compras.Add(compraA_fuera);

                var compraB_fuera = Compra.Crear(proveedorBId, sucursalId, escenario.UsuarioId, "B fuera");
                compraB_fuera.Fecha = new DateTime(2026, 3, 15, 10, 0, 0);
                compraB_fuera.AgregarDetalle(CompraDetalle.Crear(producto, 1, 50m));
                setup.Compras.Add(compraB_fuera);

                await setup.SaveChangesAsync();
            }

            // Act — test at repository level via UnitOfWork
            using var uow = _db.CreateUnitOfWork();
            var desde = new DateTime(2026, 1, 1);
            var hasta = new DateTime(2026, 1, 31, 23, 59, 59);

            // 1) "Todos" (null provider): must return only the 2 inside-window compras
            var (itemsTodos, totalTodos) = await uow.Compras.ObtenerPorSucursalPaginadoAsync(
                sucursalId, desde, hasta, 1, 100, idProveedor: null);

            totalTodos.Should().Be(2,
                "with 'Todos' only the 2 compras inside the Jan 2026 window should be returned");

            // 2) Provider A + same window: must return ONLY A's inside-window compra
            var (itemsA, totalA) = await uow.Compras.ObtenerPorSucursalPaginadoAsync(
                sucursalId, desde, hasta, 1, 100, idProveedor: proveedorAId);

            totalA.Should().Be(1,
                "with provider A only A's inside-window compra should be returned (not A's full history)");

            itemsA.Single().Id_proveedor.Should().Be(proveedorAId);
        }

        [Fact]
        public async Task ObtenerPorSucursalPaginadoAsync_ProveedorEspecifico_Fecha_FiltraCorrectamente()
        {
            // Arrange: single provider with 1 compra inside and 1 outside the window
            int sucursalId, proveedorId;

            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);

                var proveedor = Proveedor.Crear("Proveedor Único FiltroFecha", escenario.EmpresaId, cuit: "20-33333333-3");
                setup.Proveedores.Add(proveedor);
                await setup.SaveChangesAsync();
                proveedorId = proveedor.Id;
                sucursalId = escenario.SucursalId;

                var unidad = setup.UnidadesMedida.First();
                var categoria = setup.Categorias.First(c => c.Id_empresa == escenario.EmpresaId);
                var producto = Producto.Crear("Prod Único FiltroFecha", 100m, 50m,
                    escenario.EmpresaId, categoria.Id, unidad.Id, codigoBarra: $"UF-{Guid.NewGuid():N}");
                setup.Productos.Add(producto);
                await setup.SaveChangesAsync();

                // Inside: Feb 10
                var compraDentro = Compra.Crear(proveedorId, sucursalId, escenario.UsuarioId, "dentro");
                compraDentro.Fecha = new DateTime(2026, 2, 10, 10, 0, 0);
                compraDentro.AgregarDetalle(CompraDetalle.Crear(producto, 1, 50m));
                setup.Compras.Add(compraDentro);

                // Outside: Apr 5
                var compraFuera = Compra.Crear(proveedorId, sucursalId, escenario.UsuarioId, "fuera");
                compraFuera.Fecha = new DateTime(2026, 4, 5, 10, 0, 0);
                compraFuera.AgregarDetalle(CompraDetalle.Crear(producto, 1, 50m));
                setup.Compras.Add(compraFuera);

                await setup.SaveChangesAsync();
            }

            // Act
            using var uow = _db.CreateUnitOfWork();
            var desde = new DateTime(2026, 2, 1);
            var hasta = new DateTime(2026, 2, 28, 23, 59, 59);

            var (items, total) = await uow.Compras.ObtenerPorSucursalPaginadoAsync(
                sucursalId, desde, hasta, 1, 100, idProveedor: proveedorId);

            // Assert: only the inside-window compra
            total.Should().Be(1,
                "with specific provider + date window, only inside-window compras should be returned");
            items.Single().Fecha.Should().Be(new DateTime(2026, 2, 10, 10, 0, 0));
        }
    }
}
