using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.Aplicacion.Importacion
{
    public enum GuardSeverity
    {
        Error,
        Warning,
        Skip
    }

    public record GuardResult(bool Passed, string Message, string Field, GuardSeverity Severity);

    public record ImportGuardRule(string Name, GuardSeverity Severity, Func<ProductoImportarDto, GuardResult> Func);
}
