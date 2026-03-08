using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    public class CompraValidator : AbstractValidator<CompraCrearDto>
    {
        public CompraValidator(IProveedorRepositorio proveedorRepo)
        {
            RuleFor(x => x.IdProveedor)
                .GreaterThan(0).WithMessage("El proveedor es obligatorio.")
                .MustAsync(async (id, ct) =>
                    await proveedorRepo.EstaActivoAsync(id))
                    .WithMessage("El proveedor seleccionado no está activo.");

            RuleFor(x => x.IdSucursal)
                .GreaterThan(0).WithMessage("La sucursal es obligatoria.");

            RuleFor(x => x.IdUsuario)
                .GreaterThan(0).WithMessage("El usuario es obligatorio.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("La compra debe tener al menos un producto.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.IdProducto)
                    .GreaterThan(0).WithMessage("El producto es obligatorio.");

                item.RuleFor(i => i.Cantidad)
                    .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

                item.RuleFor(i => i.PrecioCosto)
                    .GreaterThan(0).WithMessage("El precio de costo debe ser mayor a 0.");
            });
        }
    }
}
