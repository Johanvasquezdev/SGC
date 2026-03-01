using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Notifications
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        private readonly SGCDbContext _context;

        public NotificacionRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notificacion>> GetNoLeidasAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
