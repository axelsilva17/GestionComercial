# SPEC: Módulo Caja - Flujo Completo

## 1. Overview

Este documento define el flujo completo del módulo de caja: apertura, operación y cierre.

## 2. Apertura de Caja

### 2.1 Flujo
1. El sistema busca la **última caja cerrada** de la sucursal actual
2. Si existe, muestra su `MontoFinal` como **"Saldo anterior"**
3. El usuario puede:
   - Aceptar el saldo anterior (default)
   - Ingresar un monto inicial diferente

### 2.2 Campos en UI
- **Saldo anterior**: Mostrar (si existe) el efectivo que quedó de la jornada anterior
- **Monto inicial**: Campo editable (opcional). Si está vacío, usar Saldo anterior
- **Caja**: ComboBox para seleccionar caja
- **Turno**: ComboBox (General/Mañana/Tarde/Noche)

### 2.3 Al abrir
```csharp
// Cargar último cierre
var ultimaCaja = historial.Where(c => !c.EstaAbierta)
                          .OrderByDescending(c => c.FechaApertura)
                          .FirstOrDefault();

SaldoAnterior = ultimaCaja?.MontoFinal ?? 0;

// Al confirmar apertura
var montoInicial = string.IsNullOrWhiteSpace(MontoInicial) ? SaldoAnterior : decimal.Parse(MontoInicial);
await _cajaServicio.AbrirCajaAsync(idSucursal, idUsuario, montoInicial);
```

## 3. Operación (CajaView)

### 3.1 Display de valores
- **Monto Inicial**: Lo que se abrió la caja (3000 en el ejemplo)
- **Ingresos**: Total de ventas en efectivo (sin contar el vuelto)
- **Egresos**: Total de gastos + Total de vuelto dado
- **Saldo Actual**: `MontoInicial + Ingresos - Egresos`

### 3.2 Cálculo correcto
```
SaldoActual = MontoInicial + TotalIngresos - TotalEgresos
```

Donde:
- **Ingresos** = Σ(movimientos donde Tipo=Ingreso Y no es Apertura Y no es Cierre)
- **Egresos** = Σ(movimientos donde Tipo=Egreso Y no es Apertura Y no es Cierre)

### 3.3 Caso de ejemplo
- Saldo inicial: 3000
- Venta: 4800 (cliente paga exactо)
  - Ingreso: 4800
  - Egreso: 0
  - Saldo: 3000 + 4800 - 0 = **7800** ✓

- Salda inicial: 3000
- Venta: 4800 (cliente paga 5000)
  - Ingreso: 4800 (recibido - vuelto = 5000 - 200)
  - Egreso: 200 (vuelto dado)
  - Saldo: 3000 + 4800 - 200 = **7600** ✓

### 3.4 En UI (CajaView.xaml)
```
┌─────────────────────────────────────┐
│ Ingresos: $4.800                    │
│ Egresos: $0                         │
│ Saldo: $7.800                       │
└─────────────────────────────────────┘
```

## 4. Cierre de Caja

### 4.1 Datos a mostrar
- **Saldo esperado**: Calculado por el sistema
  ```
  SaldoEsperado = MontoInicial + VentasEfectivo + IngresosManuales - EgresosManuales - VueltoDado
  ```
- **Saldo real**: Lo que el usuario cuenta en la caja
- **Diferencia**: SaldoReal - SaldoEsperado

### 4.2 Campos en UI
- MontoInicial (del apertura)
- VentasEfectivo (total ventas en efectivo del turno)
- IngresosEfectivo (gastos registrados como ingreso)
- EgresosEfectivo (gastos registrados + vuelto dado)
- **SaldoEsperado** (calculado)
- **SaldoReal** (input del usuario)
- **Diferencia** (calculado)

### 4.3 Al cerrar
```csharp
await _cajaServicio.CerrarCajaAsync(idCaja, idUsuario, montoReal);
// El montoReal se guarda como MontoFinal de la caja
// La próxima apertura usará este valor como SaldoAnterior
```

## 5. Detalle: Registro de Venta en Efectivo

