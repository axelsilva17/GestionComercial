using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Configuracion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ReporteMantenimientoServicio : IReporteMantenimientoServicio
    {
        private readonly IDiagnosticoServicio _diagnostico;

        private static bool _licenseSet = false;
        private static readonly object _lock = new();

        public ReporteMantenimientoServicio(IDiagnosticoServicio diagnostico)
        {
            _diagnostico = diagnostico;
        }

        private static void EnsureLicense()
        {
            if (_licenseSet) return;
            lock (_lock)
            {
                if (_licenseSet) return;
                QuestPDF.Settings.License = LicenseType.Community;
                _licenseSet = true;
            }
        }

        public byte[] GenerarPdf(
            DiagnosticoResultDto diagnostico,
            string nombreEmpresa,
            int idEmpresa,
            BackupConfig? backupConfig = null,
            List<string>? accionesRealizadas = null)
        {
            EnsureLicense();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    page.Header().Element(header =>
                    {
                        header.Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("REPORTE DE MANTENIMIENTO").FontSize(20).Bold();
                                col.Item().Text("GestionComercial").FontSize(14).FontColor(Colors.Grey.Medium);
                            });
                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text($"Fecha: {DateTime.UtcNow:dd/MM/yyyy HH:mm}").FontSize(10);
                                col.Item().Text($"Empresa: {nombreEmpresa} (ID: {idEmpresa})").FontSize(10);
                            });
                        });
                    });

                    page.Content().Element(content =>
                    {
                        content.Column(col =>
                        {
                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                            col.Item().Text("ESTADÍSTICAS").FontSize(14).Bold();
                            col.Item().PaddingBottom(5);

                            if (diagnostico.Stats != null)
                            {
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Total Ventas:      {diagnostico.Stats.TotalVentas:N0}");
                                    row.RelativeItem().Text($"Total Productos:   {diagnostico.Stats.TotalProductos:N0}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Total Clientes:    {diagnostico.Stats.TotalClientes:N0}");
                                    row.RelativeItem().Text($"Total Compras:     {diagnostico.Stats.TotalCompras:N0}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Total Proveedores: {diagnostico.Stats.TotalProveedores:N0}");
                                    row.RelativeItem().Text($"Métodos de Pago:   {diagnostico.Stats.TotalMetodosPago:N0}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Categorías:        {diagnostico.Stats.TotalCategorias:N0}");
                                    row.RelativeItem().Text($"Tamaño DB:         {diagnostico.Stats.TamanoFormateado}");
                                });
                            }

                            // ── INTEGRIDAD ──
                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                            col.Item().Text("INTEGRIDAD").FontSize(14).Bold();
                            col.Item().PaddingBottom(5);
                            col.Item().Text($"Estado: {(diagnostico.IntegridadOk ? "OK" : "ERROR")}");
                            col.Item().Text($"Detalle: {diagnostico.MensajeIntegridad}");

                            // ── BACKUP ──
                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                            col.Item().Text("BACKUP").FontSize(14).Bold();
                            col.Item().PaddingBottom(5);
                            if (backupConfig != null)
                            {
                                col.Item().Text($"Estado:    {(backupConfig.Frecuencia != Dominio.Enumeraciones.FrecuenciaBackupEnum.Desactivado ? "Configurado" : "Desactivado")}");
                                col.Item().Text($"Frecuencia: {backupConfig.Frecuencia}");
                                col.Item().Text($"Último backup: {(backupConfig.UltimoBackup?.ToString("dd/MM/yyyy HH:mm") ?? "Nunca")}");
                            }
                            else
                            {
                                col.Item().Text("Estado: No configurado");
                            }

                            // ── SISTEMA ──
                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                            col.Item().Text("SISTEMA").FontSize(14).Bold();
                            col.Item().PaddingBottom(5);
                            var version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";
                            col.Item().Text($"Versión del sistema: {version}");
                            col.Item().Text($"Último mantenimiento: {DateTime.UtcNow:dd/MM/yyyy HH:mm}");
                            col.Item().Text("Estado general: Operativo");

                            // ── ADVERTENCIAS ──
                            if (diagnostico.Warnings.Count > 0)
                            {
                                col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text("ADVERTENCIAS").FontSize(14).Bold();
                                col.Item().PaddingBottom(5);
                                foreach (var warning in diagnostico.Warnings)
                                {
                                    col.Item().Text($"• [{warning.Tipo}] {warning.Mensaje}");
                                }
                            }

                            // ── ACCIONES REALIZADAS ──
                            if (accionesRealizadas?.Any() == true)
                            {
                                col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text("ACCIONES REALIZADAS").FontSize(14).Bold();
                                col.Item().PaddingBottom(5);
                                foreach (var accion in accionesRealizadas)
                                {
                                    col.Item().Text($"• {accion}");
                                }
                            }
                        });
                    });

                    page.Footer().Element(footer =>
                    {
                        footer.AlignCenter().Text(txt =>
                        {
                            txt.Span("Generado por GestionComercial - Panel de Mantenimiento").FontSize(8).FontColor(Colors.Grey.Medium);
                        });
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        public async Task<string> GenerarReporteAsync(DiagnosticoResultDto diagnostico, string nombreEmpresa, int idEmpresa, BackupConfig? backupConfig = null, List<string>? accionesRealizadas = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine("  REPORTE DE MANTENIMIENTO");
            sb.AppendLine("  GestionComercial");
            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine($"Fecha: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"Empresa: {nombreEmpresa} (ID: {idEmpresa})");
            sb.AppendLine();

            // ── ESTADÍSTICAS ──
            sb.AppendLine("─── ESTADÍSTICAS ─────────────────────────");
            if (diagnostico.Stats != null)
            {
                sb.AppendLine($"  Total Ventas:      {diagnostico.Stats.TotalVentas:N0}");
                sb.AppendLine($"  Total Productos:   {diagnostico.Stats.TotalProductos:N0}");
                sb.AppendLine($"  Total Clientes:    {diagnostico.Stats.TotalClientes:N0}");
                sb.AppendLine($"  Total Compras:     {diagnostico.Stats.TotalCompras:N0}");
                sb.AppendLine($"  Total Proveedores: {diagnostico.Stats.TotalProveedores:N0}");
                sb.AppendLine($"  Métodos de Pago:   {diagnostico.Stats.TotalMetodosPago:N0}");
                sb.AppendLine($"  Categorías:        {diagnostico.Stats.TotalCategorias:N0}");
                sb.AppendLine($"  Tamaño DB:         {diagnostico.Stats.TamanoFormateado}");
            }
            sb.AppendLine();

            // ── INTEGRIDAD ──
            sb.AppendLine("─── INTEGRIDAD ───────────────────────────");
            sb.AppendLine($"  Estado: {(diagnostico.IntegridadOk ? "OK" : "ERROR")}");
            sb.AppendLine($"  Detalle: {diagnostico.MensajeIntegridad}");
            sb.AppendLine();

            // ── BACKUP ──
            sb.AppendLine("─── BACKUP ───────────────────────────────");
            if (backupConfig != null)
            {
                sb.AppendLine($"  Estado: {(backupConfig.Frecuencia != Dominio.Enumeraciones.FrecuenciaBackupEnum.Desactivado ? "Configurado" : "Desactivado")}");
                sb.AppendLine($"  Frecuencia: {backupConfig.Frecuencia}");
                sb.AppendLine($"  Último backup: {backupConfig.UltimoBackup?.ToString("dd/MM/yyyy HH:mm") ?? "Nunca"}");
            }
            else
            {
                sb.AppendLine("  Estado: No configurado");
            }
            sb.AppendLine();

            // ── SISTEMA ──
            var version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";
            sb.AppendLine("─── SISTEMA ─────────────────────────────");
            sb.AppendLine($"  Versión del sistema: {version}");
            sb.AppendLine($"  Último mantenimiento: {DateTime.UtcNow:dd/MM/yyyy HH:mm}");
            sb.AppendLine("  Estado general: Operativo");
            sb.AppendLine();

            if (diagnostico.Warnings.Count > 0)
            {
                sb.AppendLine("─── ADVERTENCIAS ─────────────────────────");
                foreach (var w in diagnostico.Warnings)
                    sb.AppendLine($"  [{w.Tipo}] {w.Mensaje} ({w.Fecha:HH:mm:ss})");
                sb.AppendLine();
            }

            if (accionesRealizadas?.Any() == true)
            {
                sb.AppendLine("─── ACCIONES REALIZADAS ──────────────────");
                foreach (var accion in accionesRealizadas)
                    sb.AppendLine($"  • {accion}");
                sb.AppendLine();
            }

            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine("  Fin del reporte");
            sb.AppendLine("═══════════════════════════════════════════");

            return sb.ToString();
        }
    }
}
