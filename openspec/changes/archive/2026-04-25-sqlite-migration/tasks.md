# Tasks: SQLite Migration - Persistencia

## Phase 1: Entity Configuration Fixes

- [x] 1.1 Remove `.HasFilter("[CodigoBarra] IS NOT NULL")` from Producto.CodigoBarra index in `GestionComercial.Persistencia/Contexto/GestionComercialContext.cs`
- [x] 1.2 Verify index configuration compiles: `dotnet build GestionComercial.Persistencia`

## Phase 2: UI Project Cleanup

- [x] 2.1 Remove `Microsoft.EntityFrameworkCore.SqlServer` package reference from `GestionComercial.UI/GestionComercial.UI.csproj`
- [x] 2.2 Verify no other SQL Server references remain in UI project

## Phase 3: Build Verification

- [x] 3.1 Run `dotnet build` on solution and verify all projects compile without errors
- [x] 3.2 Verify no SQL Server warnings or errors in build output

## Phase 4: Runtime Verification

- [x] 4.1 Launch `GestionComercial.UI` application
- [x] 4.2 Verify SQLite database is created successfully on first run
- [x] 4.3 Verify application starts without database connection errors

(End of file - total 22 lines)