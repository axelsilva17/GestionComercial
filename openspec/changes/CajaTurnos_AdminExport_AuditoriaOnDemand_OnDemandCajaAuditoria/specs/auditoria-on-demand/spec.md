# Auditoría On-Demand para CajaTurnos AdminExport

## Propósito
Proveer soporte de backend con mentalidad MVVM para auditar operaciones de exportación bajo demanda. Este dominio facilita registrar, consultar y exponer auditorías asociadas a exportaciones administrativas de CajaTurnos.

## Requisitos
- El sistema MUST persistir registros de auditoría con los campos: id, operación, objetivo, usuario_id, timestamp, estado, detalle.
- El repositorio debe exponer una interfaz para almacenar y recuperar registros de auditoría.
- Se MUST exponer un servicio que permita disparar una auditoría bajo demanda para una exportación dada.
- Se MUST exponer un endpoint API POST /api/audit/on-demand que acepte exportId y filtros opcionales y retorne el registro de auditoría creado.
- Los registros de auditoría MUST ser inmutables tras su creación.
- El sistema SHOULD garantizar que las operaciones de auditoría sean trazables y auditable para cumplimiento.
- El sistema MUST estar cubierto por pruebas unitarias e de integración.
- La capa de dominio, servicios y repositorios debe estar separada para facilitar pruebas y mantenimiento (arquitectura MVVM-Backend).

## Escenarios
- Escenario: Creación exitosa de auditoría bajo demanda
- GIVEN se recibe una solicitud válida con exportId y usuario
- WHEN se dispara la auditoría bajo demanda
- THEN se crea un registro de auditoría con estado CREATED y se devuelve su ID
- AND el registro es recuperable via la API de consulta

- Escenario: exportId ausente
- GIVEN la solicitud no incluye exportId
- WHEN se intenta disparar la auditoría
- THEN el sistema RESPONDE con 400 Bad Request y un mensaje de error claro

- Escenario: Recuperación de auditorías
- GIVEN existen registros de auditoría para un exportId específico
- WHEN se solicita la lista de auditorías
- THEN se devuelven los registros filtrados por exportId con paginación
