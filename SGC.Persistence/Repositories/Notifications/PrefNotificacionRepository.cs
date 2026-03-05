using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Repository.Notifications;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Notifications
{
    public class PrefNotificacionRepository : BaseRepository<PrefNotificacion>, IPrefNotificacionRepository
    {
        public PrefNotificacionRepository(SGCDbContext context) : base(context) { }

        public async Task<PrefNotificacion> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.PrefNotificaciones
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId)
                ?? throw new KeyNotFoundException($"No se encontraron preferencias de notificación para el usuario con id {usuarioId}.");
        }
    }
}
