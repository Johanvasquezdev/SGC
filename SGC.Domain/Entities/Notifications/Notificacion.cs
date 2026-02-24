using SGC.Domain.Base;
using SGC.Domain.Enums;

namespace SGC.Domain.Entities.Notifications
{
    public class Notificacion : EntidadBase
    {
        public int UsuarioId { get; set; }          // A quién va dirigida
        public string Mensaje { get; set; }          // Contenido del mensaje
        public TipoNotificacion Tipo { get; set; }  // Email, SMS, Push, etc.
        public bool Leida { get; set; } = false;     // Si fue leída o no
        public DateTime FechaEnvio { get; set; }     // Cuándo se envió
    }
}
