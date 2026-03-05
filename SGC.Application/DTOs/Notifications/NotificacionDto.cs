namespace SGC.Application.DTOs.Notifications
{
    public class NotificacionDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public bool Leida { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}
