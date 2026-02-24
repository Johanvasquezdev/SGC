using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.Repository;


    public interface ICitaRepository : IBaseRepository <Cita> // Hereda de la interfaz genérica IBaseRepository para aprovechar las operaciones CRUD estándar, y agrega métodos específicos para la entidad Cita.
    {
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<bool> ExisteConflictoAsync(int medicoId, DateTime fechaHora);
    }

