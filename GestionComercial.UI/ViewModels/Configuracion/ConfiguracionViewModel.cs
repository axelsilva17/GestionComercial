using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class ConfiguracionViewModel : NavigableViewModel
    {
        public override string Titulo    => "Configuración";
        public override string Subtitulo => "Ajustes del sistema";

        public EmpresaViewModel     Empresa     { get; }
        public SucursalesViewModel  Sucursales  { get; }
        public UsuariosViewModel    Usuarios    { get; }
        public RolesViewModel       Roles       { get; }
        public MetodosPagoViewModel MetodosPago { get; }

        public ConfiguracionViewModel(
            EmpresaViewModel     empresa,
            SucursalesViewModel  sucursales,
            UsuariosViewModel    usuarios,
            RolesViewModel       roles,
            MetodosPagoViewModel metodosPago)
        {
            Empresa     = empresa;
            Sucursales  = sucursales;
            Usuarios    = usuarios;
            Roles       = roles;
            MetodosPago = metodosPago;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(
                Empresa.CargarAsync(),
                Sucursales.CargarAsync(),
                Usuarios.CargarAsync(),
                Roles.CargarAsync(),
                MetodosPago.CargarAsync()
            );
        }
    }
}
