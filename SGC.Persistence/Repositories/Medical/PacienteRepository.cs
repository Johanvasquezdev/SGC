using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Repository.Medical;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(SGCDbContext context) : base(context) { }

        public async Task<Paciente> GetByCedulaAsync(string cedula)
        {
            return await _context.Pacientes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Cedula == cedula)
                ?? throw new KeyNotFoundException($"No se encontró un paciente con la cédula '{cedula}'.");
        }
    }
}
