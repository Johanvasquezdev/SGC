using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de horarios de disponibilidad de medicos
    public class DisponibilidadService : IDisponibilidadService
    {
        // Repositorio de disponibilidad para acceso a datos
        private readonly IDisponibilidadRepository _disponibilidadRepository;

        // Validador de reglas de negocio para disponibilidad
        private readonly DisponibilidadValidator _validator = new DisponibilidadValidator();

        public DisponibilidadService(IDisponibilidadRepository disponibilidadRepository)
        {
            _disponibilidadRepository = disponibilidadRepository;
        }

        // Crea un nuevo horario de disponibilidad para un medico
        public async Task<DisponibilidadResponse> CrearAsync(DisponibilidadRequest request)
        {
            var disponibilidad = MapToEntity(request);

            // Validar reglas de negocio antes de guardar
            _validator.Validar(disponibilidad);

            await _disponibilidadRepository.AddAsync(disponibilidad);
            return MapToResponse(disponibilidad);
        }

        // Obtiene una disponibilidad por su identificador
        public async Task<DisponibilidadResponse> GetByIdAsync(int id)
        {
            var disponibilidad = await _disponibilidadRepository.GetByIdAsync(id);
            return MapToResponse(disponibilidad);
        }

        // Obtiene todas las disponibilidades del sistema
        public async Task<IEnumerable<DisponibilidadResponse>> GetAllAsync()
        {
            var disponibilidades = await _disponibilidadRepository.GetAllAsync();
            return disponibilidades.Select(MapToResponse);
        }

        // Obtiene todos los horarios disponibles de un medico especifico
        public async Task<IEnumerable<DisponibilidadResponse>> GetByMedicoAsync(int medicoId)
        {
            var disponibilidades = await _disponibilidadRepository.GetByMedicoIdAsync(medicoId);
            return disponibilidades.Select(MapToResponse);
        }

        // Obtiene todas las disponibilidades de un dia de la semana
        public async Task<IEnumerable<DisponibilidadResponse>> GetByDiaAsync(int diaSemana)
        {
            var dia = (DiaSemana)diaSemana;
            var disponibilidades = await _disponibilidadRepository.GetByDiaAsync(dia);
            return disponibilidades.Select(MapToResponse);
        }

        // Actualiza un horario de disponibilidad existente
        public async Task ActualizarAsync(int id, DisponibilidadRequest request)
        {
            var disponibilidad = await _disponibilidadRepository.GetByIdAsync(id);

            // Actualizar propiedades con los datos del request
            disponibilidad.MedicoId = request.MedicoId;
            disponibilidad.DiaSemana = (DiaSemana)request.DiaSemana;
            disponibilidad.HoraInicio = request.HoraInicio;
            disponibilidad.HoraFin = request.HoraFin;
            disponibilidad.DuracionCitaMin = request.DuracionCitaMin;
            disponibilidad.EsRecurrente = request.EsRecurrente;

            // Validar reglas de negocio antes de actualizar
            _validator.Validar(disponibilidad);

            await _disponibilidadRepository.UpdateAsync(disponibilidad);
        }

        // Elimina un horario de disponibilidad del sistema
        public async Task EliminarAsync(int id)
        {
            var disponibilidad = await _disponibilidadRepository.GetByIdAsync(id);
            await _disponibilidadRepository.DeleteAsync(disponibilidad);
        }

        // Convierte un request a la entidad Disponibilidad
        private static Disponibilidad MapToEntity(DisponibilidadRequest request)
        {
            return new Disponibilidad
            {
                MedicoId = request.MedicoId,
                DiaSemana = (DiaSemana)request.DiaSemana,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                DuracionCitaMin = request.DuracionCitaMin,
                EsRecurrente = request.EsRecurrente
            };
        }

        // Convierte una entidad Disponibilidad a su DTO de respuesta
        private static DisponibilidadResponse MapToResponse(Disponibilidad d)
        {
            return new DisponibilidadResponse
            {
                Id = d.Id,
                MedicoId = d.MedicoId,
                MedicoNombre = d.Medico?.Nombre ?? string.Empty,
                DiaSemana = d.DiaSemana.ToString(),
                HoraInicio = d.HoraInicio,
                HoraFin = d.HoraFin,
                DuracionCitaMin = d.DuracionCitaMin,
                EsRecurrente = d.EsRecurrente
            };
        }
    }
}
