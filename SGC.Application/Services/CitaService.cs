using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Services;

namespace SGC.Application.Services
{
    // Servicio para gestionar el ciclo de vida de las citas medicas
    public class CitaService : BaseService, ICitaService
    {
        // Repositorio de citas para acceso a datos
        private readonly ICitaRepository _citaRepository;

        // Repositorio de medicos para validar disponibilidad
        private readonly IMedicoRepository _medicoRepository;

        // Servicio de dominio para validar reglas de negocio
        private readonly CitaDomainService _domainService;

        public CitaService(
            ICitaRepository citaRepository,
            IMedicoRepository medicoRepository,
            CitaDomainService domainService,
            ISGCLogger logger) : base(logger)
        {
            _citaRepository = citaRepository;
            _medicoRepository = medicoRepository;
            _domainService = domainService;
        }

        // Agenda una nueva cita validando disponibilidad y conflictos de horario
        public async Task<CitaResponse> AgendarAsync(CrearCitaRequest request)
        {
            LogOperacion("AgendarCita",
                $"Paciente: {request.PacienteId}, Medico: {request.MedicoId}");

            // Carga el medico con sus horarios para validar disponibilidad
            var medico = await _medicoRepository
                .GetByIdWithHorariosAsync(request.MedicoId);

            var cita = CitaMapper.ToEntity(request);

            // Valida reglas de dominio — disponibilidad y estado del medico
            _domainService.ValidarAgendamiento(cita, medico);

            // Verifica que no exista otra cita en el mismo horario
            var existeConflicto = await _citaRepository
                .ExisteConflictoAsync(request.MedicoId, request.FechaHora);
            if (existeConflicto)
                throw new Domain.Exceptions.CitaConflictoException(
                    "Ya existe una cita programada en ese horario.");

            await _citaRepository.AddAsync(cita);
            return CitaMapper.ToResponse(cita);
        }

        // Obtiene una cita por su identificador
        public async Task<CitaResponse> GetByIdAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            return CitaMapper.ToResponse(cita);
        }

        // Obtiene todas las citas del sistema
        public async Task<IEnumerable<CitaResponse>> GetAllAsync()
        {
            var citas = await _citaRepository.GetAllAsync();
            return citas.Select(CitaMapper.ToResponse);
        }

        // Obtiene todas las citas de un paciente especifico
        public async Task<IEnumerable<CitaResponse>> GetByPacienteAsync(int pacienteId)
        {
            var citas = await _citaRepository.GetByPacienteIdAsync(pacienteId);
            return citas.Select(CitaMapper.ToResponse);
        }

        // Obtiene todas las citas de un medico especifico
        public async Task<IEnumerable<CitaResponse>> GetByMedicoAsync(int medicoId)
        {
            var citas = await _citaRepository.GetByMedicoIdAsync(medicoId);
            return citas.Select(CitaMapper.ToResponse);
        }

        // Obtiene todas las citas de una fecha especifica
        public async Task<IEnumerable<CitaResponse>> GetByFechaAsync(DateTime fecha)
        {
            var citas = await _citaRepository.GetByFechaAsync(fecha);
            return citas.Select(CitaMapper.ToResponse);
        }

        // Confirma una cita cambiando su estado a Confirmada
        public async Task ConfirmarAsync(int citaId)
        {
            LogOperacion("ConfirmarCita", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Confirmar();
            await _citaRepository.UpdateAsync(cita);
        }

        // Cancela una cita registrando el motivo
        public async Task CancelarAsync(int citaId, string motivo)
        {
            LogAdvertencia("CancelarCita",
                $"CitaId: {citaId}, Motivo: {motivo}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Cancelar(motivo);
            await _citaRepository.UpdateAsync(cita);
        }

        // Rechaza una cita solicitada registrando el motivo
        public async Task RechazarAsync(int citaId, string motivo)
        {
            LogAdvertencia("RechazarCita",
                $"CitaId: {citaId}, Motivo: {motivo}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Rechazar(motivo);
            await _citaRepository.UpdateAsync(cita);
        }

        // Reprograma una cita a una nueva fecha validando disponibilidad
        public async Task ReprogramarAsync(int citaId, DateTime nuevaFecha)
        {
            LogOperacion("ReprogramarCita",
                $"CitaId: {citaId}, NuevaFecha: {nuevaFecha}");

            var cita = await _citaRepository.GetByIdAsync(citaId);

            // Carga el medico con horarios para validar nueva disponibilidad
            var medico = await _medicoRepository
                .GetByIdWithHorariosAsync(cita.MedicoId);

            _domainService.ValidarReprogramacion(cita, medico, nuevaFecha);

            // Verifica que no haya conflicto en la nueva fecha
            var existeConflicto = await _citaRepository
                .ExisteConflictoAsync(cita.MedicoId, nuevaFecha);
            if (existeConflicto)
                throw new Domain.Exceptions.CitaConflictoException(
                    "Ya existe una cita en ese horario.");

            cita.Reprogramar(nuevaFecha);
            await _citaRepository.UpdateAsync(cita);
        }

        // Marca una cita como no asistida por el paciente
        public async Task MarcarNoAsistioAsync(int citaId)
        {
            LogAdvertencia("MarcarNoAsistio", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.MarcarNoAsistio();
            await _citaRepository.UpdateAsync(cita);
        }

        // Inicia la consulta medica de una cita confirmada
        public async Task IniciarConsultaAsync(int citaId)
        {
            LogOperacion("IniciarConsulta", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.IniciarConsulta();
            await _citaRepository.UpdateAsync(cita);
        }

        // Completa una cita que esta en progreso
        public async Task CompletarAsync(int citaId)
        {
            LogOperacion("CompletarCita", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Completar();
            await _citaRepository.UpdateAsync(cita);
        }
    }
}