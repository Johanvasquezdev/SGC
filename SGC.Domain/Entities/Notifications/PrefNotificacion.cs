using SGC.Domain.Base;

namespace SGC.Domain.Entities.Notifications
{
    public class PrefNotificacion : EntidadBase
    {
        public int UsuarioId { get; set; }
        public bool RecibirEmail { get; set; }
        public bool RecibirSMS { get; set; }
        public bool RecibirPush { get; set; }
    }
}
