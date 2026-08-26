using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Clientes;

namespace GestionComercial.Aplicacion.Validators
{
    public class ClienteActualizarValidator : AbstractValidator<ClienteActualizarDto>
    {
        public ClienteActualizarValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El cliente es obligatorio.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(x => x.Documento)
                .GreaterThan(0).WithMessage("El documento es obligatorio.")
                .Must(d => d.ToString().Length >= 7 && d.ToString().Length <= 8)
                    .WithMessage("El documento debe tener entre 7 y 8 dígitos.");

            RuleFor(x => x.Telefono)
                .Matches(@"^[\d\s\+\-\(\)]{6,15}$")
                    .WithMessage("El teléfono tiene un formato inválido.")
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
