namespace GestionComercial.Aplicacion.Importacion
{
    public record ColumnaEsperada(
        string Name,
        Type Tipo,
        bool Obligatorio,
        int? LongitudMax = null,
        string? Default = null);

    public static class ImportacionSchema
    {
        public static IReadOnlyList<ColumnaEsperada> SchemaDefinicion { get; } = new[]
        {
            new ColumnaEsperada("Nombre",              typeof(string),  true,  200),
            new ColumnaEsperada("Descripcion",         typeof(string),  false, 500, ""),
            new ColumnaEsperada("PrecioVenta",         typeof(decimal), true),
            new ColumnaEsperada("PrecioCosto",         typeof(decimal), false),
            new ColumnaEsperada("CodigoBarra",         typeof(string),  true,  50),
            new ColumnaEsperada("Categoria",           typeof(string),  false, 100, ""),
            new ColumnaEsperada("StockActual",         typeof(int),     false),
            new ColumnaEsperada("StockMinimo",         typeof(int),     false),
            new ColumnaEsperada("UnidadMedida",        typeof(string),  false, 50, "Unidad"),
        };
    }
}
