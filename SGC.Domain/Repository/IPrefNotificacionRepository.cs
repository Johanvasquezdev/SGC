using System.Threading.Tasks;
using SGC.Domain.Entities.Notifications;

namespace SGC.Domain.Repository
{
    // Repositorio específico para la entidad PrefNotificacion.
    // Permite consultar y actualizar las preferencias de notificación de cada usuario.
    public interface IPrefNotificacionRepository : IBaseRepository<PrefNotificacion>
    {
        // Obtiene las preferencias de notificación de un usuario dado su identificador.
        Task<PrefNotificacion?> GetByUsuarioIdAsync(int usuarioId);
    }
}
