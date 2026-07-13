using FluentAssertions;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class AutenticacionServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();
        private readonly Mock<IUsuarioRepositorio> _mockUsuarioRepo = new();
        private readonly AutenticacionServicio _servicio;

        public AutenticacionServicioTests()
        {
            _mockUow.Setup(u => u.Usuarios).Returns(_mockUsuarioRepo.Object);
            _servicio = new AutenticacionServicio(_mockUow.Object, _mockPasswordHasher.Object);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Usuario existe, password correcto
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_UsuarioValido_DevuelveSesion()
        {
            var usuario = CrearUsuarioAdmin();
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("admin2026", usuario.PasswordHash))
                .Returns(true);

            var resultado = await _servicio.LoginAsync("admin@miempresa.com", "admin2026");

            resultado.Should().NotBeNull();
            resultado!.Email.Should().Be("admin@miempresa.com");
            resultado.Nombre.Should().Be("Admin");
            resultado.Rol.Should().Be("Administrador");
            resultado.IdSucursal.Should().Be(1);
            resultado.Sucursal.Should().Be("Principal");
            resultado.IdEmpresa.Should().Be(1);
            resultado.Empresa.Should().Be("Mi Empresa");

            // Verificar que se actualizó el último acceso
            _mockUsuarioRepo.Verify(r => r.Actualizar(It.Is<Usuario>(u => u.UltimoAcceso != null)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Email no existe
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_EmailNoExiste_DevuelveNull()
        {
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Usuario?)null);

            var resultado = await _servicio.LoginAsync("nadie@mail.com", "pass123");

            resultado.Should().BeNull();
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Password incorrecto
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_PasswordIncorrecto_DevuelveNull()
        {
            var usuario = CrearUsuarioAdmin();
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("wrongpass", usuario.PasswordHash))
                .Returns(false);

            var resultado = await _servicio.LoginAsync("admin@miempresa.com", "wrongpass");

            resultado.Should().BeNull();
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Email con diferentes capitalizaciones
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_EmailMayusculas_DevuelveSesion()
        {
            var usuario = CrearUsuarioAdmin();
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("ADMIN@MIEMPRESA.COM"))
                .ReturnsAsync(usuario);
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("admin2026", usuario.PasswordHash))
                .Returns(true);

            var resultado = await _servicio.LoginAsync("ADMIN@MIEMPRESA.COM", "admin2026");

            resultado.Should().NotBeNull();
        }

        // ═══════════════════════════════════════════════════════════
        // HashPassword
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void HashPassword_DelegaEnPasswordHasher()
        {
            _mockPasswordHasher
                .Setup(h => h.HashPassword("miPassword"))
                .Returns("hash_del_password");

            var hash = _servicio.HashPassword("miPassword");

            hash.Should().Be("hash_del_password");
            _mockPasswordHasher.Verify(h => h.HashPassword("miPassword"), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════

        private static Usuario CrearUsuarioAdmin()
        {
            var usuario = Usuario.Crear(
                nombre: "Admin",
                apellido: "Sistema",
                email: "admin@miempresa.com",
                passwordHash: "$2a$12$hash_fijo_para_testing",
                idSucursal: 1,
                idRol: 2);

            // Setear propiedades de navegación via reflection o setter directo
            usuario.GetType().GetProperty("Rol")!.SetValue(usuario, new Rol
            {
                Id = 2,
                Nombre = "Administrador"
            });

            usuario.GetType().GetProperty("Sucursal")!.SetValue(usuario, new Sucursal
            {
                Id = 1,
                Nombre = "Principal",
                Id_empresa = 1,
                Empresa = new Empresa
                {
                    Id = 1,
                    Nombre = "Mi Empresa"
                }
            });

            // Asignar Id via reflection (simula persistencia)
            usuario.GetType().GetProperty("Id")!.SetValue(usuario, 1);

            return usuario;
        }
    }
}
