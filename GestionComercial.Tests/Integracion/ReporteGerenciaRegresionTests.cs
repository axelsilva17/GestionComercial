using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Enumeraciones;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — Gerencia report data fixes:
    /// 1. Rotation: CantidadComprada and UltimaCompra must reflect real purchase data.
    /// 2. Payment methods: Cantidad must reflect real payment count.
    /// 3. Monthly purchases: Per-month totals must be real, not averaged.
    /// </summary>
    public class ReporteGerenciaRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public ReporteGerenciaRegresionTests(SqliteTestDatabase db) => _db = db;

        // ── Test 1: Rotation returns real CantidadComprada + UltimaCompra ──────
        [Fact]
        public async Task RotacionProductosAsync_ConCompraExistente_DevuelveCantidadCompradaYUltimaCompra()
        {
            // Arrange: product with 1 sale + 1 purchase
            int empresaId;
            DateTime compraFecha;
            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                empresaId = escenario.EmpresaId;

                var producto = Producto.Crear(
                    "Producto Rotacion Test", 200m, 100m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "ROT-REG-001");
                producto.StockActual = 10;
                setup.Productos.Add(producto);
                await setup.SaveChangesAsync();

                // Create a client (Venta.Crear requires valid idCliente)
                var cliente = Cliente.Crear("Cliente Rotacion", documento: 10000001, escenario.EmpresaId);
                setup.Clientes.Add(cliente);
                await setup.SaveChangesAsync();

                // Sale (needed so the product appears in rotation query which starts from VentaDetalle)
                var venta = Venta.Crear(escenario.SucursalId, cliente.Id, escenario.UsuarioId);
                venta.Fecha = new DateTime(2025, 4, 10, 10, 0, 0, DateTimeKind.Local);
                setup.Ventas.Add(venta);
                await setup.SaveChangesAsync();
                var ventaDetalle = new VentaDetalle
                {
                    Id_venta = venta.Id,
                    Id_producto = producto.Id,
                    Cantidad = 3,
                    PrecioUnitario = 200m,
                    CostoUnitario = 100m,
                    Subtotal = 600m
                };
                setup.VentaDetalles.Add(ventaDetalle);
                await setup.SaveChangesAsync();

                // Purchase
                var proveedor = Proveedor.Crear("Proveedor Rotacion", escenario.EmpresaId);
                setup.Proveedores.Add(proveedor);
                await setup.SaveChangesAsync();

                compraFecha = new DateTime(2025, 3, 15, 10, 0, 0, DateTimeKind.Local);
                var compra = Compra.Crear(proveedor.Id, escenario.SucursalId, escenario.UsuarioId);
                compra.Fecha = compraFecha;
                setup.Compras.Add(compra);
                await setup.SaveChangesAsync();

                var detalle = CompraDetalle.Crear(producto, 5, 80m);
                compra.AgregarDetalle(detalle);
                await setup.SaveChangesAsync();
            }

            // Act
            using var uow = _db.CreateUnitOfWork();
            var servicio = new ReporteServicio(uow);
            var desde = new DateTime(2025, 1, 1);
            var hasta = new DateTime(2025, 12, 31);
            var resultado = await servicio.RotacionProductosAsync(empresaId, desde, hasta);

            // Assert
            var item = resultado.Should().ContainSingle().Subject;
            item.CantidadComprada.Should().Be(5);
            item.UltimaCompra.Should().Be(compraFecha);
        }

        // ── Test 2: Rotation returns 0 / MinValue when no purchase exists ──────
        [Fact]
        public async Task RotacionProductosAsync_SinCompra_DevuelveCantidadCompradaCeroYMinValue()
        {
            // Arrange: product with 1 sale but NO purchase
            int empresaId;
            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                empresaId = escenario.EmpresaId;

                var producto = Producto.Crear(
                    "Producto Sin Compra", 200m, 100m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "ROT-REG-002");
                producto.StockActual = 10;
                setup.Productos.Add(producto);
                await setup.SaveChangesAsync();

                // Create a client (Venta.Crear requires valid idCliente)
                var cliente = Cliente.Crear("Cliente Sin Compra", documento: 10000002, escenario.EmpresaId);
                setup.Clientes.Add(cliente);
                await setup.SaveChangesAsync();

                // Sale (needed so the product appears in rotation query)
                var venta = Venta.Crear(escenario.SucursalId, cliente.Id, escenario.UsuarioId);
                venta.Fecha = new DateTime(2025, 5, 10, 10, 0, 0, DateTimeKind.Local);
                setup.Ventas.Add(venta);
                await setup.SaveChangesAsync();
                var ventaDetalle = new VentaDetalle
                {
                    Id_venta = venta.Id,
                    Id_producto = producto.Id,
                    Cantidad = 2,
                    PrecioUnitario = 200m,
                    CostoUnitario = 100m,
                    Subtotal = 400m
                };
                setup.VentaDetalles.Add(ventaDetalle);
                await setup.SaveChangesAsync();
            }

            // Act
            using var uow = _db.CreateUnitOfWork();
            var servicio = new ReporteServicio(uow);
            var desde = new DateTime(2025, 1, 1);
            var hasta = new DateTime(2025, 12, 31);
            var resultado = await servicio.RotacionProductosAsync(empresaId, desde, hasta);

            // Assert
            var item = resultado.Should().ContainSingle().Subject;
            item.CantidadComprada.Should().Be(0);
            item.UltimaCompra.Should().Be(DateTime.MinValue);
        }

        // ── Test 3: MetodosPagoUtilizadosAsync returns real Cantidad ───────────
        [Fact]
        public async Task MetodosPagoUtilizadosAsync_ConPagos_DevuelveCantidadReal()
        {
            // Arrange: a paid sale with 3 payments and 1 cancelled (not counted)
            int sucursalId;
            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                sucursalId = escenario.SucursalId;

                // Create a client (Venta.Crear requires valid idCliente)
                var cliente = Cliente.Crear("Cliente Pago Test", documento: 11223344, escenario.EmpresaId);
                setup.Clientes.Add(cliente);
                await setup.SaveChangesAsync();

                // Venta pagada — Fecha must be within the 2025 filter range
                var ventaPagada = Venta.Crear(sucursalId, cliente.Id, escenario.UsuarioId);
                ventaPagada.Fecha = new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Local);
                ventaPagada.Estado = (int)EstadoVentaEnum.Pagada;
                ventaPagada.TotalFinal = 300m;
                setup.Ventas.Add(ventaPagada);
                await setup.SaveChangesAsync();

                // 3 pagos
                var metodoEfectivo = setup.MetodosPago.First(m => m.Nombre == "Efectivo");
                var metodoDebito = setup.MetodosPago.First(m => m.Nombre == "Débito");

                var pago1 = GestionComercial.Dominio.Entidades.Pagos.Pago.Crear(100m, ventaPagada.Id, metodoEfectivo.Id);
                var pago2 = GestionComercial.Dominio.Entidades.Pagos.Pago.Crear(100m, ventaPagada.Id, metodoEfectivo.Id);
                var pago3 = GestionComercial.Dominio.Entidades.Pagos.Pago.Crear(100m, ventaPagada.Id, metodoDebito.Id);
                setup.Pagos.AddRange(pago1, pago2, pago3);

                // Venta anulada (must not count) — Fecha must be within the 2025 filter range
                var ventaAnulada = Venta.Crear(sucursalId, cliente.Id, escenario.UsuarioId);
                ventaAnulada.Fecha = new DateTime(2025, 7, 10, 10, 0, 0, DateTimeKind.Local);
                ventaAnulada.Estado = (int)EstadoVentaEnum.Anulada;
                ventaAnulada.TotalFinal = 50m;
                setup.Ventas.Add(ventaAnulada);
                await setup.SaveChangesAsync();

                var pagoAnulado = GestionComercial.Dominio.Entidades.Pagos.Pago.Crear(50m, ventaAnulada.Id, metodoEfectivo.Id);
                setup.Pagos.Add(pagoAnulado);
                await setup.SaveChangesAsync();
            }

            // Act
            using var uow = _db.CreateUnitOfWork();
            var servicio = new ReporteServicio(uow);
            var desde = new DateTime(2025, 1, 1);
            var hasta = new DateTime(2025, 12, 31);
            var resultado = (await servicio.MetodosPagoUtilizadosAsync(sucursalId, desde, hasta)).ToList();

            // Assert: Efectivo has 2 real payments (from the paid sale only), Debito has 1
            var efectivo = resultado.Should().ContainSingle(m => m.Metodo == "Efectivo").Subject;
            efectivo.Cantidad.Should().Be(2);

            var debito = resultado.Should().ContainSingle(m => m.Metodo == "Débito").Subject;
            debito.Cantidad.Should().Be(1);
        }

        // ── Test 4: CompraRepositorio monthly totals grouping ──────────────────
        [Fact]
        public async Task ObtenerComprasPorMesAsync_ConComprasEnDosMeses_AgrupaCorrectamente()
        {
            // Arrange
            int sucursalId;
            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                sucursalId = escenario.SucursalId;

                var proveedor = Proveedor.Crear("Proveedor Mensual", escenario.EmpresaId);
                setup.Proveedores.Add(proveedor);
                await setup.SaveChangesAsync();

                // Create product first so it's available for CompraDetalle
                var producto = Producto.Crear(
                    "Producto Mensual Test", 200m, 100m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "MES-REG-001");
                setup.Productos.Add(producto);
                await setup.SaveChangesAsync();

                // Compra en enero (total = 100) — add detail BEFORE setting estado Recibida
                var c1 = Compra.Crear(proveedor.Id, sucursalId, escenario.UsuarioId);
                c1.Fecha = new DateTime(2025, 1, 20, 10, 0, 0, DateTimeKind.Local);
                setup.Compras.Add(c1);
                await setup.SaveChangesAsync();
                c1.AgregarDetalle(CompraDetalle.Crear(producto, 10, 10m));
                c1.Estado = (int)EstadoCompraEnum.Recibida;
                await setup.SaveChangesAsync();

                // Compra en enero (total = 50) — same month
                var c2 = Compra.Crear(proveedor.Id, sucursalId, escenario.UsuarioId);
                c2.Fecha = new DateTime(2025, 1, 25, 10, 0, 0, DateTimeKind.Local);
                setup.Compras.Add(c2);
                await setup.SaveChangesAsync();
                c2.AgregarDetalle(CompraDetalle.Crear(producto, 5, 10m));
                c2.Estado = (int)EstadoCompraEnum.Recibida;
                await setup.SaveChangesAsync();

                // Compra en febrero (total = 200)
                var c3 = Compra.Crear(proveedor.Id, sucursalId, escenario.UsuarioId);
                c3.Fecha = new DateTime(2025, 2, 10, 10, 0, 0, DateTimeKind.Local);
                setup.Compras.Add(c3);
                await setup.SaveChangesAsync();
                c3.AgregarDetalle(CompraDetalle.Crear(producto, 20, 10m));
                c3.Estado = (int)EstadoCompraEnum.Recibida;
                await setup.SaveChangesAsync();

                // Compra anulada (Estado = 3) — must be excluded
                var c4 = Compra.Crear(proveedor.Id, sucursalId, escenario.UsuarioId);
                c4.Fecha = new DateTime(2025, 3, 5, 10, 0, 0, DateTimeKind.Local);
                c4.Estado = (int)EstadoCompraEnum.Anulada;
                setup.Compras.Add(c4);
                await setup.SaveChangesAsync();
            }

            // Act
            using var uow = _db.CreateUnitOfWork();
            var desde = new DateTime(2025, 1, 1);
            var hasta = new DateTime(2025, 12, 31);
            var resultado = await uow.Compras.ObtenerComprasPorMesAsync(sucursalId, desde, hasta);

            // Assert: only January and February appear; March (anulada) excluded
            resultado.Should().HaveCount(2);

            var enero = resultado.Should().ContainSingle(r => r.AnioMes == 202501).Subject;
            enero.Total.Should().Be(150m); // 100 + 50

            var febrero = resultado.Should().ContainSingle(r => r.AnioMes == 202502).Subject;
            febrero.Total.Should().Be(200m);
        }

        // ── Test 5: Metrics must not multiply totals by detail lines ──────────
        [Fact]
        public async Task ObtenerMetricasComprasAsync_CompraConVariosDetalles_NoMultiplicaElTotal()
        {
            // Arrange: 1 compra recibida con DOS detalles (total 100) + 1 compra anulada
            int sucursalId;
            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                sucursalId = escenario.SucursalId;

                var proveedor = Proveedor.Crear("Proveedor Metricas", escenario.EmpresaId);
                setup.Proveedores.Add(proveedor);
                await setup.SaveChangesAsync();

                var producto = Producto.Crear(
                    "Producto Metricas Test", 200m, 100m,
                    escenario.EmpresaId, escenario.CategoriaId, escenario.UnidadMedidaId,
                    codigoBarra: "MET-REG-001");
                setup.Productos.Add(producto);
                await setup.SaveChangesAsync();

                // Compra recibida con DOS detalles (total 100) — el join viejo la contaba 2 veces
                var c1 = Compra.Crear(proveedor.Id, sucursalId, escenario.UsuarioId);
                c1.Fecha = new DateTime(2025, 4, 10, 10, 0, 0, DateTimeKind.Local);
                setup.Compras.Add(c1);
                await setup.SaveChangesAsync();
                c1.AgregarDetalle(CompraDetalle.Crear(producto, 5, 10m)); // subtotal 50
                c1.AgregarDetalle(CompraDetalle.Crear(producto, 5, 10m)); // subtotal 50
                c1.Estado = (int)EstadoCompraEnum.Recibida;
                await setup.SaveChangesAsync();

                // Compra anulada (sin detalles) — debe quedar excluida
                var c2 = Compra.Crear(proveedor.Id, sucursalId, escenario.UsuarioId);
                c2.Fecha = new DateTime(2025, 4, 20, 10, 0, 0, DateTimeKind.Local);
                c2.Estado = (int)EstadoCompraEnum.Anulada;
                setup.Compras.Add(c2);
                await setup.SaveChangesAsync();
            }

            // Act
            using var uow = _db.CreateUnitOfWork();
            var desde = new DateTime(2025, 1, 1);
            var hasta = new DateTime(2025, 12, 31);
            var resultado = await uow.Compras.ObtenerMetricasComprasAsync(sucursalId, desde, hasta);

            // Assert: total real (100), NO multiplicado por las 2 líneas; anulada excluida
            resultado.Should().NotBeNull();
            resultado!.Value.Total.Should().Be(100m);
            resultado.Value.Count.Should().Be(1);
            resultado.Value.Promedio.Should().Be(100m);
        }
    }
}
