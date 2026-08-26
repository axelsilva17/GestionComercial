namespace GestionComercial.Aplicacion.Importacion
{
    public record ImportError(int Row, string Field, string Message, GuardSeverity Severity);

    public class ImportResult
    {
        public int Inserted { get; set; }
        public int Updated { get; set; }
        public int Skipped { get; set; }
        public List<ImportError> Errors { get; set; } = new();

        public int TotalProcessed => Inserted + Updated + Skipped;
        public bool HasErrors => Errors.Count > 0;
    }
}
