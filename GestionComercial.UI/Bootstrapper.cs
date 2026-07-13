using Caliburn.Micro;
using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Clientes;
using Microsoft.Data.Sqlite;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Validators;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Infraestructura.Servicios;
using GestionComercial.Persistencia.Contexto;
using GestionComercial.Persistencia.Repositorio;
using GestionComercial.UI.Helpers;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Pagos.Strategies;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.Views.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;

namespace GestionComercial.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = null!;

        public Bootstrapper() => Initialize();

        protected override void Configure()
        {
            _container = new SimpleContainer();
      
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // ── Base de datos ─────────────────────────────────────────────────
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")!;

            // ── Resolver ruta relativa de SQLite ──────────────────────────────
            var connBuilder = new SqliteConnectionStringBuilder(connectionString);
            if (!Path.IsPathRooted(connBuilder.DataSource))
            {
                var assemblyDir = AppDomain.CurrentDomain.BaseDirectory;
                var assemblyPath = Path.Combine(assemblyDir, connBuilder.DataSource);

                // Buscar la DB en el directorio del proyecto UI (dev, con datos reales)
                var uiProjectDir = Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", ".."));
                var sourcePath = Path.Combine(uiProjectDir, connBuilder.DataSource);

                connBuilder.DataSource = File.Exists(sourcePath) ? sourcePath : assemblyPath;
            }
            connectionString = connBuilder.ConnectionString;

            // ── Optimización SQLite: WAL mode + synchronous=NORMAL ────────────
            //    WAL permite lecturas concurrentes sin bloqueos.
            //    synchronous=NORMAL reduce fsync (más rápido) sin riesgo de corrupción.
            var sqliteConn = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            sqliteConn.Open();
            using (var cmd = sqliteConn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL;";
                cmd.ExecuteNonQuery();
            }
            sqliteConn.Close();

            _container.Handler<GestionComercialContext>(
                _ => new GestionComercialContext(
                    new DbContextOptionsBuilder<GestionComercialContext>()
                        .UseSqlite(connectionString)
                        .Options));

            _container.Handler<IUnitOfWork>(
                c => new UnitOfWork(c.GetInstance<GestionComercialContext>()));

            _container.Handler<IRolRepositorio>(c => new RolRepositorio(c.GetInstance<GestionComercialContext>()));

            // ── Servicios ─────────────────────────────────────────────────────
            _container.Singleton<SesionServicio>();
            _container.Singleton<IServicioImpresion, ServicioImpresionTermica>();
            _container.Singleton<PaymentStrategyFactory>();

            // Servicios de Dominio (implementados en Infraestructura)
            _container.Singleton<IPasswordHasher, PasswordHasher>();
            _container.Handler<IBackupService>(_ => new BackupService(connectionString));

            _container.PerRequest<AutenticacionServicio>();
            _container.PerRequest<IClienteServicio, ClienteServicio>();
            _container.PerRequest<IVentaServicio, VentaServicio>();
            _container.PerRequest<ICompraServicio, CompraServicio>();
            _container.PerRequest<IProductoServicio, ProductoServicio>();
            _container.PerRequest<ICajaServicio, CajaServicio>();
            _container.PerRequest<IAuditoriaServicio, AuditoriaServicio>();
            _container.PerRequest<IAuditoriaAppService, AuditoriaAppService>();
            _container.PerRequest<IProveedorServicio, ProveedorServicio>();
            _container.PerRequest<IStockServicio, StockServicio>();
            _container.PerRequest<IInventarioServicio, InventarioServicio>();
            _container.PerRequest<IReporteServicio, ReporteServicio>();
            _container.PerRequest<IUsuarioServicio, UsuarioServicio>();
            _container.PerRequest<RecuperacionContrasenaServicio>();
            // NOTE: VentaValidator se registra más abajo con Handler para pasar IUnitOfWork.Productos

            // ── Validators (FluentValidation) ─────────────────────────────────
            // Nota: c.GetInstance<IUnitOfWork>().Clientes asume que IUnitOfWork
            // expone los repositorios como propiedades. Ajustá los nombres
            // según tu implementación de UnitOfWork.
            _container.Handler<IValidator<ClienteCrearDto>>(
                c => new ClienteValidator(c.GetInstance<IUnitOfWork>().Clientes));

            _container.Handler<IValidator<ProductoCrearDto>>(
                c => new ProductoValidator(c.GetInstance<IUnitOfWork>().Productos));

            _container.Handler<IValidator<UsuarioCrearDto>>(
                c => new UsuarioValidator(c.GetInstance<IUnitOfWork>().Usuarios));

            _container.Handler<IValidator<VentaCrearDto>>(
                c => new VentaValidator(c.GetInstance<IUnitOfWork>().Productos));

            _container.Handler<IValidator<CompraCrearDto>>(
                c => new CompraValidator(c.GetInstance<IUnitOfWork>().Proveedores));

            _container.Handler<IValidator<ProveedorCrearDto>>(
                c => new ProveedorValidator());

            _container.Handler<IValidator<ProveedorActualizarDto>>(
                c => new ProveedorActualizarValidator());

            _container.Handler<IValidator<CajaAbrirDto>>(
                c => new CajaValidator(c.GetInstance<IUnitOfWork>().Cajas));

            _container.Handler<IValidator<ProductoActualizarDto>>(
                c => new ProductoActualizarValidator(c.GetInstance<IUnitOfWork>().Productos));

            _container.Handler<IValidator<ProductoImportarDto>>(
                c => new ProductoImportarValidator());

            // ── Navigation Service ─────────────────────────────────────────────────
            _container.Singleton<GestionComercial.UI.Views.Servicios.INavigationService, GestionComercial.UI.Views.Servicios.NavigationService>();

            // ── ViewModels ────────────────────────────────────────────────────
            var assembly = Assembly.GetExecutingAssembly();
            var viewModelTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.Namespace != null
                          && t.Namespace.StartsWith("GestionComercial.UI.ViewModels")
                         && typeof(Screen).IsAssignableFrom(t));

            foreach (var vmType in viewModelTypes)
            {
                if (vmType == typeof(ShellViewModel))
                    _container.RegisterSingleton(vmType, null, vmType);
                else
                    _container.RegisterPerRequest(vmType, null, vmType);
            }

            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var vmName = modelType.FullName ?? string.Empty;
                var viewName = vmName
                    .Replace(".ViewModels.", ".Views.")   // plural → singular no, Views directo
                    .Replace(".ViewModel.", ".Views.")   // por si alguno usa singular
                    .Replace("ViewModel", "View");
                var viewType = modelType.Assembly.GetType(viewName);
                
                if (viewType == null)
                {
                    // Intentar con namespace completo
                    viewType = Assembly.GetExecutingAssembly().GetType(viewName);
                }
                
                System.Diagnostics.Debug.WriteLine(
                    $"[ViewLocator] {modelType.Name} -> {viewType?.Name ?? "NO ENCONTRADO"}");
                return viewType;
            };
        }

        protected override object GetInstance(Type service, string key)
            => _container.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service)
            => _container.GetAllInstances(service);

        protected override void BuildUp(object instance)
            => _container.BuildUp(instance);

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            // ── Asegurar base de datos con MigrateAsync (migraciones EF Core) ──
            try
            {
                var context = _container.GetInstance<GestionComercial.Persistencia.Contexto.GestionComercialContext>();
                
                // Ejecutar migraciones pendientes (incluye baseline + views + triggers).
                await context.Database.MigrateAsync();

                // ── First-run: si no hay usuarios, mostrar configuración inicial ──
                var tieneUsuarios = await context.Usuarios.AnyAsync();
                if (!tieneUsuarios)
                {
                    await DisplayRootViewForAsync<GestionComercial.UI.ViewModels.Configuracion.ConfiguracionInicialViewModel>();
                    return;
                }
                
                // ── Seed movimientos de stock inicial ─────────────────
                var tieneMovimientos = await context.MovimientosStock.AnyAsync();
                if (!tieneMovimientos)
                {
                    var productosConStock = await context.Productos
                        .Where(p => p.StockActual > 0)
                        .ToListAsync();

                    if (productosConStock.Count > 0)
                    {
                        var hoy = DateTime.Now;
                        var diaBase = hoy.AddDays(-5);
                        var dias = Enumerable.Range(0, productosConStock.Count)
                            .Select(i => diaBase.AddMinutes(i * 15))  // 15 min entre cada uno
                            .ToList();

                        foreach (var prod in productosConStock)
                        {
                            var idx = productosConStock.IndexOf(prod);
                            var mov = MovimientoStock.Ajuste(
                                cantidad: prod.StockActual,
                                stockAnterior: 0,
                                idProducto: prod.Id,
                                idSucursal: 1,
                                idUsuario: 1,
                                observacion: "Stock inicial",
                                referenciaId: null
                            );
                            // Setear fecha explícita para no tener todos iguales
                            mov.Fecha = dias[idx];
                            context.MovimientosStock.Add(mov);
                        }

                        await context.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Creados {productosConStock.Count} movimientos de stock inicial");
                    }
                }

                // ── Backfill Turno en cajas existentes ────────────────
                var cajasSinTurno = await context.Cajas
                    .Where(c => c.Turno == null || c.Turno == "")
                    .ToListAsync();
                if (cajasSinTurno.Count > 0)
                {
                    var turnos = new[] { "Mañana", "Tarde", "Noche" };
                    var rng = new Random();
                    foreach (var caja in cajasSinTurno)
                        caja.Turno = turnos[rng.Next(turnos.Length)];

                    await context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Asignados turnos a {cajasSinTurno.Count} cajas");
                }

                // Verify
                var count = await context.Usuarios.CountAsync();
                System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Total usuarios: {count}");

                // ── Cerrar cajas huérfanas ───────────────────────
                var cajasAbiertas = await context.Cajas
                    .Where(c => c.Estado == 1)
                    .ToListAsync();
                
                foreach (var caja in cajasAbiertas)
                {
                    caja.Estado = 2;
                    caja.FechaCierre = DateTime.Now;
                }
                
                if (cajasAbiertas.Count > 0)
                {
                    await context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Cerradas {cajasAbiertas.Count} cajas huérfanas");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Error: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Inner: {ex.InnerException.Message}");
            }
            
            // ── Backup automático (si está habilitado) ───────────────────────
            try
            {
                var backupService = _container.GetInstance<GestionComercial.Dominio.Interfaces.Servicios.IBackupService>();
                var resultado = await backupService.BackupAutomaticoSiHabilitadoAsync();
                if (resultado != null && resultado.Success)
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Backup automático realizado: {resultado.RutaBackup}");
                else if (resultado != null && !resultado.Success)
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Backup automático falló: {resultado.ErrorMessage}");
                // Si null → estaba deshabilitado, no hacer nada
            }
            catch (Exception ex)
            {
                // No bloquear el inicio de la app si el backup falla
                System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Backup automático falló: {ex.Message}");
            }

            await DisplayRootViewForAsync<LoginViewModel>();
        }
    }
}
