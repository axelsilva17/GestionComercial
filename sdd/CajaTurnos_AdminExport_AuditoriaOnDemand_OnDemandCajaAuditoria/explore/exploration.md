## Exploration: CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria

### Current State
- Backend already exposes auditing capabilities for Caja and MovimientoCaja via AuditoriaServicio and AuditoriaAppService (GestionComercial.Aplicacion). The repository contains:
  - IAuditoriaServicio and AuditoriaServicio mapping AuditoriaLog entities to AuditoriaLogDto.
  - IAuditoriaAppService with methods for: ObtenerAuditoriaCompletaCajaAsync, CalcularKpisFraudeAsync, ObtenerAuditoriaFiltradaAsync, ObtenerAuditoriaCajaDeserializadaAsync.
  - AuditoriaRepositorio with query capabilities to fetch logs by table, user, date range and specific Caja/MovimientoCaja filters.
- There is currently no dedicated backend method to export Auditoría On-Demand data via a single API, but the UI spec expects POST /api/caja-turnos/auditoria/export with a payload including startDate, endDate and type: OnDemandCajaAuditoria.
- The domain entities for logs, and the DTOs used by the UI (AuditoriaLogDto, AuditoriaCompletaCajaDto, etc.) exist, but the export path would require new orchestration to serialize results into CSV/Blob or a downloadable URL.
- The UI layer (GestionComercial.UI) defines a MVVM mapping for OnDemandCajaAuditoria, including a ViewModel (OnDemandCajaAuditoriaVM) and a service CajaTurnosAuditoriaService, which will need to call the new backend API.

### Affected Areas
- Backend application services:
- GestionComercial.Aplicacion.Interfaces.Servicios: IAuditoriaAppService
- GestionComercial.Aplicacion.Servicios: AuditoriaAppService, AuditoriaServicio
- Persistence: AuditoriaRepositorio, GestionComercialContext
- UI: CajaTurnosAdminExportOnDemandView / OnDemandCajaAuditoriaViewModel (MVVM mapping)
- Data transfer objects: AuditoriaLogDto, AuditoriaCompletaCajaDto, FiltroAuditoriaDto, etc.

Affected files example (backend):
- GestionComercial.Aplicacion/Interfaces/Servicios/IAuditoriaAppService.cs
- GestionComercial.Aplicacion/Servicios/AuditoriaAppService.cs
- GestionComercial.Aplicacion/Servicios/AuditoriaServicio.cs
- GestionComercial.Persistencia/Repositorio/AuditoriaRepositorio.cs
- GestionComercial.Aplicacion/DTOs/Auditoria/AuditoriaLogDto.cs
- GestionComercial.Aplicacion/DTOs/Auditoria/AuditoriaCompletaCajaDto.cs

### Approaches
1. Export as CSV in-memory via API (Recommended)
- Description: Add a new method to IAuditoriaAppService to export logs for given date range, returning a byte[] (CSV). Implement in AuditoriaAppService by aggregating data from AuditoriaServicio (cajas y movimientos) and serializing to CSV. Expose an API controller endpoint /api/caja-turnos/auditoria/export that streams the CSV as a blob or downloadable URL.
- Pros:
  - Simple, synchronous UX path: the frontend receives binary data directly for download.
  - Reuses existing filtrado logic and DTOs.
- Cons:
  - Extra code path for serialization; needs careful handling of large data ranges.
- Effort: Medium

2. Export as background job with blob store (Alternative)
- Description: Offload export to a background job, store the CSV in a blob store (e.g., Azure Blob, S3) and return a download URL to the client.
- Pros:
  - Handles very large datasets gracefully; avoids blocking UI.
- Cons:
  - Adds infrastructure complexity and async UX requirements (polling/notifications).
- Effort: High

### Recommendation
- Proceed with Approach 1: implement a synchronous in-memory CSV export exposed via /api/caja-turnos/auditoria/export. This aligns with the current architecture, reuses existing Auditoria services, and provides a fast path for admin users with On-Demand needs. If export sizes become problematic, consider a follow-up background/export with blob storage.

### Risks
- Security: ensure only admin users can call the export endpoint.
- Performance: large date ranges may produce large payloads; consider streaming or chunked processing.
- Data governance: auditoría data sensitivity and retention policies must be respected.

### Ready for Proposal
Yes — provide a concrete API contract, a small DTO for the export payload, and implement a CSV serializer that flattens AuditoriaLogDto fields and their deserialized values.

## Notas para el equipo
- The MVVM mapping for the frontend (CajaTurnosAdminExportOnDemandView) will consume the export endpoint; ensure response is either a downloadable file or a URL depending on frontend preference.
- Add unit tests for the exporter: zero logs, some logs, logs with nested Deserializados fields.
