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

        private static void AgregarMetodoPago(DescuentoConfiguracion descuento, int idMetodoPago)
            => descuento.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = idMetodoPago });

        // ── CrearAsync ──────────────────────────────────────────────────────
        [Fact]
        public async Task CrearAsync_Producto_CualquierMetodo_CreatesAndSaves()
        {
            _mockRepo.Setup(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()))
                .Returns<DescuentoConfiguracion>(d => Task.FromResult(d));

            var resultado = await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "Test 10%", valor: 10,
                idProducto: 1, idCategoria: null,
                aplicaCualquierMetodoPago: true, idsMetodosPago: null,
                fechaDesde: null, fechaHasta: null);

            resultado.Should().NotBeNull();
            resultado.Nombre.Should().Be("Test 10%");
            resultado.Valor.Should().Be(10);
            resultado.AplicaCualquierMetodoPago.Should().BeTrue();
            _mockRepo.Verify(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task CrearAsync_MetodosEspecificos_PersistsRelacionesNM()
        {
            _mockRepo.Setup(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()))
                .Returns<DescuentoConfiguracion>(d => Task.FromResult(d));

            var resultado = await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "Débito 5%", valor: 5,
                idProducto: 1, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 1, 2 },
                fechaDesde: null, fechaHasta: null);

            resultado.Should().NotBeNull();
            resultado.AplicaCualquierMetodoPago.Should().BeFalse();
            _mockRepo.Verify(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()), Times.Once);
            _mockRepo.Verify(r => r.ActualizarMetodosPagoAsync(
                It.IsAny<int>(), It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 1, 2 }))), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CrearAsync_CualquierMetodo_NoPersisteNM()
        {
            _mockRepo.Setup(r => r.AgregarAsync(It.IsAny<DescuentoConfiguracion>()))
                .Returns<DescuentoConfiguracion>(d => Task.FromResult(d));

            await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "General 5%", valor: 5,
                idProducto: 1, idCategoria: null,
                aplicaCualquierMetodoPago: true, idsMetodosPago: null,
                fechaDesde: null, fechaHasta: null);

            _mockRepo.Verify(r => r.ActualizarMetodosPagoAsync(
                It.IsAny<int>(), It.IsAny<List<int>>()), Times.Never);
        }

        [Fact]
        public async Task CrearAsync_NombreVacio_Throws()
        {
            Func<Task> act = async () => await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "", valor: 10,
                idProducto: 1, idCategoria: null,
                aplicaCualquierMetodoPago: true, idsMetodosPago: null,
                fechaDesde: null, fechaHasta: null);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task CrearAsync_EspecificoSinMetodos_Throws()
        {
            Func<Task> act = async () => await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "Sin método", valor: 10,
                idProducto: 1, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int>(),
                fechaDesde: null, fechaHasta: null);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task CrearAsync_SinScope_Throws()
        {
            Func<Task> act = async () => await _servicio.CrearAsync(
                idEmpresa: 1, nombre: "Sin scope", valor: 10,
                idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: true, idsMetodosPago: null,
                fechaDesde: null, fechaHasta: null);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        // ── ActualizarAsync ─────────────────────────────────────────────────
        [Fact]
        public async Task ActualizarAsync_UpdatesFields_AndPersistsNM()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Old", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            _mockRepo.Setup(r => r.ObtenerPorIdAsync(descuento.Id))
                .ReturnsAsync(descuento);

            await _servicio.ActualizarAsync(
                descuento.Id, "New", 25, 1, null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 1 },
                fechaDesde: null, fechaHasta: null);

            descuento.Nombre.Should().Be("New");
            descuento.Valor.Should().Be(25);
            descuento.AplicaCualquierMetodoPago.Should().BeFalse();
            _mockRepo.Verify(r => r.ActualizarMetodosPagoAsync(
                descuento.Id, It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 1 }))), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_CambiaScopeAProducto()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Old", 10, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);
            _mockRepo.Setup(r => r.ObtenerPorIdAsync(descuento.Id))
                .ReturnsAsync(descuento);

            await _servicio.ActualizarAsync(
                descuento.Id, "ProdDiscount", 15, 1, null,
                aplicaCualquierMetodoPago: true, idsMetodosPago: null,
                fechaDesde: null, fechaHasta: null);

            descuento.Id_producto.Should().Be(1);
            descuento.Id_categoria.Should().BeNull();
            descuento.Nombre.Should().Be("ProdDiscount");
        }

        [Fact]
        public async Task ActualizarAsync_EspecificoSinMetodos_Throws()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Old", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            _mockRepo.Setup(r => r.ObtenerPorIdAsync(descuento.Id))
                .ReturnsAsync(descuento);

            Func<Task> act = async () => await _servicio.ActualizarAsync(
                descuento.Id, "New", 10, 1, null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int>(),
                fechaDesde: null, fechaHasta: null);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        // ── EliminarAsync ───────────────────────────────────────────────────
        [Fact]
        public async Task EliminarAsync_ExistingEntity_SetsInactivo()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Test", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            _mockRepo.Setup(r => r.ObtenerPorIdAsync(descuento.Id))
                .ReturnsAsync(descuento);

            await _servicio.EliminarAsync(descuento.Id);

            descuento.Activo.Should().BeFalse();
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        // ── ObtenerDescuentoAplicableAsync ──────────────────────────────────
        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_ProductoGanaACategoria()
        {
            var productDiscount = DescuentoConfiguracion.Crear(
                "Prod 20%", 20, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 10%", 10, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);

            var cache = new List<DescuentoConfiguracion> { productDiscount, categoryDiscount };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, 5, new List<int> { 1 }, true, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(productDiscount.Id);
            result.Valor.Should().Be(20);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_Categoria_Aplica()
        {
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 15%", 15, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);

            var categoria = new Categoria { Id = 5, CategoriaPadre_id = null, Id_empresa = 1 };
            var cache = new List<DescuentoConfiguracion> { categoryDiscount };
            var catCache = new Dictionary<int, Categoria> { { 5, categoria } };

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, 5, new List<int> { 1 }, true, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(15);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_EmpateMayorValor_Gana()
        {
            var bajo = DescuentoConfiguracion.Crear(
                "Cat 10%", 10, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);
            var alto = DescuentoConfiguracion.Crear(
                "Cat 20%", 20, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);

            var categoria = new Categoria { Id = 5, CategoriaPadre_id = null, Id_empresa = 1 };
            var cache = new List<DescuentoConfiguracion> { bajo, alto };
            var catCache = new Dictionary<int, Categoria> { { 5, categoria } };

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, null, 5, new List<int> { 1 }, true, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(alto.Id);
            result.Valor.Should().Be(20);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_Vencido_Excluido()
        {
            var expired = DescuentoConfiguracion.Crear(
                "Expired", 20, 1, idProducto: 10, aplicaCualquierMetodoPago: true,
                fechaHasta: DateTime.Now.AddDays(-1));

            var cache = new List<DescuentoConfiguracion> { expired };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 1 }, true, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_Inactivo_Excluido()
        {
            var disabled = DescuentoConfiguracion.Crear(
                "Disabled", 20, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            disabled.Inactivar();

            var cache = new List<DescuentoConfiguracion> { disabled };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 1 }, true, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_SinDescuento_ReturnsNull()
        {
            var cache = new List<DescuentoConfiguracion>();
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 1 }, true, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_Jerarquia_AplicaAncestro()
        {
            var ancestorDiscount = DescuentoConfiguracion.Crear(
                "Root 10%", 10, 1, idCategoria: 1, aplicaCualquierMetodoPago: true);

            var root = new Categoria { Id = 1, CategoriaPadre_id = null, Id_empresa = 1 };
            var child = new Categoria { Id = 2, CategoriaPadre_id = 1, Id_empresa = 1 };
            var grandchild = new Categoria { Id = 3, CategoriaPadre_id = 2, Id_empresa = 1 };

            var cache = new List<DescuentoConfiguracion> { ancestorDiscount };
            var catCache = new Dictionary<int, Categoria>
            {
                { 1, root }, { 2, child }, { 3, grandchild }
            };

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, null, 3, new List<int> { 1 }, true, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(ancestorDiscount.Id);
        }

        // ── Compuerta de condición de pago ──────────────────────────────────
        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_CualquierMetodo_AplicaEnPagoMixto()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "General 5%", 5, 1, idProducto: 10, aplicaCualquierMetodoPago: true);

            var cache = new List<DescuentoConfiguracion> { descuento };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 1, 2 }, false, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(5);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_MetodoEspecifico_PagoUnicoCoincide_Aplica()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            AgregarMetodoPago(descuento, 2);

            var cache = new List<DescuentoConfiguracion> { descuento };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 2 }, true, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(5);
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_MetodoEspecifico_PagoMixto_NoAplica()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            AgregarMetodoPago(descuento, 2);

            var cache = new List<DescuentoConfiguracion> { descuento };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 2, 3 }, false, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_MetodoEspecifico_MetodoFueraDeLista_NoAplica()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            AgregarMetodoPago(descuento, 2);

            var cache = new List<DescuentoConfiguracion> { descuento };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 3 }, true, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoAplicableAsync_MetodoEspecifico_UnMetodoDeLaLista_EnPagoUnico_Aplica()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Tajeta 5%", 5, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 1, 2 });
            AgregarMetodoPago(descuento, 1);
            AgregarMetodoPago(descuento, 2);

            var cache = new List<DescuentoConfiguracion> { descuento };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoAplicableAsync(
                1, 10, null, new List<int> { 2 }, true, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(5);
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerDescuentoProductoAsync (sin filtro de pago)
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_ProductoGanaACategoria()
        {
            var productDiscount = DescuentoConfiguracion.Crear(
                "Prod 20%", 20, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 10%", 10, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);

            var cache = new List<DescuentoConfiguracion> { productDiscount, categoryDiscount };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, 10, 5, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(productDiscount.Id);
            result.Valor.Should().Be(20);
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_CategoriaFallback_Aplica()
        {
            var categoryDiscount = DescuentoConfiguracion.Crear(
                "Cat 15%", 15, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);

            var categoria = new Categoria { Id = 5, CategoriaPadre_id = null, Id_empresa = 1 };
            var cache = new List<DescuentoConfiguracion> { categoryDiscount };
            var catCache = new Dictionary<int, Categoria> { { 5, categoria } };

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, 10, 5, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(15);
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_SinDescuento_ReturnsNull()
        {
            var cache = new List<DescuentoConfiguracion>();
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, 10, null, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_Jerarquia_AplicaAncestro()
        {
            var ancestorDiscount = DescuentoConfiguracion.Crear(
                "Root 10%", 10, 1, idCategoria: 1, aplicaCualquierMetodoPago: true);

            var root = new Categoria { Id = 1, CategoriaPadre_id = null, Id_empresa = 1 };
            var child = new Categoria { Id = 2, CategoriaPadre_id = 1, Id_empresa = 1 };
            var grandchild = new Categoria { Id = 3, CategoriaPadre_id = 2, Id_empresa = 1 };

            var cache = new List<DescuentoConfiguracion> { ancestorDiscount };
            var catCache = new Dictionary<int, Categoria>
            {
                { 1, root }, { 2, child }, { 3, grandchild }
            };

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, null, 3, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(ancestorDiscount.Id);
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_Vencido_Excluido()
        {
            var expired = DescuentoConfiguracion.Crear(
                "Expired", 20, 1, idProducto: 10, aplicaCualquierMetodoPago: true,
                fechaHasta: DateTime.Now.AddDays(-1));

            var cache = new List<DescuentoConfiguracion> { expired };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, 10, null, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_Inactivo_Excluido()
        {
            var disabled = DescuentoConfiguracion.Crear(
                "Disabled", 20, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            disabled.Inactivar();

            var cache = new List<DescuentoConfiguracion> { disabled };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, 10, null, cache, catCache);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_EmpateMayorValor_Gana()
        {
            var bajo = DescuentoConfiguracion.Crear(
                "Cat 10%", 10, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);
            var alto = DescuentoConfiguracion.Crear(
                "Cat 20%", 20, 1, idCategoria: 5, aplicaCualquierMetodoPago: true);

            var categoria = new Categoria { Id = 5, CategoriaPadre_id = null, Id_empresa = 1 };
            var cache = new List<DescuentoConfiguracion> { bajo, alto };
            var catCache = new Dictionary<int, Categoria> { { 5, categoria } };

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, null, 5, cache, catCache);

            result.Should().NotBeNull();
            result!.Id.Should().Be(alto.Id);
            result.Valor.Should().Be(20);
        }

        [Fact]
        public async Task ObtenerDescuentoProductoAsync_SinFiltroPago_SiempreAplica()
        {
            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            AgregarMetodoPago(descuento, 2);

            var cache = new List<DescuentoConfiguracion> { descuento };
            var catCache = new Dictionary<int, Categoria>();

            var result = await _servicio.ObtenerDescuentoProductoAsync(
                1, 10, null, cache, catCache);

            result.Should().NotBeNull();
            result!.Valor.Should().Be(5);
        }
    }
}