using GestionComercial.Aplicacion.DTOs.Descuentos;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using System.Threading;

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
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto,
            decimal? montoMinimoCompra = null,
            CancellationToken ct = default)
        {
            if (_sesion != null && !_sesion.HasPermission("Descuentos.Ver"))
                throw new InvalidOperationException("No tenés permiso para crear descuentos.");

            var descuento = DescuentoConfiguracion.Crear(
                nombre, valor, idEmpresa, idProducto, idCategoria,
                aplicaCualquierMetodoPago, idsMetodosPago, fechaDesde, fechaHasta, alcance, montoMinimoCompra);

            // ── Todo en una transacción: descuento + relaciones N:M ─────────────────
            await _unitOfWork.EjecutarEnTransaccionAsync(async () =>
            {
                // Agregar los métodos a la colección del agregado ANTES de persistir el
                // principal: así EF resuelve el FK con el Id generado y evitamos el error
                // "Id_descuentoConfiguracion is unknown" al intentar guardar.
                if (!aplicaCualquierMetodoPago && idsMetodosPago != null && idsMetodosPago.Count > 0)
                {
                    foreach (var idMetodoPago in idsMetodosPago)
                    {
                        descuento.DescuentosMetodosPago.Add(new DescuentoMetodoPago
                        {
                            Id_metodoPago = idMetodoPago
                        });
                    }
                }

                await _unitOfWork.DescuentoConfiguraciones.AgregarAsync(descuento, ct);
            }, ct);

            return descuento;
        }

        public async Task<DescuentoConfiguracion?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var resultados = await _unitOfWork.DescuentoConfiguraciones.ObtenerConMetodosPagoPorIdAsync(id, ct);
            return resultados.FirstOrDefault();
        }

        public async Task ActualizarAsync(
            int id, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto,
            decimal? montoMinimoCompra = null,
            CancellationToken ct = default)
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
            else if (alcance != AlcanceDescuentoEnum.CompraMayor)
            {
                if (!aplicaCualquierMetodoPago && (idsMetodosPago == null || idsMetodosPago.Count == 0))
                    throw new InvalidOperationException("Debe indicar cualquier método o seleccionar al menos una tarjeta.");
            }

            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Actualizar(nombre, valor, idProducto, idCategoria,
                aplicaCualquierMetodoPago, fechaDesde, fechaHasta, alcance, montoMinimoCompra);

            // Reemplazar relaciones N:M
            var effectiveIds = aplicaCualquierMetodoPago
                ? new List<int>()
                : (idsMetodosPago ?? new List<int>());

            await _unitOfWork.DescuentoConfiguraciones
                .ActualizarMetodosPagoAsync(id, effectiveIds, ct);

            // ObtenerPorIdAsync usa AsNoTracking: la entidad está DETACHED y las
            // mutaciones de Actualizar() no se persistirían sin re-adjuntarla.
            _unitOfWork.DescuentoConfiguraciones.Actualizar(descuento);
            await _unitOfWork.GuardarCambiosAsync(ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct = default)
        {
            if (_sesion != null && !_sesion.HasPermission("Descuentos.Ver"))
                throw new InvalidOperationException("No tenés permiso para eliminar descuentos.");

            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Inactivar();
            _unitOfWork.DescuentoConfiguraciones.Actualizar(descuento);
            await _unitOfWork.GuardarCambiosAsync(ct);
        }

        public async Task ActivarAsync(int id, CancellationToken ct = default)
        {
            if (_sesion != null && !_sesion.HasPermission("Descuentos.Ver"))
                throw new InvalidOperationException("No tenés permiso para activar descuentos.");

            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Reactivar();
            _unitOfWork.DescuentoConfiguraciones.Actualizar(descuento);
            await _unitOfWork.GuardarCambiosAsync(ct);
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerTodosAsync(int idEmpresa, bool? activo = null, string? texto = null, CancellationToken ct = default)
        {
            return await _unitOfWork.DescuentoConfiguraciones.BuscarAsync(idEmpresa, texto, activo, ct);
        }

        public async Task<DescuentoConfiguracion?> ObtenerDescuentoAplicableAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<int> idsMetodosPago, bool esPagoUnico,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Categoria> categoriasCache,
            CancellationToken ct = default)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return null;

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
                return null;

            // Producto gana a categoría; empate resuelto por mayor Valor.
            var winner = allCandidates
                .OrderByDescending(d => d.Id_producto.HasValue)
                .ThenByDescending(d => d.Valor)
                .First();

            return winner;
        }

        public async Task<DescuentoConfiguracion?> ObtenerDescuentoProductoAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Categoria> categoriasCache,
            CancellationToken ct = default)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return null;

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
                return null;

            var winner = allCandidates
                .OrderByDescending(d => d.Id_producto.HasValue)
                .ThenByDescending(d => d.Valor)
                .First();

            return winner;
        }

        public virtual async Task<DescuentoConfiguracion?> ObtenerDescuentoTotalVentaAsync(
            int idEmpresa,
            int idMetodoPago,
            List<DescuentoConfiguracion> descuentosCache,
            CancellationToken ct = default)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return null;

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa
                         && d.Activo
                         && d.EstaVigente
                         && d.Alcance == AlcanceDescuentoEnum.MetodoPago
                         && !d.AplicaCualquierMetodoPago
                         && d.DescuentosMetodosPago.Any(dm => dm.Id_metodoPago == idMetodoPago))
                .ToList();

            if (candidates.Count == 0)
                return null;

            var winner = candidates.OrderByDescending(d => d.Valor).First();
            return winner;
        }

        public async Task<DescuentoConfiguracion?> ObtenerDescuentoCompraMayorAsync(
            int idEmpresa,
            decimal totalVenta,
            List<DescuentoConfiguracion> descuentosCache,
            CancellationToken ct = default)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return null;

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa
                         && d.Activo
                         && d.EstaVigente
                         && d.Alcance == AlcanceDescuentoEnum.CompraMayor
                         && d.MontoMinimoCompra.HasValue
                         && totalVenta >= d.MontoMinimoCompra.Value)
                .ToList();

            if (candidates.Count == 0)
                return null;

            var winner = candidates.OrderByDescending(d => d.Valor).First();
            return winner;
        }
    }
}