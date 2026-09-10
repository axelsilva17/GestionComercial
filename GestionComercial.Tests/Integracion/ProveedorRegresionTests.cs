using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Proveedores;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — Proveedores: the list must expose a REAL count of purchases
    /// (TotalCompras). Original bug: ProveedorListadoViewModel mapped TotalCompras to a
    /// TODO placeholder (0) and the repository query had no Include(Compras), so every
    /// proveedor showed 0 purchases regardless of real data.
    /// Fix: repository loads .Include(p => p.Compras) (small POS page) and the VM maps
    /// proveedor.CantidadCompras (Compras.Count).
    /// </summary>
    public class ProveedorRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public ProveedorRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerTodosAsync_ProveedorConCompras_CalculaTotalComprasReal()
        {
            // Arrange: proveedor propio + 2 compras
            int empresaId, proveedorId;

            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                empresaId = escenario.EmpresaId;

                var proveedor = Proveedor.Crear(
                    "Proveedor Compras Regresión",
                    escenario.EmpresaId,
                    cuit: "20-12345678-1");
                setup.Proveedores.Add(proveedor);
                await setup.SaveChangesAsync();
                proveedorId = proveedor.Id;

                setup.Compras.AddRange(
                    Compra.Crear(proveedorId, escenario.SucursalId, escenario.UsuarioId, "Compra 1"),
                    Compra.Crear(proveedorId, escenario.SucursalId, escenario.UsuarioId, "Compra 2"));
                await setup.SaveChangesAsync();
            }

            // Act: el servicio listado usado por la UI de Proveedores
            using var uow = _db.CreateUnitOfWork();
            var servicio = new ProveedorServicio(uow);
            var proveedores = await servicio.ObtenerTodosAsync(empresaId);

            // Assert: la carga eager de Compras alimenta CantidadCompras de forma real
            var resultado = proveedores.Should().ContainSingle(p => p.Id == proveedorId).Subject;
            resultado.CantidadCompras.Should().Be(2);
        }
    }
}