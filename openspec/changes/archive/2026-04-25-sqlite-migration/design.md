# Design: SQLite Migration - Persistencia

## Technical Approach

The change migrates the database provider from SQL Server to SQLite, leveraging the existing EF Core infrastructure. The project already has SQLite provider configured in `GestionComercialContextFactory` and a SQLite-compatible initial migration. The focus is on removing the SQL Server reference from the UI project and fixing SQLite-incompatible index configurations.

## Architecture Decisions

### Decision: Database Provider Configuration

**Choice**: Retain existing `GestionComercialContextFactory` as the design-time DbContext factory using SQLite
**Alternatives considered**: Creating a separate factory for design-time vs runtime
**Rationale**: The existing factory already correctly loads the SQLite connection string from appsettings.json and uses `.UseSqlite()`. No changes needed.

### Decision: Filtered Index Handling

**Choice**: Remove `.HasFilter()` from `Producto.CodigoBarra` index and enforce uniqueness in application layer
**Alternatives considered**: Keep HasFilter (not supported by SQLite), Use trigger-based uniqueness
**Rationale**: SQLite does not support filtered indexes. The current Fluent API configuration `HasFilter("[CodigoBarra] IS NOT NULL")` will be ignored or cause runtime errors. The application must validate `CodigoBarra` uniqueness before insert.

### Decision: SQL Server Package Removal

**Choice**: Remove `Microsoft.EntityFrameworkCore.SqlServer` from UI project only
**Alternatives considered**: Keep in both projects
**Rationale**: Spec explicitly states UI project SHALL NOT reference SqlServer. Persistence project already uses SQLite-only packages.

### Decision: Connection String Format

**Choice**: Retain current format `"Data Source=GestionComercial.db"` in appsettings.json
**Alternatives considered**: Adding full path or WAL mode parameters
**Rationale**: Current format works with SQLite provider. WAL mode can be added at connection string level if performance issues arise.

## Data Flow

```
appsettings.json (ConnectionStrings:DefaultConnection)
    │
    ▼
GestionComercialContextFactory.CreateDbContext()
    │
    ▼
DbContextOptionsBuilder.UseSqlite(connectionString)
    │
    ▼
GestionComercialContext → EF Core Migrations → SQLite Database
```

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `GestionComercial.UI/GestionComercial.UI.csproj` | Modify | Remove `Microsoft.EntityFrameworkCore.SqlServer` package reference |
| `GestionComercial.Persistencia/Contexto/GestionComercialContext.cs` | Modify | Remove `.HasFilter()` from Producto.CodigoBarra unique index |
| `GestionComercial.UI/appsettings.json` | Verify | Ensure `Data Source=` format (already correct) |

## Interfaces / Contracts

### Modified: Producto Index Configuration

Before (SQL Server):
```csharp
modelBuilder.Entity<Producto>()
    .HasIndex(p => new { p.CodigoBarra, p.Id_empresa })
    .HasDatabaseName("IX_Producto_CodigoBarra_Empresa")
    .IsUnique()
    .HasFilter("[CodigoBarra] IS NOT NULL");
```

After (SQLite):
```csharp
modelBuilder.Entity<Producto>()
    .HasIndex(p => new { p.CodigoBarra, p.Id_empresa })
    .HasDatabaseName("IX_Producto_CodigoBarra_Empresa")
    .IsUnique(); // No HasFilter() - SQLite doesn't support it
```

### Unchanged: Connection String Format

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=GestionComercial.db"
  }
}
```

## Testing Strategy

| Layer | What to Test | Approach |
|-------|--------------|----------|
| Unit | Index configuration builds correctly | `dotnet build` - verify no warnings |
| Integration | CRUD on Producto with duplicate CodigoBarra | Application-level validation tests |
| Manual | Fresh database creation | Run app, verify tables created |

## Migration / Rollout

**No database migration required.** Existing migrations already target SQLite:
- `20260424130312_InitialCreate.cs` uses INTEGER, TEXT, decimal(18,X) types
- No data migration needed: spec explicitly excludes existing SQL Server data migration
- Deploy new `.db` file or let `EnsureCreated()` create on first run

## Open Questions

- [x] Does the existing migration work with fresh SQLite database? Yes - types are compatible.
- [x] How to handle CodigoBarra uniqueness? Application layer must validate.
- [ ] Should WAL mode be added to connection string? Defer to performance testing phase.