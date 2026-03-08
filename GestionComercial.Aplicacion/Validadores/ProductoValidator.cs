using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    public class ProductoValidator : AbstractValidator<ProductoCrearDto>
    {
        public ProductoValidator(IProductoRepositorio productoRepo)
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MustAsync(async (dto, nombre, ct) =>
                    !await productoRepo.ExisteNombreEnCategoriaAsync(nombre, dto.IdCategoria, dto.IdEmpresa))
                    .WithMessage("Ya existe un producto con ese nombre en esta categoría.");

            RuleFor(x => x.CodigoBarra)
                .NotEmpty().WithMessage("El código de barra es obligatorio.")
                .Matches(@"^\d+$").WithMessage("El código de barra debe contener solo números.")
                .MustAsync(async (dto, codigo, ct) =>
                    !await productoRepo.ExisteCodigoBarraAsync(codigo, dto.IdEmpresa))
                    .WithMessage("Ya existe un producto con ese código de barra.");

            RuleFor(x => x.PrecioVentaActual)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0.");

            RuleFor(x => x.PrecioCostoActual)
                .GreaterThan(0).WithMessage("El precio de costo debe ser mayor a 0.")
                .LessThan(x => x.PrecioVentaActual)
                    .WithMessage("El precio de costo no puede ser mayor o igual al precio de venta.");

            RuleFor(x => x.StockActual)
                .GreaterThanOrEqualTo(0).WithMessage("El stock actual no puede ser negativo.");

            RuleFor(x => x.StockMinimo)
                .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.");

            RuleFor(x => x.IdCategoria)
                .GreaterThan(0).WithMessage("La categoría es obligatoria.");

            RuleFor(x => x.IdUnidadMedida)
                .GreaterThan(0).WithMessage("La unidad de medida es obligatoria.");

            RuleFor(x => x.IdEmpresa)
                .GreaterThan(0).WithMessage("La empresa es obligatoria.");
        }
    }
}
