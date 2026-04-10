# SPEC: Gestión de Precios de Productos

## 1. Overview

Permitir al usuario gestionar precios de venta de productos de forma flexible: ingreso manual o mediante porcentuales, con opciones rápidas predefinidas.

## 2. Opciones de Precio de Venta

### 2.1 Campo de precio manual
- Input numérico para ingresar precio de venta directamente
- Formato con separador de miles y decimales
- Validación: mayor a 0

### 2.2 Campo de porcentaje de ganancia manual
- Input numérico para ingresar margen de ganancia en %
- El sistema calcula: `PrecioVenta = PrecioCosto * (1 + Margen/100)`
- Ejemplo: costo $100, margen 20% → venta $120

### 2.3 Botones de porcentaje rápido
| Botón | Acción |
|-------|--------|
| +5% | Agrega 5% al margen actual |
| +10% | Agrega 10% al margen actual |
| +15% | Agrega 15% al margen actual |
| +20% | Agrega 20% al margen actual |
| +30% | Agrega 30% al margen actual |
| +50% | Agrega 50% al margen actual |

### 2.4 Reglas de negocio
- Si el usuario modifica el precio manual, se desactiva el cálculo por porcentaje
- Si el usuario modifica el porcentaje, se recalcula el precio automáticamente
- Guardar última opción usada por producto para próxima edición

## 3. UI - ProductoFormularioView

```
┌─────────────────────────────────────────────────────────────┐
│ PRECIO Y GANANCIA                                           │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  Precio Costo:    [$__________]  (ya existe)              │
│                                                             │
│  ────────────────────── O ──────────────────────            │
│                                                             │
│  Precio Venta:    [$__________]  (manual)                   │
│                                                             │
│  Ó porcentaje:    [_____%] de ganancia                    │
│                                                             │
│  [+5%] [+10%] [+15%] [+20%] [+30%] [+50%]                  │
│                                                             │
│  Margen actual: 20% ($120 de $100)                         │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## 4. Implementación - ViewModel

```csharp
// En ProductoFormularioViewModel
private decimal _precioVentaManual;
public decimal PrecioVentaManual
{
    get => _precioVentaManual;
    set
    {
        _precioVentaManual = value;
        _usaPrecioManual = true;
        NotifyOfPropertyChange(() => PrecioVentaManual);
        NotifyOfPropertyChange(() => MargenCalculado);
    }
}

private decimal _margenPorcentaje;
public decimal MargenPorcentaje
{
    get => _margenPorcentaje;
    set
    {
        _margenPorcentaje = value;
        _usaPrecioManual = false;
        RecalcularPrecioVenta();
    }
}

private bool _usaPrecioManual = true;

public void AplicarPorcentajeRapido(decimal porcentaje)
{
    MargenPorcentaje += porcentaje;
}

private void RecalcularPrecioVenta()
{
    if (!_usaPrecioManual && PrecioCostoActual > 0)
    {
        PrecioVentaManual = PrecioCostoActual * (1 + MargenPorcentaje / 100);
    }
}

public decimal MargenCalculado => PrecioCostoActual > 0 
    ? ((PrecioVentaManual - PrecioCostoActual) / PrecioCostoActual * 100) 
    : 0;
```

## 5. Casos de Uso

| Caso | Acción |
|------|--------|
| CP1: Precio manual | Usuario ingresa $150 → se guarda precio 150, margen se calcula solo |
| CP2: Porcentaje manual | Usuario ingresa 25% sobre costo $100 → precio = $125 |
| CP3: Botón rápido | Clic en +15% → margen pasa de 20% a 35% → precio se recalcula |
| CP4: Cambiar a manual | Al editar precio manualmente, se desactiva modo porcentaje |

---

## 6. Validaciones

- [ ] Campo precio venta acepta solo números positivos
- [ ] Campo porcentaje acepta 0-500% (margen razonable)
- [ ] Botones de porcentaje actualizan UI inmediatamente
- [ ] Al guardar, persiste el último método usado
- [ ] Si precio venta < precio costo, mostrar warning (margen negativo)