using SGC.Application.DTOs.Appointments;

namespace SGC.Application.Services.Appointments
{
    public interface IDisponibilidadService
    {
        Task<DisponibilidadDto> GetByIdAsync(int id);
        Task<IEnumerable<DisponibilidadDto>> GetAllAsync();
        Task<IEnumerable<DisponibilidadDto>> GetByMedicoIdAsync(int medicoId);
        Task<DisponibilidadDto> CreateAsync(CreateDisponibilidadRequest request);
        Task<DisponibilidadDto> UpdateAsync(int id, UpdateDisponibilidadRequest request);
        Task DeleteAsync(int id);
    }
}
