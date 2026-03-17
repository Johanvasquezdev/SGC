namespace SGC.Application.DTOs.Security
{
    // DTO para la solicitud de login, con email y password
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}