## Exploration: CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria

### Current State
- The backend domain and application layers already contain support for on-demand auditing for Caja and MovimientosCaja via an in-process MVVM-like flow, with DTOs in the Application layer and Domain, and no REST surface.
- Domain: OnDemandCajaAuditoriaDomainService and InMemoryOnDemandCajaAuditoriaRepositorio provide the core GetOnDemandAsync data retrieval and sample data.
- Application: OnDemandCajaAuditoriaAppService maps domain DTOs to application DTOs (CajaAuditoriaOnDemandDto) for consumption by the UI.
- DTOs and mappers exist: GestionComercial.Dominio.DTOs (CajaAuditoriaOnDemandDto, MovimientosRecientesDto) and GestionComercial.Aplicacion.DTOs.OnDemandCajaAuditoria (CajaAuditoriaOnDemandDto, MovimientosRecientesDto), plus OnDemandCajaAuditoriaMapper.
- DI wiring is available in BackendDIExtensions to wire up the backend services.

### Affected Areas
- Dominio: OnDemandCajaAuditoria (OnDemandCajaAuditoriaDomainService, DTOs, Repositorio interface)
- Aplicación: OnDemandCajaAuditoriaAppService, OnDemandCajaAuditoriaMapper, DTOs
- Infraestructura/Repositorios: IOnDemandCajaAuditoriaRepositorio, InMemoryOnDemandCajaAuditoriaRepositorio
- UI (informational): UI could consume this through the existing MVVM pattern, no UI files touched in this exploration

### Approaches
1) Proseguir con enfoque backend in-process (recomendado):
   - Ampliar y refinar GetOnDemandAsync para cubrir más filtros (usuario, rango de fechas, rango de período) y KPIs, manteniendo el flujo in-process.
   - Mantener el repositorio en memoria para prototipos y pruebas; plan de migración a repositorio real si se aprueba.
   - Asegurar que el mapeo dominio → app DTO sea correcto y cubra todos campos requeridos por UI/consumo.

2) Exponer On-Demand export en UI en una fase posterior:
   - Reutilizar ExportHelper existente para export On-Demand sin romper export actuales.
   - Añadir nuevas UI components y ViewModels para soporte MVVM completo si se acuerda.

### Recommendation
- Empezar con Approach 1: extender backend para soporte On-Demand sin tocar UI, consolidando dominio, DTOs y AppService para GetOnDemandAsync y mapping a DTOs de Aplicación.

### Risks
- Mantener la on-demand en proceso puede limitar la interoperabilidad externa; si se requieren integraciones, se deberá contemplar REST o mensajes asíncronos en fases posteriores.
- Coherencia entre DTOs de dominio y aplicación; se deben cubrir pruebas de mapper y validación de KPIs.

### Ready for Proposal
Yes — con un plan claro de implementación y DTOs definidos para On-Demand y KPIs.

### Archivos/Sintaxis relevantes
- Backend: GestionComercial.Dominio.OnDemandCajaAuditoria, GestionComercial.Aplicacion.Servicios.OnDemandCajaAuditoriaAppService, OnDemandCajaAuditoriaMapper
- DTOs: GestionComercial.Dominio.DTOs, GestionComercial.Aplicacion.DTOs.OnDemandCajaAuditoria
- Repos/DI: BackendDIExtensions

(End of file)
