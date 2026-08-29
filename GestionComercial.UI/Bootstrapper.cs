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
using GestionComercial.Persistencia.Seed;
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
        private string _connectionString = null!;

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
            // Desactivar pooling para que cada conexión arranque con PRAGMAs por defecto.
            // Sin esto, el seeder deja foreign_keys=ON en una conexión pooled y
            // la siguiente operación EF Core reutiliza esa conexión con FKs activas.
            connBuilder.Pooling = false;
            connectionString = connBuilder.ConnectionString;
            _connectionString = connectionString;

            // ── Optimización SQLite: WAL mode + synchronous=NORMAL ────────────
            //    WAL permite lecturas concurrentes sin bloqueos.
            //    synchronous=NORMAL reduce fsync (más rápido) sin riesgo de corrupción.
            var sqliteConn = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            sqliteConn.Open();
            using (var cmd = sqliteConn.CreateCommand())
            {
                cmd.CommandText = @"
                    PRAGMA journal_mode=WAL;
                    PRAGMA synchronous=NORMAL;
                    PRAGMA temp_store=MEMORY;
                    PRAGMA cache_size=-64000;
                    PRAGMA mmap_size=268435456;
                    PRAGMA foreign_keys=ON;";
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
            _container.Handler<IBackupService>(_ => new BackupService(
                connectionString,
                _container.GetInstance<GestionComercialContext>()));

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
            _container.PerRequest<IRolServicio, RolServicio>();
            _container.PerRequest<IDescuentoConfiguracionServicio, DescuentoConfiguracionServicio>();
            _container.PerRequest<RecuperacionContrasenaServicio>();
            _container.Singleton<DemoService>();
            _container.Singleton<DemoFeatureService>();
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
            // ── Contexto SEPARADO para operaciones de arranque ──────────────
            // Se descarta al final para que el contexto del DI container
            // (usado por login/servicios) esté LIMPIO sin ChangeTracker.
            using var bootCtx = new GestionComercial.Persistencia.Contexto.GestionComercialContext(
                new DbContextOptionsBuilder<GestionComercial.Persistencia.Contexto.GestionComercialContext>()
                    .UseSqlite(_connectionString)
                    .Options);

            try
            {
                // ── Ejecutar migraciones ──
                await bootCtx.Database.MigrateAsync();

                // ── Auto-seed: si la DB está vacía, cargar datos de prueba ──
                try
                {
                    await DatabaseSeeder.SeedIfNeededAsync(bootCtx);
                }
                catch (Exception exSeed)
                {
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Auto-seed falló: {exSeed.Message}");
                    if (exSeed.InnerException != null)
                        System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Inner: {exSeed.InnerException.Message}");
                }

                // ── Fix usuarios demo: raw SqliteConnection, cero EF Core ──
                try
                {
                    var hashAdmin = BCrypt.Net.BCrypt.HashPassword("Admin123!", 10);
                    var hashVendedor = BCrypt.Net.BCrypt.HashPassword("Vendedor123!", 10);
                    var hashGerente = BCrypt.Net.BCrypt.HashPassword("Gerente123!", 10);

                    using var rawConn = new SqliteConnection(_connectionString);
                    await rawConn.OpenAsync();

                    // Desactivar FKs por si hay registros hijos
                    using (var fkCmd = rawConn.CreateCommand())
                    {
                        fkCmd.CommandText = "PRAGMA foreign_keys=OFF;";
                        await fkCmd.ExecuteNonQueryAsync();
                    }

                    var upsertSql = @"INSERT OR REPLACE INTO Usuario 
                        (Id,Nombre,Apellido,Email,PasswordHash,Id_sucursal,Id_rol,IntentosFallidos,Activo,FechaAlta) 
                        VALUES ($id,$nom,$ape,$email,$hash,$suc,$rol,0,1,$fec)";

                    // Admin - Id=1, Rol=2 (Administrador)
                    using (var cmd = rawConn.CreateCommand())
                    {
                        cmd.CommandText = upsertSql;
                        cmd.Parameters.AddWithValue("$id", 1);
                        cmd.Parameters.AddWithValue("$nom", "Admin");
                        cmd.Parameters.AddWithValue("$ape", "Sistema");
                        cmd.Parameters.AddWithValue("$email", "admin@miempresa.com");
                        cmd.Parameters.AddWithValue("$hash", hashAdmin);
                        cmd.Parameters.AddWithValue("$suc", 1);
                        cmd.Parameters.AddWithValue("$rol", 2);
                        cmd.Parameters.AddWithValue("$fec", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        var affected = await cmd.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Admin upsert: {affected} rows");
                    }

                    // Vendedor - Id=2, Rol=3 (Vendedor)
                    using (var cmd = rawConn.CreateCommand())
                    {
                        cmd.CommandText = upsertSql;
                        cmd.Parameters.AddWithValue("$id", 2);
                        cmd.Parameters.AddWithValue("$nom", "Vendedor");
                        cmd.Parameters.AddWithValue("$ape", "Demo");
                        cmd.Parameters.AddWithValue("$email", "vendedor@miempresa.com");
                        cmd.Parameters.AddWithValue("$hash", hashVendedor);
                        cmd.Parameters.AddWithValue("$suc", 1);
                        cmd.Parameters.AddWithValue("$rol", 3);
                        cmd.Parameters.AddWithValue("$fec", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        var affected = await cmd.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Vendedor upsert: {affected} rows");
                    }

                    // Gerente - Id=3, Rol=1 (Gerente)
                    using (var cmd = rawConn.CreateCommand())
                    {
                        cmd.CommandText = upsertSql;
                        cmd.Parameters.AddWithValue("$id", 3);
                        cmd.Parameters.AddWithValue("$nom", "Gerente");
                        cmd.Parameters.AddWithValue("$ape", "Demo");
                        cmd.Parameters.AddWithValue("$email", "gerente@miempresa.com");
                        cmd.Parameters.AddWithValue("$hash", hashGerente);
                        cmd.Parameters.AddWithValue("$suc", 1);
                        cmd.Parameters.AddWithValue("$rol", 1);
                        cmd.Parameters.AddWithValue("$fec", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        var affected = await cmd.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Gerente upsert: {affected} rows");
                    }

                    // Restaurar FKs
                    using (var fkCmd = rawConn.CreateCommand())
                    {
                        fkCmd.CommandText = "PRAGMA foreign_keys=ON;";
                        await fkCmd.ExecuteNonQueryAsync();
                    }

                    // VERIFICAR que los hashes quedaron correctos
                    using (var verifyCmd = rawConn.CreateCommand())
                    {
                        verifyCmd.CommandText = "SELECT Email, PasswordHash FROM Usuario ORDER BY Id";
                        using var reader = await verifyCmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            var email = reader.GetString(0);
                            var hashDb = reader.GetString(1);
                            var wf = hashDb.Length >= 7 ? hashDb.Substring(0, 7) : "?";
                            System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Verify: {email} hash_prefix={wf} len={hashDb.Length}");
                        }
                    }

                    await rawConn.CloseAsync();
                    System.Diagnostics.Debug.WriteLine("[Bootstrapper] Usuarios demo OK");
                }
                catch (Exception exUsers)
                {
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Error fix usuarios: {exUsers.Message}");
                    if (exUsers.InnerException != null)
                        System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Inner: {exUsers.InnerException.Message}");
                }

                // ── First-run: si no hay usuarios, configuración inicial ──
                var tieneUsuarios = await bootCtx.Usuarios.AnyAsync();
                if (!tieneUsuarios)
                {
                    await DisplayRootViewForAsync<GestionComercial.UI.ViewModels.Configuracion.ConfiguracionInicialViewModel>();
                    return;
                }

                // ── Seed movimientos de stock inicial ──
                var tieneMovimientos = await bootCtx.MovimientosStock.AnyAsync();
                if (!tieneMovimientos)
                {
                    var productosConStock = await bootCtx.Productos
                        .Where(p => p.StockActual > 0).ToListAsync();

                    if (productosConStock.Count > 0)
                    {
                        var hoy = DateTime.Now;
                        var diaBase = hoy.AddDays(-5);
                        foreach (var prod in productosConStock)
                        {
                            var idx = productosConStock.IndexOf(prod);
                            var mov = MovimientoStock.Ajuste(
                                prod.StockActual, 0, prod.Id, 1, 1, "Stock inicial", null);
                            mov.Fecha = diaBase.AddMinutes(idx * 15);
                            bootCtx.MovimientosStock.Add(mov);
                        }
                        await bootCtx.SaveChangesAsync();
                    }
                    bootCtx.ChangeTracker.Clear();
                }

                // ── Backfill Turno en cajas existentes ──
                var turnos = new[] { "Mañana", "Tarde", "Noche" };
                var cajasSinTurno = await bootCtx.Cajas
                    .Where(c => c.Turno == null || c.Turno == "").ToListAsync();
                if (cajasSinTurno.Count > 0)
                {
                    var rng = new Random();
                    foreach (var caja in cajasSinTurno)
                        caja.Turno = turnos[rng.Next(turnos.Length)];
                    await bootCtx.SaveChangesAsync();
                    bootCtx.ChangeTracker.Clear();
                }

                // ── Cerrar cajas huérfanas ──
                var cajasAbiertas = await bootCtx.Cajas
                    .Where(c => c.Estado == 1).ToListAsync();
                if (cajasAbiertas.Count > 0)
                {
                    foreach (var caja in cajasAbiertas)
                    {
                        caja.Estado = 2;
                        caja.FechaCierre = DateTime.Now;
                    }
                    await bootCtx.SaveChangesAsync();
                    bootCtx.ChangeTracker.Clear();
                }

                System.Diagnostics.Debug.WriteLine("[Bootstrapper] Startup OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Error: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Inner: {ex.InnerException.Message}");
            }
            // bootCtx se descarta aquí (using) — el contexto del DI container queda LIMPIO
            
            // ── Backup automático (si está habilitado) ───────────────────────
            try
            {
                var backupConfig = await bootCtx.BackupConfigs.FirstOrDefaultAsync();
                if (backupConfig != null &&
                    backupConfig.Frecuencia != GestionComercial.Dominio.Enumeraciones.FrecuenciaBackupEnum.Desactivado)
                {
                    bool debeEjecutar = backupConfig.Frecuencia switch
                    {
                        GestionComercial.Dominio.Enumeraciones.FrecuenciaBackupEnum.AlAbrirApp =>
                            backupConfig.UltimoBackup == null || backupConfig.UltimoBackup.Value.Date != DateTime.Today,

                        GestionComercial.Dominio.Enumeraciones.FrecuenciaBackupEnum.Diario =>
                            backupConfig.UltimoBackup == null || backupConfig.UltimoBackup.Value.Date < DateTime.Today,

                        GestionComercial.Dominio.Enumeraciones.FrecuenciaBackupEnum.Semanal =>
                            backupConfig.DiaSemana.HasValue &&
                            DateTime.Today.DayOfWeek == backupConfig.DiaSemana.Value &&
                            (backupConfig.UltimoBackup == null ||
                             backupConfig.UltimoBackup.Value.Date.AddDays(7) <= DateTime.Today),

                        _ => false
                    };

                    if (debeEjecutar)
                    {
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                var backupService = _container
                                    .GetInstance<GestionComercial.Dominio.Interfaces.Servicios.IBackupService>();
                                var resultado = await backupService.GenerarBackupAsync("automatico");
                                if (resultado != null && resultado.Success)
                                    System.Diagnostics.Debug.WriteLine(
                                        $"[Bootstrapper] Backup automático: {resultado.RutaBackup}");
                                else if (resultado != null)
                                    System.Diagnostics.Debug.WriteLine(
                                        $"[Bootstrapper] Backup automático falló: {resultado.ErrorMessage}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    $"[Bootstrapper] Backup automático exception: {ex.Message}");
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Backup check falló: {ex.Message}");
            }

            // ── Demo: verificar estado y mostrar showcase en primer inicio ──
            try
            {
                var demoService = _container.GetInstance<DemoService>();
                if (demoService.EsDemo)
                {
                    demoService.RegistrarInicioDemo();
                    demoService.GenerarCredencialesIniciales();

                    if (demoService.DemoExpirada)
                    {
                        MessageBox.Show(
                            $"La versión de demostración ha expirado ({demoService.DiasRestantes} días restantes).\n\n" +
                            "Contactanos para activar la licencia completa.\n" +
                            "Email: soporte@gestioncomercial.com",
                            "Demo Expirada", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Avisar días restantes
                    if (demoService.DiasRestantes <= 7 && demoService.DiasRestantes > 0)
                    {
                        MessageBox.Show(
                            $"Quedan {demoService.DiasRestantes} días de prueba.\n" +
                            "Contactanos para activar la licencia completa.",
                            "Aviso Demo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Bootstrapper] Demo check falló: {ex.Message}");
            }

            await DisplayRootViewForAsync<LoginViewModel>();
        }
    }
}
