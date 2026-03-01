using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Repository;
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Common;

namespace SGC.Persistence.Repositories.Medical
{
    // Implementación EF Core del repositorio de médicos.
    // Cubre el módulo médico (feature/persistencia-medical).
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(AppDbContext context) : base(context) { }

        // Filtra médicos por especialidad para el flujo de búsqueda del paciente.
        public async Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId)
        {
            return await _dbSet.Where(m => m.EspecialidadId == especialidadId).ToListAsync();
        }
    }
}
