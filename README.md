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

> La base de datos SQLite se crea automáticamente en la primera ejecución con las migraciones de EF Core aplicadas. El usuario administrador se genera junto con la empresa y sucursal por defecto mediante el asistente de configuración inicial.

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

Patrones: Repository, Unit of Work, MVVM (Caliburn.Micro), Strategy (procesamiento de pagos), RelayCommand, Inyección de dependencias. Principios SOLID aplicados en toda la solución.
