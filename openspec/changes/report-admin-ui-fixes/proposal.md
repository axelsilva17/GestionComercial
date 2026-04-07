# Proposal: Admin Report UI fixes - Caja Auditoría button, Turnos button, and export

## Intent
- Correct navigation flows and export surface for Admin Report pages:
  - Caja Auditoría button routes to OnDemandCajaAuditoriaView
  - Turnos button opens the scheduling view
  - Export provides a reliable, well-formed output for admin reports

## Scope

### In Scope
- UI changes for ReporteAdminView, CajaAuditoriaView, OnDemandCajaAuditoriaView
- ViewModel wiring updates and navigation changes
- Export surface wiring and formatting

### Out of Scope
- Backend API changes beyond data shape needed by export
- Non-admin UI pages

## Capabilities

### New Capabilities
- admin-report-ui-navigation: navigate admin report flows (ReporteAdminView -> CajaAuditoriaView -> OnDemandCajaAuditoriaView)
- admin-report-export: export admin report data to CSV/Excel with consistent columns

### Modified Capabilities
- admin-report-view-model-wiring: update wiring across admin views
- admin-report-navigation-structure: update routes for Turnos and CajaAuditoria

## Approach
- Review current navigation graph and identify entry points for admin reports
- Implement UI changes in ReporteAdminView to wire Caja Auditoría and Turnos buttons to targets
- Implement OnDemandCajaAuditoriaView path and ensure export hook emits correct data
- Update VM wiring to coordinate across the three views

## Affected Areas

- ui/ReporteAdminView: update Caja Auditoría and Turnos button behavior
- ui/CajaAuditoriaView: ensure data flow to OnDemandCajaAuditoriaView
- ui/OnDemandCajaAuditoriaView: support export surface
- vm/AdminReportVM: cross-view state coordination
- navigation/routes.ts: add/adjust routes for new flows
- services/export/adminExportService.ts: ensure export formatting

## Milestones
- Week 1: Confirm requirements; align Rama1/Rama2 branches
- Week 2: Implement UI and VM wiring; update routes
- Week 3: Implement export surface and tests
- Week 4: QA, defects, branch sync, and PR prep

## Risks and Mitigations
- Navigation regressions: add UI tests and route reviews
- Export format regression: centralize formatter; validate with samples
- Branch divergence: schedule regular cross-branch syncs and CI checks

## Rollback Plan
- Revert navigation/export changes and restore previous behavior
- Re-run admin report tests to verify restoration

## Dependencies
- Admin export formatter and backend data surface (read-only for delta)

## Success Criteria
- Caja Auditoría button navigates to OnDemandCajaAuditoriaView
- Turnos button opens scheduling view without errors
- Admin report export has correct structure and data
- No regressions in other admin UI areas; automated tests pass
