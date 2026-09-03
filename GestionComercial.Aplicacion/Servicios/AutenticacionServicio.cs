using GestionComercial.Aplicacion.DTOs;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Interfaces;

using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Aplicacion.Servicios
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly DevCredencialesConfig _devCreds;

        public AutenticacionServicio(IUnitOfWork uow, IPasswordHasher passwordHasher, DevCredencialesConfig devCreds)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _devCreds = devCreds;
        }

        public async Task<UsuarioSesionDto?> LoginAsync(string email, string password)
        {
            if (email.Trim().Equals(_devCreds.Email, StringComparison.OrdinalIgnoreCase)
                && password.Trim() == _devCreds.Password)
            {
                try
                {
                    var primeraEmpresa = await _uow.Empresas.PrimerODefaultAsync(_ => true);
                    await _uow.Auditoria.RegistrarAuditoriaAsync(
                        "Sesion",
                        -1,
                        OperacionAuditoriaEnum.Insert,
                        idUsuario: null,
                        nombreUsuario: _devCreds.Email,
                        valoresAnteriores: null,
                        valoresNuevos: $"Login Desarrollador at {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                        idEmpresa: primeraEmpresa?.Id ?? 1);
                }
                catch { /* Don't fail login if audit logging fails */ }

                return new UsuarioSesionDto
                {
                    IdUsuario = -1,
                    Nombre = _devCreds.Nombre,
                        Apellido = _devCreds.Apellido,
                        Email = _devCreds.Email,
                        Rol = _devCreds.Rol,
                        IdSucursal = 0,
                        Sucursal = "N/A",
                        IdEmpresa = 1,
                        Empresa = "Desarrollador",
                        Permisos = new HashSet<string>(ObtenerPermisosDesarrollador())
                    };
            }

            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email);

            if (usuario == null)
                return null;

            if (usuario.EstaBloqueado)
            {
                var restante = (int)(usuario.BloqueadoHasta!.Value - DateTime.Now).TotalMinutes + 1;
                throw new NegocioException($"Demasiados intentos fallidos. Intentá de nuevo en {restante} minutos o usá la opción '¿Olvidaste tu contraseña?'.");
            }

            if (!usuario.PuedeAcceder)
                return null;

            bool passwordValido = _passwordHasher.VerifyPassword(password, usuario.PasswordHash);

            if (!passwordValido)
            {
                usuario.RegistrarAccesoFallido(maxIntentos: 5);
                _uow.Usuarios.Actualizar(usuario);
                await _uow.GuardarCambiosAsync();
                _uow.Usuarios.Desadjuntar(usuario);
                throw new NegocioException("Email o contraseña incorrectos.");
            }

            usuario.RegistrarAccesoExitoso();
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
            _uow.Usuarios.Desadjuntar(usuario);

            var permisos = await _uow.Usuarios.ObtenerPermisosAsync(usuario.Id);
            System.Diagnostics.Debug.WriteLine($"[Login] Permisos cargados para {email} (rol={usuario.Rol?.Nombre}): {string.Join(", ", permisos)}");

            return new UsuarioSesionDto
            {
                IdUsuario = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = usuario.Rol?.Nombre ?? string.Empty,
                IdSucursal = usuario.Id_sucursal,
                Sucursal = usuario.Sucursal?.Nombre ?? string.Empty,
                IdEmpresa = usuario.Sucursal?.Id_empresa ?? 0,
                Empresa = usuario.Sucursal?.Empresa?.Nombre ?? string.Empty,
                Permisos = new HashSet<string>(permisos),
            };
        }

        public string HashPassword(string password)
            => _passwordHasher.HashPassword(password);

        public bool IsCurrentUserAdmin()
        {
            var authService = new AuthService();
            return authService.IsCurrentUserAdmin();
        }

        private static List<string> ObtenerPermisosDesarrollador()
        {
            return new List<string>
            {
                "Configuracion.Ver",
                "Mantenimiento.Ver",
                "Mantenimiento.Ejecutar"
            };
        }
    }
}
