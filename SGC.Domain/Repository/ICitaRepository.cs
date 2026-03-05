using SGC.Domain.Entities.Appointments;

namespace SGC.Domain.Repository
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Cita>> GetByEstadoAsync(string estado);
    }
}