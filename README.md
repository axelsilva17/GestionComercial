# GestiónComercial POS

**Sistema de Punto de Venta (POS) multi-empresa desarrollado en .NET 8.0 con WPF**

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows%20Desktop-blue)](https://dotnet.microsoft.com/apps/wpf)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

---

## Acerca del Sistema

GestiónComercial es un sistema POS (Point of Sale) multi-empresa que permite gestionar de forma integral un comercio minorista:

- **Ventas**: Registro de ventas con múltiples métodos de pago, descuentos e impuestos
- **Compras**: Control de proveedores y entradas de inventario
- **Inventario**: Trazabilidad completa de stock por producto y sucursal
- **Caja**: Gestión de apertura, cierre y movimientos de caja
- **Clientes y Proveedores**: Catálogos de clientes y proveedores
- **Reportes**: Dashboard y reportes gerenciales con gráficos
- **Seguridad**: Control de usuarios, roles y permisos

---

## Versión Actual

**v1.0.0** - Primera versión estable (GA) - *Marzo 2026*

Ver [CHANGELOG.md](CHANGELOG.md) para el historial de versiones.

---

## Tecnologías

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

## Arquitectura

El proyecto sigue una arquitectura limpia (Clean Architecture):

```
GestionComercial.UI          → Aplicación WPF (presentación)
GestionComercial.Aplicacion  → Casos de uso y servicios
GestionComercial.Dominio     → Entidades y reglas de negocio
GestionComercial.Persistencia→ Acceso a datos (EF Core)
GestionComercial.Infraestructura → Servicios externos
```

---

## Primeros Pasos

### Requisitos

- .NET 8.0 SDK
- SQL Server (local o remoto)
- Windows 10/11

### Configuración

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Compilar y ejecutar:

```bash
dotnet build
dotnet run --project GestionComercial.UI
```

---

## Estructura del Proyecto

```
GestionComercial/
├── GestionComercial.UI/           # Aplicación WPF
│   └── Views/                      # Vistas XAML
│       ├── Ventas/
│       ├── Compras/
│       ├── Productos/
│       ├── Clientes/
│       ├── Proveedores/
│       ├── Caja/
│       ├── Reportes/
│       ├── Configuracion/
│       └── Main/
├── GestionComercial.Aplicacion/   # Servicios y casos de uso
├── GestionComercial.Dominio/      # Entidades e interfaces
│   └── Entidades/
│       ├── Ventas/
│       ├── Compras/
│       ├── Caja/
│       ├── Movimientos/
│       └── ...
├── GestionComercial.Persistencia/ # Acceso a datos
│   ├── Repositorio/
│   ├── Contexto/
│   └── Semillas/
└── GestionComercial.Infraestructura/ # Servicios externos
```

---

## Contribución

Para contribuciones, por favor crear un issue o pull request.

---

## Licencia

MIT License - ver [LICENSE](LICENSE) para detalles.
