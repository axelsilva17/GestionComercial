using FluentAssertions;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Persistencia.Repositorio;
using Moq;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression: ObtenerVentasPorVendedorAsync (vendedor dashboard) must return ONLY
    /// Pagada ventas — Pendiente and Anulada must be excluded. This pins the SQL filter
    /// v.Estado = 2 in VentaRepositorio.
    /// </summary>
    public class VentaVendedorPagadaRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public VentaVendedorPagadaRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerVentasPorVendedorAsync_RetornaSoloPagadas()
        {
            // Arrange: 1 Pagada + 1 Pendiente + 1 Anulada for same vendor
            int sucursalId, usuarioId, empresaId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var cliente = Cliente.Crear("Cliente Vendedor", 40000001, empresaId);
                ctx.Clientes.Add(cliente);
                await ctx.SaveChangesAsync();

                var producto = Producto.Crear("Prod Vendedor", 100m, 60m,
                    empresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "VEND-001");
                producto.StockActual = 50;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();

                // Pagada
                var vPagada = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                vPagada.Fecha = DateTime.Now.AddHours(-3);
                ctx.Ventas.Add(vPagada);
                await ctx.SaveChangesAsync();
                vPagada.MarcarPagada();
                ctx.Ventas.Update(vPagada);

                // Pendiente
                var vPendiente = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                vPendiente.Fecha = DateTime.Now.AddHours(-2);
                ctx.Ventas.Add(vPendiente);

                // Anulada
                var vAnulada = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                vAnulada.Fecha = DateTime.Now.AddHours(-1);
                ctx.Ventas.Add(vAnulada);
                await ctx.SaveChangesAsync();
                vAnulada.Anular("Test", usuarioId);
                ctx.Ventas.Update(vAnulada);

                await ctx.SaveChangesAsync();
            }

            // Act
            using var ctx2 = _db.CreateContext();
            var repo = new VentaRepositorio(ctx2);
            var desde = DateTime.Today.AddDays(-1);
            var hasta = DateTime.Now.AddMinutes(1);

            var ventas = await repo.ObtenerVentasPorVendedorAsync(sucursalId, usuarioId, desde, hasta);

            // Assert: only Pagada
            ventas.Should().HaveCount(1);
            ventas.First().Estado.Should().Be((int)EstadoVentaEnum.Pagada);
        }

        [Fact]
        public async Task ObtenerVentasPorVendedorAsync_ServiceLayer_RetornaSoloPagadas()
        {
            // Arrange
            int empresaId, sucursalId, usuarioId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var cliente = Cliente.Crear("Cliente Svc Vend", 50000001, empresaId);
                ctx.Clientes.Add(cliente);
                await ctx.SaveChangesAsync();

                // 1 Pagada, 1 Pendiente
                var vp = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                vp.Fecha = DateTime.Now.AddHours(-5);
                ctx.Ventas.Add(vp);
                await ctx.SaveChangesAsync();
                vp.MarcarPagada();
                ctx.Ventas.Update(vp);

                var vpen = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                vpen.Fecha = DateTime.Now.AddHours(-3);
                ctx.Ventas.Add(vpen);

                await ctx.SaveChangesAsync();
            }

            using var uow = _db.CreateUnitOfWork();
            var servicio = new VentaServicio(uow,
                new Mock<IServicioImpresion>().Object,
                new SesionServicio(),
                new Mock<IInventarioServicio>().Object,
                new Mock<IDescuentoConfiguracionServicio>().Object);

            var desde = DateTime.Today.AddDays(-1);
            var hasta = DateTime.Now.AddMinutes(1);

            var ventas = await servicio.ObtenerVentasPorVendedorAsync(sucursalId, usuarioId, desde, hasta);

            // Assert: only Pagada
            ventas.Should().HaveCount(1);
            ventas.First().Estado.Should().Be("Pagada");
        }
    }
}
