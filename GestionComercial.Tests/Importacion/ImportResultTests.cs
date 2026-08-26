using FluentAssertions;
using GestionComercial.Aplicacion.Importacion;

namespace GestionComercial.Tests.Importacion
{
    public class ImportResultTests
    {
        [Fact]
        public void ImportResult_Nuevo_EmpiezaEnCero()
        {
            var result = new ImportResult();

            result.Inserted.Should().Be(0);
            result.Updated.Should().Be(0);
            result.Skipped.Should().Be(0);
            result.Errors.Should().BeEmpty();
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void ImportResult_ConErrores_HasErrorsEsTrue()
        {
            var result = new ImportResult();
            result.Errors.Add(new ImportError(2, "Nombre", "Vacío", GuardSeverity.Error));

            result.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void ImportResult_TotalProcessed_SumaCorrectamente()
        {
            var result = new ImportResult
            {
                Inserted = 10,
                Updated = 5,
                Skipped = 3
            };

            result.TotalProcessed.Should().Be(18);
        }

        [Fact]
        public void ImportResult_ImportacionVacia_CeroRegistros()
        {
            var result = new ImportResult();

            result.Inserted.Should().Be(0);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ImportResult_ImportacionMixta_ConteoCorrecto()
        {
            var result = new ImportResult
            {
                Inserted = 80,
                Skipped = 10,
            };
            result.Errors.AddRange(Enumerable.Range(0, 10)
                .Select(i => new ImportError(i + 2, "General", $"Error {i}", GuardSeverity.Error)));

            result.Inserted.Should().Be(80);
            result.Skipped.Should().Be(10);
            result.Errors.Should().HaveCount(10);
            result.HasErrors.Should().BeTrue();
        }
    }
}
