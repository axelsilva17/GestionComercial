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
            int idEmpresa, string nombre, TipoDescuentoEnum tipo, decimal valor,
            int? idProducto, int? idCategoria, int? idMetodoPago, DateTime? fechaDesde, DateTime? fechaHasta, int prioridad)
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre, tipo, valor, idEmpresa, idProducto, idCategoria, idMetodoPago,
                fechaDesde, fechaHasta, prioridad);

            await _unitOfWork.DescuentoConfiguraciones.AgregarAsync(descuento);
            await _unitOfWork.GuardarCambiosAsync();
            return descuento;
        }

        public async Task<DescuentoConfiguracion?> ObtenerPorIdAsync(int id)
        {
            return await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id);
        }

        public async Task ActualizarAsync(int id, string nombre, TipoDescuentoEnum tipo, decimal valor, int? idProducto, int? idCategoria, int? idMetodoPago, DateTime? fechaDesde, DateTime? fechaHasta, int prioridad)
        {
            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Actualizar(nombre, tipo, valor, idProducto, idCategoria, idMetodoPago, fechaDesde, fechaHasta, prioridad);
            await _unitOfWork.GuardarCambiosAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var descuento = await _unitOfWork.DescuentoConfiguraciones.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Descuento {id} no encontrado.");

            descuento.Inactivar();
            await _unitOfWork.GuardarCambiosAsync();
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerTodosAsync(int idEmpresa, TipoDescuentoEnum? tipo = null, bool? activo = null, string? texto = null)
        {
            return await _unitOfWork.DescuentoConfiguraciones.BuscarAsync(idEmpresa, texto, tipo, activo);
        }

        public Task<DescuentoConfiguracion?> ObtenerDescuentoAplicableAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<DescuentoConfiguracion> descuentosCache, Dictionary<int, Categoria> categoriasCache)
        {
            if (descuentosCache == null || descuentosCache.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente)
                .ToList();

            // Product-specific discounts
            var productDiscounts = candidates
                .Where(d => d.Tipo == TipoDescuentoEnum.Producto && d.Id_producto == idProducto)
                .ToList();

            // Category discounts via hierarchy walk
            var categoryDiscounts = new List<DescuentoConfiguracion>();
            if (idCategoria.HasValue && categoriasCache.TryGetValue(idCategoria.Value, out var startCategoria))
            {
                var current = startCategoria;
                var depth = 0;
                while (current != null && depth < 10)
                {
                    var matches = candidates
                        .Where(d => d.Tipo == TipoDescuentoEnum.Categoria && d.Id_categoria == current.Id)
                        .ToList();
                    categoryDiscounts.AddRange(matches);

                    if (current.CategoriaPadre_id.HasValue && categoriasCache.TryGetValue(current.CategoriaPadre_id.Value, out var parent))
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
                .OrderByDescending(d => d.Prioridad)
                .ThenByDescending(d => d.Tipo == TipoDescuentoEnum.Producto)
                .First();

            return Task.FromResult<DescuentoConfiguracion?>(winner);
        }

        public Task<DescuentoConfiguracion?> ObtenerDescuentoMetodoPagoAsync(
            int idEmpresa, List<int> idsMetodosPago, List<DescuentoConfiguracion> descuentosCache)
        {
            if (descuentosCache == null || descuentosCache.Count == 0 || idsMetodosPago == null || idsMetodosPago.Count == 0)
                return Task.FromResult<DescuentoConfiguracion?>(null);

            var candidates = descuentosCache
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente
                    && d.Tipo == TipoDescuentoEnum.MetodoPago
                    && d.Id_metodoPago.HasValue && idsMetodosPago.Contains(d.Id_metodoPago.Value))
                .OrderByDescending(d => d.Prioridad)
                .FirstOrDefault();

            return Task.FromResult(candidates);
        }
    }
}
