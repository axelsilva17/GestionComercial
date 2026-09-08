using System.Data;

namespace GestionComercial.Aplicacion.Importacion
{
    public record SchemaValidationError(string Column, string Message, bool IsFatal);
    public record SchemaValidationResult(IReadOnlyList<SchemaValidationError> Errors)
    {
        public bool HasFatalErrors => Errors.Any(e => e.IsFatal);
        public bool HasWarnings => Errors.Any(e => !e.IsFatal);
    }

    public static class ImportacionSchemaGuard
    {
        public static SchemaValidationResult Validate(DataTable table, IReadOnlyList<ColumnaEsperada>? schema = null)
        {
            schema ??= ImportacionSchema.SchemaDefinicion;
            var errors = new List<SchemaValidationError>();

            var columnNames = table.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 1. Columnas faltantes
            foreach (var col in schema.Where(c => c.Obligatorio))
            {
                if (!columnNames.Contains(col.Name))
                    errors.Add(new SchemaValidationError(col.Name, $"Columna requerida faltante: {col.Name}", true));
            }

            // 2. Columnas extra (warnings)
            foreach (var colName in columnNames)
            {
                if (!schema.Any(s => s.Name.Equals(colName, StringComparison.OrdinalIgnoreCase)))
                    errors.Add(new SchemaValidationError(colName, $"Columna no reconocida: {colName}", false));
            }

            // 3. Validación de tipos y longitudes en las primeras 100 filas
            var schemaCols = schema
                .Where(s => columnNames.Contains(s.Name))
                .ToList();

            int maxRows = Math.Min(table.Rows.Count, 100);
            for (int i = 0; i < maxRows; i++)
            {
                var row = table.Rows[i];
                foreach (var col in schemaCols)
                {
                    var value = row[col.Name]?.ToString() ?? string.Empty;
                    if (string.IsNullOrEmpty(value))
                    {
                        if (col.Default != null)
                            row[col.Name] = col.Default;
                        continue;
                    }

                    // Validar tipo
                    if (!TryConvert(value, col.Tipo, out _))
                    {
                        errors.Add(new SchemaValidationError(col.Name,
                            $"Tipo inválido en fila {i + 2}: '{value}' no es {col.Tipo.Name}", true));
                    }

                    // Validar longitud
                    if (col.LongitudMax.HasValue && value.Length > col.LongitudMax.Value)
                    {
                        errors.Add(new SchemaValidationError(col.Name,
                            $"Longitud excedida en fila {i + 2}: {value.Length} > {col.LongitudMax.Value}", true));
                    }
                }
            }

            return new SchemaValidationResult(errors);
        }

        private static bool TryConvert(string value, Type targetType, out object? result)
        {
            result = null;
            if (targetType == typeof(string))
            {
                result = value;
                return true;
            }
            if (targetType == typeof(decimal))
            {
                // Culture-tolerant parsing: es-AR ("1.234,56"), en-US ("$1,234.56")
                // and invariant ("1500.50") formats are all valid.
                if (ImportacionNormalizacion.TryParseDecimal(value, out var dec))
                {
                    result = dec;
                    return true;
                }
                return false;
            }
            if (targetType == typeof(int))
            {
                if (ImportacionNormalizacion.TryParseInt(value, out var entero))
                {
                    result = entero;
                    return true;
                }
                return false;
            }
            if (targetType == typeof(DateTime))
            {
                if (ImportacionNormalizacion.TryParseFecha(value, out var fecha))
                {
                    result = fecha;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
