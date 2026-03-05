using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Repository.Appointments;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Appointments
{
    public class DisponibilidadRepository : BaseRepository<Disponibilidad>, IDisponibilidadRepository
    {
        public DisponibilidadRepository(SGCDbContext context) : base(context) { }

        public async Task<IEnumerable<Disponibilidad>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Disponibilidades
                .AsNoTracking()
                .Where(d => d.MedicoId == medicoId)
                .ToListAsync();
        }
    }
}
