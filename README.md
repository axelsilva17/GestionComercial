# GestiónComercial POS

**Sistema de Punto de Venta (POS) para comercios minoristas — .NET 8 + WPF**

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows%20Desktop-blue)](https://dotnet.microsoft.com/apps/wpf)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

---

## Funcionalidades

| Módulo | Descripción |
|--------|-------------|
| **Ventas** | Registro con múltiples métodos de pago, descuentos por item, selección de productos con búsqueda |
| **Compras** | Control de proveedores, ingreso de mercadería, cálculo automático de costos |
| **Inventario** | Stock por producto, ajuste masivo de precios, stock crítico configurable |
| **Caja** | Apertura / cierre / movimientos, control de turnos, auditoría |
| **Clientes** | Catálogo con historial de ventas |
| **Proveedores** | Catálogo con historial de compras |
| **Reportes** | Dashboard con gráficos, reporte diario, stock crítico |
| **Seguridad** | Autenticación por email + BCrypt, roles (Admin / Vendedor) |

---

## Tecnologías

| Componente | Tecnología |
|------------|------------|
| Framework | .NET 8.0 |
| UI | WPF (Windows Presentation Foundation) |
| ORM | Entity Framework Core 8.0 |
| Base de datos | SQLite (local, sin servidor) |
| MVVM | Caliburn.Micro |
| Gráficos | LiveChartsCore.SkiaSharpView.WPF |
| Excel | ClosedXML |
| Contraseñas | BCrypt.Net-Next |
| Testing | xUnit + FluentAssertions + Moq |

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows (la app es WPF, solo Windows)
- Visual Studio 2022, VS Code, o Rider

---

## Configuración rápida

```bash
# Clonar
git clone <repo>
cd GestionComercial

# Compilar y ejecutar
dotnet build
dotnet run --project GestionComercial.UI
```

La base de datos SQLite se crea automáticamente en la primera ejecución con las migraciones de EF Core aplicadas.

> **Nota:** El usuario administrador se crea automáticamente en la primera ejecución del sistema junto con la empresa y sucursal por defecto a través del asistente de configuración inicial.

---

## Arquitectura

El proyecto sigue una arquitectura limpia (Clean Architecture) con dependencias estrictas hacia adentro:

```
GestionComercial.UI               → Presentación WPF (Vistas, ViewModels, Bootstrapper)
GestionComercial.Aplicacion       → Casos de uso, DTOs, interfaces de servicios
GestionComercial.Dominio          → Entidades, reglas de negocio, interfaces de repositorios
GestionComercial.Persistencia     → EF Core DbContext, migraciones, repositorios
GestionComercial.Infraestructura  → Servicios externos (BCrypt, backup, logging)
```

Patrones: Repository, Unit of Work, MVVM (Caliburn.Micro), RelayCommand, Inyección de dependencias. Principios SOLID aplicados en toda la solución.

---

## Tests

```bash
dotnet test
```

La suite incluye tests unitarios para servicios de dominio (ventas, pagos, productos, clientes, autenticación) y validación de reglas de negocio. SQLite en memoria para aislamiento.

---

## Contribución

1. Crear un branch desde `main` con el formato `feature/descripcion` o `fix/descripcion`
2. Commits con mensajes claros siguiendo [Conventional Commits](https://www.conventionalcommits.org/)
3. Abrir Pull Request contra `main`

---

## Licencia

MIT License — ver el archivo [LICENSE](LICENSE) para más detalles.
