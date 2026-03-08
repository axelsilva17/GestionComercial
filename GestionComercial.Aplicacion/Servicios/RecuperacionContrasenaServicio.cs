using GestionComercial.Dominio.Interfaces;
using GestionComercial.Aplicacion.Excepciones;

namespace GestionComercial.Aplicacion.Servicios
{
    public static class PreguntasSecretas
    {
        public static readonly List<string> Lista = new()
        {
            "¿Cuál es el nombre de tu primera mascota?",
            "¿En qué ciudad naciste?",
            "¿Cuál es el nombre de tu madre?",
            "¿Cuál es tu comida favorita?",
            "¿Cuál fue el nombre de tu escuela primaria?",
            "¿Cuál es el nombre de tu mejor amigo de la infancia?",
            "¿Cuál es el modelo de tu primer auto?",
            "¿Cuál es el segundo nombre de tu padre?",
        };
    }

    public class RecuperacionContrasenaServicio
    {
        private readonly IUnitOfWork _uow;
        private const int MaxIntentos = 5;
        private const int MinutosBloqueo = 30;

        public RecuperacionContrasenaServicio(IUnitOfWork uow) => _uow = uow;

        /// <summary>Obtiene la pregunta secreta del usuario por email.</summary>
        public async Task<string?> ObtenerPreguntaAsync(string email)
        {
            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email);
            if (usuario == null)
                throw new NegocioException("No se encontró un usuario con ese email.");

            if (string.IsNullOrEmpty(usuario.PreguntaSecreta))
                return null;

            else {
                return usuario.PreguntaSecreta;

            } 
        }

        /// <summary>Valida la respuesta y si es correcta permite cambiar la contraseña.</summary>
        public async Task<bool> ValidarRespuestaAsync(string email, string respuesta)
        {
            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email)
                ?? throw new NegocioException("Usuario no encontrado.");

            // Verificar bloqueo
            if (usuario.EstaBloqueado)
            {
                var restante = (int)(usuario.BloqueadoHasta!.Value - DateTime.Now).TotalMinutes;
                throw new NegocioException($"Cuenta bloqueada. Intentá de nuevo en {restante} minutos.");
            }

            bool correcta = BCrypt.Net.BCrypt.Verify(
                respuesta.Trim().ToLower(),
                usuario.RespuestaHash);

            if (!correcta)
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= MaxIntentos)
                {
                    usuario.BloqueadoHasta   = DateTime.Now.AddMinutes(MinutosBloqueo);
                    usuario.IntentosFallidos = 0;
                    _uow.Usuarios.Actualizar(usuario);
                    await _uow.GuardarCambiosAsync();
                    throw new NegocioException($"Demasiados intentos fallidos. Cuenta bloqueada por {MinutosBloqueo} minutos.");
                }

                _uow.Usuarios.Actualizar(usuario);
                await _uow.GuardarCambiosAsync();

                int restantes = MaxIntentos - usuario.IntentosFallidos;
                throw new NegocioException($"Respuesta incorrecta. Te quedan {restantes} intentos.");
            }

            // Respuesta correcta — resetear intentos
            usuario.IntentosFallidos = 0;
            usuario.BloqueadoHasta   = null;
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();

            return true;
        }

        /// <summary>Cambia la contraseña después de validar la respuesta.</summary>
        public async Task CambiarContrasenaAsync(string email, string nuevaContrasena)
        {
            if (nuevaContrasena.Length < 8)
                throw new NegocioException("La contraseña debe tener al menos 8 caracteres.");

            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email)
                ?? throw new NegocioException("Usuario no encontrado.");

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena, workFactor: 12);
            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }

        /// <summary>Configura la pregunta secreta de un usuario.</summary>
        public async Task ConfigurarPreguntaAsync(string email, string pregunta, string respuesta)
        {
            var usuario = await _uow.Usuarios.ObtenerPorEmailAsync(email)
                ?? throw new NegocioException("Usuario no encontrado.");

            if (!PreguntasSecretas.Lista.Contains(pregunta))
                throw new NegocioException("Pregunta no válida.");

            usuario.PreguntaSecreta = pregunta;
            usuario.RespuestaHash   = BCrypt.Net.BCrypt.HashPassword(
                respuesta.Trim().ToLower(), workFactor: 12);

            _uow.Usuarios.Actualizar(usuario);
            await _uow.GuardarCambiosAsync();
        }
    }
}
