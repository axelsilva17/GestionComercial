# Spec: Refactorización de Módulos de Venta y Automatización de Flujo de Caja

## 1. Sincronización de UI (Historial)

### Objetivo
Unificar la lógica y el diseño del "Historial de Ventas" para que el acceso desde la vista de ventas principal y el acceso post-pago compartan el mismo proceso de renderizado y datos.

### Requisitos
- **Regla de Sincronización**: Si se cambia un margen, color, tamaño de fuente o cualquier estilo en uno, se debe ver reflejado automáticamente en el otro.
- **Filtros existentes**:
  - Filtro por fecha (Desde/Hasta)
  - Filtro por estado (pendiente, completada, anulada)
  - Filtro por DNI del cliente
- **Mejora de UI**: Hacer el popup más usable (mejores espaciados, mejor contraste, etc.)
- **Filtro de Estado**: Debe funcionar correctamente (actualmente no filtra)

---

## 2. Corrección de Flujo (Filtro de Categorías)

### Objetivo
Depurar la reactividad del filtro de categorías para asegurar actualización inmediata.

### Problema Actual
- El ComboBox de categorías no permite abrirlo
- No aparecen las distintas categorías disponibles
- El sistema no deja seleccionar una categoría

### Requisitos
- Al abrir el ComboBox, deben aparecer todas las categorías disponibles para la empresa
- Al seleccionar una categoría, debe disparar inmediatamente el refresco de la lista de productos disponibles (tiempo < 100ms)
- Si no hay productos en esa categoría, mostrar mensaje apropiado

---

## 3. Lógica de Entrada (Scanner)

### Objetivo
Implementar validación para entrada de datos por hardware (Scanner de códigos de barras).

### Requisitos
- **Detección de entrada rápida**: Identificar cuando los datos vienen del scanner (entrada muy rápida seguida de Enter)
- **Tick instantáneo**: Procesar el "tick" del producto al instante (< 100ms) y cargar automáticamente al carrito de venta actual
- **Fallback**: Si el código no existe en la base de datos, mostrar mensaje de "Producto no encontrado"
- El usuario puede seguir escribiendo manualmente si el scanner no funciona

---

## 4. Módulo de Caja "Ciego" (Automatización y Seguridad)

### 4.1 Registro Automático

### Requisitos
- Cada transacción confirmada (venta completada) debe asentar el monto de efectivo directamente en el libro de caja en tiempo real
- Actualizar saldo de caja automáticamente sin intervención del usuario

### 4.2 Cierre de Caja Seguro

### Requisitos
- Al solicitar el cierre de caja, el sistema NO debe mostrar el saldo esperado ni el total acumulado al usuario
- El usuario debe ingresar manualmente el monto físico que tiene en caja
- El sistema comparará internamente ese valor con el saldo calculado internamente
- Generará el reporte de diferencia (sobrante/faltante) solo después de confirmar la operación

### 4.3 Permisos y Visibilidad

### Requisitos
- **Vendedor**:
  - Ve el módulo de caja para abrir/cerrar caja
  - NO ve el botón de exportar reporte de cierre
  - NO puede acceder al reporte de caja pendiente

- **Administrador**:
  - NO ve el módulo de caja en el menú
  - Puede acceder al reporte de caja pendiente desde la sección de Reportes
  - Puede exportar el reporte de cierre de caja

### 4.4 Reporte de Caja Pendiente

### Requisitos
- El reporte de cierre de caja debe aparecer como "Reporte de Caja Pendiente" cuando el Administrador abre la sección de Reportes
- El administrador lo ve instantáneamente sin necesidad de buscarlo
- El reporte incluye:
  - Fecha y hora de apertura/cierre
  - Monto declarado por el usuario
  - Monto calculado internamente
  - Diferencia (sobrante/faltante)
  - Detalle de transacciones del día

---

## Resumen de Permisos

| Funcionalidad | Vendedor | Administrador |
|---------------|----------|---------------|
| Abrir caja | ✅ | ❌ |
| Cerrar caja | ✅ | ❌ |
| Ver módulo caja en menú | ✅ | ❌ |
| Exportar reporte cierre | ❌ | ✅ |
| Ver reporte pendientes | ❌ | ✅ |
| Sección reportes | ❌ | ✅ |