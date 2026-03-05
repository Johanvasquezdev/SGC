using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Repository.Notifications;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Notifications
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(SGCDbContext context) : base(context) { }

        public async Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetNoLeidasAsync(int usuarioId)
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }
    }
}
