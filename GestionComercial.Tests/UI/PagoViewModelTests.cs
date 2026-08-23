using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Ventas;
using Moq;
using System.Windows.Input;

namespace GestionComercial.Tests.UI
{
    public class PagoViewModelTests
    {
        private readonly Mock<IVentaServicio> _mockVentaServicio = new();
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IMetodoPagoRepositorio> _mockMetodos = new();
        private readonly Mock<ISucursalRepositorio> _mockSucursales = new();
        private readonly Mock<IVentaRepostorio> _mockVentaRepo = new();
        private readonly Mock<IDescuentoConfiguracionServicio> _mockDescuentoConfig = new();
        private readonly Mock<ICategoriaRepositorio> _mockCategoriaRepo = new();
        private readonly SesionServicio _sesion;

        public PagoViewModelTests()
        {
            _mockUow.Setup(u => u.MetodosPago).Returns(_mockMetodos.Object);
            _mockUow.Setup(u => u.Sucursales).Returns(_mockSucursales.Object);
            _mockUow.Setup(u => u.Ventas).Returns(_mockVentaRepo.Object);
            _mockUow.Setup(u => u.Categorias).Returns(_mockCategoriaRepo.Object);
            _sesion = new SesionServicio();
            _sesion.IniciarSesion(new UsuarioSesionDto
            {
                IdEmpresa = 1,
                IdSucursal = 1,
                IdUsuario = 1,
                Rol = "Gerente",
                Nombre = "Test",
                Apellido = "User"
            });
        }

        private PagoViewModel CrearVM()
        {
            return new PagoViewModel(_mockVentaServicio.Object, _mockUow.Object, _sesion, _mockDescuentoConfig.Object);
        }

        private async Task<PagoViewModel> CrearVMConMetodosAsync(List<MetodoPago> metodos)
        {
            var sucursal = new Sucursal { Id = 1, Id_empresa = 1 };
            _mockSucursales.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(sucursal);
            _mockMetodos.Setup(r => r.ObtenerTodosPorEmpresaAsync(1)).ReturnsAsync(metodos);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(It.IsAny<int>()))
                .ReturnsAsync((Venta?)null);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 1000);
            // Use reflection to call the private CargarMetodosAsync
            var method = typeof(PagoViewModel).GetMethod("CargarMetodosAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)method!.Invoke(vm, null)!;
            return vm;
        }

        [Fact]
        public async Task CargarMetodos_PopulaSubcategoriaEnDto()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
                new() { Id = 3, Nombre = "Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            vm.MetodosPago.Should().Contain(m => m.NombreMetodo == "Débito" && m.Subcategoria == "Debito");
            vm.MetodosPago.Should().Contain(m => m.NombreMetodo == "Crédito" && m.Subcategoria == "Credito");
        }

        [Fact]
        public async Task AgregarDebito_PrimeraActivaConSubcategoriaDebito()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 5, Nombre = "Mastercard Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
                new() { Id = 6, Nombre = "Visa Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // F2 abre modal de Tarjeta
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);
            vm.MostrarModalTarjeta.Should().BeTrue();

            // Seleccionar Débito en el modal
            var nodoDebito = vm.OpcionesTarjeta.FirstOrDefault(n => n.Nombre == "Débito");
            nodoDebito.Should().NotBeNull();
            vm.SeleccionarMetodoModal(nodoDebito!);

            // Debería mostrar tarjetas débito en el modal
            vm.OpcionesTarjeta.Should().HaveCount(2);
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Mastercard Débito");
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Visa Débito");

