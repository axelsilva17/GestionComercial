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
    /// Regression: ObtenerRecientesPorSucursalAsync must respect the optional estado
    /// parameter so that the pending-sales list (IrVentasPendientes / FiltroEstado=Pendiente)
    /// is no longer always empty. Default (null) must include all estados.
    /// </summary>
    public class VentaRecientesEstadoRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public VentaRecientesEstadoRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ObtenerRecientesPorSucursalAsync_EstadoNull_IncluyePendientes()
        {
            // Arrange: seed 3 ventas — 1 Pendiente, 1 Pagada, 1 Anulada
            int sucursalId, usuarioId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var cliente = Cliente.Crear("Cliente Recientes", 20000001, escenario.EmpresaId);
                ctx.Clientes.Add(cliente);
                await ctx.SaveChangesAsync();

                var producto = Producto.Crear("Prod Recientes", 100m, 60m, escenario.EmpresaId,
                    escenario.CategoriaId, escenario.UnidadMedidaId, codigoBarra: "REC-001");
                producto.StockActual = 10;
                ctx.Productos.Add(producto);
                await ctx.SaveChangesAsync();

                // Pendiente
                var v1 = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                v1.Estado = (int)EstadoVentaEnum.Pendiente;
                v1.Fecha = DateTime.Now.AddHours(-3);
                ctx.Ventas.Add(v1);
                await ctx.SaveChangesAsync();

                // Pagada
                var v2 = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                v2.Fecha = DateTime.Now.AddHours(-2);
                ctx.Ventas.Add(v2);
                await ctx.SaveChangesAsync();
                v2.MarcarPagada();
                ctx.Ventas.Update(v2);

                // Anulada
                var v3 = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                v3.Fecha = DateTime.Now.AddHours(-1);
                ctx.Ventas.Add(v3);
                await ctx.SaveChangesAsync();
                v3.Anular("Test anulación", usuarioId);
                ctx.Ventas.Update(v3);

                await ctx.SaveChangesAsync();
            }

            // Act + Assert
            using var uow = _db.CreateUnitOfWork();
            var repo = new VentaRepositorio(_db.CreateContext());

            var desde = DateTime.Today.AddDays(-1);
            var hasta = DateTime.Now.AddMinutes(1);

            // Default (null) → all 3 estados
            var all = await repo.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, top: 100);
            all.Should().HaveCount(3, "default (null) should return all estados");

            // estado=Pendiente → 1
            var pendientes = await repo.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, top: 100, estado: (int)EstadoVentaEnum.Pendiente);
            pendientes.Should().HaveCount(1);
            pendientes.First().Estado.Should().Be((int)EstadoVentaEnum.Pendiente);

            // estado=Pagada → 1
            var pagadas = await repo.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, top: 100, estado: (int)EstadoVentaEnum.Pagada);
            pagadas.Should().HaveCount(1);
            pagadas.First().Estado.Should().Be((int)EstadoVentaEnum.Pagada);

            // estado=Anulada → 1
            var anuladas = await repo.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, top: 100, estado: (int)EstadoVentaEnum.Anulada);
            anuladas.Should().HaveCount(1);
            anuladas.First().Estado.Should().Be((int)EstadoVentaEnum.Anulada);
        }

        [Fact]
        public async Task ObtenerRecientesPorSucursalAsync_ServiceLayer_PasaEstadoCorrectamente()
        {
            // Arrange: service-level test using real UoW + repo
            int empresaId, sucursalId, usuarioId;
            await using (var ctx = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
                empresaId = escenario.EmpresaId;
                sucursalId = escenario.SucursalId;
                usuarioId = escenario.UsuarioId;

                var cliente = Cliente.Crear("Cliente Svc", 30000001, empresaId);
                ctx.Clientes.Add(cliente);
                await ctx.SaveChangesAsync();

                // 2 Pendientes, 1 Pagada
                for (int i = 0; i < 2; i++)
                {
                    var v = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                    v.Estado = (int)EstadoVentaEnum.Pendiente;
                    v.Fecha = DateTime.Now.AddHours(-10 + i);
                    ctx.Ventas.Add(v);
                }
                await ctx.SaveChangesAsync();

                var vp = Venta.Crear(sucursalId, cliente.Id, usuarioId);
                vp.Fecha = DateTime.Now.AddHours(-5);
                ctx.Ventas.Add(vp);
                await ctx.SaveChangesAsync();
                vp.MarcarPagada();
                ctx.Ventas.Update(vp);
                await ctx.SaveChangesAsync();
            }

            using var uow = _db.CreateUnitOfWork();
            var servicio = new VentaServicio(uow, new Mock<IServicioImpresion>().Object,
                new SesionServicio(), new Mock<IInventarioServicio>().Object,
                new Mock<IDescuentoConfiguracionServicio>().Object);

            var desde = DateTime.Today.AddDays(-1);
            var hasta = DateTime.Now.AddMinutes(1);

            // Default → 3
            var all = await servicio.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, 100);
            all.Count().Should().Be(3, "service default should return all");

            // estado=Pendiente → 2
            var pendientes = await servicio.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, 100, estado: (int)EstadoVentaEnum.Pendiente);
            pendientes.Count().Should().Be(2);

            // estado=Pagada → 1
            var pagadas = await servicio.ObtenerRecientesPorSucursalAsync(sucursalId, desde, hasta, 100, estado: (int)EstadoVentaEnum.Pagada);
            pagadas.Count().Should().Be(1);
            pagadas.First().Estado.Should().Be("Pagada");
        }
    }
}
