using SGC.Domain.Entities.Notifications;

namespace SGC.Domain.Repository.Notifications
{
    public interface IPrefNotificacionRepository : IBaseRepository<PrefNotificacion>
    {
        Task<PrefNotificacion> GetByUsuarioIdAsync(int usuarioId);
    }
}
