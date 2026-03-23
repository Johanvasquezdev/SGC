using SGC.Domain.Entities.Appointments;

<<<<<<< HEAD
public interface ICitaRepository // Interfaz para el repositorio de citas, define los métodos que se implementarán para gestionar las citas en la base de datos
=======
namespace SGC.Domain.Interfaces.Repository
>>>>>>> c200be8 (Refactor ICitaRepository to interface and add methods)
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<bool> ExisteConflictoAsync(int medicoId, DateTime fechaHora);
    }
}
