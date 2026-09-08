using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Importacion;

namespace GestionComercial.Tests.Importacion
{
    public class ProductoImportGuardrailsTests
    {
        private readonly ProductoImportGuardrails _guardrails = new();

        private static ProductoImportarDto CreateValidDto(string? nombre = "Producto A", string? codigoBarra = "123456")
        {
            return new ProductoImportarDto
            {
                Nombre = nombre ?? string.Empty,
                CodigoBarra = codigoBarra ?? string.Empty,
                PrecioVentaActual = 1500m,
                PrecioCostoActual = 800m,
                StockActual = 10,
                StockMinimo = 3,
                Categoria = "Electrónica",
                UnidadMedida = "Unidad",
                IdEmpresa = 1,
                IdCategoria = 1,
                IdUnidadMedida = 1,
            };
        }

        [Fact]
        public void ValidateBatch_FilaValida_TodosPassed()
        {
            var rows = new[] { CreateValidDto() };

            var results = _guardrails.ValidateBatch(rows);

            results.Should().HaveCount(1);
            results[0].Results.Should().AllSatisfy(r => r.Passed.Should().BeTrue());
        }

        [Fact]
        public void ValidateBatch_NombreVacio_RetornaError()
        {
            var rows = new[] { CreateValidDto(nombre: "") };

            var results = _guardrails.ValidateBatch(rows);

            var nombreResult = results[0].Results.First(r => r.Field == "Nombre");
            nombreResult.Passed.Should().BeFalse();
            nombreResult.Severity.Should().Be(GuardSeverity.Error);
        }

        [Fact]
        public void ValidateBatch_PrecioCero_RetornaWarning()
        {
            var rows = new[] { CreateValidDto() };
            rows[0].PrecioVentaActual = 0;

            var results = _guardrails.ValidateBatch(rows);

            var precioResult = results[0].Results.First(r => r.Field == "PrecioVenta");
            precioResult.Passed.Should().BeFalse();
            precioResult.Severity.Should().Be(GuardSeverity.Warning);
        }

        [Fact]
        public void ValidateBatch_CodigoBarraVacia_EsValida()
        {
            var rows = new[] { CreateValidDto(codigoBarra: "") };

            var results = _guardrails.ValidateBatch(rows);

            // Empty barcode is valid — no error or failure on the CodigoBarra field
            var codResults = results[0].Results.Where(r => r.Field == "CodigoBarra" && r.Severity == GuardSeverity.Error).ToList();
            codResults.Should().AllSatisfy(r => r.Passed.Should().BeTrue());
        }

        [Fact]
        public void ValidateBatch_CodigoBarraWhitespace_EsValida()
        {
            var rows = new[] { CreateValidDto(codigoBarra: "   ") };

            var results = _guardrails.ValidateBatch(rows);

            var codResults = results[0].Results.Where(r => r.Field == "CodigoBarra" && r.Severity == GuardSeverity.Error).ToList();
            codResults.Should().AllSatisfy(r => r.Passed.Should().BeTrue());
        }

        [Fact]
        public void ValidateBatch_CodigoBarraNoNumerico_RetornaError()
        {
            var rows = new[] { CreateValidDto(codigoBarra: "ABC-123") };

            var results = _guardrails.ValidateBatch(rows);

            var codResults = results[0].Results.Where(r => r.Field == "CodigoBarra" && r.Severity == GuardSeverity.Error).ToList();
            codResults.Should().Contain(r => !r.Passed && r.Message.Contains("numérico"));
        }

        [Fact]
        public void ValidateBatch_DuplicadosInterno_RetornaSkip()
        {
            var rows = new[]
            {
                CreateValidDto(codigoBarra: "111"),
                CreateValidDto(codigoBarra: "222"),
                CreateValidDto(codigoBarra: "111"),
            };

            var results = _guardrails.ValidateBatch(rows);

            var thirdRowResults = results[2].Results;
            thirdRowResults.Should().Contain(r => r.Passed == false && r.Severity == GuardSeverity.Skip);
        }

        [Fact]
        public void ValidateBatch_NombreLargo200_RetornaError()
        {
            var rows = new[] { CreateValidDto(nombre: new string('A', 201)) };

            var results = _guardrails.ValidateBatch(rows);

            var nombreResults = results[0].Results.Where(r => r.Field == "Nombre" && r.Severity == GuardSeverity.Error).ToList();
            nombreResults.Should().Contain(r => !r.Passed);
        }

        [Fact]
        public void ValidateBatch_FilaValidaMultiples_EjecutaTodas()
        {
            var rows = new[]
            {
                CreateValidDto(nombre: "A", codigoBarra: "111"),
                CreateValidDto(nombre: "B", codigoBarra: "222"),
            };

            var results = _guardrails.ValidateBatch(rows);

            results.Should().HaveCount(2);
            foreach (var (_, rowResults) in results)
            {
                // Cada fila debería tener resultados de todas las reglas + duplicados check
                rowResults.Count.Should().BeGreaterThanOrEqualTo(_guardrails.Rules.Count);
            }
        }
    }
}
