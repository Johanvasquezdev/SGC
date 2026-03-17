using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    // Repositorio específico para la entidad Medico, con métodos personalizados para consultas relacionadas con médicos.
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(SGCDbContext context) : base(context) { }

        // obtiene lista de medicos por el exequatur, incluyendo su especialidad. Si no se encuentra, lanza una excepcion
        public async Task<Medico> GetByExequaturAsync(string exequatur)
        {
            return await Context.Medicos
                       .Include(m => m.Especialidad)
                       .FirstOrDefaultAsync(m => m.Exequatur == exequatur)
                   ?? throw new KeyNotFoundException(
                       $"No se encontró médico con exequatur {exequatur}.");
        }

        // Obtiene una lista de médicos por su especialidad, ordenados por nombre
        public async Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId)
        {
            return await Context.Medicos
                .Where(m => m.EspecialidadId == especialidadId)
                .Include(m => m.Especialidad)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
    }
}