namespace SGC.Application.DTOs.Notifications
{
    public class UpdatePrefNotificacionRequest
    {
        public bool RecibirEmail { get; set; }
        public bool RecibirSMS { get; set; }
        public bool RecibirPush { get; set; }
    }
}
