using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Appointments
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        private readonly SGCDbContext _context;

        public CitaRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        // Obtiene todas las citas de un paciente, incluyendo medico y disponibilidad
        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId)
        {
            return await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Disponibilidad)
                .Where(c => c.PacienteId == pacienteId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }

        // Obtiene todas las citas de un medico, incluyendo paciente y disponibilidad
        public async Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Disponibilidad)
                .Where(c => c.MedicoId == medicoId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }

        // Obtiene todas las citas de una fecha especifica, incluyendo paciente y medico
        public async Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.FechaHora.Date == fecha.Date)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();
        }

        // Verifica si un medico ya tiene una cita activa en esa fecha y hora
        public async Task<bool> ExisteConflictoAsync(int medicoId, DateTime fechaHora)
        {
            return await _context.Citas
                .AnyAsync(c =>
                    c.MedicoId == medicoId &&
                    c.FechaHora == fechaHora &&
                    c.Estado != EstadoCita.Cancelada &&
                    c.Estado != EstadoCita.Rechazada);
        }
    }
}
