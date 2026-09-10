using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — Clientes: TotalVentas must count ONLY paid sales (semantics of
    /// Venta.EsPagada). Original bug: TotalVentas was always 0 because the old computed
    /// property (wrongly named CantidadCompras) relied on navigation loading that never
    /// happened, and the count included no state filter.
    /// Fix: ClienteRepositorio.ContarVentasPagadasPorClientesAsync groups by cliente with
    /// Estado == EstadoVentaEnum.Pagada (SQL, no N+1); property renamed CantidadVentas.
    /// </summary>
    public class ClienteRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public ClienteRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerTodosAsync_ClienteConVentasPagadasYNoPagadas_CuentaSoloPagadas()
        {
            // Arrange: cliente propio + 1 venta PAGADA + 1 venta PENDIENTE
            int empresaId, clienteId;

            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                empresaId = escenario.EmpresaId;

                var cliente = Cliente.Crear("Cliente Ventas Regresión", documento: 99887766, empresaId);
                setup.Clientes.Add(cliente);
                await setup.SaveChangesAsync();
                clienteId = cliente.Id;

                var pagada = Venta.Crear(escenario.SucursalId, clienteId, escenario.UsuarioId);
                pagada.Estado = (int)EstadoVentaEnum.Pagada;
                var pendiente = Venta.Crear(escenario.SucursalId, clienteId, escenario.UsuarioId); // Pendiente por defecto

                setup.Ventas.AddRange(pagada, pendiente);
                await setup.SaveChangesAsync();
            }

            // Act: el servicio listado usado por la UI de Clientes
            using var uow = _db.CreateUnitOfWork();
            var servicio = new ClienteServicio(uow);
            var clientes = await servicio.ObtenerTodosAsync(empresaId);

            // Assert: solo la venta pagada suma al TotalVentas
            var dto = clientes.Should().ContainSingle(c => c.IdCliente == clienteId).Subject;
            dto.TotalVentas.Should().Be(1);
        }
    }
}