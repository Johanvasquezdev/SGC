using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    // Repositorio para operaciones de persistencia de medicos
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(SGCDbContext context) : base(context) { }

        // Busca un medico por su numero de exequatur (licencia medica)
        public async Task<Medico> GetByExequaturAsync(string exequatur)
        {
            return await Context.Medicos
                .Include(m => m.Especialidad)
                .FirstOrDefaultAsync(m => m.Exequatur == exequatur)
                ?? throw new KeyNotFoundException($"No se encontro un medico con exequatur {exequatur}.");
        }

        // Obtiene todos los medicos de una especialidad con sus datos de especialidad incluidos
        public async Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId)
        {
            return await Context.Medicos
                .Where(m => m.EspecialidadId == especialidadId)
                .Include(m => m.Especialidad)
                .ToListAsync();
        }
    }
}
