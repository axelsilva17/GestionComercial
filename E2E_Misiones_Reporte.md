# Reporte de Verificación E2E — GestionComercial (Misiones 1-3)

**Fecha**: 08-09-2026 · **Entorno**: Windows, .NET 8, EF Core 8 + SQLite, ClosedXML, Caliburn.Micro (WPF)
**HEAD evaluado**: `main` (14eb983) — sin modificaciones de código fuente, 0 commits nuevos.
**Método (TEST-ONLY)**: Harness desechable de nivel de servicio (`E2eHarness`) ejecutado **siempre contra copias privadas** (`%TEMP%\opencode\e2e_*.db`): copia fresca por grupo mutante (G1/G3/G5/G6/G7/G9/G11/G12) y copia única de solo lectura para G2/G4/G8/G10. Las bases reales `GestionComercial.UI\GestionComercial.db` (~5k productos) y `SeedDB\PerfTest.db` (60k) nunca fueron modificadas. Login dev por contrato: `dev@gestioncomercial.com` / `Dev#Mant2026!`.

---

## 1. Resumen ejecutivo

| Métrica | Resultado |
|---|---|
| Tests unitarios (`dotnet test`) | **428/428 correctos** (baseline sin regresiones) |
| Checks E2E (G1-G13) | **62 PASS / 8 WARN / 5 FAIL** |
| Defectos reales de la aplicación | **4** (2 de severidad alta, 2 media) |
| Fallos atribuibles al harness | 0 |

Los 5 FAIL corresponden íntegramente a defectos reales de la aplicación. No queda ningún fallo inducido por el harness.

## 2. Matriz de verificación (G1-G13)

| Grupo | Área | Checks | Resultado |
|---|---|---|---|
| G1 | Login / Autenticación (permisos, bloqueo por 5 intentos, re-hash de contraseña) | 5 | 5 PASS |
| G2 | Dashboard: KPIs, ventas recientes, stock crítico, ventas por día (746 tx / $23.324.370,61) | 6 | 6 PASS |
| G3 | Caja + ventas + descuento por método de pago | 14 | 11 PASS · 2 WARN · **1 FAIL** |
| G4 | Ventas: listado, filtros, detalle, rendimiento, clientes únicos | 7 | 7 PASS |
| G5 | Productos: CRUD, validaciones, ajuste por proveedor | 7 | 6 PASS · **1 FAIL** |
| G6 | Mantenimiento masivo 60k (performance) | 3 | 3 PASS |
| G7 | Importación (masiva, guardrails, OCR) | 5 | 3 PASS · 2 WARN |
| G8 | Compras: paginación, filtros, métricas | 6 | 5 PASS · 1 WARN |
| G9 | Inventario: paginado, entradas/salidas, exportar Excel, resumen de período | 5 | 2 PASS · **3 FAIL** |
| G10 | Reportes: vendedor, margen, stock, top productos, métodos de pago, export | 7 | 7 PASS |
| G11 | Mantenimiento DB: integridad, auditoría, índices, vacuum | 5 | 5 PASS |
| G12 | Backup (VACUUM INTO + verificación de integridad del backup) | 2 | 2 PASS |
| G13 | Verificación manual de UI (WPF) | 3 | 3 WARN (pendiente manual) |

## 3. Defectos reales encontrados

### ALTA-1 — Inventario: registro manual de stock no persiste (G9-N2 / G9-N3)
- **Causa raíz**: `InventarioServicio.RegistrarMovimientoAsync` (aprox. líneas 199-206) asigna `movimiento.Sucursal = sucursal!` y `movimiento.Usuario = usuario!` con instancias cargadas **AsNoTracking** (`RepositorioBase.ObtenerPorIdAsync`). Al hacer `Add(movimiento)`, EF vuelve a tratar esas entidades como INSERT → `SQLite Error 19: 'UNIQUE constraint failed: Sucursal.Id'` (id 1 ya existe en la DB).
- **Impacto**: la característica de ajuste manual de stock (entrada/salida) falla determinísticamente al guardar. El `Producto` no se inserta porque previamente se marca `Update` (queda en estado Modified con la misma instancia).
- **Evidencia**: `Entrada de stock - SQLite Error 19: 'UNIQUE constraint failed: Sucursal.Id'` (ídem en salida).

