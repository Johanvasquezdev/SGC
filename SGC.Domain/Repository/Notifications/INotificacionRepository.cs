using SGC.Domain.Entities.Notifications;

namespace SGC.Domain.Repository.Notifications
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Notificacion>> GetNoLeidasAsync(int usuarioId);
    }
}
