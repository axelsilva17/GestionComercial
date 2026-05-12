using ClosedXML.Excel;
using GestionComercial.Aplicacion.DTOs.Inventario;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace GestionComercial.Aplicacion.Servicios
{
    public class InventarioServicio : IInventarioServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<InventarioServicio>? _logger;

        public InventarioServicio(IUnitOfWork uow, ILogger<InventarioServicio>? logger = null)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<(IEnumerable<MovimientoStockDto> Movimientos, int Total)> ObtenerMovimientosAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int pagina,
            int itemsPorPagina,
            int idEmpresa)
        {
            // Obtener todos los movimientos del período
            var movimientos = await _uow.MovimientosStock.ObtenerPorFechaAsync(fechaDesde, fechaHasta.AddDays(1), null);

            // Filtrar por tipo
            if (!string.IsNullOrWhiteSpace(filtroTipo) && filtroTipo != "Todos")
            {
                var tipoEnum = Enum.Parse<TipoMovimientoStockEnum>(filtroTipo, ignoreCase: true);
                movimientos = movimientos.Where(m => m.TipoMovimiento == (int)tipoEnum);
            }

            // Filtrar por usuario
            if (!string.IsNullOrWhiteSpace(filtroUsuario) && filtroUsuario != "Todos")
            {
                var termino = filtroUsuario.ToLower();
                movimientos = movimientos.Where(m =>
                    m.Usuario != null &&
                    (m.Usuario.Nombre.ToLower().Contains(termino) ||
                     m.Usuario.Apellido.ToLower().Contains(termino)));
            }

            // Filtrar por búsqueda (producto o código)
            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                var termino = textoBusqueda.ToLower();
                movimientos = movimientos.Where(m =>
                    (m.Producto?.Nombre?.ToLower().Contains(termino) ?? false) ||
                    (m.Producto?.CodigoBarra?.ToLower().Contains(termino) ?? false));
            }

            // Filtrar por sucursal
            if (!string.IsNullOrWhiteSpace(filtroSucursal) && filtroSucursal != "Todas")
            {
                movimientos = movimientos.Where(m => m.Sucursal?.Nombre == filtroSucursal);
            }

            var listaOrdenada = movimientos
                .OrderByDescending(m => m.Fecha)
                .ToList();

            var total = listaOrdenada.Count;

            // Paginación
            var paginados = listaOrdenada
                .Skip((pagina - 1) * itemsPorPagina)
                .Take(itemsPorPagina)
                .Select(m => new MovimientoStockDto
                {
                    IdMovimiento = m.Id,
                    TipoMovimiento = ((TipoMovimientoStockEnum)m.TipoMovimiento).ToString(),
                    Observacion = m.Observacion ?? string.Empty,
                    Cantidad = (int)m.Cantidad,
                    Fecha = m.Fecha,
                    IdProducto = m.Id_producto,
                    ProductoNombre = m.Producto?.Nombre ?? "Sin producto",
                    CodigoBarra = m.Producto?.CodigoBarra ?? string.Empty,
                    CategoriaNombre = m.Producto?.Categoria?.Nombre ?? "Sin categoría",
                    SucursalNombre = m.Sucursal?.Nombre ?? "Sin sucursal",
                    UsuarioNombre = m.Usuario != null ? $"{m.Usuario.Nombre} {m.Usuario.Apellido}" : "Sistema"
                })
                .ToList();

            _logger?.LogDebug("Obtenidos {Count} movimientos de {Total} total", paginados.Count, total);

            return (paginados, total);
        }

        public async Task<byte[]> ExportarAExcelAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int idEmpresa)
        {
            // Obtener todos los movimientos sin paginación
            var (movimientos, _) = await ObtenerMovimientosAsync(
                textoBusqueda, filtroTipo, filtroUsuario, filtroSucursal,
                fechaDesde, fechaHasta, 1, int.MaxValue, idEmpresa);

            var lista = movimientos.ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Inventario");

            // ── Header ─
            var headerRow = ws.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");
            headerRow.Style.Font.FontColor = XLColor.White;

            string[] headers = { "Fecha", "Tipo", "Producto", "Código", "Categoría", "Cantidad", "Sucursal", "Usuario", "Observación" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(1, i + 1).Value = headers[i];
            }

            // ── Data ─
            int row = 2;
            foreach (var m in lista)
            {
                ws.Cell(row, 1).Value = m.Fecha.ToString("dd/MM/yyyy HH:mm");
                ws.Cell(row, 2).Value = m.TipoMovimiento;
                ws.Cell(row, 3).Value = m.ProductoNombre;
                ws.Cell(row, 4).Value = m.CodigoBarra;
                ws.Cell(row, 5).Value = m.CategoriaNombre;
                ws.Cell(row, 6).Value = m.Cantidad;
                ws.Cell(row, 7).Value = m.SucursalNombre;
                ws.Cell(row, 8).Value = m.UsuarioNombre;
                ws.Cell(row, 9).Value = m.Observacion;

                // Colorear según tipo
                var cellTipo = ws.Cell(row, 2);
                if (m.TipoMovimiento == "Entrada")
                    cellTipo.Style.Font.FontColor = XLColor.Green;
                else if (m.TipoMovimiento == "Salida")
                    cellTipo.Style.Font.FontColor = XLColor.Red;
                else
                    cellTipo.Style.Font.FontColor = XLColor.Orange;

                row++;
            }

            // ── Ajustar ancho columnas ─
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa)
        {
            var productos = await _uow.Productos.ObtenerStockCriticoAsync(idEmpresa);
            return productos.Select(p => new ProductoListadoDto
            {
                IdProducto = p.Id,
                Nombre = p.Nombre,
                CodigoBarra = p.CodigoBarra,
                StockActual = (int)p.StockActual,
                StockMinimo = (int)p.StockMinimo,
                UnidadMedida = p.UnidadMedida?.Nombre ?? "",
                CategoriaNombre = p.Categoria?.Nombre ?? ""
            });
        }

        public async Task RegistrarMovimientoAsync(
            int idProducto,
            string tipoMovimiento,
            decimal cantidad,
            string? observacion,
            int idSucursal,
            int idUsuario,
            bool guardarCambios = true)
        {
            _logger?.LogDebug("[Inventario] Iniciando RegistrarMovimientoAsync - Producto: {IdProducto}, Tipo: {Tipo}, Cantidad: {Cantidad}, guardarCambios: {Guardar}",
                idProducto, tipoMovimiento, cantidad, guardarCambios);

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

            // Obtener producto actual
            var producto = await _uow.Productos.ObtenerPorIdAsync(idProducto)
                ?? throw new KeyNotFoundException($"Producto {idProducto} no encontrado");

            var stockAnterior = producto.StockActual;
            var tipoEnum = Enum.Parse<TipoMovimientoStockEnum>( tipoMovimiento, ignoreCase: true);

            // Crear movimiento usando factory method DDD
            MovimientoStock movimiento = tipoEnum switch
            {
                TipoMovimientoStockEnum.Entrada => MovimientoStock.Entrada(
                    cantidad, stockAnterior, idProducto, idSucursal, idUsuario, observacion),
                
                TipoMovimientoStockEnum.Salida => MovimientoStock.Salida(
                    cantidad, stockAnterior, idProducto, idSucursal, idUsuario, observacion),
                
                TipoMovimientoStockEnum.Ajuste => MovimientoStock.Ajuste(
                    cantidad, stockAnterior, idProducto, idSucursal, idUsuario, observacion),
                
                _ => throw new ArgumentException($"Tipo de movimiento inválido: {tipoMovimiento}")
            };

            // Actualizar stock del producto
            producto.StockActual = movimiento.StockNuevo;
            _uow.Productos.Actualizar(producto);

            // Adjuntar las entidades relacionadas al contexto si no están ya adjuntadas
            // Esto es necesario para que EF Core pueda guardar las foreign keys
            var sucursal = await _uow.Sucursales.ObtenerPorIdAsync(idSucursal);
            var usuario = await _uow.Usuarios.ObtenerPorIdAsync(idUsuario);
            
            // Asignar las entidades al movimiento
            movimiento.Sucursal = sucursal!;
            movimiento.Usuario = usuario!;
            movimiento.Producto = producto;

            // Guardar movimiento
            await _uow.MovimientosStock.AgregarAsync(movimiento);

            _logger?.LogInformation(
                "Movimiento agregado al DBSet: {Tipo}, Producto {ProductoId}, Cantidad {Cantidad}, Stock: {Anterior} -> {Nuevo}",
                tipoMovimiento, idProducto, cantidad, stockAnterior, movimiento.StockNuevo);

            // Guardar cambios solo si se solicita (para evitar transacciones anidadas)
            if (guardarCambios)
            {
                _logger?.LogDebug("[Inventario] Antes de GuardarCambiosAsync");
                await _uow.GuardarCambiosAsync();
                _logger?.LogInformation("[Inventario] GuardarCambios completado para movimiento de stock - Producto: {IdProducto}", idProducto);
            }
            else
            {
                _logger?.LogInformation("[Inventario] GuardarCambios omitido (dentro de transacción), se guardará al final - Producto: {IdProducto}", idProducto);
            }

            _logger?.LogDebug("[Inventario] Fin RegistrarMovimientoAsync - Producto: {IdProducto}", idProducto);
        }

        public async Task<IEnumerable<MovimientoStockDto>> ObtenerMovimientosPorProductoAsync(int idProducto)
        {
            var movimientos = await _uow.MovimientosStock.ObtenerPorProductoAsync(idProducto);

            return movimientos.Select(m => new MovimientoStockDto
            {
                IdMovimiento = m.Id,
                TipoMovimiento = ((TipoMovimientoStockEnum)m.TipoMovimiento).ToString(),
                Observacion = m.Observacion ?? string.Empty,
                Cantidad = (int)m.Cantidad,
                Fecha = m.Fecha,
                IdProducto = m.Id_producto,
                ProductoNombre = m.Producto?.Nombre ?? "Sin producto",
                CodigoBarra = m.Producto?.CodigoBarra ?? string.Empty,
                CategoriaNombre = m.Producto?.Categoria?.Nombre ?? "Sin categoría",
                SucursalNombre = m.Sucursal?.Nombre ?? "Sin sucursal",
                UsuarioNombre = m.Usuario != null ? $"{m.Usuario.Nombre} {m.Usuario.Apellido}" : "Sistema"
            }).ToList();
        }
    }
}