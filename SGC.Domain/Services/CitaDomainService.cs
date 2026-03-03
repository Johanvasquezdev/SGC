using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Exceptions;
using SGC.Domain.Validators;

namespace SGC.Domain.Services
{
    // Valida las reglas de negocio para el agendamiento, reprogramacion y cancelacion de citas medicas
    public class CitaDomainService
    {
        private readonly CitaValidator _validator;

        public CitaDomainService()
        {
            _validator = new CitaValidator();
        }

        // Regla central: verifica si se puede agendar la cita
        public void ValidarAgendamiento(Cita cita, Medico medico)
        {
            _validator.Validar(cita);

            if (!medico.TieneDisponibilidad(cita.FechaHora))
                throw new HorarioNoDisponibleException(
                    medico.Id, cita.FechaHora);

            // Valida que el medico este activo tanto como usuario como profesional
            if (!medico.Activo)
                throw new CitaConflictoException(
                    $"El medico {medico.Nombre} no esta activo en el sistema.");

            if (!medico.MedicoActivo)
                throw new CitaConflictoException(
                    $"El medico {medico.Nombre} no esta habilitado para ejercer.");
        }

        // Regla: verifica si se puede reprogramar
        public void ValidarReprogramacion(Cita cita, Medico medico, DateTime nuevaFecha)
        {
            // Validar reglas basicas con la nueva fecha
            var citaTemporal = new Cita
            {
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                DisponibilidadId = cita.DisponibilidadId,
                FechaHora = nuevaFecha
            };
            _validator.Validar(citaTemporal);

            if (!medico.TieneDisponibilidad(nuevaFecha))
                throw new HorarioNoDisponibleException(
                    medico.Id, nuevaFecha);

            if (!medico.Activo)
                throw new CitaConflictoException(
                    $"El medico {medico.Nombre} no esta activo en el sistema.");

            if (!medico.MedicoActivo)
                throw new CitaConflictoException(
                    $"El medico {medico.Nombre} no esta habilitado para ejercer.");
        }
    }
}
