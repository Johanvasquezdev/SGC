using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.Repository;

<<<<<<< HEAD
<<<<<<< HEAD
public interface ICitaRepository // Interfaz para el repositorio de citas, define los mÃ©todos que se implementarÃ¡n para gestionar las citas en la base de datos
=======
namespace SGC.Domain.Interfaces.Repository
>>>>>>> c200be8 (Refactor ICitaRepository to interface and add methods)
{
    public interface ICitaRepository : IBaseRepository<Cita>
=======

    public interface ICitaRepository : IBaseRepository <Cita> // Hereda de la interfaz genérica IBaseRepository para aprovechar las operaciones CRUD estándar, y agrega métodos específicos para la entidad Cita.
>>>>>>> 6e8b30e (agrego de notificacion y auditable)
    {
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<bool> ExisteConflictoAsync(int medicoId, DateTime fechaHora);
    }

