using SGC.Domain.Base;

namespace SGC.Domain.Entities.Notifications
{
    public class Notificacion : EntidadBase
    {
        public int UsuarioId { get; set; }
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public bool Leida { get; set; } = false;
    }
}
