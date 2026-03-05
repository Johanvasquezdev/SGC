using SGC.Application.DTOs.Appointments;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Repository.Appointments;

namespace SGC.Application.Services.Appointments
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _repository;

        public DisponibilidadService(IDisponibilidadRepository repository)
        {
            _repository = repository;
        }

        public async Task<DisponibilidadDto> GetByIdAsync(int id)
        {
            var disp = await _repository.GetByIdAsync(id);
            return MapToDto(disp);
        }

        public async Task<IEnumerable<DisponibilidadDto>> GetAllAsync()
        {
            var disponibilidades = await _repository.GetAllAsync();
            return disponibilidades.Select(MapToDto);
        }

        public async Task<IEnumerable<DisponibilidadDto>> GetByMedicoIdAsync(int medicoId)
        {
            var disponibilidades = await _repository.GetByMedicoIdAsync(medicoId);
            return disponibilidades.Select(MapToDto);
        }

        public async Task<DisponibilidadDto> CreateAsync(CreateDisponibilidadRequest request)
        {
            var disp = new Disponibilidad
            {
                MedicoId = request.MedicoId,
                DiaSemana = request.DiaSemana,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                FechaCreacion = DateTime.UtcNow
            };
            await _repository.AddAsync(disp);
            return MapToDto(disp);
        }

        public async Task<DisponibilidadDto> UpdateAsync(int id, UpdateDisponibilidadRequest request)
        {
            var disp = await _repository.GetByIdAsync(id);
            disp.DiaSemana = request.DiaSemana;
            disp.HoraInicio = request.HoraInicio;
            disp.HoraFin = request.HoraFin;
            await _repository.UpdateAsync(disp);
            return MapToDto(disp);
        }

        public async Task DeleteAsync(int id)
        {
            var disp = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(disp);
        }

        private static DisponibilidadDto MapToDto(Disponibilidad d) => new DisponibilidadDto
        {
            Id = d.Id,
            MedicoId = d.MedicoId,
            DiaSemana = d.DiaSemana,
            HoraInicio = d.HoraInicio,
            HoraFin = d.HoraFin
        };
    }
}
