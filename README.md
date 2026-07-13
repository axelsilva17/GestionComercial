# GestiónComercial POS

**Sistema de Punto de Venta (POS) para comercios minoristas — .NET 8 + WPF**

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows%20Desktop-blue)](https://dotnet.microsoft.com/apps/wpf)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

---

## Funcionalidades

| Módulo | Funciones clave |
|--------|----------------|
| **Ventas** | Nueva venta con búsqueda de productos, cobro dividido en múltiples métodos de pago (efectivo, tarjeta, transferencia, otros), descuentos por ítem, historial con detalle, anulación con motivo, comprobante |
| **Compras** | Registro de compras con selección de proveedor, ingreso de mercadería con precio de costo, cálculo automático de subtotales, historial |
| **Productos** | CRUD completo, categorías, código de barras, stock mínimo configurable, ajuste masivo de precios por categoría, importación desde Excel |
| **Inventario** | Stock por producto con movimientos (entrada/salida/ajuste), filtros por tipo y fecha, vista de stock crítico, paginación |
| **Caja** | Apertura y cierre con saldo inicial/final, control de turnos, movimientos de ingresos y egresos, auditoría con indicadores |
| **Clientes** | CRUD completo, historial de ventas asociado |
| **Proveedores** | CRUD completo, historial de compras asociado |
| **Reportes** | Dashboard ejecutivo con gráficos de torta y barras (ventas diarias/semanales), reporte diario de ventas, stock crítico, exportación a Excel |
| **Seguridad** | Autenticación por email con BCrypt, dos roles (Admin / Vendedor), sesión por empresa |
| **Configuración** | Datos de la empresa, métodos de pago (categoría y cuenta contable), backup y restauración de base de datos, perfil de usuario, cambio de contraseña |

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
