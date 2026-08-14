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
    }
}