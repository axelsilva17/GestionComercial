using System.Text.Json;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Aplicacion.Servicios;
// Aliases: evitan ambigüedad con CajaHistorialDto (Aplicacion.Interfaces.Servicios) dentro de este archivo.
using CajaExportRow = GestionComercial.Dominio.Interfaces.Repositorios.CajaExportRow;
using VentaExportRow = GestionComercial.Dominio.Interfaces.Repositorios.VentaExportRow;
using CajaAuditoriaRow = GestionComercial.Dominio.Interfaces.Repositorios.CajaAuditoriaRow;
using System.Threading;

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

        public async Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerCajaAbiertaAsync(idSucursal, ct);

        public async Task<Caja> AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial, TurnoCajaEnum? turno = null, bool esPrimaria = false, CancellationToken ct = default)
        {
            if (!_sesion.HasPermission("Caja.Abrir"))
                throw new NegocioException("No tenés permiso para abrir caja.");

            LogHelper.Log("[DEBUG-AbrirCaja] Iniciando...");

            // Validar que no exista caja abierta para el mismo turno en esta sucursal
            if (turno.HasValue)
            {
                var turnoStr = turno.Value.ToDisplayString();
                var cajaEnTurno = await _uow.Cajas.ObtenerCajaAbiertaPorSucursYTurnoAsync(idSucursal, turnoStr, ct);
                if (cajaEnTurno != null)
                    throw new NegocioException($"Ya existe una caja abierta para el turno {turnoStr} en esta sucursal");
            }
            else
            {
                var cajaExistente = await _uow.Cajas.ObtenerCajaAbiertaAsync(idSucursal, ct);
                if (cajaExistente != null)
                    throw new NegocioException("Ya existe una caja abierta para esta sucursal");
            }

            // ── Crear caja usando factory method DDD ───────────────────────────────
            var caja = Caja.Crear(idSucursal, idUsuario, montoInicial, esPrimaria, turno);

            // Serializar estado nuevo para auditoría
            var valoresNuevos = JsonSerializer.Serialize(new
            {
                caja.FechaApertura,
                caja.MontoInicial,
                caja.MontoFinal,
                caja.Estado,
                caja.Id_sucursal,
                caja.UsuarioApertura_id,
                caja.Turno,
                caja.EsPrimaria
            });

            // ── Todo en una transacción: caja + auditoría + movimiento ──────────────
            await _uow.EjecutarEnTransaccionAsync(async () =>
            {
                LogHelper.Log("[DEBUG-AbrirCaja] Paso 1: Agregando caja...");
                await _uow.Cajas.AgregarAsync(caja, ct);
                LogHelper.Log($"[DEBUG-AbrirCaja] Caja guardada con ID: {caja.Id}");

                // Registrar auditoría de apertura de caja
                try
                {
                    LogHelper.Log("[DEBUG-AbrirCaja] Paso 2: Registrando auditoría de caja...");
                    await _uow.Auditoria.RegistrarAuditoriaAsync(
                        nombreTabla: "Cajas",
                        registroId: caja.Id,
                        tipoOperacion: OperacionAuditoriaEnum.Insert,
                        idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : idUsuario,
                        nombreUsuario: _sesion.Nombre ?? "Sistema",
                        valoresAnteriores: null,
                        valoresNuevos: valoresNuevos,
                        workstation: Environment.MachineName,
                        idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                        idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : idSucursal, ct);
                    LogHelper.Log("[DEBUG-AbrirCaja] Auditoría de caja guardada OK");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("[ERROR-AbrirCaja] Fallo en auditoría de caja", ex);
                }

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

                try
                {
                    LogHelper.Log("[DEBUG-AbrirCaja] Paso 3: Registrando auditoría de movimiento...");
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
                        idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : idSucursal, ct);
                    LogHelper.Log("[DEBUG-AbrirCaja] Auditoría de movimiento guardada OK");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("[ERROR-AbrirCaja] Fallo en auditoría de movimiento", ex);
                }

                LogHelper.Log("[DEBUG-AbrirCaja] Paso 4: Agregando movimiento de apertura...");
                // Asignar navegación para que EF Core fix-up el FK Id_caja correctamente
                movimientoApertura.Caja = caja;
                await _uow.MovimientosCaja.AgregarAsync(movimientoApertura, ct);
                LogHelper.Log("[DEBUG-AbrirCaja] TODO EXITOSO!");
            }, ct);

            return caja;
        }

        public async Task<Caja> CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal, CancellationToken ct = default)
        {
            if (!_sesion.HasPermission("Caja.Cerrar"))
            {
                System.Diagnostics.Debug.WriteLine($"[CajaServicio] Sin permiso Caja.Cerrar. Permisos en sesión: [{string.Join(", ", _sesion.ObtenerSesion().Permisos ?? new())}]");
                throw new NegocioException("No tenés permiso para cerrar caja.");
            }

            LogHelper.Log("[DEBUG-CerrarCaja] Iniciando...");
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct)
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

            LogHelper.Log("[DEBUG-CerrarCaja] Paso 1: Cerrando caja con método de dominio...");
            caja.Cerrar(idUsuario, montoFinal);

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

            // ── Todo en una transacción: caja + auditoría + movimiento ──────────────
            await _uow.EjecutarEnTransaccionAsync(async () =>
            {
                _uow.Cajas.Actualizar(caja);

                try
                {
                    LogHelper.Log("[DEBUG-CerrarCaja] Paso 2: Registrando auditoría de caja...");
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
                        idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal, ct);
                    LogHelper.Log("[DEBUG-CerrarCaja] Auditoría de caja guardada OK");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("[ERROR-CerrarCaja] Fallo en auditoría de caja", ex);
                }

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

                try
                {
                    LogHelper.Log("[DEBUG-CerrarCaja] Paso 3: Registrando auditoría de movimiento...");
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
                        idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal, ct);
                    LogHelper.Log("[DEBUG-CerrarCaja] Auditoría de movimiento guardada OK");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("[ERROR-CerrarCaja] Fallo en auditoría de movimiento", ex);
                }

                LogHelper.Log("[DEBUG-CerrarCaja] Paso 4: Agregando movimiento de cierre...");
                await _uow.MovimientosCaja.AgregarAsync(movimientoCierre, ct);
                LogHelper.Log("[DEBUG-CerrarCaja] TODO EXITOSO!");
            }, ct);

            return caja;
        }

        public async Task RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo,
                                                   decimal monto, string descripcion, CancellationToken ct = default)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct)
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
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal, ct);

            await _uow.MovimientosCaja.AgregarAsync(movimiento, ct);

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
                idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal, ct);

            await _uow.GuardarCambiosAsync(ct);
        }

        // ── Nuevo: resumen automático para el cierre ──────────────────────────
        public async Task<ResumenCierreDto> ObtenerResumenCierreAsync(int idCaja, CancellationToken ct = default)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct)
                ?? throw new CajaNoAbiertaException();

            var resumen = new ResumenCierreDto
            {
                MontoInicial  = caja.MontoInicial,
                FechaApertura = caja.FechaApertura,
            };

            // ── Ventas del turno agrupadas por método de pago ─────────────────
            // Obtenemos todos los pagos de ventas de esta caja en el turno
            var pagosDelTurno = await _uow.Pagos.ObtenerTotalesPorMetodoAsync(
                caja.Id_sucursal,
                caja.FechaApertura,
                DateTime.Now,
                idCaja, // Filtro por caja específica
                ct);

            // Necesitamos saber cuáles métodos son efectivo.
            // Usamos el repositorio de MetodosPago para obtener los detalles.
            var metodosPago = await _uow.MetodosPago.ObtenerTodosPorEmpresaAsync(
                await ObtenerIdEmpresaDeSucursalAsync(caja.Id_sucursal, ct), ct);

            var metodosDict = metodosPago.ToDictionary(m => m.Nombre, m => m.Categoria);

            foreach (var (metodo, total, cantidad) in pagosDelTurno)
            {
                var categoria = metodosDict.TryGetValue(metodo, out var cat) ? cat : "Otro";

                resumen.DesglosePorMetodo.Add(new DesglosePagoDto
                {
                    Metodo     = metodo,
                    Total      = total,
                    Cantidad   = cantidad,
                    Categoria  = categoria,
                });

                if (categoria == "Efectivo")
                    resumen.VentasEfectivo += total;
                else
                {
                    // Clasificar por categoría
                    switch (categoria)
                    {
                        case "Tarjeta":
                            resumen.VentasTarjeta += total;
                            break;
                        case "Transferencia":
                            resumen.VentasTransferencia += total;
                            break;
                        default:
                            resumen.VentasOtros += total;
                            break;
                    }
                }
            }

            // ── Cantidad de transacciones ─────────────────────────────────────
            // Contar desde el desglose de pagos (cada item es una transacción)
            resumen.CantidadVentas = resumen.DesglosePorMetodo.Sum(d => d.Cantidad > 0 ? d.Cantidad : 1);

            // ── Movimientos manuales de caja (ingresos/egresos) ───────────────
            // Los movimientos de Ingreso por venta ya están en VentasEfectivo (desde los pagos)
            // Los movimientos de Egreso INCLUYEN el vuelto (tiene Id_venta) y egresos manuales
            // Apertura/Cierre son operativos y no afectan el saldo físico
            var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja, ct);
            foreach (var mov in movimientos)
            {
                if (mov.Tipo == (int)TipoMovimientoCajaEnum.Ingreso && mov.Id_venta == null)
                    resumen.IngresosEfectivo += mov.Monto;
                else if (mov.Tipo == (int)TipoMovimientoCajaEnum.Egreso
                      && mov.Tipo != (int)TipoMovimientoCajaEnum.Cierre)
                    resumen.EgresosEfectivo += mov.Monto;
            }

            return resumen;
        }

        ///         /// Diferencia entre el conteo físico y el saldo esperado, calculada
        /// a partir del resumen centralizado. Devuelve 0 si la caja sigue abierta.
        public async Task<decimal> ObtenerDiferenciaCierreAsync(int idCaja, CancellationToken ct = default)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct)
                ?? throw new CajaNoAbiertaException();

            // Caja abierta: no hay conteo físico todavía, por lo que no existe diferencia.
            if (!caja.MontoFinal.HasValue)
                return 0m;

            var resumen = await ObtenerResumenCierreAsync(idCaja, ct);
            return caja.MontoFinal.Value - resumen.SaldoEsperado;
        }

        // Helper: obtener IdEmpresa desde IdSucursal
        private async Task<int> ObtenerIdEmpresaDeSucursalAsync(int idSucursal, CancellationToken ct = default)
        {
            var sucursal = await _uow.Sucursales.ObtenerPorIdAsync(idSucursal, ct);
            return sucursal?.Id_empresa ?? 0;
        }

        // ── Auditoría en lote: historial ligero + resúmenes por caja (sin N+1) ─────────
        public async Task<List<CajaAuditoriaRow>> ObtenerHistorialAuditoriaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerHistorialAuditoriaAsync(idSucursal, desde, hasta, ct);

        public async Task<List<CajaAuditoriaResumenDto>> ObtenerAuditoriaResumenesAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            // Mismas filas que el historial de auditoría (mismo filtro y fuente de verdad).
            var cajas = await _uow.Cajas.ObtenerHistorialAuditoriaAsync(idSucursal, desde, hasta, ct);

            // Agregación SQL en lote: una consulta por caja para pagos y movimientos.
            var pagos = await _uow.Pagos.ObtenerTotalesPorMetodoPorCajaAsync(idSucursal, desde, hasta, ct);
            var movimientos = await _uow.MovimientosCaja.ObtenerResumenPorCajaEnPeriodoAsync(idSucursal, desde, hasta, ct);

            // Categorías de métodos de pago una sola vez (misma lógica que ObtenerResumenCierreAsync).
            var metodosPago = await _uow.MetodosPago.ObtenerTodosPorEmpresaAsync(
                await ObtenerIdEmpresaDeSucursalAsync(idSucursal, ct), ct);
            var metodosDict = metodosPago.ToDictionary(m => m.Nombre, m => m.Categoria);

            var ventasEfectivoPorCaja = pagos
                .Where(p => metodosDict.TryGetValue(p.Metodo, out var categoria) && categoria == "Efectivo")
                .GroupBy(p => p.IdCaja)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Total));

            var movimientosPorCaja = movimientos.ToDictionary(m => m.IdCaja);

            return cajas.Select(c =>
            {
                var ventasEfectivo = ventasEfectivoPorCaja.TryGetValue(c.Id, out var efectivo) ? efectivo : 0m;
                var ingresos = movimientosPorCaja.TryGetValue(c.Id, out var movIng) ? movIng.Ingresos : 0m;
                var egresos = movimientosPorCaja.TryGetValue(c.Id, out var movEgr) ? movEgr.Egresos : 0m;
                var saldoEsperado = c.MontoInicial + ventasEfectivo + ingresos - egresos;
                var diferencia = c.MontoFinal.HasValue ? c.MontoFinal.Value - saldoEsperado : 0m;
                return new CajaAuditoriaResumenDto(c.Id, ventasEfectivo, ingresos, egresos, saldoEsperado, diferencia);
            }).ToList();
        }

        public async Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerHistorialAsync(idSucursal, desde, hasta, ct);

        public async Task<IEnumerable<Caja>> ObtenerUltimasCajasConVentasAsync(
            int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerUltimasCajasConVentasAsync(idSucursal, desde, hasta, take, ct);

        public async Task<(decimal Ingresos, decimal Egresos)> ObtenerResumenMoviCajaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _uow.MovimientosCaja.ObtenerResumenPorSucursalAsync(idSucursal, desde, hasta, ct);

        public async Task<(int Total, int Cerradas)> ObtenerConteoCajasPeriodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerConteoCajasPeriodoAsync(idSucursal, desde, hasta, ct);

        // Nuevo: historial con proyección ligera y Take en SQL
        public async Task<List<CajaHistorialDto>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default)
        {
            var cajas = await _uow.Cajas.ObtenerHistorialAsync(idSucursal, desde, hasta, take, ct);
            return cajas.Select(c => new CajaHistorialDto
            {
                Id = c.Id,
                FechaApertura = c.FechaApertura,
                FechaCierre = c.FechaCierre,
                Estado = c.Estado,
                SaldoInicial = c.SaldoInicial,
                SaldoFinal = c.SaldoFinal,
                SucursalNombre = c.SucursalNombre,
                UsuarioApertura = c.UsuarioApertura
            }).ToList();
        }

        // ── Nuevo: proyecciones ligeras para exportación Excel ──────────────────
        public async Task<List<CajaExportRow>> ObtenerHistorialExportAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerHistorialExportAsync(idSucursal, desde, hasta, ct);

        public async Task<List<VentaExportRow>> ObtenerVentasExportPorCajaAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _uow.Cajas.ObtenerVentasExportPorCajaAsync(idSucursal, desde, hasta, ct);

        ///         /// Registra la auditoría del cierre de caja (diferencia, modo, etc.)
        public async Task RegistrarAuditoriaCierreAsync(int idCaja, int idUsuario, string datosAuditoriaJson, decimal montoFinal, decimal diferencia, CancellationToken ct = default)
        {
            try
            {
                LogHelper.Log($"[CajaServicio] Registrando auditoría de cierre: idCaja={idCaja}, diferencia={diferencia}");
                
                // Registrar en auditoría del sistema (usar Update ya que no existe Close en el enum)
                await _uow.Auditoria.RegistrarAuditoriaAsync(
                    nombreTabla: "Cajas_Cierre",
                    registroId: idCaja,
                    tipoOperacion: OperacionAuditoriaEnum.Update,
                    idUsuario: idUsuario,
                    nombreUsuario: _sesion.Nombre ?? "Sistema",
                    valoresAnteriores: null,
                    valoresNuevos: datosAuditoriaJson,
                    workstation: Environment.MachineName,
                    idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                    idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : null, ct);
                    
                LogHelper.Log("[CajaServicio] Auditoría de cierre registrada exitosamente");
            }
            catch (Exception ex)
            {
                LogHelper.LogError("[CajaServicio] Error al registrar auditoría de cierre", ex);
                // No lanzar - el cierre principal no debe fallar por auditoría
            }
        }

        ///         /// Obtiene el total de efectivo recibido por caja desde las ventas.
        /// Usado para cierre automático de caja.
        public async Task<decimal> ObtenerTotalEfectivoPorCajaAsync(int idCaja, CancellationToken ct = default)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct);
            if (caja == null)
                return 0;

            var ventas = await _uow.Ventas.ObtenerVentasLigerasPorCajaAsync(
                idCaja,
                caja.FechaApertura,
                DateTime.Now, ct);

            return ventas
                .Where(v => v.Estado == 2)
                .Sum(v => v.EfectivoRecibido ?? 0);
        }

        // ── Métodos para CajaViewModel ───────────────────────────────────────────
        public async Task<IEnumerable<MovimientoCajaDto>> ObtenerMovimientosAsync(int idCaja, CancellationToken ct = default)
        {
            var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja, ct);
            return movimientos.Select(m => new MovimientoCajaDto
            {
                IdMovimiento = m.Id,
                Tipo = m.Tipo switch
                {
                    (int)TipoMovimientoCajaEnum.Apertura => "Apertura",
                    (int)TipoMovimientoCajaEnum.Cierre => "Cierre",
                    (int)TipoMovimientoCajaEnum.Ingreso => "Ingreso",
                    (int)TipoMovimientoCajaEnum.Egreso => "Egreso",
                    _ => "Otro"
                },
                Monto = m.Monto,
                Fecha = m.Fecha,
                Descripcion = m.Concepto ?? string.Empty,
                ReferenciaId = m.Id_venta
            });
        }

        public async Task<IEnumerable<VentaDto>> ObtenerVentasDelDiaAsync(int idCaja, CancellationToken ct = default)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct);
            if (caja == null)
                return Enumerable.Empty<VentaDto>();

            var ventas = await _uow.Ventas.ObtenerVentasLigerasPorCajaAsync(
                idCaja,
                caja.FechaApertura,
                DateTime.Now, ct);

            return ventas
                .Where(v => v.Estado == 2)
                .Select(v => new VentaDto
                {
                    IdVenta = v.Id,
                    Total = v.TotalFinal,
                    TotalFinal = v.TotalFinal,
                    EfectivoRecibido = v.EfectivoRecibido ?? 0
                });
        }

        public async Task<IEnumerable<DesglosePagoDto>> ObtenerDesglosePorMetodoAsync(int idCaja, CancellationToken ct = default)
        {
            var pagos = await _uow.Ventas.ObtenerPagosPorCajaAsync(idCaja, ct);

            return pagos.Select(p => new DesglosePagoDto
            {
                Metodo = p.Metodo,
                Total = p.Total,
                Cantidad = p.Cantidad
            }).ToList();
        }

        public async Task EliminarCajaAsync(int idCaja, CancellationToken ct = default)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja, ct)
                ?? throw new NegocioException("Caja no encontrada.");

            if (caja.EsPrimaria)
                throw new NegocioException("No se puede eliminar una caja primaria.");

            if (caja.EstaAbierta)
                throw new NegocioException("No se puede eliminar una caja abierta. Cerrala primero.");

            var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja, ct);
            if (movimientos.Any(m => m.Tipo == (int)TipoMovimientoCajaEnum.Ingreso
                                  || m.Tipo == (int)TipoMovimientoCajaEnum.Egreso))
                throw new NegocioException("La caja tiene movimientos de ingreso/egreso y no puede ser eliminada.");

            // Capturar estado anterior para auditoría
            var valoresAnteriores = JsonSerializer.Serialize(new
            {
                caja.Id,
                caja.FechaApertura,
                caja.MontoInicial,
                caja.Estado,
                caja.Turno,
                caja.EsPrimaria
            });

            caja.Inactivar();

            // ── Todo en una transacción: inactivar + auditoría ──────────────────────
            await _uow.EjecutarEnTransaccionAsync(async () =>
            {
                _uow.Cajas.Actualizar(caja);

                try
                {
                    var valoresNuevos = JsonSerializer.Serialize(new
                    {
                        caja.Id,
                        caja.Activo,
                        caja.Estado
                    });

                    await _uow.Auditoria.RegistrarAuditoriaAsync(
                        nombreTabla: "Cajas",
                        registroId: caja.Id,
                        tipoOperacion: OperacionAuditoriaEnum.Delete,
                        idUsuario: _sesion.IdUsuario != 0 ? _sesion.IdUsuario : null,
                        nombreUsuario: _sesion.Nombre ?? "Sistema",
                        valoresAnteriores: valoresAnteriores,
                        valoresNuevos: valoresNuevos,
                        workstation: Environment.MachineName,
                        idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null,
                        idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal, ct);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("[CajaServicio] Error al registrar auditoría de eliminación", ex);
                }
            }, ct);
        }
    }
}