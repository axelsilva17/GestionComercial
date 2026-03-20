# SPEC: Caja + Auditoría + Excel

## Overview

Extender y corregir el módulo de Caja para implementar auditoría completa de movimientos, métricas de prevención de fraudes, y exportación Excel de reportes administrativos y gerenciales.

## Requirements

### Caja Module

| # | Requirement | Strength | Description |
|---|-------------|----------|-------------|
| CAJA-001 | AperturaCajaViewModel | MUST | Consultar último cierre real de la DB y mostrar saldo anterior y fecha/hora del cierre |
| CAJA-002 | CajaViewModel | MUST | Cargar movimientos reales de la DB con desglose de ventas por forma de pago |
| CAJA-003 | CierreCajaViewModel | MUST | Persistir observación de cierre en la BD |
| CAJA-004 | Ingreso/Egreso Manual | MUST | Crear diálogo para registrar ingresos/egresos con validación de monto positivo |
| CAJA-005 | TipoMovimientoCaja | MUST | Registrar movimientos manuales en la entidad correspondiente |

### Auditoría Module

| # | Requirement | Strength | Description |
|---|-------------|----------|-------------|
| AUD-001 | Deserializar JSON | MUST | Mostrar ValoresAnteriores/ValoresNuevos de forma legible |
| AUD-002 | KPIs de Fraude | MUST | Calcular: caja con mayor diferencia, ventas anuladas por usuario, movimientos fuera de horario (antes 8am, después 10pm), forma de pago por vendedor |
| AUD-003 | Filtros de Auditoría | MUST | Filtrar por usuario, tipo de operación, y rango de fechas |

### Export Excel Module

| # | Requirement | Strength | Description |
|---|-------------|----------|-------------|
| EXP-001 | ClosedXML | MUST | Instalar paquete ClosedXML y habilitar flag |
| EXP-002 | ExportAuditoriaCaja | MUST | Implementar ExportarAuditoriaCajaExcel(AuditoriaCompletaCajaDto) |
| EXP-003 | ExportReporteAdmin | MUST | Implementar ExportarReporteAdminExcel con datos completos |
| EXP-004 | ExportReporteGerencia | MUST | Implementar exportación Excel para reportes de gerencia |

## Scenarios

### CAJA-001: Apertura con saldo real

- GIVEN el sistema tiene un cierre de caja anterior
- WHEN el usuario abre AperturaCajaView
- THEN se muestra el saldo anterior real y fecha/hora del último cierre
- AND se muestra el nombre del usuario que cerró

### CAJA-002: Ver movimientos reales

- GIVEN existe una caja abierta con movimientos
- WHEN el usuario abre CajaView
- THEN se cargan movimientos desde la DB
- AND se muestra desglose por forma de pago (efectivo, tarjeta, transferencia)
- AND se calculan totales en tiempo real

### CAJA-004: Registrar egreso manual

- GIVEN hay una caja abierta
- WHEN el usuario registra un egreso con monto negativo
- THEN el sistema muestra error de validación
- AND no se persiste el movimiento

### CAJA-004: Registrar egreso válido

- GIVEN hay una caja abierta
- WHEN el usuario registra un egreso de $500 con descripción "Compra de insumos"
- THEN se crea TipoMovimientoCaja con monto positivo
- AND el movimiento aparece en la lista de movimientos

### AUD-002: Detectar movimiento fuera de horario

- GIVEN existe un movimiento registrado a las 23:30
- WHEN se generan KPIs de fraude
- THEN el movimiento se marca como atípico
- AND se incluye en el reporte de movimientos fuera de horario

### AUD-002: KPI caja con mayor diferencia

- GIVEN existen cierres de caja con diferencias entre ventas y cobrado
- WHEN se calculan KPIs de fraude
- THEN se identifica la caja con mayor diferencia absoluta
- AND se calcula el porcentaje de diferencia

### EXP-002: Exportar auditoría a Excel

- GIVEN existen registros de auditoría de caja
- WHEN el usuario hace clic en Exportar Excel
- THEN se genera archivo .xlsx con headers: Fecha, Usuario, Operación, ValoresAnteriores, ValoresNuevos
- AND el archivo se descarga automáticamente

## Technical Changes

### Archivos a Modificar

| Archivo | Cambio |
|---------|--------|
| `AperturaCajaViewModel.cs` | Consultar último cierre de DB |
| `CajaViewModel.cs` | Cargar movimientos reales + desglose formas de pago |
| `CierreCajaViewModel.cs` | Persistir observación en CierreCaja.Observacion |
| `RegistrarIngresoEgresoView.xaml/.cs` | **NUEVO** - Diálogo para movimientos manuales |
| `AuditoriaLogDto.cs` | Deserializar JSON de ValoresAnteriores/ValoresNuevos |
| `AuditoriaAppService.cs` | Agregar KPIs de fraude + filtros |
| `ExportHelper.cs` | Habilitar CLOSEDXML_INSTALADO + nuevos métodos |
| `GestionComercial.csproj` | Agregar paquete ClosedXML |

### Archivos a Crear

| Archivo | Descripción |
|---------|-------------|
| `RegistrarIngresoEgresoView.xaml` | Diálogo WPF para ingreso/egreso |
| `RegistrarIngresoEgresoViewModel.cs` | ViewModel del diálogo |
| `MovimientoCajaDto.cs` | DTO para movimientos con desglose |

## Acceptance Criteria

- [ ] AperturaCajaViewModel muestra saldo real del último cierre
- [ ] CajaViewModel carga y muestra movimientos de la DB
- [ ] CierreCajaViewModel persiste observación exitosamente
- [ ] Diálogo de ingreso/egreso valida montos positivos
- [ ] AuditoriaLogDto muestra JSON deserializado de forma legible
- [ ] KPIs de fraude calculan: diferencia por caja, anulaciones por usuario, horarios atípicos
- [ ] Filtros de auditoría funcionan por usuario, tipo, y fechas
- [ ] ClosedXML instalado y flag habilitado
- [ ] ExportAuditoriaCajaExcel genera archivo descargable
- [ ] ExportReporteAdminExcel incluye todos los datos del reporte
- [ ] ExportReporteGerenciaExcel genera reporte completo

## Tasks

1. **Caja Core**
   - [ ] Modificar AperturaCajaViewModel para consultar DB
   - [ ] Modificar CajaViewModel para cargar movimientos reales
   - [ ] Modificar CierreCajaViewModel para persistir observación
   - [ ] Crear diálogo RegistrarIngresoEgresoView
   - [ ] Crear RegistrarIngresoEgresoViewModel
   - [ ] Agregar validación de monto positivo

2. **Auditoría**
   - [ ] Modificar AuditoriaLogDto para deserializar JSON
   - [ ] Agregar método CalcularKpisFraude en AuditoriaAppService
   - [ ] Agregar filtros por usuario/tipo/fechas

3. **Excel Export**
   - [ ] Instalar ClosedXML via dotnet add package
   - [ ] Cambiar CLOSEDXML_INSTALADO a true
   - [ ] Implementar ExportarAuditoriaCajaExcel
   - [ ] Implementar ExportarReporteAdminExcel
   - [ ] Implementar ExportarReporteGerenciaExcel
   - [ ] Agregar botones de exportar en UI
