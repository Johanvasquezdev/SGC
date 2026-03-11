namespace SGC.Application.DTOs.Security
{
    // Datos de respuesta despues de un inicio de sesion exitoso
    public class LoginResponse
    {
        // Token JWT para autenticacion
        public string Token { get; set; } = string.Empty;

        // Token de refresco para renovar el JWT
        public string RefreshToken { get; set; } = string.Empty;

        // Fecha y hora de expiracion del token
        public DateTime Expiracion { get; set; }

        // Informacion basica del usuario autenticado
        public UsuarioResponse Usuario { get; set; } = null!;
    }
}