            // Seleccionar primera tarjeta
            vm.SeleccionarMetodoModal(vm.OpcionesTarjeta.First());
            vm.MostrarModalTarjeta.Should().BeFalse();
            vm.Pagos.Should().HaveCount(1);
        }

        [Fact]
        public async Task AgregarCredito_PrimeraActivaConSubcategoriaCredito()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 7, Nombre = "Naranja", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
                new() { Id = 8, Nombre = "Mastercard Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // F2 abre modal de Tarjeta
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);
            vm.MostrarModalTarjeta.Should().BeTrue();

            // Seleccionar Crédito en el modal
            var nodoCredito = vm.OpcionesTarjeta.FirstOrDefault(n => n.Nombre == "Crédito");
            nodoCredito.Should().NotBeNull();
            vm.SeleccionarMetodoModal(nodoCredito!);

            // Debería mostrar tarjetas crédito en el modal
            vm.OpcionesTarjeta.Should().HaveCount(2);
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Naranja");
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Mastercard Crédito");

            // Seleccionar primera tarjeta
            vm.SeleccionarMetodoModal(vm.OpcionesTarjeta.First());
            vm.MostrarModalTarjeta.Should().BeFalse();
            vm.Pagos.Should().HaveCount(1);
        }

        [Fact]
        public async Task AgregarDebito_SinMetodos_MuestraSoloCredito()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 3, Nombre = "Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // F2 abre modal de Tarjeta
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);
            vm.MostrarModalTarjeta.Should().BeTrue();

            // Solo debería tener Crédito (no Débito)
            vm.OpcionesTarjeta.Should().HaveCount(1);
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Crédito");
            vm.OpcionesTarjeta.Should().NotContain(n => n.Nombre == "Débito");
        }

        [Fact]
        public async Task AgregarCredito_SinMetodos_MuestraSoloDebito()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // F2 abre modal de Tarjeta
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);
            vm.MostrarModalTarjeta.Should().BeTrue();

            // Solo debería tener Débito (no Crédito)
            vm.OpcionesTarjeta.Should().HaveCount(1);
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Débito");
            vm.OpcionesTarjeta.Should().NotContain(n => n.Nombre == "Crédito");
        }

        [Fact]
        public async Task AgregarDebito_SoloMetodosActivos()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = false, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // Métodos inactivos no se muestran
            vm.NodosVisibles.Should().BeEmpty();
        }

        // ── T6: Descuentos aplicados tests ──────────────────────────────

        [Fact]
        public async Task InicializarConVenta_ConDescuentos_MuestraLineas()
        {
            var venta = GestionComercial.Dominio.Entidades.Ventas.Venta.Crear(1, 1, 1, 5);
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            var producto = new GestionComercial.Dominio.Entidades.Producto.Producto
            {
                Id = 1, Nombre = "Producto Test", PrecioVentaActual = 1000, PrecioCostoActual = 500
            };
            var detalle = GestionComercial.Dominio.Entidades.Ventas.VentaDetalle.Crear(
                producto, 2, 1000, 500, descuentoPorItem: 100);
            venta.AgregarDetalle(detalle);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1)).ReturnsAsync(venta);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 1800);

            vm.LineasDescuento.Should().HaveCount(1);
            vm.LineasDescuento[0].ProductoNombre.Should().Be("Producto Test");
            vm.LineasDescuento[0].Descripcion.Should().Be("Configurado");
            vm.LineasDescuento[0].EsMetodoPago.Should().BeFalse();
            vm.TieneDescuentos.Should().BeTrue();
        }

        [Fact]
        public async Task InicializarConVenta_ConDescuentoMetodoPago_MuestraLineaExtra()
        {
            var venta = GestionComercial.Dominio.Entidades.Ventas.Venta.Crear(1, 1, 1, 5);
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);
            venta.GetType().GetProperty("DescuentoMetodoPago")!.SetValue(venta, 50m);

            var producto = new GestionComercial.Dominio.Entidades.Producto.Producto
            {
                Id = 1, Nombre = "Producto Test", PrecioVentaActual = 1000, PrecioCostoActual = 500
            };
            var detalle = GestionComercial.Dominio.Entidades.Ventas.VentaDetalle.Crear(
                producto, 1, 1000, 500);
            venta.AgregarDetalle(detalle);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1)).ReturnsAsync(venta);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 950);

            vm.LineasDescuento.Should().HaveCount(1); // Solo la línea de método de pago
            vm.LineasDescuento[0].EsMetodoPago.Should().BeTrue();
            vm.LineasDescuento[0].Monto.Should().Be(50m);
            vm.TieneDescuentos.Should().BeTrue();
        }

        [Fact]
        public async Task InicializarConVenta_SinDescuentos_NoMuestraLineas()
        {
            var venta = GestionComercial.Dominio.Entidades.Ventas.Venta.Crear(1, 1, 1, 5);
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            var producto = new GestionComercial.Dominio.Entidades.Producto.Producto
            {
                Id = 1, Nombre = "Producto Test", PrecioVentaActual = 1000, PrecioCostoActual = 500
            };
            var detalle = GestionComercial.Dominio.Entidades.Ventas.VentaDetalle.Crear(
                producto, 1, 1000, 500);
            venta.AgregarDetalle(detalle);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1)).ReturnsAsync(venta);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 1000);

            vm.LineasDescuento.Should().BeEmpty();
            vm.TieneDescuentos.Should().BeFalse();
        }

        [Fact]
        public async Task InicializarConVenta_VentaNoExistente_NoMuestraLineas()
        {
            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(999))
                .ReturnsAsync((Venta?)null);

            var vm = CrearVM();
            await vm.InicializarConVenta(999, "Test", 1000);

            vm.LineasDescuento.Should().BeEmpty();
            vm.TieneDescuentos.Should().BeFalse();
        }

        // ── T7: Preview de descuento por método de pago ─────────────────

        private Venta CrearVentaParaPreview(decimal totalBruto)
        {
            var venta = GestionComercial.Dominio.Entidades.Ventas.Venta.Crear(1, 1, 1, 5);
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);
            var producto = new GestionComercial.Dominio.Entidades.Producto.Producto
            {
                Id = 1, Nombre = "Producto Test", PrecioVentaActual = totalBruto, PrecioCostoActual = totalBruto / 2
            };
            var detalle = GestionComercial.Dominio.Entidades.Ventas.VentaDetalle.Crear(
                producto, 1, totalBruto, totalBruto / 2);
            venta.AgregarDetalle(detalle);
            return venta;
        }

        private void SetupCachesVenta(int idVenta, Venta venta, List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion> descuentos)
        {
            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(idVenta)).ReturnsAsync(venta);
            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>()))
                .ReturnsAsync(descuentos);
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1))
                .ReturnsAsync(new List<GestionComercial.Dominio.Entidades.Producto.Categoria>());
        }

        [Fact]
        public async Task Preview_Visa5Pct_UpdatesTotalVenta()
        {
            var venta = CrearVentaParaPreview(1000m);
            var descuento = GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion.Crear(
                "Visa 5%", 5, 1, idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 2 },
                alcance: GestionComercial.Dominio.Entidades.Descuento.AlcanceDescuentoEnum.MetodoPago);
            descuento.DescuentosMetodosPago.Add(new GestionComercial.Dominio.Entidades.Descuento.DescuentoMetodoPago { Id_metodoPago = 2 });

            SetupCachesVenta(1, venta, new List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion> { descuento });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion>>(),
                    It.IsAny<Dictionary<int, GestionComercial.Dominio.Entidades.Producto.Categoria>>()))
                .ReturnsAsync((GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    1, 2, It.IsAny<List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion>>()))
                .ReturnsAsync(descuento);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 1000);

            // Simular pago único con Visa (método 2)
            vm.MetodoSeleccionado = new PagoItemDto { IdMetodoPago = 2, NombreMetodo = "Visa", Categoria = "Tarjeta" };
            vm.MontoIngresado = "950";
            vm.AgregarPago();
            await InvocarRecalcularDescuentoPreviewAsync(vm);

            vm.TotalVenta.Should().Be(950m); // 1000 - 5% = 950
            vm.LineasDescuento.Should().Contain(l => l.EsMetodoPago && l.Monto == 50m);
        }

        [Fact]
        public async Task Preview_PagoMixto_SinDescuento()
        {
            var venta = CrearVentaParaPreview(1000m);
            var descuento = GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion.Crear(
                "Visa 5%", 5, 1, idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 2 },
                alcance: GestionComercial.Dominio.Entidades.Descuento.AlcanceDescuentoEnum.MetodoPago);
            descuento.DescuentosMetodosPago.Add(new GestionComercial.Dominio.Entidades.Descuento.DescuentoMetodoPago { Id_metodoPago = 2 });

            SetupCachesVenta(1, venta, new List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion> { descuento });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion>>(),
                    It.IsAny<Dictionary<int, GestionComercial.Dominio.Entidades.Producto.Categoria>>()))
                .ReturnsAsync((GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion?)null);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 1000);

            // Pago mixto: Efectivo (1) + Visa (2)
            vm.MetodoSeleccionado = new PagoItemDto { IdMetodoPago = 1, NombreMetodo = "Efectivo", Categoria = "Efectivo" };
            vm.MontoIngresado = "600";
            vm.AgregarPago();
            vm.MetodoSeleccionado = new PagoItemDto { IdMetodoPago = 2, NombreMetodo = "Visa", Categoria = "Tarjeta" };
            vm.MontoIngresado = "400";
            vm.AgregarPago();
            await InvocarRecalcularDescuentoPreviewAsync(vm);

            vm.TotalVenta.Should().Be(1000m);
            vm.LineasDescuento.Should().NotContain(l => l.EsMetodoPago);
        }

        [Fact]
        public async Task Preview_SinMatch_SoloPerItem()
        {
            var venta = CrearVentaParaPreview(1000m);
            var perItem = GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion.Crear(
                "Prod 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);

            SetupCachesVenta(1, venta, new List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion> { perItem });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion>>(),
                    It.IsAny<Dictionary<int, GestionComercial.Dominio.Entidades.Producto.Categoria>>()))
                .ReturnsAsync(perItem);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    1, 2, It.IsAny<List<GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion>>()))
                .ReturnsAsync((GestionComercial.Dominio.Entidades.Descuento.DescuentoConfiguracion?)null);

            var vm = CrearVM();
            await vm.InicializarConVenta(1, "Test", 1000);

            // Pago único con Visa (2) que no tiene descuento total-venta → solo per-item 10%
            vm.MetodoSeleccionado = new PagoItemDto { IdMetodoPago = 2, NombreMetodo = "Visa", Categoria = "Tarjeta" };
            vm.MontoIngresado = "900";
            vm.AgregarPago();
            await InvocarRecalcularDescuentoPreviewAsync(vm);

            vm.TotalVenta.Should().Be(900m); // 1000 - 10% = 900
        }

        private static async Task InvocarRecalcularDescuentoPreviewAsync(PagoViewModel vm)
        {
            var method = typeof(PagoViewModel).GetMethod("RecalcularDescuentoPreviewAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)method!.Invoke(vm, null)!;
        }

        // ── T6: Tests de jerarquía de métodos de pago ─────────────────────

        [Fact]
        public async Task SeleccionarNodo_Hoja_SeleccionaYConfirmaPago()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // Encontrar nodo Efectivo
            var nodoEfectivo = vm.NodosVisibles.FirstOrDefault(n => n.Nombre == "Efectivo");
            nodoEfectivo.Should().NotBeNull();
            nodoEfectivo!.EsHoja.Should().BeTrue();

            vm.SeleccionarNodo(nodoEfectivo);

            vm.Pagos.Should().HaveCount(1);
            vm.Pagos.First().IdMetodoPago.Should().Be(1);
            vm.Pagos.First().NombreMetodo.Should().Be("Efectivo");
        }

        [Fact]
        public async Task SeleccionarNodo_NodoNoHoja_AbreModal()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
                new() { Id = 3, Nombre = "Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            var nodoTarjeta = vm.NodosVisibles.FirstOrDefault(n => n.Nombre == "Tarjeta");
            nodoTarjeta.Should().NotBeNull();
            nodoTarjeta!.EsHoja.Should().BeFalse();

            vm.SeleccionarNodo(nodoTarjeta);

            // Tarjeta abre modal en vez de navegar
            vm.MostrarModalTarjeta.Should().BeTrue();
            vm.OpcionesTarjeta.Should().HaveCount(2);
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Débito");
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Crédito");
        }

        [Fact]
        public async Task CerrarModalTarjeta_Cierra()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            var nodoTarjeta = vm.NodosVisibles.FirstOrDefault(n => n.Nombre == "Tarjeta");
            vm.SeleccionarNodo(nodoTarjeta!);
            vm.MostrarModalTarjeta.Should().BeTrue();

            vm.CerrarModal();
            vm.MostrarModalTarjeta.Should().BeFalse();
        }

        [Fact]
        public async Task Breadcrumb_MuestraRaizInicial()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // En raíz
            vm.Breadcrumb.Should().Be("Raíz");

            // Tarjeta abre modal, no cambia breadcrumb
            var nodoTarjeta = vm.NodosVisibles.FirstOrDefault(n => n.Nombre == "Tarjeta");
            vm.SeleccionarNodo(nodoTarjeta!);
            vm.Breadcrumb.Should().Be("Raíz");
        }

        [Fact]
        public async Task EsNivelRaiz_TrueEnNivelInicial()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            vm.EsNivelRaiz.Should().BeTrue();
        }

        [Fact]
        public async Task ModalTarjeta_SeAbreAlSeleccionar()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            var nodoTarjeta = vm.NodosVisibles.FirstOrDefault(n => n.Nombre == "Tarjeta");
            vm.SeleccionarNodo(nodoTarjeta!);

            vm.MostrarModalTarjeta.Should().BeTrue();
            vm.EsNivelRaiz.Should().BeTrue(); // No navega, abre modal
        }

        [Fact]
        public async Task HandleKeyDown_EscapeCierraModal()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            // Abrir modal de Tarjeta
            var nodoTarjeta = vm.NodosVisibles.FirstOrDefault(n => n.Nombre == "Tarjeta");
            vm.SeleccionarNodo(nodoTarjeta!);
            vm.MostrarModalTarjeta.Should().BeTrue();

            // Escape cierra modal
            vm.HandleKeyDown(Key.Escape, ModifierKeys.None);
            vm.MostrarModalTarjeta.Should().BeFalse();
        }

        [Fact]
        public async Task HandleKeyDown_F1EnNivelRaiz_SeleccionaEfectivo()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            vm.HandleKeyDown(Key.F1, ModifierKeys.None);

            vm.Pagos.Should().HaveCount(1);
            vm.Pagos.First().IdMetodoPago.Should().Be(1);
        }

        [Fact]
        public async Task HandleKeyDown_F2EnNivelRaiz_AbreModalTarjeta()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            vm.HandleKeyDown(Key.F2, ModifierKeys.None);

            // F2 abre modal de Tarjeta
            vm.MostrarModalTarjeta.Should().BeTrue();
            vm.OpcionesTarjeta.Should().Contain(n => n.Nombre == "Débito");
        }
    }
}
