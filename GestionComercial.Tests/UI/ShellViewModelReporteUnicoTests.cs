using Caliburn.Micro;
using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.ViewModels.Reportes;
using Moq;

namespace GestionComercial.Tests.UI
{
    public class ShellViewModelReporteUnicoTests : IDisposable
    {
        private readonly Mock<IUsuarioRepositorio> _mockUsuarioRepo = new();
        private readonly SimpleContainer _container;

        public ShellViewModelReporteUnicoTests()
        {
            _container = new SimpleContainer();
            _container.Instance<IUsuarioRepositorio>(_mockUsuarioRepo.Object);
            _container.PerRequest<ReporteGerenciaViewModel>();
            _container.PerRequest<ReporteAdminViewModel>();

            IoC.GetInstance = (type, key) => _container.GetInstance(type, key);
            IoC.GetAllInstances = (type) => _container.GetAllInstances(type);
        }

        public void Dispose()
        {
            IoC.GetInstance = null!;
            IoC.GetAllInstances = null!;
        }

        private ShellViewModel CrearShell()
        {
            var vm = new ShellViewModel();
            var sesion = new UsuarioSesionDto
            {
                Permisos = new HashSet<string> { "Reportes.Ver" }
            };
            vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", sesion);
            return vm;
        }

        private async Task<(ShellViewModel vm, int count)> SetupShellAsync(string rol, int usuarioCount, string[]? permisos = null)
        {
            _mockUsuarioRepo.Setup(r => r.ContarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(usuarioCount);

            var vm = new ShellViewModel();
            var sesion = new UsuarioSesionDto
            {
                Permisos = permisos?.ToHashSet() ?? new HashSet<string>()
            };
            await vm.ConfigurarSesion("Test", rol, "Sucursal1", sesion);
            return (vm, usuarioCount);
        }

        // ── T1: EsUsuarioUnico — ContarAsync results ──────────────────────

        [Fact]
        public async Task ConfigurarSesion_ContarAsyncReturns1_EsUsuarioUnicoTrue()
        {
            _mockUsuarioRepo.Setup(r => r.ContarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(1);

            var vm = CrearShell();
            await vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", vm.SesionActual);

            vm.EsUsuarioUnico.Should().BeTrue();
        }

        [Fact]
        public async Task ConfigurarSesion_ContarAsyncReturns2_EsUsuarioUnicoFalse()
        {
            _mockUsuarioRepo.Setup(r => r.ContarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(2);

            var vm = CrearShell();
            await vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", vm.SesionActual);

            vm.EsUsuarioUnico.Should().BeFalse();
        }

        [Fact]
        public async Task ConfigurarSesion_ContarAsyncReturns0_EsUsuarioUnicoFalse()
        {
            _mockUsuarioRepo.Setup(r => r.ContarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(0);

            var vm = CrearShell();
            await vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", vm.SesionActual);

            vm.EsUsuarioUnico.Should().BeFalse();
        }

        [Fact]
        public async Task ConfigurarSesion_ContarAsyncThrows_EsUsuarioUnicoFalse()
        {
            _mockUsuarioRepo.Setup(r => r.ContarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ThrowsAsync(new Exception("DB connection failed"));

            var vm = CrearShell();
            await vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", vm.SesionActual);

            vm.EsUsuarioUnico.Should().BeFalse();
        }

        [Fact]
        public void EsUsuarioUnico_DefaultIsFalse()
        {
            var vm = new ShellViewModel();
            vm.EsUsuarioUnico.Should().BeFalse();
        }

        // ── T2: IrReportes branching ──────────────────────────────────────

        [Fact]
        public async Task IrReportes_AdminMulti_ShowsReporteAdmin()
        {
            var (vm, _) = await SetupShellAsync("administrador", 2);

            await vm.IrReportes();

            vm.ActiveItem.Should().BeOfType<ReporteAdminViewModel>();
        }

        [Fact]
        public async Task IrReportes_GerenteMulti_ShowsReporteGerencia()
        {
            var (vm, _) = await SetupShellAsync("gerente", 2);

            await vm.IrReportes();

            vm.ActiveItem.Should().BeOfType<ReporteGerenciaViewModel>();
        }

        [Fact]
        public async Task IrReportes_AdminUnico_ShowsReporteGerencia()
        {
            var (vm, _) = await SetupShellAsync("administrador", 1);

            await vm.IrReportes();

            vm.ActiveItem.Should().BeOfType<ReporteGerenciaViewModel>();
        }

        [Fact]
        public async Task IrReportes_GerenteUnico_ShowsReporteGerencia()
        {
            var (vm, _) = await SetupShellAsync("gerente", 1);

            await vm.IrReportes();

            vm.ActiveItem.Should().BeOfType<ReporteGerenciaViewModel>();
        }

        [Fact]
        public async Task IrReportes_VendedorUnicoConPermiso_ShowsReporteGerencia()
        {
            var (vm, _) = await SetupShellAsync("vendedor", 1, new[] { "Reportes.Ver" });

            await vm.IrReportes();

            vm.ActiveItem.Should().BeOfType<ReporteGerenciaViewModel>();
        }

        [Fact]
        public async Task IrReportes_VendedorMultiConPermiso_ShowsReporteAdmin()
        {
            var (vm, _) = await SetupShellAsync("vendedor", 2, new[] { "Reportes.Ver" });

            await vm.IrReportes();

            vm.ActiveItem.Should().BeOfType<ReporteAdminViewModel>();
        }

        [Fact]
        public async Task IrReportes_VendedorSinPermiso_MenuReportesNotVisible()
        {
            var (vm, _) = await SetupShellAsync("vendedor", 1, new[] { "Ventas.Ver" });

            vm.MostrarReportes.Should().BeFalse();
        }
    }
}
