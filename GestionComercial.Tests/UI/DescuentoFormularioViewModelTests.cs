using Caliburn.Micro;
using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Eventos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Descuentos;
using Moq;

namespace GestionComercial.Tests.UI
{
    public class DescuentoFormularioViewModelTests
    {
        private readonly Mock<IDescuentoConfiguracionServicio> _mockServicio = new();
        private readonly Mock<IProductoServicio> _mockProductoServicio = new();
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
                _sesion,
                _mockEventAggregator.Object);
        }

        [Fact]
        public void TipoSeleccionado_DefaultIsProducto()
        {
            var vm = CrearVM();
            vm.TipoSeleccionado.Should().Be(TipoDescuentoEnum.Producto);
        }

        [Fact]
        public void TipoSeleccionado_UpdateNotifiesEsTipoProducto()
        {
            var vm = CrearVM();
            vm.TipoSeleccionado.Should().Be(TipoDescuentoEnum.Producto);
            vm.EsTipoProducto.Should().BeTrue();
            vm.EsTipoCategoria.Should().BeFalse();

            vm.TipoSeleccionado = TipoDescuentoEnum.Categoria;

            vm.EsTipoProducto.Should().BeFalse();
            vm.EsTipoCategoria.Should().BeTrue();
        }

        [Fact]
        public void TipoSeleccionadoStr_GetReturnsEnumName()
        {
            var vm = CrearVM();
            vm.TipoSeleccionadoStr.Should().Be("Producto");
        }

        [Fact]
        public void TipoSeleccionadoStr_SetParsesEnum()
        {
            var vm = CrearVM();
            vm.TipoSeleccionadoStr = "Categoria";
            vm.TipoSeleccionado.Should().Be(TipoDescuentoEnum.Categoria);
        }

        [Fact]
        public void TipoSeleccionadoStr_SetInvalidValue_DoesNotCrash()
        {
            var vm = CrearVM();
            var original = vm.TipoSeleccionado;
            vm.TipoSeleccionadoStr = "InvalidValue";
            vm.TipoSeleccionado.Should().Be(original);
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
