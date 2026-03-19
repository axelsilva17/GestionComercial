using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Servicios
{
    public class AuditoriaAppService : IAuditoriaAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditoriaAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuditoriaCompletaCajaDto> ObtenerAuditoriaCompletaCajaAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken cancellationToken = default)
        {
            // Obtener todos los logs de auditoría del período
            var logs = await _unitOfWork.AuditoriaLog.ObtenerPorRangoFechasAsync(
                fechaDesde, fechaHasta, cancellationToken);

            // Filtrar por tablas relacionadas con caja
            var tablasCaja = new[] { "Cajas", "MovimientosCaja", "AperturaCaja", "CierreCaja" };
            var tablasMovimiento = new[] { "MovimientosCaja", "MovimientoCaja" };

            var auditoriaCajas = logs
                .Where(l => tablasCaja.Contains(l.Tabla, StringComparer.OrdinalIgnoreCase))
                .Where(l => !tablasMovimiento.Contains(l.Tabla, StringComparer.OrdinalIgnoreCase))
                .Select(l => new AuditoriaLogDto
                {
                    Id            = l.Id,
                    Fecha         = l.Fecha,
                    Usuario       = l.Usuario?.Nombre ?? "—",
                    TipoOperacion = ObtenerTipoOperacion(l.TipoOperacion),
                    Tabla         = l.Tabla,
                    RegistroId    = l.RegistroId,
                    Detalles      = l.Detalles,
                })
                .OrderByDescending(l => l.Fecha)
                .ToList();

            var movimientosCaja = logs
                .Where(l => tablasMovimiento.Contains(l.Tabla, StringComparer.OrdinalIgnoreCase))
                .Select(l => new AuditoriaLogDto
                {
                    Id            = l.Id,
                    Fecha         = l.Fecha,
                    Usuario       = l.Usuario?.Nombre ?? "—",
                    TipoOperacion = ObtenerTipoOperacion(l.TipoOperacion),
                    Tabla         = l.Tabla,
                    RegistroId    = l.RegistroId,
                    Detalles      = l.Detalles,
                })
                .OrderByDescending(l => l.Fecha)
                .ToList();

            return new AuditoriaCompletaCajaDto
            {
                AuditoriaCajas    = auditoriaCajas,
                MovimientosCaja   = movimientosCaja,
            };
        }

        private static string ObtenerTipoOperacion(OperacionAuditoriaEnum tipo)
            => tipo switch
            {
                OperacionAuditoriaEnum.Creacion      => "Creación",
                OperacionAuditoriaEnum.Modificacion  => "Modificación",
                OperacionAuditoriaEnum.Eliminacion   => "Eliminación",
                _                                      => tipo.ToString(),
            };
    }
}
