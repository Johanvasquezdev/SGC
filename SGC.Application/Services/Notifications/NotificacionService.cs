using SGC.Application.DTOs.Notifications;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Repository.Notifications;

namespace SGC.Application.Services.Notifications
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _repository;

        public NotificacionService(INotificacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<NotificacionDto>> GetByUsuarioIdAsync(int usuarioId)
        {
            var notificaciones = await _repository.GetByUsuarioIdAsync(usuarioId);
            return notificaciones.Select(MapToDto);
        }

        public async Task<IEnumerable<NotificacionDto>> GetNoLeidasAsync(int usuarioId)
        {
            var notificaciones = await _repository.GetNoLeidasAsync(usuarioId);
            return notificaciones.Select(MapToDto);
        }

        public async Task<NotificacionDto> CreateAsync(int usuarioId, string titulo, string mensaje)
        {
            var notificacion = new Notificacion
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensaje = mensaje,
                Leida = false,
                FechaEnvio = DateTime.UtcNow
            };
            await _repository.AddAsync(notificacion);
            return MapToDto(notificacion);
        }

        public async Task MarcarComoLeidaAsync(int id)
        {
            var notificacion = await _repository.GetByIdAsync(id);
            notificacion.Leida = true;
            await _repository.UpdateAsync(notificacion);
        }

        public async Task DeleteAsync(int id)
        {
            var notificacion = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(notificacion);
        }

        private static NotificacionDto MapToDto(Notificacion n) => new NotificacionDto
        {
            Id = n.Id,
            UsuarioId = n.UsuarioId,
            Titulo = n.Titulo,
            Mensaje = n.Mensaje,
            Leida = n.Leida,
            FechaEnvio = n.FechaEnvio
        };
    }
}
