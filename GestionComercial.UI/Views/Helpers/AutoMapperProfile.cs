// NOTA: Este archivo requiere el paquete NuGet AutoMapper.
// Install-Package AutoMapper
//
// Si tu proyecto NO usa AutoMapper todavía, podés ignorar este archivo
// y hacer los mapeos manualmente en los servicios con métodos de extensión.
//
// Para registrarlo en el Bootstrapper:
//   var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
//   _container.RegisterInstance(typeof(IMapper), config.CreateMapper());

#if AUTOMAPPER_INSTALADO   // ← Reemplazá por "true" cuando instales AutoMapper

using AutoMapper;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Usuarios;

// Ajustá el namespace de tus entidades según tu proyecto
using GestionComercial.Dominio.Entidades;

namespace GestionComercial.UI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ── Usuarios ──────────────────────────────────────────────────────
            // Ajustar propiedades según cómo estén definidas tus entidades
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(d => d.SucursalNombre, o => o.MapFrom(s => s.Sucursal.Nombre))
                .ForMember(d => d.RolNombre,      o => o.MapFrom(s => s.Sucursal.Rol.Nombre.ToString()))
                .ForMember(d => d.Activo,         o => o.MapFrom(s => s.Activo == 1));

            CreateMap<Usuario, UsuarioSesionDto>()
                .ForMember(d => d.SucursalNombre, o => o.MapFrom(s => s.Sucursal.Nombre))
                .ForMember(d => d.EmpresaNombre,  o => o.MapFrom(s => s.Sucursal.Empresa.nombre))
                .ForMember(d => d.IdEmpresa,      o => o.MapFrom(s => s.Sucursal.Empresa.Id_empresa))
                .ForMember(d => d.RolNombre,      o => o.MapFrom(s => s.Sucursal.Rol.Nombre.ToString()))
                .ForMember(d => d.Token,          o => o.Ignore());

            // ── Empresa ───────────────────────────────────────────────────────
            CreateMap<Empresa, EmpresaDto>()
                .ForMember(d => d.Nombre,    o => o.MapFrom(s => s.nombre))
                .ForMember(d => d.CUIT,      o => o.MapFrom(s => s.CUIT.ToString()))
                .ForMember(d => d.Direccion, o => o.MapFrom(s => s.Direccion.ToString()))
                .ForMember(d => d.Activa,    o => o.MapFrom(s => s.Activa == 1));

            // ── Sucursal ──────────────────────────────────────────────────────
            CreateMap<Sucursal, SucursalDto>()
                .ForMember(d => d.Activa,    o => o.MapFrom(s => s.Activa == 1))
                .ForMember(d => d.Direccion, o => o.MapFrom(s => s.Direccion.ToString()))
                .ForMember(d => d.RolNombre, o => o.MapFrom(s => s.Rol.Nombre.ToString()));

            CreateMap<Rol, RolDto>()
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.Nombre.ToString()));

            // ── Productos ─────────────────────────────────────────────────────
            CreateMap<Producto, ProductoDto>()
                .ForMember(d => d.Activo, o => o.MapFrom(s => s.Activo == 1));

            // ── Clientes ──────────────────────────────────────────────────────
            CreateMap<Cliente, ClienteDto>()
                .ForMember(d => d.Activo, o => o.MapFrom(s => s.Activo == 1));

            // ── Método de Pago ────────────────────────────────────────────────
            CreateMap<MetodoPago, MetodoPagoDto>()
                .ForMember(d => d.EsEfectivo, o => o.MapFrom(s => s.EsEfectivo == 1));
        }
    }
}

#endif
