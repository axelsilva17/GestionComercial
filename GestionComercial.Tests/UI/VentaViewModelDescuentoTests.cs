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

        private static void SeedAllCaches(
            VentaViewModel vm,
            List<DescuentoConfiguracion> descuentos,
            Dictionary<int, Categoria> categorias,
            List<ProductoListadoDto> productos)
        {
            typeof(VentaViewModel).GetField("_descuentosCache", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(vm, descuentos);
            typeof(VentaViewModel).GetField("_categoriasCache", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(vm, categorias);
            typeof(VentaViewModel).GetField("_productosCache", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(vm, productos);
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
        public void SeleccionarProductoDelPopup_ConDescuento_AplicaDescuento()
        {
            var vm = CrearVMconSesion();
            var producto = new ProductoListadoDto
            {
                IdProducto = 10,
                Nombre = "Leche",
                PrecioVentaActual = 1000,
                StockActual = 50,
                IdCategoria = 5
            };

            var descuento = DescuentoConfiguracion.Crear(
                "Leche 20%", TipoDescuentoEnum.Producto, 20, 1, idProducto: 10);

            SeedAllCaches(vm,
                new List<DescuentoConfiguracion> { descuento },
                new Dictionary<int, Categoria>(),
                new List<ProductoListadoDto> { producto });

            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 10, 5,
                    It.IsAny<List<DescuentoConfiguracion>>(),
                    It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync(descuento);

            vm.SeleccionarProductoDelPopup(producto);

            vm.Items.Should().HaveCount(1);
            vm.Items[0].DescuentoPorItem.Should().Be(200m);
            vm.TotalDescuento.Should().Be(200m);
            vm.TotalFinal.Should().Be(800m);
        }

        [Fact]
        public void SeleccionarProductoDelPopup_SinDescuento_DescuentoPorItemCero()
        {
            var vm = CrearVMconSesion();
            var producto = new ProductoListadoDto
            {
                IdProducto = 10,
                Nombre = "Leche",
                PrecioVentaActual = 1000,
                StockActual = 50,
                IdCategoria = 5
            };

            SeedAllCaches(vm,
                new List<DescuentoConfiguracion>(),
                new Dictionary<int, Categoria>(),
                new List<ProductoListadoDto> { producto });

            _mockDescuentoServicio
                .Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 10, 5,
                    It.IsAny<List<DescuentoConfiguracion>>(),
                    It.IsAny<Dictionary<int, Categoria>>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            vm.SeleccionarProductoDelPopup(producto);

            vm.Items.Should().HaveCount(1);
            vm.Items[0].DescuentoPorItem.Should().Be(0m);
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

            vm.SeleccionarProductoDelPopup(producto);
            vm.DescuentoManual = "3";

            vm.TotalBruto.Should().Be(1000m);
            vm.TotalDescuento.Should().Be(30m);
            vm.TotalFinal.Should().Be(970m);
        }
    }
}
