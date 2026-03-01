using System;
using SGC.Domain.Base;
using SGC.Domain.Enums;

namespace SGC.Domain.Entities.Notifications
{
    // Representa una notificación enviada a un usuario del sistema.
    // Puede ser de tipo Email, SMS o Push según el canal configurado.
    public class Notificacion : EntidadBase
    {
        public int UsuarioId { get; set; } // Destinatario de la notificación
        public CanalNotificacion Canal { get; set; } // Email | SMS | Push
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public bool Enviado { get; set; } = false;
        public DateTime? FechaEnvio { get; set; }
    }
}
