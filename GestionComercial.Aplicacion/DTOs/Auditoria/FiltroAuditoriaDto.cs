using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    /// <summary>
    /// DTO para filtros de auditoría.
    /// </summary>
    public class FiltroAuditoriaDto
    {
        public int? IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? TipoOperacion { get; set; } // Creación | Modificación | Eliminación
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? NombreTabla { get; set; }
        public int? RegistroId { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int TamanioPagina { get; set; } = 50;

        /// <summary>
        /// Convierte el filtro de TipoOperacion string a código numérico.
        /// </summary>
        public int? TipoOperacionCodigo => TipoOperacion switch
        {
            "Creación" => 1,
            "Modificación" => 2,
            "Eliminación" => 3,
            _ => null
        };
    }

    /// <summary>
    /// Resultado de auditoría con filtros aplicados.
    /// </summary>
    public class AuditoriaFiltradaDto
    {
        public List<AuditoriaLogDto> Registros { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int TamanioPagina { get; set; } = 50;
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanioPagina);
        public FiltroAuditoriaDto FiltrosAplicados { get; set; } = new();
    }
}
