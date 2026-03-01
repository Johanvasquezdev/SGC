using SGC.Domain.Entities.Notifications;
using System.Threading.Tasks;

namespace SGC.Domain.Repository.Notifications
{
    // Interfaz de repositorio específica para la entidad PrefNotificacion.
    // Permite gestionar las preferencias de notificación por usuario.
    public interface IPrefNotificacionRepository : IBaseRepository<PrefNotificacion>
    {
        // Recupera las preferencias de notificación activas de un usuario.
        Task<PrefNotificacion> GetByUsuarioIdAsync(int usuarioId);
    }
}
