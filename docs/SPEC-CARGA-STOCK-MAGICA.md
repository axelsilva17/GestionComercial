# SPEC: Carga de Stock "Mágica" (IA Offline)

## 1. Overview

Sistema de carga de stock mediante lectura de facturas/proveedores de forma offline, sin necesidad de conexión a internet y funcionando en PCs con recursos limitados.

## 2. Filosofía del Sistema

```
┌─────────────────────────────────────────────────────────────────┐
│                    ARQUITECTURA LIGERA                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   FACTURA PDF/IMAGEN                                           │
│        │                                                       │
│        ▼                                                       │
│   ┌─────────────────────────────────────┐                      │
│   │  EXTRACCIÓN DE TEXTO (Tesseract)    │ ← Ligero, sin GPU   │
│   └─────────────────────────────────────┘                      │
│        │                                                       │
│        ▼                                                       │
│   ┌─────────────────────────────────────┐                      │
│   │  PARSEO PATRONES (Regex/Keywords)   │ ← Sin ML/NN         │
│   └─────────────────────────────────────┘                      │
│        │                                                       │
│        ▼                                                       │
│   ┌─────────────────────────────────────┐                      │
│   │  TABLA DE PRE-VISUALIZACIÓN        │ ← Confirmación UI   │
│   └─────────────────────────────────────┘                      │
│        │                                                       │
│        ▼                                                       │
│   ACTUALIZAR STOCK (sí确认)                                  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 3. Requisitos del Sistema

### 3.1 Hardware mínimo (PCs antiguas)
- **Procesador**: Intel Core 2 Duo o equivalente AMD
- **RAM**: 2 GB (ideal 4 GB)
- **Espacio**: 500 MB para Tesseract + datos
- **GPU**: No requerida (procesamiento CPU)
- **SO**: Windows 7+ (x86 o x64)

### 3.2 Dependencias ligeras
| Componente | Peso | Alternativa |
|------------|------|-------------|
| Tesseract 5.x | ~20MB | Windows OCR API (内置) |
| PDFParser | ~5MB | iText7 (más pesado) |
| OCR Data | ~20MB (spa+eng) | Solo español (~10MB) |

## 4. Flujo de Uso

```
┌──────────────────────────────────────────────────────────────┐
│ 1. SELECCIONAR ARCHIVO                                      │
│    └─ Botón "Importar Factura" → FileDialog → PDF/PNG/JPG  │
└──────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌──────────────────────────────────────────────────────────────┐
│ 2. PROCESAR (BackgroundWorker - no freeze UI)               │
│    - Extraer texto                                          │
│    - Detectar patrones de línea (producto/cantidad/precio)  │
│    - Intentar mapear con productos existentes              │
└──────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌──────────────────────────────────────────────────────────────┐
│ 3. MOSTRAR TABLA DE VISUALIZACIÓN                           │
│                                                              │
│  ┌─────┬─────────────────┬────────┬────────┬────────┐       │
│  │ ✓   │ Producto       │ Costo  │ Cant   │ Estado  │       │
│  ├─────┼─────────────────┼────────┼────────┼────────┤       │
│  │ ☑   │ Gaseosa 2L     │ $1500  │ 24     │ Nuevo   │       │
│  │ ☐    │ Agua Mineral  │ $800   │ 12     │ No enc. │       │
│  │ ☑   │ Cuaderno x10   │ $4500  │ 5      │ existente│      │
│  └─────┴─────────────────┴────────┴────────┴────────┘       │
│                                                              │
│  [Seleccionar todo] [Deseleccionar todo] [Importar]         │
└──────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌──────────────────────────────────────────────────────────────┐
│ 4. CONFIRMAR Y ACTUALIZAR                                   │
│    └─ Usuario hace clic en "Confirmar Importación"         │
│    └─ Sistema actualiza stock y precios de costo           │
└──────────────────────────────────────────────────────────────┘
```

## 5. Técnicas de Extracción (Sin AI Pesada)

### 5.1 Extracción de texto
```
Primary: Tesseract OCR (CPU-only, ~50-100ms por página)
Fallback: Windows.Media.Ocr (Windows 10+)
```

### 5.2 Reconocimiento de patrones (Regex)
```csharp
// Patrones comunes en facturas:
var patrones = new[]
{
    // "Gaseosa 2L      $1500     24"
    @"(.+?)\s{2,}(\$?\d+[\d.,]*)\s{2,}(\d+)",

    // "24 x Gaseosa 2L $1500"
    @"(\d+)\s*[xX]\s*(.+?)\s+(\$?\d+[\d.,]*)",

    // "GASEOSA 2L - 1500 - 24"
    @"(.+?)\s+-\s+(\d+)\s+-\s+(\d+)",
};
```

### 5.3 Mapeo con productos existentes
```
1. Normalizar nombre extraído (quitar acentos, lowercase)
2. Buscar en catálogo por coincidence (Levenshtein simple)
3. Si >80% coincidencia → sugerir producto existente
4. Si no coincide → marcar como "nuevo" para crear
```

## 6. Detección de Producto Existente

### 6.1 Algoritmo ligero
```csharp
public int CalcularCoincidencia(string nombre1, string nombre2)
{
    // Normalizar
    var n1 = Normalizar(nombre1); // lowercase, quitar acentos
    var n2 = Normalizar(nombre2);
    
    // Si es exactamente igual → 100%
    if (n1 == n2) return 100;
    
    // Si uno contiene al otro → 80%
    if (n1.Contains(n2) || n2.Contains(n1)) return 80;
    
    // Coincidencia parcial simple
    var palabras1 = n1.Split(' ');
    var palabras2 = n2.Split(' ');
    var coincidentes = palabras1.Intersect(palabras2).Count();
    return (coincidentes * 100) / Math.Max(palabras1.Length, palabras2.Length);
}
```

### 6.2 Thresholds
| Score | Acción |
|-------|--------|
| ≥90% | Auto-match (mostrar checked) |
| 70-89% | Sugerir (mostrar unchecked, highlight) |
| <70% | No encontrado (crear nuevo) |

## 7. UI - Pantalla de Importación

```xml
<UserControl>
    <Grid Margin="20">
        <!-- Header -->
        <StackPanel Orientation="Horizontal" Margin="0,0,0,16">
            <TextBlock Text="Carga de Stock" FontSize="24" FontWeight="Bold"/>
            <Button Content="Importar Factura" Margin="20,0,0,0"
                    Style="{StaticResource PrimaryButton}"/>
        </StackPanel>

        <!-- Subtítulo instructivo -->
        <TextBlock Text="Seleccione un archivo PDF o imagen de factura del proveedor"
                   Foreground="Gray" Margin="0,0,0,16"/>

        <!-- Tabla de resultados -->
        <DataGrid ItemsSource="{Binding ItemsImportacion}"
                  AutoGenerateColumns="False"
                  CanUserAddRows="False">
            <DataGrid.Columns>
                <DataGridCheckBoxColumn Header="" Binding="{Binding Seleccionado}"/>
                <DataGridTextColumn Header="Producto" Binding="{Binding NombreDetectado}"/>
                <DataGridTextColumn Header="Precio Costo" Binding="{Binding PrecioCosto}"/>
                <DataGridTextColumn Header="Cantidad" Binding="{Binding Cantidad}"/>
                <DataGridComboBoxColumn Header="Producto Sistema" 
                                        ItemsSource="{Binding ProductosSugeridos}"
                                        SelectedItem="{Binding ProductoMapeado}"/>
                <DataGridTextColumn Header="Estado" Binding="{Binding EstadoMatch}"/>
            </DataGrid.Columns>
        </DataGrid>

        <!-- Resumen -->
        <Border Background="LightGray" Padding="10" Margin="0,16,0,0">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="Total items: "/>
                <TextBlock Text="{Binding TotalItems}" FontWeight="Bold"/>
                <TextBlock Text=" | Nuevos: " Margin="20,0,0,0"/>
                <TextBlock Text="{Binding ItemsNuevos}" FontWeight="Bold" Foreground="Green"/>
                <TextBlock Text=" | Sin match: " Margin="20,0,0,0"/>
                <TextBlock Text="{Binding ItemsSinMatch}" FontWeight="Bold" Foreground="Orange"/>
            </StackPanel>
        </Border>

        <!-- Botones acción -->
        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,16,0,0">
            <Button Content="Seleccionar Todos"/>
            <Button Content="Limpiar" Margin="10,0"/>
            <Button Content="Confirmar Importación" 
                    Style="{StaticResource PrimaryButton}"/>
        </StackPanel>
    </Grid>
