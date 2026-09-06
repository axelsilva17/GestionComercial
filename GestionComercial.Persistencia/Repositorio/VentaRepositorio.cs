using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class VentaRepositorio : RepositorioBase<Venta>, IVentaRepostorio
    {
        public VentaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Venta?> ObtenerConDetallesAsync(int idVenta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .Include(v => v.Detalles).ThenInclude(d => d.Descuentos)
                .Include(v => v.Pagos).ThenInclude(p => p.MetodoPago)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .AsSplitQuery()
                .FirstOrDefaultAsync(v => v.Id == idVenta, ct);

        public async Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta && v.Id_sucursal == idSucursal)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(v => v.Id_cliente == idCliente)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Venta>> ObtenerConDetallesPorFechaAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(v => v.Sucursal.Id_empresa == idEmpresa
                         && v.Fecha >= desde
                         && v.Fecha <= hasta)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto).ThenInclude(p => p!.Categoria)
                .Include(v => v.Usuario)
                .Include(v => v.Sucursal)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync(ct);

        public async Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal, CancellationToken ct = default)
        {
            var hoy = DateTime.Today;
            return await _dbSet
                .Where(v => v.Id_sucursal == idSucursal && v.Fecha >= hoy && v.Estado == 2)
                .SumAsync(v => v.TotalFinal, ct);
        }

        public async Task<IEnumerable<Venta>> ObtenerVentasAnuladasAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta && v.Estado == 3) // 3 = Anulada
                .Include(v => v.Usuario)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<Venta>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
                .Include(v => v.Usuario)
                .Include(v => v.Pagos).ThenInclude(p => p.MetodoPago)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync(ct);

        public async Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>>
            ObtenerTopProductosAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, int top, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<TopProductoAgrupado>(
                    @"SELECT vd.Id_producto AS IdProducto,
                             p.Nombre,
                             COALESCE(c.Nombre, '') AS Categoria,
                             CAST(SUM(vd.Cantidad) AS INTEGER) AS Cantidad,
                             SUM(vd.Subtotal) AS Ingresos,
                             SUM(vd.CostoUnitario * vd.Cantidad) AS Costo,
                             MAX(v.Fecha) AS UltimaFecha
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      LEFT JOIN Categoria c ON p.Id_categoria = c.Id
                      WHERE v.Id_sucursal = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY Cantidad DESC
                      LIMIT {3}",
                    idSucursal, desde, hasta, top)
                .ToListAsync(ct);
            return rows.Select(r => (r.IdProducto, r.Nombre, r.Categoria, r.Cantidad, r.Ingresos, r.Costo, r.UltimaFecha)).ToList();
        }

        public async Task<List<(int IdProducto, string Nombre, string Categoria, decimal StockActual, int CantidadVendida, DateTime? UltimaVenta)>>
            ObtenerRotacionProductosAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int? top = null, CancellationToken ct = default)
        {
            var sql = @"SELECT vd.Id_producto AS IdProducto,
                             p.Nombre,
                             COALESCE(c.Nombre, '') AS Categoria,
                             p.StockActual,
                             CAST(SUM(vd.Cantidad) AS INTEGER) AS CantidadVendida,
                             MAX(v.Fecha) AS UltimaVenta
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      LEFT JOIN Categoria c ON p.Id_categoria = c.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY CantidadVendida DESC"
                + (top.HasValue ? " LIMIT {3}" : "");

            var parameters = top.HasValue
                ? new object[] { idEmpresa, desde, hasta, top.Value }
                : new object[] { idEmpresa, desde, hasta };

            var rows = await _context.Database
                .SqlQueryRaw<RotacionProductoAgrupado>(sql, parameters)
                .ToListAsync(ct);
            return rows.Select(r => (r.IdProducto, r.Nombre, r.Categoria, r.StockActual, r.CantidadVendida, r.UltimaVenta)).ToList();
        }

        public async Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>>
            ObtenerTopProductosPorEmpresaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int? top = null, CancellationToken ct = default)
        {
            // `top` opcional: null devuelve todos los productos sin LIMIT (exportación completa);
            // un valor limita las filas en SQL (grillas en pantalla).
            var sql = @"SELECT vd.Id_producto AS IdProducto,
                             p.Nombre,
                             COALESCE(c.Nombre, '') AS Categoria,
                             CAST(SUM(vd.Cantidad) AS INTEGER) AS Cantidad,
                             SUM(vd.Subtotal) AS Ingresos,
                             SUM(vd.CostoUnitario * vd.Cantidad) AS Costo,
                             MAX(v.Fecha) AS UltimaFecha
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      LEFT JOIN Categoria c ON p.Id_categoria = c.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY Cantidad DESC"
                + (top.HasValue ? " LIMIT {3}" : "");

            var parameters = top.HasValue
                ? new object[] { idEmpresa, desde, hasta, top.Value }
                : new object[] { idEmpresa, desde, hasta };

            var rows = await _context.Database
                .SqlQueryRaw<TopProductoAgrupado>(sql, parameters)
                .ToListAsync(ct);
            return rows.Select(r => (r.IdProducto, r.Nombre, r.Categoria, r.Cantidad, r.Ingresos, r.Costo, r.UltimaFecha)).ToList();
        }

        public async Task<List<(string Dia, decimal Total, int Cantidad)>>
            ObtenerVentasPorDiaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<VentaPorDiaAgrupado>(
                    @"SELECT strftime('%Y-%m-%d', v.Fecha) AS Dia,
                             SUM(v.TotalFinal) AS Total,
                             COUNT(*) AS Cantidad
                      FROM Venta v
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY strftime('%Y-%m-%d', v.Fecha)
                      ORDER BY strftime('%Y-%m-%d', v.Fecha)",
                    idEmpresa, desde, hasta)
                .ToListAsync(ct);
            return rows.Select(r => (r.Dia, r.Total, r.Cantidad)).ToList();
        }

        public async Task<List<(int IdSucursal, string SucursalNombre, decimal Total, int Cantidad)>>
            ObtenerVentasPorSucursalAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<VentaPorSucursalAgrupado>(
                    @"SELECT v.Id_sucursal AS IdSucursal,
                             s.Nombre AS SucursalNombre,
                             SUM(v.TotalFinal) AS Total,
                             COUNT(*) AS Cantidad
                      FROM Venta v
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY v.Id_sucursal
                      ORDER BY Total DESC",
                    idEmpresa, desde, hasta)
                .ToListAsync(ct);
            return rows.Select(r => (r.IdSucursal, r.SucursalNombre, r.Total, r.Cantidad)).ToList();
        }

        public async Task<List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>>
            ObtenerVentasPorVendedorAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<VentaPorVendedorAgrupado>(
                    @"SELECT v.Id_usuario AS IdUsuario,
                             u.Nombre || ' ' || u.Apellido AS UsuarioNombre,
                             s.Nombre AS SucursalNombre,
                             COUNT(*) AS CantidadVentas,
                             SUM(v.TotalFinal) AS TotalVendido,
                             SUM(v.TotalDescuento) AS TotalDescuentos
                      FROM Venta v
                      INNER JOIN Usuario u ON v.Id_usuario = u.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE v.Id_sucursal = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY v.Id_usuario
                      ORDER BY TotalVendido DESC",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);
            return rows.Select(r => (r.IdUsuario, r.UsuarioNombre, r.SucursalNombre, r.CantidadVentas, r.TotalVendido, r.TotalDescuentos)).ToList();
        }

        // ── Nuevo: ventas por vendedor filtrando por Id_usuario ────────────────
        public async Task<List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>>
            ObtenerVentasPorVendedorAsync(int idSucursal, int idUsuario, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<VentaPorVendedorAgrupado>(
                    @"SELECT v.Id_usuario AS IdUsuario,
                             u.Nombre || ' ' || u.Apellido AS UsuarioNombre,
                             s.Nombre AS SucursalNombre,
                             COUNT(*) AS CantidadVentas,
                             SUM(v.TotalFinal) AS TotalVendido,
                             SUM(v.TotalDescuento) AS TotalDescuentos
                      FROM Venta v
                      INNER JOIN Usuario u ON v.Id_usuario = u.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE v.Id_sucursal = {0}
                        AND v.Id_usuario = {3}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY v.Id_usuario
                      ORDER BY TotalVendido DESC",
                    idSucursal, desde, hasta, idUsuario)
                .ToListAsync(ct);
            return rows.Select(r => (r.IdUsuario, r.UsuarioNombre, r.SucursalNombre, r.CantidadVentas, r.TotalVendido, r.TotalDescuentos)).ToList();
        }

        public async Task<(decimal TotalVentas, int TotalTransacciones, decimal TicketPromedio)?>
            ObtenerKpisVentasAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<KpiVentasAgrupado>(
                    @"SELECT SUM(v.TotalFinal) AS TotalVentas,
                             COUNT(*) AS TotalTransacciones,
                             CASE WHEN COUNT(*) > 0 THEN SUM(v.TotalFinal) / COUNT(*) ELSE 0 END AS TicketPromedio
                      FROM Venta v
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3",
                    idEmpresa, desde, hasta)
                .ToListAsync(ct);
            var r = rows.FirstOrDefault();
            if (r == null || r.TotalVentas == 0) return null;
            return (r.TotalVentas, r.TotalTransacciones, r.TicketPromedio);
        }

        // Agregación SQL del resumen de ventas por sucursal y período, sin filtrar por Estado
        // (incluye anuladas) para replicar la semántica in-memory original del reporte admin.
        public async Task<(decimal TotalVentas, int CantidadVentas, decimal PromedioVenta)>
            ObtenerResumenVentasPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<ResumenVentasSucursalRaw>(
                    @"SELECT SUM(CAST(v.TotalFinal AS REAL)) AS TotalVentas,
                             COUNT(*) AS CantidadVentas,
                             CASE WHEN COUNT(*) > 0 THEN SUM(CAST(v.TotalFinal AS REAL)) / COUNT(*) ELSE 0 END AS PromedioVenta
                      FROM Venta v
                      WHERE v.Id_sucursal = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);
            var r = rows.FirstOrDefault();
            var totalVentas = r?.TotalVentas ?? 0m;
            var cantidadVentas = r?.CantidadVentas ?? 0;
            var promedio = cantidadVentas > 0 ? totalVentas / cantidadVentas : 0m;
            return (totalVentas, cantidadVentas, promedio);
        }

        public async Task<int> ObtenerClientesUnicosAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<ClientesUnicosAgrupado>(
                    @"SELECT COUNT(DISTINCT v.Id_cliente) AS Clientes
                      FROM Venta v
                      WHERE v.Id_sucursal = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);
            return rows.FirstOrDefault()?.Clientes ?? 0;
        }

        public async Task<string?> ObtenerTopVendedorAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<TopVendedorAgrupado>(
                    @"SELECT u.Nombre || ' ' || u.Apellido AS Nombre
                      FROM Venta v
                      INNER JOIN Usuario u ON v.Id_usuario = u.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY v.Id_usuario
                      ORDER BY SUM(v.TotalFinal) DESC
                      LIMIT 1",
                    idEmpresa, desde, hasta)
                .ToListAsync(ct);
            return rows.FirstOrDefault()?.Nombre;
        }

        public async Task<string?> ObtenerTopProductoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<TopProductoNombreAgrupado>(
                    @"SELECT p.Nombre
                      FROM VentaDetalle vd
                      INNER JOIN Venta v ON vd.Id_venta = v.Id
                      INNER JOIN Producto p ON vd.Id_producto = p.Id
                      INNER JOIN Sucursal s ON v.Id_sucursal = s.Id
                      WHERE s.Id_empresa = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado != 3
                      GROUP BY vd.Id_producto
                      ORDER BY SUM(vd.Subtotal) DESC
                      LIMIT 1",
                    idEmpresa, desde, hasta)
                .ToListAsync(ct);
            return rows.FirstOrDefault()?.Nombre;
        }

        public async Task<IEnumerable<(int Id, decimal TotalFinal, int Estado, int? IdCaja, decimal? EfectivoRecibido)>>
            ObtenerVentasLigerasPorCajaAsync(int idCaja, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var resultado = await _dbSet.AsNoTracking()
                .Where(v => v.Id_caja == idCaja
                         && v.Fecha >= desde
                         && v.Fecha <= hasta
                         && v.Estado == 2)
                .Select(v => new
                {
                    v.Id,
                    v.TotalFinal,
                    v.Estado,
                    v.Id_caja,
                    v.EfectivoRecibido
                })
                .ToListAsync(ct);

            return resultado.Select(v => (v.Id, v.TotalFinal, v.Estado, v.Id_caja, v.EfectivoRecibido));
        }

        // Uses SqlQueryRaw with CAST(p.Monto AS REAL) to avoid SQLite's inability to handle
        // decimal SUM / ORDER BY on large groups. Mirrors PagoRepositorio.ObtenerTotalesPorMetodoAsync.
        public async Task<IEnumerable<(string Metodo, decimal Total, int Cantidad)>>
            ObtenerPagosPorCajaAsync(int idCaja, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<PagoCajaAgrupado>(
                    @"SELECT mp.Nombre AS Metodo,
                             SUM(CAST(p.Monto AS REAL)) AS Total,
                             COUNT(*) AS Cantidad
                      FROM Pago p
                      INNER JOIN Venta v ON p.Id_venta = v.Id
                      INNER JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id
                      WHERE v.Id_caja = {0}
                        AND v.Estado = 2
                      GROUP BY mp.Nombre
                      ORDER BY Total DESC",
                    idCaja)
                .ToListAsync(ct);

            return rows.Select(r => (r.Metodo, r.Total, r.Cantidad));
        }
    }

    // ── Tipos para SqlQueryRaw (EF Core 8) ─────────────────────────
    public record TopProductoAgrupado(
        int IdProducto, string Nombre, string Categoria,
        int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha);

    public record RotacionProductoAgrupado(
        int IdProducto, string Nombre, string Categoria,
        decimal StockActual, int CantidadVendida, DateTime? UltimaVenta);

    public record VentaPorDiaAgrupado(string Dia, decimal Total, int Cantidad);

    public record VentaPorSucursalAgrupado(int IdSucursal, string SucursalNombre, decimal Total, int Cantidad);

    public record VentaPorVendedorAgrupado(
        int IdUsuario, string UsuarioNombre, string SucursalNombre,
        int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos);

    public record KpiVentasAgrupado(decimal TotalVentas, int TotalTransacciones, decimal TicketPromedio);

    public record TopVendedorAgrupado(string Nombre);

    public record TopProductoNombreAgrupado(string Nombre);

    public record PagoCajaAgrupado(string Metodo, decimal Total, int Cantidad);

    public record ClientesUnicosAgrupado(int Clientes);

    public record ResumenVentasSucursalRaw(decimal TotalVentas, int CantidadVentas, decimal PromedioVenta);
}