using SGC.Application.Contracts;
using SGC.Application.DTOs.Notifications;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.Repository;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de notificaciones del sistema
    public class NotificacionService : INotificacionService
    {
        // Repositorio de notificaciones para acceso a datos
        private readonly INotificacionRepository _notificacionRepository;

        public NotificacionService(INotificacionRepository notificacionRepository)
        {
            _notificacionRepository = notificacionRepository;
        }

        // Obtiene una notificacion por su identificador
        public async Task<NotificacionResponse> GetByIdAsync(int id)
        {
            var notificacion = await _notificacionRepository.GetByIdAsync(id);
            return MapToResponse(notificacion);
        }

        // Obtiene todas las notificaciones de un usuario ordenadas por fecha
        public async Task<IEnumerable<NotificacionResponse>> GetByUsuarioAsync(int usuarioId)
        {
            var notificaciones = await _notificacionRepository.GetByUsuarioIdAsync(usuarioId);
            return notificaciones.Select(MapToResponse);
        }

        // Obtiene solo las notificaciones no leidas de un usuario
        public async Task<IEnumerable<NotificacionResponse>> GetNoLeidasAsync(int usuarioId)
        {
            var notificaciones = await _notificacionRepository.GetNoLeidasAsync(usuarioId);
            return notificaciones.Select(MapToResponse);
        }

        // Marca una notificacion como leida usando la regla de dominio
        public async Task MarcarLeidaAsync(int notificacionId)
        {
            var notificacion = await _notificacionRepository.GetByIdAsync(notificacionId);
            notificacion.MarcarLeida();
            await _notificacionRepository.UpdateAsync(notificacion);
        }

        // Convierte una entidad Notificacion a su DTO de respuesta
        private static NotificacionResponse MapToResponse(Notificacion n)
        {
            return new NotificacionResponse
            {
                Id = n.Id,
                UsuarioId = n.UsuarioId,
                CitaId = n.CitaId,
                Tipo = n.Tipo.ToString(),
                Mensaje = n.Mensaje,
                Leida = n.Leida,
                FechaEnvio = n.FechaEnvio
            };
        }
    }
}