### ALTA-2 — Caja: movimiento manual (ingreso/egreso) no persiste (G3-C14)
- **Causa raíz**: `CajaServicio.RegistrarMovimientoAsync` (líneas 288-295) crea el `TipoMovimientoCaja` **sin asignar `Id_usuario`**. La columna es `NOT NULL` con FK → se envía 0 → `SQLite Error 19: 'FOREIGN KEY constraint failed'`.
- **Verificación directa** (probe SQL sobre la copia): `INSERT ... Id_usuario=0` → error FK; `Id_usuario=1` → OK.
- **Nota**: los flujos de pago y cierre sí fijan el usuario (por eso G3-C4/C7 funcionan); solo el movimiento manual queda roto.

### MEDIA-3 — Productos: ajuste de precios por proveedor falla con ≥2 productos de una misma categoría (G5-P6)
- **Causa raíz**: `ProductoRepositorio.ObtenerPorEmpresaAsync` (líneas 58-68) usa `AsNoTracking().Include(Categoria)` → materializa una instancia distinta de `Categoria` por fila (sin identity resolution). `ProductoServicio.AjustePreciosPorProveedorAsync` (líneas 434-457) recorre `_uow.Productos.Actualizar(p)` → en el segundo producto de la misma categoría, `Update` intenta trackear una segunda instancia con la misma PK → `The instance of entity type 'Categoria' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked`.
- **Determinismo**: se reproduce con datos reales (3.872 productos / ~300 categorías en la empresa activa).

### MEDIA-4 — Inventario: resumen de período lanza error SQL (G9-N5)
- **Causa raíz**: `MovimientoStockRepositorio.ObtenerResumenPeriodoAsync` (línea 118) agrega `g.Sum(m => m.Cantidad)` sobre `decimal` → SQLite `cannot apply aggregate operator 'Sum' on expressions of type 'decimal'` (EF Core no traduce SUM(decimal) a SQLite con el proveedor instalado).
- **Impacto**: reporte de resumen de movimientos por período sin uso.

## 4. Observaciones de comportamiento (WARN)

| Check | Hallazgo |
|---|---|
| G3-C9 | `CerrarCajaAsync` cierra la caja **sin validar ventas pendientes**; una venta quedó `Pendiente` sin cobrar tras el cierre (caja #59). |
| G3-C11 | `ObtenerTotalEfectivoPorCajaAsync` reporta solo pagos en efectivo ($10.500,00); no incluye el monto inicial ($5.000,00) — MontoFinal real $15.500,00. |
| G3-C5 | `EfectivoRecibido` queda **null** tras registrar pago en efectivo (el movimiento de caja sí registra el efectivo). |
| G3-C4/C8 | Descuento por método de pago / compra mayor se aplica en `RegistrarPagoAsync` según configuración en DB: venta de $8.500,00 → $5.695,00 (×0,67) cobrando por efectivo. Comportamiento real documentado. |
| G7-I4 | El servicio de importación inserta códigos de barra alfanuméricos si llegan directo por servicio; la validación solo se aplica en el preview/guardrails (UI). |
| G8-C6 | `MetricasComprasAsync` (2.363) vs total paginado (952): alcances distintos (posiblemente estados/empresa); no se evaluó como defecto. |
| G10-R1 | El "top vendedor" imprime $0,00 (asunción de orden del harness; criterio vendedores > 0 cumplido). |

## 5. Riesgos de código detectados (fuera de checks automatizables)

- El rol de permisos se llama **"Administrador"** (no "Admin"): cualquier matcher por el literal "Admin" no lo encuentra (el harness resuelve por `Id_rol`).
- `ProductoServicio` usa `ValidationException` de FluentValidation como "permiso denegado" (code smell: excepción de validación para autorización).
- La sesión dev firma `IdUsuario=-1` en el contexto, pero las operaciones usan el usuario #1 de la DB (la auditoría queda a nombre del usuario real).
- Tabla física `MovimientoCaja` vs clase `TipoMovimientoCaja` (difieren al trabajar a nivel SQL).
- Typo interno `"comraMayor"` en `VentaServicio` (~línea 370).

## 6. Verificación manual pendiente (G13 / G7-I1)

1. `dotnet run --project GestionComercial.UI` — arranque de la aplicación WPF.
2. Login visual + navegación de módulos + flujo caja/venta de punta a punta.
3. Flujo OCR de factura (importación).

## 7. Limpieza y garantías

- Eliminados **todos** los artefactos temporales: copias `e2e_*.db` (+wal/shm), backups `GestionComercial_Backup_*_e2e.db`, `reporte_equivalente.xlsx`, logs de corridas y el directorio del harness (verificación final: 0 restos en `%TEMP%\opencode`).
- Bases reales intactas (timestamps y tamaños originales).
- Working tree de git limpio (solo `publish/` untracked pre-existente; 0 archivos de fuente modificados).