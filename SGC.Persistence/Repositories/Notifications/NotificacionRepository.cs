using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Repository;
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Common;

namespace SGC.Persistence.Repositories.Notifications
{
    // Implementación EF Core del repositorio de notificaciones.
    // Cubre el módulo de notificaciones (feature/persistencia-notifications).
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(AppDbContext context) : base(context) { }

        // Obtiene todas las notificaciones de un usuario para mostrar su bandeja.
        public async Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet.Where(n => n.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
