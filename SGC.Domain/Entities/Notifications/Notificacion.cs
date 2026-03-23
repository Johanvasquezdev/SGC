using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Entities.Notifications
{
<<<<<<< HEAD
    public class Notificacion // para notificaciones relacionadas con citas, recordatorios, etc.
=======
    public sealed class Notificacion : EntidadBase // La clase Notificacion representa una notificación que se envía a un usuario, con información sobre el tipo de notificación, el mensaje, si ha sido leída y la fecha de envío. Hereda de EntidadBase para incluir propiedades comunes como Id y FechaCreacion.
>>>>>>> 6e8b30e (agrego de notificacion y auditable)
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