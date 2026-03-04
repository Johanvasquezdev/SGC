using SGC.Domain.Base;

namespace SGC.Domain.Entities.Notifications
{
    public class PrefNotificacion : EntidadBase
    {
        public int UsuarioId { get; set; }
        public string Canal { get; set; }   // Email, SMS, Push, etc.
        public bool Activo { get; set; } = true;
    }
}
