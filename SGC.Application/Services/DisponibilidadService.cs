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
    // Servicio de aplicacion para gestionar la disponibilidad de los medicos, con validaciones y logueo de operaciones
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

        // Crea una nueva disponibilidad para un medico, validando que no se solape con las existentes y logueando la operacion
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

        // Obtiene una disponibilidad por su ID, logueando la consulta
        public async Task<DisponibilidadResponse> GetByIdAsync(int id)
        {
            var disponibilidad = await _disponibilidadRepository
                .GetByIdAsync(id);
            return DisponibilidadMapper.ToResponse(disponibilidad);
        }

        // Obtiene todas las disponibilidades, logueando la consulta
        public async Task<IEnumerable<DisponibilidadResponse>> GetAllAsync()
        {
            var disponibilidades = await _disponibilidadRepository
                .GetAllAsync();
            return disponibilidades.Select(DisponibilidadMapper.ToResponse);
        }

        // Obtiene las disponibilidades de un medico por su ID, logueando la consulta
        public async Task<IEnumerable<DisponibilidadResponse>> GetByMedicoAsync(
            int medicoId)
        {
            var disponibilidades = await _disponibilidadRepository
                .GetByMedicoIdAsync(medicoId);
            return disponibilidades.Select(DisponibilidadMapper.ToResponse);
        }

        // Obtiene las disponibilidades de un medico por el dia de la semana, logueando la consulta
        public async Task<IEnumerable<DisponibilidadResponse>> GetByDiaAsync(
            int diaSemana)
        {
            var dia = (DiaSemana)diaSemana;
            var disponibilidades = await _disponibilidadRepository
                .GetByDiaAsync(dia);
            return disponibilidades.Select(DisponibilidadMapper.ToResponse);
        }

        // Actualiza una disponibilidad existente, validando que no se solape con las otras y logueando la operacion
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

        // Elimina una disponibilidad por su ID, logueando la operacion
        public async Task EliminarAsync(int id)
        {
            LogAdvertencia("EliminarDisponibilidad", $"Id: {id}");
            var disponibilidad = await _disponibilidadRepository
                .GetByIdAsync(id);
            await _disponibilidadRepository.DeleteAsync(disponibilidad);
        }
    }
}