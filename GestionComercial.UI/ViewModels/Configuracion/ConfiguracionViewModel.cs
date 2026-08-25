using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.Helpers;
using GestionComercial.UI.ViewModels.Base;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class ConfiguracionViewModel : NavigableViewModel
    {
        private readonly IUnitOfWork _uow;
        private readonly DemoFeatureService? _demoFeatures;

        public override string Titulo    => "Configuración";
        public override string Subtitulo => "Ajustes del sistema";

        public EmpresaViewModel     Empresa     { get; }
        public SucursalesViewModel  Sucursales  { get; }
        public UsuariosViewModel    Usuarios    { get; }
        public RolesViewModel       Roles       { get; }
        public MetodosPagoViewModel MetodosPago { get; }
        public BackupViewModel      Backup      { get; }

        // Demo: ocultar pestañas bloqueadas
        public bool MostrarBackup  => _demoFeatures == null || _demoFeatures.PuedeEjecutarAccion("configuracion", "backup");
        public bool MostrarRoles   => _demoFeatures == null || _demoFeatures.PuedeEjecutarAccion("configuracion", "roles");

        public ConfiguracionViewModel(
            IUnitOfWork          uow,
            EmpresaViewModel     empresa,
            SucursalesViewModel  sucursales,
            UsuariosViewModel    usuarios,
            RolesViewModel       roles,
            MetodosPagoViewModel metodosPago,
            BackupViewModel      backup,
            DemoFeatureService?  demoFeatures = null)
        {
            _uow         = uow;
            _demoFeatures = demoFeatures;
            Empresa      = empresa;
            Sucursales   = sucursales;
            Usuarios     = usuarios;
            Roles        = roles;
            MetodosPago  = metodosPago;
            Backup       = backup;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var tareas = new List<Task>
            {
                Empresa.CargarAsync(),
                Sucursales.CargarAsync(),
                Usuarios.CargarAsync(),
                MetodosPago.CargarAsync()
            };

            // Solo cargar Backup y Roles si están habilitados
            if (MostrarBackup) tareas.Add(Backup.CargarAsync());
            if (MostrarRoles)  tareas.Add(Roles.CargarAsync());

            await Task.WhenAll(tareas);
        }
    }
}
