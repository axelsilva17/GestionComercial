using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.UI.ViewModels.Main;

namespace GestionComercial.Tests.UI
{
    public class ShellViewModelDescuentoTests
    {
        private async Task<ShellViewModel> CrearShellConPermisosAsync(string[] permisos)
        {
            var vm = new ShellViewModel();
            var sesion = new UsuarioSesionDto
            {
                Permisos = permisos.ToHashSet()
            };
            await vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", sesion);
            return vm;
        }

        [Fact]
        public async Task MostrarDescuentos_Gerente_ReturnsTrue()
        {
            var vm = await CrearShellConPermisosAsync(new[] { "Descuentos.Ver" });
            vm.Rol = RolUsuario.Gerente;

            vm.MostrarDescuentos.Should().BeTrue();
        }

        [Fact]
        public async Task MostrarDescuentos_Administrador_ReturnsTrue()
        {
            var vm = await CrearShellConPermisosAsync(new[] { "Descuentos.Ver" });
            vm.Rol = RolUsuario.Administrador;

            vm.MostrarDescuentos.Should().BeTrue();
        }

        [Fact]
        public async Task MostrarDescuentos_Vendedor_ReturnsFalse()
        {
            var vm = await CrearShellConPermisosAsync(new[] { "Ventas.Ver" });
            vm.Rol = RolUsuario.Vendedor;

            vm.MostrarDescuentos.Should().BeFalse();
        }

        [Fact]
        public async Task MostrarDescuentos_SinPermiso_ReturnsFalse()
        {
            var vm = await CrearShellConPermisosAsync(new string[0]);
            vm.Rol = RolUsuario.Gerente;

            vm.MostrarDescuentos.Should().BeFalse();
        }
    }
}
