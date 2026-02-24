using SGC.Domain.Base;
using SGC.Domain.Enums;

namespace SGC.Domain.Entities.Notifications
{
    public sealed class Notificacion : EntidadBase // clase para notificacion del sistema
    {
        public int UsuarioId { get; set; }
        public int? CitaId { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public bool Leida { get; set; } = false;
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

        // marcar notificación como leída
        public void MarcarLeida()
        {
            if (Leida)
                throw new InvalidOperationException(
                    "La notificación ya fue marcada como leída.");
            Leida = true;
        }
    }
}
