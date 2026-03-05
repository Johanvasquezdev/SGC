using SGC.Domain.Entities.Appointments;

namespace SGC.Domain.Repository.Appointments
{
    public interface IDisponibilidadRepository : IBaseRepository<Disponibilidad>
    {
        Task<IEnumerable<Disponibilidad>> GetByMedicoIdAsync(int medicoId);
    }
}
