using BCrypt.Net;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Medical;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class PacienteService : BaseService, IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly PacienteValidator _validator;

        public PacienteService(
            IPacienteRepository pacienteRepository,
            PacienteValidator validator,
            ISGCLogger logger) : base(logger)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
        }

        public async Task<PacienteResponse> CrearAsync(CrearPacienteRequest request)
        {
            LogOperacion("CrearPaciente", $"Email: {request.Email}");

            var paciente = new Paciente
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = BCrypt.HashPassword(request.Password),
                Rol = RolUsuario.Paciente,
                Cedula = request.Cedula,
                Telefono = request.Telefono,
                FechaNacimiento = request.FechaNacimiento,
                TipoSeguro = request.TipoSeguro,
                NumeroSeguro = request.NumeroSeguro
            };

            _validator.Validar(paciente);
            await _pacienteRepository.AddAsync(paciente);
            return PacienteMapper.ToResponse(paciente);
        }

        public async Task<PacienteResponse> GetByIdAsync(int id)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            return PacienteMapper.ToResponse(paciente);
        }

        public async Task<IEnumerable<PacienteResponse>> GetAllAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(PacienteMapper.ToResponse);
        }

        public async Task<PacienteResponse> GetByCedulaAsync(string cedula)
        {
            var paciente = await _pacienteRepository.GetByCedulaAsync(cedula);
            return PacienteMapper.ToResponse(paciente);
        }

        public async Task ActualizarAsync(ActualizarPacienteRequest request)
        {
            LogOperacion("ActualizarPaciente", $"Id: {request.Id}");
            var paciente = await _pacienteRepository.GetByIdAsync(request.Id);
            PacienteMapper.UpdateEntity(paciente, request);
            _validator.Validar(paciente);
            await _pacienteRepository.UpdateAsync(paciente);
        }

        public async Task DesactivarAsync(int id)
        {
            LogAdvertencia("DesactivarPaciente", $"Id: {id}");
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            paciente.Desactivar();
            await _pacienteRepository.UpdateAsync(paciente);
        }

        public async Task ActivarAsync(int id)
        {
            LogOperacion("ActivarPaciente", $"Id: {id}");
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            paciente.Activar();
            await _pacienteRepository.UpdateAsync(paciente);
        }
    }
}