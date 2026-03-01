using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Repository;
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Common;

namespace SGC.Persistence.Repositories.Medical
{
    // Implementación EF Core del repositorio de pacientes.
    // Cubre el módulo médico (feature/persistencia-medical).
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(AppDbContext context) : base(context) { }

        // Busca un paciente por cédula para identificación única en el sistema.
        public async Task<Paciente?> GetByCedulaAsync(string cedula)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Cedula == cedula);
        }
    }
}
