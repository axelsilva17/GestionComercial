using GestionComercial.Aplicacion.DTOs.Descuentos;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class DescuentoConfiguracionServicio : IDescuentoConfiguracionServicio
    {
        private readonly IUnitOfWork _unitOfWork;

        public DescuentoConfiguracionServicio(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DescuentoConfiguracion> CrearAsync(
            int idEmpresa, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre, valor, idEmpresa, idProducto, idCategoria,
                aplicaCualquierMetodoPago, idsMetodosPago, fechaDesde, fechaHasta);

            await _unitOfWork.DescuentoConfiguraciones.AgregarAsync(descuento);
            await _unitOfWork.GuardarCambiosAsync();

            // Persistir relaciones N:M (solo cuando restringe métodos específicos)
            if (!aplicaCualquierMetodoPago && idsMetodosPago != null && idsMetodosPago.Count > 0)
            {
                await _unitOfWork.DescuentoConfiguraciones
                    .ActualizarMetodosPagoAsync(descuento.Id, idsMetodosPago);
                await _unitOfWork.GuardarCambiosAsync();
            }

            return descuento;
        }

        public async Task<DescuentoConfiguracion?> ObtenerPorIdAsync(int id)
        {
            var resultados = await _unitOfWork.DescuentoConfiguraciones.ObtenerConMetodosPagoPorIdAsync(id);
            return resultados.FirstOrDefault();
        }

        public async Task ActualizarAsync(
            int id, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (!aplicaCualquierMetodoPago && (idsMetodosPago == null || idsMetodosPago.Count == 0))
                throw new InvalidOperationException("Debe indicar cualquier método o seleccionar al menos una tarjeta.");

            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Actualizar(nombre, valor, idProducto, idCategoria,
                aplicaCualquierMetodoPago, fechaDesde, fechaHasta);

            // Reemplazar relaciones N:M
            var effectiveIds = aplicaCualquierMetodoPago
                ? new List<int>()
                : (idsMetodosPago ?? new List<int>());

            await _unitOfWork.DescuentoConfiguraciones
                .ActualizarMetodosPagoAsync(id, effectiveIds);
            await _unitOfWork.GuardarCambiosAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Inactivar();
            await _unitOfWork.GuardarCambiosAsync();
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerTodosAsync(int idEmpresa, bool? activo = null, string? texto = null)
        {
            return await _unitOfWork.DescuentoConfiguraciones.BuscarAsync(idEmpresa, texto, activo);
        }

        public Task<DescuentoConfiguracion?> ObtenerDescuentoAplicableAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<int> idsMetodosPago, bool esPagoUnico,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Categoria> categoriasCache)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente)
                .ToList();

            // ── Compuerta de condición de pago ──────────────────────────────
            // Aplica si: cualquier método, o pago único cuyo método esté en la lista N:M.
            var paymentFiltered = candidates.Where(d =>
            {
                if (d.AplicaCualquierMetodoPago) return true;
                if (!esPagoUnico) return false;
                var singleId = idsMetodosPago != null && idsMetodosPago.Count > 0
                    ? idsMetodosPago[0]
                    : 0;
                return d.DescuentosMetodosPago.Any(dm => dm.Id_metodoPago == singleId);
            }).ToList();

            // ── Match por scope ─────────────────────────────────────────────
            var productDiscounts = paymentFiltered
                .Where(d => d.Id_producto.HasValue && d.Id_producto == idProducto)
                .ToList();

            var categoryDiscounts = new List<DescuentoConfiguracion>();
            if (idCategoria.HasValue && categoriasCache.TryGetValue(idCategoria.Value, out var startCategoria))
            {
                var current = startCategoria;
                var depth = 0;
                while (current != null && depth < 10)
                {
                    var matches = paymentFiltered
                        .Where(d => d.Id_categoria.HasValue && d.Id_categoria == current.Id)
                        .ToList();
                    categoryDiscounts.AddRange(matches);

                    if (current.CategoriaPadre_id.HasValue
                        && categoriasCache.TryGetValue(current.CategoriaPadre_id.Value, out var parent))
                        current = parent;
                    else
                        break;

                    depth++;
                }
            }

            var allCandidates = productDiscounts.Concat(categoryDiscounts).ToList();
            if (allCandidates.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            // Producto gana a categoría; empate resuelto por mayor Valor.
            var winner = allCandidates
                .OrderByDescending(d => d.Id_producto.HasValue)
                .ThenByDescending(d => d.Valor)
                .First();

            return Task.FromResult<DescuentoConfiguracion?>(winner);
        }
    }
}