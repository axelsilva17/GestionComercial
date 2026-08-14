using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.UI.ViewModels.Main;
using Moq;

namespace GestionComercial.Tests.UI
{
    public class ShellViewModelDescuentoTests
    {
        private ShellViewModel CrearShellConPermisos(string[] permisos)
        {
            var vm = new ShellViewModel();
            var sesion = new UsuarioSesionDto
            {
                Permisos = permisos.ToHashSet()
            };
            vm.ConfigurarSesion("Test", "vendedor", "Sucursal1", sesion);
            return vm;
        }

        [Fact]
        public void MostrarDescuentos_Gerente_ReturnsTrue()
        {
            var vm = CrearShellConPermisos(new[] { "Descuentos.Ver" });
            vm.Rol = RolUsuario.Gerente;

            vm.MostrarDescuentos.Should().BeTrue();
        }

        [Fact]
        public void MostrarDescuentos_Administrador_ReturnsTrue()
        {
            var vm = CrearShellConPermisos(new[] { "Descuentos.Ver" });
            vm.Rol = RolUsuario.Administrador;

            vm.MostrarDescuentos.Should().BeTrue();
        }

        [Fact]
        public void MostrarDescuentos_Vendedor_ReturnsFalse()
        {
            var vm = CrearShellConPermisos(new[] { "Ventas.Ver" });
            vm.Rol = RolUsuario.Vendedor;

            vm.MostrarDescuentos.Should().BeFalse();
        }

        [Fact]
        public void MostrarDescuentos_SinPermiso_ReturnsFalse()
        {
            var vm = CrearShellConPermisos(new string[0]);
            vm.Rol = RolUsuario.Gerente;

            vm.MostrarDescuentos.Should().BeFalse();
        }
    }
}
