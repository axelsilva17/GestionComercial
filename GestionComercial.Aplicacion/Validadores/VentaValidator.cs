using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Aplicacion.Validators
{
    public class VentaValidator : AbstractValidator<VentaCrearDto>
    {
        public VentaValidator(IProductoRepositorio productoRepo)
        {
            RuleFor(x => x.IdCliente)
                .GreaterThan(0).WithMessage("El cliente es obligatorio.");

            RuleFor(x => x.IdSucursal)
                .GreaterThan(0).WithMessage("La sucursal es obligatoria.");

            RuleFor(x => x.IdUsuario)
                .GreaterThan(0).WithMessage("El usuario es obligatorio.");

            RuleFor(x => x.IdCaja)
                .GreaterThan(0).WithMessage("Debe haber una caja abierta para registrar la venta.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("La venta debe tener al menos un producto.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.IdProducto)
                    .GreaterThan(0).WithMessage("El producto es obligatorio.");

                item.RuleFor(i => i.Cantidad)
                    .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

                item.RuleFor(i => i.PrecioUnitario)
                    .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a 0.");

                item.RuleFor(i => i)
                    .MustAsync(async (detalle, ct) =>
                    {
                        var stock = await productoRepo.ObtenerStockAsync(detalle.IdProducto);
                        return detalle.Cantidad <= stock;
                    })
                    .WithMessage(i => $"Stock insuficiente para el producto #{i.IdProducto}.");
            });

            // NOTA: La validación de pagos se elimina porque el método de pago
            // se selecciona en PagoView, NO en VentaView. La venta se crea
            // como "Pendiente" en IrACobrar() y los pagos se registran después.
            // RuleFor(x => x.Pagos)
            //     .NotEmpty().WithMessage("La venta debe tener al menos un método de pago.");

            RuleFor(x => x)
                .Must(v =>
                {
                    var totalPagado = v.Pagos.Sum(p => p.Monto);
                    return Math.Abs(totalPagado - v.TotalFinal) < 0.01m;
                })
                .WithMessage("El total de los pagos no coincide con el total de la venta.")
                .When(x => x.Pagos.Any());

            RuleFor(x => x.TotalFinal)
                .GreaterThan(0).WithMessage("El total de la venta debe ser mayor a 0.");
        }
    }
}
