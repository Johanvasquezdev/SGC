using System;
using SGC.Domain.Base;

namespace SGC.Domain.Entities.Notifications
{
    public class Notificacion : EntidadBase
    {
        public int UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leida { get; set; }
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    }
}