</UserControl>
```

## 8. Manejo de Errores

| Escenario | Solución |
|-----------|----------|
| Archivo corrupto | Mostrar mensaje "No se pudo leer el archivo" |
| Ningún producto detectado | Mostrar "No se detectaron productos en el documento" |
| OCR muy lento | Mostrar progress bar, permitir cancelar |
| Memoria baja | Procesar en chunks de 10 líneas |

## 9. Logging y Debug

```csharp
LogHelper.Log($"[StockImport] Archivo: {fileName}");
LogHelper.Log($"[StockImport] Texto extraído: {texto.Length} caracteres");
LogHelper.Log($"[StockImport] Líneas detectadas: {lineas.Count}");
LogHelper.Log($"[StockImport] Productos mapeados: {mapeados.Count}");
LogHelper.Log($"[StockImport]ERROR: {ex.Message}");
```

## 10. Próximas Mejoras (No en V1)

- [ ] Soporte para más formatos de factura
- [ ] Auto-detectar proveedor por formato
- [ ] Machine Learning ligero para mejor matching
- [ ] Exportar a Excel para revisión manual

---

## 11. Validaciones V1

- [ ] Funciona con PDFs de al menos 3 proveedores diferentes
- [ ] Tiempo de procesamiento < 5 segundos por página
- [ ] Memoria RAM usada < 200MB
- [ ] Interfaz responde durante procesamiento
- [ ] Permite cancelar operación larga
- [ ] Exporta a Excel la tabla de预览 para revisión