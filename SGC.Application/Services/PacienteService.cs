using SGC.Application.Contracts;
using SGC.Application.DTOs.Medical;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de pacientes
    public class PacienteService : IPacienteService
    {
        // Repositorio de pacientes para acceso a datos
        private readonly IPacienteRepository _pacienteRepository;

        // Validador de reglas de negocio para pacientes
        private readonly PacienteValidator _validator = new PacienteValidator();

        public PacienteService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        // Registra un nuevo paciente en el sistema con rol Paciente
        public async Task<PacienteResponse> CrearAsync(CrearPacienteRequest request)
        {
            var paciente = new Paciente
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = request.Password, // En produccion se debe hashear la contrasena
                Rol = RolUsuario.Paciente,
                Cedula = request.Cedula,
                Telefono = request.Telefono,
                FechaNacimiento = request.FechaNacimiento,
                TipoSeguro = request.TipoSeguro,
                NumeroSeguro = request.NumeroSeguro
            };

            // Validar reglas de negocio antes de guardar
            _validator.Validar(paciente);

            await _pacienteRepository.AddAsync(paciente);
            return MapToResponse(paciente);
        }

        // Obtiene un paciente por su identificador
        public async Task<PacienteResponse> GetByIdAsync(int id)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            return MapToResponse(paciente);
        }

        // Obtiene todos los pacientes del sistema
        public async Task<IEnumerable<PacienteResponse>> GetAllAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(MapToResponse);
        }

        // Busca un paciente por su numero de cedula de identidad
        public async Task<PacienteResponse> GetByCedulaAsync(string cedula)
        {
            var paciente = await _pacienteRepository.GetByCedulaAsync(cedula);
            return MapToResponse(paciente);
        }

        // Actualiza la informacion de un paciente existente
        public async Task ActualizarAsync(ActualizarPacienteRequest request)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(request.Id);

            // Actualizar propiedades con los datos del request
            paciente.Nombre = request.Nombre;
            paciente.Email = request.Email;
            paciente.Cedula = request.Cedula;
            paciente.Telefono = request.Telefono;
            paciente.FechaNacimiento = request.FechaNacimiento;
            paciente.TipoSeguro = request.TipoSeguro;
            paciente.NumeroSeguro = request.NumeroSeguro;

            // Validar reglas de negocio antes de actualizar
            _validator.Validar(paciente);

            await _pacienteRepository.UpdateAsync(paciente);
        }

        // Desactiva un paciente del sistema usando la regla de dominio
        public async Task DesactivarAsync(int id)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            paciente.Desactivar();
            await _pacienteRepository.UpdateAsync(paciente);
        }

        // Activa un paciente en el sistema usando la regla de dominio
        public async Task ActivarAsync(int id)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            paciente.Activar();
            await _pacienteRepository.UpdateAsync(paciente);
        }

        // Convierte una entidad Paciente a su DTO de respuesta incluyendo la edad calculada
        private static PacienteResponse MapToResponse(Paciente paciente)
        {
            return new PacienteResponse
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Email = paciente.Email,
                Cedula = paciente.Cedula,
                Telefono = paciente.Telefono,
                FechaNacimiento = paciente.FechaNacimiento,
                Edad = paciente.CalcularEdad(),
                TipoSeguro = paciente.TipoSeguro,
                NumeroSeguro = paciente.NumeroSeguro,
                Activo = paciente.Activo
            };
        }
    }
}
