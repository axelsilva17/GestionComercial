using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class ClienteServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IClienteRepositorio> _mockClienteRepo = new();
        private readonly ClienteServicio _servicio;

        public ClienteServicioTests()
        {
            _mockUow.Setup(u => u.Clientes).Returns(_mockClienteRepo.Object);
            _servicio = new ClienteServicio(_mockUow.Object);
        }

        [Fact]
        public async Task ObtenerTodosAsync_ConClientes_DevuelveDtos()
        {
            var clientes = new List<Cliente>
            {
                new() { Id = 1, Nombre = "Cliente A", Documento = 123, Telefono = "3794000000", Email = "a@mail.com", Activo = true, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Cliente B", Documento = 456, Telefono = "3794111111", Email = "b@mail.com", Activo = true, Id_empresa = 1 },
            };
            _mockClienteRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1))
                .ReturnsAsync(clientes);

            var resultado = await _servicio.ObtenerTodosAsync(1);

            resultado.Should().HaveCount(2);
            resultado.Should().Contain(c => c.Nombre == "Cliente A");
        }

        [Fact]
        public async Task ObtenerTodosAsync_SinClientes_DevuelveVacio()
        {
            _mockClienteRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1))
                .ReturnsAsync(new List<Cliente>());

            var resultado = await _servicio.ObtenerTodosAsync(1);

            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task ObtenerPorIdAsync_ClienteExistente_DevuelveDto()
        {
            var cliente = new Cliente { Id = 1, Nombre = "Test", Documento = 123456, Id_empresa = 1 };
            _mockClienteRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(cliente);

            var resultado = await _servicio.ObtenerPorIdAsync(1);

            resultado.Should().NotBeNull();
            resultado!.Nombre.Should().Be("Test");
            resultado.IdCliente.Should().Be(1);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_ClienteNoExiste_DevuelveNull()
        {
            _mockClienteRepo.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((Cliente?)null);

            var resultado = await _servicio.ObtenerPorIdAsync(999);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task CrearAsync_ClienteValido_AgregaYDevuelveDto()
        {
            _mockClienteRepo
                .Setup(r => r.AgregarAsync(It.IsAny<Cliente>()))
                .Returns<Cliente>(c =>
                {
                    typeof(Cliente).GetProperty("Id")!.SetValue(c, 1);
                    return Task.FromResult(c);
                });

            _mockClienteRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(new Cliente { Id = 1, Nombre = "Nuevo", Documento = 789, Id_empresa = 1 });

            var dto = new Aplicacion.DTOs.Clientes.ClienteCrearDto
            {
                Nombre = "Nuevo",
                Documento = 789,
                IdEmpresa = 1,
                Activo = true
            };

            var resultado = await _servicio.CrearAsync(dto);

            resultado.Should().NotBeNull();
            _mockClienteRepo.Verify(r => r.AgregarAsync(It.IsAny<Cliente>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_ClienteExistente_ActualizaPropiedades()
        {
            var cliente = new Cliente { Id = 1, Nombre = "Viejo", Documento = 111, Id_empresa = 1 };
            _mockClienteRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(cliente);

            var dto = new Aplicacion.DTOs.Clientes.ClienteActualizarDto
            {
                Id = 1,
                Nombre = "Nuevo Nombre",
                Documento = 222,
                Activo = true
            };

            await _servicio.ActualizarAsync(dto);

            cliente.Nombre.Should().Be("Nuevo Nombre");
            cliente.Documento.Should().Be(222);
            _mockClienteRepo.Verify(r => r.Actualizar(cliente), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_ClienteNoExiste_LanzaKeyNotFound()
        {
            _mockClienteRepo.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((Cliente?)null);

            var act = () => _servicio.ActualizarAsync(new Aplicacion.DTOs.Clientes.ClienteActualizarDto
            {
                Id = 999,
                Nombre = "Test",
                Documento = 123,
                Activo = true
            });

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DesactivarAsync_ClienteExistente_MarcaInactivo()
        {
            var cliente = new Cliente { Id = 1, Nombre = "Test", Activo = true };
            _mockClienteRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(cliente);

            await _servicio.DesactivarAsync(1);

            cliente.Activo.Should().BeFalse();
            _mockClienteRepo.Verify(r => r.Actualizar(cliente), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task ContarClientesConVentasAsync_DelegaEnRepositorio()
        {
            _mockClienteRepo
                .Setup(r => r.ContarClientesConVentasAsync(1))
                .ReturnsAsync(15);

            var count = await _servicio.ContarClientesConVentasAsync(1);

            count.Should().Be(15);
            _mockClienteRepo.Verify(r => r.ContarClientesConVentasAsync(1), Times.Once);
        }
    }
}
