using FluentAssertions;
using GestionComercial.Aplicacion.Importacion;
using System.Data;

namespace GestionComercial.Tests.Importacion
{
    public class ImportacionSchemaGuardTests
    {
        private static DataTable CreateValidTable()
        {
            var table = new DataTable();
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("PrecioVenta", typeof(string));
            table.Columns.Add("CodigoBarra", typeof(string));
            table.Columns.Add("Categoria", typeof(string));
            table.Columns.Add("StockActual", typeof(string));
            table.Columns.Add("StockMinimo", typeof(string));
            table.Columns.Add("UnidadMedida", typeof(string));

            var row = table.NewRow();
            row["Nombre"] = "Producto A";
            row["PrecioVenta"] = "1500";
            row["CodigoBarra"] = "123456";
            row["Categoria"] = "Electrónica";
            row["StockActual"] = "10";
            row["StockMinimo"] = "3";
            row["UnidadMedida"] = "Unidad";
            table.Rows.Add(row);

            return table;
        }

        [Fact]
        public void Validate_EsquemaCompleto_NoTieneErroresFatales()
        {
            var table = CreateValidTable();

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeFalse();
        }

        [Fact]
        public void Validate_ColumnaRequeridaFaltante_RetornaFatalError()
        {
            var table = CreateValidTable();
            table.Columns.Remove("Nombre");

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Column == "Nombre" && e.IsFatal);
        }

        [Fact]
        public void Validate_TipoInvalido_RetornaFatalError()
        {
            var table = CreateValidTable();
            var badRow = table.NewRow();
            badRow["Nombre"] = "Producto B";
            badRow["PrecioVenta"] = "no_es_numero";
            badRow["CodigoBarra"] = "123457";
            table.Rows.Add(badRow);

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Column == "PrecioVenta" && e.IsFatal);
        }

        [Fact]
        public void Validate_ColumnaOpcionalFaltante_NoEsFatal()
        {
            var table = CreateValidTable();
            table.Columns.Remove("Categoria");

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeFalse();
        }

        [Fact]
        public void Validate_ColumnaExtra_RetornaWarning()
        {
            var table = CreateValidTable();
            table.Columns.Add("ColumnaX", typeof(string));

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeFalse();
            result.HasWarnings.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Column == "ColumnaX" && !e.IsFatal);
        }

        [Fact]
        public void Validate_ViolacionLongitud_RetornaFatalError()
        {
            var table = CreateValidTable();
            var row = table.NewRow();
            row["Nombre"] = new string('A', 250);
            row["PrecioVenta"] = "100";
            row["CodigoBarra"] = "123456";
            table.Rows.Add(row);

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Column == "Nombre" && e.IsFatal);
        }

        [Fact]
        public void Validate_TablaVacia_SoloValidaEstructura()
        {
            var table = CreateValidTable();
            table.Rows.Clear();

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeFalse();
        }

        [Fact]
        public void Validate_MultiplesColumnasFaltantes_TodasReportadas()
        {
            var table = new DataTable();
            table.Columns.Add("Categoria", typeof(string));

            var result = ImportacionSchemaGuard.Validate(table);

            result.HasFatalErrors.Should().BeTrue();
            result.Errors.Count(e => e.IsFatal).Should().BeGreaterThanOrEqualTo(2);
        }
    }
}
