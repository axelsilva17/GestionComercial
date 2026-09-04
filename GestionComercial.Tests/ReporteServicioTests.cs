using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;
using FluentAssertions;

namespace GestionComercial.Tests
{
    /*
     * ================================================================
     * CÓMO ESCRIBIR TESTS CON xUNIT - GUIA DE APRENDIZAJE
     * ================================================================
     * 
     * Concepto básico: Un test es un método que verifica que una 
     * pieza de código se comporta como esperamos.
     * 
     * Estructura de un test (AAA):
     * 
     * 1. ARRANGE (Organizar) - Prepara los datos y objetos
     * 2. ACT (Actuar) - Ejecuta el método que quieres probar
     * 3. ASSERT (Verificar) - Comprueba que el resultado es correcto
     * 
     * Ejemplo sencillo:
     * 
     *   [Fact]
     *   public void Sumar_DosNumeros_DevuelveSuSuma()
     *   {
     *       // ARRANGE - Preparamos los datos
     *       var calculadora = new Calculadora();
     *       var a = 5;
     *       var b = 3;
     *       
     *       // ACT - Ejecutamos el método
     *       var resultado = calculadora.Sumar(a, b);
     *       
     *       // ASSERT - Verificamos el resultado
     *       resultado.Should().Be(8);  // FluentAssertions
     *   }
     * 
     * ================================================================
     * QUÉ ES UN MOCK?
     * ================================================================
     * 
     * Un Mock es un "falso" objeto que simula el comportamiento de 
     * un objeto real. Se usa para:
     * - Simular base de datos sin tocar BD real
     * - Simular servicios externos (APIs, emails)
     * - Hacer que el test sea rápido y reproducible
     * 
     * Ejemplo con Moq:
     * 
     *   // Crear un mock del repositorio
     *   var mockRepositorio = new Mock<IVentaRepostorio>();
     *   
     *   // Configurar qué devuelve cuando se llama a un método
     *   mockRepositorio
     *       .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
     *       .ReturnsAsync(new List<Venta> { ... });
     *   
     *   // Usar el mock en el servicio
     *   var servicio = new ReporteServicio(mockUow.Object);
     */

    public class ReporteServicioTests
    {
        // ═══════════════════════════════════════════════════════════
        // TEST 1: Verificar que VentasPorVendedorAsync agrupa por usuario
        // ═══════════════════════════════════════════════════════════
        [Fact]
        public async Task VentasPorVendedorAsync_DebeAgruparPorUsuario()
        {
            // ╔══════════════════════════════════════════════════════╗
            // ║ ARRANGE - Preparamos el entorno de test             ║
            // ╚══════════════════════════════════════════════════════╝
            
            // 1. Creamos MOCK del IUnitOfWork (el repositorio falso)
            var mockUow = new Mock<IUnitOfWork>();
            
            // 2. Creamos MOCK del repositorio de ventas
            var mockVentaRepo = new Mock<IVentaRepostorio>();
            
            // 3. Creamos datos de prueba agrupados (resultado del SQL)
            var ventasDePrueba = new List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>
            {
                (101, "Juan Pérez", "Sucursal Central", 2, 3000m, 150m),
                (102, "María López", "Sucursal Central", 1, 1500m, 50m),
            };
            
            // 4. Configuramos el MOCK: cuando se llame ObtenerVentasPorVendedorAgrupadoAsync,
            //    que devuelva nuestros datos de prueba
            mockVentaRepo
                .Setup(r => r.ObtenerVentasPorVendedorAgrupadoAsync(
                    It.IsAny<int>(), 
                    It.IsAny<DateTime>(), 
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(ventasDePrueba);
            
            // 5. Configuramos el UnitOfWork para que devuelva nuestro mock
            mockUow.Setup(u => u.Ventas).Returns(mockVentaRepo.Object);
            
            // 6. Creamos el servicio a testear (con el mock inyectado)
            var servicio = new ReporteServicio(mockUow.Object);
            
            // ╔══════════════════════════════════════════════════════╗
            // ║ ACT - Ejecutamos el método que queremos probar     ║
            // ╚══════════════════════════════════════════════════════╝
            
            var resultado = await servicio.VentasPorVendedorAsync(
                idSucursal: 1,
                desde: DateTime.Now.AddDays(-7),
                hasta: DateTime.Now);
            
            // ╔══════════════════════════════════════════════════════╗
            // ║ ASSERT - Verificamos que el resultado es correcto   ║
            // ╚══════════════════════════════════════════════════════╝
            
            var lista = resultado.ToList();
            
            // Debe haber 2 vendedores
            lista.Should().HaveCount(2);
            
            // El usuario 101 debe tener 2 ventas con total 3000
            var vendedor101 = lista.First(v => v.IdUsuario == 101);
            vendedor101.CantidadVentas.Should().Be(2);
            vendedor101.TotalVendido.Should().Be(3000);
            
            // El usuario 102 debe tener 1 venta con total 1500
            var vendedor102 = lista.First(v => v.IdUsuario == 102);
            vendedor102.CantidadVentas.Should().Be(1);
            vendedor102.TotalVendido.Should().Be(1500);
        }
        
        // ═══════════════════════════════════════════════════════════
        // TEST 2: Verificar que maneja lista vacía correctamente
        // ═══════════════════════════════════════════════════════════
        [Fact]
        public async Task VentasPorVendedorAsync_SinVentas_DevuelveListaVacia()
        {
            // ARRANGE
            var mockUow = new Mock<IUnitOfWork>();
            var mockVentaRepo = new Mock<IVentaRepostorio>();
            
            // Configurar que no hay ventas
            mockVentaRepo
                .Setup(r => r.ObtenerVentasPorVendedorAgrupadoAsync(
                    It.IsAny<int>(), 
                    It.IsAny<DateTime>(), 
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<(int, string, string, int, decimal, decimal)>());
            
            mockUow.Setup(u => u.Ventas).Returns(mockVentaRepo.Object);
            
            var servicio = new ReporteServicio(mockUow.Object);
            
            // ACT
            var resultado = await servicio.VentasPorVendedorAsync(
                idSucursal: 1,
                desde: DateTime.Now.AddDays(-7),
                hasta: DateTime.Now);
            
            // ASSERT
            resultado.Should().BeEmpty();
        }
        
        // ═══════════════════════════════════════════════════════════
        // TEST 3: Verificar que TopProductosAsync funciona
        // ═══════════════════════════════════════════════════════════
        [Fact]
        public async Task TopProductosAsync_DebeDevolverTopNProductos()
        {
            // ARRANGE
            var mockUow = new Mock<IUnitOfWork>();
            var mockVentaRepo = new Mock<IVentaRepostorio>();
            
            // Configurar mock del nuevo método SQL agrupado
            mockVentaRepo
                .Setup(r => r.ObtenerTopProductosAgrupadoAsync(
                    It.IsAny<int>(), 
                    It.IsAny<DateTime>(), 
                    It.IsAny<DateTime>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<(int, string, string, int, decimal, decimal, DateTime?)>());
            
            mockUow.Setup(u => u.Ventas).Returns(mockVentaRepo.Object);
            
            var servicio = new ReporteServicio(mockUow.Object);
            
            // ACT
            var resultado = await servicio.TopProductosAsync(
                idSucursal: 1,
                desde: DateTime.Now.AddDays(-30),
                hasta: DateTime.Now,
                top: 5);
            
            // ASSERT
            // Como no hay ventas, la lista debe estar vacía
            resultado.Should().BeEmpty();
        }
    }
}
