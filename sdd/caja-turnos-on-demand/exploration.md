## Exploration: Caja Turnos On-Demand

### Current State
- AperturaCaja flow currently allows entry of vendor information but does not incorporate Caja (cash register) and Turno (shift) selection.
- OnDemandCajaAuditoriaView does not exist yet; auditing of caja-related operations is not exposed in the UI.
- ReporteAdminView lacks explicit Caja Auditoría, Exportación Completa, and Turnos buttons to drive caja auditing/exporting and turno management.
- The tech stack (WPF, C#, Caliburn.Micro, Entity Framework) implies MVVM pattern with data binding, and EF as the data layer.

### Affected Areas
- Path: src/GestionComercial.UI/AperturaCajaView.xaml + AperturaCajaViewModel.cs
- Path: src/GestionComercial.UI/OnDemandCajaAuditoriaView.xaml + OnDemandCajaAuditoriaViewModel.cs
- Path: src/GestionComercial.UI/Reportes/ReporteAdminView.xaml + ReporteAdminViewModel.cs
- Path: src/GestionComercial.Core/Domain/Entities.cs (Caja, Turno models or references)
- Migration/Seed logic if Caja/Turno are new entities

### Approaches
1) Monolithic update (recommended for this change)
   - Update AperturaCaja to include Caja and Turno selection (data-bound dropdowns, EF queries to fetch available cajas/turnos)
   - Implement a full OnDemandCajaAuditoriaView (UI + view model) to support On-Demand Caja Auditing workflows
   - Extend ReporteAdminView to include three new buttons: Caja Auditoría, Exportación Completa, Turnos; wire to corresponding actions
   - Ensure data access and services support On-Demand flows (repositories, services)
   - Sync main, Rama1, Rama2 after implementation
   - Pros: single coherent change; easier to review end-to-end
   - Cons: bigger surface area, higher risk
   - Effort: High

2) Phased approach (alternative)
   - Phase 1: Add Caja/Turno fields to AperturaCaja and wire basic persistence
   - Phase 2: Implement OnDemandCajaAuditoriaView with core features (read-only explorer first, then full actions)
   - Phase 3: Update ReporteAdminView with buttons and wiring
   - Phase 4: Git branches sync and merge strategy
   - Pros: smaller risk per phase; easier rollout
   - Cons: longer overall delivery
   - Effort: Medium to High per phase

### Recommendation
Proceed with a phased approach to minimize risk while delivering incremental value. Start by enhancing AperturaCaja to support Caja/Turno selection, followed by the OnDemandCajaAuditoriaView, then the ReporteAdminView enhancements, and finally perform branch synchronization across main, Rama1, and Rama2.

### Risks
- UI changes must align with existing styling and Caliburn.Micro conventions to avoid regressions in navigation and binding
- EF mappings for Caja and Turno (if new entities) require careful migrations and seed data
- Concurrent edits to ReporteAdminView in different branches could cause merge conflicts
- QA needs to validate permissions and data integrity across On-Demand workflows

### Ready for Proposal
Yes — the team can proceed to elaborate delta specs, create task boards, and implement in sprints.
