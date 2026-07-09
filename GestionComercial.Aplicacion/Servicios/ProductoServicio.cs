using FluentValidation;
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

            // ── 1. Cargar datos existentes (UNA SOLA CONSULTA) ──────────────────
            // ✅ Optimización #1: Cargar todos los productos en un Dictionary,
            //    eliminando el N+1 de ObtenerPorCodigoBarraAsync por cada DTO.
            var productosPorCodigo = (await _uow.Productos.Consultar()
                    .Where(p => p.Id_empresa == idEmpresa && !string.IsNullOrEmpty(p.CodigoBarra))
                    .ToListAsync())
                .ToDictionary(p => p.CodigoBarra!, StringComparer.OrdinalIgnoreCase);

            var categoriasExistentes = (await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa))
                .ToDictionary(c => c.Nombre.ToLower().Trim(), c => c.Id);

            // ── 2. Ejecutar TODO dentro de una transacción ─────────────────────
            //    Si falla en cualquier punto (crear categorías, procesar, guardar),
            //    TODO se revierte — no quedan categorías huérfanas.
            var mapCategorias = categoriasExistentes;
            var resultado = (Nuevos: 0, Actualizados: 0, Omitidos: 0);

            await _uow.EjecutarEnTransaccionAsync(async () =>
            {
                var categoriasMap = new Dictionary<string, int>(mapCategorias, StringComparer.OrdinalIgnoreCase);
                var nuevasCategorias = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Detectar categorías nuevas
                foreach (var dto in productosAImportar)
                {
                    if (string.IsNullOrWhiteSpace(dto.Categoria)) continue;
                    var catKey = dto.Categoria.Trim();
                    if (!categoriasMap.ContainsKey(catKey.ToLower()))
                        nuevasCategorias.Add(catKey);
                }

                // Crear categorías nuevas y obtener IDs
                if (nuevasCategorias.Count > 0)
                {
                    foreach (var nombreCat in nuevasCategorias)
                    {
                        _uow.Categorias.AgregarAsync(new Categoria
                        {
                            Nombre = nombreCat,
                            Id_empresa = idEmpresa,
                            Activo = true,
                        });
                    }
                    await _uow.GuardarCambiosAsync();

                    var categoriasActualizadas = await _uow.Categorias.ObtenerPorEmpresaAsync(idEmpresa);
                    foreach (var c in categoriasActualizadas)
                        categoriasMap[c.Nombre.ToLower().Trim()] = c.Id;
                }

                // Procesar productos
                var batchNuevos = new List<Producto>();
                foreach (var dto in productosAImportar)
                {
                    procesados++;

                    if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.PrecioVentaActual <= 0)
                    {
                        resultado.Omitidos++;
                        progreso?.Report((procesados, total, $"Omitido: '{dto.Nombre}' (datos inválidos)..."));
                        continue;
                    }

                    var idCategoria = dto.IdCategoria;
                    if (!string.IsNullOrWhiteSpace(dto.Categoria))
                    {
                        var catKey = dto.Categoria.ToLower().Trim();
                        if (categoriasMap.TryGetValue(catKey, out int idCat))
                            idCategoria = idCat;
                    }

                    var productoExistente = actualizarExistentes
                        && !string.IsNullOrWhiteSpace(dto.CodigoBarra)
                        && productosPorCodigo.TryGetValue(dto.CodigoBarra, out var prod)
                            ? prod
                            : null;

                    if (productoExistente != null)
                    {
                        productoExistente.Nombre = dto.Nombre;
                        productoExistente.PrecioVentaActual = dto.PrecioVentaActual;
                        productoExistente.PrecioCostoActual = dto.PrecioCostoActual;
                        productoExistente.StockActual = dto.StockActual;
                        productoExistente.StockMinimo = dto.StockMinimo > 0 ? dto.StockMinimo : 10;
                        productoExistente.Id_categoria = idCategoria;
                        productoExistente.Id_unidadMedida = dto.IdUnidadMedida > 0 ? dto.IdUnidadMedida : 1;
                        resultado.Actualizados++;
                    }
                    else
                    {
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
                        resultado.Nuevos++;
                    }

                    if (procesados % PROGRESS_STEP == 0)
                    {
                        var porcentaje = (int)((procesados / (double)total) * 100);
                        progreso?.Report((procesados, total, $"Importando {porcentaje}% ({procesados}/{total})..."));
                    }
                }

                // Agregar nuevos productos al tracker (sin SaveChanges — lo hace EjecutarEnTransaccionAsync)
                if (batchNuevos.Count > 0)
                    await _uow.Productos.AgregarRangoAsync(batchNuevos);

                // ✅ EjecutarEnTransaccionAsync llama a SaveChangesAsync automáticamente al salir
            });

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
            // ⚡ Sin tracking (solo lectura) + async real
            var unidades = await _uow.Productos.ConsultarSinTracking()
                .Include(p => p.UnidadMedida)
                .Select(p => p.UnidadMedida)
                .Where(u => u != null)
                .Distinct()
                .Select(u => new UnidadMedidaItemDto
                {
                    IdUnidadMedida = u!.Id,
                    Nombre = u.Nombre,
                    Abreviatura = u.Abreviatura
                })
                .ToListAsync();
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
