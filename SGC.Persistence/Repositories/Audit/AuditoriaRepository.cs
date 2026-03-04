using Microsoft.EntityFrameworkCore;
using SGC.Domain.Base;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Audit
{
    public class AuditoriaRepository : BaseRepository<AuditEntity>, IAuditoriaRepository
    {
        private readonly SGCDbContext _context;

        public AuditoriaRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        // Filtra eventos de auditoria por nombre de entidad (ej. "Cita", "Medico")
        public async Task<IEnumerable<AuditEntity>> GetByEntidadAsync(string entidad)
        {
            return await _context.EventosAuditoria
                .Where(a => a.Entidad == entidad)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        // Filtra eventos de auditoria por el usuario que realizo la accion
        public async Task<IEnumerable<AuditEntity>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.EventosAuditoria
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        // Filtra eventos de auditoria por fecha (compara solo la parte Date)
        public async Task<IEnumerable<AuditEntity>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.EventosAuditoria
                .Where(a => a.Fecha.Date == fecha.Date)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }
    }
}
