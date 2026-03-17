using SGC.Application.DTOs.Appointments;
using SGC.Domain.Entities.Appointments;

namespace SGC.Application.Mappers
{
    public static class CitaMapper
    {
        public static CitaResponse ToResponse(Cita cita)
        {
            return new CitaResponse
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Estado = cita.Estado.ToString(),
                Motivo = cita.Motivo,
                Notas = cita.Notas,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                DisponibilidadId = cita.DisponibilidadId,
                FechaCreacion = cita.FechaCreacion,
                NombrePaciente = cita.Paciente?.Nombre,
                NombreMedico = cita.Medico?.Nombre
            };
        }

        public static Cita ToEntity(CrearCitaRequest request)
        {
            return new Cita
            {
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                DisponibilidadId = request.DisponibilidadId,
                FechaHora = request.FechaHora,
                Motivo = request.Motivo
            };
        }

        public static void UpdateEntity(Cita cita,
            ActualizarCitaRequest request)
        {
            cita.FechaHora = request.FechaHora ?? cita.FechaHora;
            cita.Motivo = request.Motivo ?? cita.Motivo;
            cita.Notas = request.Notas ?? cita.Notas;
        }
    }
}