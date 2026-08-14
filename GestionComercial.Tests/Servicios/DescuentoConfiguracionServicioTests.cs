using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class DescuentoConfiguracionServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IDescuentoConfiguracionRepositorio> _mockRepo = new();
        private readonly DescuentoConfiguracionServicio _servicio;

        public DescuentoConfiguracionServicioTests()
        {
            _mockUow.Setup(u => u.DescuentoConfiguraciones).Returns(_mockRepo.Object);
            _servicio = new DescuentoConfiguracionServicio(_mockUow.Object);
        }

        [Fact]
        public async Task CrearAsync_ValidDto_CreatesAndSaves()
        {
            _mockRepo.Setup(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()))
                .Returns<DescuentoConfiguracion>(d => Task.FromResult(d));

            var resultado = await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "Test 10%", tipo: TipoDescuentoEnum.Producto,
                valor: 10, idProducto: 1, idCategoria: null,
                fechaDesde: null, fechaHasta: null, prioridad: 0);

            resultado.Should().NotBeNull();
            resultado.Nombre.Should().Be("Test 10%");
            resultado.Valor.Should().Be(10);
            _mockRepo.Verify(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task CrearAsync_InvalidDto_Throws()
        {
            Func<Task> act = async () => await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "", tipo: TipoDescuentoEnum.Producto,
                valor: 10, idProducto: 1, idCategoria: null,
                fechaDesde: null, fechaHasta: null, prioridad: 0);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_ProductDiscount_Wins()
        {
            var productDiscount = DescuentoConfiguracion.Crear(
                "Prod 20%", TipoDescuentoEnum.Producto, 20, 1, idProducto: 10);
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 10%", TipoDescuentoEnum.Categoria, 10, 1, idCategoria: 5);

            var cache = new List<DescuentoConfiguracion> { productDiscount, categoryDiscount };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, 5, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(productDiscount.Id);
            result.Valor.Should().Be(20);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_CategoryDiscount_Applies()
        {
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 15%", TipoDescuentoEnum.Categoria, 15, 1, idCategoria: 5);

            var categoria = new Categoria { Id = 5, CategoriaPadre_id = null, Id_empresa = 1 };
            var cache = new List<DescuentoConfiguracion> { categoryDiscount };
            var catCache = new Dictionary<int, Categoria> { { 5, categoria } };

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, 5, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(15);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_PriorityWins()
        {
            var productDiscount = DescuentoConfiguracion.Crear(
                "Prod 20%", TipoDescuentoEnum.Producto, 20, 1, idProducto: 10, prioridad: 0);
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 10%", TipoDescuentoEnum.Categoria, 10, 1, idCategoria: 5, prioridad: 5);

            var categoria = new Categoria { Id = 5, CategoriaPadre_id = null, Id_empresa = 1 };
            var cache = new List<DescuentoConfiguracion> { productDiscount, categoryDiscount };
            var catCache = new Dictionary<int, Categoria> { { 5, categoria } };

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, 5, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(categoryDiscount.Id);
            result.Valor.Should().Be(10);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_ExpiredDiscount_Excluded()
        {
            var expired = DescuentoConfiguracion.Crear(
                "Expired", TipoDescuentoEnum.Producto, 20, 1, idProducto: 10,
                fechaHasta: DateTime.Now.AddDays(-1));

            var cache = new List<DescuentoConfiguracion> { expired };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_DisabledDiscount_Excluded()
        {
            var disabled = DescuentoConfiguracion.Crear(
                "Disabled", TipoDescuentoEnum.Producto, 20, 1, idProducto: 10);
            disabled.Inactivar();

            var cache = new List<DescuentoConfiguracion> { disabled };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_NoDiscount_ReturnsNull()
        {
            var cache = new List<DescuentoConfiguracion>();
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_HierarchyWalk_AppliesAncestor()
        {
            var ancestorDiscount = DescuentoConfiguracion.Crear(
                "Root 10%", TipoDescuentoEnum.Categoria, 10, 1, idCategoria: 1);

            var root = new Categoria { Id = 1, CategoriaPadre_id = null, Id_empresa = 1 };
            var child = new Categoria { Id = 2, CategoriaPadre_id = 1, Id_empresa = 1 };
            var grandchild = new Categoria { Id = 3, CategoriaPadre_id = 2, Id_empresa = 1 };

            var cache = new List<DescuentoConfiguracion> { ancestorDiscount };
            var catCache = new Dictionary<int, Categoria>
            {
                { 1, root }, { 2, child }, { 3, grandchild }
            };

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, null, 3, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(ancestorDiscount.Id);
        }

        [Fact]
        public async Task EliminarAsync_ExistingEntity_SetsInactivo()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Test", TipoDescuentoEnum.Producto, 10, 1, idProducto: 1);
            _mockRepo.Setup(r => r.ObtenerPorIdAsync(descuento.Id))
                .ReturnsAsync(descuento);

            await _servicio.EliminarAsync(descuento.Id);

            descuento.Activo.Should().BeFalse();
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_ExistingEntity_UpdatesFields()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Old", TipoDescuentoEnum.Producto, 10, 1, idProducto: 1);
            _mockRepo.Setup(r => r.ObtenerPorIdAsync(descuento.Id))
                .ReturnsAsync(descuento);

            await _servicio.ActualizarAsync(descuento.Id, "New", 25, null, null, 5);

            descuento.Nombre.Should().Be("New");
            descuento.Valor.Should().Be(25);
            descuento.Prioridad.Should().Be(5);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }
    }
}
