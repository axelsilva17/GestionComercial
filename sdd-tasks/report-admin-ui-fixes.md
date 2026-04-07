# Admin Report UI Fixes Delta — Actionable Tasks

- UI changes in ReporteAdminView
  - [ ] Add Turnos navigation button
  - [ ] Ensure layout remains compatible across viewports
  - [ ] Ensure "Caja Auditoría" button exists and is wired to the correct navigation
- CajaAuditoriaView
  - [ ] Ensure navigation from Admin to CajaAuditoria
  - [ ] Verify export controls exist (export button, format options)
  - [ ] Verify period filters exist and are wired (start, end dates or period selector)
- OnDemandCajaAuditoriaView
  - [ ] Verify it exists and is wired for export
  - [ ] Verify date filters are wired (from/to or date range)
- ViewModels
  - [ ] Implement MostrarTurnos in ReporteAdminViewModel to navigate to CajaTurnosViewModel
  - [ ] Ensure CajaAuditoriaViewModel exposes correct data flow (inputs/outputs)
  - [ ] Ensure OnDemandCajaAuditoriaViewModel exposes correct data flow
- Converters
  - [ ] Verify BoolToVisibilityConverter namespace exists and is used by all relevant views
  - [ ] Verify DecimalToMonedaConverter namespace exists and is used by all relevant views
- Branching
  - [ ] Ensure main, Rama1, Rama2 contain consistent code paths
  - [ ] Plan merge strategy (branching model, rebase vs merge, conflict resolution, CI checks)
- Tests
  - [ ] Outline unit tests for navigation, export, and data load
  - [ ] Outline integration/UI tests for admin navigation to CajaAuditoria and OnDemand path
  - [ ] Outline UI tests for presence of buttons and export triggers

Notes:
- The patch intentionally focuses on task scoping; actual code changes will be captured in sdd-tasks/tasks.md or in the relevant changes folders.
