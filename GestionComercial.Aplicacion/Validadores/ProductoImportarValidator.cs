using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.Aplicacion.Validators
{
    ///     /// Valida un DTO individual de importación antes de procesarlo.
    /// Nota: en importación masiva se usa validación inline por performance
    /// (no se valida con FluentValidation item por item para 10K registros).
    /// Este validador es útil para imports individuales o pruebas unitarias.
    public class ProductoImportarValidator : AbstractValidator<ProductoImportarDto>
    {
        public ProductoImportarValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.");

            RuleFor(x => x.PrecioVentaActual)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0.");

            RuleFor(x => x.IdEmpresa)
                .GreaterThan(0).WithMessage("ID de empresa inválido.");
        }
    }
}
