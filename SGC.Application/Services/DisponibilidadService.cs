using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class DisponibilidadService : BaseService, IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _disponibilidadRepository;
        private readonly DisponibilidadValidator _validator;

        public DisponibilidadService(
            IDisponibilidadRepository disponibilidadRepository,
            DisponibilidadValidator validator,
            ISGCLogger logger) : base(logger)
        {
            _disponibilidadRepository = disponibilidadRepository;
            _validator = validator;
        }

        public async Task<DisponibilidadResponse> CrearAsync(
            DisponibilidadRequest request)
        {
            LogOperacion("CrearDisponibilidad",
                $"MedicoId: {request.MedicoId}");
            var disponibilidad = DisponibilidadMapper.ToEntity(request);
            _validator.Validar(disponibilidad);
            await _disponibilidadRepository.AddAsync(disponibilidad);
            return DisponibilidadMapper.ToResponse(disponibilidad);
        }

        public async Task<DisponibilidadResponse> GetByIdAsync(int id)
        {
            var disponibilidad = await _disponibilidadRepository
                .GetByIdAsync(id);
            return DisponibilidadMapper.ToResponse(disponibilidad);
        }

        public async Task<IEnumerable<DisponibilidadResponse>> GetAllAsync()
        {
            var disponibilidades = await _disponibilidadRepository
                .GetAllAsync();
            return disponibilidades.Select(DisponibilidadMapper.ToResponse);
        }

        public async Task<IEnumerable<DisponibilidadResponse>> GetByMedicoAsync(
            int medicoId)
        {
            var disponibilidades = await _disponibilidadRepository
                .GetByMedicoIdAsync(medicoId);
            return disponibilidades.Select(DisponibilidadMapper.ToResponse);
        }

        public async Task<IEnumerable<DisponibilidadResponse>> GetByDiaAsync(
            int diaSemana)
        {
            var dia = (DiaSemana)diaSemana;
            var disponibilidades = await _disponibilidadRepository
                .GetByDiaAsync(dia);
            return disponibilidades.Select(DisponibilidadMapper.ToResponse);
        }

        public async Task ActualizarAsync(int id, DisponibilidadRequest request)
        {
            LogOperacion("ActualizarDisponibilidad", $"Id: {id}");
            var disponibilidad = await _disponibilidadRepository
                .GetByIdAsync(id);
            disponibilidad.MedicoId = request.MedicoId;
            disponibilidad.DiaSemana = (DiaSemana)request.DiaSemana;
            disponibilidad.HoraInicio = request.HoraInicio;
            disponibilidad.HoraFin = request.HoraFin;
            disponibilidad.DuracionCitaMin = request.DuracionCitaMin;
            disponibilidad.EsRecurrente = request.EsRecurrente;
            _validator.Validar(disponibilidad);
            await _disponibilidadRepository.UpdateAsync(disponibilidad);
        }

        public async Task EliminarAsync(int id)
        {
            LogAdvertencia("EliminarDisponibilidad", $"Id: {id}");
            var disponibilidad = await _disponibilidadRepository
                .GetByIdAsync(id);
            await _disponibilidadRepository.DeleteAsync(disponibilidad);
        }
    }
}