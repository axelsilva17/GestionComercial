using FluentAssertions;
using GestionComercial.Aplicacion.Excepciones;
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
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPermisosAsync(usuario.Id))
                .ReturnsAsync(new[] { "Ventas.Ver", "Productos.Ver", "Usuarios.Gestionar" });
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
            resultado.Permisos.Should().Contain(new[] { "Ventas.Ver", "Productos.Ver", "Usuarios.Gestionar" });

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
        // LoginAsync - Password incorrecto (P1: brute force)
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_PasswordIncorrecto_LanzaNegocioException()
        {
            var usuario = CrearUsuarioAdmin();
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("wrongpass", usuario.PasswordHash))
                .Returns(false);

            var act = () => _servicio.LoginAsync("admin@miempresa.com", "wrongpass");

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("*incorrectos*");

            _mockUsuarioRepo.Verify(r => r.Actualizar(It.IsAny<Usuario>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Password incorrecto registra intento fallido
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_PasswordIncorrecto_RegistraAccesoFallido()
        {
            var usuario = CrearUsuarioAdmin();
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("wrongpass", usuario.PasswordHash))
                .Returns(false);

            await _servicio.Invoking(s => s.LoginAsync("admin@miempresa.com", "wrongpass"))
                .Should().ThrowAsync<NegocioException>();

            usuario.IntentosFallidos.Should().Be(1);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Password correcto registra acceso exitoso
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_PasswordCorrecto_RegistraAccesoExitoso()
        {
            var usuario = CrearUsuarioAdmin();
            usuario.IntentosFallidos = 2;
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPermisosAsync(usuario.Id))
                .ReturnsAsync(new[] { "Ventas.Ver" });
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("admin2026", usuario.PasswordHash))
                .Returns(true);

            var resultado = await _servicio.LoginAsync("admin@miempresa.com", "admin2026");

            resultado.Should().NotBeNull();
            usuario.IntentosFallidos.Should().Be(0);
            usuario.BloqueadoHasta.Should().BeNull();
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - Usuario bloqueado no verifica password
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_UsuarioBloqueado_LanzaExcepcionSinVerificarPassword()
        {
            var usuario = CrearUsuarioAdmin();
            usuario.RegistrarAccesoFallido(maxIntentos: 3);
            usuario.RegistrarAccesoFallido(maxIntentos: 3);
            usuario.RegistrarAccesoFallido(maxIntentos: 3);
            //此时 usuario.EstaBloqueado == true

            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);

            var act = () => _servicio.LoginAsync("admin@miempresa.com", "admin2026");

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("*bloqueado*");

            _mockPasswordHasher.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ═══════════════════════════════════════════════════════════
        // LoginAsync - 3 fallos consecutivos bloquea el usuario
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task LoginAsync_TresFallosConsecutivos_BloqueaUsuario()
        {
            var usuario = CrearUsuarioAdmin();
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorEmailAsync("admin@miempresa.com"))
                .ReturnsAsync(usuario);
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword(It.IsAny<string>(), usuario.PasswordHash))
                .Returns(false);

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    await _servicio.LoginAsync("admin@miempresa.com", "wrong");
                }
                catch (NegocioException) { }
            }

            usuario.EstaBloqueado.Should().BeTrue();
            usuario.IntentosFallidos.Should().Be(3);
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
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPermisosAsync(usuario.Id))
                .ReturnsAsync(new[] { "Ventas.Ver" });
            _mockPasswordHasher
                .Setup(h => h.VerifyPassword("admin2026", usuario.PasswordHash))
                .Returns(true);

            var resultado = await _servicio.LoginAsync("ADMIN@MIEMPRESA.COM", "admin2026");

            resultado.Should().NotBeNull();
            resultado!.Permisos.Should().Contain("Ventas.Ver");
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
