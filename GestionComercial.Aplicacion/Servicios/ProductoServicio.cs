using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Importacion;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using System.Collections.Generic;
using System.Threading;

namespace GestionComercial.Aplicacion.Servicios
{
public class ProductoServicio : IProductoServicio
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<ProductoCrearDto>? _crearValidator;
    private readonly IValidator<ProductoActualizarDto>? _actualizarValidator;
    private readonly SesionServicio? _sesion;

    public ProductoServicio(
        IUnitOfWork uow,
        IValidator<ProductoCrearDto>? crearValidator = null,
        IValidator<ProductoActualizarDto>? actualizarValidator = null,
        SesionServicio? sesion = null)
    {
        _uow = uow;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
        _sesion = sesion;
    }

        public async Task<IEnumerable<Proveedor>> ObtenerProveedoresAsync()
        {
            return await _uow.Proveedores.ObtenerTodosAsync();
        }

        public async Task<IEnumerable<ProductoListadoDto>> ObtenerTodosAsync(int idEmpresa, bool soloActivos = true, CancellationToken ct = default)
        {
            var productos = await _uow.Productos.ObtenerPorEmpresaAsync(idEmpresa, soloActivos, ct);
            return productos.Select(MapearListado);
        }

        public async Task<(IEnumerable<ProductoListadoDto> Items, int TotalCount)> ObtenerTodosPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, int? idCategoria = null, bool? soloActivos = null, CancellationToken ct = default)
        {
            var (items, totalCount) = await _uow.Productos.ObtenerPorEmpresaPaginadoAsync(idEmpresa, page, pageSize, searchTerm, idCategoria, soloActivos, ct);
            return (items.Select(MapearListado), totalCount);
        }

        public async Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa, CancellationToken ct = default)
        {
            var umbral = await ObtenerUmbralStockCriticoAsync(idEmpresa, ct);
            var productos = await _uow.Productos.ObtenerStockCriticoAsync(idEmpresa, umbral, ct);
            return productos.Select(MapearListado);
        }

        // Ajuste masivo de precios: mismo cálculo que el preview del popup (factor para porcentaje,
        // delta para fijo), aplicado en una sola UPDATE SQL sobre TODO el set filtrado.
        public async Task<int> AplicarAjusteMasivoAsync(
            int idEmpresa, string? texto, int? idCategoria, bool? soloActivos,
            string tipoAjuste, string direccionAjuste, decimal porcentaje, decimal montoFijo,
            bool aplicarVenta, bool aplicarCosto, CancellationToken ct = default)
        {
            var signo = direccionAjuste == "reducir" ? -1m : 1m;
            decimal factor = 1m, delta = 0m;
            bool esPorcentaje = tipoAjuste == "porcentaje";
            if (esPorcentaje)
                factor = 1m + signo * porcentaje / 100m;
            else
                delta = signo * montoFijo;

            return await _uow.Productos.AplicarAjustePreciosMasivoAsync(
                idEmpresa, texto, idCategoria, soloActivos,
                factor, delta, esPorcentaje, aplicarVenta, aplicarCosto, ct);
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var p = await _uow.Productos.ObtenerPorIdConDetallesAsync(id, ct);
            return p == null ? null : MapearDto(p);
        }

        public async Task<ProductoDto> CrearAsync(ProductoCrearDto dto, CancellationToken ct = default)
        {
            if (_sesion != null && !_sesion.HasPermission("Productos.Crear"))
                throw new ValidationException("No tenés permiso para crear productos.");

            // Validar entrada con FluentValidation
            if (_crearValidator != null)
            {
                var result = await _crearValidator.ValidateAsync(dto, ct);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                // Empty barcode = no barcode: store NULL so the UNIQUE index
                // (IX_Producto_CodigoBarra*) allows several products without one
                // (SQLite UNIQUE treats each NULL as distinct).
                CodigoBarra = string.IsNullOrWhiteSpace(dto.CodigoBarra) ? null : dto.CodigoBarra,
                PrecioVentaActual = dto.PrecioVentaActual,
                PrecioCostoActual = dto.PrecioCostoActual,
                StockActual = dto.StockActual,
                StockMinimo = dto.StockMinimo,
                Activo = true,
                Id_empresa = dto.IdEmpresa,
                Id_categoria = dto.IdCategoria,
                Id_unidadMedida = dto.IdUnidadMedida,
            };
            await _uow.Productos.AgregarAsync(producto, ct);
            await _uow.GuardarCambiosAsync(ct);
            return await ObtenerPorIdAsync(producto.Id, ct) ?? throw new Exception("Error al crear producto");
        }

        public async Task ActualizarAsync(ProductoActualizarDto dto, CancellationToken ct = default)
        {
            if (_sesion != null && !_sesion.HasPermission("Productos.Editar"))
                throw new ValidationException("No tenés permiso para editar productos.");

            // Validar entrada con FluentValidation
            if (_actualizarValidator != null)
            {
                var result = await _actualizarValidator.ValidateAsync(dto, ct);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

            var producto = await _uow.Productos.ObtenerPorIdAsync(dto.IdProducto, ct)
                ?? throw new KeyNotFoundException($"Producto {dto.IdProducto} no encontrado");
            producto.Nombre = dto.Nombre;
            // Same normalization as CrearAsync: empty barcode stores NULL.
            producto.CodigoBarra = string.IsNullOrWhiteSpace(dto.CodigoBarra) ? null : dto.CodigoBarra;
            producto.PrecioVentaActual = dto.PrecioVentaActual;
            producto.PrecioCostoActual = dto.PrecioCostoActual;
            producto.StockMinimo = dto.StockMinimo;
            producto.Id_categoria = dto.IdCategoria;
            producto.Id_unidadMedida = dto.IdUnidadMedida;
            _uow.Productos.Actualizar(producto);
            await _uow.GuardarCambiosAsync(ct);
        }

        public async Task<(ProductoDto Producto, bool FueActualizacion)> CrearOActualizarAsync(ProductoImportarDto dto, bool actualizarExistentes, CancellationToken ct = default)
        {
            // Si tiene código de barra y la opción está activa → buscar existente
            if (!string.IsNullOrWhiteSpace(dto.CodigoBarra) && actualizarExistentes)
            {
                var existente = await _uow.Productos.ObtenerPorCodigoBarraAsync(dto.CodigoBarra, ct);
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
                    await _uow.GuardarCambiosAsync(ct);
                    var resultado = await ObtenerPorIdAsync(existente.Id, ct) ?? throw new Exception("Error al actualizar");
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
            }, ct);
            return (nuevo, false);
        }

        /// Importación masiva con guardrails y commit parcial por lotes.
        /// Procesa en lotes de TAMANIO_LOTE: ejecuta guardrails, filtra válidos,
        /// bulk insert, commit independiente. Un lote fallido no revierte anteriores.
        public async Task<ImportResult> ImportarMasivoAsync(
            IEnumerable<ProductoImportarDto> dtos,
            bool actualizarExistentes,
            IProgress<(int current, int total, string message)>? progreso = null,
            CancellationToken ct = default)
        {
            const int TAMANIO_LOTE = 50;
            var resultado = new ImportResult();
            var productosAImportar = dtos.ToList();
            var total = productosAImportar.Count;

            var idEmpresa = productosAImportar.FirstOrDefault()?.IdEmpresa ?? 0;
            if (idEmpresa <= 0)
            {
                resultado.Errors.AddRange(productosAImportar.Select((_, i) =>
                    new ImportError(i + 2, "IdEmpresa", "Empresa inválida", GuardSeverity.Error)));
                return resultado;
            }

            // Cargar datos existentes (UNA SOLA CONSULTA)
            var productosPorCodigo = (await _uow.Productos.ObtenerConCodigoBarraPorEmpresaAsync(idEmpresa, ct))
                .ToDictionary(p => p.CodigoBarra!, StringComparer.OrdinalIgnoreCase);

            var categoriasExistentes = (await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa, ct))
                .GroupBy(c => c.Nombre.ToLower().Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

            // Normalizar datos
            foreach (var dto in productosAImportar)
            {
                // Empty barcode → null so the UNIQUE index (SQLite UNIQUE treats
                // each NULL as distinct) allows several products without one.
                dto.CodigoBarra = string.IsNullOrWhiteSpace(dto.CodigoBarra)
                    ? null
                    : dto.CodigoBarra.Trim().ToUpperInvariant();
                dto.Nombre = dto.Nombre?.Trim() ?? string.Empty;
                dto.Categoria = dto.Categoria?.Trim() ?? string.Empty;
            }

            // Reconstruir diccionario con claves normalizadas
            productosPorCodigo = productosPorCodigo
                .GroupBy(p => p.Key.Trim().ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.First().Value, StringComparer.Ordinal);

            // Pre-clasificar
            var dtosNuevos = new List<ProductoImportarDto>();
            var dtosActualizar = new List<ProductoImportarDto>();
            var barcodesVistos = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < productosAImportar.Count; i++)
            {
                var dto = productosAImportar[i];

                if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.PrecioVentaActual <= 0)
                {
                    resultado.Errors.Add(new ImportError(i + 2, "General", "Datos inválidos", GuardSeverity.Error));
                    resultado.Skipped++;
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(dto.CodigoBarra) && !barcodesVistos.Add(dto.CodigoBarra))
                {
                    resultado.Errors.Add(new ImportError(i + 2, "CodigoBarra", "Código duplicado en archivo", GuardSeverity.Skip));
                    resultado.Skipped++;
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(dto.CodigoBarra)
                    && productosPorCodigo.TryGetValue(dto.CodigoBarra, out _))
                {
                    if (!actualizarExistentes)
                    {
                        resultado.Skipped++;
                        continue;
                    }
                    dtosActualizar.Add(dto);
                    continue;
                }

                dtosNuevos.Add(dto);
            }

            var categoriasMap = new Dictionary<string, int>(categoriasExistentes, StringComparer.OrdinalIgnoreCase);

            // Pre-create ALL new categories in one transaction (avoids per-batch overhead)
            var todasLasCategoriasNuevas = dtosNuevos
                .Select(d => d.Categoria)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(c => !categoriasMap.ContainsKey(c.ToLowerInvariant()))
                .ToList();

            if (todasLasCategoriasNuevas.Count > 0)
            {
                try
                {
                    await _uow.EjecutarEnTransaccionAsync(async () =>
                    {
                        foreach (var nombreCat in todasLasCategoriasNuevas)
                        {
                            await _uow.Categorias.AgregarAsync(new Categoria
                            {
                                Nombre = nombreCat,
                                Id_empresa = idEmpresa,
                                Activo = true,
                            }, ct);
                        }
                        await _uow.GuardarCambiosAsync(ct);
                    });

                    // Re-query to get IDs for all categories
                    var catsActualizadas = await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa, ct);
                    foreach (var c in catsActualizadas)
                        categoriasMap[c.Nombre.ToLower().Trim()] = c.Id;
                }
                catch
                {
                    // Category creation failure is non-fatal; products will use default category
                }
            }

            // Insert new products in larger batches (500) for throughput
            const int tamLoteInsercion = 500;
            var todosLosNuevos = dtosNuevos.ToList();
            for (int offset = 0; offset < todosLosNuevos.Count; offset += tamLoteInsercion)
            {
                int batchSize = Math.Min(tamLoteInsercion, todosLosNuevos.Count - offset);
                var lote = todosLosNuevos.GetRange(offset, batchSize);
                var loteExitoso = true;

                try
                {
                    await _uow.EjecutarEnTransaccionAsync(async () =>
                    {
                        // Bulk insert del lote
                        var batchEntidades = new List<Producto>(batchSize);
                        foreach (var dto in lote)
                        {
                            var idCat = dto.IdCategoria;
                            if (!string.IsNullOrWhiteSpace(dto.Categoria)
                                && categoriasMap.TryGetValue(dto.Categoria.ToLowerInvariant(), out int idCatM))
                                idCat = idCatM;

                            batchEntidades.Add(new Producto
                            {
                                Nombre = dto.Nombre,
                                CodigoBarra = dto.CodigoBarra,
                                PrecioVentaActual = dto.PrecioVentaActual,
                                PrecioCostoActual = dto.PrecioCostoActual,
                                StockActual = dto.StockActual,
                                StockMinimo = dto.StockMinimo > 0 ? dto.StockMinimo : 10,
                                Id_empresa = dto.IdEmpresa,
                                Id_categoria = idCat > 0 ? idCat : 1,
                                Id_unidadMedida = dto.IdUnidadMedida > 0 ? dto.IdUnidadMedida : 1,
                                Activo = true,
                            });
                        }

                        await _uow.Productos.AgregarRangoAsync(batchEntidades, ct);
                        await _uow.GuardarCambiosAsync(ct);
                    });
                }
                catch
                {
                    loteExitoso = false;
                }

                if (loteExitoso)
                    resultado.Inserted += lote.Count;
                else
                {
                    resultado.Errors.AddRange(lote.Select((dto, idx) =>
                        new ImportError(offset + idx + 2, "Lote", "Error al insertar lote", GuardSeverity.Error)));
                    resultado.Skipped += lote.Count;
                }

                // Throttle progress: report every ~500 rows
                var processed = offset + batchSize;
                var porcentaje = Math.Min(100, (int)(processed / (double)total * 100));
                progreso?.Report((processed, total, $"Procesando productos nuevos... {porcentaje}%"));
            }

            // Actualizar existentes en lotes (misma convención que los inserts):
            // commit independiente por lote y re-adjuntar entidades AsNoTracking
            // con Actualizar() para que el UPDATE realmente persista.
            var todosLosActualizar = dtosActualizar.ToList();
            for (int offset = 0; offset < todosLosActualizar.Count; offset += TAMANIO_LOTE)
            {
                int batchSize = Math.Min(TAMANIO_LOTE, todosLosActualizar.Count - offset);
                var lote = todosLosActualizar.GetRange(offset, batchSize);
                var loteExitoso = true;

                try
                {
                    await _uow.EjecutarEnTransaccionAsync(async () =>
                    {
                        foreach (var dto in lote)
                        {
                            if (!productosPorCodigo.TryGetValue(dto.CodigoBarra, out var prod))
                                continue;

                            var idCat = dto.IdCategoria;
                            if (!string.IsNullOrWhiteSpace(dto.Categoria)
                                && categoriasMap.TryGetValue(dto.Categoria.ToLowerInvariant(), out int idCatM))
                                idCat = idCatM;

                            prod.Nombre = dto.Nombre;
                            prod.PrecioVentaActual = dto.PrecioVentaActual;
                            prod.PrecioCostoActual = dto.PrecioCostoActual;
                            prod.StockActual = dto.StockActual;
                            prod.StockMinimo = dto.StockMinimo > 0 ? dto.StockMinimo : 10;
                            prod.Id_categoria = idCat;
                            prod.Id_unidadMedida = dto.IdUnidadMedida > 0 ? dto.IdUnidadMedida : 1;

                            // Los productos vienen de una consulta AsNoTracking (desadjuntos):
                            // Actualizar los re-adjunta como Modified para que el SaveChanges
                            // de la transacción emita los UPDATEs.
                            _uow.Productos.Actualizar(prod);
                        }
                    });
                }
                catch
                {
                    loteExitoso = false;
                }

                if (loteExitoso)
                    resultado.Updated += lote.Count;
                else
                {
                    resultado.Errors.AddRange(lote.Select((dto, idx) =>
                        new ImportError(offset + idx + 2, "General",
                            $"Error al actualizar: {dto.CodigoBarra}", GuardSeverity.Error)));
                    resultado.Skipped += lote.Count;
                }

                var porcentajeAct = Math.Min(100, (int)(((offset + lote.Count) / (double)total) * 100));
                progreso?.Report((offset + lote.Count, total, $"Procesando actualizaciones... {porcentajeAct}%"));
            }

            progreso?.Report((total, total,
                $"Completado: {resultado.Inserted} nuevos, {resultado.Updated} actualizados, {resultado.Skipped} omitidos"));

            return resultado;
        }

        // Nuevo: Ajuste de precios por proveedor (global por empresa del proveedor)
        public async Task<(int Nuevos, int Actualizados)> AjustePreciosPorProveedorAsync(int idProveedor, decimal porcentaje, CancellationToken ct = default)
        {
            if (idProveedor <= 0) throw new ArgumentException("ID de proveedor inválido.", nameof(idProveedor));
            if (porcentaje == 0) return (0, 0);

            var proveedor = await _uow.Proveedores.ObtenerPorIdAsync(idProveedor, ct)
                ?? throw new KeyNotFoundException($"Proveedor {idProveedor} no encontrado");

            var factor = 1 + porcentaje / 100m;
            // Sin navegaciones materializadas: Update() de cada producto no reintenta
            // trackear Categoria/UnidadMedida (evita el error de duplicado de key).
            var productos = await _uow.Productos.ObtenerPorEmpresaSinNavegacionesAsync(proveedor.Id_empresa, true, ct);
            int actualizados = 0;
            foreach (var p in productos)
            {
                var nuevoVenta = p.PrecioVentaActual * factor;
                var nuevoCosto = p.PrecioCostoActual * factor;
                p.ActualizarPrecios(nuevoVenta, nuevoCosto);
                _uow.Productos.Actualizar(p);
                actualizados++;
            }

            await _uow.GuardarCambiosAsync(ct);
            // Nuevos equivale a la cantidad de productos actualizados en este approach simple
            return (actualizados, actualizados);
        }

        public async Task DesactivarAsync(int id, CancellationToken ct = default)
        {
            var producto = await _uow.Productos.ObtenerPorIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Producto {id} no encontrado");
            producto.Activo = false;
            _uow.Productos.Actualizar(producto);
            await _uow.GuardarCambiosAsync(ct);
        }

        public async Task ActualizarPreciosLoteAsync(IEnumerable<ProductoActualizarDto> dtos, CancellationToken ct = default)
        {
            var dtoList = dtos.ToList();
            var ids = dtoList.Select(d => d.IdProducto).Distinct().ToList();
            var productos = await _uow.Productos.BuscarAsync(p => ids.Contains(p.Id), ct);
            var productosDict = productos.ToDictionary(p => p.Id);

            foreach (var dto in dtoList)
            {
                if (!productosDict.TryGetValue(dto.IdProducto, out var producto))
                    continue;

                if (_sesion != null && !_sesion.HasPermission("Productos.Editar"))
                    throw new ValidationException("No tenés permiso para editar productos.");

                if (dto.PrecioVentaActual > 0)
                    producto.PrecioVentaActual = dto.PrecioVentaActual;
                if (dto.PrecioCostoActual > 0)
                    producto.PrecioCostoActual = dto.PrecioCostoActual;

                _uow.Productos.Actualizar(producto);
            }

            await _uow.GuardarCambiosAsync(ct);
        }

        // Búsqueda con StartsWith (prefijo) para uso de índices
        public async Task<IEnumerable<ProductoListadoDto>> BuscarProductosAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default)
        {
            var productos = await _uow.Productos.BuscarProductosAsync(idEmpresa, texto, idCategoria, soloActivos, take, ct);
            return productos.Select(p => new ProductoListadoDto
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
                CategoriaNombre = p.Categoria?.Nombre ?? "",
                UnidadMedida = ""
            });
        }

        // Búsqueda con Contains (subcadena) para búsquedas de texto libre
        public async Task<IEnumerable<ProductoListadoDto>> BuscarProductosContieneAsync(int idEmpresa, string? texto, int? idCategoria, bool? soloActivos, int take = 10, CancellationToken ct = default)
        {
            var productos = await _uow.Productos.BuscarProductosContieneAsync(idEmpresa, texto, idCategoria, soloActivos, take, ct);
            return productos.Select(p => new ProductoListadoDto
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
                CategoriaNombre = p.Categoria?.Nombre ?? "",
                UnidadMedida = ""
            });
        }

        // Búsqueda exacta por código de barras (case-insensitive) para escáner
        public async Task<ProductoListadoDto?> BuscarPorCodigoBarraExactoAsync(int idEmpresa, string codigoBarra, CancellationToken ct = default)
        {
            var producto = await _uow.Productos.BuscarPorCodigoBarraExactoAsync(idEmpresa, codigoBarra, ct);
            return producto == null ? null : MapearListado(producto);
        }

        public async Task<IEnumerable<CategoriaItemDto>> ObtenerCategoriasAsync(int idEmpresa, CancellationToken ct = default)
        {
            var categorias = await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa, ct);
            return categorias
                .GroupBy(c => c.Nombre.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => new CategoriaItemDto
                {
                    IdCategoria = g.First().Id,
                    Nombre = g.Key,
                    CategoriaPadre = g.First().CategoriaPadre_id
                })
                .OrderBy(c => c.Nombre);
        }

        public async Task<IEnumerable<UnidadMedidaItemDto>> ObtenerUnidadesMedidaAsync(CancellationToken ct = default)
        {
            var unidades = await _uow.Productos.ObtenerUnidadesMedidaDistintasAsync(ct);
            return unidades.Select(u => new UnidadMedidaItemDto
            {
                IdUnidadMedida = u.Id,
                Nombre = u.Nombre,
                Abreviatura = u.Abreviatura
            });
        }

        public async Task<IEnumerable<Proveedor>> ObtenerProveedoresAsync(CancellationToken ct = default)
        {
            return await _uow.Proveedores.ObtenerTodosAsync(ct);
        }

        public async Task<int> ObtenerUmbralStockCriticoAsync(int idEmpresa, CancellationToken ct = default)
        {
            var empresa = await _uow.Empresas.PrimerODefaultAsync(e => e.Id == idEmpresa, ct);
            return empresa?.UmbralStockCritico ?? 10;
        }

        public async Task<ProductoMetricasDto> ObtenerMetricasAsync(int idEmpresa, CancellationToken ct = default)
        {
            var (activos, stockBajo, sinStock) = await _uow.Productos.ObtenerMetricasAsync(idEmpresa, ct);
            return new ProductoMetricasDto
            {
                ProductosActivos   = activos,
                ProductosStockBajo = stockBajo,
                ProductosSinStock  = sinStock,
            };
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

        public async Task<CategoriaItemDto> CrearCategoriaAsync(int idEmpresa, string nombre, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío", nameof(nombre));

            var categoria = new Categoria
            {
                Nombre = nombre.Trim(),
                Id_empresa = idEmpresa,
                Activo = true,
            };

            await _uow.Categorias.AgregarAsync(categoria, ct);
            await _uow.GuardarCambiosAsync(ct);

            return new CategoriaItemDto
            {
                IdCategoria = categoria.Id,
                Nombre = categoria.Nombre,
            };
        }

        public async Task<bool> EliminarCategoriaAsync(int idCategoria, CancellationToken ct = default)
        {
            if (idCategoria <= 0) return false;

            var categoria = await _uow.Categorias.ObtenerPorIdAsync(idCategoria, ct);
            if (categoria == null) return false;

            // ── 1. Desvincular subcategorías (CategoriaPadre_id → null) ────────
            var subCategorias = await _uow.Categorias.ObtenerSubCategoriasAsync(idCategoria, ct);
            foreach (var sub in subCategorias)
                sub.CategoriaPadre_id = null;

            // ── 2. Buscar productos asociados ──────────────────────────────────
            var productosAsociados = await _uow.Productos.ObtenerPorCategoriaAsync(idCategoria, ct);

            if (productosAsociados.Count > 0)
            {
                // ── 2a. Buscar o crear "Sin Categoría" ────────────────────────
                var sinCategoria = await _uow.Categorias.ObtenerPorNombreAsync("Sin Categoría", categoria.Id_empresa, ct);

                if (sinCategoria == null)
                {
                    sinCategoria = new Categoria
                    {
                        Nombre = "Sin Categoría",
                        Id_empresa = categoria.Id_empresa,
                        Activo = true,
                    };
                    await _uow.Categorias.AgregarAsync(sinCategoria, ct);
                }

                // ── 2b. Reasignar productos usando la NAVEGACIÓN ──────────────
                //   Usar la propiedad de navegación Categoria en vez de Id_categoria
                //   permite a EF Core resolver el FK correctamente aunque la
                //   categoría aún no tenga Id asignado por la DB.
                foreach (var p in productosAsociados)
                    p.Categoria = sinCategoria;
            }

            // ── 3. Eliminar la categoría ───────────────────────────────────────
            //   TODO en un SOLO SaveChangesAsync para mantener consistencia:
            //   EF Core ordena automáticamente INSERT → UPDATE → DELETE.
            _uow.Categorias.Eliminar(categoria);
            await _uow.GuardarCambiosAsync(ct);
            return true;
        }

        public async Task<CategoriaItemDto> ActualizarCategoriaAsync(int idCategoria, string nuevoNombre, CancellationToken ct = default)
        {
            if (idCategoria <= 0) throw new ArgumentException("ID de categoría inválido", nameof(idCategoria));
            if (string.IsNullOrWhiteSpace(nuevoNombre)) throw new ArgumentException("El nombre no puede estar vacío", nameof(nuevoNombre));

            var categoria = await _uow.Categorias.ObtenerPorIdAsync(idCategoria, ct)
                ?? throw new KeyNotFoundException($"Categoría {idCategoria} no encontrada");

            categoria.Nombre = nuevoNombre.Trim();
            _uow.Categorias.Actualizar(categoria);
            await _uow.GuardarCambiosAsync(ct);

            return new CategoriaItemDto
            {
                IdCategoria = categoria.Id,
                Nombre = categoria.Nombre,
            };
        }

        public async Task<int> EliminarProductosPorCategoriaAsync(int idCategoria, CancellationToken ct = default)
        {
            if (idCategoria <= 0) return 0;

            var productos = await _uow.Productos.ObtenerPorCategoriaAsync(idCategoria, ct);

            if (productos.Count == 0) return 0;

            _uow.Productos.EliminarRango(productos);
            await _uow.GuardarCambiosAsync(ct);
            return productos.Count;
        }

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