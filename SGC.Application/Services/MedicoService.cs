using SGC.Application.Contracts;
using SGC.Application.DTOs.Medical;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de medicos
    public class MedicoService : IMedicoService
    {
        // Repositorio de medicos para acceso a datos
        private readonly IMedicoRepository _medicoRepository;

        // Validador de reglas de negocio para medicos
        private readonly MedicoValidator _validator = new MedicoValidator();

        public MedicoService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        // Registra un nuevo medico en el sistema con rol Medico
        public async Task<MedicoResponse> CrearAsync(CrearMedicoRequest request)
        {
            var medico = new Medico
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = request.Password, // En produccion se debe hashear la contrasena
                Rol = RolUsuario.Medico,
                Exequatur = request.Exequatur,
                EspecialidadId = request.EspecialidadId,
                ProveedorSaludId = request.ProveedorSaludId,
                TelefonoConsultorio = request.TelefonoConsultorio,
                Foto = request.Foto
            };

            // Validar reglas de negocio antes de guardar
            _validator.Validar(medico);

            await _medicoRepository.AddAsync(medico);
            return MapToResponse(medico);
        }

        // Obtiene un medico por su identificador
        public async Task<MedicoResponse> GetByIdAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            return MapToResponse(medico);
        }

        // Obtiene todos los medicos del sistema
        public async Task<IEnumerable<MedicoResponse>> GetAllAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Select(MapToResponse);
        }

        // Busca un medico por su numero de exequatur (licencia medica)
        public async Task<MedicoResponse> GetByExequaturAsync(string exequatur)
        {
            var medico = await _medicoRepository.GetByExequaturAsync(exequatur);
            return MapToResponse(medico);
        }

        // Obtiene todos los medicos que pertenecen a una especialidad
        public async Task<IEnumerable<MedicoResponse>> GetByEspecialidadAsync(int especialidadId)
        {
            var medicos = await _medicoRepository.GetByEspecialidadAsync(especialidadId);
            return medicos.Select(MapToResponse);
        }

        // Actualiza la informacion de un medico existente
        public async Task ActualizarAsync(ActualizarMedicoRequest request)
        {
            var medico = await _medicoRepository.GetByIdAsync(request.Id);

            // Actualizar propiedades con los datos del request
            medico.Nombre = request.Nombre;
            medico.Email = request.Email;
            medico.Exequatur = request.Exequatur;
            medico.EspecialidadId = request.EspecialidadId;
            medico.ProveedorSaludId = request.ProveedorSaludId;
            medico.TelefonoConsultorio = request.TelefonoConsultorio;
            medico.Foto = request.Foto;

            // Validar reglas de negocio antes de actualizar
            _validator.Validar(medico);

            await _medicoRepository.UpdateAsync(medico);
        }

        // Desactiva un medico del sistema usando la regla de dominio
        public async Task DesactivarAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            medico.Desactivar();
            await _medicoRepository.UpdateAsync(medico);
        }

        // Activa un medico en el sistema usando la regla de dominio
        public async Task ActivarAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            medico.Activar();
            await _medicoRepository.UpdateAsync(medico);
        }

        // Convierte una entidad Medico a su DTO de respuesta
        private static MedicoResponse MapToResponse(Medico medico)
        {
            return new MedicoResponse
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Email = medico.Email,
                Exequatur = medico.Exequatur,
                Especialidad = medico.Especialidad?.Nombre,
                ProveedorSalud = medico.ProveedorSalud?.Nombre,
                TelefonoConsultorio = medico.TelefonoConsultorio,
                Foto = medico.Foto,
                Activo = medico.Activo,
                MedicoActivo = medico.MedicoActivo
            };
        }
    }
}
