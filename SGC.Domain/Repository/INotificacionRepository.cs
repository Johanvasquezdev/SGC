using System.Collections.Generic;
using System.Threading.Tasks;
using SGC.Domain.Entities.Notifications;

namespace SGC.Domain.Repository
{
    // Repositorio específico para la entidad Notificacion.
    // Extiende las operaciones CRUD base con consultas propias del módulo de notificaciones.
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        // Obtiene todas las notificaciones dirigidas a un usuario específico.
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId);
    }
}
