using SGC.Application.DTOs.Appointments;

namespace SGC.Application.Contracts
{
    // Contrato para las operaciones de gestion de citas medicas
    public interface ICitaService
    {
        // Agenda una nueva cita medica
        Task<CitaResponse> AgendarAsync(CrearCitaRequest request);

        // Obtiene una cita por su identificador
        Task<CitaResponse> GetByIdAsync(int id);

        // Obtiene todas las citas
        Task<IEnumerable<CitaResponse>> GetAllAsync();

        // Obtiene todas las citas de un paciente
        Task<IEnumerable<CitaResponse>> GetByPacienteAsync(int pacienteId);

        // Obtiene todas las citas de un medico
        Task<IEnumerable<CitaResponse>> GetByMedicoAsync(int medicoId);

        // Obtiene todas las citas de una fecha especifica
        Task<IEnumerable<CitaResponse>> GetByFechaAsync(DateTime fecha);

        // Confirma una cita que esta en estado Solicitada
        Task ConfirmarAsync(int citaId);

        // Cancela una cita con un motivo
        Task CancelarAsync(int citaId, string motivo);

        // Rechaza una cita que esta en estado Solicitada
        Task RechazarAsync(int citaId, string motivo);

        // Reprograma una cita a una nueva fecha y hora
        Task ReprogramarAsync(int citaId, DateTime nuevaFecha);

        // Marca una cita como no asistida por el paciente
        Task MarcarNoAsistioAsync(int citaId);

        // Inicia la consulta de una cita confirmada
        Task IniciarConsultaAsync(int citaId);

        // Completa una cita que esta en progreso
        Task CompletarAsync(int citaId);
    }
}
