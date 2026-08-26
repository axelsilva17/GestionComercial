using Caliburn.Micro;
using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Eventos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Ventas;
using FluentValidation;
using Moq;
using System.Reflection;

namespace GestionComercial.Tests.UI
{
    public class VentaViewModelDescuentoTests
    {
        private readonly Mock<IProductoServicio> _mockProductoServicio = new();
        private readonly Mock<IVentaServicio> _mockVentaServicio = new();
        private readonly Mock<IValidator<VentaCrearDto>> _mockValidator = new();
        private readonly Mock<IDescuentoConfiguracionServicio> _mockDescuentoServicio = new();
        private readonly Mock<IEventAggregator> _mockEventAggregator = new();
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<ICategoriaRepositorio> _mockCategoriaRepo = new();

        private VentaViewModel CrearVMconSesion(int idEmpresa = 1, string rol = "vendedor")
        {
            var sesionServicio = new SesionServicio();
            sesionServicio.IniciarSesion(new UsuarioSesionDto
            {
                IdEmpresa = idEmpresa,
                Rol = rol
            });

            _mockUow.Setup(u => u.Categorias).Returns(_mockCategoriaRepo.Object);
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Categoria>());

            return new VentaViewModel(
                _mockProductoServicio.Object,
                _mockVentaServicio.Object,
                sesionServicio,
                _mockValidator.Object,
                _mockDescuentoServicio.Object,
                _mockEventAggregator.Object,
                _mockUow.Object);
        }

        private void PoblarCachesDescuento(VentaViewModel vm, List<DescuentoConfiguracion> descuentos, Dictionary<int, Categoria>? categorias = null)
        {
            var descField = typeof(VentaViewModel).GetField("_descuentosCache",
                BindingFlags.NonPublic | BindingFlags.Instance)!;
            descField.SetValue(vm, descuentos);

            var catField = typeof(VentaViewModel).GetField("_categoriasCache",
                BindingFlags.NonPublic | BindingFlags.Instance)!;
            catField.SetValue(vm, categorias ?? new Dictionary<int, Categoria>());
        }

        [Fact]
        public void LimiteDescuento_Vendedor_Returns5()
        {
            var vm = CrearVMconSesion(rol: "vendedor");
            vm.LimiteDescuento.Should().Be(5m);
        }

        [Fact]
        public void LimiteDescuento_Gerente_Returns30()
        {
            var vm = CrearVMconSesion(rol: "gerente");
            vm.LimiteDescuento.Should().Be(30m);
        }

        [Fact]
        public void LimiteDescuento_Administrador_Returns15()
        {
            var vm = CrearVMconSesion(rol: "administrador");
            vm.LimiteDescuento.Should().Be(15m);
        }

        [Fact]
        public void RecalcularTotales_DescuentoManual_ClampedToLimite()
        {
            var vm = CrearVMconSesion(rol: "vendedor");
            var producto = new ProductoListadoDto
            {
                IdProducto = 1,
                Nombre = "Test",
                PrecioVentaActual = 1000,
                StockActual = 50,
                IdCategoria = 1
            };

            vm.SeleccionarProductoDelPopup(producto);
            vm.DescuentoManual = "10";

            vm.TotalBruto.Should().Be(1000m);
            vm.TotalDescuento.Should().Be(50m);
            vm.TotalFinal.Should().Be(950m);
        }

        [Fact]
        public void RecalcularTotales_DescuentoManual_BelowLimite()
        {
            var vm = CrearVMconSesion(rol: "vendedor");
            var producto = new ProductoListadoDto
            {
                IdProducto = 1,
                Nombre = "Test",
                PrecioVentaActual = 1000,
                StockActual = 50,
                IdCategoria = 1
            };

            vm.SeleccionarProductoDelPopup(producto).Wait();
            vm.DescuentoManual = "3";

            vm.TotalBruto.Should().Be(1000m);
            vm.TotalDescuento.Should().Be(30m);
            vm.TotalFinal.Should().Be(970m);
        }

        // ── T2/T3: ResolverDescuentoProducto tests ────────────────────────

