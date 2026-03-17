using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Notifications
{
    // Repositorio de notificaciones, con metodos personalizados para obtener notificaciones por usuario y estado de lectura
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(SGCDbContext context) : base(context) { }

        // Obtiene todas las notificaciones de un usuario, ordenadas por fecha de envio descendente
        public async Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await Context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }

        // Obtiene solo las notificaciones no leidas de un usuario, ordenadas por fecha de envio descendente
        public async Task<IEnumerable<Notificacion>> GetNoLeidasAsync(int usuarioId)
        {
            return await Context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }
    }
}