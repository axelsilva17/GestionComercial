using Caliburn.Micro;
using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Clientes;
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
using GestionComercial.Dominio.Repositorio;
using GestionComercial.Infraestructura.Servicios;
using GestionComercial.Persistencia.Contexto;
using GestionComercial.Persistencia.Repositorio;
using GestionComercial.UI.Helpers;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.Views.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                // Si la BD fue creada con EnsureCreated, el baseline inserta el historial
                // de migraciones y MigrateAsync no intenta recrear tablas existentes.
                await context.Database.MigrateAsync();
                
                // Check if we need seed data (fallback si HasData no corrió)
                var tieneUsuarios = await context.Usuarios.AnyAsync();
                if (!tieneUsuarios)
                {
                    // Empresa
                    var empresa = new GestionComercial.Dominio.Entidades.Organizacion.Empresa
                    {
                        Id = 1,
                        Nombre = "Mi Empresa",
                        CUIT = "20-12345678-9",
                        Direccion = "Direccion 123",
                        Email = "admin@miempresa.com",
                        Telefono = "3794000000"
                    };
                    context.Empresas.Add(empresa);
                    
                    // Sucursal - necesaria porque Usuario tiene FK Id_sucursal
                    var sucursal = new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                    {
                        Id = 1,
                        Nombre = "Sucursal Principal",
                        Direccion = "Direccion 123",
                        Id_empresa = 1
                    };
                    context.Sucursales.Add(sucursal);

                    // Rol - Administrador con Id=2 (coherente con SemillaRoles)
                    context.Roles.Add(new GestionComercial.Dominio.Entidades.Seguridad.Rol
                    {
                        Id = 2,
                        Nombre = "Administrador",
                        Descripcion = "Acceso total",
                        Activo = true
                    });
                    
                    // Usuario
                    var hash = "$2a$12$1afFAY7Q1dY9UOpV5EboqOM9P1IO41RZz4F01zEqC918SeOU0qaRy";
                    var usuario = new GestionComercial.Dominio.Entidades.Seguridad.Usuario
                    {
                        Id = 1,
                        Nombre = "Admin",
                        Apellido = "Sistema",
                        Email = "admin@miempresa.com",
                        PasswordHash = hash,
                        Id_sucursal = 1,
                        Id_rol = 2,  // Administrador
                        Activo = true
                    };
                    context.Usuarios.Add(usuario);
                    
                    await context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("[Bootstrapper] Seed data created via EF Core");
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