namespace GestionComercial.Aplicacion.DTOs;

/// /// Envelope genérico para respuestas paginadas.
/// Reemplaza tuplas (datos, total) con metadata completa para el frontend.
/// <typeparam name="T">Tipo de los elementos de la página.</typeparam>
public class PagedResult<T>
{
    public List<T> Items { get; init; } = [];
    public int     TotalItems   { get; init; }
    public int     Pagina       { get; init; }
    public int     TotalPaginas { get; init; }
    public int     ItemsPorPagina { get; init; }

    public bool   HasPreviousPage => Pagina > 1;
    public bool   HasNextPage     => Pagina < TotalPaginas;

    public static PagedResult<T> Create(IEnumerable<T> items, int totalItems, int pagina, int itemsPorPagina)
        => new()
        {
            Items = items.ToList(),
            TotalItems = totalItems,
            Pagina = pagina,
            ItemsPorPagina = itemsPorPagina,
            TotalPaginas = (int)Math.Ceiling(totalItems / (double)itemsPorPagina)
        };
}
