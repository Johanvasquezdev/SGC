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
    public class MedicoService : BaseService, IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly MedicoValidator _validator;

        public MedicoService(
            IMedicoRepository medicoRepository,
            MedicoValidator validator,
            ISGCLogger logger) : base(logger)
        {
            _medicoRepository = medicoRepository;
            _validator = validator;
        }

        public async Task<MedicoResponse> CrearAsync(CrearMedicoRequest request)
        {
            LogOperacion("CrearMedico", $"Email: {request.Email}");

            var medico = new Medico
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = BCrypt.HashPassword(request.Password),
                Rol = RolUsuario.Medico,
                Exequatur = request.Exequatur,
                EspecialidadId = request.EspecialidadId,
                ProveedorSaludId = request.ProveedorSaludId,
                TelefonoConsultorio = request.TelefonoConsultorio,
                Foto = request.Foto
            };

            _validator.Validar(medico);
            await _medicoRepository.AddAsync(medico);
            return MedicoMapper.ToResponse(medico);
        }

        public async Task<MedicoResponse> GetByIdAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            return MedicoMapper.ToResponse(medico);
        }

        public async Task<IEnumerable<MedicoResponse>> GetAllAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Select(MedicoMapper.ToResponse);
        }

        public async Task<MedicoResponse> GetByExequaturAsync(string exequatur)
        {
            var medico = await _medicoRepository.GetByExequaturAsync(exequatur);
            return MedicoMapper.ToResponse(medico);
        }

        public async Task<IEnumerable<MedicoResponse>> GetByEspecialidadAsync(
            int especialidadId)
        {
            var medicos = await _medicoRepository
                .GetByEspecialidadAsync(especialidadId);
            return medicos.Select(MedicoMapper.ToResponse);
        }

        public async Task ActualizarAsync(ActualizarMedicoRequest request)
        {
            LogOperacion("ActualizarMedico", $"Id: {request.Id}");
            var medico = await _medicoRepository.GetByIdAsync(request.Id);
            MedicoMapper.UpdateEntity(medico, request);
            _validator.Validar(medico);
            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task DesactivarAsync(int id)
        {
            LogAdvertencia("DesactivarMedico", $"Id: {id}");
            var medico = await _medicoRepository.GetByIdAsync(id);
            medico.Desactivar();
            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task ActivarAsync(int id)
        {
            LogOperacion("ActivarMedico", $"Id: {id}");
            var medico = await _medicoRepository.GetByIdAsync(id);
            medico.Activar();
            await _medicoRepository.UpdateAsync(medico);
        }
    }
}