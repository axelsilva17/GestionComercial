using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.DTOs.Ventas;
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
            LogHelper.Log("[DEBUG-AbrirCaja] Iniciando...");
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

            LogHelper.Log("[DEBUG-AbrirCaja] Paso 1: Agregando caja...");
            await _uow.Cajas.AgregarAsync(caja);
            await _uow.GuardarCambiosAsync();
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
                    idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : idSucursal
                );
                await _uow.GuardarCambiosAsync();
                LogHelper.Log("[DEBUG-AbrirCaja] Auditoría de caja guardada OK");
            }
            catch (DbUpdateException ex)
            {
                LogHelper.LogError("[ERROR-AbrirCaja] Fallo en auditoría de caja", ex);
                // Continuar aunque falle la auditoría
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
                    idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : idSucursal
                );
                await _uow.GuardarCambiosAsync();
                LogHelper.Log("[DEBUG-AbrirCaja] Auditoría de movimiento guardada OK");
            }
            catch (DbUpdateException ex)
            {
                LogHelper.LogError("[ERROR-AbrirCaja] Fallo en auditoría de movimiento", ex);
                // Continuar aunque falle la auditoría
            }

            try
            {
                LogHelper.Log("[DEBUG-AbrirCaja] Paso 4: Agregando movimiento de apertura...");
                await _uow.MovimientosCaja.AgregarAsync(movimientoApertura);
                await _uow.GuardarCambiosAsync();
                LogHelper.Log("[DEBUG-AbrirCaja] TODO EXITOSO!");
            }
            catch (DbUpdateException ex)
            {
                LogHelper.LogError("[ERROR-AbrirCaja] Fallo al guardar movimiento", ex);
                throw;
            }

            return caja;
        }

        public async Task<Caja> CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal)
        {
            LogHelper.Log("[DEBUG-CerrarCaja] Iniciando...");
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

            LogHelper.Log("[DEBUG-CerrarCaja] Paso 1: Actualizando caja...");
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
                    idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal
                );
                await _uow.GuardarCambiosAsync();
                LogHelper.Log("[DEBUG-CerrarCaja] Auditoría de caja guardada OK");
            }
            catch (DbUpdateException ex)
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
                    idSucursal: _sesion.IdSucursal != 0 ? _sesion.IdSucursal : caja.Id_sucursal
                );
                await _uow.GuardarCambiosAsync();
                LogHelper.Log("[DEBUG-CerrarCaja] Auditoría de movimiento guardada OK");
            }
            catch (DbUpdateException ex)
            {
                LogHelper.LogError("[ERROR-CerrarCaja] Fallo en auditoría de movimiento", ex);
            }

            try
            {
                LogHelper.Log("[DEBUG-CerrarCaja] Paso 4: Agregando movimiento de cierre...");
                await _uow.MovimientosCaja.AgregarAsync(movimientoCierre);
                await _uow.GuardarCambiosAsync();
                LogHelper.Log("[DEBUG-CerrarCaja] TODO EXITOSO!");
            }
            catch (DbUpdateException ex)
            {
                LogHelper.LogError("[ERROR-CerrarCaja] Fallo al guardar movimiento", ex);
                throw;
            }

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

            // ── Cantidad de transacciones ─────────────────────────────────────
            // Contar desde el desglose de pagos (cada item es una transacción)
            resumen.CantidadVentas = resumen.DesglosePorMetodo.Sum(d => d.Cantidad > 0 ? d.Cantidad : 1);

            // ── Movimientos manuales de caja (ingresos/egresos) ───────────────
            // IMPORTANTE: Solo incluir movimientos MANUALES, NO los de ventas
            // Los movimientos de ventas ya están incluidos en VentasEfectivo (desde los pagos)
            // El vuelto de ventas ya está incluido en EgresosEfectivo (desde los pagos)
            var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja);
            foreach (var mov in movimientos)
            {
                // Solo incluir Ingreso y Egreso MANUAL (los de ventas tienen Id_venta)
                // Apertura/Cierre son operativos y no afectan el saldo físico
                if (mov.Tipo == (int)TipoMovimientoCajaEnum.Ingreso && mov.Id_venta == null)
                    resumen.IngresosEfectivo += mov.Monto;
                else if (mov.Tipo == (int)TipoMovimientoCajaEnum.Egreso && mov.Id_venta == null)
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

        public async Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta)
            => await _uow.Cajas.ObtenerHistorialAsync(idSucursal, desde, hasta);

        /// <summary>
        /// Registra la auditoría del cierre de caja (diferencia, modo, etc.)
        /// </summary>
        public async Task RegistrarAuditoriaCierreAsync(int idCaja, int idUsuario, string datosAuditoriaJson, decimal montoFinal, decimal diferencia)
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
                    idEmpresa: _sesion.IdEmpresa != 0 ? _sesion.IdEmpresa : null);
                    
                LogHelper.Log("[CajaServicio] Auditoría de cierre registrada exitosamente");
            }
            catch (Exception ex)
            {
                LogHelper.LogError("[CajaServicio] Error al registrar auditoría de cierre", ex);
                // No lanzar - el cierre principal no debe fallar por auditoría
            }
        }

        /// <summary>
        /// Obtiene el total de efectivo recibido por caja desde las ventas.
        /// Usado para cierre automático de caja.
        /// </summary>
        public async Task<decimal> ObtenerTotalEfectivoPorCajaAsync(int idCaja)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja);
            if (caja == null)
                return 0;

            // Obtener ventas pagadas en efectivo de esta caja
            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(
                caja.FechaApertura,
                DateTime.Now,
                caja.Id_sucursal);

            // Filtrar ventas de esta caja que tienen EfectivoRecibido
            return ventas
                .Where(v => v.Id_caja == idCaja 
                         && v.Estado == 2 // Pagada
                         && v.EfectivoRecibido.HasValue)
                .Sum(v => v.EfectivoRecibido!.Value);
        }

        // ── Métodos para CajaViewModel ───────────────────────────────────────────
        public async Task<IEnumerable<MovimientoCajaDto>> ObtenerMovimientosAsync(int idCaja)
        {
            var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja);
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

        public async Task<IEnumerable<VentaDto>> ObtenerVentasDelDiaAsync(int idCaja)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja);
            if (caja == null)
                return Enumerable.Empty<VentaDto>();

            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(
                caja.FechaApertura,
                DateTime.Now,
                caja.Id_sucursal);

            return ventas
                .Where(v => v.Id_caja == idCaja && v.Estado == 2) // Solo ventas pagadas
                .Select(v => new VentaDto
                {
                    IdVenta = v.Id,
                    Fecha = v.Fecha,
                    Total = v.TotalFinal,
                    TotalFinal = v.TotalFinal,
                    TotalBruto = v.TotalBruto,
                    TotalDescuento = v.TotalDescuento,
                    Estado = v.Estado.ToString(),
                    IdSucursal = v.Id_sucursal,
                    IdCaja = v.Id_caja ?? 0,
                    EfectivoRecibido = v.EfectivoRecibido ?? 0
                });
        }

        public async Task<IEnumerable<DesglosePagoDto>> ObtenerDesglosePorMetodoAsync(int idCaja)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja);
            if (caja == null)
                return Enumerable.Empty<DesglosePagoDto>();

            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(
                caja.FechaApertura,
                DateTime.Now,
                caja.Id_sucursal);

            var ventasCaja = ventas.Where(v => v.Id_caja == idCaja && v.Estado == 2).ToList();

            // Agrupar por método de pago
            var desglose = ventasCaja
                .SelectMany(v => v.Pagos)
                .GroupBy(p => p.MetodoPago.Nombre)
                .Select(g => new DesglosePagoDto
                {
                    Metodo = g.Key,
                    Total = g.Sum(p => p.Monto),
                    Cantidad = g.Count()
                })
                .ToList();

            return desglose;
        }
    }
}
