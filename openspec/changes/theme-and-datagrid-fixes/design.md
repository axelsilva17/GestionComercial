# Design: theme-and-datagrid-fixes

## Technical Approach

Se corrigen dos problemas interrelacionados: (1) `SemanticColors.xaml` no está cargado en App.xaml, causando que `PrimaryAlphaBrush` no esté disponible globalmente - esto afecta a `DataGridStyles.xaml` que lo usa, y (2) `HistorialControl.xaml` referencia estilos inexistentes y tiene brushes hardcodeados que no responden al cambio de tema. El enfoque es agregar el diccionario faltante, añadir el brush faltante en ambos temas (valor alpha del PrimaryColor), y convertir los brushes locales de HistorialControl a DynamicResource para theme dinámico.

## Architecture Decisions

### Decision: Cargar SemanticColors.xaml después de Colors.xaml en App.xaml

**Choice**: Ubicar `SemanticColors.xaml` inmediatamente después de `Colors.xaml` en el orden de MergedDictionaries.

**Alternatives considered**: Cargar al final de todos los diccionarios | Cargar antes de Colors.xaml

**Rationale**: SemanticColors.xaml contiene brushes que dependen del tema (PrimaryAlphaBrush se basará en los PrimaryColor de ThemeLight/ThemeDark). Al cargarlo después de Colors.xaml pero antes de DataGridStyles.xaml, se asegura que los colores semánticos estén disponibles cuando DataGridStyles los requiera (ver líneas 75, 95 de DataGridStyles.xaml).

### Decision: PrimaryAlphaBrush usa versión alpha del PrimaryColor del tema

