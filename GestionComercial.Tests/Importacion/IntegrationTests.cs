using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Importacion;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Importacion
{
    public class IntegrationTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IProductoRepositorio> _mockProductoRepo = new();
        private readonly Mock<ICategoriaRepositorio> _mockCategoriaRepo = new();

        public IntegrationTests()
        {
            _mockUow.Setup(u => u.Productos).Returns(_mockProductoRepo.Object);
            _mockUow.Setup(u => u.Categorias).Returns(_mockCategoriaRepo.Object);
        }

        [Fact]
        public async Task LoteExitoso_InsertaTodos()
        {
            var dtos = Enumerable.Range(0, 10)
                .Select(i => CreateDto($"Producto {i}", $"111{i:D3}"))
                .ToList();

            _mockProductoRepo.Setup(r => r.ObtenerConCodigoBarraPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Producto>());
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());
            _mockUow.Setup(u => u.EjecutarEnTransaccionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(async (action, ct) => await action());

            var servicio = new ProductoServicio(_mockUow.Object);
            var result = await servicio.ImportarMasivoAsync(dtos, false);

            result.Inserted.Should().BeGreaterThanOrEqualTo(10);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task LoteConErroresParciales_InsertaLosValidos()
        {
            var dtos = new List<ProductoImportarDto>
            {
                CreateDto("Producto A", "111"),
                CreateDto("", "222"),          // Nombre vacío → error
                CreateDto("Producto C", "333"),
            };

            _mockProductoRepo.Setup(r => r.ObtenerConCodigoBarraPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Producto>());
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());
            _mockUow.Setup(u => u.EjecutarEnTransaccionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(async (action, ct) => await action());

            var servicio = new ProductoServicio(_mockUow.Object);
            var result = await servicio.ImportarMasivoAsync(dtos, false);

            result.Inserted.Should().BeGreaterThanOrEqualTo(2);
            result.Skipped.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task LoteFallidoCompleto_RegistraErrores()
        {
            var dtos = Enumerable.Range(0, 5)
                .Select(i => CreateDto($"", $"111{i:D3}"))
                .ToList();

            _mockProductoRepo.Setup(r => r.ObtenerConCodigoBarraPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Producto>());
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var servicio = new ProductoServicio(_mockUow.Object);
            var result = await servicio.ImportarMasivoAsync(dtos, false);

            result.Inserted.Should().Be(0);
            result.Skipped.Should().Be(5);
        }

        [Fact]
        public async Task EmpresaInvalida_ReturnsError()
        {
            var dtos = new List<ProductoImportarDto>
            {
                new() { Nombre = "Test", CodigoBarra = "111", IdEmpresa = 0, PrecioVentaActual = 100 }
            };

            var servicio = new ProductoServicio(_mockUow.Object);
            var result = await servicio.ImportarMasivoAsync(dtos, false);

            result.Errors.Should().NotBeEmpty();
            result.Inserted.Should().Be(0);
        }

        [Fact]
        public async Task DuplicadosInterno_EnLote_SeSaltan()
        {
            var dtos = new List<ProductoImportarDto>
            {
                CreateDto("Producto A", "111"),
                CreateDto("Producto B", "111"),  // Duplicado
            };

            _mockProductoRepo.Setup(r => r.ObtenerConCodigoBarraPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Producto>());
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());
            _mockUow.Setup(u => u.EjecutarEnTransaccionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(async (action, ct) => await action());

            var servicio = new ProductoServicio(_mockUow.Object);
            var result = await servicio.ImportarMasivoAsync(dtos, false);

            result.Skipped.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task ProgressCallback_SeaInvocado()
        {
            var dtos = Enumerable.Range(0, 3)
                .Select(i => CreateDto($"Producto {i}", $"111{i:D3}"))
                .ToList();

            _mockProductoRepo.Setup(r => r.ObtenerConCodigoBarraPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Producto>());
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());
            _mockUow.Setup(u => u.EjecutarEnTransaccionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(async (action, ct) => await action());

            var progressReports = new List<(int, int, string)>();
            var progress = new Progress<(int, int, string)>(p => progressReports.Add(p));

            var servicio = new ProductoServicio(_mockUow.Object);
            await servicio.ImportarMasivoAsync(dtos, false, progress);

            progressReports.Should().NotBeEmpty();
        }

        private static ProductoImportarDto CreateDto(string nombre, string codigoBarra)
        {
            return new ProductoImportarDto
            {
                Nombre = nombre,
                CodigoBarra = codigoBarra,
                PrecioVentaActual = 1500m,
                PrecioCostoActual = 800m,
                StockActual = 10,
                StockMinimo = 3,
                Categoria = "Test",
                UnidadMedida = "Unidad",
                IdEmpresa = 1,
                IdCategoria = 1,
                IdUnidadMedida = 1,
            };
        }
    }
}
