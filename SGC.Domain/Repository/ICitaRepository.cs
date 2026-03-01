using System.Collections.Generic;
using System.Threading.Tasks;
using SGC.Domain.Entities.Appointments;

namespace SGC.Domain.Repository
{
    // Repositorio específico para la entidad Cita.
    // Extiende las operaciones CRUD base con consultas propias del módulo de citas.
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        // Obtiene todas las citas asociadas a un paciente específico.
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);

        // Obtiene todas las citas agendadas para un médico específico.
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
    }
}