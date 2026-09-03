using Caliburn.Micro;
using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Configuracion;
using Moq;

namespace GestionComercial.Tests.UI
{
    public class DescuentoFormularioInnerViewModelTests
    {
        private readonly Mock<IDescuentoConfiguracionServicio> _mockServicio = new();
        private readonly Mock<IProductoServicio> _mockProductoServicio = new();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly Mock<IMetodoPagoRepositorio> _mockMetodos = new();
        private readonly Mock<IEventAggregator> _mockEventAggregator = new();
        private readonly SesionServicio _sesion;

        public DescuentoFormularioInnerViewModelTests()
        {
            _mockUnitOfWork.Setup(u => u.MetodosPago).Returns(_mockMetodos.Object);
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

        private DescuentoFormularioInnerViewModel CrearVM()
        {
            return new DescuentoFormularioInnerViewModel(
                _mockServicio.Object,
                _mockProductoServicio.Object,
                _mockUnitOfWork.Object,
                _sesion,
                _mockEventAggregator.Object);
        }

        private void SetupCargarAsyncBasico()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Visa", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
                new() { Id = 3, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };
            _mockMetodos.Setup(r => r.ObtenerTodosPorEmpresaAsync(1)).ReturnsAsync(metodos);
            _mockProductoServicio.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool>())).ReturnsAsync(new List<ProductoListadoDto>
            {
                new() { IdProducto = 10, Nombre = "Leche", CodigoBarra = "123", PrecioVentaActual = 100, StockActual = 50 },
            });
            _mockProductoServicio.Setup(s => s.ObtenerCategoriasAsync(1)).ReturnsAsync(new List<CategoriaItemDto>
            {
                new() { IdCategoria = 5, Nombre = "Lácteos" },
            });
        }

        // ── Fix 3a: GuardarAsync en modo EDICIÓN preserva Nombre ingresado por usuario ──
        [Fact]
        public async Task GuardarAsync_Edicion_PreservaNombreUsuario()
        {
            SetupCargarAsyncBasico();

            var descuentoExistente = DescuentoConfiguracion.Crear(
                "Nombre Original 10%", 10, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            descuentoExistente.Id = 1; // Set explicit ID for testing
            _mockServicio.Setup(s => s.ObtenerPorIdAsync(1))
                .ReturnsAsync(descuentoExistente);

            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 1;
            await vm.CargarAsync();

            // Usuario edita el nombre en el modal
            vm.Nombre = "Mi Nombre Personalizado 15%";
            vm.Valor = 15;

            await vm.GuardarAsync();

            // Verificar que se llamó ActualizarAsync con el nombre que ingresó el usuario
            _mockServicio.Verify(s => s.ActualizarAsync(
                1,
                "Mi Nombre Personalizado 15%", // Nombre preservado
                15m,
                10, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                AlcanceDescuentoEnum.Producto, It.IsAny<decimal?>()),
                Times.Once);
        }

        [Fact]
        public async Task GuardarAsync_Edicion_NombreVacio_RegeneraNombre()
        {
            SetupCargarAsyncBasico();

            var descuentoExistente = DescuentoConfiguracion.Crear(
                "Nombre Original 10%", 10, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            descuentoExistente.Id = 2;
            _mockServicio.Setup(s => s.ObtenerPorIdAsync(2))
                .ReturnsAsync(descuentoExistente);

            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 2;
            await vm.CargarAsync();

            // Usuario deja el nombre vacío
            vm.Nombre = "";
            vm.Valor = 20;

            await vm.GuardarAsync();

            // Debe regenerar el nombre (Leche 20%)
            _mockServicio.Verify(s => s.ActualizarAsync(
                2,
                "Leche 20%", // Nombre regenerado
                20m,
                10, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                AlcanceDescuentoEnum.Producto, It.IsAny<decimal?>()),
                Times.Once);
        }

        // ── Fix 3a: GuardarAsync en modo CREACIÓN genera nombre automáticamente ──
        [Fact]
        public async Task GuardarAsync_Creacion_GeneraNombreAutomatico()
        {
            SetupCargarAsyncBasico();

            var vm = CrearVM();
            vm.EsModoEdicion = false;
            vm.DescuentoId = 0;
            await vm.CargarAsync();

            vm.ProductoNombre = "Leche";
            vm.IdProducto = 10;
            vm.Valor = 10;

            await vm.GuardarAsync();

            // En creación siempre genera el nombre
            _mockServicio.Verify(s => s.CrearAsync(
                It.IsAny<int>(), "Leche 10%", 10m, 10, null,
                It.IsAny<bool>(), It.IsAny<List<int>?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                AlcanceDescuentoEnum.Producto, It.IsAny<decimal?>()),
                Times.Once);
        }

        // ── Fix 3b: CargarDescuentoAsync carga Nombre y respeta AplicaCualquierMetodoPago ──
        [Fact]
        public async Task CargarDescuentoAsync_CargaNombreDesdeEntidad()
        {
            SetupCargarAsyncBasico();

            var descuentoExistente = DescuentoConfiguracion.Crear(
                "Mi Descuento Guardado", 15, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            descuentoExistente.Id = 3;
            descuentoExistente.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockServicio.Setup(s => s.ObtenerPorIdAsync(3))
                .ReturnsAsync(descuentoExistente);

            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 3;
            await vm.CargarAsync();

            // Verificar que el nombre se cargó desde la entidad
            vm.Nombre.Should().Be("Mi Descuento Guardado");
        }

        [Fact]
        public async Task CargarDescuentoAsync_RespetaAplicaCualquierMetodoPago_MetodoPagoScope()
        {
            SetupCargarAsyncBasico();

            // Descuento de scope "Método de Pago" con AplicaCualquierMetodoPago = false
            var descuentoExistente = DescuentoConfiguracion.Crear(
                "Visa 5%", 5, 1,
                idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 },
                alcance: AlcanceDescuentoEnum.MetodoPago);
            descuentoExistente.Id = 4;
            descuentoExistente.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockServicio.Setup(s => s.ObtenerPorIdAsync(4))
                .ReturnsAsync(descuentoExistente);

            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 4;
            await vm.CargarAsync();

            // Verificar que el scope es "Método de Pago"
            vm.AmbitoSeleccionado.Should().Be("Método de Pago");
            // Verificar que AplicaCualquierMetodoPago se respeta (false) y NO se sobrescribe por el setter de AmbitoSeleccionado
            vm.AplicaCualquierMetodoPago.Should().BeFalse();
            // Verificar que el selector de métodos se muestra
            vm.MuestraSelectorMetodosPago.Should().BeTrue();
            // Verificar que el método correcto está tildado
            vm.MetodosPagoDisponibles.Should().Contain(m => m.MetodoPago.Id == 2 && m.EstaSeleccionado);
        }

        [Fact]
        public async Task CargarDescuentoAsync_RespetaAplicaCualquierMetodoPago_ProductoScope()
        {
            SetupCargarAsyncBasico();

            // Descuento de scope "Producto" con AplicaCualquierMetodoPago = false (restringe a métodos específicos)
            var descuentoExistente = DescuentoConfiguracion.Crear(
                "Leche 10% solo Débito", 10, 1, idProducto: 10, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 3 }); // Débito
            descuentoExistente.Id = 5;
            descuentoExistente.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 3 });

            _mockServicio.Setup(s => s.ObtenerPorIdAsync(5))
                .ReturnsAsync(descuentoExistente);

            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 5;
            await vm.CargarAsync();

            // Verificar que el scope es "Producto"
            vm.AmbitoSeleccionado.Should().Be("Producto");
            // Verificar que AplicaCualquierMetodoPago se respeta (false) - NO se sobrescribe a true por el setter
            vm.AplicaCualquierMetodoPago.Should().BeFalse();
            // Verificar que el selector de métodos se muestra (porque AplicaCualquierMetodoPago = false)
            vm.MuestraSelectorMetodosPago.Should().BeTrue();
            // Verificar que el método correcto está tildado
            vm.MetodosPagoDisponibles.Should().Contain(m => m.MetodoPago.Id == 3 && m.EstaSeleccionado);
        }

        [Fact]
        public async Task CargarDescuentoAsync_ProductoScope_AplicaCualquierTrue_NoMuestraSelectorMetodos()
        {
            SetupCargarAsyncBasico();

            var descuentoExistente = DescuentoConfiguracion.Crear(
                "Leche 10% cualquier método", 10, 1, idProducto: 10, aplicaCualquierMetodoPago: true);
            descuentoExistente.Id = 6;

            _mockServicio.Setup(s => s.ObtenerPorIdAsync(6))
                .ReturnsAsync(descuentoExistente);

            var vm = CrearVM();
            vm.EsModoEdicion = true;
            vm.DescuentoId = 6;
            await vm.CargarAsync();

            vm.AmbitoSeleccionado.Should().Be("Producto");
            vm.AplicaCualquierMetodoPago.Should().BeTrue();
            vm.MuestraSelectorMetodosPago.Should().BeFalse();
        }
    }
}