using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class CajaServicio : ICajaServicio
    {
        private readonly IUnitOfWork _uow;
        public CajaServicio(IUnitOfWork uow) => _uow = uow;

        // ── Existentes sin cambios ────────────────────────────────────────────
        public async Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal)
            => await _uow.Cajas.ObtenerCajaAbiertaAsync(idSucursal);

        public async Task<Caja> AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial)
        {
            var cajaExistente = await _uow.Cajas.ObtenerCajaAbiertaAsync(idSucursal);
            if (cajaExistente != null)
                throw new NegocioException("Ya existe una caja abierta para esta sucursal");

            var caja = new Caja
            {
                FechaApertura      = DateTime.Now,
                MontoInicial       = montoInicial,
                MontoFinal         = montoInicial,
                Estado             = 1, // 1 = Abierta
                Id_sucursal        = idSucursal,
                UsuarioApertura_id = idUsuario,
            };
            await _uow.Cajas.AgregarAsync(caja);

            // Registrar movimiento de apertura
            var movimientoApertura = new TipoMovimientoCaja
            {
                Id_caja    = caja.Id,
                Tipo       = (int)TipoMovimientoCajaEnum.Apertura,
                Monto      = montoInicial,
                Fecha      = DateTime.Now,
                Concepto   = "Apertura de caja",
                Id_usuario = idUsuario,
            };
            await _uow.MovimientosCaja.AgregarAsync(movimientoApertura);

            await _uow.GuardarCambiosAsync();
            return caja;
        }

        public async Task<Caja> CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja)
                ?? throw new CajaNoAbiertaException();
            if (!caja.EstaAbierta)
                throw new CajaNoAbiertaException();

            caja.FechaCierre      = DateTime.Now;
            caja.MontoFinal       = montoFinal;
            caja.Estado           = 2; // 2 = Cerrada
            caja.UsuarioCierre_id = idUsuario;
            _uow.Cajas.Actualizar(caja);

            // Registrar movimiento de cierre
            var movimientoCierre = new TipoMovimientoCaja
            {
                Id_caja    = caja.Id,
                Tipo       = (int)TipoMovimientoCajaEnum.Cierre,
                Monto      = montoFinal,
                Fecha      = DateTime.Now,
                Concepto   = "Cierre de caja",
                Id_usuario = idUsuario,
            };
            await _uow.MovimientosCaja.AgregarAsync(movimientoCierre);

            await _uow.GuardarCambiosAsync();
            return caja;
        }

        public async Task RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo,
                                                   decimal monto, string descripcion)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja)
                ?? throw new CajaNoAbiertaException();
            if (!caja.EstaAbierta)
                throw new CajaNoAbiertaException();

            var movimiento = new TipoMovimientoCaja
            {
                Id_caja  = idCaja,
                Tipo     = (int)tipo,
                Monto    = monto,
                Fecha    = DateTime.Now,
                Concepto = descripcion,
            };
            await _uow.MovimientosCaja.AgregarAsync(movimiento);

            caja.MontoFinal += tipo == TipoMovimientoCajaEnum.Ingreso ? monto : -monto;
            _uow.Cajas.Actualizar(caja);
            await _uow.GuardarCambiosAsync();
        }

        // ── Nuevo: resumen automático para el cierre ──────────────────────────
        public async Task<ResumenCierreDto> ObtenerResumenCierreAsync(int idCaja)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja)
                ?? throw new CajaNoAbiertaException();

            var resumen = new ResumenCierreDto
            {
                MontoInicial  = caja.MontoInicial,
                FechaApertura = caja.FechaApertura,
            };

            // ── Ventas del turno agrupadas por método de pago ─────────────────
            // Obtenemos todos los pagos de ventas de esta caja/sucursal en el turno
            var pagosDelTurno = await _uow.Pagos.ObtenerTotalesPorMetodoAsync(
                caja.Id_sucursal,
                caja.FechaApertura,
                DateTime.Now);

            // Necesitamos saber cuáles métodos son efectivo.
            // Usamos el repositorio de MetodosPago para obtener los detalles.
            var metodosPago = await _uow.MetodosPago.ObtenerTodosPorEmpresaAsync(
                await ObtenerIdEmpresaDeSucursalAsync(caja.Id_sucursal));

            var metodosDict = metodosPago.ToDictionary(m => m.Nombre, m => m.EsEfectivo);

            foreach (var (metodo, total) in pagosDelTurno)
            {
                var esEfectivo = metodosDict.TryGetValue(metodo, out var ef) && ef == true;
                var cantidad   = 0; // se puede extender si el repo devuelve cantidad

                resumen.DesglosePorMetodo.Add(new DesglosePagoDto
                {
                    Metodo     = metodo,
                    Total      = total,
                    EsEfectivo = esEfectivo,
                });

                if (esEfectivo)
                    resumen.VentasEfectivo += total;
                else
                {
                    // Clasificar por nombre para mostrar en UI
                    var nombreUpper = metodo.ToUpper();
                    if (nombreUpper.Contains("TARJETA") || nombreUpper.Contains("DEBITO") || nombreUpper.Contains("CRÉDITO"))
                        resumen.VentasTarjeta += total;
                    else if (nombreUpper.Contains("TRANSFER"))
                        resumen.VentasTransferencia += total;
                    else if (nombreUpper.Contains("QR") || nombreUpper.Contains("MERCADO") || nombreUpper.Contains("MP"))
                        resumen.VentasQR += total;
                    else if (nombreUpper.Contains("CUENTA") || nombreUpper.Contains("CTE") || nombreUpper.Contains("CORRIENTE"))
                        resumen.VentasCuentaCte += total;
                    else
                        resumen.VentasOtros += total;
                }
            }

            // ── Movimientos manuales de caja (ingresos/egresos) ───────────────
            var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja);
            foreach (var mov in movimientos)
            {
                // Solo incluir Ingreso y Egreso en el resumen (Apertura/Cierre son operativos)
                if (mov.Tipo == (int)TipoMovimientoCajaEnum.Ingreso)
                    resumen.IngresosEfectivo += mov.Monto;
                else if (mov.Tipo == (int)TipoMovimientoCajaEnum.Egreso)
                    resumen.EgresosEfectivo += mov.Monto;
            }

            return resumen;
        }

        // Helper: obtener IdEmpresa desde IdSucursal
        private async Task<int> ObtenerIdEmpresaDeSucursalAsync(int idSucursal)
        {
            var sucursal = await _uow.Sucursales.ObtenerPorIdAsync(idSucursal);
            return sucursal?.Id_empresa ?? 0;
        }
    }
}
