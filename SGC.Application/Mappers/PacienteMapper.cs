using SGC.Application.DTOs.Medical;
using SGC.Domain.Entities.Medical;

namespace SGC.Application.Mappers
{
    public static class PacienteMapper
    {
        public static PacienteResponse ToResponse(Paciente paciente)
        {
            return new PacienteResponse
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Email = paciente.Email,
                Cedula = paciente.Cedula,
                Telefono = paciente.Telefono,
                FechaNacimiento = paciente.FechaNacimiento,
                TipoSeguro = paciente.TipoSeguro,
                NumeroSeguro = paciente.NumeroSeguro,
                Activo = paciente.Activo
            };
        }

        public static Paciente ToEntity(CrearPacienteRequest request)
        {
            return new Paciente
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = request.Password,
                Cedula = request.Cedula,
                Telefono = request.Telefono,
                FechaNacimiento = request.FechaNacimiento,
                TipoSeguro = request.TipoSeguro,
                NumeroSeguro = request.NumeroSeguro
            };
        }

        public static void UpdateEntity(Paciente paciente,
            ActualizarPacienteRequest request)
        {
            paciente.Nombre = request.Nombre ?? paciente.Nombre;
            paciente.Telefono = request.Telefono ?? paciente.Telefono;
            paciente.TipoSeguro = request.TipoSeguro ?? paciente.TipoSeguro;
            paciente.NumeroSeguro = request.NumeroSeguro ?? paciente.NumeroSeguro;
        }
    }
}