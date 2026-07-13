using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    ///     /// DTO para filtros de auditoría.
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

        ///         /// Convierte el filtro de TipoOperacion string a código numérico.
        public int? TipoOperacionCodigo => TipoOperacion switch
        {
            "Creación" => 1,
            "Modificación" => 2,
            "Eliminación" => 3,
            _ => null
        };
    }

    ///     /// Resultado de auditoría con filtros aplicados.
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
