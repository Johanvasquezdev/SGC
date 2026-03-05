namespace SGC.Application.DTOs.Security
{
    public class UpdateUsuarioRequest
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
