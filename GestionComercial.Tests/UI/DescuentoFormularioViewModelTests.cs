using Caliburn.Micro;
using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Descuentos;
using Moq;

namespace GestionComercial.Tests.UI
{
    public class DescuentoFormularioViewModelTests
    {
        private readonly Mock<IDescuentoConfiguracionServicio> _mockServicio = new();
        private readonly Mock<IProductoServicio> _mockProductoServicio = new();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly Mock<IEventAggregator> _mockEventAggregator = new();
        private readonly SesionServicio _sesion;

        public DescuentoFormularioViewModelTests()
        {
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

        private DescuentoFormularioViewModel CrearVM()
        {
            return new DescuentoFormularioViewModel(
                _mockServicio.Object,
                _mockProductoServicio.Object,
                _mockUnitOfWork.Object,
                _sesion,
                _mockEventAggregator.Object);
        }

        [Fact]
        public void AmbitoSeleccionado_DefaultEsProducto()
        {
            var vm = CrearVM();
            vm.AmbitoSeleccionado.Should().Be("Producto");
            vm.MuestraSelectorProducto.Should().BeTrue();
            vm.MuestraSelectorCategoria.Should().BeFalse();
        }

        [Fact]
        public void AmbitoSeleccionado_AlCambiarACategoria_ActualizaSelectoresYLimpiarProducto()
        {
            var vm = CrearVM();
            vm.ProductoSeleccionado = new ProductoListadoDto { IdProducto = 42, Nombre = "Leche" };

            vm.AmbitoSeleccionado = "Categoría";

            vm.MuestraSelectorProducto.Should().BeFalse();
            vm.MuestraSelectorCategoria.Should().BeTrue();
            vm.IdProducto.Should().BeNull();
        }

        [Fact]
        public void AmbitoSeleccionado_AlCambiarAProducto_LimpiaCategoria()
        {
            var vm = CrearVM();
            vm.CategoriaSeleccionada = new CategoriaItemDto { IdCategoria = 5, Nombre = "Lácteos" };

            vm.AmbitoSeleccionado = "Producto";

            vm.MuestraSelectorProducto.Should().BeTrue();
            vm.MuestraSelectorCategoria.Should().BeFalse();
            vm.IdCategoria.Should().BeNull();
        }

        [Fact]
        public void AplicaCualquierMetodoPago_DefaultEsTrue()
        {
            var vm = CrearVM();
            vm.AplicaCualquierMetodoPago.Should().BeTrue();
            vm.MuestraSelectorMetodosPago.Should().BeFalse();
        }

        [Fact]
        public void AplicaCualquierMetodoPago_AlDesmarcar_MuestraSelectorMetodosPago()
        {
            var vm = CrearVM();

            vm.AplicaCualquierMetodoPago = false;

            vm.MuestraSelectorMetodosPago.Should().BeTrue();
        }

        [Fact]
        public void AplicaCualquierMetodoPago_AlMarcar_OcultaSelectorMetodosPago()
        {
            var vm = CrearVM();
            vm.AplicaCualquierMetodoPago = false;

            vm.AplicaCualquierMetodoPago = true;

            vm.MuestraSelectorMetodosPago.Should().BeFalse();
        }

        [Fact]
        public void ProductoSeleccionado_SetsIdProductoAndNombre()
        {
            var vm = CrearVM();
            var producto = new ProductoListadoDto { IdProducto = 42, Nombre = "Leche" };

            vm.ProductoSeleccionado = producto;

            vm.IdProducto.Should().Be(42);
            vm.ProductoNombre.Should().Be("Leche");
        }

        [Fact]
        public void ProductoSeleccionado_NullClearsIdAndNombre()
        {
            var vm = CrearVM();
            vm.ProductoSeleccionado = new ProductoListadoDto { IdProducto = 42, Nombre = "Leche" };

            vm.ProductoSeleccionado = null;

            vm.IdProducto.Should().BeNull();
            vm.ProductoNombre.Should().BeEmpty();
        }

        [Fact]
        public void CategoriaSeleccionada_SetsIdCategoriaAndNombre()
        {
            var vm = CrearVM();
            var categoria = new CategoriaItemDto { IdCategoria = 5, Nombre = "Lácteos" };

            vm.CategoriaSeleccionada = categoria;

            vm.IdCategoria.Should().Be(5);
            vm.CategoriaNombre.Should().Be("Lácteos");
        }

        [Fact]
        public void CategoriaSeleccionada_NullClearsIdAndNombre()
        {
            var vm = CrearVM();
            vm.CategoriaSeleccionada = new CategoriaItemDto { IdCategoria = 5, Nombre = "Lácteos" };

            vm.CategoriaSeleccionada = null;

            vm.IdCategoria.Should().BeNull();
            vm.CategoriaNombre.Should().BeEmpty();
        }

        [Fact]
        public async Task GuardarAsync_Crea_Producto_GeneraNombreProductoValorPorcentaje()
        {
            var vm = CrearVM();
            vm.ProductoNombre = "Leche";
            vm.IdProducto = 42;
            vm.Valor = 15;

            await vm.GuardarAsync();

            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), "Leche 15%", 15m, 42, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_Crea_Categoria_GeneraNombreCategoriaValorPorcentaje()
        {
            var vm = CrearVM();
            vm.CategoriaNombre = "Carnes";
            vm.IdCategoria = 5;
            vm.Valor = 10;

            await vm.GuardarAsync();

            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), "Categoría Carnes 10%", 10m, null, 5,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_Crea_ValorDecimal_FormateaSinCeroDecimal()
        {
            var vm = CrearVM();
            vm.ProductoNombre = "Leche";
            vm.IdProducto = 42;
            vm.Valor = 7.5m;

            await vm.GuardarAsync();

            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), "Leche 7.5%", 7.5m, 42, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_Crea_ValorEntero_OmiteDecimales()
        {
            var vm = CrearVM();
            vm.ProductoNombre = "Leche";
            vm.IdProducto = 42;
            vm.Valor = 10.00m;

            await vm.GuardarAsync();

            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), "Leche 10%", 10m, 42, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_Crea_Valor100_GeneraNombreValido()
        {
            var vm = CrearVM();
            vm.ProductoNombre = "Leche";
            vm.IdProducto = 42;
            vm.Valor = 100;

            await vm.GuardarAsync();

            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), "Leche 100%", 100m, 42, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_Edita_GeneraNombreRegenerado()
        {
            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 1;
            vm.ProductoNombre = "Queso";
            vm.IdProducto = 7;
            vm.Valor = 20;

            await vm.GuardarAsync();

            _mockServicio.Verify(s => s.ActualizarAsync(
                1, "Queso 20%", 20m, 7, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_SinScope_MuestraError()
        {
            var vm = CrearVM();
            vm.Valor = 15;

            await vm.GuardarAsync();

            vm.ErrorMessage.Should().Be("Debe seleccionar un producto o una categoría.");
            vm.ErrorVisible.Should().BeTrue();
            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(),
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Never);
        }

        [Fact]
        public async Task GuardarAsync_ValorCero_MuestraError()
        {
            var vm = CrearVM();
            vm.ProductoNombre = "Leche";
            vm.IdProducto = 42;
            vm.Valor = 0;

            await vm.GuardarAsync();

            vm.ErrorMessage.Should().Be("El valor debe ser entre 1 y 100.");
            vm.ErrorVisible.Should().BeTrue();
            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(),
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()),
                Times.Never);
        }
    }
}