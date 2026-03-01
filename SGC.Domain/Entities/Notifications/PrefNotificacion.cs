using SGC.Domain.Base;

namespace SGC.Domain.Entities.Notifications
{
    // Almacena las preferencias de notificación de cada usuario.
    // Permite activar o desactivar los distintos canales de envío (Email, SMS, Push).
    public class PrefNotificacion : EntidadBase
    {
        public int UsuarioId { get; set; }
        public bool RecibeEmail { get; set; } = true;
        public bool RecibeSMS { get; set; } = false;
        public bool RecibePush { get; set; } = true;
    }
}
