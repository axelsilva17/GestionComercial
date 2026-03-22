using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly IUnitOfWork _uow;
        public ProductoServicio(IUnitOfWork uow) => _uow = uow;

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