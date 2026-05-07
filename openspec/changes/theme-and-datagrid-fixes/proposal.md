# Proposal: theme-and-datagrid-fixes

## Intent

Corregir dos problemas críticos en el sistema de temas y DataGrid: (1) el theme light usa un color celeste incorrecto porque `SemanticColors.xaml` no está cargado y falta `PrimaryAlphaBrush`, y (2) el `HistorialControl` intenta referenciar un `DataGrid` que no existe y tiene estilos hardcodeados que no responden al cambio de tema. El objetivo es que los temas se apliquen consistentemente y los controles respondan dinámicamente al cambio de tema.

## Scope

### In Scope
- Cargar `SemanticColors.xaml` en `App.xaml` para que los colores semánticos estén disponibles
- Agregar `PrimaryAlphaBrush` faltante en `ThemeLight.xaml` y `ThemeDark.xaml`
- Corregir referencias a `DataGridHistorial` inexistente en `HistorialControl.xaml`
- Reemplazar brushes hardcodeados por `DynamicResource` para theme dinámico
- Verificar que el theme switching funcione correctamente tras los cambios

### Out of Scope
- Nuevos features de theming
- Reestructuración del sistema de temas más allá de los fixes necesarios
- Cambios en otros controles que no sean `HistorialControl`
- Migración a otro sistema de theming (seguir usando el actual)

## Capabilities

### New Capabilities
- Ninguno — son todos fixes de problemas existentes

### Modified Capabilities
- Ninguno — no hay cambios a nivel de requisitos, solo corrección técnica

## Approach

Se sigue el Approach 1 + 3 recomendado por la exploración:

1. **Cargar SemanticColors.xaml**: Agregar el resource dictionary en `App.xaml` después de `Colors.xaml` para que esté disponible al inicio de la aplicación
2. **Agregar PrimaryAlphaBrush**: Añadir el brush faltante en ambos archivos de tema (`ThemeLight.xaml`, `ThemeDark.xaml`) con valores apropiados para cada tema
3. **Corregir HistorialControl**: Eliminar la referencia a `DataGridHistorial` que no existe y usar los estilos locales directamente
4. **DynamicResource**: Reemplazar todos los brushes hardcodeados por referencias `DynamicResource` para que el cambio de tema se refleje inmediatamente en el control

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `App.xaml` | Modified | Agregar SemanticColors.xaml al final de merged dictionaries |
| `Themes/ThemeLight.xaml` | Modified | Agregar PrimaryAlphaBrush con color celeste correcto |
| `Themes/ThemeDark.xaml` | Modified | Agregar PrimaryAlphaBrush para modo oscuro |
| `UI/Controls/HistorialControl.xaml` | Modified | Corregir referencia a DataGridHistorial y usar DynamicResource |
| `UI/Styles/DataGridStyles.xaml` | Modified | Verificar si hay conflictos de estilos implícitos |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Romper theme existente al agregar recursos | Low | Verificar que los nuevos brushes no conflictúen con existentes |
| Que otros controles dependedan de los valores hardcodeados | Medium | Revisión manual de todos los controles después del cambio |

## Rollback Plan

1. Revertir cambios en `App.xaml` eliminando la línea de `SemanticColors.xaml`
2. Eliminar `PrimaryAlphaBrush` de ambos archivos de tema
3. Revertir cambios en `HistorialControl.xaml` restaurando referencias originales
4. Rebuild y verificar quecompile y que los temas funcionen

## Dependencies

- Ninguna dependencia externa — todos los cambios son internos al proyecto

## Success Criteria

- [ ] SemanticColors.xaml carga correctamente al inicio de la aplicación
- [ ] PrimaryAlphaBrush está disponible en ambos temas y muestra el color correcto
- [ ] HistorialControl compila sin errores de referencia a DataGridHistorial
- [ ] Cambiar de tema (light/dark) actualiza los colores del HistorialControl dinámicamente
- [ ] La aplicación inicia correctamente con ambos temas