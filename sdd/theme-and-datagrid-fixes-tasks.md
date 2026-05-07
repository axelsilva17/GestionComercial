# Tasks: theme-and-datagrid-fixes

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | ~15 |
| 400-line budget risk | Low |
| Chained PRs recommended | No |
| Suggested split | Single PR |

Decision needed before apply: No
Chained PRs recommended: No
Chain strategy: pending
400-line budget risk: Low

### Suggested Work Units

| Unit | Goal | Likely PR | Notes |
|------|------|-----------|-------|
| 1 | Fix theme resources and DataGrid references | PR 1 | All fixes in single PR |

## Phase 1: Theme Resources Fixes

- [ ] 1.1 Agregar SemanticColors.xaml a App.xaml MergedDictionaries después de Colors.xaml
- [ ] 1.2 Agregar PrimaryAlphaBrush (#1A38BDF8) a ThemeLight.xaml
- [ ] 1.3 Agregar PrimaryAlphaBrush (#1A0EA5E9) a ThemeDark.xaml

## Phase 2: HistorialControl Fixes

- [ ] 2.1 Quitar referencia a DataGridHistorial en línea 164 de HistorialControl.xaml
- [ ] 2.2 Quitar referencia a HeaderHistorial en línea 174 de HistorialControl.xaml
- [ ] 2.3 Cambiar brushes locales de StaticResource a DynamicResource en líneas 12-18

## Phase 3: Verification

- [ ] 3.1 Verificar que PrimaryAlphaBrush esté disponible como DynamicResource en toda la app
- [ ] 3.2 Verificar que DataGrid en HistorialControluse estilos implícitos de DataGridStyles.xaml