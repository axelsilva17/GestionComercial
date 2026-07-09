using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    public class ProductoActualizarValidator : AbstractValidator<ProductoActualizarDto>
    {
        public ProductoActualizarValidator(IProductoRepositorio productoRepo)
        {
            RuleFor(x => x.IdProducto)
                .GreaterThan(0).WithMessage("ID de producto inválido.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(x => x.CodigoBarra)
                .NotEmpty().WithMessage("El código de barra es obligatorio.")
                .Matches(@"^\d+$").WithMessage("El código de barra debe contener solo números.")
                .MustAsync(async (dto, codigo, ct) =>
                {
                    var existente = await productoRepo.ObtenerPorCodigoBarraAsync(codigo);
                    return existente == null || existente.Id == dto.IdProducto;
                }).WithMessage("Ya existe otro producto con ese código de barra.");

            RuleFor(x => x.PrecioVentaActual)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0.");

            RuleFor(x => x.PrecioCostoActual)
                .GreaterThan(0).WithMessage("El precio de costo debe ser mayor a 0.")
                .LessThan(x => x.PrecioVentaActual)
                    .WithMessage("El precio de costo no puede ser mayor o igual al precio de venta.");

            RuleFor(x => x.StockMinimo)
                .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.");

            RuleFor(x => x.IdCategoria)
                .GreaterThan(0).WithMessage("La categoría es obligatoria.");

            RuleFor(x => x.IdUnidadMedida)
                .GreaterThan(0).WithMessage("La unidad de medida es obligatoria.");
        }
    }
}
