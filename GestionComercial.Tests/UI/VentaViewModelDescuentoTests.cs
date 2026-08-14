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

            vm.SeleccionarProductoDelPopup(producto);
            vm.DescuentoManual = "3";

            vm.TotalBruto.Should().Be(1000m);
            vm.TotalDescuento.Should().Be(30m);
            vm.TotalFinal.Should().Be(970m);
        }
    }
}
