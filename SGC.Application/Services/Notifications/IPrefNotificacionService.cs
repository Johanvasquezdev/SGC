using SGC.Application.DTOs.Notifications;

namespace SGC.Application.Services.Notifications
{
    public interface IPrefNotificacionService
    {
        Task<PrefNotificacionDto> GetByUsuarioIdAsync(int usuarioId);
        Task<PrefNotificacionDto> CreateAsync(int usuarioId);
        Task<PrefNotificacionDto> UpdateAsync(int usuarioId, UpdatePrefNotificacionRequest request);
    }
}
