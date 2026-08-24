using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels
{
    public class FeatureShowcaseViewModel : Screen
    {
        private int _currentIndex;
        public int CurrentIndex
        {
            get => _currentIndex;
            set { _currentIndex = value; NotifyOfPropertyChange(() => CurrentIndex); NotifyOfPropertyChange(() => TituloActual); NotifyOfPropertyChange(() => DescripcionActual); NotifyOfPropertyChange(() => IconoActual); NotifyOfPropertyChange(() => PuedeAvanzar); NotifyOfPropertyChange(() => PuedeRetroceder); NotifyOfPropertyChange(() => Progreso); }
        }

        public string TituloActual => Features[CurrentIndex].Titulo;
        public string DescripcionActual => Features[CurrentIndex].Descripcion;
        public string IconoActual => Features[CurrentIndex].Icono;
        public bool PuedeAvanzar => CurrentIndex < Features.Count - 1;
        public bool PuedeRetroceder => CurrentIndex > 0;
        public string Progreso => $"{CurrentIndex + 1} / {Features.Count}";

        public List<FeatureInfo> Features { get; } = new()
        {
            new("🛒", "Punto de Venta",
                "Sistema completo de ventas con carrito, descuentos por ítem, múltiples métodos de pago (efectivo, tarjeta, transferencia) y comprobantes automáticos."),
            new("📦", "Gestión de Productos",
                "CRUD completo de productos con categorías, código de barras, stock mínimo, alertas de stock crítico y ajuste masivo de precios."),
            new("👥", "Clientes y Proveedores",
                "Base de datos de clientes con búsqueda rápida, historial de compras. Proveedores con gestión de costos y compras asociadas."),
            new("💳", "Control de Caja",
                "Apertura y cierre de caja obligatorio, turnos (mañana/tarde/noche), movimientos manuales de ingreso/egreso y auditoría completa."),
            new("📊", "Reportes y Análisis",
                "Dashboard con KPIs, ventas por día/sucursal/método de pago, productos más vendidos, márgenes de ganancia, rotación de stock y reporte de gerencia."),
            new("🏷️", "Sistema de Descuentos",
                "Descuentos automáticos por método de pago, por producto o categoría, con condiciones de vigencia y límites configurables."),
            new("📥", "Importación Excel",
                "Importación masiva de productos desde archivos Excel con validación automática de datos duplicados y erróneos."),
            new("🔒", "Seguridad y Permisos",
                "5 roles preconfigurados (Admin, Gerente, Vendedor, Compras, Inventario) con permisos granulares por módulo. Contraseñas hasheadas con BCrypt."),
            new("💾", "Backup Automático",
                "Respaldo automático de la base de datos con configuración de frecuencia y ruta personalizable. Restauración con un clic."),
            new("⚡", "Rendimiento",
                "Búsqueda server-side, queries optimizadas, lotes de actualización y 404 tests unitarios garantizando la calidad del código."),
        };

        public FeatureShowcaseViewModel() { }

        public void Avanzar()
        {
            if (PuedeAvanzar) CurrentIndex++;
        }

        public void Retroceder()
        {
            if (PuedeRetroceder) CurrentIndex--;
        }

        public async Task Finalizar()
        {
            await TryCloseAsync(true);
        }
    }

    public record FeatureInfo(string Icono, string Titulo, string Descripcion);
}
