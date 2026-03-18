using System.Text.Json;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Aplicacion.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class CajaServicio : ICajaServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly SesionServicio _sesion;

        public CajaServicio(IUnitOfWork uow, SesionServicio sesion)
        {
            _uow = uow;
            _sesion = sesion ?? throw new ArgumentNullException(nameof(sesion));
        }

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

            // Serializar estado nuevo para auditoría
            var valoresNuevos = JsonSerializer.Serialize(new
            {
                caja.FechaApertura,
                caja.MontoInicial,
                caja.MontoFinal,
                caja.Estado,
                caja.Id_sucursal,
                caja.UsuarioApertura_id
            });

            // Registrar auditoría de apertura de caja
            await _uow.Auditoria.RegistrarAuditoriaAsync(
                nombreTabla: "Cajas",
                registroId: 0, // Se actualiza después de guardar
                tipoOperacion: OperacionAuditoriaEnum.Insert,
                idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : idUsuario,
                nombreUsuario: _sesion.Nombre ?? "Sistema",
                valoresAnteriores: null,
                valoresNuevos: valoresNuevos,
                workstation: Environment.MachineName,
                idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : idSucursal
            );

            await _uow.Cajas.AgregarAsync(caja);

            // Actualizar el ID de registro en la auditoría
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

            // Auditoría del movimiento de apertura
            var movimientoValoresNuevos = JsonSerializer.Serialize(new
            {
                movimientoApertura.Id_caja,
                movimientoApertura.Tipo,
                movimientoApertura.Monto,
                movimientoApertura.Fecha,
                movimientoApertura.Concepto,
                movimientoApertura.Id_usuario
            });

            await _uow.Auditoria.RegistrarAuditoriaAsync(
                nombreTabla: "MovimientosCaja",
                registroId: 0,
                tipoOperacion: OperacionAuditoriaEnum.Insert,
                idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : idUsuario,
                nombreUsuario: _sesion.Nombre ?? "Sistema",
                valoresAnteriores: null,
                valoresNuevos: movimientoValoresNuevos,
                workstation: Environment.MachineName,
                idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : idSucursal
            );

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

            // Capturar estado anterior para auditoría
            var valoresAnteriores = JsonSerializer.Serialize(new
            {
                caja.FechaApertura,
                caja.MontoInicial,
                caja.MontoFinal,
                caja.Estado,
                caja.Id_sucursal,
                caja.UsuarioApertura_id
            });

            caja.FechaCierre      = DateTime.Now;
            caja.MontoFinal       = montoFinal;
            caja.Estado           = 2; // 2 = Cerrada
            caja.UsuarioCierre_id = idUsuario;
            _uow.Cajas.Actualizar(caja);

            // Registrar auditoría de cierre de caja
            var valoresNuevos = JsonSerializer.Serialize(new
            {
                caja.FechaApertura,
                caja.FechaCierre,
                caja.MontoInicial,
                caja.MontoFinal,
                caja.Estado,
                caja.Id_sucursal,
                caja.UsuarioApertura_id,
                caja.UsuarioCierre_id
            });

            await _uow.Auditoria.RegistrarAuditoriaAsync(
                nombreTabla: "Cajas",
                registroId: caja.Id,
                tipoOperacion: OperacionAuditoriaEnum.Update,
                idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : idUsuario,
                nombreUsuario: _sesion.Nombre ?? "Sistema",
                valoresAnteriores: valoresAnteriores,
                valoresNuevos: valoresNuevos,
                workstation: Environment.MachineName,
                idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal
            );

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

            // Auditoría del movimiento de cierre
            var movimientoValoresNuevos = JsonSerializer.Serialize(new
            {
                movimientoCierre.Id_caja,
                movimientoCierre.Tipo,
                movimientoCierre.Monto,
                movimientoCierre.Fecha,
                movimientoCierre.Concepto,
                movimientoCierre.Id_usuario
            });

            await _uow.Auditoria.RegistrarAuditoriaAsync(
                nombreTabla: "MovimientosCaja",
                registroId: 0,
                tipoOperacion: OperacionAuditoriaEnum.Insert,
                idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : idUsuario,
                nombreUsuario: _sesion.Nombre ?? "Sistema",
                valoresAnteriores: null,
                valoresNuevos: movimientoValoresNuevos,
                workstation: Environment.MachineName,
                idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal
            );

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

            // Capturar estado anterior de la caja para auditoría
            var valoresAnterioresCaja = JsonSerializer.Serialize(new
            {
                caja.FechaApertura,
                caja.MontoInicial,
                caja.MontoFinal,
                caja.Estado
            });

            var movimiento = new TipoMovimientoCaja
            {
                Id_caja  = idCaja,
                Tipo     = (int)tipo,
                Monto    = monto,
                Fecha    = DateTime.Now,
                Concepto = descripcion,
            };

            // Auditoría del movimiento de caja (ingreso/egreso)
            var movimientoValoresNuevos = JsonSerializer.Serialize(new
            {
                movimiento.Id_caja,
                movimiento.Tipo,
                movimiento.Monto,
                movimiento.Fecha,
                movimiento.Concepto
            });

            await _uow.Auditoria.RegistrarAuditoriaAsync(
                nombreTabla: "MovimientosCaja",
                registroId: 0,
                tipoOperacion: OperacionAuditoriaEnum.Insert,
                idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : null,
                nombreUsuario: _sesion.Nombre ?? "Sistema",
                valoresAnteriores: null,
                valoresNuevos: movimientoValoresNuevos,
                workstation: Environment.MachineName,
                idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal
            );

            await _uow.MovimientosCaja.AgregarAsync(movimiento);

            // Actualizar monto final de la caja
            caja.MontoFinal += tipo == TipoMovimientoCajaEnum.Ingreso ? monto : -monto;
            _uow.Cajas.Actualizar(caja);

            // Auditoría del cambio en la caja (actualización del monto)
            var valoresNuevosCaja = JsonSerializer.Serialize(new
            {
                caja.FechaApertura,
                caja.MontoInicial,
                caja.MontoFinal,
                caja.Estado
            });

            await _uow.Auditoria.RegistrarAuditoriaAsync(
                nombreTabla: "Cajas",
                registroId: caja.Id,
                tipoOperacion: OperacionAuditoriaEnum.Update,
                idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : null,
                nombreUsuario: _sesion.Nombre ?? "Sistema",
                valoresAnteriores: valoresAnterioresCaja,
                valoresNuevos: valoresNuevosCaja,
                workstation: Environment.MachineName,
                idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal
            );

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
