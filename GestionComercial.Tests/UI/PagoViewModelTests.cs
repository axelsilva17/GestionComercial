using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.UI.ViewModels.Ventas;
using Moq;
using System.Windows.Input;

namespace GestionComercial.Tests.UI
{
    public class PagoViewModelTests
    {
        private readonly Mock<IVentaServicio> _mockVentaServicio = new();
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IMetodoPagoRepositorio> _mockMetodos = new();
        private readonly Mock<ISucursalRepositorio> _mockSucursales = new();
        private readonly SesionServicio _sesion;

        public PagoViewModelTests()
        {
            _mockUow.Setup(u => u.MetodosPago).Returns(_mockMetodos.Object);
            _mockUow.Setup(u => u.Sucursales).Returns(_mockSucursales.Object);
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

        private PagoViewModel CrearVM()
        {
            return new PagoViewModel(_mockVentaServicio.Object, _mockUow.Object, _sesion);
        }

        private async Task<PagoViewModel> CrearVMConMetodosAsync(List<MetodoPago> metodos)
        {
            var sucursal = new Sucursal { Id = 1, Id_empresa = 1 };
            _mockSucursales.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(sucursal);
            _mockMetodos.Setup(r => r.ObtenerTodosPorEmpresaAsync(1)).ReturnsAsync(metodos);

            var vm = CrearVM();
            vm.InicializarConVenta(1, "Test", 1000);
            // Use reflection to call the private CargarMetodosAsync
            var method = typeof(PagoViewModel).GetMethod("CargarMetodosAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)method!.Invoke(vm, null)!;
            return vm;
        }

        [Fact]
        public async Task CargarMetodos_PopulaSubcategoriaEnDto()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
                new() { Id = 3, Nombre = "Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);

            vm.MetodosPago.Should().Contain(m => m.NombreMetodo == "Débito" && m.Subcategoria == "Debito");
            vm.MetodosPago.Should().Contain(m => m.NombreMetodo == "Crédito" && m.Subcategoria == "Credito");
        }

        [Fact]
        public async Task AgregarDebito_PrimeraActivaConSubcategoriaDebito()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 5, Nombre = "Mastercard Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
                new() { Id = 6, Nombre = "Visa Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);

            vm.Pagos.Should().HaveCount(1);
            vm.Pagos.First().IdMetodoPago.Should().Be(5);
        }

        [Fact]
        public async Task AgregarCredito_PrimeraActivaConSubcategoriaCredito()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 7, Nombre = "Naranja", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
                new() { Id = 8, Nombre = "Mastercard Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);
            vm.HandleKeyDown(Key.F3, ModifierKeys.None);

            vm.Pagos.Should().HaveCount(1);
            vm.Pagos.First().IdMetodoPago.Should().Be(7);
        }

        [Fact]
        public async Task AgregarDebito_SinMatch_NoAgregaLinea_YMuestraError()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 3, Nombre = "Crédito", Categoria = "Tarjeta", Subcategoria = "Credito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);

            vm.Pagos.Should().BeEmpty();
            vm.TieneError.Should().BeTrue();
            vm.MensajeError.Should().Contain("débito");
        }

        [Fact]
        public async Task AgregarCredito_SinMatch_NoAgregaLinea_YMuestraError()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = true, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);
            vm.HandleKeyDown(Key.F3, ModifierKeys.None);

            vm.Pagos.Should().BeEmpty();
            vm.TieneError.Should().BeTrue();
            vm.MensajeError.Should().Contain("crédito");
        }

        [Fact]
        public async Task AgregarDebito_SoloMetodosActivos()
        {
            var metodos = new List<MetodoPago>
            {
                new() { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito", Activo = false, Id_empresa = 1 },
            };

            var vm = await CrearVMConMetodosAsync(metodos);
            vm.HandleKeyDown(Key.F2, ModifierKeys.None);

            vm.Pagos.Should().BeEmpty();
            vm.TieneError.Should().BeTrue();
        }
    }
}
