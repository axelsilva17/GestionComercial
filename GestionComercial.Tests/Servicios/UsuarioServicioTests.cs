using FluentAssertions;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class UsuarioServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IUsuarioRepositorio> _mockUsuarioRepo = new();
        private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();
        private readonly UsuarioServicio _servicio;

        public UsuarioServicioTests()
        {
            _mockUow.Setup(u => u.Usuarios).Returns(_mockUsuarioRepo.Object);
            _servicio = new UsuarioServicio(_mockUow.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task ObtenerTodosAsync_ConUsuarios_DevuelveDtos()
        {
            var usuarios = new List<Usuario>
            {
                Usuario.Crear("Admin", "Sistema", "admin@mail.com", "hash", 1, 2),
                Usuario.Crear("Vendedor", "Uno", "ven@mail.com", "hash", 1, 3),
            };
            usuarios[0].GetType().GetProperty("Rol")!.SetValue(usuarios[0], new Rol { Id = 2, Nombre = "Administrador" });
            usuarios[1].GetType().GetProperty("Rol")!.SetValue(usuarios[1], new Rol { Id = 3, Nombre = "Vendedor" });

            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorSucursalAsync(1))
                .ReturnsAsync(usuarios);

            var resultado = await _servicio.ObtenerTodosAsync(1);

            resultado.Should().HaveCount(2);
            resultado.Should().Contain(u => u.Rol == "Administrador");
        }

        [Fact]
        public async Task ObtenerPorIdAsync_UsuarioExistente_DevuelveDto()
        {
            var usuario = Usuario.Crear("Test", "User", "test@mail.com", "hash", 1, 2);
            usuario.GetType().GetProperty("Id")!.SetValue(usuario, 1);
            usuario.GetType().GetProperty("Rol")!.SetValue(usuario, new Rol { Nombre = "Vendedor" });

            _mockUsuarioRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);

            var resultado = await _servicio.ObtenerPorIdAsync(1);

            resultado.Should().NotBeNull();
            resultado!.Email.Should().Be("test@mail.com");
        }

        [Fact]
        public async Task CrearAsync_EmailUnico_CreaUsuario()
        {
            _mockUsuarioRepo
                .Setup(r => r.ExisteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(false);

            _mockUsuarioRepo
                .Setup(r => r.AgregarAsync(It.IsAny<Usuario>()))
                .Returns<Usuario>(u =>
                {
                    typeof(Usuario).GetProperty("Id")!.SetValue(u, 1);
                    return Task.FromResult(u);
                });

            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(() =>
                {
                    var u = Usuario.Crear("Nuevo", "User", "nuevo@mail.com", "hash", 1, 2);
                    u.GetType().GetProperty("Id")!.SetValue(u, 1);
                    u.GetType().GetProperty("Rol")!.SetValue(u, new Rol { Nombre = "Vendedor" });
                    return u;
                });

            _mockPasswordHasher
                .Setup(h => h.HashPassword("pass123"))
                .Returns("hash_del_password");

            var resultado = await _servicio.CrearAsync("Nuevo", "User", "nuevo@mail.com", "pass123", 2, 1);

            resultado.Should().NotBeNull();
            _mockUsuarioRepo.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task CrearAsync_EmailDuplicado_LanzaExcepcion()
        {
            _mockUsuarioRepo
                .Setup(r => r.ExisteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(true);

            var act = () => _servicio.CrearAsync("Test", "User", "existente@mail.com", "pass", 2, 1);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*email*ya está en uso*");
        }

        [Fact]
        public async Task CambiarPasswordAsync_PasswordCorrecto_Actualiza()
        {
            var usuario = Usuario.Crear("Test", "User", "test@mail.com", "hash_viejo", 1, 2);
            usuario.GetType().GetProperty("Id")!.SetValue(usuario, 1);

            _mockUsuarioRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);
            _mockPasswordHasher.Setup(h => h.VerifyPassword("pass_viejo", "hash_viejo")).Returns(true);
            _mockPasswordHasher.Setup(h => h.HashPassword("pass_nuevo")).Returns("hash_nuevo");

            await _servicio.CambiarPasswordAsync(1, "pass_viejo", "pass_nuevo");

            _mockUsuarioRepo.Verify(r => r.Actualizar(It.Is<Usuario>(u => u.PasswordHash == "hash_nuevo")), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task CambiarPasswordAsync_PasswordIncorrecto_LanzaUnauthorized()
        {
            var usuario = Usuario.Crear("Test", "User", "test@mail.com", "hash_real", 1, 2);
            usuario.GetType().GetProperty("Id")!.SetValue(usuario, 1);

            _mockUsuarioRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);
            _mockPasswordHasher.Setup(h => h.VerifyPassword("wrong", "hash_real")).Returns(false);

            var act = () => _servicio.CambiarPasswordAsync(1, "wrong", "nuevo");

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("*Contraseña actual incorrecta*");
        }

        [Fact]
        public async Task DesactivarAsync_UsuarioExistente_Inactiva()
        {
            var usuario = Usuario.Crear("Test", "User", "test@mail.com", "hash", 1, 2);
            usuario.GetType().GetProperty("Id")!.SetValue(usuario, 1);

            _mockUsuarioRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);

            await _servicio.DesactivarAsync(1);

            usuario.Activo.Should().BeFalse();
            _mockUsuarioRepo.Verify(r => r.Actualizar(usuario), Times.Once);
        }
    }
}
