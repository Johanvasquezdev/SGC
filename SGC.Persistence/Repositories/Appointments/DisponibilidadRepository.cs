using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Appointments
{
    public class DisponibilidadRepository : BaseRepository<Disponibilidad>, IDisponibilidadRepository
    {
        private readonly SGCDbContext _context;

        public DisponibilidadRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        // Obtiene todos los horarios disponibles de un medico especifico
        public async Task<IEnumerable<Disponibilidad>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Disponibilidades
                .Where(d => d.MedicoId == medicoId)
                .OrderBy(d => d.DiaSemana)
                .ThenBy(d => d.HoraInicio)
                .ToListAsync();
        }

        // Obtiene todos los horarios disponibles de un dia de la semana, incluyendo el medico
        public async Task<IEnumerable<Disponibilidad>> GetByDiaAsync(DiaSemana diaSemana)
        {
            return await _context.Disponibilidades
                .Include(d => d.Medico)
                .Where(d => d.DiaSemana == diaSemana)
                .OrderBy(d => d.HoraInicio)
                .ToListAsync();
        }
    }
}
