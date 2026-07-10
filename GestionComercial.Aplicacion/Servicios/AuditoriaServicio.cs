using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    ///     /// Implementación de IAuditoriaServicio. Convierte entidades AuditoriaLog a DTOs
    /// y aplica localización de TipoOperacion.
    public class AuditoriaServicio : IAuditoriaServicio
    {
        private readonly IUnitOfWork _uow;

        public AuditoriaServicio(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            var entidades = await _uow.Auditoria.ObtenerAuditoriaCajaAsync(
                idCaja: null,
                fechaDesde: fechaDesde,
                fechaHasta: fechaHasta);

            return entidades.Select(MapearADto);
        }

        public async Task<IEnumerable<AuditoriaLogDto>> ObtenerAuditoriaMovimientoCajaAsync(
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            var entidades = await _uow.Auditoria.ObtenerAuditoriaMovimientoCajaAsync(
                idMovimiento: null,
                fechaDesde: fechaDesde,
                fechaHasta: fechaHasta);

            return entidades.Select(MapearADto);
        }

        ///         /// Mapea una entidad AuditoriaLog a AuditoriaLogDto, localizando TipoOperacion
        /// de código numérico a texto legible.
        internal static AuditoriaLogDto MapearADto(AuditoriaLog entidad)
        {
            return new AuditoriaLogDto
            {
                Id = entidad.Id,
                NombreTabla = entidad.NombreTabla,
                RegistroId = entidad.RegistroId,
                TipoOperacion = entidad.TipoOperacion switch
                {
                    (int)OperacionAuditoriaEnum.Insert => "Creación",
                    (int)OperacionAuditoriaEnum.Update => "Modificación",
                    (int)OperacionAuditoriaEnum.Delete => "Eliminación",
                    _ => "Desconocido"
                },
                Usuario = entidad.NombreUsuario ?? "—",
                FechaOperacion = entidad.FechaOperacion,
                ValoresAnteriores = entidad.ValoresAnteriores,
                ValoresNuevos = entidad.ValoresNuevos
            };
        }
    }
}
