using System;

namespace GestionComercial.Aplicacion.DTOs.Inventario
{
    /// <summary>
    /// Representa un movimiento de stock — usado en InventarioViewModel y Dashboard.
    /// </summary>
    public class MovimientoStockDto
    {
        public int      IdMovimiento    { get; set; }
        public string   TipoMovimiento  { get; set; }  // "Entrada" | "Salida" | "Ajuste"
        public string   Observacion     { get; set; }
        public int      Cantidad        { get; set; }
        public DateTime Fecha           { get; set; }
        public int      IdProducto      { get; set; }
        public string   ProductoNombre  { get; set; }
        public string   CodigoBarra     { get; set; }
        public string   CategoriaNombre { get; set; }
        public string   SucursalNombre  { get; set; }
        public string   UsuarioNombre   { get; set; }

        // Para UI
        public bool   EsEntrada  => TipoMovimiento == "Entrada";
        public string TipoIcono  => TipoMovimiento switch
        {
            "Entrada" => "↑",
            "Salida"  => "↓",
            "Ajuste"  => "⇄",
            _         => "·"
        };
    }
}

namespace GestionComercial.Aplicacion.DTOs.Productos
{
    /// <summary>
    /// DTO liviano para la lista de productos críticos en el Dashboard.
    /// </summary>
    public class ProductoCriticoDash
    {
        public int    IdProducto   { get; set; }
        public string Nombre       { get; set; }
        public int    StockActual  { get; set; }
        public int    StockMinimo  { get; set; }
        public string Inicial      => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
