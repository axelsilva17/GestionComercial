# CajaTurnos_AdminExport_AuditoriaOnDemand_OnDemandCajaAuditoria - MVVM Frontend Specification

## Purpose

Provide a MVVM-based UI capability to export the Auditoria On-Demand data for CajaTurnos Admin, enabling admins to trigger on-demand exports from the frontend.

## Domain

- CajaTurnos Admin Export

## Requirements

### R1: Export button
- The system MUST render an "Exportar Auditoría On-Demand" button on the CajaTurnos Admin Export screen for users with admin role.

### R2: Date range validation
- The user MUST be able to select a start date and an end date.
- The system SHALL validate that the start date is before or equal to the end date; otherwise, a validation message is shown and the export is not triggered.

### R3: API contract
- The frontend SHALL call POST /api/caja-turnos/auditoria/export with payload:
- { "startDate": "<YYYY-MM-DD>", "endDate": "<YYYY-MM-DD>", "type": "OnDemandCajaAuditoria" }
- The backend SHALL respond with a file blob (or a direct download URL) for the auditoria export.

### R4: UI state and feedback
- The UI SHALL indicate exporting state with a progress indicator and SHALL disable the export button during the request.
- On success, the UI SHALL trigger a file download named with the export date range, e.g., CajaTurnos_Auditoria_OnDemand_2026-03-01_2026-03-31.csv.
- On failure, the UI SHALL present a user-friendly error message and log the error for telemetry.

### R5: Accessibility
- All actionable elements MUST have accessible labels and keyboard focus management.

## Scenarios

### Scenario 1: Happy path
- GIVEN an admin user on the CajaTurnos Admin Export screen with a valid date range
- WHEN the user clicks the Export button
- THEN a loading indicator appears and the request is sent
- AND the backend returns a file
- THEN a download starts for CajaTurnos_Auditoria_OnDemand_<start>_<end>.csv
- AND a success notification is shown

### Scenario 2: Backend error
- GIVEN a backend error during export
- WHEN the user initiates export
- THEN a user-friendly error message is shown
- AND error telemetry is recorded

### Scenario 3: Invalid date range
- GIVEN an invalid date range (start > end or missing dates)
- WHEN the user attempts export
- THEN a validation message is shown and no API call is made

## MVVM Mapping
- Model: AuditoriaExportRequest
- View: CajaTurnosAdminExportOnDemandView
- ViewModel: OnDemandCajaAuditoriaVM
- Service: CajaTurnosAuditoriaService

## Acceptance Criteria
- [ ] UI contains Exportar Auditoría On-Demand button
- [ ] Date range validation prevents invalid exports
- [ ] API call matches contract and returns downloadable content
- [ ] Success path downloads a file and shows a toast
- [ ] Error path shows user-friendly message and logs telemetry

## Where
- Frontend MVVM module: GestionComercial.UI/src/app/features/caja-turnos/admin-export/

## Learnings
- Keep the spec under 650 words; prefer concise, measurable criteria.
