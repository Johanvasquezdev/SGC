using SGC.Domain.Entities.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Domain.Repository.Notifications
{
    // Interfaz de repositorio específica para la entidad Notificacion.
    // Extiende las operaciones CRUD base con consultas propias del contexto de notificaciones.
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        // Recupera todas las notificaciones de un usuario, ordenadas por fecha descendente.
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId);

        // Recupera solo las notificaciones no leídas de un usuario.
        Task<IEnumerable<Notificacion>> GetNoLeidasByUsuarioIdAsync(int usuarioId);
    }
}
