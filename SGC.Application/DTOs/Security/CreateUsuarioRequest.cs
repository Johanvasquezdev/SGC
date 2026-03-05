namespace SGC.Application.DTOs.Security
{
    public class CreateUsuarioRequest
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
