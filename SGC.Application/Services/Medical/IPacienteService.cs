using SGC.Application.DTOs.Medical;

namespace SGC.Application.Services.Medical
{
    public interface IPacienteService
    {
        Task<PacienteDto> GetByIdAsync(int id);
        Task<IEnumerable<PacienteDto>> GetAllAsync();
        Task<PacienteDto> GetByCedulaAsync(string cedula);
        Task<PacienteDto> CreateAsync(CreatePacienteRequest request);
        Task<PacienteDto> UpdateAsync(int id, UpdatePacienteRequest request);
        Task DeleteAsync(int id);
    }
}
