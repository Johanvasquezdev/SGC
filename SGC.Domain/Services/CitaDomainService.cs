using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Exceptions;
using SGC.Domain.Validators;

namespace SGC.Domain.Services
{
    public class CitaDomainService // la clase valida las reglas de negocio relacionadas con el agendamiento, reprogramación y cancelación de citas médicas, asegurando que se cumplan las condiciones necesarias para cada acción, como la disponibilidad del médico y el estado de la cita.
    {
        private readonly CitaValidator _validator;

        public CitaDomainService()
        {
            _validator = new CitaValidator();
        }

        // Regla central: verifica si se puede agendar la cita
        public void ValidarAgendamiento(Cita cita, Medico medico)
        {
            // Primero valida las reglas básicas de la cita
            _validator.Validar(cita);

            // Luego valida que el médico tenga disponibilidad
            if (!medico.TieneDisponibilidad(cita.FechaHora))
                throw new HorarioNoDisponibleException(
                    medico.Id, cita.FechaHora);

            // Valida que el médico esté activo
            if (!medico.Activo)
                throw new CitaConflictoException(
                    $"El médico {medico.Nombre} no está activo en el sistema.");
        }

        // Regla: verifica si se puede reprogramar
        public void ValidarReprogramacion(Cita cita, Medico medico,
            DateTime nuevaFecha)
        {
            if (!medico.TieneDisponibilidad(nuevaFecha))
                throw new HorarioNoDisponibleException(
                    medico.Id, nuevaFecha);

            if (!medico.Activo)
                throw new CitaConflictoException(
                    $"El médico {medico.Nombre} no está activo en el sistema.");
        }
    }
}
