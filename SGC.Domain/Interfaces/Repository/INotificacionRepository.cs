using SGC.Domain.Entities.Notifications;

namespace SGC.Domain.Interfaces.Repository
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Notificacion>> GetNoLeidasAsync(int usuarioId);
    }
}
