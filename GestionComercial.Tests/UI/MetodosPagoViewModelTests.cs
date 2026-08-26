using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.UI.ViewModels.Configuracion;
using Moq;

namespace GestionComercial.Tests.UI
{
    public class MetodosPagoViewModelTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IMetodoPagoRepositorio> _mockMetodos = new();
        private readonly Mock<IEmpresaRepositorio> _mockEmpresas = new();

        public MetodosPagoViewModelTests()
        {
            _mockUow.Setup(u => u.MetodosPago).Returns(_mockMetodos.Object);
            _mockUow.Setup(u => u.Empresas).Returns(_mockEmpresas.Object);
        }

        private MetodosPagoViewModel CrearVM() => new(_mockUow.Object);

        [Fact]
        public void MostrarSubcategoria_EsTrue_CuandoCategoriaEsTarjeta()
        {
            var vm = CrearVM();
            vm.EditCategoria = "Tarjeta";
            vm.MostrarSubcategoria.Should().BeTrue();
        }

        [Theory]
        [InlineData("Efectivo")]
        [InlineData("Transferencia")]
        [InlineData("Otro")]
        public void MostrarSubcategoria_EsFalse_CuandoCategoriaNoEsTarjeta(string categoria)
        {
            var vm = CrearVM();
            vm.EditCategoria = categoria;
            vm.MostrarSubcategoria.Should().BeFalse();
        }

        [Fact]
        public void Guardar_TarjetaSinSubcategoria_MuestraError()
        {
            var vm = CrearVM();
            vm.NuevoMetodo();
            vm.EditNombre = "Mastercard";
            vm.EditCategoria = "Tarjeta";
            vm.EditSubcategoria = null;

            vm.Guardar().Wait();

            vm.TieneError.Should().BeTrue();
            vm.MensajeError.Should().Contain("categoría");
        }

        [Fact]
        public void Guardar_TarjetaConSubcategoria_GuardaCorrectamente()
        {
            _mockMetodos.Setup(r => r.AgregarAsync(It.IsAny<MetodoPago>()))
                .ReturnsAsync((MetodoPago m) => { m.Id = 10; return m; });
            _mockUow.Setup(u => u.GuardarCambiosAsync()).ReturnsAsync(1);
            _mockEmpresas.Setup(r => r.PrimerODefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Empresa, bool>>>()))
                .ReturnsAsync(new Empresa { Id = 1, Activo = true });

            var vm = CrearVM();
            vm.NuevoMetodo();
            vm.EditNombre = "Mastercard";
            vm.EditCategoria = "Tarjeta";
            vm.EditSubcategoria = "Credito";

            vm.Guardar().Wait();

            vm.TieneError.Should().BeFalse();
            vm.PanelVisible.Should().BeFalse();
            _mockMetodos.Verify(r => r.AgregarAsync(It.Is<MetodoPago>(
                m => m.Subcategoria == "Credito" && m.Categoria == "Tarjeta")), Times.Once);
        }

        [Fact]
        public void Guardar_Efectivo_LimpiaSubcategoriaANull()
        {
            _mockMetodos.Setup(r => r.AgregarAsync(It.IsAny<MetodoPago>()))
                .ReturnsAsync((MetodoPago m) => { m.Id = 11; return m; });
            _mockUow.Setup(u => u.GuardarCambiosAsync()).ReturnsAsync(1);
            _mockEmpresas.Setup(r => r.PrimerODefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Empresa, bool>>>()))
                .ReturnsAsync(new Empresa { Id = 1, Activo = true });

            var vm = CrearVM();
            vm.NuevoMetodo();
            vm.EditNombre = "Efectivo";
            vm.EditCategoria = "Efectivo";
            vm.EditSubcategoria = "Debito"; // should be cleared

            vm.Guardar().Wait();

            vm.TieneError.Should().BeFalse();
            _mockMetodos.Verify(r => r.AgregarAsync(It.Is<MetodoPago>(
                m => m.Subcategoria == null && m.Categoria == "Efectivo")), Times.Once);
        }

        [Fact]
        public void Guardar_Editar_LimpiaSubcategoriaAlCambiarANoTarjeta()
        {
            var existing = new MetodoPago
            {
                Id = 5,
                Nombre = "Visa",
                Categoria = "Tarjeta",
                Subcategoria = "Credito",
                Activo = true,
                Id_empresa = 1
            };
            _mockMetodos.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(existing);
            _mockUow.Setup(u => u.GuardarCambiosAsync()).ReturnsAsync(1);

            var vm = CrearVM();
            vm.Editar(new MetodoPagoDto
            {
                IdMetodoPago = 5,
                Nombre = "Visa",
                Categoria = "Tarjeta",
                Subcategoria = "Credito",
                IdEmpresa = 1
            });
            vm.EditCategoria = "Transferencia"; // changed to non-Tarjeta

            vm.Guardar().Wait();

            _mockMetodos.Verify(r => r.Actualizar(It.Is<MetodoPago>(
                m => m.Subcategoria == null)), Times.Once);
        }

        [Fact]
        public void NuevoMetodo_ReseteaSubcategoria()
        {
            var vm = CrearVM();
            vm.EditSubcategoria = "Credito";

            vm.NuevoMetodo();

            vm.EditSubcategoria.Should().BeNull();
        }

        [Fact]
        public async Task CargarAsync_MapeaSubcategoria()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
                new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null, Activo = true, Id_empresa = 1 }
            };
            _mockMetodos.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(metodos);

            var vm = CrearVM();
            await vm.CargarAsync();

            vm.Items.Should().HaveCount(2);
            vm.Items.Should().Contain(i => i.Nombre == "Débito" && i.Subcategoria == "Debito");
            vm.Items.Should().Contain(i => i.Nombre == "Efectivo" && i.Subcategoria == null);
        }
    }
}
