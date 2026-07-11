using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.Servicios
{
public class ProductoServicio : IProductoServicio
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<ProductoCrearDto>? _crearValidator;
    private readonly IValidator<ProductoActualizarDto>? _actualizarValidator;

    public ProductoServicio(
        IUnitOfWork uow,
        IValidator<ProductoCrearDto>? crearValidator = null,
        IValidator<ProductoActualizarDto>? actualizarValidator = null)
    {
        _uow = uow;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

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
            // Validar entrada con FluentValidation
            if (_crearValidator != null)
            {
                var result = await _crearValidator.ValidateAsync(dto);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

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
            // Validar entrada con FluentValidation
            if (_actualizarValidator != null)
            {
                var result = await _actualizarValidator.ValidateAsync(dto);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

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

        ///         /// Importación masiva optimizada.
        /// 1) Crea categorías nuevas primero para tener IDs válidos.
        /// 2) Para cada producto: si el código de barra ya existe y actualizarExistentes = true,
        ///    actualiza el producto existente; si no existe (o no se busca por barra), crea uno nuevo.
        /// 3) Procesa en batches de 50 para no agotar memoria.
        public async Task<(int Nuevos, int Actualizados, int Omitidos)> ImportarMasivoAsync(
            IEnumerable<ProductoImportarDto> dtos,
            bool actualizarExistentes,
            IProgress<(int current, int total, string message)>? progreso = null)
        {
            var productosAImportar = dtos.ToList();
            var total = productosAImportar.Count;

            var idEmpresa = productosAImportar.FirstOrDefault()?.IdEmpresa ?? 0;
            if (idEmpresa <= 0)
                return (0, 0, total);

            // ── 1. Cargar datos existentes (UNA SOLA CONSULTA) ──────────────────
            // ✅ Optimización #1: Cargar todos los productos en un Dictionary,
            //    eliminando el N+1 de ObtenerPorCodigoBarraAsync por cada DTO.
            var productosPorCodigo = (await _uow.Productos.ObtenerConCodigoBarraPorEmpresaAsync(idEmpresa))
                .ToDictionary(p => p.CodigoBarra!, StringComparer.OrdinalIgnoreCase);

            var categoriasExistentes = (await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa))
                .GroupBy(c => c.Nombre.ToLower().Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

            // ── 2. Ejecutar TODO dentro de una transacción ─────────────────────
            //    Si falla en cualquier punto (crear categorías, procesar, guardar),
            //    TODO se revierte — no quedan categorías huérfanas.
            var mapCategorias = categoriasExistentes;
            var resultado = (Nuevos: 0, Actualizados: 0, Omitidos: 0);

            // ⚠️ Pre-validar y normalizar: trim + uppercase en todos los códigos de barra
            //    para evitar UNIQUE constraint por diferencias de espacios o mayúsculas.
            //    CRÍTICO: códigos vacíos → null (no "") porque SQLite trata NULL como distinto
            //    en UNIQUE constraints, pero "" como valor repetible viola el constraint.
            foreach (var dto in productosAImportar)
            {
                dto.CodigoBarra = string.IsNullOrWhiteSpace(dto.CodigoBarra)
                    ? null
                    : dto.CodigoBarra.Trim().ToUpperInvariant();
                dto.Nombre = dto.Nombre?.Trim() ?? string.Empty;
                dto.Categoria = dto.Categoria?.Trim() ?? string.Empty;
            }

            // ⚠️ Reconstruir el diccionario con claves NORMALIZADAS (trim + upper)
            //    porque los datos en DB pueden tener espacios extra o diferencias de mayúsculas.
            productosPorCodigo = productosPorCodigo
                .GroupBy(p => p.Key.Trim().ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.First().Value, StringComparer.Ordinal);

            // ── 2a. PRE-VALIDACIÓN: clasificar TODOS los DTOs antes de la transacción ──
            var dtosNuevos = new List<ProductoImportarDto>();
            var dtosActualizar = new List<ProductoImportarDto>();
            var dtosOmitir = new List<(ProductoImportarDto, string)>();
            var barcodesVistos = new HashSet<string>(StringComparer.Ordinal);

            foreach (var dto in productosAImportar)
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.PrecioVentaActual <= 0)
                {
                    dtosOmitir.Add((dto, "datos inválidos"));
                    continue;
                }

                // Duplicado dentro del mismo archivo
                if (!string.IsNullOrWhiteSpace(dto.CodigoBarra) && !barcodesVistos.Add(dto.CodigoBarra))
                {
                    dtosOmitir.Add((dto, "código duplicado en el archivo"));
                    continue;
                }

                // Ya existe en la DB
                if (!string.IsNullOrWhiteSpace(dto.CodigoBarra)
                    && productosPorCodigo.TryGetValue(dto.CodigoBarra, out _))
                {
                    if (!actualizarExistentes)
                    {
                        dtosOmitir.Add((dto, "ya existe en BD y actualizar está desactivado"));
                        continue;
                    }
                    dtosActualizar.Add(dto);
                    continue;
                }

                dtosNuevos.Add(dto);
            }

            // ── 2b. Ejecutar TODO dentro de una transacción ─────────────────────
            var categoriasMap = new Dictionary<string, int>(mapCategorias, StringComparer.OrdinalIgnoreCase);

            await _uow.EjecutarEnTransaccionAsync(async () =>
            {
                // ── Categorías nuevas ──────────────────────────────────────────
                var nuevasCategorias = dtosNuevos
                    .Concat(dtosActualizar)
                    .Select(d => d.Categoria)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Select(c => c.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Where(c => !categoriasMap.ContainsKey(c.ToLowerInvariant()))
                    .ToList();

                foreach (var nombreCat in nuevasCategorias)
                {
                    await _uow.Categorias.AgregarAsync(new Categoria
                    {
                        Nombre = nombreCat,
                        Id_empresa = idEmpresa,
                        Activo = true,
                    });
                }

                if (nuevasCategorias.Count > 0)
                {
                    await _uow.GuardarCambiosAsync();
                    var catsActualizadas = await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa);
                    foreach (var c in catsActualizadas)
                        categoriasMap[c.Nombre.ToLower().Trim()] = c.Id;
                }

                // ── Actualizar existentes ──────────────────────────────────────
                foreach (var dto in dtosActualizar)
                {
                    if (!productosPorCodigo.TryGetValue(dto.CodigoBarra, out var prod)) continue;

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
                }

                // ── Insertar nuevos en BATCHES de 50 ──────────────────────────
                const int BATCH_SIZE = 50;
                for (int offset = 0; offset < dtosNuevos.Count; offset += BATCH_SIZE)
                {
                    var batch = dtosNuevos.Skip(offset).Take(BATCH_SIZE).ToList();
                    var batchEntidades = new List<Producto>();

                    foreach (var dto in batch)
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

                    await _uow.Productos.AgregarRangoAsync(batchEntidades);
                    await _uow.GuardarCambiosAsync();

                    var porcentaje = (int)(((offset + batch.Count) / (double)dtosNuevos.Count) * 100);
                    progreso?.Report((offset + batch.Count, dtosNuevos.Count, $"Insertando {porcentaje}%..."));
                }
            });

            // ── 3. Resultado final ─────────────────────────────────────────────
            resultado.Nuevos = dtosNuevos.Count;
            resultado.Actualizados = dtosActualizar.Count;
            resultado.Omitidos = dtosOmitir.Count;

            var totalProcesados = dtosNuevos.Count + dtosActualizar.Count + dtosOmitir.Count;
            progreso?.Report((totalProcesados, totalProcesados,
                $"Completado: {resultado.Nuevos} nuevos, {resultado.Actualizados} actualizados, {resultado.Omitidos} omitidos"));

            progreso?.Report((total, total, $"Completado: {resultado.Nuevos} nuevos, {resultado.Actualizados} actualizados, {resultado.Omitidos} omitidos"));
            return resultado;
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

        public async Task<IEnumerable<UnidadMedidaItemDto>> ObtenerUnidadesMedidaAsync()
        {
            var unidades = await _uow.Productos.ObtenerUnidadesMedidaDistintasAsync();
            return unidades.Select(u => new UnidadMedidaItemDto
            {
                IdUnidadMedida = u.Id,
                Nombre = u.Nombre,
                Abreviatura = u.Abreviatura
            });
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

        public async Task<CategoriaItemDto> CrearCategoriaAsync(int idEmpresa, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío", nameof(nombre));

            var categoria = new Categoria
            {
                Nombre = nombre.Trim(),
                Id_empresa = idEmpresa,
                Activo = true,
            };

            await _uow.Categorias.AgregarAsync(categoria);
            await _uow.GuardarCambiosAsync();

            return new CategoriaItemDto
            {
                IdCategoria = categoria.Id,
                Nombre = categoria.Nombre,
            };
        }

        public async Task<bool> EliminarCategoriaAsync(int idCategoria)
        {
            if (idCategoria <= 0) return false;

            var categoria = await _uow.Categorias.ObtenerPorIdAsync(idCategoria);
            if (categoria == null) return false;

            // ── 1. Desvincular subcategorías (CategoriaPadre_id → null) ────────
            var subCategorias = await _uow.Categorias.ObtenerSubCategoriasAsync(idCategoria);
            foreach (var sub in subCategorias)
                sub.CategoriaPadre_id = null;

            // ── 2. Buscar productos asociados ──────────────────────────────────
            var productosAsociados = await _uow.Productos.ObtenerPorCategoriaAsync(idCategoria);

            if (productosAsociados.Count > 0)
            {
                // ── 2a. Buscar o crear "Sin Categoría" ────────────────────────
                var sinCategoria = await _uow.Categorias.ObtenerPorNombreAsync("Sin Categoría", categoria.Id_empresa);

                if (sinCategoria == null)
                {
                    sinCategoria = new Categoria
                    {
                        Nombre = "Sin Categoría",
                        Id_empresa = categoria.Id_empresa,
                        Activo = true,
                    };
                    await _uow.Categorias.AgregarAsync(sinCategoria);
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
            await _uow.GuardarCambiosAsync();
            return true;
        }

        public async Task<CategoriaItemDto> ActualizarCategoriaAsync(int idCategoria, string nuevoNombre)
        {
            if (idCategoria <= 0) throw new ArgumentException("ID de categoría inválido", nameof(idCategoria));
            if (string.IsNullOrWhiteSpace(nuevoNombre)) throw new ArgumentException("El nombre no puede estar vacío", nameof(nuevoNombre));

            var categoria = await _uow.Categorias.ObtenerPorIdAsync(idCategoria)
                ?? throw new KeyNotFoundException($"Categoría {idCategoria} no encontrada");

            categoria.Nombre = nuevoNombre.Trim();
            _uow.Categorias.Actualizar(categoria);
            await _uow.GuardarCambiosAsync();

            return new CategoriaItemDto
            {
                IdCategoria = categoria.Id,
                Nombre = categoria.Nombre,
            };
        }

        public async Task<int> EliminarProductosPorCategoriaAsync(int idCategoria)
        {
            if (idCategoria <= 0) return 0;

            var productos = await _uow.Productos.ObtenerPorCategoriaAsync(idCategoria);

            if (productos.Count == 0) return 0;

            _uow.Productos.EliminarRango(productos);
            await _uow.GuardarCambiosAsync();
            return productos.Count;
        }

        public async Task<int> ObtenerUmbralStockCriticoAsync(int idEmpresa)
        {
            var empresa = await _uow.Empresas.PrimerODefaultAsync(e => e.Id == idEmpresa);
            return empresa?.UmbralStockCritico ?? 10;
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