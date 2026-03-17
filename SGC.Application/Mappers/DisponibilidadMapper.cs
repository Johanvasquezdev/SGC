using SGC.Application.DTOs.Appointments;
using SGC.Domain.Entities.Appointments;

namespace SGC.Application.Mappers
{
    public static class DisponibilidadMapper
    {
        public static DisponibilidadResponse ToResponse(
            Disponibilidad disponibilidad)
        {
            return new DisponibilidadResponse
            {
                Id = disponibilidad.Id,
                MedicoId = disponibilidad.MedicoId,
                DiaSemana = disponibilidad.DiaSemana.ToString(),
                HoraInicio = disponibilidad.HoraInicio,
                HoraFin = disponibilidad.HoraFin,
                DuracionCitaMin = disponibilidad.DuracionCitaMin,
                EsRecurrente = disponibilidad.EsRecurrente
            };
        }

        public static Disponibilidad ToEntity(
            DisponibilidadRequest request)
        {
            return new Disponibilidad
            {
                MedicoId = request.MedicoId,
                DiaSemana = request.DiaSemana,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                DuracionCitaMin = request.DuracionCitaMin,
                EsRecurrente = request.EsRecurrente
            };
        }
    }
}