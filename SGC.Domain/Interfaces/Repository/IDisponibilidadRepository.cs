using SGC.Domain.Entities.Appointments;

namespace SGC.Domain.Interfaces.Repository
{
    public interface IDisponibilidadRepository : IBaseRepository<Disponibilidad> // Interfaz específica para la entidad Disponibilidad, que extiende las operaciones CRUD básicas definidas en IBaseRepository. Esta interfaz incluye métodos adicionales para recuperar disponibilidades según criterios específicos, como el ID del médico o el día de la semana, lo que facilita la gestión de los horarios disponibles para citas médicas.
    {
        Task<IEnumerable<Disponibilidad>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Disponibilidad>> GetByDiaAsync(string diaSemana);
    }
}
