using SGC.Domain.Entities.Appointments;

public class ICitaRepository // Interfaz para el repositorio de citas, define los métodos que se implementarán para gestionar las citas en la base de datos
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<bool> ExisteConflictoAsync(int medicoId, DateTime fechaHora);
    }
}
