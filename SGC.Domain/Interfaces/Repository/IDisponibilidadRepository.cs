using SGC.Domain.Entities.Appointments;

namespace SGC.Domain.Interfaces.Repository
{
    public interface IDisponibilidadRepository : IBaseRepository<Disponibilidad>
    {
        Task<IEnumerable<Disponibilidad>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Disponibilidad>> GetByDiaAsync(string diaSemana);
    }
}