### 5.1 Flujo en VentaServicio
1. Cliente compra $4800, paga en efectivo
2. Registrar Pago:
   - Si hay vuelto: registrar **Ingreso** por (recibido - vuelto) Y **Egreso** por el vuelto
   - Si no hay vuelto: registrar **Ingreso** por el monto total

### 5.2 Código
```csharp
if (pago.EsEfectivo)
{
    // Ingreso: lo que queda en caja (sin el vuelto)
    var montoNeto = pago.Monto - pago.Vuelto;
    var movimientoIngreso = new TipoMovimientoCaja
    {
        Tipo = TipoMovimientoCajaEnum.Ingreso,
        Monto = montoNeto,
        Concepto = $"Venta #{venta.Id}",
        // ...
    };
    
    // Egreso: el vuelto dado (si hay)
    if (pago.Vuelto > 0)
    {
        var movimientoVuelto = new TipoMovimientoCaja
        {
            Tipo = TipoMovimientoCajaEnum.Egreso,
            Monto = pago.Vuelto,
            Concepto = $"Vuelto venta #{venta.Id}",
            // ...
        };
    }
}
```

## 6. Entidades

### Caja
| Campo | Tipo | Descripción |
|-------|------|-------------|
| Id | int | PK |
| FechaApertura | DateTime | Momento de apertura |
| FechaCierre | DateTime? | Momento de cierre |
| MontoInicial | decimal | Efectivo al abrir |
| MontoFinal | decimal? | Efectivo al cerrar |
| Estado | int | 1=Abierta, 2=Cerrada |
| Id_sucursal | int | FK |
| Turno | string | General/Mañana/Tarde/Noche |

### MovimientoCaja
| Campo | Tipo | Descripción |
|-------|------|-------------|
| Id | int | PK |
| Id_caja | int | FK |
| Tipo | int | 1=Apertura, 2=Ingreso, 3=Egreso, 4=Cierre |
| Monto | decimal | Monto del movimiento |
| Concepto | string | Descripción |
| Fecha | DateTime | |
| Id_usuario | int | |

## 7. Casos de Prueba

### CP1: Venta en efectivo exactо
- Apertura: 3000
- Venta: 4800 (paga exactо)
- **Esperado**: Ingresos=4800, Egresos=0, Saldo=7800

### CP2: Venta con vuelto
- Apertura: 3000
- Venta: 4800 (cliente paga 5000)
- **Esperado**: Ingresos=4800, Egresos=200, Saldo=7600

### CP3: Múltiples ventas
- Apertura: 3000
- Venta1: 1000 (paga 1000)
- Venta2: 2500 (paga 3000, vuelto 500)
- Venta3: 800 (paga 800)
- **Esperado**: Ingresos=4300, Egresos=500, Saldo=6800

### CP4: Cierre y próxima apertura
- Caja1 cierra con MontoFinal=7800
- Apertura siguiente debe mostrar SaldoAnterior=7800

## 8. Vistas Afectadas

1. **AperturaCajaView.xaml** + ViewModel
   - Mostrar SaldoAnterior correctamente
   - Labels de selección arriba del ComboBox

2. **CajaView.xaml** + ViewModel
   - Calcular Ingresos sin incluir Apertura/Cierre
   - Calcular Egresos sin incluir Apertura/Cierre
   - Mostrar saldo correcto

3. **CierreCajaView.xaml** + ViewModel
   - Calcular SaldoEsperado correctamente
   - No mostrar botón Exportar
   - Simplificar datos

## 9. Validaciones Requeridas

- [ ] Apertura muestra SaldoAnterior del último cierre
- [ ] Apertura permite modificar monto inicial
- [ ] Caja muestra Ingresos = ventas efectivo (sin vuelto)
- [ ] Caja muestra Egresos = gastos + vuelto
- [ ] Caja calcula Saldo = Inicial + Ingresos - Egresos
- [ ] Cierre calcula SaldoEsperado correctamente
- [ ] Cierre guarda monto real como MontoFinal
- [ ] Próxima apertura hereda el MontoFinal como SaldoAnterior