using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Repository;
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Common;

namespace SGC.Persistence.Repositories.Notifications
{
    // Implementación EF Core del repositorio de preferencias de notificación.
    // Cubre el módulo de notificaciones (feature/persistencia-notifications).
    public class PrefNotificacionRepository : BaseRepository<PrefNotificacion>, IPrefNotificacionRepository
    {
        public PrefNotificacionRepository(AppDbContext context) : base(context) { }

        // Obtiene las preferencias de un usuario para configurar los canales de envío.
        public async Task<PrefNotificacion?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }
    }
}
