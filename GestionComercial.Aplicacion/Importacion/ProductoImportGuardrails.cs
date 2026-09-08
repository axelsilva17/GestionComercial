using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.Aplicacion.Importacion
{
    public class ProductoImportGuardrails
    {
        public IReadOnlyList<ImportGuardRule> Rules { get; }

        public ProductoImportGuardrails()
        {
            Rules = new List<ImportGuardRule>
            {
                new("NombreVacio", GuardSeverity.Error, ValidateNombreVacio),
                new("PrecioCero", GuardSeverity.Warning, ValidatePrecioCero),
                // CodigoBarra is OPTIONAL: empty/whitespace = product without barcode.
                // Only validate numeric format when a value IS provided.
                new("CodigoBarraNumerico", GuardSeverity.Error, ValidateCodigoBarraNumerico),
                new("NombreLongitud", GuardSeverity.Error, ValidateNombreLongitud),
            };
        }

        public List<(ProductoImportarDto Row, List<GuardResult> Results)> ValidateBatch(
            IEnumerable<ProductoImportarDto> rows)
        {
            var allRows = rows.ToList();
            var results = new List<(ProductoImportarDto Row, List<GuardResult> Results)>();
            var barcodesSeen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in allRows)
            {
                var rowResults = new List<GuardResult>();

                foreach (var rule in Rules)
                {
                    rowResults.Add(rule.Func(row));
                }

                // Duplicado interno (G5) — se evalúa contra el lote
                if (!string.IsNullOrWhiteSpace(row.CodigoBarra))
                {
                    if (!barcodesSeen.Add(row.CodigoBarra))
                    {
                        rowResults.Add(new GuardResult(false,
                            $"Código de barra duplicado en el lote: {row.CodigoBarra}",
                            "CodigoBarra", GuardSeverity.Skip));
                    }
                }

                results.Add((row, rowResults));
            }

            return results;
        }

        private static GuardResult ValidateNombreVacio(ProductoImportarDto row)
        {
            if (string.IsNullOrWhiteSpace(row.Nombre))
                return new GuardResult(false, "Nombre vacío", "Nombre", GuardSeverity.Error);
            return new GuardResult(true, "", "Nombre", GuardSeverity.Error);
        }

        private static GuardResult ValidatePrecioCero(ProductoImportarDto row)
        {
            if (row.PrecioVentaActual <= 0)
                return new GuardResult(false, "Precio de venta ≤ 0", "PrecioVenta", GuardSeverity.Warning);
            return new GuardResult(true, "", "PrecioVenta", GuardSeverity.Warning);
        }

        private static GuardResult ValidateCodigoBarraNumerico(ProductoImportarDto row)
        {
            if (!string.IsNullOrWhiteSpace(row.CodigoBarra) && !row.CodigoBarra.All(char.IsDigit))
                return new GuardResult(false, $"Código de barra debe ser numérico: '{row.CodigoBarra}'", "CodigoBarra", GuardSeverity.Error);
            return new GuardResult(true, "", "CodigoBarra", GuardSeverity.Error);
        }

        private static GuardResult ValidateNombreLongitud(ProductoImportarDto row)
        {
            if (!string.IsNullOrWhiteSpace(row.Nombre) && row.Nombre.Length > 200)
                return new GuardResult(false, $"Nombre excede 200 caracteres ({row.Nombre.Length})", "Nombre", GuardSeverity.Error);
            return new GuardResult(true, "", "Nombre", GuardSeverity.Error);
        }
    }
}
