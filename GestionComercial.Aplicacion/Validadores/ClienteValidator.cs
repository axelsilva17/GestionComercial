using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteCrearDto>
    {
        public ClienteValidator(IClienteRepositorio clienteRepo)
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(x => x.Documento)
                .GreaterThan(0).WithMessage("El documento es obligatorio.")
                .Must(d => d.ToString().Length >= 7 && d.ToString().Length <= 8)
                    .WithMessage("El documento debe tener entre 7 y 8 dígitos.")
                .MustAsync(async (dto, documento, ct) =>
                    !await clienteRepo.ExisteDocumentoAsync(documento, dto.IdEmpresa))
                    .WithMessage("Ya existe un cliente con ese documento.");

            RuleFor(x => x.Telefono)
                .Matches(@"^[\d\s\+\-\(\)]{6,15}$")
                    .WithMessage("El teléfono tiene un formato inválido.")
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .MustAsync(async (dto, email, ct) =>
    !await clienteRepo.ExisteEmailAsync(email, dto.IdEmpresa))
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.IdEmpresa)
                .GreaterThan(0).WithMessage("La empresa es obligatoria.");
        }
    }
}
