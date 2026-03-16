using FluentValidation;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    /// <summary>
    /// DTO para abrir una caja. Agregarlo en DTOs/Caja si no existe.
    /// </summary>
    public class CajaAbrirDto
    {
        public int     IdSucursal   { get; set; }
        public int     IdUsuario    { get; set; }
        public decimal MontoInicial { get; set; }
    }

    public class CajaValidator : AbstractValidator<CajaAbrirDto>
    {
        public CajaValidator(ICajaRepositorio cajaRepo)
        {
            RuleFor(x => x.IdSucursal)
                .GreaterThan(0).WithMessage("La sucursal es obligatoria.")
                .MustAsync(async (id, ct) =>
                    !await cajaRepo.ExisteCajaAbiertaAsync(id))
                    .WithMessage("Ya existe una caja abierta en esta sucursal.");

            RuleFor(x => x.IdUsuario)
                .GreaterThan(0).WithMessage("El usuario es obligatorio.");

            RuleFor(x => x.MontoInicial)
                .GreaterThanOrEqualTo(0).WithMessage("El monto inicial no puede ser negativo.");
        }
    }
}
