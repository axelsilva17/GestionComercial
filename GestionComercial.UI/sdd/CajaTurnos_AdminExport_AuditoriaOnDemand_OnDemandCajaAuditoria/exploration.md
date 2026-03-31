## Exploration: CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria (Frontend)

### Current State
- There are existing design/spec artifacts for this change:
  - Specs: CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria.md
  - Design: CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria.md
- The frontend paths indicated by the design/docs imply a MVVM-based feature under the CajaTurnos admin export area.
- Current CajaTurnos export UI exists (Export button, date filters) in the repo; OnDemand export for Auditoria is not yet implemented, but the design suggests reusing CajaTurnos MVVM conventions:
  - MVVM mapping: View, ViewModel, and a service/IoC integration
  - Navigation: IrCajaTurnos or similar patterns
- Current code references:
  - CajaTurnos admin export MVVM area: GestionComercial.UI/src/app/features/caja-turnos/admin-export/
  - Export process utilities: ExportHelper.cs
- The repo contains a pair of docs showing intended UI components:
  - AuditoriaOnDemandView, AuditoriaOnDemandViewModel
  - Interactions with IUnitOfWork and a UI-layer bridge service

### Affected Areas
- GestionComercial.UI/sdd/specs/CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria.md — MVVM frontend spec
- GestionComercial.UI/docs/designs/CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria.md — MVVM frontend design
- GestionComercial.UI/src/app/features/caja-turnos/admin-export/ — target MVVM module for the new export feature
- GestionComercial.UI/ViewModels/Cajas/AuditoriaOnDemandViewModel.cs — (to be added) new ViewModel
- GestionComercial.UI/Views/Cajas/AuditoriaOnDemandView.xaml — (to be added) new View
- GestionComercial.UI/ShellViewModel.cs — (likely to be updated to add navigation IrAuditoriaOnDemand)

### Approaches
1. Reuse CajaTurnos MVVM, add AuditoriaOnDemand components
   - Pros: Keeps architecture consistent; leverages existing IoC, navigation, and unit-of-work patterns
   - Cons: Requires careful coordination with backend contracts; potential coupling
   - Effort: Medium
2. Create a standalone MVVM module for auditoria with minimal coupling
   - Pros: Clear separation; easier testing
   - Cons: Higher initial boilerplate; duplicate patterns
   - Effort: Medium-High

### Recommendation
- Proceed with Approach 1: integrate AuditoriaOnDemand into the existing CajaTurnos admin export MVVM area, reusing patterns (ViewModel, View, IoC, and navigation). This aligns with the design rationale and reduces cognitive load, enabling faster delivery.

### Risks
- Backend API shape and payload format are not yet defined; ensure alignment with /api/caja-turnos/auditoria/export contract
- Permissions and admin role checks must be enforced on the UI side
- Potential large data payloads; consider streaming or background export to avoid UI blocking

### Ready for Proposal
Yes — need confirmation on backend API contracts and exact data models, then proceed with implementing skeleton MVVM components and UI wiring.

### Open Questions
- Confirm backend API payload and endpoint specifics for on-demand auditoria export
- Decide final location for AuditoriaRecord model (UI-only vs shared with domain)
- Confirm navigation trigger naming in ShellViewModel (e.g., IrAuditoriaOnDemand)

## Observations for the Orchestrator
- The change is well-scoped to the frontend MVVM pattern; it leverages existing CajaTurnos structures rather than introducing a new architectural pattern.
- The included specs mention a downloadable export file named CajaTurnos_Auditoria_OnDemand_YYYY-MM-DD_YYYY-MM-DD.csv, which should shape the ViewModel export logic.

End of exploration
