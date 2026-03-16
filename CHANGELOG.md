# GestiónComercial POS

**Versión:** 1.0.0  
**Fecha de Release:** 16 de Marzo de 2026  
**Estado:** Primera versión estable (GA)

---

## Descripción del Sistema

GestiónComercial es un sistema POS (Point of Sale) multi-empresa desarrollado en .NET 8.0 con WPF. Permite gestionar ventas, compras, inventario, caja, clientes, proveedores y reportes empresariales.

### Características Principales

- **Multi-empresa y multi-sucursal**: Una empresa puede tener múltiples sucursales, cada una con su propia caja y inventario.
- **Gestión de ventas**: Registro de ventas con múltiples métodos de pago, cálculo de descuentos e impuestos.
- **Gestión de compras**: Control de proveedores y órdenes de compra con entrada de inventario.
- **Control de inventario**: Trazabilidad completa de movimientos de stock por producto.
- **Gestión de caja**: Apertura, cierre y movimientos de caja por sucursal.
- **Reportes**: Dashboard y reportes gerenciales con gráficos.
- **Seguridad**: Control de usuarios, roles y permisos con autenticación.

---

## Arquitectura

El proyecto sigue una arquitectura limpia (Clean Architecture) con separación en capas:

```
GestionComercial.UI          → Aplicación WPF (presentación)
GestionComercial.Aplicacion  → Casos de uso y servicios
GestionComercial.Dominio     → Entidades y reglas de negocio
GestionComercial.Persistencia→ Acceso a datos (Entity Framework Core)
GestionComercial.Infraestructura → Servicios externos (impresión, etc.)
```

### Stack Tecnológico

| Componente | Tecnología |
|------------|------------|
| Framework | .NET 8.0 |
| UI | WPF (Windows Presentation Foundation) |
| ORM | Entity Framework Core 8.0 |
| Base de datos | SQL Server |
| MVVM | Caliburn.Micro |
| Gráficos | LiveChartsCore.SkiaSharpView.WPF |
| Excel | ClosedXML |
| Seguridad | BCrypt.Net-Next |

---

## Módulos del Sistema

### 1. Ventas
- Registro de ventas con selección de cliente
- Múltiples métodos de pago (split payment)
- Cálculo automático de totales, descuentos e impuestos
- Generación de comprobantes
- Anulación de ventas con reversión de stock

### 2. Compras
- Registro de compras a proveedores
- Entrada de inventario automática
- Control de condiciones de pago

### 3. Productos
- Catálogo de productos con categorías jerárquicas
- Importación masiva desde Excel
- Control de stock por sucursal
- Alertas de stock mínimo

### 4. Clientes
- Registro y gestión de clientes
- Tipos de documento soportados

### 5. Proveedores
- Catálogo de proveedores
- Condiciones de pago

### 6. Caja
- Apertura y cierre de caja por sucursal
- Movimientos de ingreso/egreso
- Control de una caja abierta a la vez

### 7. Configuración
- Gestión de usuarios y roles
- Permisos por rol
- Datos de la empresa

### 8. Reportes
- Dashboard con métricas principales
- Reportes gerenciales con gráficos

---

## Entidades del Dominio

- **Empresa** - Raíz multi-tenant
- **Sucursal** - Sucursales por empresa
- **Usuario** - Usuarios con roles
- **Rol / Permiso** - Control de acceso
- **Producto** - Catálogo de productos
- **Categoria** - Jerarquía de categorías
- **Cliente** - Clientes del negocio
- **Proveedor** - Proveedores
- **Venta / VentaDetalle** - Registro de ventas
- **Compra / CompraDetalle** - Registro de compras
- **Pago** - Métodos de pago
- **Caja / MovimientoCaja** - Gestión de caja
- **MovimientoStock** - Trazabilidad de inventario

---

## Configuración

### Requisitos

- .NET 8.0 SDK
- SQL Server (local o remoto)
- Windows 10/11

### Instalación

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar las migraciones de Entity Framework
4. Ejecutar las semillas (seeds) para datos iniciales
5. Compilar y ejecutar

```bash
dotnet build
dotnet run --project GestionComercial.UI
```

### Cadena de conexión

Editar `GestionComercial.UI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=GestionComercial;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Datos Iniciales (Seeds)

El sistema incluye semillas para:
- Roles del sistema
- Métodos de pago
- Unidades de medida
- Categorías de productos
- Estados de venta y caja
- Tipos de movimiento
- Usuarios demo

---

## Known Issues / Limitaciones

1. **Sin tests unitarios**: El proyecto no cuenta con suite de tests.
2. **Sin migraciones automáticas**: Requiere ejecución manual de migrations.
3. **Solo Windows**: La aplicación es WPF y solo funciona en Windows.
4. **Sin API REST**: Actualmente solo tiene interfaz de escritorio.
5. **Sin soporte multi-idioma**: Solo español.

---

## Roadmap Sugerido

### Versión 1.1.0 (sugerido)
- API REST para integración externa
- Tests unitarios en servicios críticos

### Versión 1.2.0 (sugerido)
- Exportación a PDF de comprobantes
- Integración con impresoras fiscales

### Versión 2.0.0 (sugerido)
- Versión web (Blazor)
- Aplicación móvil (Xamarin/MAUI)

---

## Licencia

MIT License

---

## Autor

Axel Silva
