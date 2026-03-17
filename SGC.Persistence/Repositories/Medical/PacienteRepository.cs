using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    // Implementación del repositorio para la entidad Paciente, con métodos específicos para consultas por cédula
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(SGCDbContext context) : base(context) { }

        // Obtiene un paciente por su cédula, lanzando una excepción si no se encuentra
        public async Task<Paciente> GetByCedulaAsync(string cedula)
        {
            return await Context.Pacientes
                       .FirstOrDefaultAsync(p => p.Cedula == cedula)
                   ?? throw new KeyNotFoundException(
                       $"No se encontró paciente con cédula {cedula}.");
        }
    }
}