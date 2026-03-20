using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    /// <summary>
    /// DTO para mostrar registros de auditoría en la UI.
    /// Deserializa JSON de ValoresAnteriores/ValoresNuevos para mostrar cambios legibles.
    /// </summary>
    public class AuditoriaLogDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la tabla afectada ("Cajas" | "MovimientosCaja").
        /// </summary>
        public string NombreTabla { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del registro afectado.
        /// </summary>
        public int RegistroId { get; set; }

        /// <summary>
        /// Tipo de operación localized ("Creación" | "Modificación" | "Eliminación").
        /// </summary>
        public string TipoOperacion { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario que realizó la operación.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de la operación.
        /// </summary>
        public DateTime FechaOperacion { get; set; }

        /// <summary>
        /// Estado anterior del registro (JSON serializado).
        /// </summary>
        public string? ValoresAnteriores { get; set; }

        /// <summary>
        /// Estado nuevo del registro (JSON serializado).
        /// </summary>
        public string? ValoresNuevos { get; set; }

        /// <summary>
        /// ValoresAnteriores deserializados como diccionario para mostrar de forma legible.
        /// </summary>
        public Dictionary<string, object?>? ValoresAnterioresDeserializados { get; set; }

        /// <summary>
        /// ValoresNuevos deserializados como diccionario para mostrar de forma legible.
        /// </summary>
        public Dictionary<string, object?>? ValoresNuevosDeserializados { get; set; }

        /// <summary>
        /// Cambios detectados entre valores anteriores y nuevos (para auditoría de caja).
        /// </summary>
        public List<CambioAuditoriaDto>? Cambios { get; set; }

        /// <summary>
        /// Deserializa el JSON de ValoresAnteriores y ValoresNuevos.
        /// Llamar después de mapear desde la entidad.
        /// </summary>
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

        /// <summary>
        /// Compara valores anteriores y nuevos para detectar qué campos cambiaron.
        /// </summary>
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

    /// <summary>
    /// Representa un cambio individual detectado en la auditoría.
    /// </summary>
    public class CambioAuditoriaDto
    {
        public string Campo { get; set; } = string.Empty;
        public string ValorAnterior { get; set; } = string.Empty;
        public string ValorNuevo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción legible del cambio.
        /// </summary>
        public string Descripcion => $"{Campo}: {ValorAnterior} → {ValorNuevo}";
    }
}
