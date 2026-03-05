using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Repository;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Appointments
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        public CitaRepository(SGCDbContext context) : base(context) { }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId)
        {
            return await _context.Citas
                .AsNoTracking()
                .Where(c => c.PacienteId == pacienteId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Citas
                .AsNoTracking()
                .Where(c => c.MedicoId == medicoId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByEstadoAsync(string estado)
        {
            return await _context.Citas
                .AsNoTracking()
                .Where(c => c.Estado == estado)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }
    }
}
