using System.Security.Cryptography;
using System.Text;
using SGC.Application.DTOs.Medical;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Entities.Security;
using SGC.Domain.Repository.Medical;

namespace SGC.Application.Services.Medical
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<PacienteDto> GetByIdAsync(int id)
        {
            var paciente = await _repository.GetByIdAsync(id);
            return MapToDto(paciente);
        }

        public async Task<IEnumerable<PacienteDto>> GetAllAsync()
        {
            var pacientes = await _repository.GetAllAsync();
            return pacientes.Select(MapToDto);
        }

        public async Task<PacienteDto> GetByCedulaAsync(string cedula)
        {
            var paciente = await _repository.GetByCedulaAsync(cedula);
            return MapToDto(paciente);
        }

        public async Task<PacienteDto> CreateAsync(CreatePacienteRequest request)
        {
            var paciente = new Paciente
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Rol = RolUsuario.Paciente,
                Cedula = request.Cedula,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                TipoSeguro = request.TipoSeguro,
                FechaCreacion = DateTime.UtcNow
            };
            await _repository.AddAsync(paciente);
            return MapToDto(paciente);
        }

        public async Task<PacienteDto> UpdateAsync(int id, UpdatePacienteRequest request)
        {
            var paciente = await _repository.GetByIdAsync(id);
            paciente.Nombre = request.Nombre;
            paciente.Email = request.Email;
            paciente.Cedula = request.Cedula;
            paciente.Telefono = request.Telefono;
            paciente.Direccion = request.Direccion;
            paciente.TipoSeguro = request.TipoSeguro;
            await _repository.UpdateAsync(paciente);
            return MapToDto(paciente);
        }

        public async Task DeleteAsync(int id)
        {
            var paciente = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(paciente);
        }

        private static PacienteDto MapToDto(Paciente p) => new PacienteDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Email = p.Email,
            Rol = p.Rol.ToString(),
            Cedula = p.Cedula,
            Telefono = p.Telefono,
            Direccion = p.Direccion,
            TipoSeguro = p.TipoSeguro,
            FechaCreacion = p.FechaCreacion
        };

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password), salt,
                iterations: 100_000, HashAlgorithmName.SHA256, outputLength: 32);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
    }
}
