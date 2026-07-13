using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    ///     /// DTO para mostrar registros de auditoría en la UI.
    /// Deserializa JSON de ValoresAnteriores/ValoresNuevos para mostrar cambios legibles.
    /// Incluye propiedades calculadas para visualización financiera en prevención de fraude.
    public class AuditoriaLogDto
    {
        public int Id { get; set; }

        ///         /// Nombre de la tabla afectada ("Cajas" | "MovimientosCaja").
        public string NombreTabla { get; set; } = string.Empty;

        ///         /// Identificador del registro afectado.
        public int RegistroId { get; set; }

        ///         /// Tipo de operación localized ("Creación" | "Modificación" | "Eliminación").
        public string TipoOperacion { get; set; } = string.Empty;

        ///         /// Nombre del usuario que realizó la operación.
        public string Usuario { get; set; } = string.Empty;

        ///         /// Fecha y hora de la operación.
        public DateTime FechaOperacion { get; set; }

        ///         /// Estado anterior del registro (JSON serializado).
        public string? ValoresAnteriores { get; set; }

        ///         /// Estado nuevo del registro (JSON serializado).
        public string? ValoresNuevos { get; set; }

        ///         /// ValoresAnteriores deserializados como diccionario para mostrar de forma legible.
        public Dictionary<string, object?>? ValoresAnterioresDeserializados { get; set; }

        ///         /// ValoresNuevos deserializados como diccionario para mostrar de forma legible.
        public Dictionary<string, object?>? ValoresNuevosDeserializados { get; set; }

        ///         /// Cambios detectados entre valores anteriores y nuevos (para auditoría de caja).
        public List<CambioAuditoriaDto>? Cambios { get; set; }

        // ═══════════════════════════════════════════════════════════════════════════
        // PROPIEDADES CALCULADAS PARA PREVENCIÓN DE FRAUDE
        // ═══════════════════════════════════════════════════════════════════════════

        ///         /// Resumen de todos los cambios detectados en formato legible.
        /// Usa el campo CAMBIOS del binding en la UI.
        public string DetalleCambios => Cambios != null && Cambios.Any()
            ? string.Join(" | ", Cambios.Select(c => c.Descripcion))
            : "Sin cambios";

        ///         /// Número de caja afectada (extraído del JSON deserializado).
        public string? NumeroCaja
        {
            get
            {
                // Buscar en ValoresNuevos primero (si es creación), si no en Anteriores
                if (ValoresNuevosDeserializados?.TryGetValue("NumeroCaja", out var num) == true && num != null)
                    return num.ToString();
                if (ValoresAnterioresDeserializados?.TryGetValue("NumeroCaja", out var numAnt) == true && numAnt != null)
                    return numAnt.ToString();
                return null;
            }
        }

        ///         /// Monto involucrado en la operación (extraído de ValoresNuevos para creación,
        /// o de Cambios para modificaciones).
        public decimal? MontoMostrar
        {
            get
            {
                // Para Creación: buscar MontoInicial, MontoFinal o Monto en ValoresNuevos
                if (TipoOperacion == "Creación")
                {
                    if (ValoresNuevosDeserializados?.TryGetValue("MontoInicial", out var mi) == true)
                        return ParseDecimal(mi);
                    if (ValoresNuevosDeserializados?.TryGetValue("MontoFinal", out var mf) == true)
                        return ParseDecimal(mf);
                    if (ValoresNuevosDeserializados?.TryGetValue("Monto", out var m) == true)
                        return ParseDecimal(m);
                }
                
                // Para Modificación: buscar en Cambios el campo Monto o MontoFinal
                var cambioMonto = Cambios?.FirstOrDefault(c => c.Campo == "Monto" || c.Campo == "MontoFinal" || c.Campo == "MontoInicial");
                if (cambioMonto != null)
                {
                    return ParseDecimal(cambioMonto.ValorNuevo);
                }

                // Valor actual en ValoresNuevos
                if (ValoresNuevosDeserializados?.TryGetValue("Monto", out var monto) == true)
                    return ParseDecimal(monto);
                if (ValoresNuevosDeserializados?.TryGetValue("MontoFinal", out var montoFinal) == true)
                    return ParseDecimal(montoFinal);

                return null;
            }
        }

        ///         /// Valor monetario anterior (antes del cambio).
        public decimal? ValorAnteriorMostrar
        {
            get
            {
                // Buscar en ValoresAnteriores
                if (ValoresAnterioresDeserializados?.TryGetValue("MontoInicial", out var mi) == true)
                    return ParseDecimal(mi);
                if (ValoresAnterioresDeserializados?.TryGetValue("MontoFinal", out var mf) == true)
                    return ParseDecimal(mf);
                if (ValoresAnterioresDeserializados?.TryGetValue("Monto", out var m) == true)
                    return ParseDecimal(m);

                // Buscar en Cambios
                var cambioMonto = Cambios?.FirstOrDefault(c => c.Campo == "Monto" || c.Campo == "MontoFinal" || c.Campo == "MontoInicial");
                if (cambioMonto != null)
                {
                    return ParseDecimal(cambioMonto.ValorAnterior);
                }

                return null;
            }
        }

        ///         /// Valor monetario nuevo (después del cambio).
        public decimal? ValorNuevoMostrar
        {
            get
            {
                // Buscar en ValoresNuevos
                if (ValoresNuevosDeserializados?.TryGetValue("MontoInicial", out var mi) == true)
                    return ParseDecimal(mi);
                if (ValoresNuevosDeserializados?.TryGetValue("MontoFinal", out var mf) == true)
                    return ParseDecimal(mf);
                if (ValoresNuevosDeserializados?.TryGetValue("Monto", out var m) == true)
                    return ParseDecimal(m);

                return null;
            }
        }

        ///         /// Diferencia monetaria entre ValorNuevo y ValorAnterior.
        /// Positivo = aumento, Negativo = disminución.
        public decimal? DiferenciaMonetaria
        {
            get
            {
                if (ValorAnteriorMostrar.HasValue && ValorNuevoMostrar.HasValue)
                {
                    return ValorNuevoMostrar.Value - ValorAnteriorMostrar.Value;
                }
                
                // Si solo tenemos MontoMostrar, buscar si hay un valor anterior en Cambios
                var cambioMonto = Cambios?.FirstOrDefault(c => c.Campo == "Monto" || c.Campo == "MontoFinal" || c.Campo == "MontoInicial");
                if (cambioMonto != null && !string.IsNullOrEmpty(cambioMonto.ValorAnterior) && cambioMonto.ValorAnterior != "—")
                {
                    var anterior = ParseDecimal(cambioMonto.ValorAnterior);
                    var nuevo = ParseDecimal(cambioMonto.ValorNuevo);
                    if (anterior.HasValue && nuevo.HasValue)
                        return nuevo.Value - anterior.Value;
                }

                return null;
            }
        }

        ///         /// Indica si la diferencia monetaria es sospechosa (diferencia > 10% del valor original).
        public bool EsDiferenciaSospechosa
        {
            get
            {
                if (!DiferenciaMonetaria.HasValue || !ValorAnteriorMostrar.HasValue)
                    return false;

                if (ValorAnteriorMostrar.Value == 0)
                    return DiferenciaMonetaria.Value != 0;

                var porcentajeCambio = Math.Abs(DiferenciaMonetaria.Value / ValorAnteriorMostrar.Value) * 100;
                return porcentajeCambio > 10; // Más del 10% de variación
            }
        }

        ///         /// Tipo de operación de caja derivado (Apertura, Cierre, Movimiento, etc.).
        public string TipoOperacionCaja
        {
            get
            {
                return TipoOperacion switch
                {
                    "Creación" when NombreTabla == "Cajas" => "Apertura",
                    "Modificación" when NombreTabla == "Cajas" => EsCierre() ? "Cierre" : "Modificación",
                    "Modificación" when NombreTabla == "MovimientosCaja" => "Movimiento",
                    _ => TipoOperacion
                };
            }
        }

        private bool EsCierre()
        {
            if (ValoresNuevosDeserializados?.TryGetValue("FechaCierre", out var fc) == true && fc != null)
                return true;
            if (ValoresAnterioresDeserializados?.TryGetValue("Estado", out var estadoAnt) == true)
            {
                var estado = ParseInt(estadoAnt);
                return estado == 2; // Estado 2 = Cerrada
            }
            return false;
        }

        private static decimal? ParseDecimal(object? valor)
        {
            if (valor == null) return null;

            try
            {
                // Si es JsonElement, obtener el valor interno
                if (valor is JsonElement je)
                {
                    if (je.ValueKind == JsonValueKind.Number)
                        return je.GetDecimal();
                    if (je.ValueKind == JsonValueKind.String)
                    {
                        var strVal = je.GetString();
                        decimal.TryParse(strVal, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedFromStr);
                        return parsedFromStr;
                    }
                    return null;
                }

                // Intentar parsear directamente
                decimal.TryParse(valor.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedDirect);
                return parsedDirect;
            }
            catch
            {
                return null;
            }
        }

        private static int? ParseInt(object? valor)
        {
            if (valor == null) return null;

            try
            {
                if (valor is JsonElement je)
                {
                    if (je.ValueKind == JsonValueKind.Number)
                        return je.GetInt32();
                    if (je.ValueKind == JsonValueKind.String)
                    {
                        var strVal = je.GetString();
                        int.TryParse(strVal, out var parsedFromStr);
                        return parsedFromStr;
                    }
                    return null;
                }

                int.TryParse(valor.ToString(), out var parsedDirect);
                return parsedDirect;
            }
            catch
            {
                return null;
            }
        }

        ///         /// Deserializa el JSON de ValoresAnteriores y ValoresNuevos.
        /// Llamar después de mapear desde la entidad.
        public void DeserializarJson()
        {
            if (!string.IsNullOrWhiteSpace(ValoresAnteriores))
            {
                try
                {
                    ValoresAnterioresDeserializados = JsonSerializer.Deserialize<Dictionary<string, object?>>(ValoresAnteriores);
                }
                catch
                {
                    // Si falla, guardar como texto plano
                    ValoresAnterioresDeserializados = new Dictionary<string, object?> { { "raw", ValoresAnteriores } };
                }
            }

            if (!string.IsNullOrWhiteSpace(ValoresNuevos))
            {
                try
                {
                    ValoresNuevosDeserializados = JsonSerializer.Deserialize<Dictionary<string, object?>>(ValoresNuevos);
                }
                catch
                {
                    ValoresNuevosDeserializados = new Dictionary<string, object?> { { "raw", ValoresNuevos } };
                }
            }

            // Detectar cambios específicos para auditoría de caja
            DetectarCambios();
        }

        ///         /// Compara valores anteriores y nuevos para detectar qué campos cambiaron.
        private void DetectarCambios()
        {
            if (ValoresAnterioresDeserializados == null || ValoresNuevosDeserializados == null)
                return;

            Cambios = new List<CambioAuditoriaDto>();

            // Campos comunes en auditoría de caja
            var camposRelevantes = new[] { "MontoInicial", "MontoFinal", "Estado", "Observacion", "Monto", "Concepto", "Tipo" };

            foreach (var campo in camposRelevantes)
            {
                ValoresAnterioresDeserializados.TryGetValue(campo, out var anterior);
                ValoresNuevosDeserializados.TryGetValue(campo, out var nuevo);

                if (!Equals(anterior, nuevo))
                {
                    Cambios.Add(new CambioAuditoriaDto
                    {
                        Campo = campo,
                        ValorAnterior = FormatearValor(anterior),
                        ValorNuevo = FormatearValor(nuevo)
                    });
                }
            }
        }

        private static string FormatearValor(object? valor)
        {
            return valor?.ToString() ?? "—";
        }
    }

    ///     /// Representa un cambio individual detectado en la auditoría.
    public class CambioAuditoriaDto
    {
        public string Campo { get; set; } = string.Empty;
        public string ValorAnterior { get; set; } = string.Empty;
        public string ValorNuevo { get; set; } = string.Empty;

        ///         /// Descripción legible del cambio.
        public string Descripcion => $"{Campo}: {ValorAnterior} → {ValorNuevo}";
    }
}
