using SGC.Application.DTOs.Appointments;

namespace SGC.Application.Services.Appointments
{
    public interface ICitaService
    {
        Task<CitaDto> GetByIdAsync(int id);
        Task<IEnumerable<CitaDto>> GetAllAsync();
        Task<IEnumerable<CitaDto>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<CitaDto>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<CitaDto>> GetByEstadoAsync(string estado);
        Task<CitaDto> CreateAsync(CreateCitaRequest request);
        Task<CitaDto> UpdateAsync(int id, UpdateCitaRequest request);
        Task DeleteAsync(int id);
    }
}
