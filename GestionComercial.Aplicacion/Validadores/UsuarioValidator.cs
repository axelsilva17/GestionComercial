using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    public class UsuarioCrearDto
    {
        public string Nombre     { get; set; } = string.Empty;
        public string Apellido   { get; set; } = string.Empty;
        public string Email      { get; set; } = string.Empty;
        public string Password   { get; set; } = string.Empty;
        public int    IdSucursal { get; set; }
        public int    IdRol      { get; set; }
    }

    public class UsuarioValidator : AbstractValidator<UsuarioCrearDto>
    {
        public UsuarioValidator(IUsuarioRepositorio usuarioRepo)
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MinimumLength(2).WithMessage("El apellido debe tener al menos 2 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .MustAsync(async (email, ct) =>
                    !await usuarioRepo.ExisteEmailAsync(email))
                    .WithMessage("Ya existe un usuario con ese email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe tener al menos una mayúscula.")
                .Matches(@"\d").WithMessage("La contraseña debe tener al menos un número.")
                .Must((dto, password) =>
                    !password.Equals(dto.Email, System.StringComparison.OrdinalIgnoreCase))
                    .WithMessage("La contraseña no puede ser igual al email.");

            RuleFor(x => x.IdSucursal)
                .GreaterThan(0).WithMessage("La sucursal es obligatoria.");

            RuleFor(x => x.IdRol)
                .GreaterThan(0).WithMessage("El rol es obligatorio.");
        }
    }
}
