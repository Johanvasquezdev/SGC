using SGC.Domain.Base;

namespace SGC.Domain.Entities.Notifications
{
    // La clase PrefNotificacion representa las preferencias de notificación de un usuario y hereda de EntidadBase para incluir propiedades comunes como Id y FechaCreacion.
    public sealed class PrefNotificacion : EntidadBase 
    {
        public int UsuarioId { get; set; }
        public bool RecibirEmail { get; set; } = true;
        public bool RecibirSMS { get; set; } = true;
        public bool RecibirPush { get; set; } = true;
        public int HorasAntesRecordatorio { get; set; } = 24;

        // Método para verificar si el usuario tiene al menos un canal de notificación activo
        public bool TieneAlgunCanalActivo()
        {
            return RecibirEmail || RecibirSMS || RecibirPush;
        }
    }
}
