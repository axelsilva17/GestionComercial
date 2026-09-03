using GestionComercial.Dominio.DTOs.Inventario;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{

    public interface IMovimientoStockRepositorio : IRepositorioBase<MovimientoStock>
    {
        Task<IEnumerable<MovimientoStock>> ObtenerPorProductoAsync(int idProducto, CancellationToken ct = default);
        Task<IEnumerable<MovimientoStock>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int? idSucursal = null, CancellationToken ct = default);

        // Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion
        Task<(List<MovimientoStock> Items, int Total)> ObtenerPaginadoAsync(
            string? textoBusqueda,
            string? filtroTipo,
            string? filtroUsuario,
            string? filtroSucursal,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int pagina,
            int itemsPorPagina,
            CancellationToken ct = default);

        Task<ResumenMovimientoStockDto?> ObtenerResumenPeriodoAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int? idEmpresa,
            int? idSucursal = null,
            CancellationToken ct = default);
    }


}