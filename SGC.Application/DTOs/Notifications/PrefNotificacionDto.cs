namespace SGC.Application.DTOs.Notifications
{
    public class PrefNotificacionDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public bool RecibirEmail { get; set; }
        public bool RecibirSMS { get; set; }
        public bool RecibirPush { get; set; }
    }
}
