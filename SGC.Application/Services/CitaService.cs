using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para gestionar citas medicas, implementa la logica de negocio y coordina con los repositorios y servicios de dominio.
    public class CitaService : BaseService, ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IMedicoRepository _medicoRepository;
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

        // Agendar una nueva cita, validando disponibilidad y reglas de negocio
        public async Task<CitaResponse> AgendarAsync(CrearCitaRequest request)
        {
            LogOperacion("AgendarCita",
                $"Paciente: {request.PacienteId}, Medico: {request.MedicoId}");

            var medico = await _medicoRepository.GetByIdAsync(request.MedicoId);
            var cita = CitaMapper.ToEntity(request);

            _domainService.ValidarAgendamiento(cita, medico);

            var existeConflicto = await _citaRepository
                .ExisteConflictoAsync(request.MedicoId, request.FechaHora);
            if (existeConflicto)
                throw new Domain.Exceptions.CitaConflictoException(
                    "Ya existe una cita programada en ese horario.");

            await _citaRepository.AddAsync(cita);
            return CitaMapper.ToResponse(cita);
        }

        // Obtener los detalles de una cita por su ID
        public async Task<CitaResponse> GetByIdAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            return CitaMapper.ToResponse(cita);
        }

        // Obtener todas las citas, con opciones de filtrado por paciente, medico o fecha
        public async Task<IEnumerable<CitaResponse>> GetAllAsync()
        {
            var citas = await _citaRepository.GetAllAsync();
            return citas.Select(CitaMapper.ToResponse);
        }

        // Obtener las citas de un paciente específico
        public async Task<IEnumerable<CitaResponse>> GetByPacienteAsync(int pacienteId)
        {
            var citas = await _citaRepository.GetByPacienteIdAsync(pacienteId);
            return citas.Select(CitaMapper.ToResponse);
        }

        // Obtener las citas de un medico específico
        public async Task<IEnumerable<CitaResponse>> GetByMedicoAsync(int medicoId)
        {
            var citas = await _citaRepository.GetByMedicoIdAsync(medicoId);
            return citas.Select(CitaMapper.ToResponse);
        }

        // Obtener las citas programadas para una fecha específica
        public async Task<IEnumerable<CitaResponse>> GetByFechaAsync(DateTime fecha)
        {
            var citas = await _citaRepository.GetByFechaAsync(fecha);
            return citas.Select(CitaMapper.ToResponse);
        }

        // Confirmar una cita, cambiando su estado a Confirmada
        public async Task ConfirmarAsync(int citaId)
        {
            LogOperacion("ConfirmarCita", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Confirmar();
            await _citaRepository.UpdateAsync(cita);
        }

        // Cancelar una cita, cambiando su estado a Cancelada y registrando el motivo
        public async Task CancelarAsync(int citaId, string motivo)
        {
            LogAdvertencia("CancelarCita",
                $"CitaId: {citaId}, Motivo: {motivo}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Cancelar(motivo);
            await _citaRepository.UpdateAsync(cita);
        }

        // Rechazar una cita, cambiando su estado a Rechazada y registrando el motivo
        public async Task RechazarAsync(int citaId, string motivo)
        {
            LogAdvertencia("RechazarCita",
                $"CitaId: {citaId}, Motivo: {motivo}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Rechazar(motivo);
            await _citaRepository.UpdateAsync(cita);
        }

        // Reprogramar una cita, cambiando su fecha y estado a Solicitada
        public async Task ReprogramarAsync(int citaId, DateTime nuevaFecha)
        {
            LogOperacion("ReprogramarCita",
                $"CitaId: {citaId}, NuevaFecha: {nuevaFecha}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            var medico = await _medicoRepository.GetByIdAsync(cita.MedicoId);

            _domainService.ValidarReprogramacion(cita, medico, nuevaFecha);

            var existeConflicto = await _citaRepository
                .ExisteConflictoAsync(cita.MedicoId, nuevaFecha);
            if (existeConflicto)
                throw new Domain.Exceptions.CitaConflictoException(
                    "Ya existe una cita en ese horario.");

            cita.Reprogramar(nuevaFecha);
            await _citaRepository.UpdateAsync(cita);
        }

        // Marcar una cita como No Asistió, cambiando su estado a NoAsistio
        public async Task MarcarNoAsistioAsync(int citaId)
        {
            LogAdvertencia("MarcarNoAsistio", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.MarcarNoAsistio();
            await _citaRepository.UpdateAsync(cita);
        }

        // Iniciar una consulta médica, cambiando el estado de la cita a EnConsulta
        public async Task IniciarConsultaAsync(int citaId)
        {
            LogOperacion("IniciarConsulta", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.IniciarConsulta();
            await _citaRepository.UpdateAsync(cita);
        }

        // Completar una consulta médica, cambiando el estado de la cita a Completada
        public async Task CompletarAsync(int citaId)
        {
            LogOperacion("CompletarCita", $"CitaId: {citaId}");
            var cita = await _citaRepository.GetByIdAsync(citaId);
            cita.Completar();
            await _citaRepository.UpdateAsync(cita);
        }
    }
}