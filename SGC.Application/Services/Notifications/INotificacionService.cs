using SGC.Application.DTOs.Notifications;

namespace SGC.Application.Services.Notifications
{
    public interface INotificacionService
    {
        Task<IEnumerable<NotificacionDto>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<NotificacionDto>> GetNoLeidasAsync(int usuarioId);
        Task<NotificacionDto> CreateAsync(int usuarioId, string titulo, string mensaje);
        Task MarcarComoLeidaAsync(int id);
        Task DeleteAsync(int id);
    }
}
