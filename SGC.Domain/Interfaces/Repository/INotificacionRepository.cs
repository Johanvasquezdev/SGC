using SGC.Domain.Entities.Notifications;

namespace SGC.Domain.Interfaces.Repository

{
    public interface INotificacionRepository : IBaseRepository<Notificacion> // Extiende la interfaz genérica IBaseRepository para incluir métodos específicos relacionados con las notificaciones.
    {
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Notificacion>> GetNoLeidasAsync(int usuarioId);
    }
}