**Choice**: ThemeLight: `#1A38BDF8` (PrimaryColor #38BDF8 con 10% alpha) | ThemeDark: `#1A0EA5E9` (PrimaryHoverColor #0EA5E9 con 10% alpha - más visible en fondo oscuro).

**Alternatives considered**: Usar PrimaryColor directo en ambos temas | Usar valor fijo #1A38BDF8 en ambos temas

**Rationale**: En modo oscuro, PrimaryColor #38BDF8 es muy claro sobre fondo #0F1419. Usar PrimaryHoverColor (más oscuro) con alpha da mejor contraste visual. El formato #1A = 10% alpha (~26/255 ≈ 10%) es consistente con SemanticColors.xaml existente.

### Decision: Eliminar referencias a DataGridHistorial y HeaderHistorial inexistentes

**Choice**: Eliminar `Style="{StaticResource DataGridHistorial}"` (línea 164) y `ColumnHeaderStyle="{StaticResource HeaderHistorial}"` (línea 174) del DataGrid.

**Alternatives considered**: Crear los estilos DataGridHistorial y HeaderHistorial vacíos | Usar estilos implícitos de DataGridStyles.xaml directamente

**Rationale**: El archivo DataGridStyles.xaml ya define estilos implícitos (TargetType="DataGrid", "DataGridColumnHeader") que se aplican automáticamente. Al no especificar Style explícito, estos se usarán-por-defecto. Los estilos locales `FilaHistorial` y `CeldaHistorial` ya están definidos en líneas 20-61 y se aplican vía RowStyle/CellStyle explícitos.

### Decision: Brushes locales de HistorialControl usan DynamicResource

**Choice**: Cambiar todos los `StaticResource` de brushes locales a `DynamicResource` en HistorialControl.xaml.

**Alternatives considered**: Mantener StaticResource y aceptar que no responden al theme | Definir los brushes en ThemeLight/ThemeDark

**Rationale**: Los brushes locales (BgOscuro, CardOscuro, etc.) están hardcodeados con colores de modo oscuro (#0F1419, #1A2332). Convertirlos a DynamicResource permite que:
1. Puedan referenciar brushes del tema como `CardBrush`, `TextPrimaryBrush`
2. El control responda al cambio light → dark dinámicamente
3. Se elimine la duplicación de valores hardcodeados

## Data Flow

```
App.xaml (ThemeLight/ThemeDark cargados primero)
    │
    ├── ThemeLight.xaml ──→ PrimaryColor: #38BDF8
    │              └── PrimaryAlphaBrush: #1A38BDF8
    │
    ├── ThemeDark.xaml ──→ PrimaryColor: #38BDF8
    │               └── PrimaryAlphaBrush: #1A0EA5E9
    │
    ├── Colors.xaml ──→ estilos que usan DynamicResource
    │
    └── SemanticColors.xaml (AGREGAR) ──→ PrimaryAlphaBrush
           │
           └── DataGridStyles.xaml ────→ usa PrimaryAlphaBrush

HistorialControl.xaml
    ├── Brushes locales → DynamicResource → Cards/Text brushes del tema
    ├── FilaHistorial → DynamicResource → TextPrimaryBrush
    ├── CeldaHistorial → DynamicResource → TextPrimaryBrush
    └── DataGrid → sin Style explícito → usa implícitos de DataGridStyles.xaml
```

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `GestionComercial.UI/App.xaml` | Modify | Agregar SemanticColors.xaml a MergedDictionaries después de Colors.xaml |
| `GestionComercial.UI/Views/Recursos/Estilos/ThemeLight.xaml` | Modify | Agregar PrimaryAlphaBrush con #1A38BDF8 |
| `GestionComercial.UI/Views/Recursos/Estilos/ThemeDark.xaml` | Modify | Agregar PrimaryAlphaBrush con #1A0EA5E9 |
| `GestionComercial.UI/Views/Controls/HistorialControl.xaml` | Modify | Quitar DataGridHistorial/HeaderHistorial, cambiar brushes locales a DynamicResource |

## Interfaces / Contracts

No hay cambios de interfaces públicas. Todos los cambios son internos a recursos XAML.

### Brushes a definir

**ThemeLight.xaml** (agregar al final de brushes):
```xml
<SolidColorBrush x:Key="PrimaryAlphaBrush" Color="#1A38BDF8"/>
```

**ThemeDark.xaml** (agregar al final de brushes):
```xml
<SolidColorBrush x:Key="PrimaryAlphaBrush" Color="#1A0EA5E9"/>
```

### App.xaml (cambio en MergedDictionaries):
```xml
<ResourceDictionary Source="/Views/Recursos/Diccionarios/Colors.xaml"/>
<ResourceDictionary Source="/Views/Recursos/Diccionarios/SemanticColors.xaml"/>  <!-- AGREGAR -->
```

### HistorialControl.xaml (cambios):
- Eliminar línea 164: `Style="{StaticResource DataGridHistorial}"`
- Eliminar línea 174: `ColumnHeaderStyle="{StaticResource HeaderHistorial}"`
- Cambiar brushes locales (líneas 12-17) para usar DynamicResource:
  - `BgOscuro` → `{DynamicResource BackgroundBrush}`
  - `CardOscuro` → `{DynamicResource CardBrush}`
  - `TextoClaro` → `{DynamicResource TextPrimaryBrush}`
  - `BordeOscuro` → `{DynamicResource BorderBrush}`
  - `HoverOscuro` → `{DynamicResource HoverBrush}`
  - `AzulOscuro` → `{DynamicResource PrimaryBrush}`
- Cambiar referencias en FilaHistorial y CeldaHistorial de StaticResource a DynamicResource

## Testing Strategy

| Layer | What to Test | Approach |
|-------|-------------|----------|
| Build | Compilación sin errores de referencia faltante | dotnet build después de cambios |
| Theme | PrimaryAlphaBrush disponible en ambos temas | Verificar que DataGrid con selección muestre color alpha |
| Dinámico | Cambio de tema actualiza HistorialControl | Cambiar tema en runtime y verificar colores |
| Manual | HistorialControl muestra datos correctamente | Abrir historial de ventas en cada tema |

**Nota**: No hay tests unitarios automatizados para XAML en este proyecto actualmente.

## Migration / Rollback

No migración requerida - son cambios in-place en archivos existentes.

**Rollback plan**:
1. Revertir App.xaml: eliminar línea de SemanticColors.xaml
2. Revertir ThemeLight.xaml y ThemeDark.xaml: eliminar PrimaryAlphaBrush
3. Revertir HistorialControl.xaml: restaurar StaticResource original y referencias a DataGridHistorial

## Open Questions

- [ ] Los brushes originales (BgOscuro, CardOscuro, etc.) deben eliminarse o stay como fallback para controles que no tienen brushes de tema correspondientes? → **Decision**: Eliminar y reemplezar completamente por DynamicResource a brushes del tema.

- [ ] El header del DataGrid (HeaderHistorial en línea 174) debe tener un estilo explícito o usar el implícito de DataGridStyles.xaml? → **Decision**: Usar implícito - se aplica automáticamente.