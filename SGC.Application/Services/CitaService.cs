using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.Repository;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de citas medicas
    public class CitaService : ICitaService
    {
        // Repositorio de citas para acceso a datos
        private readonly ICitaRepository _citaRepository;

        // Repositorio de medicos para validaciones de disponibilidad
        private readonly IMedicoRepository _medicoRepository;

        // Servicio de dominio para validar reglas de negocio de citas
        private readonly CitaDomainService _domainService;

        public CitaService(
            ICitaRepository citaRepository,
            IMedicoRepository medicoRepository,
            CitaDomainService domainService)
        {
            _citaRepository = citaRepository;
            _medicoRepository = medicoRepository;
            _domainService = domainService;
        }

        // Agenda una nueva cita validando reglas de negocio y disponibilidad del medico
        public async Task<CitaResponse> AgendarAsync(CrearCitaRequest request)
        {
            var medico = await _medicoRepository.GetByIdAsync(request.MedicoId);

            var cita = new Cita
            {
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                DisponibilidadId = request.DisponibilidadId,
                FechaHora = request.FechaHora,
                Motivo = request.Motivo
            };

            // Validar reglas de dominio (disponibilidad, horario, conflictos)
            _domainService.ValidarAgendamiento(cita, medico);

            // Verificar que no exista conflicto de horario en la base de datos
            var existeConflicto = await _citaRepository.ExisteConflictoAsync(request.MedicoId, request.FechaHora);
            if (existeConflicto)
                throw new Domain.Exceptions.CitaConflictoException(
                    "Ya existe una cita programada para el medico en ese horario.");

            await _citaRepository.AddAsync(cita);
            return MapToResponse(cita);
        }

        // Obtiene una cita por su identificador
        public async Task<CitaResponse> GetByIdAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            return MapToResponse(cita);
        }

        // Obtiene todas las citas del sistema
        public async Task<IEnumerable<CitaResponse>> GetAllAsync()
        {
            var citas = await _citaRepository.GetAllAsync();
            return citas.Select(MapToResponse);
        }

        // Obtiene todas las citas de un paciente especifico
        public async Task<IEnumerable<CitaResponse>> GetByPacienteAsync(int pacienteId)
        {
            var citas = await _citaRepository.GetByPacienteIdAsync(pacienteId);
            return citas.Select(MapToResponse);
        }

        // Obtiene todas las citas de un medico especifico
        public async Task<IEnumerable<CitaResponse>> GetByMedicoAsync(int medicoId)
        {
            var citas = await _citaRepository.GetByMedicoIdAsync(medicoId);
            return citas.Select(MapToResponse);
        }

        // Obtiene todas las citas de una fecha especifica
        public async Task<IEnumerable<CitaResponse>> GetByFechaAsync(DateTime fecha)
        {
            var citas = await _citaRepository.GetByFechaAsync(fecha);
            return citas.Select(MapToResponse);
        }

        // Confirma una cita cambiando su estado de Solicitada a Confirmada
        public async Task ConfirmarAsync(int citaId)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Confirmar();
            await _citaRepository.UpdateAsync(cita);
        }

        // Cancela una cita registrando el motivo de cancelacion
        public async Task CancelarAsync(int citaId, string motivo)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Cancelar(motivo);
            await _citaRepository.UpdateAsync(cita);
        }

        // Rechaza una cita solicitada registrando el motivo de rechazo
        public async Task RechazarAsync(int citaId, string motivo)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Rechazar(motivo);
            await _citaRepository.UpdateAsync(cita);
        }

        // Reprograma una cita a una nueva fecha validando disponibilidad del medico
        public async Task ReprogramarAsync(int citaId, DateTime nuevaFecha)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            var medico = await _medicoRepository.GetByIdAsync(cita.MedicoId);

            // Validar reglas de dominio para la nueva fecha
            _domainService.ValidarReprogramacion(cita, medico, nuevaFecha);

            // Verificar conflictos en la nueva fecha
            var existeConflicto = await _citaRepository.ExisteConflictoAsync(cita.MedicoId, nuevaFecha);
            if (existeConflicto)
                throw new Domain.Exceptions.CitaConflictoException(
                    "Ya existe una cita programada para el medico en ese horario.");

            cita.Reprogramar(nuevaFecha);
            await _citaRepository.UpdateAsync(cita);
        }

        // Marca una cita confirmada como no asistida por el paciente
        public async Task MarcarNoAsistioAsync(int citaId)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.MarcarNoAsistio();
            await _citaRepository.UpdateAsync(cita);
        }

        // Inicia la consulta medica de una cita confirmada
        public async Task IniciarConsultaAsync(int citaId)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.IniciarConsulta();
            await _citaRepository.UpdateAsync(cita);
        }

        // Completa una cita que esta en progreso
        public async Task CompletarAsync(int citaId)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Completar();
            await _citaRepository.UpdateAsync(cita);
        }

        // Convierte una entidad Cita a su DTO de respuesta
        private static CitaResponse MapToResponse(Cita cita)
        {
            return new CitaResponse
            {
                Id = cita.Id,
                PacienteId = cita.PacienteId,
                PacienteNombre = cita.Paciente?.Nombre ?? string.Empty,
                MedicoId = cita.MedicoId,
                MedicoNombre = cita.Medico?.Nombre ?? string.Empty,
                FechaHora = cita.FechaHora,
                Estado = cita.Estado.ToString(),
                Motivo = cita.Motivo,
                Notas = cita.Notas,
                FechaCreacion = cita.FechaCreacion
            };
        }
    }
}
