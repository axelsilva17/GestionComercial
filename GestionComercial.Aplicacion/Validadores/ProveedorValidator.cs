using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Proveedores;

namespace GestionComercial.Aplicacion.Validators
{
    public class ProveedorValidator : AbstractValidator<ProveedorCrearDto>
    {
        public ProveedorValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Matches(@"^[\d\s\+\-\(\)]{6,15}$")
                    .WithMessage("El teléfono tiene un formato inválido.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.");

            RuleFor(x => x.IdEmpresa)
                .GreaterThan(0).WithMessage("La empresa es obligatoria.");
        }
    }

    public class ProveedorActualizarValidator : AbstractValidator<ProveedorActualizarDto>
    {
        public ProveedorActualizarValidator()
        {
            RuleFor(x => x.IdProveedor)
                .GreaterThan(0).WithMessage("El proveedor es obligatorio.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Matches(@"^[\d\s\+\-\(\)]{6,15}$")
                    .WithMessage("El teléfono tiene un formato inválido.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.");
        }
    }
}
