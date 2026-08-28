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
        private readonly SesionServicio? _sesion;

        public DescuentoConfiguracionServicio(IUnitOfWork unitOfWork, SesionServicio? sesion = null)
        {
            _unitOfWork = unitOfWork;
            _sesion = sesion;
        }

        public async Task<DescuentoConfiguracion> CrearAsync(
            int idEmpresa, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto)
        {
            if (_sesion != null && !_sesion.HasPermission("Descuentos.Ver"))
                throw new InvalidOperationException("No tenés permiso para crear descuentos.");

            var descuento = DescuentoConfiguracion.Crear(
                nombre, valor, idEmpresa, idProducto, idCategoria,
                aplicaCualquierMetodoPago, idsMetodosPago, fechaDesde, fechaHasta, alcance);

            // ── Todo en una transacción: descuento + relaciones N:M ─────────────────
            await _unitOfWork.EjecutarEnTransaccionAsync(async () =>
            {
                await _unitOfWork.DescuentoConfiguraciones.AgregarAsync(descuento);

                // Persistir relaciones N:M (solo cuando restringe métodos específicos)
                if (!aplicaCualquierMetodoPago && idsMetodosPago != null && idsMetodosPago.Count > 0)
                {
                    await _unitOfWork.DescuentoConfiguraciones
                        .ActualizarMetodosPagoAsync(descuento.Id, idsMetodosPago);
                }
            });

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
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto)
        {
            if (_sesion != null && !_sesion.HasPermission("Descuentos.Ver"))
                throw new InvalidOperationException("No tenés permiso para editar descuentos.");

            if (alcance == AlcanceDescuentoEnum.MetodoPago)
            {
                if (aplicaCualquierMetodoPago)
                    throw new InvalidOperationException("Para descuentos por método de pago, no puede aplicar a cualquier método.");
                if (idsMetodosPago == null || idsMetodosPago.Count == 0)
                    throw new InvalidOperationException("Debe seleccionar al menos un método de pago.");
            }
            else
            {
                if (!aplicaCualquierMetodoPago && (idsMetodosPago == null || idsMetodosPago.Count == 0))
                    throw new InvalidOperationException("Debe indicar cualquier método o seleccionar al menos una tarjeta.");
            }

            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Actualizar(nombre, valor, idProducto, idCategoria,
                aplicaCualquierMetodoPago, fechaDesde, fechaHasta, alcance);

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
            if (_sesion != null && !_sesion.HasPermission("Descuentos.Ver"))
                throw new InvalidOperationException("No tenés permiso para eliminar descuentos.");

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
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente
                         && d.Alcance != AlcanceDescuentoEnum.MetodoPago)
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

        public Task<DescuentoConfiguracion?> ObtenerDescuentoProductoAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Categoria> categoriasCache)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente)
                .ToList();

            var productDiscounts = candidates
                .Where(d => d.Id_producto.HasValue && d.Id_producto == idProducto)
                .ToList();

            var categoryDiscounts = new List<DescuentoConfiguracion>();
            if (idCategoria.HasValue && categoriasCache.TryGetValue(idCategoria.Value, out var startCategoria))
            {
                var current = startCategoria;
                var depth = 0;
                while (current != null && depth < 10)
                {
                    var matches = candidates
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

            var winner = allCandidates
                .OrderByDescending(d => d.Id_producto.HasValue)
                .ThenByDescending(d => d.Valor)
                .First();

            return Task.FromResult<DescuentoConfiguracion?>(winner);
        }

        public virtual Task<DescuentoConfiguracion?> ObtenerDescuentoTotalVentaAsync(
            int idEmpresa,
            int idMetodoPago,
            List<DescuentoConfiguracion> descuentosCache)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa
                         && d.Activo
                         && d.EstaVigente
                         && d.Alcance == AlcanceDescuentoEnum.MetodoPago
                         && !d.AplicaCualquierMetodoPago
                         && d.DescuentosMetodosPago.Any(dm => dm.Id_metodoPago == idMetodoPago))
                .ToList();

            if (candidates.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            var winner = candidates.OrderByDescending(d => d.Valor).First();
            return Task.FromResult<DescuentoConfiguracion?>(winner);
        }
    }
}