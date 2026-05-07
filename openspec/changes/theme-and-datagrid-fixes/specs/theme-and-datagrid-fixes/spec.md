# Delta para ThemeAndDatagridFixes

## ADDED Requirements

### Requirement: SemanticColors.xaml debe cargarse al inicio de la aplicación

El diccionario `SemanticColors.xaml` DEBE estar disponible en el `ResourceDictionary.MergedDictionaries` de `App.xaml` para que los brushes semánticos (como `PrimaryAlphaBrush`) estén accesibles globalmente desde el inicio de la aplicación.

#### Scenario: SemanticColors.xaml se carga después de Colors.xaml

- GIVEN `App.xaml` con `ResourceDictionary.MergedDictionaries` configurado
- WHEN la aplicación inicia
- THEN `SemanticColors.xaml` debe aparecer en el merged dictionaries después de `Colors.xaml`

---

### Requirement: PrimaryAlphaBrush debe estar disponible en ambos temas

El brush `PrimaryAlphaBrush` DEBE estar definido tanto en `ThemeLight.xaml` como en `ThemeDark.xaml` con valores apropiados para modo claro y oscuro respectively.

#### Scenario: PrimaryAlphaBrush presente en ThemeLight.xaml

- GIVEN `ThemeLight.xaml` se ha cargado
- WHEN se referencia `PrimaryAlphaBrush`
- THEN el brush está disponible con color #1A38BDF8

#### Scenario: PrimaryAlphaBrush presente en ThemeDark.xaml

- GIVEN `ThemeDark.xaml` se ha cargado
- WHEN se referencia `PrimaryAlphaBrush`
- THEN el brush está disponible con color #1A0EA5E9

---

## MODIFIED Requirements

### Requirement: HistorialControl no debereferenciar DataGridHistorial inexistente

El control HistorialControl no DEBE usar una referencia a `DataGridHistorial` que no existe en los estilos disponibles. Los estilos locales ya definidos en el mismo archivo deben usarse directamente.

#### Scenario: DataGridHistorial eliminado de HistorialControl

- GIVEN `HistorialControl.xaml` con línea `Style="{StaticResource DataGridHistorial}"`
- WHEN se compila el proyecto
- THEN la referencia a `DataGridHistorial` no debe aparecer en el DataGrid
- AND el DataGrid debe usar los estilos locales `RowStyle` y `CellStyle` directamente

(Previously: DataGrid referenciaba estilo DataGridHistorial inexistente)

---

### Requirement: Brushes locales de HistorialControl deben usar DynamicResource

Los brushes definidos localmente en `HistorialControl.xaml` que hardcodean colores para modo escuro DEBEN usar `DynamicResource` en lugar de `StaticResource` para que respondan al cambio de tema dinámicamente.

#### Scenario: Brushes convertidos a DynamicResource

- GIVEN brushes locales definidos en `UserControl.Resources` como `BgOscuro`, `CardOscuro`, `TextoClaro`
- WHEN se cambia el tema de light a dark
- THEN los colores del control se actualizan automáticamente
- AND los estilos `FilaHistorial` y `CeldaHistorial` usan `DynamicResource` para referencias de brush

(Previously: Brushes usaban StaticResource con colores hardcodeados sin responder al theme)

---

## Verification Scenarios

| Scenario | GIVEN | WHEN | THEN |
|----------|------|------|------|
| SemanticColors carga | App.xaml sin SemanticColors en merged dictionaries | Aplicación inicia | SemanticColors aparece después de Colors.xaml |
| PrimaryAlphaBrush disponible | Tema Light cargado | Se referencia PrimaryAlphaBrush | Brush disponible con color correcto |
| DataGridHistorial eliminado | HistorialControl.xaml compilado | Build del proyecto | Sin error de referencia faltante |
| Theme dinámico | HistorialControl visible | Cambio light → dark | Colores se actualizan |
| App inicia ambos temas | Tema configurado | App inicia con Light | Sin errores de carga |
| App inicia ambos temas | Tema configurado | App inicia con Dark | Sin errores de carga |

---

## Affected Files

| File | Action |
|------|--------|
| `GestionComercial.UI/App.xaml` | Add SemanticColors.xaml a MergedDictionaries |
| `GestionComercial.UI/Views/Recursos/Estilos/ThemeLight.xaml` | Add PrimaryAlphaBrush |
| `GestionComercial.UI/Views/Recursos/Estilos/ThemeDark.xaml` | Add PrimaryAlphaBrush |
| `GestionComercial.UI/Views/Controls/HistorialControl.xaml` | Remove DataGridHistorial, change StaticResource to DynamicResource |

---

## Next Steps

Ready for design (sdd-design). If design already exists, ready for tasks (sdd-tasks).