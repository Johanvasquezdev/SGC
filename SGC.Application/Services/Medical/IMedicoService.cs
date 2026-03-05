using SGC.Application.DTOs.Medical;

namespace SGC.Application.Services.Medical
{
    public interface IMedicoService
    {
        Task<MedicoDto> GetByIdAsync(int id);
        Task<IEnumerable<MedicoDto>> GetAllAsync();
        Task<MedicoDto> GetByExequaturAsync(string exequatur);
        Task<IEnumerable<MedicoDto>> GetByEspecialidadAsync(int especialidadId);
        Task<MedicoDto> CreateAsync(CreateMedicoRequest request);
        Task<MedicoDto> UpdateAsync(int id, UpdateMedicoRequest request);
        Task DeleteAsync(int id);
    }
}
