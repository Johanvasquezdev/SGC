using System.Security.Cryptography;
using System.Text;
using SGC.Application.DTOs.Medical;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Entities.Security;
using SGC.Domain.Repository.Medical;

namespace SGC.Application.Services.Medical
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _repository;

        public MedicoService(IMedicoRepository repository)
        {
            _repository = repository;
        }

        public async Task<MedicoDto> GetByIdAsync(int id)
        {
            var medico = await _repository.GetByIdAsync(id);
            return MapToDto(medico);
        }

        public async Task<IEnumerable<MedicoDto>> GetAllAsync()
        {
            var medicos = await _repository.GetAllAsync();
            return medicos.Select(MapToDto);
        }

        public async Task<MedicoDto> GetByExequaturAsync(string exequatur)
        {
            var medico = await _repository.GetByExequaturAsync(exequatur);
            return MapToDto(medico);
        }

        public async Task<IEnumerable<MedicoDto>> GetByEspecialidadAsync(int especialidadId)
        {
            var medicos = await _repository.GetByEspecialidadAsync(especialidadId);
            return medicos.Select(MapToDto);
        }

        public async Task<MedicoDto> CreateAsync(CreateMedicoRequest request)
        {
            var medico = new Medico
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Rol = RolUsuario.Medico,
                Exequatur = request.Exequatur,
                EspecialidadId = request.EspecialidadId,
                TelefonoConsultorio = request.TelefonoConsultorio,
                FechaCreacion = DateTime.UtcNow
            };
            await _repository.AddAsync(medico);
            return MapToDto(medico);
        }

        public async Task<MedicoDto> UpdateAsync(int id, UpdateMedicoRequest request)
        {
            var medico = await _repository.GetByIdAsync(id);
            medico.Nombre = request.Nombre;
            medico.Email = request.Email;
            medico.Exequatur = request.Exequatur;
            medico.EspecialidadId = request.EspecialidadId;
            medico.TelefonoConsultorio = request.TelefonoConsultorio;
            await _repository.UpdateAsync(medico);
            return MapToDto(medico);
        }

        public async Task DeleteAsync(int id)
        {
            var medico = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(medico);
        }

        private static MedicoDto MapToDto(Medico m) => new MedicoDto
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Email = m.Email,
            Rol = m.Rol.ToString(),
            Exequatur = m.Exequatur,
            EspecialidadId = m.EspecialidadId,
            TelefonoConsultorio = m.TelefonoConsultorio,
            FechaCreacion = m.FechaCreacion
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