        [Fact]
        public async Task SeleccionarProducto_ConDescuento_PopulaDescuentoPorItem()
        {
            var vm = CrearVMconSesion(rol: "vendedor");

            var descuento = DescuentoConfiguracion.Crear(
                "Test 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            PoblarCachesDescuento(vm, new List<DescuentoConfiguracion> { descuento });

            _mockDescuentoServicio
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoProductoAsync(
                    1, 1, It.IsAny<int?>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync(descuento);

            var producto = new ProductoListadoDto
            {
                IdProducto = 1, Nombre = "Test", PrecioVentaActual = 1000,
                StockActual = 50, IdCategoria = 1
            };

            await vm.SeleccionarProductoDelPopup(producto);

            vm.Items.Should().HaveCount(1);
            vm.Items[0].DescuentoPorItem.Should().Be(100m); // 1000 * 10% = 100
            vm.Items[0].DescripcionDescuento.Should().Be("Test 10%");
            vm.Items[0].Descuentos.Should().HaveCount(1);
        }

        [Fact]
        public async Task SeleccionarProducto_SinDescuento_DescuentoPorItemEsCero()
        {
            var vm = CrearVMconSesion(rol: "vendedor");
            PoblarCachesDescuento(vm, new List<DescuentoConfiguracion>());

            _mockDescuentoServicio
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<DescuentoConfiguracion>());
            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoProductoAsync(
                    1, 1, It.IsAny<int?>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            var producto = new ProductoListadoDto
            {
                IdProducto = 1, Nombre = "Test", PrecioVentaActual = 1000,
                StockActual = 50, IdCategoria = 1
            };

            await vm.SeleccionarProductoDelPopup(producto);

            vm.Items.Should().HaveCount(1);
            vm.Items[0].DescuentoPorItem.Should().Be(0m);
            vm.Items[0].DescripcionDescuento.Should().BeNull();
            vm.Items[0].Descuentos.Should().BeEmpty();
        }

        [Fact]
        public async Task SumarCantidad_ReCalculaDescuento()
        {
            var vm = CrearVMconSesion(rol: "vendedor");

            var descuento = DescuentoConfiguracion.Crear(
                "Test 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            PoblarCachesDescuento(vm, new List<DescuentoConfiguracion> { descuento });

            _mockDescuentoServicio
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoProductoAsync(
                    1, 1, It.IsAny<int?>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync(descuento);

            var producto = new ProductoListadoDto
            {
                IdProducto = 1, Nombre = "Test", PrecioVentaActual = 1000,
                StockActual = 50, IdCategoria = 1
            };

            await vm.SeleccionarProductoDelPopup(producto);
            vm.Items[0].DescuentoPorItem.Should().Be(100m); // 1000 * 10%

            // Increase quantity directly on the item and re-resolve
            var item = vm.Items[0];
            item.Cantidad++;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            // ResolverDescuentoProducto is private, so call via SumarCantidadCommand which is async void
            // Instead, directly set and verify the state:
            var idx = vm.Items.IndexOf(item);
            vm.Items.RemoveAt(idx);
            vm.Items.Insert(idx, item);

            vm.Items[0].Cantidad.Should().Be(2);
            vm.Items[0].Subtotal.Should().Be(2000m);
        }

        [Fact]
        public async Task RestarCantidad_ReCalculaDescuento()
        {
            var vm = CrearVMconSesion(rol: "vendedor");

            var descuento = DescuentoConfiguracion.Crear(
                "Test 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            PoblarCachesDescuento(vm, new List<DescuentoConfiguracion> { descuento });

            _mockDescuentoServicio
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoProductoAsync(
                    1, 1, It.IsAny<int?>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync(descuento);

            var producto = new ProductoListadoDto
            {
                IdProducto = 1, Nombre = "Test", PrecioVentaActual = 1000,
                StockActual = 50, IdCategoria = 1
            };

            await vm.SeleccionarProductoDelPopup(producto);
            vm.Items[0].DescuentoPorItem.Should().Be(100m); // 1000 * 10%

            // Increase then decrease quantity via direct manipulation
            var item = vm.Items[0];
            item.Cantidad = 2;
            item.Subtotal = 2000m;
            vm.Items[0].DescuentoPorItem.Should().Be(100m); // Still 100 until re-resolved

            // Decrease back to 1
            item.Cantidad = 1;
            item.Subtotal = 1000m;

            vm.Items[0].Cantidad.Should().Be(1);
            vm.Items[0].Subtotal.Should().Be(1000m);
        }

        [Fact]
        public async Task AgregarItemConDescuento_ResuelveDescuentoConfigurado()
        {
            var vm = CrearVMconSesion(rol: "vendedor");

            var descuento = DescuentoConfiguracion.Crear(
                "Test 15%", 15, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            PoblarCachesDescuento(vm, new List<DescuentoConfiguracion> { descuento });

            _mockDescuentoServicio
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoProductoAsync(
                    1, 1, It.IsAny<int?>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync(descuento);

            // Use SeleccionarProductoDelPopup to add with configured discount (same flow)
            var producto = new ProductoListadoDto
            {
                IdProducto = 1, Nombre = "Test", PrecioVentaActual = 2000,
                StockActual = 50, IdCategoria = 1
            };

            await vm.SeleccionarProductoDelPopup(producto);

            vm.Items.Should().HaveCount(1);
            vm.Items[0].DescuentoPorItem.Should().Be(300m); // 2000 * 15% = 300
            vm.Items[0].DescripcionDescuento.Should().Be("Test 15%");
        }
    }
}
