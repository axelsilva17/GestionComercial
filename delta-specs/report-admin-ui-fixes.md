# Delta Spec: report-admin-ui-fixes

Project: GestionComercial
Change scope: admin report UI, navigation, export
Target branches: main, Rama1, Rama2

## Domain
Admin UI – Reports

## Objectives
- Enable a coherent admin reports experience with improved navigation and export capabilities.
- Ensure the reports grid can be exported (CSV, XLSX, PDF) reflecting current filters, sorting, and pagination state.
- Align with existing UI views ReporteAdminView, CajaAuditoriaView, and OnDemandCajaAuditoriaView while minimizing coupling.

## Scope
- Admin report UI changes, navigation entry, and export behavior.
- Affects the following views: ReporteAdminView, CajaAuditoriaView, OnDemandCajaAuditoriaView.
- No changes to core report data models; focus is on presentation and export behavior.

## Requirements (RFC 2119)
- The system SHALL expose an Admin Reports section under the admin navigation at /admin/reports.
- The ReporteAdminView SHALL provide export controls for CSV, XLSX, and PDF formats. Each export SHALL reflect the current filter state (date range, search terms, and other grid filters).
- The CajaAuditoriaView and OnDemandCajaAuditoriaView SHALL support exporting their current datasets using the same export mechanisms.
- Exports SHOULD be performed server-side when possible and downloaded by the client; large datasets SHOULD stream or paginate exports to avoid timeouts.
- Admin users SHALL be required to have the proper authorization to export reports.
- Export controls SHOULD be accessible via keyboard and screen readers (ARIA labels, focus order).

## Functional Scenarios
- Happy path: Admin navigates to Reports, applies filters, clicks Export CSV; a file downloads and its content matches the currently displayed grid data.
- Happy path: Admin exports in XLSX and PDF formats; the generated file preserves column headers and data types; user receives a success notification.
- Edge: No data after filters; exporting yields a file containing only headers or a lightweight placeholder; user sees a warning toast.
- Edge: Server export fails (timeout or error); UI shows an error toast with retry option.
- Edge: Large dataset; export operation queues or streams; progress indicator shows status; user can continue using the UI.
- Edge: Access control violation attempted export; UI denies with an authorization message.

## Data Models Impacted
- No new core data models introduced. The export layer SHALL respect existing data contracts for reports.
- When exporting, the payload SHOULD include: current filters, sort orders, and pagination state; response returns a downloadable file with appropriate MIME type.

## Non-functional Considerations
- Performance: Export MUST avoid blocking the UI; prefer server-side generation and streaming for large datasets; implement reasonable timeouts and user feedback.
- Accessibility: Export controls SHALL include aria-labels; keyboard focus order MUST be intuitive; color contrast of UI elements follows WCAG AA guidelines.
- Usability: Loading indicators during export; non-blocking UI interactions; consistent styling with existing admin views.
- Internationalization: Date, number formats in exported data MUST respect locale settings where applicable.

## Integration Points with Existing UI
- ReporteAdminView: Augment with export controls; reuse existing grid state (filters, sorts, pagination).
- CajaAuditoriaView: Ensure export action mirrors ReporteAdminView behavior for its dataset.
- OnDemandCajaAuditoriaView: Ensure export handles on-demand data, including possibly dynamic query generation.
- Common export utility/service: Centralize export logic to ensure consistency across views and formats.

## Testing Strategy
- Unit: Verify mapping from grid state to export payload; test export utility for CSV/XLSX/PDF formatting placeholders.
- Integration: Test navigation to /admin/reports and export API endpoints; verify correct query parameters reflect filters.
- UI/E2E: End-to-end tests (Playwright/Cypress) to validate navigation, filter application, export downloads, and accessibility checks.
- Regression: Ensure existing admin views (unrelated to reports) remain unaffected.

## Acceptance Criteria
- Admin Reports section exists in admin navigation and loads at /admin/reports.
- ReporteAdminView provides functional CSV, XLSX, and PDF export reflecting current filters and grid state.
- CajaAuditoriaView and OnDemandCajaAuditoriaView exports mirror the same behavior.
- Exports are accessible, handle large datasets, and fail gracefully with appropriate messages.
- Automated tests cover unit, integration, and UI scenarios.

## Notes
- This delta is designed to be implemented in parallel across the three target branches; ensure branch alignment and merge strategy to avoid conflicts.
