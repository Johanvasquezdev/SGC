using SGC.Application.DTOs.Appointments;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Repository;

namespace SGC.Application.Services.Appointments
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _repository;

        public CitaService(ICitaRepository repository)
        {
            _repository = repository;
        }

        public async Task<CitaDto> GetByIdAsync(int id)
        {
            var cita = await _repository.GetByIdAsync(id);
            return MapToDto(cita);
        }

        public async Task<IEnumerable<CitaDto>> GetAllAsync()
        {
            var citas = await _repository.GetAllAsync();
            return citas.Select(MapToDto);
        }

        public async Task<IEnumerable<CitaDto>> GetByPacienteIdAsync(int pacienteId)
        {
            var citas = await _repository.GetByPacienteIdAsync(pacienteId);
            return citas.Select(MapToDto);
        }

        public async Task<IEnumerable<CitaDto>> GetByMedicoIdAsync(int medicoId)
        {
            var citas = await _repository.GetByMedicoIdAsync(medicoId);
            return citas.Select(MapToDto);
        }

        public async Task<IEnumerable<CitaDto>> GetByEstadoAsync(string estado)
        {
            var citas = await _repository.GetByEstadoAsync(estado);
            return citas.Select(MapToDto);
        }

        public async Task<CitaDto> CreateAsync(CreateCitaRequest request)
        {
            var cita = new Cita
            {
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                FechaHora = request.FechaHora,
                Motivo = request.Motivo,
                Estado = EstadoCita.Solicitada,
                FechaCreacion = DateTime.UtcNow
            };
            await _repository.AddAsync(cita);
            return MapToDto(cita);
        }

        public async Task<CitaDto> UpdateAsync(int id, UpdateCitaRequest request)
        {
            var cita = await _repository.GetByIdAsync(id);
            cita.Estado = request.Estado;
            cita.Motivo = request.Motivo;
            await _repository.UpdateAsync(cita);
            return MapToDto(cita);
        }

        public async Task DeleteAsync(int id)
        {
            var cita = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(cita);
        }

        private static CitaDto MapToDto(Cita c) => new CitaDto
        {
            Id = c.Id,
            PacienteId = c.PacienteId,
            MedicoId = c.MedicoId,
            FechaHora = c.FechaHora,
            Estado = c.Estado,
            Motivo = c.Motivo,
            FechaCreacion = c.FechaCreacion
        };
    }
}
