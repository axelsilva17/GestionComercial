# GestionComercial

Sistema de gestión integral para comercios locales — ferreterías, locales de ropa, pinturerías y negocios similares.

Soporta múltiples empresas y sucursales.

---

## Funcionalidades

- Gestión de productos con categorías, código de barras y precios
- Ventas con múltiples métodos de pago
- Control de stock e importación desde Excel
- Gestión de compras y proveedores
- Apertura y cierre de caja
- Clientes e historial de ventas
- Reportes exportables a Excel
- Dashboard según rol del usuario
- Gestión de usuarios con roles y permisos

---

## Tecnologías

- C# / .NET 8 — WPF
- SQL Server + Entity Framework Core
- Caliburn.Micro (MVVM)
- FluentValidation
- BCrypt.Net
- ClosedXML

---

## Requisitos

- .NET 8 SDK
- SQL Server
- Visual Studio 2022

---

## Configuración

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar migraciones: `Update-Database -Project GestionComercial.Persistencia`
4. Credenciales iniciales: `admin@sistema.com` / `Admin1234`

---

## Arquitectura

Clean Architecture — 4 capas: Dominio, Aplicación, Persistencia, Infraestructura y UI.

Patrones: Repository, Unit of Work, MVVM, Inyección de dependencias.

Principios SOLID aplicados en toda la solución.
