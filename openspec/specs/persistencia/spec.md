# Spec: SQLite Migration - Persistencia

**Project**: GestionComercial
**Change scope**: Persistencia layer — SQL Server → SQLite migration
**Target layer**: GestionComercial.Persistencia
**Status**: Active

---

## Domain
Persistence / Data Access Layer

## Motivation
Eliminate SQL Server dependency for a desktop application targeting small businesses. SQLite provides:
- Zero-configuration deployment
- Single-file database (portable)
- No server installation required
- Ideal for single-instance desktop apps (POS/ventas)

## Scope
### In scope
- Replace `Microsoft.EntityFrameworkCore.SqlServer` provider with `Microsoft.EntityFrameworkCore.Sqlite`
- Update `DbContext` configuration for SQLite compatibility
- Fix type mappings that differ between providers (decimal, datetime)
- Adjust filtered indexes for SQLite compatibility
- Update connection string format
- Remove SQL Server-specific features (e.g., `HasFilter` on indexes)
- Verify all entity configurations work with SQLite
- Validate existing migrations or create new ones

### Out of scope
- Changes to domain entities (interfaces, repositories, DTOs)
- Changes to application services
- UI layer modifications
- Data migration scripts (existing SQL Server data)

## Requirements (RFC 2119)

### Database Provider
- The persistence layer SHALL use `Microsoft.EntityFrameworkCore.Sqlite` as the only database provider
- The UI project SHALL NOT reference `Microsoft.EntityFrameworkCore.SqlServer` after migration
- Connection string SHALL use `Data Source` format for SQLite

### Type Mappings
- Decimal columns SHALL use `decimal(18,3)` for stock quantities and `decimal(18,2)` for currency
- SQLite stores decimals as TEXT by default; EF Core SQLite provider handles precision correctly
- DateTime values SHALL be stored as ISO 8601 strings; no timezone conversion issues expected in single-instance desktop app

### Indexes
- Filtered indexes (SQL Server `WHERE` clause) SHALL be removed from SQLite configurations
- Unique indexes with nullable columns SHALL use `IsUnique(false)` and manual validation in application layer
- Composite indexes SHALL remain functional

### Cascade Deletes
- SQLite enforces foreign keys by default when enabled
- All existing `DeleteBehavior` configurations SHALL remain consistent
- Ensure `ON DELETE CASCADE` relationships work correctly

### Migrations
- New migration SHALL be generated targeting SQLite
- Existing SQL Server migrations SHALL NOT be used
- Initial migration SHALL create all tables matching current schema

### Seeds
- All seed data (roles, permisos, métodos de pago, categorías, etc.) SHALL continue working
- Seeds are provider-agnostic and require no changes

## Functional Scenarios

### Happy Path: Clean Build
- **Given** a fresh clone of the repository
- **When** `dotnet build` is executed on the solution
- **Then** all projects build successfully without SQL Server references
- **And** `dotnet test` passes for existing unit tests

### Happy Path: Fresh Database
- **Given** no existing database file
- **When** the application starts
- **And** EF Core migration is applied (`database.EnsureCreated()` or `migrate()`)
- **Then** all tables are created with correct column types
- **And** seed data is inserted

### Edge: Existing SQL Server Data
- **Given** an existing SQL Server database with data
- **When** migrating to SQLite
- **Then** a data export/import process is documented separately
- **And** the migration spec acknowledges this as out of scope

### Edge: Filtered Index Compatibility
- **Given** `Producto.CodigoBarra` unique index with filter
- **When** running on SQLite
- **Then** the index is created as non-filtered unique index
- **And** application validates uniqueness before insert

## Data Models Impacted

### Entities (no changes, only provider mapping adjustments)
- Empresa, Sucursal, Rol, Permiso, RolPermiso
- Usuario, Producto, Categoria, UnidadMedida
- Cliente, Proveedor, MetodoPago
- Venta, VentaDetalle, VentaDetalleDescuento, VentaDetalleImpuesto
- Pago
- Compra, CompraDetalle
- MovimientoStock, TipoMovimientoStock
- Caja, TipoMovimientoCaja
- AuditoriaLog, TablaAuditada

### Configuration Changes Required
| Entity | Current (SQL Server) | SQLite Target |
|--------|---------------------|---------------|
| Producto.CodigoBarra index | `HasFilter("[CodigoBarra] IS NOT NULL")` | Remove filter, add app-level validation |
| All decimal(18,X) | Supported natively | Supported (SQLite stores as TEXT) |
| DateTime | SQL Server datetime2 | SQLite ISO 8601 text |

## Non-Functional Considerations

### Performance
- SQLite is optimized for single-writer scenarios (POS use case)
- WAL mode (Write-Ahead Logging) SHOULD be enabled for better concurrency
- Connection string SHOULD include `Journal Mode=WAL`

### Deployment
- Single `.db` file in user data folder
- No installer prerequisites for database
- Portable across machines (copy `.db` file)

### Reliability
- SQLite has no transaction log; backup is file copy
- Consider `PRAGMA integrity_check` on startup

## Integration Points

### UI Layer
- `appsettings.json` connection string changes from `Server=...` to `Data Source=...`
- Remove `Microsoft.EntityFrameworkCore.SqlServer` from `GestionComercial.UI.csproj`
- Ensure `GestionComercial.Persistencia.csproj` is the only EF Core provider reference

### Application Layer
- No changes expected (uses repository interfaces)
- Unit tests should continue passing

### Infrastructure Layer
- Check for any SQL Server specific implementations

## Testing Strategy

### Unit Tests
- Existing xUnit tests in `GestionComercial.Tests` should pass
- Add tests for SQLite-specific behaviors if needed

### Integration Tests
- Test DbContext creation with SQLite provider
- Test migrations apply correctly
- Test seed data insertion

### Manual Verification
- Start application, verify database creation
- Perform CRUD operations on core entities (Producto, Venta, Caja)
- Verify reports still work with new data source

## Rollback Plan
- Revert `Microsoft.EntityFrameworkCore.Sqlite` → `Microsoft.EntityFrameworkCore.SqlServer`
- Restore connection strings
- Discard SQLite migrations (keep SQL Server ones)
- **Risk**: Low — changes are isolated to persistence configuration

## Acceptance Criteria
- [x] Solution builds without errors
- [x] No `Microsoft.EntityFrameworkCore.SqlServer` references remain
- [x] All tests pass (`dotnet test`)
- [x] Application creates SQLite database on first run
- [x] All CRUD operations work correctly
- [x] Seed data loads successfully
- [x] Reports function with SQLite data source