using SGC.Application.DTOs.Notifications;
using SGC.Domain.Entities.Notifications;

namespace SGC.Application.Mappers
{
    public static class NotificacionMapper
    {
        public static NotificacionResponse ToResponse(
            Notificacion notificacion)
        {
            return new NotificacionResponse
            {
                Id = notificacion.Id,
                UsuarioId = notificacion.UsuarioId,
                CitaId = notificacion.CitaId,
                Tipo = notificacion.Tipo.ToString(),
                Mensaje = notificacion.Mensaje,
                Leida = notificacion.Leida,
                FechaEnvio = notificacion.FechaEnvio
            };
        }
    }
}