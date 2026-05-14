using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.Servicios
{
public class ProductoServicio : IProductoServicio
{
    private readonly IUnitOfWork _uow;
    public ProductoServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<Proveedor>> ObtenerProveedoresAsync()
        {
            return await _uow.Proveedores.ObtenerTodosAsync();
        }

        public async Task<IEnumerable<ProductoListadoDto>> ObtenerTodosAsync(int idEmpresa)
        {
            var productos = await _uow.Productos.ObtenerPorEmpresaAsync(idEmpresa);
            return productos.Select(MapearListado);
        }

        public async Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa)
        {
            var productos = await _uow.Productos.ObtenerStockCriticoAsync(idEmpresa);
            return productos.Select(MapearListado);
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            var p = await _uow.Productos.ObtenerPorIdConDetallesAsync(id);
            return p == null ? null : MapearDto(p);
        }

        public async Task<ProductoDto> CrearAsync(ProductoCrearDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                CodigoBarra = dto.CodigoBarra,
                PrecioVentaActual = dto.PrecioVentaActual,
                PrecioCostoActual = dto.PrecioCostoActual,
                StockActual = dto.StockActual,
                StockMinimo = dto.StockMinimo,
                Activo = true,
                Id_empresa = dto.IdEmpresa,
                Id_categoria = dto.IdCategoria,
                Id_unidadMedida = dto.IdUnidadMedida,
            };
            await _uow.Productos.AgregarAsync(producto);
            await _uow.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(producto.Id) ?? throw new Exception("Error al crear producto");
        }

        public async Task ActualizarAsync(ProductoActualizarDto dto)
        {
            var producto = await _uow.Productos.ObtenerPorIdAsync(dto.IdProducto)
                ?? throw new KeyNotFoundException($"Producto {dto.IdProducto} no encontrado");
            producto.Nombre = dto.Nombre;
            producto.CodigoBarra = dto.CodigoBarra;
            producto.PrecioVentaActual = dto.PrecioVentaActual;
            producto.PrecioCostoActual = dto.PrecioCostoActual;
            producto.StockMinimo = dto.StockMinimo;
            producto.Id_categoria = dto.IdCategoria;
            producto.Id_unidadMedida = dto.IdUnidadMedida;
            _uow.Productos.Actualizar(producto);
            await _uow.GuardarCambiosAsync();
        }

        public async Task<(ProductoDto Producto, bool FueActualizacion)> CrearOActualizarAsync(ProductoImportarDto dto, bool actualizarExistentes)
        {
            // Si tiene código de barra y la opción está activa → buscar existente
            if (!string.IsNullOrWhiteSpace(dto.CodigoBarra) && actualizarExistentes)
            {
                var existente = await _uow.Productos.ObtenerPorCodigoBarraAsync(dto.CodigoBarra);
                if (existente != null)
                {
                    existente.Nombre = dto.Nombre;
                    existente.PrecioVentaActual = dto.PrecioVentaActual;
                    existente.PrecioCostoActual = dto.PrecioCostoActual;
                    existente.StockActual = dto.StockActual;
                    existente.StockMinimo = dto.StockMinimo;
                    existente.Id_categoria = dto.IdCategoria;
                    existente.Id_unidadMedida = dto.IdUnidadMedida;
                    _uow.Productos.Actualizar(existente);
                    await _uow.GuardarCambiosAsync();
                    var resultado = await ObtenerPorIdAsync(existente.Id) ?? throw new Exception("Error al actualizar");
                    return (resultado, true);
                }
            }

            // Crear nuevo
            var nuevo = await CrearAsync(new ProductoCrearDto
            {
                Nombre = dto.Nombre,
                CodigoBarra = dto.CodigoBarra,
                PrecioVentaActual = dto.PrecioVentaActual,
                PrecioCostoActual = dto.PrecioCostoActual,
                StockActual = dto.StockActual,
                StockMinimo = dto.StockMinimo,
                IdCategoria = dto.IdCategoria,
                IdUnidadMedida = dto.IdUnidadMedida,
                IdEmpresa = dto.IdEmpresa,
            });
            return (nuevo, false);
        }

        /// <summary>
        /// Importación masiva optimizada.
        /// 1) Crea categorías nuevas primero para tener IDs válidos.
        /// 2) Para cada producto: si el código de barra ya existe y actualizarExistentes = true,
        ///    actualiza el producto existente; si no existe (o no se busca por barra), crea uno nuevo.
        /// 3) Procesa en batches de 50 para no agotar memoria.
        /// </summary>
        public async Task<(int Nuevos, int Actualizados, int Omitidos)> ImportarMasivoAsync(
            IEnumerable<ProductoImportarDto> dtos,
            bool actualizarExistentes,
            IProgress<(int current, int total, string message)>? progreso = null)
        {
            const int BATCH_SIZE = 50;
            const int PROGRESS_STEP = 5;

            var productosAImportar = dtos.ToList();
            var total = productosAImportar.Count;
            var nuevos = 0;
            var actualizados = 0;
            var omitidos = 0;
            var procesados = 0;

            var idEmpresa = productosAImportar.FirstOrDefault()?.IdEmpresa ?? 0;
            if (idEmpresa <= 0)
                return (0, 0, total);

            // ── 1. Cargar datos existentes ──────────────────────────────────────
            var codigosExistentes = new HashSet<string>(
                _uow.Productos.Consultar()
                    .Where(p => p.Id_empresa == idEmpresa && !string.IsNullOrEmpty(p.CodigoBarra))
                    .Select(p => p.CodigoBarra!),
                StringComparer.OrdinalIgnoreCase);

            var categoriasExistentes = (await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa))
                .ToDictionary(c => c.Nombre.ToLower().Trim(), c => c.Id);

            // ── 2. Identificar categorías nuevas y crearlas ANTES ────────────────
            var categoriasMap = new Dictionary<string, int>(categoriasExistentes, StringComparer.OrdinalIgnoreCase);
            var nuevasCategorias = new List<string>();

            // Primera pasada: detectar categorías nuevas
            foreach (var dto in productosAImportar)
            {
                if (string.IsNullOrWhiteSpace(dto.Categoria)) continue;
                var catKey = dto.Categoria.ToLower().Trim();
                if (!categoriasMap.ContainsKey(catKey) && !nuevasCategorias.Contains(catKey))
                    nuevasCategorias.Add(dto.Categoria.Trim());
            }

            // Crear categorías nuevas y agregarlas al map
            if (nuevasCategorias.Count > 0)
            {
                foreach (var nombreCat in nuevasCategorias)
                {
                    var nuevaCat = new Categoria
                    {
                        Nombre = nombreCat,
                        Id_empresa = idEmpresa,
                        Activo = true,
                    };
                    _uow.Categorias.AgregarAsync(nuevaCat);
                }
                await _uow.GuardarCambiosAsync();

                // Recargar categorías para tener los IDs generados
                var categoriasActualizadas = await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa);
                foreach (var c in categoriasActualizadas)
                    categoriasMap[c.Nombre.ToLower().Trim()] = c.Id;
            }

            var batchNuevos = new List<Producto>();
            foreach (var dto in productosAImportar)
            {
                procesados++;

                if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.PrecioVentaActual <= 0)
                {
                    omitidos++;
                    progreso?.Report((procesados, total, $"Omitido: '{dto.Nombre}' (datos inválidos)..."));
                    continue;
                }

                // Resolver categoría
                var idCategoria = dto.IdCategoria;
                if (!string.IsNullOrWhiteSpace(dto.Categoria))
                {
                    var catKey = dto.Categoria.ToLower().Trim();
                    if (categoriasMap.TryGetValue(catKey, out int idCat))
                        idCategoria = idCat;
                }

                // Buscar por código de barra si corresponde
                var productoExistente = actualizarExistentes
                    && !string.IsNullOrWhiteSpace(dto.CodigoBarra)
                    && codigosExistentes.Contains(dto.CodigoBarra)
                        ? await _uow.Productos.ObtenerPorCodigoBarraAsync(dto.CodigoBarra)
                        : null;

                if (productoExistente != null)
                {
                    // ── ACTUALIZAR existente ──
                    productoExistente.Nombre = dto.Nombre;
                    productoExistente.PrecioVentaActual = dto.PrecioVentaActual;
                    productoExistente.PrecioCostoActual = dto.PrecioCostoActual;
                    productoExistente.StockActual = dto.StockActual;
                    productoExistente.StockMinimo = dto.StockMinimo > 0 ? dto.StockMinimo : 10;
                    productoExistente.Id_categoria = idCategoria;
                    productoExistente.Id_unidadMedida = dto.IdUnidadMedida > 0 ? dto.IdUnidadMedida : 1;
                    _uow.Productos.Actualizar(productoExistente);
                    actualizados++;
                }
                else
                {
                    // ── CREAR nuevo ──
                    batchNuevos.Add(new Producto
                    {
                        Nombre = dto.Nombre,
                        CodigoBarra = dto.CodigoBarra,
                        PrecioVentaActual = dto.PrecioVentaActual,
                        PrecioCostoActual = dto.PrecioCostoActual,
                        StockActual = dto.StockActual,
                        StockMinimo = dto.StockMinimo > 0 ? dto.StockMinimo : 10,
                        Id_empresa = dto.IdEmpresa,
                        Id_categoria = idCategoria > 0 ? idCategoria : 1,
                        Id_unidadMedida = dto.IdUnidadMedida > 0 ? dto.IdUnidadMedida : 1,
                        Activo = true,
                    });
                    nuevos++;
                }

                // Reportar progreso
                if (procesados % PROGRESS_STEP == 0)
                {
                    var porcentaje = (int)((procesados / (double)total) * 100);
                    progreso?.Report((procesados, total, $"Importando {porcentaje}% ({procesados}/{total})..."));
                }

                // Guardar batch de nuevos cada BATCH_SIZE
                if (batchNuevos.Count >= BATCH_SIZE)
                {
                    await _uow.Productos.AgregarRangoMasivoAsync(batchNuevos);
                    batchNuevos.Clear();
                }
            }

            // Guardar resto de nuevos
            if (batchNuevos.Count > 0)
                await _uow.Productos.AgregarRangoMasivoAsync(batchNuevos);

            // Guardar cambios de actualizaciones (se ejecuta una sola vez al final)
            if (actualizados > 0)
                await _uow.GuardarCambiosAsync();

            progreso?.Report((total, total, $"Completado: {nuevos} nuevos, {actualizados} actualizados, {omitidos} omitidos"));
            return (nuevos, actualizados, omitidos);
        }

        // Nuevo: Ajuste de precios por proveedor (global por empresa del proveedor)
        public async Task<(int Nuevos, int Actualizados)> AjustePreciosPorProveedorAsync(int idProveedor, decimal porcentaje)
        {
            if (idProveedor <= 0) throw new ArgumentException("ID de proveedor inválido.", nameof(idProveedor));
            if (porcentaje == 0) return (0, 0);

            var proveedor = await _uow.Proveedores.ObtenerPorIdAsync(idProveedor)
                ?? throw new KeyNotFoundException($"Proveedor {idProveedor} no encontrado");

            var factor = 1 + porcentaje / 100m;
            var productos = await _uow.Productos.ObtenerPorEmpresaAsync(proveedor.Id_empresa);
            int actualizados = 0;
            foreach (var p in productos)
            {
                var nuevoVenta = p.PrecioVentaActual * factor;
                var nuevoCosto = p.PrecioCostoActual * factor;
                p.ActualizarPrecios(nuevoVenta, nuevoCosto);
                _uow.Productos.Actualizar(p);
                actualizados++;
            }

            await _uow.GuardarCambiosAsync();
            // Nuevos equivale a la cantidad de productos actualizados en este approach simple
            return (actualizados, actualizados);
        }

        public async Task DesactivarAsync(int id)
        {
            var producto = await _uow.Productos.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Producto {id} no encontrado");
            producto.Activo = false;
            _uow.Productos.Actualizar(producto);
            await _uow.GuardarCambiosAsync();
        }

        public async Task<IEnumerable<CategoriaItemDto>> ObtenerCategoriasAsync(int idEmpresa)
        {
            var categorias = await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa);
            return categorias.Select(c => new CategoriaItemDto
            {
                IdCategoria = c.Id,
                Nombre = c.Nombre,
                CategoriaPadre = c.CategoriaPadre_id
            });
        }

        public async Task<IEnumerable<UnidadMedidaItemDto>> ObtenerUnidadesMedidaAsync()
        {
            var unidades = await Task.FromResult(
                _uow.Productos.Consultar()
                    .Include(p => p.UnidadMedida)
                    .Select(p => p.UnidadMedida)
                    .Where(u => u != null)
                    .Distinct()
                    .Select(u => new UnidadMedidaItemDto
                    {
                        IdUnidadMedida = u!.Id,
                        Nombre = u.Nombre,
                        Abreviatura = u.Abreviatura
                    }).ToList()
            );
            return unidades;
        }

        private static ProductoListadoDto MapearListado(Producto p) => new()
        {
            IdProducto = p.Id,
            Nombre = p.Nombre,
            CodigoBarra = p.CodigoBarra,
            IdCategoria = p.Id_categoria,
            PrecioVentaActual = p.PrecioVentaActual,
            PrecioCostoActual = p.PrecioCostoActual,
            StockActual = (int)p.StockActual,
            StockMinimo = (int)p.StockMinimo,
            Activo = p.Activo,
            CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
            UnidadMedida = p.UnidadMedida?.Nombre ?? string.Empty,
        };

        private static ProductoDto MapearDto(Producto p) => new()
        {
            IdProducto = p.Id,
            Nombre = p.Nombre,
            CodigoBarra = p.CodigoBarra,
            PrecioVentaActual = p.PrecioVentaActual,
            PrecioCostoActual = p.PrecioCostoActual,
            StockActual = (int)p.StockActual,
            StockMinimo = (int)p.StockMinimo,
            Activo = p.Activo,
            IdCategoria = p.Id_categoria,
            CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
            IdUnidadMedida = p.Id_unidadMedida,
            UnidadMedida = p.UnidadMedida?.Nombre ?? string.Empty,
        };
    }
}
